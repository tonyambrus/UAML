using System;
using System.Collections.Generic;
using Uaml.UX;

namespace Uaml.Internal
{
    internal class DependencyPropertySet : QualifiedPropertySet<DependencyProperty>
    {
        public DependencyPropertySet(Type ownerType) 
            : base(ownerType)
        {
        }

        public DependencyPropertySet(Type ownerType, IDictionary<string, DependencyProperty> dict)
            : base(ownerType, new Dictionary<string, DependencyProperty>(dict))
        {
        }
    }
}