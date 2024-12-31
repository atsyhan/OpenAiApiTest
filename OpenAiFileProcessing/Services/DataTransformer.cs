using OpenAiFileProcessing.Models;

namespace OpenAiFileProcessing.Services
{
	public class DataTransformer
	{
		public static CategorizedData ToCategorizedData(CsvDataRow data, string category, string subcategory)
		{
			var categorizedData = new CategorizedData
			{
				Id = data.Id,
				Date = data.Date,
				Amount = data.Amount,
				Vendor = data.Vendor,
				Region = data.Region,
				Notes = data.Notes,
				Category = category,
				Subcategory = subcategory
			};
			return categorizedData;
		}
	}
}
