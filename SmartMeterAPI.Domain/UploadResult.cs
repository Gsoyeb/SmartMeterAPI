using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMeterAPI.Domain
{
    public class UploadResult
    {
        public int SuccessfulReads { get; set; }
        public int FailedReads { get; set; }
    }

}
