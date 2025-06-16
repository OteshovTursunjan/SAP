using Moq;
using Moq.Protected;
using SAP.Application.Model.Employees;
using SAP.Application.Service.lmpl;
using SAP.Application.Service;
using System.Net;
using System.Text;
using System.Text.Json;
using SAP.Application.Model.User;

namespace SAP.Test;

public class EmployeeServiceTest
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<ISAPSessionStorage> _sessionStorageMock;
    private readonly EmployeeService _service;
    private readonly HttpClient _httpClient;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;

    public EmployeeServiceTest()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _sessionStorageMock = new Mock<ISAPSessionStorage>();
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(_httpClient);

        _service = new EmployeeService(_httpClientFactoryMock.Object, _sessionStorageMock.Object);
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
    public async Task CreateEmployee_ShouldReturnEmployeeResponse()
    {
        SetupSession();

        var employee = new CreateEmployeeModel
        {
            Branch = "1",
            Department = "2",
            FirstName = "John",
            LastName = "Doe"
        };

        var responseModel = new EmployeeResponseModel
        {
            Branch = 1,
            Department = 2,
            FirstName = "John",
            LastName = "Doe"
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

        var result = await _service.CreateEmployee(employee);

        Assert.Equal("John", result.FirstName);
        Assert.Equal("Doe", result.LastName);
    }

    [Fact]
    public async Task GetEmployeeById_ShouldReturnEmployee()
    {
        SetupSession();

        var responseModel = new GetEmployeeResponseModel
        {
            Branch = 1,
            Department = 2,
            FirstName = "John",
            LastName = "Doe"
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

        var result = await _service.GetEmployeeById(1);

        Assert.Equal("John", result.FirstName);
        Assert.Equal("Doe", result.LastName);
    }

    [Fact]
    public async Task GetAllEmployees_ShouldReturnListOfEmployees()
    {
        SetupSession();

        var responseModel = new EmployeeListWrapper
        {
            Value = new List<GetEmployeeResponseModel>
            {
                new GetEmployeeResponseModel { FirstName = "John", LastName = "Doe" }
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

        var result = await _service.GetAllEmployees();

        Assert.Single(result);
        Assert.Equal("John", result[0].FirstName);
    }

    [Fact]
    public async Task DeleteEmployees_ShouldReturnTrue_OnSuccess()
    {
        SetupSession();

        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var result = await _service.DeleteEmployees(1);

        Assert.True(result);
    }
}
