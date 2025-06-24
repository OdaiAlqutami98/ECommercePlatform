using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odai.Domain.Enums;

namespace ECommercePlatform.Domain.Report
{
   public class ReportRequest
    {
        public List<ExpandoObject> Items { get; set; }
        public List<ExpandoObject> Headers { get; set; }
        public string ReportType { get; set; } // "PDF" or "Excel"
        public string Title { get; set; }
        public string UserName { get; set; }

    }
}
