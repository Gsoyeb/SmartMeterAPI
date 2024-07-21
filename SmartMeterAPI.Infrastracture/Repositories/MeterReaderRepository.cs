using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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
    public class MeterReaderRepository: IMeterReaderRepository
    {
        ApplicationDbContext _dbContext;
        public MeterReaderRepository(ApplicationDbContext dbContext) { 
            this._dbContext = dbContext;
        }
        public async Task<MeterReading> GetLatestReading(int accountId)
        {
            return await _dbContext.MeterReadings
                                 .Where(m => m.AccountId == accountId)
                                 .OrderByDescending(m => m.MeterReadingDateTime)
                                 .FirstOrDefaultAsync();
        }

        public async Task AddReading(MeterReading reading)
        {
            _dbContext.MeterReadings.Add(reading);
            await _dbContext.SaveChangesAsync();
        }
    }
}
