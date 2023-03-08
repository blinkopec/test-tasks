using System.Net;
using RestSharp;
using Newtonsoft.Json;


class foo
{
    static string uri = $"https://reqres.in"; 
    public static void Main(string[] args)
    {
        var client = new RestClient(uri);
        var request = new RestRequest("/api/users?page=2");
        var response = client.Execute(request);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            string rawResponse = response.Content;
            Rootobject result = JsonConvert.DeserializeObject<Rootobject>(rawResponse);

            for (int i = 0; i < result.data.Length; i++)
            {
                if (result.data[i].id == 10) Console.Write(result.data[i].first_name + " " + result.data[i].last_name);
            }
        }
    }
}

public class Rootobject
{
    public int page { get; set; }
    public int per_page { get; set; }
    public int total { get; set; }
    public int total_pages { get; set; }
    public Datum[] data { get; set; }
    public Support support { get; set; }
}

public class Support
{
    public string url { get; set; }
    public string text { get; set; }
}

public class Datum
{
    public int id { get; set; }
    public string email { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string avatar { get; set; }
}
