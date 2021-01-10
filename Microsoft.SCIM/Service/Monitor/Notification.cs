// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

namespace Microsoft.SCIM
{
    using System;
    using System.Globalization;

    public abstract class Notification<TPayload> : INotification<TPayload> where TPayload : class
    {
        private const string Template = @"{0:O} {1} {2}
{3}";

        [ThreadStatic]
        private static string correlationIdentifierDefault;

        private string correlationIdentifierValue;

        protected Notification(TPayload payload)
        {
            Message = payload ?? throw new ArgumentNullException(nameof(payload));
        }

        protected Notification(TPayload payload, long identifier)
            : this(payload)
        {
            if (null == payload)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            Identifier = identifier;
        }

        protected Notification(TPayload payload, string correlationIdentifier)
            : this(payload)
        {
            if (null == payload)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            if (string.IsNullOrWhiteSpace(correlationIdentifier))
            {
                throw new ArgumentNullException(nameof(correlationIdentifier));
            }

            CorrelationIdentifier = correlationIdentifier;
        }

        protected Notification(TPayload payload, string correlationIdentifier, long identifier)
            : this(payload, correlationIdentifier)
        {
            if (null == payload)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            if (string.IsNullOrWhiteSpace(correlationIdentifier))
            {
                throw new ArgumentNullException(nameof(correlationIdentifier));
            }

            Identifier = identifier;
        }

        public string CorrelationIdentifier
        {
            get
            {
                string result =
                    string.IsNullOrWhiteSpace(correlationIdentifierValue) ?
                        Notification<TPayload>.correlationIdentifierDefault : correlationIdentifierValue;
                return result;
            }

            private set => Notification<TPayload>.correlationIdentifierDefault =
                    correlationIdentifierValue =
                        value;
        }

        public long? Identifier
        {
            get;
            private set;
        }

        public TPayload Message
        {
            get;
            private set;
        }

        public override string ToString()
        {
            string result =
                string.Format(
                    CultureInfo.InvariantCulture,
                    Notification<TPayload>.Template,
                    DateTime.UtcNow,
                    CorrelationIdentifier,
                    Identifier,
                    Message);
            return result;

        }
    }
}
