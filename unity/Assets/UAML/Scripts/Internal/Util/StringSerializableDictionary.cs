using System;
using System.Collections.Generic;

namespace Uaml.Core
{
    [Serializable]
    public class StringSerializableDictionary : SerializableDictionary<string, string>, IReadOnlyDictionary<string, string>
    {
    }
}
