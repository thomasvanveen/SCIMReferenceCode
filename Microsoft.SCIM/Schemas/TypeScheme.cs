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
    public sealed class TypeScheme : IJsonSerializable
    {
        [DataMember(Name = AttributeNames.Attributes, Order = 0)]
        private List<AttributeScheme> attributes;
        private IReadOnlyCollection<AttributeScheme> attributesWrapper;

        private IJsonSerializable serializer;
        private object thisLock;

        public TypeScheme()
        {
            OnInitialization();
            OnInitialized();
        }

        public IReadOnlyCollection<AttributeScheme> Attributes => attributesWrapper;

        [DataMember(Name = AttributeNames.Description)]
        public string Description
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Identifier)]
        public string Identifier
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Name)]
        public string Name
        {
            get;
            set;
        }

        public void AddAttribute(AttributeScheme attribute)
        {
            if (null == attribute)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            Func<bool> containsFunction =
                new Func<bool>(
                        () =>

                            attributes
                            .Any(
                                (AttributeScheme item) =>
                                    string.Equals(item.Name, attribute.Name, StringComparison.OrdinalIgnoreCase)));


            if (!containsFunction())
            {
                lock (thisLock)
                {
                    if (!containsFunction())
                    {
                        attributes.Add(attribute);
                    }
                }
            }
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
            attributes = new List<AttributeScheme>();
        }

        private void OnInitialized()
        {
            attributesWrapper = attributes.AsReadOnly();
        }

        public string Serialize()
        {
            string result = serializer.Serialize();
            return result;
        }

        public Dictionary<string, object> ToJson()
        {
            Dictionary<string, object> result = serializer.ToJson();
            return result;
        }
    }
}