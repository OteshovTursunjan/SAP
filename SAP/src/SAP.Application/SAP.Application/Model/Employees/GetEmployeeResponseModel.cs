
using SAP.Application.Service.lmpl;
using System.Text.Json.Serialization;

namespace SAP.Application.Model.Employees;

public  class GetEmployeeResponseModel
{
    [JsonConverter(typeof(IntOrStringToIntConverter))]
    public int? Branch { get; set; }

    [JsonConverter(typeof(IntOrStringToIntConverter))]
    public int? Department { get; set; }

    public required string FirstName { get; set; }
    public string? JobTitle { get; set; }
    public required string LastName { get; set; }
    public string? Remarks { get; set; }
    public string? WorkCountryCode { get; set; }
}
public class EmployeeListWrapper
{
    public List<GetEmployeeResponseModel> Value { get; set; } = [];
}