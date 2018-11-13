using AuthServiceWCF.Helpers;
using AuthServiceWCF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AuthServiceWCF
{
    [ServiceContract]
    public interface IAuthService
    {

        [OperationContract]
        IActionResultWCF Login(LoginModel loginModel);

        [OperationContract]
        IActionResultWCF Register(RegisterModel registerModel);

    }
}
