using CsvHelper;
using Microsoft.Extensions.DependencyInjection;
using SmartMeterAPI.Domain;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMeterAPI.Infrastracture.Data
{
    public static class CustomerSeeder
    {
        public static void SeedCSV(IServiceProvider serviceProvider, string filepath) {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (!context.Customers.Any())
                {
                    using (var reader = new StreamReader(filepath))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        List<Customer> records = csv.GetRecords<Customer>().ToList();

                        context.Customers.AddRange(records);
                        context.SaveChanges();
                    }
                }
            }
        }



    }
}
