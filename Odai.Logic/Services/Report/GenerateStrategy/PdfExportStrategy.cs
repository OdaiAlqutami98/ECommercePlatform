using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommercePlatform.Logic.Services.Report.ExportService;
using ECommercePlatform.Logic.Services.Report.ExportStrategy;
using Odai.DataModel;
using Odai.Domain.Enums;

namespace ECommercePlatform.Logic.Services.Report.GenerateStrategy
{
    public class PdfExportStrategy : IReportExportStrategy
    {
        private readonly OdaiDbContext _context;

        public PdfExportStrategy(OdaiDbContext context)
        {
            _context = context;
        }
        public Task<byte[]> Export(List<ExpandoObject> items, List<ExpandoObject> TableHeaderReport, string reportTitle, string userName)
        {
            var pdfService = new PdfExportService(_context);
            return  pdfService.ExportToPDF(items, TableHeaderReport, reportTitle, userName);
        }
    }
}
