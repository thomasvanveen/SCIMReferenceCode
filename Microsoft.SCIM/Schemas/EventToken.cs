//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.SCIM
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;

    using Microsoft.IdentityModel.Tokens;
    using Microsoft.SCIM.Schemas;

    // Implements https://tools.ietf.org/html/draft-ietf-secevent-token
    public class EventToken : IEventToken
    {
        public const string HeaderKeyAlgorithm = "alg";
        public const string JwtAlgorithmNone = "none";

        private static readonly Lazy<JwtHeader> HeaderDefault =
            new Lazy<JwtHeader>(
                () =>
                    EventToken.ComposeDefaultHeader());

        private static readonly Lazy<SecurityTokenHandler> TokenSerializer =
            new Lazy<SecurityTokenHandler>(
                    () =>
                        new JwtSecurityTokenHandler());

        private EventToken(string issuer, JwtHeader header)
        {
            if (string.IsNullOrWhiteSpace(issuer))
            {
                throw new ArgumentNullException(nameof(issuer));
            }

            Issuer = issuer;
            Header = header ?? throw new ArgumentNullException(nameof(header));

            Identifier = Guid.NewGuid().ToString();
            IssuedAt = DateTime.UtcNow;
        }

        public EventToken(string issuer, JwtHeader header, IDictionary<string, object> events)
            : this(issuer, header)
        {
            Events = events ?? throw new ArgumentNullException(nameof(events));
        }

        public EventToken(string issuer, Dictionary<string, object> events)
            : this(issuer, EventToken.HeaderDefault.Value, events)
        {
        }

        public EventToken(string serialized)
        {
            if (string.IsNullOrWhiteSpace(serialized))
            {
                throw new ArgumentNullException(nameof(serialized));
            }

            JwtSecurityToken token = new JwtSecurityToken(serialized);
            Header = token.Header;

            ParseIdentifier(token.Payload);
            ParseIssuer(token.Payload);
            ParseAudience(token.Payload);
            ParseIssuedAt(token.Payload);
            ParseNotBefore(token.Payload);
            ParseSubject(token.Payload);
            ParseExpiration(token.Payload);
            ParseEvents(token.Payload);
            ParseTransaction(token.Payload);
        }

        public IReadOnlyCollection<string> Audience
        {
            get;
            set;
        }

        public IDictionary<string, object> Events
        {
            get;
            private set;
        }

        public DateTime? Expiration
        {
            get;
            set;
        }

        public JwtHeader Header
        {
            get;
            private set;
        }

        public string Identifier
        {
            get;
            private set;
        }

        public DateTime IssuedAt
        {
            get;
            private set;
        }

        public string Issuer
        {
            get;
            private set;
        }

        public DateTime? NotBefore
        {
            get;
            private set;
        }

        public string Subject
        {
            get;
            set;
        }

        public string Transaction
        {
            get;
            set;
        }

        private static JwtHeader ComposeDefaultHeader()
        {
            JwtHeader result = new JwtHeader
            {
                { EventToken.HeaderKeyAlgorithm, EventToken.JwtAlgorithmNone }
            };
            return result;
        }

        private void ParseAudience(JwtPayload payload)
        {
            if (null == payload)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            if (!payload.TryGetValue(EventTokenClaimTypes.Audience, out object value) || null == value)
            {
                return;
            }

            object[] values = value as object[];
            if (null == values)
            {
                string exceptionMessage =
                string.Format(
                    CultureInfo.InvariantCulture,
                    SchemasResources.ExceptionEventTokenInvalidClaimValueTemplate,
                    EventTokenClaimTypes.Audience,
                    value);
                throw new ArgumentException(exceptionMessage);
            }

            IReadOnlyCollection<string> audience =
                values
                .OfType<string>()
                .ToArray();
            if (audience.Count != values.Length)
            {
                string exceptionMessage =
                string.Format(
                    CultureInfo.InvariantCulture,
                    SchemasResources.ExceptionEventTokenInvalidClaimValueTemplate,
                    EventTokenClaimTypes.Audience,
                    value);
                throw new ArgumentException(exceptionMessage);
            }

            Audience = audience;
        }

        private void ParseEvents(JwtPayload payload)
        {
            if (null == payload)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            if (!payload.TryGetValue(EventTokenClaimTypes.Events, out object value) || null == value)
            {
                string exceptionMessage =
                    string.Format(
                        CultureInfo.InvariantCulture,
                        SchemasResources.ExceptionEventTokenMissingClaimTemplate,
                        EventTokenClaimTypes.Events);
                throw new ArgumentException(exceptionMessage);
            }

            IDictionary<string, object> events = value as Dictionary<string, object>;
            if (null == events)
            {
                string exceptionMessage =
                    string.Format(
                        CultureInfo.InvariantCulture,
                        SchemasResources.ExceptionEventTokenInvalidClaimValueTemplate,
                        EventTokenClaimTypes.Events,
                        value);
                throw new ArgumentException(exceptionMessage);
            }
            Events = events;
        }

        private void ParseExpiration(JwtPayload payload)
        {
            if (null == payload)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            if (!payload.TryGetValue(EventTokenClaimTypes.Expiration, out object value) || null == value)
            {
                return;
            }

            string serializedValue = value.ToString();
            if (!long.TryParse(serializedValue, out long expiration))
            {
                string exceptionMessage =
                    string.Format(
                        CultureInfo.InvariantCulture,
                        SchemasResources.ExceptionEventTokenInvalidClaimValueTemplate,
                        EventTokenClaimTypes.Expiration,
                        value);
                throw new ArgumentException(exceptionMessage);
            }

            Expiration = new UnixTime(expiration).ToUniversalTime();
            if (Expiration > DateTime.UtcNow)
            {
                throw new SecurityTokenExpiredException(SchemasResources.ExceptionEventTokenExpired);
            }
        }

        private void ParseIdentifier(JwtPayload payload)
        {
            if (null == payload)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            if (!payload.TryGetValue(EventTokenClaimTypes.Identifier, out object value) || null == value)
            {
                string exceptionMessage =
                    string.Format(
                        CultureInfo.InvariantCulture,
                        SchemasResources.ExceptionEventTokenMissingClaimTemplate,
                        EventTokenClaimTypes.Identifier);
                throw new ArgumentException(exceptionMessage);
            }

            string identifier = value as string;
            if (string.IsNullOrWhiteSpace(identifier))
            {
                string exceptionMessage =
                    string.Format(
                        CultureInfo.InvariantCulture,
                        SchemasResources.ExceptionEventTokenInvalidClaimValueTemplate,
                        EventTokenClaimTypes.Identifier,
                        value);
                throw new ArgumentException(exceptionMessage);
            }
            Identifier = identifier;
        }

        private void ParseIssuedAt(JwtPayload payload)
        {
            if (null == payload)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            if (!payload.TryGetValue(EventTokenClaimTypes.IssuedAt, out object value) || null == value)
            {
                string exceptionMessage =
                    string.Format(
                        CultureInfo.InvariantCulture,
                        SchemasResources.ExceptionEventTokenMissingClaimTemplate,
                        EventTokenClaimTypes.IssuedAt);
                throw new ArgumentException(exceptionMessage);
            }

            string serializedValue = value.ToString();
            if (!long.TryParse(serializedValue, out long issuedAt))
            {
                string exceptionMessage =
                    string.Format(
                        CultureInfo.InvariantCulture,
                        SchemasResources.ExceptionEventTokenMissingClaimTemplate,
                        EventTokenClaimTypes.IssuedAt);
                throw new ArgumentException(exceptionMessage);
            }
            IssuedAt = new UnixTime(issuedAt).ToUniversalTime();
        }

        private void ParseIssuer(JwtPayload payload)
        {
            if (null == payload)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            if (!payload.TryGetValue(EventTokenClaimTypes.Issuer, out object value) || null == value)
            {
                string exceptionMessage =
                    string.Format(
                        CultureInfo.InvariantCulture,
                        SchemasResources.ExceptionEventTokenMissingClaimTemplate,
                        EventTokenClaimTypes.Issuer);
                throw new ArgumentException(exceptionMessage);
            }

            string issuer = value as string;
            if (string.IsNullOrWhiteSpace(issuer))
            {
                string exceptionMessage =
                    string.Format(
                        CultureInfo.InvariantCulture,
                        SchemasResources.ExceptionEventTokenInvalidClaimValueTemplate,
                        EventTokenClaimTypes.Issuer,
                        value);
                throw new ArgumentException(exceptionMessage);
            }
            Issuer = issuer;
        }

        private void ParseNotBefore(JwtPayload payload)
        {
            if (null == payload)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            if (!payload.TryGetValue(EventTokenClaimTypes.NotBefore, out object value) || null == value)
            {
                return;
            }

            string serializedValue = value.ToString();
            if (!long.TryParse(serializedValue, out long notBefore))
            {
                string exceptionMessage =
                    string.Format(
                        CultureInfo.InvariantCulture,
                        SchemasResources.ExceptionEventTokenInvalidClaimValueTemplate,
                        EventTokenClaimTypes.NotBefore,
                        value);
                throw new ArgumentException(exceptionMessage);
            }

            NotBefore = new UnixTime(notBefore).ToUniversalTime();
        }

        private void ParseSubject(JwtPayload payload)
        {
            if (null == payload)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            if (!payload.TryGetValue(EventTokenClaimTypes.Subject, out object value) || null == value)
            {
                return;
            }

            string subject = value as string;
            if (null == subject)
            {
                string exceptionMessage =
                    string.Format(
                        CultureInfo.InvariantCulture,
                        SchemasResources.ExceptionEventTokenInvalidClaimValueTemplate,
                        EventTokenClaimTypes.Subject,
                        value);
                throw new ArgumentException(exceptionMessage);
            }

            Subject = subject;
        }

        private void ParseTransaction(JwtPayload payload)
        {
            if (null == payload)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            if (!payload.TryGetValue(EventTokenClaimTypes.Transaction, out object value) || null == value)
            {
                return;
            }

            string transaction = value as string;
            if (null == transaction)
            {
                string exceptionMessage =
                string.Format(
                    CultureInfo.InvariantCulture,
                    SchemasResources.ExceptionEventTokenInvalidClaimValueTemplate,
                    EventTokenClaimTypes.Transaction,
                    value);
                throw new ArgumentException(exceptionMessage);
            }

            Transaction = transaction;
        }

        public override string ToString()
        {
            JwtPayload payload = new JwtPayload
            {
                { EventTokenClaimTypes.Identifier, Identifier },

                { EventTokenClaimTypes.Issuer, Issuer }
            };

            if (Audience != null && Audience.Any())
            {
                string[] audience = Audience.ToArray();
                payload.Add(EventTokenClaimTypes.Audience, audience);
            }

            long issuedAt = new UnixTime(IssuedAt).EpochTimestamp;
            payload.Add(EventTokenClaimTypes.IssuedAt, issuedAt);

            if (NotBefore.HasValue)
            {
                long notBefore = new UnixTime(NotBefore.Value).EpochTimestamp;
                payload.Add(EventTokenClaimTypes.NotBefore, notBefore);
            }

            if (!string.IsNullOrWhiteSpace(Subject))
            {
                payload.Add(EventTokenClaimTypes.Subject, Subject);
            }

            if (Expiration.HasValue)
            {
                long expiration = new UnixTime(Expiration.Value).EpochTimestamp;
                payload.Add(EventTokenClaimTypes.Expiration, expiration);
            }

            payload.Add(EventTokenClaimTypes.Events, Events);

            if (!string.IsNullOrWhiteSpace(Transaction))
            {
                payload.Add(EventTokenClaimTypes.Transaction, Transaction);
            }

            SecurityToken token = new JwtSecurityToken(Header, payload);
            string result = EventToken.TokenSerializer.Value.WriteToken(token);
            return result;
        }
    }
}