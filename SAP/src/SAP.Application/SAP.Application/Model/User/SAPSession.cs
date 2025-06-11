using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Application.Model.User;

public  class SAPSession
{
    public string B1Session { get; set; }
    public string RouteId { get; set; }
    public DateTime CreatedAt { get; set; }
}
