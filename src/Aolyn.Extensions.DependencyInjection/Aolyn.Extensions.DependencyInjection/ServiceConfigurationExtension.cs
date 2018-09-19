using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Aolyn.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceConfigurationExtension
	{
		/// <summary>
		/// register all service of methods's return type, which methods with ServiceAttribute
		/// </summary>
		/// <typeparam name="T">configuration type, methods in type will be scan</typeparam>
		/// <param name="services">DI container reigister collection</param>
		/// <returns></returns>
		public static IServiceCollection AddConfigurationType<T>(this IServiceCollection services)
		{
			return AddConfigurationType(services, typeof(T));
		}

		/// <summary>
		/// register all service of methods's return type, which methods with ServiceAttribute
		/// </summary>
		/// <param name="services">DI container reigister collection</param>
		/// <param name="type">configuration type, methods in type will be scan</param>
		/// <returns></returns>
		public static IServiceCollection AddConfigurationType(this IServiceCollection services, Type type)
		{
			services.AddSingleton(type);

			var getServiceMethod = typeof(ServiceConfigurationExtension).GetMethod("GetServiceInstance");
			if (getServiceMethod == null) return services;

			var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public);
			foreach (var method in methods)
			{
				var attr = method.GetCustomAttributes<ServiceAttribute>().FirstOrDefault();
				if (attr == null)
					return services;

				var serviceParaExp = Expression.Parameter(typeof(IServiceProvider), "services");
				var configInst = Expression.Variable(type, "configInst");

				//var configInst = GetServiceInstance(type);
				var getConfigInstance = Expression.Call(getServiceMethod, serviceParaExp, Expression.Constant(type));
				var configInstance = Expression.Convert(getConfigInstance, type);
				var assignConfigInst = Expression.Assign(configInst, configInstance);

				var argumentList = new List<Expression>();
				foreach (var parameter in method.GetParameters())
				{
					var parax = Expression.Call(getServiceMethod, serviceParaExp,
						Expression.Constant(parameter.ParameterType));
					var tparax = Expression.Convert(parax, parameter.ParameterType);
					argumentList.Add(tparax);
				}

				//configInst.Xxx(p1, p2...);
				var getBeanExp = Expression.Call(configInst, method, argumentList.ToArray());

				var labelTarget = Expression.Label(typeof(object));
				var returnExp = Expression.Return(labelTarget, getBeanExp);
				var lbl = Expression.Label(labelTarget, Expression.Constant(null));

				var blocks = Expression.Block(new[] { configInst }, assignConfigInst, returnExp, lbl);
				var lam = Expression.Lambda<Func<IServiceProvider, object>>(blocks, serviceParaExp);
				var func = lam.Compile();

				var desc = new ServiceDescriptor(method.ReturnType, func, attr.Lifetime);
				services.Add(desc);
			}
			return services;
		}

		[Obsolete("Only for emit use")]
		public static object GetServiceInstance(IServiceProvider serviceProvider, Type type)
		{
			return serviceProvider.GetService(type);
		}

		/// <summary>
		/// register all service type with ConfigurationAttribute or ServiceAttribute in assembly of T
		/// </summary>
		/// <typeparam name="T">assemlby of T will be scan</typeparam>
		/// <param name="services">service to add to</param>
		/// <returns></returns>
		public static IServiceCollection AddConfigurationAssembly<T>(this IServiceCollection services)
		{
			return services.AddConfigurationAssembly(typeof(T).Assembly);
		}

		/// <summary>
		/// register all service type with ConfigurationAttribute or ServiceAttribute in assembly
		/// </summary>
		/// <param name="assembly">assembly to scan</param>
		/// <param name="services">service to add to</param>
		/// <returns></returns>
		public static IServiceCollection AddConfigurationAssembly(this IServiceCollection services, Assembly assembly)
		{
			var types = assembly.GetTypes();
			foreach (var type in types)
			{
				//var existDescriptor = services.FirstOrDefault(it => it.ServiceType == type);
				//if (existDescriptor != null)
				//{
				//	continue;
				//}

				var attr = type.GetCustomAttributes<ConfigurationAttribute>().FirstOrDefault();
				if (attr != null)
				{
					AddConfigurationType(services, type);
					continue;
				}

				var serviceAttribute = type.GetCustomAttributes<ServiceAttribute>().FirstOrDefault();
				if (serviceAttribute == null)
					continue;

				var desc = new ServiceDescriptor(serviceAttribute.ServiceType ?? type, type, serviceAttribute.Lifetime);
				services.Add(desc);
			}

			return services;
		}
	}
}