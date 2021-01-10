//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.SCIM
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Microsoft.SCIM.Protocols;

    public sealed class SchematizedJsonDeserializingFactory : SchematizedJsonDeserializingFactoryBase
    {
        private ISchematizedJsonDeserializingFactory<PatchRequest2> patchSerializer;

        public override IReadOnlyCollection<IExtension> Extensions
        {
            get;
            set;
        }
        public override IResourceJsonDeserializingFactory<GroupBase> GroupDeserializationBehavior
        {
            get;
            set;
        }

        public override ISchematizedJsonDeserializingFactory<PatchRequest2> PatchRequest2DeserializationBehavior
        {
            get
            {
                ISchematizedJsonDeserializingFactory<PatchRequest2> result =
                    LazyInitializer.EnsureInitialized<ISchematizedJsonDeserializingFactory<PatchRequest2>>(
                        ref patchSerializer,
                        SchematizedJsonDeserializingFactory.InitializePatchSerializer);
                return result;
            }

            set => patchSerializer = value;
        }

        public override IResourceJsonDeserializingFactory<Core2UserBase> UserDeserializationBehavior
        {
            get;
            set;
        }

        private Resource CreateGroup(
            IReadOnlyCollection<string> schemaIdentifiers,
            IReadOnlyDictionary<string, object> json)
        {
            if (null == schemaIdentifiers)
            {
                throw new ArgumentNullException(nameof(schemaIdentifiers));
            }

            if (null == json)
            {
                throw new ArgumentNullException(nameof(json));
            }

            if (GroupDeserializationBehavior != null)
            {
                Resource group = GroupDeserializationBehavior.Create(json);
                return group;
            }

            if (schemaIdentifiers.Count != 1)
            {
                throw new ArgumentException(ProtocolResources.ExceptionInvalidResource);
            }

            Resource result = new Core2GroupJsonDeserializingFactory().Create(json);
            return result;
        }

        private Schematized CreatePatchRequest(IReadOnlyDictionary<string, object> json)
        {
            if (null == json)
            {
                throw new ArgumentNullException(nameof(json));
            }

            if (TryCreatePatchRequest2Legacy(json, out Schematized result))
            {
                return result;
            }

            if (SchematizedJsonDeserializingFactory.TryCreatePatchRequest2Compliant(json, out result))
            {
                return result;
            }

            throw new InvalidOperationException(
                ProtocolResources.ExceptionInvalidRequest);
        }

        private Resource CreateUser(
            IReadOnlyCollection<string> schemaIdentifiers,
            IReadOnlyDictionary<string, object> json)
        {
            if (null == schemaIdentifiers)
            {
                throw new ArgumentNullException(nameof(schemaIdentifiers));
            }

            if (null == json)
            {
                throw new ArgumentNullException(nameof(json));
            }

            if (UserDeserializationBehavior != null)
            {
                Resource result = UserDeserializationBehavior.Create(json);
                return result;
            }

            if
            (
                    schemaIdentifiers
                    .SingleOrDefault(
                        (string item) =>
                            item.Equals(
                                SchemaIdentifiers.Core2EnterpriseUser,
                                StringComparison.OrdinalIgnoreCase)) != null
            )
            {
                Resource result = new Core2EnterpriseUserJsonDeserializingFactory().Create(json);
                return result;
            }
            else
            {
                if (schemaIdentifiers.Count != 1)
                {
                    throw new ArgumentException(ProtocolResources.ExceptionInvalidResource);
                }

                Resource result = new Core2UserJsonDeserializingFactory().Create(json);
                return result;
            }
        }

        public override Schematized Create(IReadOnlyDictionary<string, object> json)
        {
            if (null == json)
            {
                return null;
            }

            IReadOnlyDictionary<string, object> normalizedJson = Normalize(json);
            if (!normalizedJson.TryGetValue(AttributeNames.Schemas, out object value))
            {
                throw new ArgumentException(ProtocolResources.ExceptionUnidentifiableSchema);
            }

            IReadOnlyCollection<string> schemaIdentifiers;

            switch (value)
            {
                case IEnumerable schemas:
                    schemaIdentifiers = schemas.ToCollection<string>();
                    break;
                default:
                    throw new ArgumentException(
                        ProtocolResources.ExceptionUnidentifiableSchema);
            }

#pragma warning disable IDE0018 // Inline variable declaration
            Schematized result;
#pragma warning restore IDE0018 // Inline variable declaration
            if (TryCreateResourceFrom(normalizedJson, schemaIdentifiers, out result))
            {
                return result;
            }

            if (TryCreateProtocolObjectFrom(normalizedJson, schemaIdentifiers, out result))
            {
                return result;
            }

            if (TryCreateExtensionObjectFrom(normalizedJson, schemaIdentifiers, out result))
            {
                return result;
            }

            string allSchemaIdentifiers = string.Join(Environment.NewLine, schemaIdentifiers);
            throw new NotSupportedException(allSchemaIdentifiers);
        }

        private static ISchematizedJsonDeserializingFactory<PatchRequest2> InitializePatchSerializer()
        {
            ISchematizedJsonDeserializingFactory<PatchRequest2> result = new PatchRequest2JsonDeserializingFactory();
            return result;
        }

        private bool TryCreateExtensionObjectFrom(
            IReadOnlyDictionary<string, object> json,
            IReadOnlyCollection<string> schemaIdentifiers,
            out Schematized schematized)
        {
            schematized = null;

            if (null == json)
            {
                throw new ArgumentNullException(nameof(json));
            }

            if (null == schemaIdentifiers)
            {
                throw new ArgumentNullException(nameof(schemaIdentifiers));
            }

            if (null == Extensions)
            {
                return false;
            }

            if (Extensions.TryMatch(schemaIdentifiers, out IExtension matchingExtension))
            {
                schematized = matchingExtension.JsonDeserializingFactory(json);
                return true;
            }

            return false;
        }

        private static bool TryCreatePatchRequest2Compliant(
            IReadOnlyDictionary<string, object> json,
            out Schematized schematized)
        {
            schematized = null;
            try
            {
                ISchematizedJsonDeserializingFactory<PatchRequest2> deserializer =
                    new PatchRequest2JsonDeserializingFactory();
                schematized = deserializer.Create(json);
            }
            catch (OutOfMemoryException)
            {
                throw;
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (ThreadInterruptedException)
            {
                throw;
            }
            catch (StackOverflowException)
            {
                throw;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
            {
                return false;
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return true;
        }

        private bool TryCreatePatchRequest2Legacy(
            IReadOnlyDictionary<string, object> json,
            out Schematized schematized)
        {
            schematized = null;
            try
            {
                ISchematizedJsonDeserializingFactory<PatchRequest2> deserializer =
                    PatchRequest2DeserializationBehavior ?? new PatchRequest2JsonDeserializingFactory();
                schematized = deserializer.Create(json);
            }
            catch (OutOfMemoryException)
            {
                throw;
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (ThreadInterruptedException)
            {
                throw;
            }
            catch (StackOverflowException)
            {
                throw;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
            {
                return false;
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return true;
        }

        private bool TryCreateProtocolObjectFrom(
            IReadOnlyDictionary<string, object> json,
            IReadOnlyCollection<string> schemaIdentifiers,
            out Schematized schematized)
        {
            schematized = null;

            if (null == json)
            {
                throw new ArgumentNullException(nameof(json));
            }

            if (null == schemaIdentifiers)
            {
                throw new ArgumentNullException(nameof(schemaIdentifiers));
            }

            if (schemaIdentifiers.Count != 1)
            {
                return false;
            }

            if
            (
                schemaIdentifiers
                .SingleOrDefault(
                    (string item) =>
                        item.Equals(
                            ProtocolSchemaIdentifiers.Version2PatchOperation,
                            StringComparison.OrdinalIgnoreCase)) != null
            )
            {
                schematized = CreatePatchRequest(json);
                return true;
            }

            if
            (
                schemaIdentifiers
                .SingleOrDefault(
                    (string item) =>
                        item.Equals(
                            ProtocolSchemaIdentifiers.Version2Error,
                            StringComparison.OrdinalIgnoreCase)) != null
            )
            {
                schematized = new ErrorResponseJsonDeserializingFactory().Create(json);
                return true;
            }

            return false;
        }

        private bool TryCreateResourceFrom(
            IReadOnlyDictionary<string, object> json,
            IReadOnlyCollection<string> schemaIdentifiers,
            out Schematized schematized)
        {
            schematized = null;

            if (null == json)
            {
                throw new ArgumentNullException(nameof(json));
            }

            if (null == schemaIdentifiers)
            {
                throw new ArgumentNullException(nameof(schemaIdentifiers));
            }

            if
            (
                schemaIdentifiers
                .SingleOrDefault(
                    (string item) =>
                        item.Equals(
                            SchemaIdentifiers.Core2User,
                            StringComparison.OrdinalIgnoreCase)) != null
            )
            {
                schematized = CreateUser(schemaIdentifiers, json);
                return true;
            }

            if
            (
                schemaIdentifiers
                .SingleOrDefault(
                    (string item) =>
                        item.Equals(
                            SchemaIdentifiers.Core2Group,
                            StringComparison.OrdinalIgnoreCase)) != null
            )
            {
                schematized = CreateGroup(schemaIdentifiers, json);
                return true;
            }

            return false;
        }
    }
}