using System.Dynamic;
using System.Text.RegularExpressions;
using ECommercePlatform.Shared.Constants;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using Odai.DataModel;

namespace ECommercePlatform.Logic.Services.Report.ExportService
{
    public class PdfExportService 
    {
        private readonly OdaiDbContext _context;
        public PdfExportService(OdaiDbContext context)
        {
            _context = context;
        }
       
        public async Task<byte[]> ExportToPDF(List<ExpandoObject> items, List<ExpandoObject> TableHeaderReport, string reportTitle, string userName)
        {
            using (MemoryStream stream = new MemoryStream())
            {

                int columnCount = GetColumnCount(items);
                int columnWidth = 170;
                int basePageWidth = 80;
                int pageWidth = CalculatePageWidth(columnCount, columnWidth, basePageWidth);
                int minPageHeight = 800;

                using (Document pdfDoc = CreateDocument(pageWidth, minPageHeight))
                using (PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream))
                {
                    pdfDoc.Open();
                    string logoBytes = await GetLogoBytesAsync();
                    AddDocumentHeader(pdfDoc, reportTitle, userName, logoBytes);
                    if (TableHeaderReport.Any() == true)
                    {
                        AddTableToDocument(pdfDoc, TableHeaderReport);
                        AddSpacing(pdfDoc, 3);
                    }

                    if (items?.Any() == true)
                    {
                        AddTableToDocument(pdfDoc, items);
                    }
                    pdfDoc.Close();

                }
                return stream.ToArray();

            }

        }
        private void AddTableToDocument(Document pdfDoc, List<ExpandoObject> data)
        {
            int columnCount = GetColumnCount(data);
            PdfPTable table = CreateTable(data, columnCount);
            var firstRow = data.FirstOrDefault() as IDictionary<string, object>;
            if (firstRow != null)
            {
                AddTableHeaders(table, firstRow);
            }
            AddTableRows(table, data);
            table.SplitLate = false;
            table.KeepTogether = true;
            pdfDoc.Add(table);

        }
        private void AddTableRows(PdfPTable table, List<ExpandoObject> items)
        {
            if (items == null || items.Count == 0) return;
            Font contentFont = GetFont(15);

            foreach (var item in items)
            {
                var itemDict = item as IDictionary<string, object>;
                var keys = itemDict.Keys.ToList();


                foreach (var key in keys)
                {
                    string value = itemDict[key]?.ToString() ?? "N/A";
                    PdfPCell cell;
                    if (key.ToLower().Contains("image")|| key.ToLower().Contains("file")|| IsImageFile(value))
                    {
                        if (File.Exists(value))
                        {
                           Image img = Image.GetInstance(value);
                            img.ScaleAbsolute(70f, 50f); // Adjust size
                            cell = new PdfPCell(img, fit: false)
                            {
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                VerticalAlignment = Element.ALIGN_MIDDLE,
                                Padding = 5
                            };
                        }
                        else
                        {
                            cell = new PdfPCell(new Phrase(ResponseMessages.ImageNotFound, contentFont));
                        }
                    }
                    else
                    {
                         cell = CreateCell(key, value, contentFont, itemDict);
                    }
                        table.AddCell(cell);
                }
            }
        }
        private bool IsImageFile(string path)
        {
            var extensions = new[] { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };
            return extensions.Any(ext => path.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
        }
        private PdfPCell CreateCell(string key, string value, Font contentFont, IDictionary<string, object> itemDict)
        {
            PdfPCell cell = new PdfPCell
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                NoWrap = false,
            };
           
                cell = new PdfPCell(new Phrase(value, contentFont))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    NoWrap = false,
                    Padding = 6f

                };
           

