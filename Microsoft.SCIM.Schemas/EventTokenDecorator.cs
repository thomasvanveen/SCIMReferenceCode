//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.SCIM
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;

    public abstract class EventTokenDecorator : IEventToken
    {
        protected EventTokenDecorator(IEventToken innerToken)
        {
            InnerToken = innerToken ?? throw new ArgumentNullException(nameof(innerToken));
        }

        protected EventTokenDecorator(string serialized)
            : this(new EventToken(serialized))
        {
        }

        public IReadOnlyCollection<string> Audience
        {
            get
            {
                IReadOnlyCollection<string> result = InnerToken.Audience;
                return result;
            }

            set => InnerToken.Audience = value;
        }

        public IDictionary<string, object> Events
        {
            get
            {
                IDictionary<string, object> results = InnerToken.Events;
                return results;
            }
        }

        public DateTime? Expiration
        {
            get
            {
                DateTime? result = InnerToken.Expiration;
                return result;
            }

            set => InnerToken.Expiration = value;
        }

        public JwtHeader Header
        {
            get
            {
                JwtHeader result = InnerToken.Header;
                return result;
            }
        }

        public string Identifier
        {
            get
            {
                string result = InnerToken.Identifier;
                return result;
            }
        }

        public IEventToken InnerToken
        {
            get;
            private set;
        }

        public DateTime IssuedAt
        {
            get
            {
                DateTime result = InnerToken.IssuedAt;
                return result;
            }
        }

        public string Issuer
        {
            get
            {
                string result = InnerToken.Issuer;
                return result;
            }
        }

        public DateTime? NotBefore
        {
            get
            {
                DateTime? result = InnerToken.NotBefore;
                return result;
            }
        }

        public string Subject
        {
            get
            {
                string result = InnerToken.Subject;
                return result;
            }

            set => throw new NotImplementedException();
        }

        public string Transaction
        {
            get
            {
                string result = InnerToken.Transaction;
                return result;
            }

            set => InnerToken.Transaction = value;
        }
    }
}