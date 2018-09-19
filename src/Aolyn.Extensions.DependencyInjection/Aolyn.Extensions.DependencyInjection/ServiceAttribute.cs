using System;
using Microsoft.Extensions.DependencyInjection;

namespace Aolyn.Extensions.DependencyInjection
{
	/// <summary>
	/// mark a type as DI service
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class ServiceAttribute : Attribute
	{
		/// <summary>
		/// life time of service, default is Singleton
		/// </summary>
		public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Singleton;

		/// <summary>
		/// Service type, default is same as implementation type
		/// </summary>
		public Type ServiceType { get; set; }

		public ServiceAttribute() { }

		/// <summary>
		/// new instance with specific service lifetime
		/// </summary>
		/// <param name="lifetime"></param>
		public ServiceAttribute(ServiceLifetime lifetime)
		{
			Lifetime = lifetime;
		}

		/// <summary>
		/// new instance with specific service lifetime and service type
		/// </summary>
		/// <param name="lifetime">life time of service</param>
		/// <param name="serviceType">service type, current class marked is implement type</param>
		public ServiceAttribute(ServiceLifetime lifetime, Type serviceType)
		{
			Lifetime = lifetime;
			ServiceType = serviceType;
		}
	}
}