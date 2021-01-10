//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.SCIM
{
    using System.Runtime.Serialization;

    [DataContract]
    public sealed class Core2ServiceConfiguration : ServiceConfigurationBase
    {
        public Core2ServiceConfiguration(
            BulkRequestsFeature bulkRequestsSupport,
            bool supportsEntityTags,
            bool supportsFiltering,
            bool supportsPasswordChange,
            bool supportsPatching,
            bool supportsSorting)
        {
            AddSchema(SchemaIdentifiers.Core2ServiceConfiguration);
            Metadata =
                new Core2Metadata()
                {
                    ResourceType = Types.ServiceProviderConfiguration
                };

            BulkRequests = bulkRequestsSupport;
            EntityTags = new Feature(supportsEntityTags);
            Filtering = new Feature(supportsFiltering);
            PasswordChange = new Feature(supportsPasswordChange);
            Patching = new Feature(supportsPatching);
            Sorting = new Feature(supportsSorting);
        }

        [DataMember(Name = AttributeNames.Metadata)]
        public Core2Metadata Metadata
        {
            get;
            set;
        }
    }
}