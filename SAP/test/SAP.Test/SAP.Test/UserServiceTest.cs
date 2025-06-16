using Moq.Protected;
using Moq;
using SAP.Application.Model.User;
using SAP.Application.Service.lmpl;
using SAP.Application.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Test;

public  class UserServiceTest
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<ISAPSessionStorage> _sessionStorageMock;
    private readonly UserService _service;
    private readonly HttpClient _httpClient;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;

    public UserServiceTest()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _sessionStorageMock = new Mock<ISAPSessionStorage>();
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(_httpClient);

        _service = new UserService(_httpClientFactoryMock.Object, _sessionStorageMock.Object);
    }

    [Fact]
    public async Task Login_ShouldStoreSessionAndReturnSessionId()
    {
        // Arrange
        var loginModel = new LoginModel
        {
            CompanyDB = "SBODEMO",
            UserName = "manager",
            Password = "1234"
        };

        var jsonResponse = "{}"; // Ответ API неважен, так как ты не используешь тело ответа

        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
        };

        httpResponse.Headers.Add("Set-Cookie", "B1SESSION=session-id-123; Path=/b1s/v2/; Secure; HttpOnly");
        httpResponse.Headers.Add("Set-Cookie", "ROUTEID=route-id-456; Path=/b1s/v2/; Secure; HttpOnly");

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act
        var result = await _service.Login(loginModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("session-id-123", result.SessionId);

        _sessionStorageMock.VerifySet(s => s.Session = It.Is<SAPSession>(
      sess => sess.B1Session == "session-id-123" &&
              sess.RouteId == "route-id-456" &&
              sess.CreatedAt <= DateTime.UtcNow
  ), Times.Once);

    }
}
