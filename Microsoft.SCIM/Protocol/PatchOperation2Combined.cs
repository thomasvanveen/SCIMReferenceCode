//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.SCIM
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.Serialization;

    using Newtonsoft.Json;

    [DataContract]
    public sealed class PatchOperation2Combined : PatchOperation2Base
    {
        private const string Template = "{0}: [{1}]";

        [DataMember(Name = AttributeNames.Value, Order = 2)]
        private object values;


        public PatchOperation2Combined()
        {
        }

        public PatchOperation2Combined(OperationName operationName, string pathExpression)
            : base(operationName, pathExpression)
        {
        }

        public string Value
        {
            get
            {
                if (values == null)
                {
                    return null;
                }

                string result = JsonConvert.SerializeObject(values);
                return result;
            }

            set => values = value;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (Value == null)
            {
                if
                (
                    this?.Path?.AttributePath != null &&
                    Path.AttributePath.Contains(AttributeNames.Members, StringComparison.OrdinalIgnoreCase) &&
                    Name == SCIM.OperationName.Remove &&
                    Path?.SubAttributes?.Count == 1
                )
                {
                    Value = Path.SubAttributes.First().ComparisonValue;
                    IPath path = SCIM.Path.Create(AttributeNames.Members);
                    Path = path;
                }
            }
        }

        public override string ToString()
        {
            string allValues = string.Join(Environment.NewLine, Value);
            string operation = base.ToString();
            string result =
                string.Format(
                    CultureInfo.InvariantCulture,
                    PatchOperation2Combined.Template,
                    operation,
                    allValues);
            return result;
        }
    }
}