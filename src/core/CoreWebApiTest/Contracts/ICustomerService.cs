using CoreWebApiTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApiTest.Contracts
{
    public interface ICustomerService
    {
        IEnumerable<CustomerModel> List();
        void Create(CustomerModel model);
        void Delete(int id);
    }
}
