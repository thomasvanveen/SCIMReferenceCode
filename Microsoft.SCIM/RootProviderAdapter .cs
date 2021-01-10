// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

namespace Microsoft.SCIM
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class RootProviderAdapter : ProviderAdapterTemplate<Resource>
    {
        public RootProviderAdapter(IProvider provider)
            : base(provider)
        {
        }

        public override string SchemaIdentifier => SchemaIdentifiers.None;

        public override Task<Resource> Create(
            HttpRequestMessage request,
            Resource resource,
            string correlationIdentifier)
        {
            Resource user = new User
            {
                ExternalIdentifier = resource.ExternalIdentifier,
                Identifier = resource.Identifier,
                UserName = "Dummy"
            };

            return Task.FromResult(user);
        }

        public override IResourceIdentifier CreateResourceIdentifier(
            string identifier)
        {
            var resourceIdentifier = new ResourceIdentifier
            {
                Identifier = identifier,
                SchemaIdentifier = SchemaIdentifier,
            };

            return resourceIdentifier;
        }

        public override Task Delete(
            HttpRequestMessage request,
            string identifier,
            string correlationIdentifier)
        {
            throw new HttpResponseException(HttpStatusCode.NotImplemented);
        }

        public override Task<Resource> Replace(
            HttpRequestMessage request,
            Resource resource, string
            correlationIdentifier)
        {
            return Task.FromResult(resource);
        }

        public override Task<Resource> Retrieve(
            HttpRequestMessage request,
            string identifier,
            IReadOnlyCollection<string> requestedAttributePaths,
            IReadOnlyCollection<string> excludedAttributePaths,
            string correlationIdentifier)
        {
            Resource user = new User
            {
                ExternalIdentifier = null,
                Identifier = identifier,
                UserName = "DummyUser"
            };

            return Task.FromResult(user);
        }

        public override Task Update(
            HttpRequestMessage request,
            string identifier,
            PatchRequestBase patchRequest,
            string correlationIdentifier)
        {
            return Task.CompletedTask;
        }
    }
}
