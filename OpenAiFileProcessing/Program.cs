using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using OpenAiFileProcessing.Models;
using OpenAiFileProcessing.Services;
using System.Collections.Concurrent;

namespace OpenAiFileProcessing
{
	public class Program
	{
		static async Task Main(string[] args)
		{
			var host = Host.CreateDefaultBuilder(args)
			.ConfigureAppConfiguration((context, config) =>
			{
				config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
					  .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true)
					  .AddEnvironmentVariables();
			})
			.ConfigureServices((context, services) =>
			{
				services.Configure<AppSettings>(context.Configuration);

				services.AddTransient<IMyService, MyService>()
				.AddTransient<ICsvReader, CsvReader>()
				.AddTransient<IPromptService, PromptService>()
				.AddSingleton<ChatClient>(provider =>
				{
					var settings = provider.GetRequiredService<IOptions<AppSettings>>().Value;
					return new ChatClient(model: settings.OpenAIModel, apiKey: settings.OpenAIKey);
				})
				.AddSingleton(new ConcurrentDictionary<int, CategorizedData>());

			})
			.ConfigureLogging((context, logging) =>
			{
				logging.ClearProviders();
				logging.AddConsole();
			})
			.Build();

			var myService = host.Services.GetRequiredService<IMyService>();
			await myService.RunAsync();
		}


	}
}
