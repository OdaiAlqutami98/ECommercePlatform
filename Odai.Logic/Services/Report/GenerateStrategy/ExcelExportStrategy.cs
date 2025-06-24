using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommercePlatform.Logic.Services.Report.ExportService;
using ECommercePlatform.Logic.Services.Report.ExportStrategy;

namespace ECommercePlatform.Logic.Services.Report.GenerateStrategy
{
   public class ExcelExportStrategy : IReportExportStrategy
    {
        public Task<byte[]> Export(List<ExpandoObject> items, List<ExpandoObject> TableHeaderReport, string reportTitle, string userName)
        {
            var excelService = new ExcelExportService(); 
            var result=  excelService.ExportToExcel(items, TableHeaderReport, reportTitle, userName);
            return Task.FromResult(result);
        }
    }
}
