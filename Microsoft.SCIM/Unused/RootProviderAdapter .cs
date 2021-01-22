//// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

//using System.Collections.Generic;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace Microsoft.SCIM
//{
//    public class RootProviderAdapter : ProviderAdapterTemplate<Resource>
//    {
//        public RootProviderAdapter(IProvider provider)
//            : base(provider)
//        {
//        }

//        public override string SchemaIdentifier => SchemaIdentifiers.None;

//        public override Task<Resource> Create(
//            HttpRequestMessage request,
//            Resource resource,
//            string correlationIdentifier)
//        {
//#if DEBUG
//            Resource user = new User
//            {
//                ExternalIdentifier = resource.ExternalIdentifier,
//                Identifier = resource.Identifier,
//                UserName = "Dummy"
//            };

//            return Task.FromResult(user);
//#else
//            throw new HttpResponseException(HttpStatusCode.NotImplemented);
//#endif
//        }

//        public override IResourceIdentifier CreateResourceIdentifier(
//            string identifier)
//        {
//#if DEBUG
//            var resourceIdentifier = new ResourceIdentifier
//            {
//                Identifier = identifier,
//                SchemaIdentifier = SchemaIdentifier,
//            };

//            return resourceIdentifier;
//#else
//            throw new HttpResponseException(HttpStatusCode.NotImplemented);
//#endif
//        }

//        public override Task Delete(
//            HttpRequestMessage request,
//            string identifier,
//            string correlationIdentifier)
//        {
//#if DEBUG
//            return Task.CompletedTask;
//#else
//            throw new HttpResponseException(HttpStatusCode.NotImplemented);
//#endif
//        }

//        public override Task<Resource> Replace(
//            HttpRequestMessage request,
//            Resource resource, string
//            correlationIdentifier)
//        {
//#if DEBUG
//            return Task.FromResult(resource);
//#else
//            throw new HttpResponseException(HttpStatusCode.NotImplemented);
//#endif
//        }

//        public override Task<Resource> Retrieve(
//            HttpRequestMessage request,
//            string identifier,
//            IReadOnlyCollection<string> requestedAttributePaths,
//            IReadOnlyCollection<string> excludedAttributePaths,
//            string correlationIdentifier)
//        {
//#if DEBUG
//            Resource user = new User
//            {
//                ExternalIdentifier = null,
//                Identifier = identifier,
//                UserName = "DummyUser"
//            };

//            return Task.FromResult(user);
//#else
//            throw new HttpResponseException(HttpStatusCode.NotImplemented);
//#endif
//        }

//        public override Task Update(
//            HttpRequestMessage request,
//            string identifier,
//            PatchRequestBase patchRequest,
//            string correlationIdentifier)
//        {
//#if DEBUG
//            return Task.CompletedTask;
//#else
//            throw new HttpResponseException(HttpStatusCode.NotImplemented);
//#endif
//        }
//    }
//}