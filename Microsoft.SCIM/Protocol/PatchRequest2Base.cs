//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.SCIM
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public abstract class PatchRequest2Base<TOperation> : PatchRequestBase
        where TOperation : PatchOperation2Base
    {
        [DataMember(Name = ProtocolAttributeNames.Operations, Order = 2)]
        private List<TOperation> operationsValue;
        private IReadOnlyCollection<TOperation> operationsWrapper;

        protected PatchRequest2Base()
        {
            OnInitialization();
            OnInitialized();
            AddSchema(ProtocolSchemaIdentifiers.Version2PatchOperation);
        }

        protected PatchRequest2Base(IReadOnlyCollection<TOperation> operations)
            : this()
        {
            operationsValue.AddRange(operations);
        }

        public IReadOnlyCollection<TOperation> Operations => operationsWrapper;

        public void AddOperation(TOperation operation)
        {
            if (null == operation)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            operationsValue.Add(operation);
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
            operationsValue = new List<TOperation>();
        }

        private void OnInitialized()
        {
            operationsWrapper = operationsValue.AsReadOnly();
        }
    }
}
