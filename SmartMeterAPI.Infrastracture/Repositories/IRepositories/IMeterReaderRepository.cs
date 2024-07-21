using SmartMeterAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMeterAPI.Infrastracture.Repositories.IRepositories
{
    public interface IMeterReaderRepository
    {
        Task<MeterReading> GetLatestReading(int accountId);
        Task AddReading(MeterReading reading);
    }
}
