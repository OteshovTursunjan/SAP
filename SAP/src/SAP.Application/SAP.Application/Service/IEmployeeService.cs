using SAP.Application.Model.Employees;
using SAP.Application.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Application.Service;

public  interface IEmployeeService
{
    Task<EmployeeResponseModel> CreateEmployee(CreateEmployeeModel employee);
    Task<GetEmployeeResponseModel> GetEmployeeById(int id);
    Task<List<GetEmployeeResponseModel>> GetAllEmployees();
    Task<bool> DeleteEmployees(int id);
}
