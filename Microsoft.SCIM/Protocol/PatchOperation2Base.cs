//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.SCIM
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    [DataContract]
    public abstract class PatchOperation2Base : IPatchOperation2Base
    {
        private const string Template = "{0} {1}";

        private OperationName name;
        private string operationName;

        private IPath path;
        [DataMember(Name = ProtocolAttributeNames.Path, Order = 1)]
        private string pathExpression;

        protected PatchOperation2Base()
        {
        }

        protected PatchOperation2Base(OperationName operationName, string pathExpression)
        {
            if (string.IsNullOrWhiteSpace(pathExpression))
            {
                throw new ArgumentNullException(nameof(pathExpression));
            }

            Name = operationName;
            Path = Microsoft.SCIM.Path.Create(pathExpression);
        }

        public OperationName Name
        {
            get => name;

            set
            {
                name = value;
                operationName = Enum.GetName(typeof(OperationName), value);
            }
        }

        // It's the value of 'op' parameter within the json of request body.
        [DataMember(Name = ProtocolAttributeNames.Patch2Operation, Order = 0)]
        public string OperationName
        {
            get => operationName;

            set
            {
                if (!Enum.TryParse(value, true, out name))
                {
                    throw new NotSupportedException();
                }

                operationName = value;
            }
        }

        public IPath Path
        {
            get
            {
                if (null == path && !string.IsNullOrWhiteSpace(pathExpression))
                {
                    path = Microsoft.SCIM.Path.Create(pathExpression);
                }

                return path;
            }

            set
            {
                pathExpression = value?.ToString();
                path = value;
            }
        }

        public override string ToString()
        {
            string result =
                string.Format(
                    CultureInfo.InvariantCulture,
                    PatchOperation2Base.Template,
                    operationName,
                    pathExpression);
            return result;
        }
    }
}
