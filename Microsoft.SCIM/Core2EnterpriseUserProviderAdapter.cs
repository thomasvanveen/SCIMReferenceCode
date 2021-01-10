// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

namespace Microsoft.SCIM
{
    public class Core2EnterpriseUserProviderAdapter : ProviderAdapterTemplate<Core2EnterpriseUser>
    {
        public Core2EnterpriseUserProviderAdapter(IProvider provider)
            : base(provider)
        {
        }

        public override string SchemaIdentifier => SchemaIdentifiers.Core2EnterpriseUser;
    }
}
