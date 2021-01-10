//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.SCIM
{
    using System;
    using System.Collections.Generic;

    public class SingularUnsecuredEventTokenFactory : UnsecuredEventTokenFactory
    {
        public SingularUnsecuredEventTokenFactory(string issuer, string eventSchemaIdentifier)
            : base(issuer)
        {
            if (string.IsNullOrWhiteSpace(eventSchemaIdentifier))
            {
                throw new ArgumentNullException(nameof(eventSchemaIdentifier));
            }

            EventSchemaIdentifier = eventSchemaIdentifier;
        }

        private string EventSchemaIdentifier
        {
            get;
            set;
        }

        public override IEventToken Create(IDictionary<string, object> events)
        {
            IDictionary<string, object> tokenEvents = new Dictionary<string, object>(1)
            {
                { EventSchemaIdentifier, events }
            };
            IEventToken result = new EventToken(Issuer, Header, tokenEvents);
            return result;
        }
    }
}