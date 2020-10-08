using System;
using System.Reflection;
using UnityEngine;

namespace Tweakable
{
    public class FieldData
    {
        public FieldInfo field;
        private TweakAttribute tweak;
        private MethodInfo notifyMethod;

        public Type FieldType => field.FieldType;

        public FieldData(FieldInfo field)
        {
            this.field = field;
        }

        public object GetValue(object instance) => field.GetValue(instance);

        public void SetValue(object instance, object value)
        {
            field.SetValue(instance, value);

            if (tweak == null)
            {
                tweak = field.GetCustomAttribute<TweakAttribute>();
            }

            var notify = tweak.Notify;
            if (notify != null)
            {
                if (notifyMethod == null)
                {
                    var flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
                    var method = field.DeclaringType.GetMethod(notify, flags);
                    if (method != null && method.GetParameters().Length == 0)
                    {
                        notifyMethod = method;
                    }
                    else
                    {
                        Debug.LogError($"Can't find method {field.DeclaringType}.{notify}()");
                    }
                }

                if (notifyMethod != null)
                {
                    notifyMethod.Invoke(instance, null);
                }
            }
        }
    }
}