using SAP.Application.Model;
using SAP.Application.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Application.Service;

public  interface IUserService
{
    Task<LoginResponseModel> Login(LoginModel login);

}
