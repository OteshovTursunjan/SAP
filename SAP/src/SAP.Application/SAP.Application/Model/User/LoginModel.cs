using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Application.Model.User
{
    public  class LoginModel
    {
        public required string CompanyDB {  get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }

    }
}
