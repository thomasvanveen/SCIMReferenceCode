// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

namespace Microsoft.SCIM.WebHostSample.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Microsoft.SCIM;
    using Microsoft.SCIM.WebHostSample.Resources;

    public class InMemoryUserProvider : ProviderBase
    {
        private readonly InMemoryStorage storage;

        public InMemoryUserProvider()
        {
            storage = InMemoryStorage.Instance;
        }

        public override Task<Resource> CreateAsync(Resource resource, string correlationIdentifier)
        {
            if (resource.Identifier != null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            Core2EnterpriseUser user = resource as Core2EnterpriseUser;
            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            IEnumerable<Core2EnterpriseUser> exisitingUsers = storage.Users.Values;
            if
            (
                exisitingUsers.Any(
                    (Core2EnterpriseUser exisitingUser) =>
                        string.Equals(exisitingUser.UserName, user.UserName, StringComparison.Ordinal))
            )
            {
                throw new HttpResponseException(HttpStatusCode.Conflict);
            }

            string resourceIdentifier = Guid.NewGuid().ToString();
            resource.Identifier = resourceIdentifier;
            storage.Users.Add(resourceIdentifier, user);

            return Task.FromResult(resource);
        }

        public override Task DeleteAsync(IResourceIdentifier resourceIdentifier, string correlationIdentifier)
        {
            if (string.IsNullOrWhiteSpace(resourceIdentifier?.Identifier))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            string identifier = resourceIdentifier.Identifier;

            if (storage.Users.ContainsKey(identifier))
            {
                storage.Users.Remove(identifier);
            }

            return Task.CompletedTask;
        }

        public override Task<Resource[]> QueryAsync(IQueryParameters parameters, string correlationIdentifier)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (string.IsNullOrWhiteSpace(correlationIdentifier))
            {
                throw new ArgumentNullException(nameof(correlationIdentifier));
            }

            if (null == parameters.AlternateFilters)
            {
                throw new ArgumentException(SampleServiceResources.ExceptionInvalidParameters);
            }

            if (string.IsNullOrWhiteSpace(parameters.SchemaIdentifier))
            {
                throw new ArgumentException(SampleServiceResources.ExceptionInvalidParameters);
            }

            Resource[] results;
            IFilter queryFilter = parameters.AlternateFilters.SingleOrDefault();
            if (queryFilter == null)
            {
                IEnumerable<Core2EnterpriseUser> allUsers = storage.Users.Values;
                results =
                    allUsers.Select((Core2EnterpriseUser user) => user as Resource).ToArray();

                return Task.FromResult(results);
            }

            if (string.IsNullOrWhiteSpace(queryFilter.AttributePath))
            {
                throw new ArgumentException(SampleServiceResources.ExceptionInvalidParameters);
            }

            if (string.IsNullOrWhiteSpace(queryFilter.ComparisonValue))
            {
                throw new ArgumentException(SampleServiceResources.ExceptionInvalidParameters);
            }

            if (queryFilter.FilterOperator != ComparisonOperator.Equals)
            {
                throw new NotSupportedException(SampleServiceResources.UnsupportedComparisonOperator);
            }

            if (queryFilter.AttributePath.Equals(AttributeNames.UserName))
            {
                IEnumerable<Core2EnterpriseUser> allUsers = storage.Users.Values;
                results =
                    allUsers.Where(
                        (Core2EnterpriseUser item) =>
                           string.Equals(
                                item.UserName,
                               parameters.AlternateFilters.Single().ComparisonValue,
                               StringComparison.OrdinalIgnoreCase))
                               .Select((Core2EnterpriseUser user) => user as Resource).ToArray();

                return Task.FromResult(results);
            }

            if (queryFilter.AttributePath.Equals(AttributeNames.ExternalIdentifier))
            {
                IEnumerable<Core2EnterpriseUser> allUsers = storage.Users.Values;
                results =
                    allUsers.Where(
                        (Core2EnterpriseUser item) =>
                           string.Equals(
                                item.ExternalIdentifier,
                               parameters.AlternateFilters.Single().ComparisonValue,
                               StringComparison.OrdinalIgnoreCase))
                               .Select((Core2EnterpriseUser user) => user as Resource).ToArray();

                return Task.FromResult(results);
            }

            throw new NotSupportedException(SampleServiceResources.UnsupportedFilterAttributeUser);
        }

        public override Task<Resource> ReplaceAsync(Resource resource, string correlationIdentifier)
        {
            if (resource.Identifier == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            Core2EnterpriseUser user = resource as Core2EnterpriseUser;

            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            IEnumerable<Core2EnterpriseUser> exisitingUsers = storage.Users.Values;
            if
            (
                exisitingUsers.Any(
                    (Core2EnterpriseUser exisitingUser) =>
                        string.Equals(exisitingUser.UserName, user.UserName, StringComparison.Ordinal) &&
                        !string.Equals(exisitingUser.Identifier, user.Identifier, StringComparison.OrdinalIgnoreCase))
            )
            {
                throw new HttpResponseException(HttpStatusCode.Conflict);
            }

            if (!storage.Users.TryGetValue(user.Identifier, out Core2EnterpriseUser _))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            storage.Users[user.Identifier] = user;
            Resource result = user;
            return Task.FromResult(result);
        }

        public override Task<Resource> RetrieveAsync(IResourceRetrievalParameters parameters, string correlationIdentifier)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (string.IsNullOrWhiteSpace(correlationIdentifier))
            {
                throw new ArgumentNullException(nameof(correlationIdentifier));
            }

            if (string.IsNullOrEmpty(parameters?.ResourceIdentifier?.Identifier))
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            Resource result = null;
            string identifier = parameters.ResourceIdentifier.Identifier;

            if (storage.Users.ContainsKey(identifier))
            {
                if (storage.Users.TryGetValue(identifier, out Core2EnterpriseUser user))
                {
                    result = user;
                    return Task.FromResult(result);
                }
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        public override Task UpdateAsync(IPatch patch, string correlationIdentifier)
        {
            if (null == patch)
            {
                throw new ArgumentNullException(nameof(patch));
            }

            if (null == patch.ResourceIdentifier)
            {
                throw new ArgumentException(SampleServiceResources.ExceptionInvalidPatch);
            }

            if (string.IsNullOrWhiteSpace(patch.ResourceIdentifier.Identifier))
            {
                throw new ArgumentException(SampleServiceResources.ExceptionInvalidPatch);
            }

            if (null == patch.PatchRequest)
            {
                throw new ArgumentException(SampleServiceResources.ExceptionInvalidPatch);
            }

            PatchRequest2 patchRequest =
                patch.PatchRequest as PatchRequest2;

            if (null == patchRequest)
            {
                string unsupportedPatchTypeName = patch.GetType().FullName;
                throw new NotSupportedException(unsupportedPatchTypeName);
            }

            if (storage.Users.TryGetValue(patch.ResourceIdentifier.Identifier, out Core2EnterpriseUser user))
            {
                user.Apply(patchRequest);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return Task.CompletedTask;
        }
    }
}
