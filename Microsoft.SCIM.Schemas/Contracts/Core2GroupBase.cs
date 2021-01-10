//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.SCIM
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    [DataContract]
    public abstract class Core2GroupBase : GroupBase
    {
        private IDictionary<string, IDictionary<string, object>> customExtension;

        protected Core2GroupBase()
        {
            AddSchema(SchemaIdentifiers.Core2Group);
            Metadata =
                new Core2Metadata()
                {
                    ResourceType = Types.Group
                };
            OnInitialization();
        }

        public virtual IReadOnlyDictionary<string, IDictionary<string, object>> CustomExtension => new ReadOnlyDictionary<string, IDictionary<string, object>>(customExtension);

        [DataMember(Name = AttributeNames.Metadata)]
        public Core2Metadata Metadata
        {
            get;
            set;
        }

        public virtual void AddCustomAttribute(string key, object value)
        {
            if
            (
                    key != null
                && key.StartsWith(SchemaIdentifiers.PrefixExtension, StringComparison.OrdinalIgnoreCase)
                && value is Dictionary<string, object> nestedObject
            )
            {
                customExtension.Add(key, nestedObject);
            }
        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {
            OnInitialization();
        }

        private void OnInitialization()
        {
            customExtension = new Dictionary<string, IDictionary<string, object>>();
        }

        public override Dictionary<string, object> ToJson()
        {
            Dictionary<string, object> result = base.ToJson();

            foreach (KeyValuePair<string, IDictionary<string, object>> entry in CustomExtension)
            {
                result.Add(entry.Key, entry.Value);
            }

            return result;
        }
    }
}