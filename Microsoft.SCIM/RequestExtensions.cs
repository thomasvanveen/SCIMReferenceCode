// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

namespace Microsoft.SCIM
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    public static class RequestExtensions
    {
        private const string SegmentInterface =
            RequestExtensions.SegmentSeparator +
            SchemaConstants.PathInterface +
            RequestExtensions.SegmentSeparator;
        private const string SegmentSeparator = "/";

        private static readonly Lazy<char[]> SegmentSeparators =
            new Lazy<char[]>(
                () =>
                    SegmentSeparator.ToArray());

        public static Uri GetBaseResourceIdentifier(this HttpRequestMessage request)
        {
            if (null == request.RequestUri)
            {
                throw new ArgumentException(ServiceResources.ExceptionInvalidRequest);
            }

            string lastSegment =
                request.RequestUri.AbsolutePath.Split(
                    RequestExtensions.SegmentSeparators.Value,
                    StringSplitOptions.RemoveEmptyEntries)
                .Last();
            if (string.Equals(lastSegment, SchemaConstants.PathInterface, StringComparison.OrdinalIgnoreCase))
            {
                return request.RequestUri;
            }

            string resourceIdentifier = request.RequestUri.AbsoluteUri;

            int indexInterface =
                resourceIdentifier
                .LastIndexOf(
                    RequestExtensions.SegmentInterface,
                    StringComparison.OrdinalIgnoreCase);

            if (indexInterface < 0)
            {
                throw new ArgumentException(ServiceResources.ExceptionInvalidRequest);
            }

            string baseResource = resourceIdentifier.Substring(0, indexInterface);
            Uri result = new Uri(baseResource, UriKind.Absolute);
            return result;
        }

        public static bool TryGetRequestIdentifier(this HttpRequestMessage request, out string requestIdentifier)
        {
            request?.Headers.TryGetValues("client-id", out IEnumerable<string> _);
            requestIdentifier = Guid.NewGuid().ToString();
            return true;
        }
    }
}
