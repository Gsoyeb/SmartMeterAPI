using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartMeterAPI.Domain
{
    public class MeterReading
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int AccountId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public int MeterReadValue { get; set; }

        public Customer Customer { get; set; }
    }

}
