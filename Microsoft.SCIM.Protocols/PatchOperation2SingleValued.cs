﻿//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.SCIM
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    [DataContract]
    public sealed class PatchOperation2SingleValued : PatchOperation2Base
    {
        private const string Template = "{0}: [{1}]";
        [DataMember(Name = AttributeNames.Value, Order = 2)]
        private string valueValue;

        public PatchOperation2SingleValued()
        {
            OnInitialization();
        }

        public PatchOperation2SingleValued(OperationName operationName, string pathExpression, string value)
            : base(operationName, pathExpression)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            valueValue = value;
        }

        public string Value => valueValue;

        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {
            OnInitialization();
        }

        private void OnInitialization()
        {
            valueValue = string.Empty;
        }

        public override string ToString()
        {
            string operation = base.ToString();
            string result =
                string.Format(
                    CultureInfo.InvariantCulture,
                    PatchOperation2SingleValued.Template,
                    operation,
                    valueValue);
            return result;
        }
    }
}
