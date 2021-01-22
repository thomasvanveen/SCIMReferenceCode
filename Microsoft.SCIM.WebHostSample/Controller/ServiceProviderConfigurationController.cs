// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Microsoft.AspNetCore.Mvc;

namespace Microsoft.SCIM.Controllers
{
    [Route(ServiceConstants.RouteServiceConfiguration)]
    [ApiController]
    public sealed class ServiceProviderConfigurationController : ControllerTemplate
    {
        public ServiceProviderConfigurationController(IProvider provider, IMonitor monitor)
            : base(provider, monitor)
        {
        }

        [HttpGet]
        public ServiceConfigurationBase Get()
        {
            string correlationIdentifier = null;

            try
            {
                HttpRequestMessage request = ConvertRequest();
                if (!request.TryGetRequestIdentifier(out correlationIdentifier))
                {
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
                }

                IProvider provider = this.provider;
                if (null == provider)
                {
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
                }

                ServiceConfigurationBase result = provider.Configuration;
                return result;
            }
            catch (ArgumentException argumentException)
            {
                if (TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            argumentException,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ServiceProviderConfigurationControllerGetArgumentException);
                    monitor.Report(notification);
                }

                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            catch (NotImplementedException notImplementedException)
            {
                if (TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            notImplementedException,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ServiceProviderConfigurationControllerGetNotImplementedException);
                    monitor.Report(notification);
                }

                throw new HttpResponseException(HttpStatusCode.NotImplemented);
            }
            catch (NotSupportedException notSupportedException)
            {
                if (TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            notSupportedException,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ServiceProviderConfigurationControllerGetNotSupportedException);
                    monitor.Report(notification);
                }

                throw new HttpResponseException(HttpStatusCode.NotImplemented);
            }
            catch (Exception exception)
            {
                if (TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            exception,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ServiceProviderConfigurationControllerGetException);
                    monitor.Report(notification);
                }

                throw;
            }
        }
    }
}