//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.SCIM
{
    using System;

    public class SchemaIdentifier : ISchemaIdentifier
    {
        public SchemaIdentifier(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
        }

        public string Value
        {
            get;
            private set;
        }

        public string FindPath()
        {
            if (!TryFindPath(out string result))
            {
                throw new NotSupportedException(Value);
            }

            return result;
        }

        public bool TryFindPath(out string path)
        {
            path = null;

            switch (Value)
            {
                case SchemaIdentifiers.Core2EnterpriseUser:
                case SchemaIdentifiers.Core2User:
                    path = ProtocolConstants.PathUsers;
                    return true;
                case SchemaIdentifiers.Core2Group:
                    path = ProtocolConstants.PathGroups;
                    return true;
                case SchemaIdentifiers.None:
                    path = SchemaConstants.PathInterface;
                    return true;
                default:
                    return false;
            }
        }
    }
}