using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tweakable
{
    public class TypeData
    {
        public Type type;
        public Dictionary<string, FieldData> fields;
        public Dictionary<string, MethodInfo> methods;
    }
}