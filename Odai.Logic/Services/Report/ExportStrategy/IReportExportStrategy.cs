using System.Dynamic;
using Odai.Domain.Enums;

namespace ECommercePlatform.Logic.Services.Report.ExportStrategy
{
   public interface IReportExportStrategy
    {
        Task<byte[]> Export(List<ExpandoObject> items, List<ExpandoObject> TableHeaderReport, string reportTitle, string userName);
    }
}
