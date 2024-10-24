using InventorySystem.Models;
using InventorySystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using Microsoft.AspNetCore.Authorization;
using InventorySystem.Enums;
using Org.BouncyCastle.Asn1.X509.Qualified;
using System.Reflection;
using OfficeOpenXml.Drawing.Chart;

namespace InventorySystem.Controllers
{
    public class ReportController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> ProductReport(string alertLevel)
        {
            try
            {
                var productsQuery = _unitOfWork.Products.GetAll()
                    .Include(p => p.Category)
                    .Include(p => p.Suppliers);

                var products = await productsQuery.ToListAsync();

                ViewBag.Categories = await _unitOfWork.Products.GetAll()
                    .Select(p => p.Category)
                    .Distinct()
                    .ToListAsync();

                return View(products);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        public async Task<IActionResult> StockLevelReport()
        {
            var alerts = new List<AlertReport>();
            var stocks = await _unitOfWork.Products.GetAll()
                .Where(X=> X.AlertLevel > X.Count)
                .ToListAsync();

            if(stocks.Count == 0)
            {
                var reports = await _unitOfWork.AlertLevelReports.GetAllAsync();

                foreach(var report in reports)
                {
                    await _unitOfWork.AlertLevelReports.DeleteAsync(report.Id);
                }
            }
            else
            { 
                foreach (var item in stocks)
                {
                    alerts.Add(new AlertReport
                    {
                        Product = item,
                        Date = DateTime.Now,
                        Status = item.Count == 0 ? AlertStatus.OutOfStock : AlertStatus.warning
                    });
                }
                foreach (var item in alerts)
                {
                    if(!await _unitOfWork.AlertLevelReports.AnyAsync(x=> x.Product.Id == item.Product.Id))
                    { 
                        await _unitOfWork.AlertLevelReports.AddAsync(item);
                    }
                    else if(await _unitOfWork.AlertLevelReports.AnyAsync(x=> x.Status != item.Status))
                    {
                        var reports = await _unitOfWork.AlertLevelReports.FindAsync(x=> x.Product.Id == item.Product.Id);

                        var singleReport = reports.SingleOrDefault();

                        if(singleReport != null) 
                        {  
                            singleReport.Status = item.Status;
                            singleReport.Date = item.Date;
                            await _unitOfWork.AlertLevelReports.UpdateAsync(singleReport);
                        }
                    }
                }
            }           
            return View(alerts);
        }

        public async Task<IActionResult> SupplierReport()
        {
            // Logic for generating supplier report
            var suppliers = await _unitOfWork.Suppliers.GetAll().ToListAsync();
            return View(suppliers);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Export(string format, string reportType)
        {
            MemoryStream stream = new MemoryStream();

            switch (reportType.ToLower())
            {
                case "product":
                    var products = await _unitOfWork.Products.GetAll()
                        .Include(p => p.Category)
                        .Include(p => p.Suppliers)
                        .ToListAsync();

                    return format.ToLower() switch
                    {
                        "pdf" => ExportToPdf(products),
                        "excel" => ExportToExcel(products),
                        _ => BadRequest("Unsupported format")
                    };

                case "stock":
                    var stocks = await _unitOfWork.AlertLevelReports.GetAll()
                        .Include(s => s.Product)
                        .ToListAsync();

                    return format.ToLower() switch
                    {
                        "pdf" => ExportToPdf(stocks),
                        "excel" => ExportToExcel(stocks),
                        _ => BadRequest("Unsupported format")
                    };

                case "supplier":
                    var suppliers = await _unitOfWork.Suppliers.GetAll().ToListAsync();

                    return format.ToLower() switch
                    {
                        "pdf" => ExportToPdf(suppliers),
                        "excel" => ExportToExcel(suppliers),
                        _ => BadRequest("Unsupported format")
                    };

                default:
                    return BadRequest("Unsupported report type");
            }
        }

        private IActionResult ExportToPdf<T>(List<T> data)
        {
            using (var memoryStream = new MemoryStream())
            {
                var document = new Document();
                PdfWriter.GetInstance(document, memoryStream);
                document.Open();
                document.Add(new Paragraph("Report"));
                document.Add(new Paragraph(" "));

                // Create table
                var properties = typeof(T).GetProperties().Where(p => p.PropertyType.IsPrimitive ||
                                                                      p.PropertyType == typeof(string) ||
                                                                      p.PropertyType == typeof(DateTime) ||
                                                                      p.PropertyType == typeof(decimal)).ToList();

                if(typeof(T) == typeof(AlertReport))
                {
                    var productProp = typeof(AlertReport).GetProperty("Product");

                    if(productProp != null)
                    { 
                        var additionalPropType = productProp.PropertyType;

                        var additionalProps = additionalPropType.GetProperties().Where(p=> p.Name == "Name"  || p.Name == "Id").ToList();

                        properties.AddRange(additionalProps);
                    }
                }

                var table = new PdfPTable(properties.Count());
                table.WidthPercentage = 100;

                // Add headers
                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                foreach (var prop in properties)
                {   
                    string headerName = SplitCamelCase(prop.Name);

                    if(typeof(T) == typeof(AlertReport) && prop.Name == "Id" && prop.DeclaringType == typeof(Product))
                    {
                        headerName = "Product Id";
                    }
                    else if(typeof(T) == typeof(AlertReport) && prop.Name == "Name" && prop.DeclaringType == typeof(Product))
                    {
                        headerName = "Product Name";
                    }

                    var cell = new PdfPCell(new Phrase(headerName, headerFont));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }

                // Add data rows
                var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                foreach (var item in data)
                {
                    foreach (var prop in properties)
                    {
                        string value = string.Empty;

                        if(typeof(T) == typeof(AlertReport) && (prop.Name == "Name" || prop.Name == "Id") && prop.DeclaringType == typeof(Product))
                        {
                            var product = typeof(T).GetProperty("Product")?.GetValue(item);

                            if(product != null)
                            {
                                value = product.GetType().GetProperty(prop.Name)?.GetValue(product)?.ToString() ?? "";
                            }
                        }
                        else
                        {
                            value = prop.GetValue(item)?.ToString() ?? "";
                        }

                        var cell = new PdfPCell(new Phrase(value, normalFont));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);
                    }
                }

                document.Add(table);
                document.Close();

                var file = memoryStream.ToArray();
                return File(file, "application/pdf", $"{typeof(T).Name}Report.pdf");
            }
        }

        private IActionResult ExportToExcel<T>(List<T> data)
        {
            using (var memoryStream = new MemoryStream())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(memoryStream))
                {
                    var worksheet = package.Workbook.Worksheets.Add($"{typeof(T).Name} Report");

                    // Get properties to include in the export
                    var properties = typeof(T).GetProperties()
                        .Where(p => p.PropertyType.IsPrimitive || 
                                    p.PropertyType == typeof(string) || 
                                    p.PropertyType == typeof(DateTime) || 
                                    p.PropertyType == typeof(decimal))
                        .ToList();

                    // Add title
                    worksheet.Cells[1, 1].Value = $"{typeof(T).Name} Report";
                    worksheet.Cells[1, 1, 1, properties.Count].Merge = true; // Merge title cells
                    worksheet.Cells[1, 1].Style.Font.Size = 18;
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    // Add headers
                    for (int col = 0; col < properties.Count; col++)
                    {
                        worksheet.Cells[3, col + 1].Value = SplitCamelCase(properties[col].Name);
                        worksheet.Cells[3, col + 1].Style.Font.Bold = true;
                        worksheet.Cells[3, col + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[3, col + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        worksheet.Cells[3, col + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }

                    // Add data rows
                    for (int row = 0; row < data.Count; row++)
                    {
                        for (int col = 0; col < properties.Count; col++)
                        {
                            var value = properties[col].GetValue(data[row]);
                            worksheet.Cells[row + 4, col + 1].Value = value;

                            // Format dates
                            if (value is DateTime)
                            {
                                worksheet.Cells[row + 4, col + 1].Style.Numberformat.Format = "yyyy-mm-dd";
                            }
                            // Format decimals/numbers
                            else if (value is decimal || value is double)
                            {
                                worksheet.Cells[row + 4, col + 1].Style.Numberformat.Format = "#,##0.00";
                            }
                        }
                    }

                    // Auto-fit columns
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    // Finalize and return the Excel file
                    package.Save();
                    return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{typeof(T).Name}Report.xlsx");
                }
            }
        }

        private string SplitCamelCase(string input)
        {
            return string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? " " + x : x.ToString()));
        }
    }
}
