using System.Net.Http.Json;

public class RestClient
{
    static readonly HttpClient httpClient = new();
    public static readonly string Host = "https://2ch.hk/";
    public static async Task<Catalog> GetCatalog(string board)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"{Host}/{board}/catalog.json");
            response.EnsureSuccessStatusCode();
            Catalog responseBody = await response.Content.ReadFromJsonAsync<Catalog>();
            return responseBody;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }

        return null;
    }
    
    public static async Task<Posts[]> GetPosts(string board, int idThread)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"{Host}/{board}/res/{idThread}.json");
            response.EnsureSuccessStatusCode();
            Threads responseBody = await response.Content.ReadFromJsonAsync<Threads>();
            return responseBody.threads[0].posts;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }

        return null;
    }
}