

namespace SAP.Application.Model.Employees;

public class EmployeeResponseModel
{
    public int Branch { get; set; }
    public int Department { get; set; }
    public required string FirstName { get; set; }
    public string? JobTitle { get; set; }
    public required string LastName { get; set; }
    public string? WorkCountryCode { get; set; }
    public string? MobilePhone { get; set; }
    public string? OfficePhone { get; set; }

    public string? eMail { get; set; }
}
