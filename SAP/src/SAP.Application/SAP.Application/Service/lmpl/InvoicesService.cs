﻿

using SAP.Application.Model.Invoices;
using SAP.Application.Model.Items;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using static SAP.Application.Model.Invoices.DocumentRequest;

namespace SAP.Application.Service.lmpl;

public class InvoicesService : IInvoicesService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ISAPSessionStorage _sessionStorage;
    private readonly string _baseUrl = "https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/";
    private readonly string _baseUrl2 = "https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/Invoices";

    public InvoicesService(IHttpClientFactory httpClientFactory, ISAPSessionStorage sessionStorage)
    {
        _httpClientFactory = httpClientFactory;
        _sessionStorage = sessionStorage;
    }
    public async Task<InvoicesResponseModel> CreateInvoice(CreateInvoice.CreateInvoicesModel model)
    {
        var session = _sessionStorage.Session;
        if (session == null || string.IsNullOrEmpty(session.B1Session) ||
            string.IsNullOrEmpty(session.RouteId))
            throw new Exception("SAP session is not intialized");

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={session.B1Session};ROUTEID={session.RouteId}");

        var content = new StringContent(
            JsonSerializer.Serialize(model),
           Encoding.UTF8,
            "application/json"
        );
        var response = await client.PostAsync(_baseUrl2, content);
        var responseContent = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Response Status Code: {response.StatusCode}");
        Console.WriteLine($"Response Content: {responseContent}");

        if (response.StatusCode == HttpStatusCode.Created)
        {
            var result = JsonSerializer.Deserialize<InvoicesResponseModel>(responseContent);
            return result!;
        }
        else if (response.StatusCode == HttpStatusCode.OK)
        {

            throw new InvalidOperationException($"Invoices was not created. Status: {response.StatusCode}. Response: {responseContent}");
        }
        else
        {
            response.EnsureSuccessStatusCode();
            return null;
        }
    }


    public async Task<List<InvoicesResponseModel>> GetAllInvoices()
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
            var result = JsonSerializer.Deserialize<InvoicesListWrapper>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result?.Value ?? new List<InvoicesResponseModel>();
        }

        throw new InvalidOperationException($"Failed to get employees. Status: {response.StatusCode}. Response: {responseContent}");
    }

    public async Task<InvoicesResponseModel> GetInvoiceById(int id)
    {
        var session = _sessionStorage.Session;
        if (session == null || string.IsNullOrEmpty(session.B1Session) || string.IsNullOrEmpty(session.B1Session))
            throw new Exception("SAP session is not initialized.");

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={session.B1Session}; ROUTEID={session.RouteId}");

        var requestUrl = $"{_baseUrl}Invoices({id})";
        var response = await client.GetAsync(requestUrl);
        var responseContent = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"StatusCode: {response.StatusCode}");
        Console.WriteLine($"Response: {responseContent}");

        if (response.IsSuccessStatusCode)
        {
            var employee = JsonSerializer.Deserialize<InvoicesResponseModel>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return employee!;
        }
        throw new InvalidOperationException($"Failed to get employee. Status: {response.StatusCode}. Response: {responseContent}");
    }
    public async Task<bool> CloseInvoices(int id)
    {
        var session = _sessionStorage.Session;
        if (session == null || string.IsNullOrEmpty(session.B1Session) || string.IsNullOrEmpty(session.RouteId))
            throw new Exception("SAP session is not initialized.");

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={session.B1Session}; ROUTEID={session.RouteId}");

        var requestUrl = $"{_baseUrl2}('{id}')/Close";
        var response = await client.DeleteAsync(requestUrl);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        return false;
    }

   
}
