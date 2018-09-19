using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Aolyn.Extensions.DependencyInjection.Tests
{
	public class AddConfigurationTypeTest
	{
		[Fact]
		public void Test1()
		{
			var services = new ServiceCollection();
			services.AddConfigurationType<DiConfiguration>();
			var serviceProvider = services.BuildServiceProvider();

			var emailService = serviceProvider.GetService<EmailService>();
			Assert.Equal("Hello", emailService.Name);
		}

		public class DiConfiguration
		{
			[Service]
			public EmailService GetEmailService(IServiceProvider serviceProvider, Configuration configuration)
			{
				return new EmailService
				{
					Name = configuration.Name,
				};
			}

			[Service]
			public Configuration GetConfiguration()
			{
				return new Configuration();
			}
		}

		public class EmailService
		{
			public string Name { get; set; }
		}

		public class Configuration
		{
			public string Name { get; set; } = "Hello";
		}
	}
}
