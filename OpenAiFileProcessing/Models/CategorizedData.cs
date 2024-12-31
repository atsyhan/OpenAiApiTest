namespace OpenAiFileProcessing.Models
{
	public class CategorizedData: CsvDataRow
	{
		public string Category { get; set; }
		public string Subcategory { get; set; }
	}
}
