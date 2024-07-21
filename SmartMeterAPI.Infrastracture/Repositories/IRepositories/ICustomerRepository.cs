using SmartMeterAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMeterAPI.Infrastracture.Repositories.IRepositories
{
    public interface ICustomerRepository
    {
        Task<bool> Exists(int accountId);
        Task AddCustomer(Customer customer);
    }
}
