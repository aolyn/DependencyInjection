# Aolyn.Extensions.DependencyInjection

This is a DependencyInjection utility tool used to auto register service to IServiceCollection.


##Quick start


###Register all services with ServiceAttribute


```csharp
public class AddConfigurationAssemblyTest
{
    [Fact]
    public void Test()
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
```

###Register all services in a configuration class
Add configuratin type, register all methods return type as services to IServiceCollection

```csharp
public class AddConfigurationTypeTest
{
    [Fact]
	public void Test()
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
```

