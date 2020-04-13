using SerializableCollections;
using System;
using System.Collections.Generic;

namespace Uaml.Core
{
    [Serializable]
    public class StringSerializableDictionary : SerializableDictionary<string, string>, IReadOnlyDictionary<string, string>
    {
        IEnumerable<string> IReadOnlyDictionary<string, string>.Keys => base.Keys;
        IEnumerable<string> IReadOnlyDictionary<string, string>.Values => base.Values;
    }
}
