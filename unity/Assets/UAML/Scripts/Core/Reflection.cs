using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Uaml.Core
{
    public static class Reflection
    {
        public static object Static<T>(string method, params object[] args)
        {
            return Static(typeof(T), method, args);
        }

        public static object Static(Type type, string methodName, params object[] args)
        {
            return Invoke(type, default, methodName, args);
        }

        public static object Instance<T>(T instance, string method, params object[] args)
        {
            return Instance(typeof(T), instance, method, args);
        }

        public static object Instance(Type type, object instance, string methodName, params object[] args)
        {
            return Invoke(type, instance, methodName, args);
        }

        private static object Invoke(Type type, object instance, string methodName, params object[] args)
        {
            var flags = instance != null ? BindingFlags.Instance : BindingFlags.Static;
            flags |= BindingFlags.Public | BindingFlags.NonPublic;

            MethodInfo method;

            try
            {
                method = type.GetMethod(methodName, flags);
            }
            catch (Exception)
            {
                var types = args.Select(a => a.GetType()).ToArray();
                method = type.GetMethod(methodName, flags, null, CallingConventions.Any, types, null);
            }

            return method.Invoke(instance, args);
        }

        public static C GetPath<C>(this Component comp, string path) where C : Component
        {
            var t = comp.transform.Find(path);
            return t ? t.GetComponent<C>() : null;
        }

        public static T GetValue<T>(this Component comp, string field)
        {
            var t = comp.GetType();
            var f = t.GetField(field);
            return (T)f.GetValue(comp);
        }

        public static void SetValue<T>(this Component comp, string field, T value)
        {
            var t = comp.GetType();
            var f = t.GetField(field);
            f.SetValue(comp, value);
        }

        public static T GetValue<C, T>(this C comp, Func<C, T> func) where C : Component => func(comp);
        public static void SetValue<C>(this C comp, Action<C> action) where C : Component => action(comp);
    }
}