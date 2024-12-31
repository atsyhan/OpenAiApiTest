using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using OpenAiFileProcessing.Models;
using System.Collections.Concurrent;

namespace OpenAiFileProcessing.Services
{

	public interface IMyService
	{
		Task RunAsync();
	}

	public class MyService: IMyService
	{
		private readonly ChatClient _client;
		private readonly ICsvReader _csvReader;
		private readonly IPromptService _promptService;
		private readonly ConcurrentDictionary<int, CategorizedData> _cache;
		private readonly AppSettings _appSettings;
		private readonly ILogger<MyService> _logger;

		public MyService(ChatClient client,
			ICsvReader csvReader,
			IPromptService promptService,
			ConcurrentDictionary<int, CategorizedData> cache,
			IOptions<AppSettings> options,
			ILogger<MyService> logger)
		{
			_client = client;
			_csvReader = csvReader;
			_promptService = promptService;
			_cache = cache;
			_appSettings = options.Value;
			_logger = logger;
		}

		public async Task RunAsync()
		{
			string? response;
			do
			{
				await ProcessFile();
				DisplayData();

				Console.WriteLine("Do you want to repeat? y/n");
				response = Console.ReadLine()?.ToLower();
			} while (response != "n");

			Console.WriteLine("Exiting. Goodbye!");
		}

		private async Task ProcessFile()
		{
			var data = await _csvReader.ReadCsvAsync(Path.Combine(AppContext.BaseDirectory, _appSettings.CsvFilePath));

			// Limit threads(In general ThreadPool will controll amount of threads with this approach)
			int _maxThreads = 4;
			var semaphore = new SemaphoreSlim(_maxThreads);
			var tasks = new List<Task>();
			foreach (var dataRow in data)
			{
				if (_cache.ContainsKey(int.Parse(dataRow.Id)))
				{
					continue;
				}

				await semaphore.WaitAsync();

				// do not await, just stack tasks
				tasks.Add(ProcessDataRow(dataRow));

				semaphore.Release();
			}

			await Task.WhenAll(tasks);
		}

		private async Task ProcessDataRow(CsvDataRow dataRow)
		{
			ChatCompletion completion = await _client.CompleteChatAsync(_promptService.PreparePrompt(dataRow));
			var (category, subcategory) = AiResponseParser.ParseResponse(completion.Content[0].Text);
			_cache.TryAdd(int.Parse(dataRow.Id), DataTransformer.ToCategorizedData(dataRow, category, subcategory));
		}

		private void DisplayData()
		{
			foreach (var item in _cache)
			{
				Console.WriteLine($"Id: {item.Value.Id}, Date: {item.Value.Date}, Amount: {item.Value.Amount}, Vendor: {item.Value.Vendor}, Region: {item.Value.Region}, Notes: {item.Value.Notes}, Category: {item.Value.Category}, Subcategory: {item.Value.Subcategory}");
			}
		}
	}
}
