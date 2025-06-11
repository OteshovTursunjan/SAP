

using SAP.Application.Model.User;

namespace SAP.Application.Service.lmpl;

public  class SAPSessionStorage : ISAPSessionStorage
{
    public SAPSession Session { get; set; }
}
