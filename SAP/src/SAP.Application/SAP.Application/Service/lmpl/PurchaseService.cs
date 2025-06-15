
using SAP.Application.Model.Items;
using SAP.Application.Model.Purchase;
using System.Text.Json;

namespace SAP.Application.Service.lmpl;

public class PurchaseService : IPurchaseService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ISAPSessionStorage _sessionStorage;
    private readonly string _baseUrl = "https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/";
    private readonly string _baseUrl2 = "https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/PurchaseInvoices";

    public PurchaseService(IHttpClientFactory httpClientFactory, ISAPSessionStorage sessionStorage)
    {
        _httpClientFactory = httpClientFactory;
        _sessionStorage = sessionStorage;
    }

    public async Task<List<PurchaseResponseModel>> GetAllPurchaseBy()
    {
        var session = _sessionStorage.Session;
        if (session == null || string.IsNullOrEmpty(session.B1Session) || string.IsNullOrEmpty(session.RouteId))
            throw new Exception("SAP session is not initialized.");

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={session.B1Session}; ROUTEID={session.RouteId}");

        var requestUrl = $"{_baseUrl2}";
        var response = await client.GetAsync(requestUrl);
        var responseContent = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"StatusCode: {response.StatusCode}");
        Console.WriteLine($"Response: {responseContent}");

        if (response.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize<PurchaseListWrapper>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result?.Value ?? new List<PurchaseResponseModel>();
        }

        throw new InvalidOperationException($"Failed to get employees. Status: {response.StatusCode}. Response: {responseContent}");
    }

    public async Task<PurchaseResponseModel> GetPurchaseById(int id)
    {

        var session = _sessionStorage.Session;
        if (session == null || string.IsNullOrEmpty(session.B1Session) || string.IsNullOrEmpty(session.B1Session))
            throw new Exception("SAP session is not initialized.");

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={session.B1Session}; ROUTEID={session.RouteId}");

        var requestUrl = $"{_baseUrl2}({id})";
        var response = await client.GetAsync(requestUrl);
        var responseContent = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"StatusCode: {response.StatusCode}");
        Console.WriteLine($"Response: {responseContent}");

        if (response.IsSuccessStatusCode)
        {
            var employee = JsonSerializer.Deserialize<PurchaseResponseModel>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return employee!;
        }
        throw new InvalidOperationException($"Failed to get Purchase. Status: {response.StatusCode}. Response: {responseContent}");
    }
}
