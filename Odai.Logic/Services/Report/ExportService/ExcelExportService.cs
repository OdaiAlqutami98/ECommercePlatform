using System.Dynamic;
using ClosedXML.Excel;

namespace ECommercePlatform.Logic.Services.Report.ExportService
{
    public class ExcelExportService 
    {
        public byte[] ExportToExcel(List<ExpandoObject> data, List<ExpandoObject> tableHeaderReport, string reportTitle, string userName)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(reportTitle ?? "Report");

                AddReportTitle(worksheet, reportTitle, data.First().Count());

                int currentRow = 3;

                if (tableHeaderReport != null && tableHeaderReport.Any())
                {
                    var headerHeaders = GetHeaders(tableHeaderReport);
                    var orderedHeaderHeaders = headerHeaders.Keys.Reverse();
                    AddColumnHeaders(worksheet, orderedHeaderHeaders, currentRow);
                    currentRow++;

                    AddDataRows(worksheet, tableHeaderReport, orderedHeaderHeaders, currentRow);
                    currentRow += tableHeaderReport.Count + 2;
                }

                var headers = GetHeaders(data);
                var orderedHeaders = headers.Keys.Reverse();
                AddColumnHeaders(worksheet, orderedHeaders, currentRow);
                AddDataRows(worksheet, data, orderedHeaders, currentRow + 1);

                AutoFitColumns(worksheet, orderedHeaders.Count());

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
        private void AddReportTitle(IXLWorksheet worksheet, string reportTitle, int columnCount)
        {
            var cell = worksheet.Cell(1, 1);
            cell.Value = reportTitle;
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontSize = 20;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            worksheet.Range(1, 1, 1, columnCount).Merge();
            worksheet.Row(1).Height = 25;
        }
        private IDictionary<string, object> GetHeaders(List<ExpandoObject> data)
        {
            var headers = data.FirstOrDefault() as IDictionary<string, object>;
            return headers;
        }
        private void AddColumnHeaders(IXLWorksheet worksheet, IEnumerable<string> headers, int startRow)
        {
            int col = 1;
            foreach (var header in headers)
            {
                var cell = worksheet.Cell(startRow, col);
                cell.Value = header;
                cell.Style.Font.Bold = true;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                col++;
            }
            worksheet.Row(startRow).Height = 20;
        }
        private void AddDataRows(IXLWorksheet worksheet, List<ExpandoObject> data, IEnumerable<string> headers, int startRow)
        {
            int rowNumber = startRow;
            foreach (var item in data)
            {
                var dict = item as IDictionary<string, object>;
                int colNumber = 1;
                foreach (var header in headers)
                {
                    if (!dict.ContainsKey(header))
                        continue;

                    AddCellData(worksheet, rowNumber, colNumber, header, dict);
                    colNumber++;
                }
                rowNumber++;
            }
        }
        private void AddCellData(IXLWorksheet worksheet, int rowNumber, int colNumber, string header, IDictionary<string, object> itemDict)
        {
            string value = itemDict.TryGetValue(header, out var cellValue) ? cellValue?.ToString() : "N/A";
            var cell = worksheet.Cell(rowNumber, colNumber);
            if ((IsImageHeader(header) || IsImageFile(value)) && File.Exists(value))
            {
                var image = worksheet.AddPicture(value);
                int imgWidth = 60;  
                int imgHeight = 50; 
                image.MoveTo(cell);
                image.WithSize(imgWidth, imgHeight);
                worksheet.Row(rowNumber).Height = 40; 
                worksheet.Column(colNumber).Width = 15;
            }
            else
            {
                cell.Value = value;
                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }
        }
        private bool IsImageHeader(string key)
        {
            return key.ToLower().Contains("image") || key.ToLower().Contains("file");
        }
        private bool IsImageFile(string path)
        {
            var extensions = new[] { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };
            return extensions.Any(ext => path.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
        }
        private void AutoFitColumns(IXLWorksheet worksheet, int columnCount)
        {
            for (int col = 1; col <= columnCount; col++)
            {
                worksheet.Column(col).AdjustToContents();
            }
        }
    }
}
