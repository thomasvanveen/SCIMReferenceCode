// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using System;
using System.Collections.Generic;

namespace Microsoft.SCIM.WebHostSample.Provider
{
    public class InMemoryStorage
    {
        internal readonly IDictionary<string, Core2Group> Groups;
        internal readonly IDictionary<string, Core2EnterpriseUser> Users;

        private InMemoryStorage()
        {
            Groups = new Dictionary<string, Core2Group>();
            Users = new Dictionary<string, Core2EnterpriseUser>();
        }

        private static readonly Lazy<InMemoryStorage> InstanceValue = new Lazy<InMemoryStorage>(() => new InMemoryStorage());

        public static InMemoryStorage Instance => InMemoryStorage.InstanceValue.Value;
    }
}