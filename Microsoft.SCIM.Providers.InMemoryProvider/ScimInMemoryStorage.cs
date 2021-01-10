// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using System;
using System.Collections.Generic;

namespace Microsoft.SCIM.Providers.InMemoryProvider
{
    public class ScimInMemoryStorage
    {
        internal readonly IDictionary<string, Core2Group> Groups;
        internal readonly IDictionary<string, Core2EnterpriseUser> Users;

        private ScimInMemoryStorage()
        {
            Groups = new Dictionary<string, Core2Group>();
            Users = new Dictionary<string, Core2EnterpriseUser>();
        }

        private static readonly Lazy<ScimInMemoryStorage> InstanceValue = new Lazy<ScimInMemoryStorage>(() => new ScimInMemoryStorage());

        public static ScimInMemoryStorage Instance => InstanceValue.Value;
    }
}