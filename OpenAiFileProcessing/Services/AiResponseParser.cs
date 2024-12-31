namespace OpenAiFileProcessing.Services
{
	public class AiResponseParser
	{
		public static (string, string) ParseResponse(string response)
		{
			var split = response.Split(',');
			return (split[0], split[1]);
		}
	}
}
