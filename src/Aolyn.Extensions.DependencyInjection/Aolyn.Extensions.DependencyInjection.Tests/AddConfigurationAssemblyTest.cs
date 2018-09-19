using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Aolyn.Extensions.DependencyInjection.Tests
{
	public class AddConfigurationAssemblyTest
	{
		[Fact]
		public void Test1()
		{
			var services = new ServiceCollection();
			services.AddConfigurationAssembly<AddConfigurationAssemblyTest>();
			var serviceProvider = services.BuildServiceProvider();

			var printerService = serviceProvider.GetService<PrinterService>();
			Assert.Equal("Hello", printerService.Name);
		}

		[Service]
		public class PrinterService
		{
			public PrinterService(Configuration config)
			{
				Name = config.Name;
			}

			public string Name { get; set; }
		}

		[Service]
		public class Configuration
		{
			public string Name { get; set; } = "Hello";
		}
	}
}
