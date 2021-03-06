﻿using System;
using System.Collections.Generic;
using Uaml.Internal.Reflection;
using Uaml.UX;
using UnityEditor;
using UnityEngine;

namespace Uaml.Editor
{
    [CustomEditor(typeof(FrameworkElement), editorForChildClasses: true)]
    public class UamlElementEditor : UnityEditor.Editor
    {
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

        public override void OnInspectorGUI()
        {
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                DrawEvents();
                DrawProperties();

                var element = (FrameworkElement)target;
                element.transform.position = element.instance.transform.position;
                element.transform.rotation = element.instance.transform.rotation;
                element.transform.localScale = element.instance.transform.localScale;

                if (check.changed)
                {
                   serializedObject.Update();
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
                    foreach (var pair in e.selfProps)
                    {
                        var name = pair.Key;
                        var property = pair.Value;

                        if (TryDrawField(name, property.PropertyType, property.GetValue(target), out var result))
                        {
                            property.SetValue(target, result);
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