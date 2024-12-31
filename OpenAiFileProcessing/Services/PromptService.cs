using OpenAiFileProcessing.Models;

namespace OpenAiFileProcessing.Services
{
	public interface IPromptService
	{
		string PreparePrompt(CsvDataRow data);
	}

	public class PromptService: IPromptService
	{
		private static string prePrompt = "Based on knowledge of real-world financial transactions, you need to categorize the provided information into one of the following categories and subcategories from this JSON: ";

		private static string format = "{\"categories\":[{\"name\":\"Operational Expenses\",\"subcategories\":[{\"name\":\"Office Supplies and Equipment\",\"description\":\"Purchases for daily office operations, such as stationery, furniture, and electronics.\"},{\"name\":\"Salaries and Benefits\",\"description\":\"Compensation for employees, including executive salaries, bonuses, and benefits.\"},{\"name\":\"Travel and Entertainment\",\"description\":\"Costs associated with business travel and entertainment for employees or clients.\"}]},{\"name\":\"Investment-Related Expenses\",\"subcategories\":[{\"name\":\"Due Diligence\",\"description\":\"Costs related to researching potential investments, such as market studies and feasibility analysis.\"},{\"name\":\"Legal and Compliance Fees\",\"description\":\"Expenses for legal support, regulatory filings, and compliance-related activities.\"},{\"name\":\"Transaction Costs\",\"description\":\"Fees and costs incurred during mergers, acquisitions, or portfolio transactions.\"}]},{\"name\":\"Fund Management Expenses\",\"subcategories\":[{\"name\":\"Fundraising Costs\",\"description\":\"Marketing, travel, and event costs to raise new funds or attract investors.\"},{\"name\":\"Management Fees\",\"description\":\"Payments to fund managers or external advisors for their services.\"}]},{\"name\":\"Portfolio Company Support\",\"subcategories\":[{\"name\":\"Operational Improvement\",\"description\":\"Investments in improving the operations of portfolio companies, such as training programs.\"},{\"name\":\"Strategic Consulting\",\"description\":\"Costs for strategic advice or growth planning for portfolio companies.\"}]},{\"name\":\"Marketing and Business Development\",\"subcategories\":[{\"name\":\"Branding and Public Relations\",\"description\":\"Efforts to improve the firm’s image through PR campaigns, advertising, or branding activities.\"},{\"name\":\"Client Engagement\",\"description\":\"Hosting dinners, events, or meetings with clients and investors.\"}]},{\"name\":\"Debt and Financing Costs\",\"subcategories\":[{\"name\":\"Interest Payments\",\"description\":\"Payments on loans or debt servicing related to investments or operations.\"},{\"name\":\"Bank Fees\",\"description\":\"Costs such as account maintenance fees or transaction fees charged by banks.\"}]},{\"name\":\"Miscellaneous Expenses\",\"subcategories\":[{\"name\":\"Professional Services\",\"description\":\"Payments for external professional services like auditing, accounting, or consulting.\"},{\"name\":\"Insurance\",\"description\":\"Costs for policies like liability insurance, property insurance, or employee coverage.\"}]}]}";

		private static string postPrompt = "Please provide the Category and Subcategory for the data below in a single-line response, with comma-separated Category and Subcategory names from the format above (Example: Operational Expenses, Office Supplies and Equipment).";

		public string PreparePrompt(CsvDataRow data)
		{
			return prePrompt + "\n" + format + "\n" + postPrompt + "\n" +
				   "Id: " + data.Id + ", " +
				   "Date: " + data.Date + ", " +
				   "Amount: " + data.Amount + ", " +
				   "Vendor: " + data.Vendor + ", " +
				   "Region: " + data.Region + ", " +
				   "Notes: " + data.Notes;
		}
	}
}
