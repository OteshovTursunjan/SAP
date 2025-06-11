
using SAP.Application.Model.User;
using System.Text.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace SAP.Application.Service.lmpl;
public class UserService : IUserService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ISAPSessionStorage _sessionStorage;
    private readonly string _baseUrl = "https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/";

    public UserService(IHttpClientFactory httpClientFactory, ISAPSessionStorage sessionStorage)
    {
        _httpClientFactory = httpClientFactory;
        _sessionStorage = sessionStorage;
    }
    public async Task<LoginResponseModel> Login(LoginModel login)
    {
        var client = _httpClientFactory.CreateClient();

        var content = new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(_baseUrl + "Login", content);
        response.EnsureSuccessStatusCode();

        string b1Session = null;
        string routeId = null;

        if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
        {
            foreach (var cookie in cookies)
            {
                if (cookie.Contains("B1SESSION"))
                    b1Session = Regex.Match(cookie, @"B1SESSION=([^;]+)").Groups[1].Value;
                if (cookie.Contains("ROUTEID"))
                    routeId = Regex.Match(cookie, @"ROUTEID=([^;]+)").Groups[1].Value;
            }
        }

        _sessionStorage.Session = new SAPSession
        {
            B1Session = b1Session,
            RouteId = routeId,
            CreatedAt = DateTime.UtcNow
        };

        return new LoginResponseModel { SessionId = b1Session };
    }


}
