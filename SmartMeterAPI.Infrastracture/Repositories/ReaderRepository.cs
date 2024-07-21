using SmartMeterAPI.Domain;
using SmartMeterAPI.Infrastracture.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMeterAPI.Infrastracture.Repositories
{
    public class ReaderRepository
    {
        ApplicationDbContext _dbContext;
        public ReaderRepository(ApplicationDbContext dbContext) { 
            this._dbContext = dbContext;
        }

        public MeterReading Get(int ID) { 
            MeterReading meterReading = this._dbContext.MeterReadings.Where(u => u.Id == ID).FirstOrDefault();
            
            return meterReading;
        }
    }
}
