﻿
using SAP.Application.Model.BussinesPartner;
using SAP.Application.Model.Items;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace SAP.Application.Service.lmpl;

public class BussinesPartnerService : IBussinesPartnerService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ISAPSessionStorage _sessionStorage;
    private readonly string _baseUrl = "https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/";
    private readonly string _baseUrl2 = "https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/BusinessPartners";

    public BussinesPartnerService(IHttpClientFactory httpClientFactory, ISAPSessionStorage sessionStorage)
    {
        _httpClientFactory = httpClientFactory;
        _sessionStorage = sessionStorage;
    }

   
    public async Task<List<BussinesPartnerResponseModel>> GetAllBussinesPartner()
    {
        var session = _sessionStorage.Session;
        if (session == null || string.IsNullOrEmpty(session.B1Session) || string.IsNullOrEmpty(session.RouteId))
            throw new Exception("SAP session is not initialized.");

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={session.B1Session}; ROUTEID={session.RouteId}");

        var requestUrl = $"{_baseUrl}BusinessPartners";
        var response = await client.GetAsync(requestUrl);
        var responseContent = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"StatusCode: {response.StatusCode}");
        Console.WriteLine($"Response: {responseContent}");

        if (response.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize<BussinesPartnerListWrapper>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result?.Value ?? new List<BussinesPartnerResponseModel>();
        }

        throw new InvalidOperationException($"Failed to get employees. Status: {response.StatusCode}. Response: {responseContent}");
    }

    public async Task<BussinesPartnerResponseModel> GetBussinesPartner(string id)
    {
        var session = _sessionStorage.Session;
        if (session == null || string.IsNullOrEmpty(session.B1Session) || string.IsNullOrEmpty(session.B1Session))
            throw new Exception("SAP session is not initialized.");

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={session.B1Session}; ROUTEID={session.RouteId}");

        var requestUrl = $"{_baseUrl}BusinessPartners('{id}')";
        var response = await client.GetAsync(requestUrl);
        var responseContent = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"StatusCode: {response.StatusCode}");
        Console.WriteLine($"Response: {responseContent}");

        if (response.IsSuccessStatusCode)
        {
            var employee = JsonSerializer.Deserialize<BussinesPartnerResponseModel>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return employee!;
        }
        throw new InvalidOperationException($"Failed to get employee. Status: {response.StatusCode}. Response: {responseContent}");

    }
    public async Task<bool> DeleteBussinesPartner(string id)
    {
        var session = _sessionStorage.Session;
        if (session == null || string.IsNullOrEmpty(session.B1Session) || string.IsNullOrEmpty(session.RouteId))
            throw new Exception("SAP session is not initialized.");

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={session.B1Session}; ROUTEID={session.RouteId}");

        var requestUrl = $"{_baseUrl2}('{id}')";
        var response = await client.DeleteAsync(requestUrl);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        return false;
    }

    public async Task<BussinesPartnerResponseModel> CreatBussinesPartner(CreateBussinesPartnerModel createBussinesPartnerModel)
    {

        var session = _sessionStorage.Session;
        if (session == null || string.IsNullOrEmpty(session.B1Session) ||
            string.IsNullOrEmpty(session.RouteId))
            throw new Exception("SAP session is not intialized");

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={session.B1Session};ROUTEID={session.RouteId}");

        var content = new StringContent(
            JsonSerializer.Serialize(createBussinesPartnerModel),
           Encoding.UTF8,
            "application/json"
        );
        var response = await client.PostAsync(_baseUrl2, content);
        var responseContent = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Response Status Code: {response.StatusCode}");
        Console.WriteLine($"Response Content: {responseContent}");

        if (response.StatusCode == HttpStatusCode.Created)
        {
            var result = JsonSerializer.Deserialize<BussinesPartnerResponseModel>(responseContent);
            return result!;
        }
        else if (response.StatusCode == HttpStatusCode.OK)
        {

            throw new InvalidOperationException($"Employee was not created. Status: {response.StatusCode}. Response: {responseContent}");
        }
        else
        {
            response.EnsureSuccessStatusCode();
            return null;
        }
    }

    public async Task<List<BussinesPartnerResponseModel>> FilterBussinesPartnersByPhone(string phone)
    {
        var session = _sessionStorage.Session;
        if (session == null || string.IsNullOrEmpty(session.B1Session) || string.IsNullOrEmpty(session.RouteId))
            throw new Exception("SAP session is not initialized.");

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={session.B1Session}; ROUTEID={session.RouteId}");

        string safePhone = phone.Replace("'", "''");
        string requestUrl = $"{_baseUrl}BusinessPartners?$select=CardCode,CardName,CardType,Phone1,Phone2,Address" +
                            $"&$filter=contains(Phone1,'{safePhone}') or contains(Phone2,'{safePhone}')" +
                            "&$orderby=CardCode&$top=100";

        var response = await client.GetAsync(requestUrl);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize<BussinesPartnerListWrapper>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result?.Value ?? new List<BussinesPartnerResponseModel>();
        }

        throw new InvalidOperationException($"Failed to get Business Partners by phone. Status: {response.StatusCode}. Response: {responseContent}");
    }
    public async Task<List<BussinesPartnerResponseModel>> FilterBussinesPartnersByAddress(string address)
    {
        var session = _sessionStorage.Session;
        if (session == null || string.IsNullOrEmpty(session.B1Session) || string.IsNullOrEmpty(session.RouteId))
            throw new Exception("SAP session is not initialized.");

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={session.B1Session}; ROUTEID={session.RouteId}");

        string safeAddress = address.Replace("'", "''");
        string requestUrl = $"{_baseUrl}BusinessPartners?$select=CardCode,CardName,CardType,Phone1,Phone2,Address" +
                            $"&$filter=contains(Address,'{safeAddress}')" +
                            "&$orderby=CardCode&$top=100";

        var response = await client.GetAsync(requestUrl);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize<BussinesPartnerListWrapper>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result?.Value ?? new List<BussinesPartnerResponseModel>();
        }

        throw new InvalidOperationException($"Failed to get Business Partners by address. Status: {response.StatusCode}. Response: {responseContent}");
    }
    public async Task<List<BussinesPartnerResponseModel>> FilterBussinesPartnersByName(string name)
    {
        var session = _sessionStorage.Session;
        if (session == null || string.IsNullOrEmpty(session.B1Session) || string.IsNullOrEmpty(session.RouteId))
            throw new Exception("SAP session is not initialized.");

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={session.B1Session}; ROUTEID={session.RouteId}");

        // Экранируем апострофы в имени
        string safeName = name.Replace("'", "''");
        string requestUrl = $"{_baseUrl}BusinessPartners?$select=CardCode,CardName,CardType,Phone1,Phone2,Address" +
                            $"&$filter=contains(CardName,'{safeName}')" +
                            "&$orderby=CardCode&$top=100";

        var response = await client.GetAsync(requestUrl);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize<BussinesPartnerListWrapper>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result?.Value ?? new List<BussinesPartnerResponseModel>();
        }

        throw new InvalidOperationException($"Failed to get Business Partners by name. Status: {response.StatusCode}. Response: {responseContent}");
    }

    public async Task<bool> UpdateBussinesPartner(string cardCode, UpdateBussinesPartnerModel updateModel)
    {
        var session = _sessionStorage.Session;
        if (session == null || string.IsNullOrEmpty(session.B1Session) || string.IsNullOrEmpty(session.RouteId))
            throw new Exception("SAP session is not initialized");

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={session.B1Session};ROUTEID={session.RouteId}");

        var content = new StringContent(
            JsonSerializer.Serialize(updateModel),
            Encoding.UTF8,
            "application/json"
        );

        // Формируем URL для PATCH запроса
        var requestUrl = $"{_baseUrl}BusinessPartners('{cardCode}')";

        var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUrl)
        {
            Content = content
        };

        var response = await client.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Response Status Code: {response.StatusCode}");
        Console.WriteLine($"Response Content: {responseContent}");

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Failed to update BusinessPartner. Status: {response.StatusCode}. Response: {responseContent}");
        }
        return true;
    }

}
