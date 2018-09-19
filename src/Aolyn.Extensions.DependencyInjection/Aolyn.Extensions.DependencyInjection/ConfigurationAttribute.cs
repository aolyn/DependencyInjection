using System;

namespace Aolyn.Extensions.DependencyInjection
{
	/// <inheritdoc />
	/// <summary>
	/// mark class as Configuration class, all method with ServiceAttribute will add to DI container
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class ConfigurationAttribute : Attribute
	{
	}
}