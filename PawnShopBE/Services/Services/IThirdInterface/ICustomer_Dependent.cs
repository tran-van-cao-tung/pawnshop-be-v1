using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IThirdInterface
{
    public interface ICustomer_Dependent:ICustomerService,IDependentService
    {
    }
}
