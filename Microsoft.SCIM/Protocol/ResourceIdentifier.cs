//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.SCIM
{
    using System;
    using System.Globalization;

    using Microsoft.SCIM.Protocol;

    public sealed class ResourceIdentifier : IResourceIdentifier
    {
        public ResourceIdentifier()
        {
        }

        public ResourceIdentifier(string schemaIdentifier, string resourceIdentifier)
        {
            if (string.IsNullOrWhiteSpace(schemaIdentifier))
            {
                throw new ArgumentNullException(nameof(schemaIdentifier));
            }

            if (string.IsNullOrWhiteSpace(resourceIdentifier))
            {
                throw new ArgumentNullException(nameof(resourceIdentifier));
            }

            SchemaIdentifier = schemaIdentifier;
            Identifier = resourceIdentifier;
        }

        public string Identifier
        {
            get;
            set;
        }

        public string SchemaIdentifier
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            IResourceIdentifier otherIdentifier = obj as IResourceIdentifier;
            if (null == otherIdentifier)
            {
                return false;
            }

            if (!string.Equals(SchemaIdentifier, otherIdentifier.SchemaIdentifier, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.Equals(Identifier, otherIdentifier.Identifier, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            int identifierCode = string.IsNullOrWhiteSpace(Identifier) ? 0 : Identifier.GetHashCode(StringComparison.InvariantCulture);
            int schemaIdentifierCode = string.IsNullOrWhiteSpace(SchemaIdentifier) ? 0 : SchemaIdentifier.GetHashCode(StringComparison.InvariantCulture);
            int result = identifierCode ^ schemaIdentifierCode;
            return result;
        }

        public override string ToString()
        {
            string result =
                string.Format(
                    CultureInfo.InvariantCulture,
                    ProtocolResources.ResourceIdentifierTemplate,
                    SchemaIdentifier,
                    Identifier);
            return result;
        }
    }
}