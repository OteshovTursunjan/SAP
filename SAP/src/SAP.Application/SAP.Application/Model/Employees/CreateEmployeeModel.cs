using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Application.Model.Employees;

public class CreateEmployeeModel
{
    public int Branch { get; set; }
    public int Department { get; set; }
    public required string FirstName { get; set; }
    public string? JobTitle { get; set; }
    public required string LastName { get; set; }
    public string? Remarks { get; set; }
    public string? WorkCountryCode { get; set; }

}