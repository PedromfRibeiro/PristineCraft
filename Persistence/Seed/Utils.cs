using System.Net;

namespace Persistence.Seed;

public class Utils
{
	internal static byte[] DownloadImageFromUrl(string url)
	{
		byte[] imageBytes;
		using (WebClient webClient = new())
		{
			imageBytes = webClient.DownloadData(url);
		}
		return imageBytes;
	}
}