using System.Net;
using System.Text;
using System.Text.Json;
using Moq;
using Moq.Protected;
using SAP.Application.Model.BussinesPartner;
using SAP.Application.Model.User;
using SAP.Application.Service;
using SAP.Application.Service.lmpl;
namespace SAP.Test;

public  class BussinesPartnerTest
{
    private readonly Mock<ISAPSessionStorage> _mockSessionStorage;
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly SAPSession _sapSession;

    public BussinesPartnerTest()
    {
        _mockSessionStorage = new Mock<ISAPSessionStorage>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();

        _sapSession = new SAPSession
        {
            B1Session = "test-session",
            RouteId = "test-route"
        };
    }

    private HttpClient CreateHttpClient(HttpResponseMessage response)
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        return new HttpClient(handler.Object);
    }

    [Fact]
    public async Task GetAllBussinesPartner_ReturnsList()
    {
        _mockSessionStorage.Setup(x => x.Session).Returns(_sapSession);

        var responseObj = new BussinesPartnerListWrapper
        {
            Value = new List<BussinesPartnerResponseModel>
            {
                new BussinesPartnerResponseModel
                {
                    CardCode = "C001",
                    CardName = "Test Partner",
                    CardType = "cCustomer"
                }
            }
        };

        var responseJson = JsonSerializer.Serialize(responseObj);
        var httpClient = CreateHttpClient(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
        });

        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var service = new BussinesPartnerService(_mockHttpClientFactory.Object, _mockSessionStorage.Object);
        var result = await service.GetAllBussinesPartner();

        Assert.Single(result);
        Assert.Equal("C001", result[0].CardCode);
    }

    [Fact]
    public async Task GetBussinesPartner_ReturnsPartner()
    {
        _mockSessionStorage.Setup(x => x.Session).Returns(_sapSession);

        var partner = new BussinesPartnerResponseModel
        {
            CardCode = "C001",
            CardName = "Test Partner",
            CardType = "cCustomer"
        };

        var responseJson = JsonSerializer.Serialize(partner);
        var httpClient = CreateHttpClient(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
        });

        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var service = new BussinesPartnerService(_mockHttpClientFactory.Object, _mockSessionStorage.Object);
        var result = await service.GetBussinesPartner("C001");

        Assert.Equal("C001", result.CardCode);
    }

    [Fact]
    public async Task DeleteBussinesPartner_ReturnsTrue()
    {
        _mockSessionStorage.Setup(x => x.Session).Returns(_sapSession);

        var httpClient = CreateHttpClient(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NoContent
        });

        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var service = new BussinesPartnerService(_mockHttpClientFactory.Object, _mockSessionStorage.Object);
        var result = await service.DeleteBussinesPartner("C001");

        Assert.True(result);
    }

    [Fact]
    public async Task CreatBussinesPartner_ReturnsCreatedPartner()
    {
        _mockSessionStorage.Setup(x => x.Session).Returns(_sapSession);

        var createModel = new CreateBussinesPartnerModel
        {
            CardCode = "C001",
            CardName = "New Partner",
            CardType = "cCustomer"
        };

        var responseJson = JsonSerializer.Serialize(new BussinesPartnerResponseModel
        {
            CardCode = "C001",
            CardName = "New Partner",
            CardType = "cCustomer"
        });

        var httpClient = CreateHttpClient(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Created,
            Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
        });

        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var service = new BussinesPartnerService(_mockHttpClientFactory.Object, _mockSessionStorage.Object);
        var result = await service.CreatBussinesPartner(createModel);

        Assert.Equal("C001", result.CardCode);
    }

    [Fact]
    public async Task FilterBussinesPartnersByPhone_ReturnsFilteredList()
    {
        _mockSessionStorage.Setup(x => x.Session).Returns(_sapSession);

        var responseObj = new BussinesPartnerListWrapper
        {
            Value = new List<BussinesPartnerResponseModel>
            {
                new BussinesPartnerResponseModel
                {
                    CardCode = "C001",
                    CardName = "Partner 1",
                    CardType = "cCustomer"
                }
            }
        };

        var responseJson = JsonSerializer.Serialize(responseObj);
        var httpClient = CreateHttpClient(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
        });

        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var service = new BussinesPartnerService(_mockHttpClientFactory.Object, _mockSessionStorage.Object);
        var result = await service.FilterBussinesPartnersByPhone("123");

        Assert.Single(result);
    }

    [Fact]
    public async Task FilterBussinesPartnersByAddress_ReturnsFilteredList()
    {
        _mockSessionStorage.Setup(x => x.Session).Returns(_sapSession);

        var responseObj = new BussinesPartnerListWrapper
        {
            Value = new List<BussinesPartnerResponseModel>
            {
                new BussinesPartnerResponseModel
                {
                    CardCode = "C002",
                    CardName = "Partner 2",
                    CardType = "cCustomer"
                }
            }
        };

        var responseJson = JsonSerializer.Serialize(responseObj);
        var httpClient = CreateHttpClient(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
        });

        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var service = new BussinesPartnerService(_mockHttpClientFactory.Object, _mockSessionStorage.Object);
        var result = await service.FilterBussinesPartnersByAddress("Some Street");

        Assert.Single(result);
    }

    [Fact]
    public async Task FilterBussinesPartnersByName_ReturnsFilteredList()
    {
        _mockSessionStorage.Setup(x => x.Session).Returns(_sapSession);

        var responseObj = new BussinesPartnerListWrapper
        {
            Value = new List<BussinesPartnerResponseModel>
            {
                new BussinesPartnerResponseModel
                {
                    CardCode = "C003",
                    CardName = "Partner 3",
                    CardType = "cCustomer"
                }
            }
        };

        var responseJson = JsonSerializer.Serialize(responseObj);
        var httpClient = CreateHttpClient(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
        });

        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var service = new BussinesPartnerService(_mockHttpClientFactory.Object, _mockSessionStorage.Object);
        var result = await service.FilterBussinesPartnersByName("Partner 3");

        Assert.Single(result);
    }

    [Fact]
    public async Task UpdateBussinesPartner_ReturnsTrue()
    {
        _mockSessionStorage.Setup(x => x.Session).Returns(_sapSession);

        var httpClient = CreateHttpClient(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NoContent
        });

        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var service = new BussinesPartnerService(_mockHttpClientFactory.Object, _mockSessionStorage.Object);
        var result = await service.UpdateBussinesPartner("C001", new UpdateBussinesPartnerModel { CardName = "Updated Name" });

        Assert.True(result);
    }
}
