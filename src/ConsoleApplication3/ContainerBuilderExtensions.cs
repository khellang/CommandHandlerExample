using System;
using System.Linq;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Features.Scanning;

namespace ConsoleApplication3
{
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// A helper method to register closed types of a generic type with service name.
        /// </summary>
        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationData> AsClosedTypesOf<TLimit, TActivatorData, TRegistrationData>(
            this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationData> registration, Type openGenericType, string serviceName)
            where TActivatorData : ScanningActivatorData
        {
            return registration.As(t => t.GetInterfaces().Where(i => i.IsClosedTypeOf(openGenericType)).Select(x => new KeyedService(serviceName, x)));
        }
    }
}