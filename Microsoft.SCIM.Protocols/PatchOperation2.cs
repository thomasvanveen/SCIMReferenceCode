//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.SCIM
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.Serialization;

    using Microsoft.SCIM.Protocols;

    [DataContract]
    public sealed class PatchOperation2 : PatchOperation2Base
    {
        private const string Template = "{0}: [{1}]";

        [DataMember(Name = AttributeNames.Value, Order = 2)]
        private List<OperationValue> values;
        private IReadOnlyCollection<OperationValue> valuesWrapper;

        public PatchOperation2()
        {
            OnInitialization();
            OnInitialized();
        }

        public PatchOperation2(OperationName operationName, string pathExpression)
            : base(operationName, pathExpression)
        {
            OnInitialization();
            OnInitialized();
        }

        public IReadOnlyCollection<OperationValue> Value => valuesWrapper;

        public void AddValue(OperationValue value)
        {
            if (null == value)
            {
                throw new ArgumentNullException(nameof(value));
            }

            values.Add(value);
        }

        public static PatchOperation2 Create(OperationName operationName, string pathExpression, string value)
        {
            if (string.IsNullOrWhiteSpace(pathExpression))
            {
                throw new ArgumentNullException(nameof(pathExpression));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            OperationValue operationValue = new OperationValue
            {
                Value = value
            };

            PatchOperation2 result = new PatchOperation2(operationName, pathExpression);
            result.AddValue(operationValue);

            return result;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            OnInitialized();
        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {
            OnInitialization();
        }

        private void OnInitialization()
        {
            values = new List<OperationValue>();
        }

        private void OnInitialized()
        {
            switch (values)
            {
                case List<OperationValue> valueList:
                    valuesWrapper = valueList.AsReadOnly();
                    break;
                default:
                    throw new NotSupportedException(ProtocolResources.ExceptionInvalidValue);
            }
        }

        public override string ToString()
        {
            string allValues = string.Join(Environment.NewLine, Value);
            string operation = base.ToString();
            string result =
                string.Format(
                    CultureInfo.InvariantCulture,
                    PatchOperation2.Template,
                    operation,
                    allValues);
            return result;
        }
    }
}