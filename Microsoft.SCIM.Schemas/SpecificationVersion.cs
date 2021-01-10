//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.SCIM
{
    using System;

    public static class SpecificationVersion
    {
        private static readonly Lazy<Version> VersionOneOhValue =
            new Lazy<System.Version>(
                () =>
                    new Version(1, 0));

        private static readonly Lazy<Version> VersionOneOneValue =
            new Lazy<Version>(
                () =>
                    new Version(1, 1));

        private static readonly Lazy<Version> VersionTwoOhValue =
            new Lazy<Version>(
                () =>
                    new Version(2, 0));

        // NOTE: This version is to be used for DCaaS only.
        private static readonly Lazy<Version> VersionTwoOhOneValue =
            new Lazy<Version>(
                () =>
                    new Version(2, 0, 1));

        public static Version VersionOneOh => SpecificationVersion.VersionOneOhValue.Value;

        public static Version VersionOneOne => SpecificationVersion.VersionOneOneValue.Value;

        public static Version VersionTwoOhOne => SpecificationVersion.VersionTwoOhOneValue.Value;

        public static Version VersionTwoOh => SpecificationVersion.VersionTwoOhValue.Value;
    }
}