using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SmartMeterAPI.Domain;
using SmartMeterAPI.Infrastracture.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Diagnostics;

namespace SmartMeterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeterReadingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MeterReadingController(ApplicationDbContext context)
        {
            _context = context;
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

                    if (data.Length < 3 || !int.TryParse(data[0].Trim(), out int accountId) ||
                        !DateTime.TryParseExact(data[1].Trim(), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime) ||
                        !int.TryParse(data[2].Trim(), out int meterValue))
                    {
                        failedReads++;
                        // Debug.WriteLine($"Failure: ID = {data[0]} at time {data[1]}");
                        continue;
                    }


                    if (!_context.Customers.Any(c => c.AccountId == accountId))
                    {
                        // Debug.WriteLine($"Failure: ID = {data[0]} at time {data[1]}");
                        failedReads++;
                        continue;
                    }

                    var existingReading = _context.MeterReadings
                        .Where(m => m.AccountId == accountId)
                        .OrderByDescending(m => m.MeterReadingDateTime)
                        .FirstOrDefault();

                    if (existingReading != null && existingReading.MeterReadingDateTime >= dateTime)
                    {
                        // Debug.WriteLine($"Failure: ID = {data[0]} at time {data[1]}");
                        failedReads++;
                        continue;
                    }

                    _context.MeterReadings.Add(new MeterReading
                    {
                        AccountId = accountId,
                        MeterReadingDateTime = dateTime,
                        MeterReadValue = meterValue
                    });

                    successfulReads++;

                    // Debug.WriteLine($"Success: ID = {data[0]} at time {data[1]}");
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new { SuccessfulReads = successfulReads, FailedReads = failedReads });
        }
    }
}
