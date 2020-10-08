using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Tweakable
{
    public static class Registry
    {
        internal static HashSet<Type> monobehaviors = new HashSet<Type>();
        internal static Dictionary<Type, TypeData> instanceData = new Dictionary<Type, TypeData>();
        internal static Dictionary<Type, TypeData> staticData = new Dictionary<Type, TypeData>();

        public static void Init(IEnumerable<string> assembliesToSearch)
        {
            var pattern = new Regex(string.Join("|", assembliesToSearch));
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => pattern.IsMatch(a.FullName))
                .ToList();

            var types = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetCustomAttribute<TweakableAttribute>() != null)
                .ToList();

            foreach (var t in types)
            {
                InitType(t);
            }
        }

        public static bool TryGetData(Type type, bool instance, out TypeData data)
        {
            return instance ? instanceData.TryGetValue(type, out data) : staticData.TryGetValue(type, out data);
        }

        private static void InitType(Type type)
        {
            if (!instanceData.TryGetValue(type, out var info))
            {
                var fields = type
                    .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(f => !f.IsStatic && f.GetCustomAttribute<TweakAttribute>() != null)
                    .ToDictionary(f => f.Name, f => new FieldData(f));

                var methods = type
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(f => f.GetCustomAttribute<TweakButtonAttribute>() != null)
                    .ToDictionary(f => f.Name);

                info = new TypeData
                {
                    type = type,
                    fields = fields,
                    methods = methods,
                };

                instanceData[type] = info;

                if (typeof(MonoBehaviour).IsAssignableFrom(type))
                {
                    monobehaviors.Add(type);
                }
            }

            if (!staticData.ContainsKey(type))
            {
                var staticFields = type
                    .GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(f => f.GetCustomAttribute<TweakAttribute>() != null)
                    .ToDictionary(f => f.Name, f => new FieldData(f));

                var staticMethods = type
                    .GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(f => f.GetCustomAttribute<TweakButtonAttribute>() != null)
                    .ToDictionary(f => f.Name);

                if (staticFields.Count > 0 || staticMethods.Count > 0)
                {
                    staticData[type] = new TypeData
                    {
                        type = type,
                        fields = staticFields,
                        methods = staticMethods,
                    };
                }
            }
        }
    }
}