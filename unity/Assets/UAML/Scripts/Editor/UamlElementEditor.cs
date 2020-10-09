using System;
using System.Collections.Generic;
using System.IO;
using Uaml.Internal;
using Uaml.Internal.Reflection;
using Uaml.UX;
using UnityEditor;
using UnityEngine;

namespace Uaml.Editor
{
    [CustomEditor(typeof(FrameworkElement), editorForChildClasses: true)]
    public class UamlElementEditor : UnityEditor.Editor
    {
        private enum SaveFlag
        {
            Off,
            Dirty,
            Force
        }

        private const string prefKey = "Synchronize With File";
        private bool Synchronize
        {
            get => EditorPrefs.GetBool(prefKey, false);
            set => EditorPrefs.SetBool(prefKey, value);
        }

        private ElementType elemInfo;
        private Dictionary<string, bool> showGroup = new Dictionary<string, bool>();
        private Dictionary<string, Func<string, object, object>> map = new Dictionary<string, Func<string, object, object>>()
        { 
            { "String",     (n,v) => Draw<string>(n, v, EditorGUILayout.TextField) },
            { "Integer",    (n,v) => Draw<int>(n, v, EditorGUILayout.IntField) },
            { "Single",     (n,v) => Draw<float>(n, v, EditorGUILayout.FloatField) },
            { "Vector3",    (n,v) => Draw<Vector3>(n, v, EditorGUILayout.Vector3Field) },
            { "Vector3Int", (n,v) => Draw<Vector3Int>(n, v, EditorGUILayout.Vector3IntField) },
            { "Boolean",    (n,v) => Draw<bool>(n, v, EditorGUILayout.Toggle) },
            { "Color",      (n,v) => Draw<Color>(n, v, EditorGUILayout.ColorField) },
        };

        private delegate T Drawer<T>(string label, T value, params GUILayoutOption[] options);
        private static object Draw<T>(string name, object value, Drawer<T> drawer)
        {
            return drawer(name, (T)value);
        }

        public void OnEnable()
        {
            elemInfo = ElementRegistry.GetElementType(target.GetType());
        }

        public void OnDisable()
        {
        }

        private static void OpenFileInVisualStudioIDE(string path, int gotoLine)
        {
            System.Diagnostics.Process.Start("devenv", " /edit \"" + path + "\" /command \"edit.goto " + gotoLine.ToString() + " \" ");
        }

        public override void OnInspectorGUI()
        {
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                var saveFlag = SaveFlag.Off;

                var element = (FrameworkElement)target;
                var instance = element.EditorInstance;

                using (new EditorGUILayout.HorizontalScope("Box"))
                {
                    var oldSync = Synchronize;
                    var newSync = GUILayout.Toggle(oldSync, "Synchronize", GUILayout.Width(90));
                    if (oldSync != newSync)
                    {
                        Synchronize = newSync;
                    }

                    GUILayout.FlexibleSpace();

                    if (newSync)
                    {
                        saveFlag = SaveFlag.Dirty;
                    }
                    else if (GUILayout.Button("Save", EditorStyles.miniButton))
                    {
                        saveFlag = SaveFlag.Force;
                    }

                    if (GUILayout.Button("Open", EditorStyles.miniButton))
                    {
                        var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(element.RootElement);
                        OpenFileInVisualStudioIDE(path, 1);
                    }
                }

                // TODO: switch to serialized property system
                Undo.RecordObject(element, "Modify property");

                DrawEvents();
                DrawProperties();

                if (saveFlag != SaveFlag.Dirty)
                {
                    PrefabUtility.RecordPrefabInstancePropertyModifications(element);
                }

                element.ShowSelf = EditorGUILayout.Toggle("Show Debug", element.ShowSelf);
                if (element.ShowSelf)
                {
                    if (GUILayout.Button("Select Shadow Element"))
                    {
                        element.ForceShow();
                        EditorGUIUtility.PingObject(instance);
                    }
                }

                if (instance)
                {
                    var e = element.transform;
                    var i = instance.transform;

                    var from = element.IsRoot ? e : i;
                    var to = element.IsRoot ? i : e;

                    to.position   = from.position;
                    to.rotation   = from.rotation;
                    to.localScale = from.localScale;
                }

                if (check.changed)
                {
                   serializedObject.Update();
                }

                if (saveFlag == SaveFlag.Force || (saveFlag == SaveFlag.Dirty && check.changed))
                {
                    var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(element.RootElement);
                    if (!string.IsNullOrEmpty(path))
                    {
                        var uaml = Exporter.Export(element.RootElement);
                        File.WriteAllText(path, uaml);
                        AssetDatabase.ImportAsset(path);

                        element.RootElement.ApplyPropertiesToDependencyObject(true);
                    }
                }
            }
        }

        private void DrawEvents()
        {
            // TODO
        }

        private void DrawProperties()
        {
            for (var e = elemInfo; e != null; e = e.baseClass)
            {
                if (e == null || e.selfProps.Count == 0)
                {
                    continue;
                }

                var groupName = e.type.Name;
                var show = showGroup.TryGetValue(groupName, out var s) ? s : true;

                var newShow = EditorGUILayout.Foldout(show, groupName);
                if (newShow)
                {
                    var element = (FrameworkElement)target;

                    foreach (var pair in e.selfProps)
                    {
                        var name = pair.Key;
                        var property = pair.Value;

                        if (TryDrawField(name, property.PropertyType, property.GetValue(target), out var result))
                        {
                            property.SetValue(element, result);
                        }
                    }
                }

                if (show != newShow)
                {
                    showGroup[groupName] = newShow;
                }
            }
        }

        private bool TryDrawField(string name, Type type, object value, out object result)
        {
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (typeof(UnityEngine.Object).IsAssignableFrom(type))
                {
                    result = EditorGUILayout.ObjectField(name, (UnityEngine.Object)value, type, allowSceneObjects: true);
                    return check.changed;
                }

                if (map.TryGetValue(type.Name, out var action))
                {
                    result = action(name, value);
                    return check.changed;
                }
            }

            EditorGUILayout.LabelField(name, $"Don't know how to draw {type.Name}");
            result = null;
            return false;
        }
    }
}