            return cell;
        }
        private void AddTableHeaders(PdfPTable table, IDictionary<string, object> firstItem)
        {
            if (firstItem == null) return;

            Font headerFont = GetFont(16, Font.BOLD);
            var keys = firstItem.Keys.ToList();

           

            foreach (var key in keys)
            {
                PdfPCell cell = new PdfPCell(new Phrase(key, headerFont))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    BackgroundColor = new BaseColor(200, 200, 200),
                    Padding = 10
                };
                table.AddCell(cell);
            }
        }
        private PdfPTable CreateTable(List<ExpandoObject> items, int columnCount)
        {
            PdfPTable table = new PdfPTable(columnCount) { WidthPercentage = 100 };
            float[] columnWidths = CalculateColumnWidths(items, columnCount);
            table.SetWidths(columnWidths);
            return table;
        }
        private float[] CalculateColumnWidths(List<ExpandoObject> items, int columnCount)
        {
            float[] columnWidths = new float[columnCount];

            var keys = (items.First() as IDictionary<string, object>).Keys
                .ToList();


            for (int colIndex = 0; colIndex < columnCount; colIndex++)
            {
                string key = keys[colIndex];
                int maxWordCount = items.Max(item =>
                {
                    var value = (item as IDictionary<string, object>)[key]?.ToString() ?? "";

                    if (value.Contains("\n"))
                    {
                        (item as IDictionary<string, object>)[key] = value;

                    }
                    return CountWords(value);
                });
                columnWidths[colIndex] = CalculateColumnWidth(maxWordCount);
            }
            return columnWidths;
        }
        private int CountWords(string text)
        {
            return string.IsNullOrWhiteSpace(text) ? 0 : text.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }
        private float CalculateColumnWidth(int wordCount)
        {
            //return wordCount <= 7 ? 100 : wordCount / 7f * 100;
            float width = wordCount <= 7 ? 100 : wordCount / 7f * 100;
            return Math.Min(width, 200);
        }
        private int GetColumnCount(List<ExpandoObject> items)
        {
            return items.Count > 0
                ? (items.First() as IDictionary<string, object>).Keys
                    .Count()
                : 0;
        }
        private int CalculatePageWidth(int columnCount, int columnWidth, int basePageWidth)
        {
            int totalColumnWidth = columnCount * columnWidth;
            return Math.Max(700, totalColumnWidth);
        }
        private Document CreateDocument(int pageWidth, int pageHeight)
        {
            //return new Document(new Rectangle(pageWidth, pageHeight), 40, 40, 40, 40);
            return new Document(new Rectangle(595, 842), 40, 40, 40, 40);

        }
        public async Task<string> GetLogoBytesAsync()
        {
            var logoBytes = await _context.Settings.Select(l => l.FilePath).FirstOrDefaultAsync();
            return logoBytes;
        }
        private void AddDocumentHeader(Document pdfDoc, string reportTitle, string userName, string logoBytes = null)
        {
            if (logoBytes != null && logoBytes.Length > 0)
            {
                AddLogoAtTop(pdfDoc, logoBytes);
            }
            AddHeaderInfo(pdfDoc, userName);
            AddTitle(pdfDoc, reportTitle);
        }
        private void AddLogoAtTop(Document pdfDoc, string logoBytes)
        {
            if (logoBytes == null || logoBytes.Length == 0)
                return;


            Image logoImage = Image.GetInstance(logoBytes);

            logoImage.ScaleToFit(160f, 160f);

            logoImage.Alignment = Element.ALIGN_CENTER;

            pdfDoc.Add(logoImage);

            AddSpacing(pdfDoc, 2);
        }
        private void AddSpacing(Document pdfDoc, int lines)
        {
            for (int i = 0; i < lines; i++)
            {
                pdfDoc.Add(new Paragraph(" "));
            }
        }
        private void AddHeaderInfo(Document pdfDoc, string userName)
        {
            var dateLabel = (ReportTranslate.ReportDate);
            var createdByLabel = (ReportTranslate.CreatedBy);

            var dateFont = GetFont(10, Font.NORMAL);
            var dateFormat = $"{dateLabel}: {DateTime.Now:yyyy-MM-dd}";
            var dateLabelCell = new PdfPCell(new Phrase(dateFormat, dateFont))
            {

                Border = Rectangle.NO_BORDER,
                Padding = 10f
            };

            var createdByFont = GetFont(10, Font.NORMAL);
            var createdFormat = $"{createdByLabel}: {userName}";
            var createdLabelCell = new PdfPCell(new Phrase(createdFormat, createdByFont))
            {

                Border = Rectangle.NO_BORDER,
                Padding = 10f
            };

            var titleTable = new PdfPTable(1)
            {
                WidthPercentage = 100,
                RunDirection = PdfWriter.RUN_DIRECTION_RTL
            };
            titleTable.AddCell(dateLabelCell);
            titleTable.AddCell(createdLabelCell);


            pdfDoc.Add(titleTable);

        }

        private void AddTitle(Document pdfDoc, string reportTitle)
        {
            var boldFont = GetFont(16, Font.BOLD);

            var titleTable = new PdfPTable(1)
            {
                WidthPercentage = 100,
                RunDirection = Regex.IsMatch(reportTitle, @"\p{IsArabic}") ? PdfWriter.RUN_DIRECTION_RTL : PdfWriter.RUN_DIRECTION_LTR
            };
            var cell = new PdfPCell(new Phrase(reportTitle, boldFont))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = Rectangle.NO_BORDER,
                Padding = 10f
            };
            Paragraph titleParagraph = new Paragraph(reportTitle, GetFont(20, Font.BOLD));
            titleParagraph.Alignment = Element.ALIGN_CENTER;
            titleTable.AddCell(cell);
            pdfDoc.Add(titleTable);
            AddSpacingButtom(pdfDoc, 4);
        }
        private void AddSpacingButtom(Document pdfDoc, int lines)
        {
            for (int i = 0; i < lines; i++)
            {
                pdfDoc.Add(new Paragraph("\n"));
            }

        }
        private Font GetFont(float size, int style = Font.NORMAL)
        {
            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "Arial.ttf");
            FontFactory.Register(fontPath, "Arial");
            return FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, size, style);
        }
    }
}
