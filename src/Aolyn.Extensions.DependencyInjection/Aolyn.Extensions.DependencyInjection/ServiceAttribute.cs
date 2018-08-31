using System;
using Microsoft.Extensions.DependencyInjection;

namespace Aolyn.Extensions.DependencyInjection
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class ServiceAttribute : Attribute
	{
		public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Singleton;

		/// <summary>
		/// Service type, default is same as implementation type
		/// </summary>
		public Type ServiceType { get; set; }

		public ServiceAttribute() { }

		public ServiceAttribute(ServiceLifetime lifetime)
		{
			Lifetime = lifetime;
		}

		public ServiceAttribute(ServiceLifetime lifetime, Type serviceType)
		{
			Lifetime = lifetime;
			ServiceType = serviceType;
		}
	}
}