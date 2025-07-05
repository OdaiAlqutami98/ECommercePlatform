using ECommercePlatform.Logic.Services.Report.ExportService;
using ECommercePlatform.Logic.Services.Report.ExportStrategy;
using ECommercePlatform.Logic.Services.Report.GenerateStrategy;
using Odai.DataModel;
using Odai.Domain.Enums;

namespace ECommercePlatform.Logic.Services.Report.ExportStrategy
{
    public class ReportExportStrategy
    {
        public static IReportExportStrategy GetStrategy(string reportType, OdaiDbContext context)
        {
            return reportType switch
            {
                "pdf" => new PdfExportStrategy(context),
                "xlsx" => new ExcelExportStrategy(),
                _ =>  new PdfExportStrategy(context)
            };

        }
    }
}
