// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

namespace Microsoft.SCIM
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Http;

    public sealed class ResourceQuery : IResourceQuery
    {
        private const char SeperatorAttributes = ',';

        private static readonly Lazy<char[]> SeperatorsAttributes =
            new Lazy<char[]>(
                () =>
                    new char[]
                        {
                            ResourceQuery.SeperatorAttributes
                        });

        public ResourceQuery()
        {
            Filters = Array.Empty<Filter>();
            Attributes = Array.Empty<string>();
            ExcludedAttributes = Array.Empty<string>();
        }

        public ResourceQuery(
            IReadOnlyCollection<IFilter> filters,
            IReadOnlyCollection<string> attributes,
            IReadOnlyCollection<string> excludedAttributes)
        {
            Filters = filters ?? throw new ArgumentNullException(nameof(filters));
            Attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
            ExcludedAttributes = excludedAttributes ?? throw new ArgumentNullException(nameof(excludedAttributes));
        }

        public ResourceQuery(Uri resource)
        {
            if (null == resource)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            string query = resource.Query;
            if (!string.IsNullOrWhiteSpace(query))
            {
                NameValueCollection keyedValues = HttpUtility.ParseQueryString(query);
                IEnumerable<string> keys = keyedValues.AllKeys;
                foreach (string key in keys)
                {
                    if (string.Equals(key, QueryKeys.Attributes, StringComparison.OrdinalIgnoreCase))
                    {
                        string attributeExpression = keyedValues[key];
                        if (!string.IsNullOrWhiteSpace(attributeExpression))
                        {
                            Attributes = ResourceQuery.ParseAttributes(attributeExpression);
                        }
                    }

                    if (string.Equals(key, QueryKeys.Count, StringComparison.OrdinalIgnoreCase))
                    {
                        Action<IPaginationParameters, int> action =
                            new Action<IPaginationParameters, int>(
                                (IPaginationParameters pagination, int paginationValue) =>
                                    pagination.Count = paginationValue);
                        ApplyPaginationParameter(keyedValues[key], action);
                    }

                    if (string.Equals(key, QueryKeys.ExcludedAttributes, StringComparison.OrdinalIgnoreCase))
                    {
                        string attributeExpression = keyedValues[key];
                        if (!string.IsNullOrWhiteSpace(attributeExpression))
                        {
                            ExcludedAttributes = ResourceQuery.ParseAttributes(attributeExpression);
                        }
                    }

                    if (string.Equals(key, QueryKeys.Filter, StringComparison.OrdinalIgnoreCase))
                    {
                        string filterExpression = keyedValues[key];
                        if (!string.IsNullOrWhiteSpace(filterExpression))
                        {
                            Filters = ResourceQuery.ParseFilters(filterExpression);
                        }
                    }

                    if (string.Equals(key, QueryKeys.StartIndex, StringComparison.OrdinalIgnoreCase))
                    {
                        Action<IPaginationParameters, int> action =
                            new Action<IPaginationParameters, int>(
                                (IPaginationParameters pagination, int paginationValue) =>
                                    pagination.StartIndex = paginationValue);
                        ApplyPaginationParameter(keyedValues[key], action);
                    }
                }
            }

            if (null == Filters)
            {
                Filters = Array.Empty<Filter>();
            }

            if (null == Attributes)
            {
                Attributes = Array.Empty<string>();
            }

            if (null == ExcludedAttributes)
            {
                ExcludedAttributes = Array.Empty<string>();
            }
        }

        public IReadOnlyCollection<string> Attributes
        {
            get;
            private set;
        }

        public IReadOnlyCollection<string> ExcludedAttributes
        {
            get;
            private set;
        }

        public IReadOnlyCollection<IFilter> Filters
        {
            get;
            private set;
        }

        public IPaginationParameters PaginationParameters
        {
            get;
            set;
        }

        private void ApplyPaginationParameter(
            string value,
            Action<IPaginationParameters, int> action)
        {
            if (null == action)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            int parsedValue = int.Parse(value, CultureInfo.InvariantCulture);
            if (null == PaginationParameters)
            {
                PaginationParameters = new PaginationParameters();
            }
            action(PaginationParameters, parsedValue);
        }

        private static IReadOnlyCollection<string> ParseAttributes(string attributeExpression)
        {
            if (string.IsNullOrWhiteSpace(attributeExpression))
            {
                throw new ArgumentNullException(nameof(attributeExpression));
            }

            IReadOnlyCollection<string> results =
                attributeExpression
                .Split(ResourceQuery.SeperatorsAttributes.Value)
                .Select(
                    (string item) =>
                        item.Trim())
                .ToArray();
            return results;
        }

        private static IReadOnlyCollection<IFilter> ParseFilters(string filterExpression)
        {
            if (string.IsNullOrWhiteSpace(filterExpression))
            {
                throw new ArgumentNullException(nameof(filterExpression));
            }

            if (!Filter.TryParse(filterExpression, out IReadOnlyCollection<IFilter> results))
            {
                throw new HttpResponseException(HttpStatusCode.NotAcceptable);
            }

            return results;
        }
    }
}
