//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.SCIM
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract]
    public abstract class Schematized : IJsonSerializable
    {
        [DataMember(Name = AttributeNames.Schemas, Order = 0)]
        private List<string> schemas;
        private IReadOnlyCollection<string> schemasWrapper;

        private object thisLock;
        private IJsonSerializable serializer;

        protected Schematized()
        {
            OnInitialization();
            OnInitialized();
        }

        public virtual IReadOnlyCollection<string> Schemas => schemasWrapper;

        public void AddSchema(string schemaIdentifier)
        {
            if (string.IsNullOrWhiteSpace(schemaIdentifier))
            {
                throw new ArgumentNullException(nameof(schemaIdentifier));
            }

            Func<bool> containsFunction =
                new Func<bool>(
                    () =>

                        schemas
                        .Any(
                            (string item) =>
                                string.Equals(
                                    item,
                                    schemaIdentifier,
                                    StringComparison.OrdinalIgnoreCase)));


            if (!containsFunction())
            {
                lock (thisLock)
                {
                    if (!containsFunction())
                    {
                        schemas.Add(schemaIdentifier);
                    }
                }
            }
        }

        public bool Is(string scheme)
        {
            if (string.IsNullOrWhiteSpace(scheme))
            {
                throw new ArgumentNullException(nameof(scheme));
            }

            bool result =

                schemas
                .Any(
                    (string item) =>
                        string.Equals(item, scheme, StringComparison.OrdinalIgnoreCase));
            return result;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            OnInitialized();
        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {
            OnInitialization();
        }

        private void OnInitialization()
        {
            thisLock = new object();
            serializer = new JsonSerializer(this);
            schemas = new List<string>();
        }

        private void OnInitialized()
        {
            schemasWrapper = schemas.AsReadOnly();
        }

        public virtual Dictionary<string, object> ToJson()
        {
            Dictionary<string, object> result = serializer.ToJson();
            return result;
        }

        public virtual string Serialize()
        {
            IDictionary<string, object> json = ToJson();
            string result = JsonFactory.Instance.Create(json, true);
            return result;
        }

        public override string ToString()
        {
            string result = Serialize();
            return result;
        }

        public virtual bool TryGetPath(out string path)
        {
            path = null;
            return false;
        }

        public virtual bool TryGetSchemaIdentifier(out string schemaIdentifier)
        {
            schemaIdentifier = null;
            return false;
        }
    }
}