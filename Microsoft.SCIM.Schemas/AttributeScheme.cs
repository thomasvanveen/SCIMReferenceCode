//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.SCIM
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public sealed class AttributeScheme
    {
        private AttributeDataType dataType;
        private string dataTypeValue;
        private Mutability mutability;
        private string mutabilityValue;
        private Returned returned;
        private string returnedValue;
        private Uniqueness uniqueness;
        private string uniquenessValue;

        public AttributeScheme()
        {
        }

        public AttributeScheme(string name, AttributeDataType type, bool plural)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            DataType = type;
            Plural = plural;
            Mutability = Mutability.readWrite;
            Returned = Returned.@default;
            Uniqueness = Uniqueness.none;
        }

        [DataMember(Name = AttributeNames.CaseExact)]
        public bool CaseExact
        {
            get;
            set;
        }

        public AttributeDataType DataType
        {
            get => dataType;

            set
            {
                dataTypeValue = Enum.GetName(typeof(AttributeDataType), value);
                dataType = value;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called upon serialization")]
        [DataMember(Name = AttributeNames.Type)]
#pragma warning disable IDE0051 // Remove unused private members
        private string DataTypeValue
#pragma warning restore IDE0051 // Remove unused private members
        {
            get => dataTypeValue;

            set
            {
                dataType = (AttributeDataType)Enum.Parse(typeof(AttributeDataType), value);
                dataTypeValue = value;
            }
        }

        [DataMember(Name = AttributeNames.Description)]
        public string Description
        {
            get;
            set;
        }

        public Mutability Mutability
        {
            get => mutability;

            set
            {
                mutabilityValue = Enum.GetName(typeof(Mutability), value);
                mutability = value;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called upon serialization")]
        [DataMember(Name = AttributeNames.Mutability)]
#pragma warning disable IDE0051 // Remove unused private members
        private string MutabilityValue
#pragma warning restore IDE0051 // Remove unused private members
        {
            get => mutabilityValue;

            set
            {
                mutability = (Mutability)Enum.Parse(typeof(Mutability), value);
                mutabilityValue = value;
            }
        }

        [DataMember(Name = AttributeNames.Name)]
        public string Name
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Plural)]
        public bool Plural
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Required)]
        public bool Required
        {
            get;
            set;
        }

        public Returned Returned
        {
            get => returned;

            set
            {
                returnedValue = Enum.GetName(typeof(Returned), value);
                returned = value;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called upon serialization")]
        [DataMember(Name = AttributeNames.Returned)]
#pragma warning disable IDE0051 // Remove unused private members
        private string ReturnedValue
#pragma warning restore IDE0051 // Remove unused private members
        {
            get => returnedValue;

            set
            {
                returned = (Returned)Enum.Parse(typeof(Returned), value);
                returnedValue = value;
            }
        }

        public Uniqueness Uniqueness
        {
            get => uniqueness;

            set
            {
                uniquenessValue = Enum.GetName(typeof(Uniqueness), value);
                uniqueness = value;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called upon serialization")]
        [DataMember(Name = AttributeNames.Uniqueness)]
#pragma warning disable IDE0051 // Remove unused private members
        private string UniquenessValue
#pragma warning restore IDE0051 // Remove unused private members
        {
            get => uniquenessValue;

            set
            {
                uniqueness = (Uniqueness)Enum.Parse(typeof(Uniqueness), value);
                uniquenessValue = value;
            }
        }
    }
}