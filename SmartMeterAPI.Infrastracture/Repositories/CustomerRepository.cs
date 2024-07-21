using Microsoft.EntityFrameworkCore;
using SmartMeterAPI.Domain;
using SmartMeterAPI.Infrastracture.Data;
using SmartMeterAPI.Infrastracture.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMeterAPI.Infrastracture.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private ApplicationDbContext _dbContext;

        public CustomerRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task AddCustomer(Customer customer)
        {
            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> Exists(int accountId)
        {
            return await _dbContext.Customers.AnyAsync(c => c.AccountId == accountId);
        }
    }
}
