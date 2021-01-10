//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.SCIM
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;

    [DataContract]
    public sealed class ErrorResponse : Schematized
    {
        private ErrorType errorType;

        [DataMember(Name = ProtocolAttributeNames.ErrorType)]
        internal string errorTypeValue;

        private Response response;

        public ErrorResponse()
        {
            Initialize();
            AddSchema(ProtocolSchemaIdentifiers.Version2Error);
        }

        [DataMember(Name = ProtocolAttributeNames.Detail)]
        public string Detail
        {
            get;
            set;
        }

        public ErrorType ErrorType
        {
            get => errorType;

            set
            {
                errorType = value;
                errorTypeValue = Enum.GetName(typeof(ErrorType), value);
            }
        }

        public HttpStatusCode Status
        {
            get => response.Status;

            set => response.Status = value;
        }

        private void Initialize()
        {
            response = new Response();
        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {
            Initialize();
        }
    }
}