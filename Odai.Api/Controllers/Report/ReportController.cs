using ECommercePlatform.Domain.Report;
using ECommercePlatform.Logic.Services.Report.ExportFactory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Odai.DataModel;
using Odai.Domain.Enums;

namespace ECommercePlatform.Api.Controllers.Report
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private OdaiDbContext _context;
        public ReportController(OdaiDbContext context)
        {
            _context = context;
        }
        [HttpPost("export")]
        public async Task<IActionResult> ExportReport([FromBody] ReportRequest request)
        {
            try
            {
                var strategy = ReportExportStrategy.GetStrategy(request.ReportType, _context);

                byte[] reportBytes = await strategy.Export(
                    request.Items,
                    request.Headers,
                    request.Title,
                    request.UserName);

                string contentType = request.ReportType == "PDF" ? "application/pdf" : "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string fileExtension = request.ReportType == "PDF" ? ".pdf" : ".xlsx";
                string fileName = $"{request.Title}_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";

                return File(reportBytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
