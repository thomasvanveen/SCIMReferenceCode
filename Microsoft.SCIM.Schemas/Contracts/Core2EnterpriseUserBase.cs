//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.SCIM
{
    using System.Runtime.Serialization;

    [DataContract]
    public abstract class Core2EnterpriseUserBase : Core2UserBase
    {
        protected Core2EnterpriseUserBase()
            : base()
        {
            AddSchema(SchemaIdentifiers.Core2EnterpriseUser);
            EnterpriseExtension = new ExtensionAttributeEnterpriseUser2();
        }

        [DataMember(Name = AttributeNames.ExtensionEnterpriseUser2)]
        public ExtensionAttributeEnterpriseUser2 EnterpriseExtension
        {
            get;
            set;
        }
    }
}