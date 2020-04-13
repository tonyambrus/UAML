#if false
using System.IO;
using Uaml.Core;
using Uaml.Internal;
using Uaml.UX;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Uaml.Editor
{
    public class UamlEditorWindow : EditorWindow
    {
        private Schema schema;
        private PrefabStage stage;
        private UamlDocument root;
        private UamlDocument source;

        [MenuItem("Window/UAML/Editor")]
        public static void ShowDefaultWindow()
        {
            var wnd = GetWindow<UamlEditorWindow>();
            wnd.titleContent = new GUIContent("UWAML Editor");
        }

        public void OnEnable()
        {
            OnSelectionChange();
        }

        public void OnDisable()
        {
            StageUtility.GoToMainStage();
        }

        private bool IsUaml(GameObject go) => go && go.TryGetComponent<UamlDocument>(out var _);

        private bool IsEditingUAML => PrefabStageUtility.GetCurrentPrefabStage()?.prefabContentsRoot?.GetComponent<UamlDocument>() != null;

        public void OnGUI()
        {
            using (new EditorGUI.DisabledGroupScope(IsEditingUAML))
            {
                schema = (Schema)EditorGUILayout.ObjectField(schema, typeof(Schema), allowSceneObjects: true);
                if (!schema)
                {
                    EditorGUILayout.HelpBox("Need a schema", MessageType.Warning);
                }
            }

            if (GUILayout.Button("Serialize", EditorStyles.miniButton))
            {
                var asset = Selection.activeGameObject;
                var assetPath = AssetDatabase.GetAssetPath(asset);
                if (assetPath != null)
                {
                    var text = Exporter.Export(asset.GetComponent<UamlDocument>());
                    if (text != null)
                    {
                        File.WriteAllText(assetPath, text);
                        EditorUtility.SetDirty(asset);
                        Debug.Log(text);
                    }
                }
            }


            if (stage != null && IsEditingUAML)
            {
                if (GUILayout.Button("Stop Editing", EditorStyles.miniButton))
                {
                    StageUtility.GoToMainStage();
                    stage = null;
                }

                if (GUILayout.Button("Deserialize", EditorStyles.miniButton))
                {
                    //Deserialize();
                }

                if (GUILayout.Button("Serialize", EditorStyles.miniButton))
                {
                    var text = Exporter.Export(root);
                    File.WriteAllText(AssetDatabase.GetAssetPath(source), text);
                    EditorUtility.SetDirty(source);
                    Debug.Log(text);
                }
            }
            else
            {
                var sourceCandidate = (Selection.activeObject as GameObject)?.GetComponent<UamlDocument>(); ;
                if (!sourceCandidate)
                {
                    EditorGUILayout.HelpBox("Need a xml source", MessageType.Warning);
                }

                using (new EditorGUI.DisabledGroupScope(!sourceCandidate))
                {
                    if (GUILayout.Button("Open Editor", EditorStyles.miniButton))
                    {
                        source = sourceCandidate;

                        var editorPath = AssetDatabase.GetAssetPath(source);
                        stage = (PrefabStage)Reflection.Static<PrefabStageUtility>("OpenPrefab", editorPath);

                        root = stage.prefabContentsRoot.GetComponent<UamlDocument>();

                        stage.ClearDirtiness();

                        //Deserialize();
                    }
                }
            }
        }

        public void OnSelectionChange()
        {
            //var source = Selection.activeObject as TextAsset;
            //if (source)
            //{
            //    // Create serialization object
            //    SerializedObject so = new SerializedObject(source);

            //    // Bind it to the root of the hierarchy. It will find the right object to bind to...
            //    //rootVisualElement.Bind(so);
            //}
            //else
            //{
            //    // Unbind the object from the actual visual element
            //    //rootVisualElement.Unbind();
            //}
        }
    }
}
#endif