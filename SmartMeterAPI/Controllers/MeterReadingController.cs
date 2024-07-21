using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SmartMeterAPI.Domain;
using SmartMeterAPI.Infrastracture.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using SmartMeterAPI.Infrastracture.Repositories.IRepositories;

namespace SmartMeterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeterReadingController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMeterReaderRepository _meterReaderRepository;

        public MeterReadingController(ICustomerRepository customerRepository, IMeterReaderRepository meterReaderRepository)
        {
            _customerRepository = customerRepository;
            _meterReaderRepository = meterReaderRepository;
        }

        [HttpPost("meter-reading-uploads")]
        public async Task<IActionResult> UploadMeterReadings(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            int successfulReads = 0, failedReads = 0;

            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                string line;
                while ((line = await stream.ReadLineAsync()) != null)
                {
                    var data = line.Split(',');

                    if (data.Length < 3 || 
                        !int.TryParse(data[0].Trim(), out int accountId) ||
                        !DateTime.TryParseExact(data[1].Trim(), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime) ||
                        !int.TryParse(data[2].Trim(), out int meterValue))
                    {
                        failedReads++;
                        continue;
                    }


                    if (!await _customerRepository.Exists(accountId))
                    {
                        failedReads++;
                        continue;
                    }

                    // Duplicate check
                    var existingReading = await _meterReaderRepository.GetLatestReading(accountId);
                    if (existingReading != null && existingReading.MeterReadingDateTime >= dateTime)
                    {
                        failedReads++;
                        continue;
                    }

                    await _meterReaderRepository.AddReading(new MeterReading
                    {
                        AccountId = accountId,
                        MeterReadingDateTime = dateTime,
                        MeterReadValue = meterValue
                    });

                    successfulReads++;

                }
            }

            // await _context.SaveChangesAsync();

            return Ok(new UploadResult { SuccessfulReads = successfulReads, FailedReads = failedReads });
        }
    }
}
