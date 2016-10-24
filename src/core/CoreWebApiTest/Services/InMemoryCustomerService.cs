using CoreWebApiTest.Models;
using System;
using System.Collections.Generic;

namespace CoreWebApiTest.Services
{
    public class InMemoryCustomerService : ICustomerService
    {
        private IDictionary<int, CustomerModel> _internalStorage = new Dictionary<int, CustomerModel>();

        public void Create(CustomerModel model)
        {
            model.Id = _internalStorage.Count + 1;
            _internalStorage.Add(model.Id, model);
        }

        public void Delete(int id)
        {
            _internalStorage.Remove(id);
        }

        public IEnumerable<CustomerModel> List()
        {
            return _internalStorage.Values;
        }
    }
}
