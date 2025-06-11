

namespace SAP.Application.Model.Employees;

public class EmployeeResponseModel
{
    public int Branch { get; set; }
    public int Department { get; set; }
    public required string FirstName { get; set; }
    public string? JobTitle { get; set; }
    public required string LastName { get; set; }
    public string? Remarks { get; set; }
    public string? WorkCountryCode { get; set; }
}
