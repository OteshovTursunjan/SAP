using SAP.Application.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Application.Service;

public interface ISAPSessionStorage
{
    SAPSession Session { get; set; }
}
