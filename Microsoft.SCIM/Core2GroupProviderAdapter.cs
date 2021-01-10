// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

namespace Microsoft.SCIM
{
    public class Core2GroupProviderAdapter : ProviderAdapterTemplate<Core2Group>
    {
        public Core2GroupProviderAdapter(IProvider provider)
            : base(provider)
        {
        }

        public override string SchemaIdentifier => SchemaIdentifiers.Core2Group;
    }
}
