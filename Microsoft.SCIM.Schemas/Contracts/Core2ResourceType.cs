//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.SCIM
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public sealed class Core2ResourceType : Schematized
    {
        private Uri endpoint;

        [DataMember(Name = AttributeNames.Endpoint)]
        private string endpointValue;

        [DataMember(Name = AttributeNames.Identifier)]
        public string Identifier
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Name)]
        private string name;

        public Core2ResourceType()
        {
            AddSchema(SchemaIdentifiers.Core2ResourceType);
            Metadata =
                new Core2Metadata()
                {
                    ResourceType = Types.ResourceType
                };
        }

        public Uri Endpoint
        {
            get => endpoint;

            set
            {
                endpoint = value;
                endpointValue = new SystemForCrossDomainIdentityManagementResourceIdentifier(value).RelativePath;
            }
        }

        [DataMember(Name = AttributeNames.Metadata)]
        public Core2Metadata Metadata
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Schema)]
        public string Schema
        {
            get;
            set;
        }

        private void InitializeEndpoint(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                endpoint = null;
                return;
            }

            endpoint = new Uri(value, UriKind.Relative);
        }

        private void InitializeEndpoint()
        {
            InitializeEndpoint(endpointValue);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            InitializeEndpoint();
        }

        [OnSerializing]
        private void OnSerializing(StreamingContext context)
        {
            name = Identifier;
        }
    }
}