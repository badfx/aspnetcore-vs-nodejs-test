using CoreWebApiTest.Models;
using System.Collections.Generic;

namespace CoreWebApiTest.Services
{
    public interface ICustomerService
    {
        IEnumerable<CustomerModel> List();
        void Create(CustomerModel model);
        void Delete(int id);
    }
}
