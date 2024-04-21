namespace Infrastructure.Seed;

public class Utils
{
    internal static byte[] DownloadImageFromUrl(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    byte[] imageBytes = response.Content.ReadAsByteArrayAsync().Result;
                    return imageBytes;
                }
                else
                {
                    Console.WriteLine($"Failed to download image. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return new byte[0];
        }
    }
}