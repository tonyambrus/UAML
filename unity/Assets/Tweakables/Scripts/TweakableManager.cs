using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tweakable
{
    public class TweakableManager : MonoBehaviour
    {
        public const float rowHeight = 30;

        private enum Mode
        {
            Static,
            Instance,
            Pick
        }

        [Serializable]
        public struct References
        {
            public RectTransform itemList;
            public RectTransform inspector;
            public Text inspectorTitle;
            public Text itemListTitle;
            public Button buttonStatic;
            public Button buttonInstance;
            public Button buttonPicker;
        }

        public References refs;
        public ControlTemplates templates;
        public TweakableObjectPicker picker;

        [Tooltip("Regex match")]
        public string[] assembliesToSearch = new string[] { "Assembly-*" };

        private CancellationTokenSource pickCancel = null;
        private object pickedObject;
        private Mode mode = Mode.Static;

        private readonly Dictionary<Type, ControlFactory> typeFactories = new Dictionary<Type, ControlFactory>
        {
            { typeof(bool), new ToggleFactory() },
            { typeof(int), new InputFactory<int>(InputField.ContentType.IntegerNumber) },
            { typeof(uint), new InputFactory<uint>(InputField.ContentType.IntegerNumber) },
            { typeof(float), new InputFactory<float>(InputField.ContentType.DecimalNumber) },
            { typeof(double), new InputFactory<double>(InputField.ContentType.DecimalNumber) },
            { typeof(string), new InputFactory<string>(InputField.ContentType.Standard) },
        };

        private readonly Dictionary<Type, ControlFactory> derivedTypefactories = new Dictionary<Type, ControlFactory>
        {
            { typeof(Enum), new EnumDropdownFactory() },
        };

        public void Awake()
        {
            Registry.Init(assembliesToSearch);

            refs.buttonStatic.onClick.AddListener(new UnityAction(() => SetMode(Mode.Static)));
            refs.buttonInstance.onClick.AddListener(new UnityAction(() => SetMode(Mode.Instance)));
            refs.buttonPicker.onClick.AddListener(new UnityAction(TogglePick));
        }

        private void SetButtonColor(Button button, Color color)
        {
            var cb = button.colors;
            cb.normalColor = color;
            cb.selectedColor = color;
            cb.highlightedColor = color;
            button.colors = cb;
        }

        private bool CancelPick()
        {
            if (pickCancel != null)
            {
                pickCancel.Cancel();
                pickCancel = null;
                SetButtonColor(refs.buttonPicker, Color.white);
                UpdateItemListTitle();
                return true;
            }

            return false;
        }

        private void UpdateItemListTitle()
        {
            var title = "";
            if (pickCancel != null)
            {
                title = $"Picking...";
            }
            else if (mode == Mode.Pick)
            {
                title = $"Picked: {pickedObject}";
            }
            else if (mode == Mode.Static)
            {
                title = $"Global";
            }
            else if (mode == Mode.Instance)
            {
                title = $"Objects";
            }
            else if (mode == Mode.Instance)
            {
                title = $"Objects";
            }

            refs.itemListTitle.text = title;
        }

        private async void TogglePick()
        {
            if (CancelPick())
            {
                return;
            }

            pickCancel = new CancellationTokenSource();
            SetButtonColor(refs.buttonPicker, Color.yellow);
            UpdateItemListTitle();

            try
            {
                var newObject = await picker.PickObjectAsync(pickCancel.Token);
                if (newObject != null)
                {
                    pickedObject = newObject;
                    mode = Mode.Pick;
                    Refresh();
                }
            }
            catch (TaskCanceledException)
            {
                Debug.Log("Cancelling Pick");
            }

            CancelPick();
        }

        private void SetMode(Mode mode)
        {
            this.mode = mode;
            Refresh();
        }

        public void Start()
        {
            Refresh();
        }

        private void Refresh()
        {
            Clear(refs.itemList);

            switch (mode)
            {
                case Mode.Static:
                    ShowStaticClasses();
                    break;

                case Mode.Instance:
                    ShowInstances();
                    break;

                case Mode.Pick:
                    ShowPicked();
                    break;
            }
        }

        private void Clear(RectTransform rt)
        {
            Enumerable
                .Range(0, rt.childCount)
                .Select(i => rt.GetChild(i))
                .ToList()
                .ForEach(t => Destroy(t.gameObject));
        }

        private void ShowStaticClasses()
        {
            UpdateItemListTitle();

            var items = Registry.staticData.Keys.Select(p => (p.Name, new Action(() => SelectStaticClass(p))));
            CreateItemList(items);
        }

        private void ShowInstances()
        {
            UpdateItemListTitle();

            var roots = new List<GameObject>();
            var items = new List<(string name, Action action)>();
            for (var i = 0; i < SceneManager.sceneCount; ++i)
            {
                var scene = SceneManager.GetSceneAt(i);

                roots.Clear();
                scene.GetRootGameObjects(roots);

                foreach (var root in roots)
                {
                    foreach (var type in Registry.monobehaviors)
                    {
                        foreach (var c in root.GetComponentsInChildren(type))
                        {
                            var name = $"{c.gameObject.name} ({c.GetType().Name})";
                            var action = new Action(() => SelectObject(c));
                            items.Add((name, action));
                        }
                    }
                }
            }

            CreateItemList(items);
        }

        private void ShowPicked()
        {
            if (pickedObject == null)
            {
                return;
            }
        
            if (pickedObject is GameObject go)
            {
                var items = go
                    .GetComponentsInChildren<MonoBehaviour>()
                    .Where(mb => Registry.monobehaviors.Contains(mb.GetType()))
                    .Select(b => (name: $"{b.gameObject.name} ({b.GetType().Name})", action: new Action(() => SelectObject(b))))
                    .ToList();

                CreateItemList(items);
            }
            else
            {
                var item = ($"{pickedObject}", new Action(() => SelectObject(pickedObject)));
                CreateItemList(new[] { item });

                SelectObject(pickedObject);
            }
        }

        private void CreateItemList(IEnumerable<(string name, Action action)> list, bool selectFirstItem = true)
        {
            var offset = new Vector2();
            foreach (var (name, action) in list)
            {
                var button = Instantiate(templates.button);
                button.GetComponentInChildren<Text>().text = name;
                button.transform.SetParent(refs.itemList, false);
                button.transform.localPosition = offset;
                button.onClick.AddListener(new UnityAction(action));

                offset.y -= (button.transform as RectTransform).rect.height;
            }

            if (selectFirstItem)
            {
                list.FirstOrDefault().action?.Invoke();
            }
        }
        
        private void CreateWithFactory(ControlFactory factory, object instance, FieldData field, RectTransform parent, Vector2 offset)
        {
            try
            {
                var item = factory.Create(templates, instance, field);
                item.SetParent(parent, false);
                item.localPosition = offset;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private RectTransform CreateFromField(string name, object instance, FieldData field, RectTransform parent = null)
        {
            var offset = new Vector2(0,0);

            var rt = new GameObject("row").AddComponent<RectTransform>();

            if (parent)
            {
                rt.SetParent(parent, false);
            }

            rt.MatchParent();
            rt.SetAnchors(0, 1, 1, 1);
            rt.sizeDelta = new Vector2(0, rowHeight);

            var label = Instantiate(templates.label);
            var labelRT = label.transform as RectTransform;
            labelRT.SetParent(rt, false);
            labelRT.localPosition = new Vector2(10,0);
            label.text = name;

            offset.x += 100;

            if (typeFactories.TryGetValue(field.FieldType, out var factory))
            {
                CreateWithFactory(factory, instance, field, rt, offset);
                return rt;
            }
            else
            {
                foreach (var pair in derivedTypefactories)
                {
                    if (pair.Key.IsAssignableFrom(field.FieldType))
                    {
                        CreateWithFactory(pair.Value, instance, field, rt, offset);
                        return rt;
                    }
                }
            }

            // show error
            return rt;
        }

        private void SelectStaticClass(Type type) => ShowInspector(type.Name, type, null);
        private void SelectObject(object instance) => ShowInspector(instance.ToString(), instance.GetType(), instance);

        private void ShowInspector(string title, Type type, object instance)
        {
            Clear(refs.inspector);

            refs.inspectorTitle.text = title;

            if (Registry.TryGetData(type, instance != null, out var typeData))
            {
                var offset = new Vector2(0, -5);
                foreach (var p in typeData.fields)
                {
                    var rt = CreateFromField(p.Key, instance, p.Value, refs.inspector);
                    rt.localPosition = offset;
                    offset.y -= rt.rect.height;
                }
            }
        }
    }
}