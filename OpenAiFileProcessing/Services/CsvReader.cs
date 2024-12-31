using System.Globalization;
using OpenAiFileProcessing.Models;

namespace OpenAiFileProcessing.Services
{
	public interface ICsvReader
	{
		Task<IEnumerable<CsvDataRow>> ReadCsvAsync(string filePath);
	}

	public class CsvReader : ICsvReader
	{
		public async Task<IEnumerable<CsvDataRow>> ReadCsvAsync(string filePath)
		{
			using var reader = new StreamReader(filePath);
			using var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture);
			var records = csv.GetRecordsAsync<CsvDataRow>();
			var list = new List<CsvDataRow>();
			await foreach (var record in records)
			{
				list.Add(record);
			}
			return list;
		}
	}
}
