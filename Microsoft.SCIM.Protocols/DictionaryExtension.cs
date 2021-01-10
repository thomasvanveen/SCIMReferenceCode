//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.SCIM
{
    using System.Collections.Generic;
    using System.Linq;

    internal static class DictionaryExtension
    {
        public static void Trim(this IDictionary<string, object> dictionary)
        {
            IReadOnlyCollection<string> keys = dictionary.Keys.ToArray();
            foreach (string key in keys)
            {
                object value = dictionary[key];
                if (null == value)
                {
                    dictionary.Remove(key);
                }

                if (value is IDictionary<string, object> dictionaryValue)
                {
                    dictionaryValue.Trim();
                    if (dictionaryValue.Count <= 0)
                    {
                        dictionary.Remove(key);
                    }
                }
            }
        }
    }
}