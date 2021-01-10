// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

namespace Microsoft.SCIM
{
    using System.Collections.Generic;
    using System.Net.Http;

    public sealed class RetrievalRequest :
        ScimRequest<IResourceRetrievalParameters>
    {
        public RetrievalRequest(
            HttpRequestMessage request,
            IResourceRetrievalParameters payload,
            string correlationIdentifier,
            IReadOnlyCollection<IExtension> extensions)
            : base(request, payload, correlationIdentifier, extensions)
        {
        }
    }
}
