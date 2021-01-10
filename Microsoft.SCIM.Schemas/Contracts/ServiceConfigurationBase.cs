//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.SCIM
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    using Microsoft.SCIM.Schemas;

    [DataContract]
    public abstract class ServiceConfigurationBase : Schematized
    {
        [DataMember(Name = AttributeNames.AuthenticationSchemes)]
        private List<AuthenticationScheme> authenticationSchemes;
        private IReadOnlyCollection<AuthenticationScheme> authenticationSchemesWrapper;

        private object thisLock;

        protected ServiceConfigurationBase()
        {
            OnInitialization();
            OnInitialized();
        }

        public IReadOnlyCollection<AuthenticationScheme> AuthenticationSchemes => authenticationSchemesWrapper;

        [DataMember(Name = AttributeNames.Bulk)]
        public BulkRequestsFeature BulkRequests
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Documentation)]
        public Uri DocumentationResource
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.EntityTag)]
        public Feature EntityTags
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Filter)]
        public Feature Filtering
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.ChangePassword)]
        public Feature PasswordChange
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Patch)]
        public Feature Patching
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Sort)]
        public Feature Sorting
        {
            get;
            set;
        }

        public void AddAuthenticationScheme(AuthenticationScheme authenticationScheme)
        {
            if (null == authenticationScheme)
            {
                throw new ArgumentNullException(nameof(authenticationScheme));
            }

            if (string.IsNullOrWhiteSpace(authenticationScheme.Name))
            {
                throw new ArgumentException(
                    SchemasResources.ExceptionUnnamedAuthenticationScheme);
            }

            Func<bool> containsFunction =
                new Func<bool>(
                        () =>

                            authenticationSchemes
                            .Any(
                                (AuthenticationScheme item) =>
                                    string.Equals(item.Name, authenticationScheme.Name, StringComparison.OrdinalIgnoreCase)));


            if (!containsFunction())
            {
                lock (thisLock)
                {
                    if (!containsFunction())
                    {
                        authenticationSchemes.Add(authenticationScheme);
                    }
                }
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            OnInitialized();
        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {
            OnInitialization();
        }

        private void OnInitialization()
        {
            thisLock = new object();
            authenticationSchemes = new List<AuthenticationScheme>();
        }

        private void OnInitialized()
        {
            authenticationSchemesWrapper = authenticationSchemes.AsReadOnly();
        }
    }
}