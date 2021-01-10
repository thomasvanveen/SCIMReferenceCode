// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

namespace Microsoft.SCIM
{
    public sealed class InformationNotification : Notification<string>, IInformationNotification
    {
        public InformationNotification(string payload, bool verbose)
            : base(payload)
        {
            Verbose = verbose;
        }

        public InformationNotification(string payload)
            : this(payload, false)
        {
        }

        public InformationNotification(string payload, long identifier)
            : base(payload, identifier)
        {
            Verbose = false;
        }

        public InformationNotification(string payload, bool verbose, long identifier)
            : base(payload, identifier)
        {
            Verbose = verbose;
        }

        public InformationNotification(
            string payload,
            bool verbose,
            string correlationIdentifier)
            : base(payload, correlationIdentifier)
        {
            Verbose = verbose;
        }

        public InformationNotification(
            string payload,
            string correlationIdentifier)
            : this(payload, false, correlationIdentifier)
        {
        }

        public InformationNotification(
            string payload,
            bool verbose,
            string correlationIdentifier,
            long identifier)
            : base(payload, correlationIdentifier, identifier)
        {
            Verbose = verbose;
        }

        public InformationNotification(
            string payload,
            string correlationIdentifier,
            long identifier)
            : this(payload, false, correlationIdentifier, identifier)
        {
        }

        public bool Verbose
        {
            get;
            set;
        }
    }
}
