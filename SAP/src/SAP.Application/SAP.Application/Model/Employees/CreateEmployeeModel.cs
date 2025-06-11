using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Application.Model.Employees;

public class CreateEmployeeModel
{
    public required string Branch { get; set; }
    public required string Department { get; set; }
    public required string FirstName { get; set; }
    public string? JobTitle { get; set; }
    public required string LastName { get; set; }
    public string? WorkCountryCode { get; set; }
    public string? MobilePhone { get; set; }
    public string? OfficePhone { get; set; }

    public string? eMail { get; set; }

}