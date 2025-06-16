using Moq.Protected;
using Moq;
using SAP.Application.Model.Items;
using SAP.Application.Model.User;
using SAP.Application.Service.lmpl;
using SAP.Application.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace SAP.Test;

public  class ItemServiceTest
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<ISAPSessionStorage> _sessionStorageMock;
    private readonly ItemService _service;
    private readonly HttpClient _httpClient;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;

    public ItemServiceTest()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _sessionStorageMock = new Mock<ISAPSessionStorage>();
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(_httpClient);

        _service = new ItemService(_httpClientFactoryMock.Object, _sessionStorageMock.Object);
    }

    private void SetupSession()
    {
        _sessionStorageMock.Setup(s => s.Session).Returns(new SAPSession
        {
            B1Session = "test-session",
            RouteId = "test-route"
        });
    }

    [Fact]
    public async Task CreateItem_ShouldReturnItemResponse()
    {
        SetupSession();

        var item = new CreateItemsModel
        {
            ItemCode = "1001",
            ItemName = "Test Item",
            ItemType = "Item",
            U_TypeGroup = "Group1"
        };

        var responseModel = new ItemsResponseModel
        {
            ItemCode = "1001",
            ItemName = "Test Item",
            ItemType = "Item",
            U_TypeGroup = "Group1",
            SalesItem = "Y"
        };

        var jsonResponse = JsonSerializer.Serialize(responseModel);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.Created)
        {
            Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
        };

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var result = await _service.CreateItem(item);

        Assert.Equal("1001", result.ItemCode);
        Assert.Equal("Test Item", result.ItemName);
    }

    [Fact]
    public async Task GetItemById_ShouldReturnItem()
    {
        SetupSession();

        var responseModel = new ItemsResponseModel
        {
            ItemCode = "1001",
            ItemName = "Test Item",
            ItemType = "Item",
            U_TypeGroup = "Group1",
            SalesItem = "Y"
        };

        var jsonResponse = JsonSerializer.Serialize(responseModel);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
        };

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var result = await _service.GetItemById("1001");

        Assert.Equal("1001", result.ItemCode);
        Assert.Equal("Test Item", result.ItemName);
    }

    [Fact]
    public async Task GetAllItems_ShouldReturnListOfItems()
    {
        SetupSession();

        var responseModel = new ItemsListWrapper
        {
            Value = new List<ItemsResponseModel>
            {
                new ItemsResponseModel
                {
                    ItemCode = "1001",
                    ItemName = "Test Item",
                    ItemType = "Item",
                    U_TypeGroup = "Group1",
                    SalesItem = "Y"
                }
            }
        };

        var jsonResponse = JsonSerializer.Serialize(responseModel);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
        };

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var result = await _service.GetAllItems();

        Assert.Single(result);
        Assert.Equal("1001", result[0].ItemCode);
    }

    [Fact]
    public async Task DeleteItem_ShouldReturnTrue_OnSuccess()
    {
        SetupSession();

        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var result = await _service.DeleteItem("1001");

        Assert.True(result);
    }
}
