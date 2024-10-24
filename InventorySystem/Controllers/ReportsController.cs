using InventorySystem.Models;
using InventorySystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystem.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportsController(IUnitOfWork unitOfWork)
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

        public async Task<IActionResult> StockReport()
        {
            // Logic for generating stock report
            var stocks = await _unitOfWork.StockReports.GetAll()
                .Include(s => s.Product)
                .Include(s => s.Supplier)
                .ToListAsync();

            return View( stocks);
        }

        public async Task<IActionResult> SupplierReport()
        {
            // Logic for generating supplier report
            var suppliers = await _unitOfWork.Suppliers.GetAll().ToListAsync();
            return View(suppliers);
        }

        [HttpGet]
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
                    var stocks = await _unitOfWork.StockReports.GetAll()
                        .Include(s => s.Product)
                        .Include(s => s.Supplier)
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

                // Create a table based on the type of data
                // Add relevant table cells based on T's properties

                document.Close();
                var file = memoryStream.ToArray();
                return File(file, "application/pdf", "Report.pdf");
            }
        }

        private IActionResult ExportToExcel<T>(List<T> data)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Report");

                // Add header row and data rows based on T's properties

                worksheet.Cells.AutoFitColumns();
                var file = package.GetAsByteArray();
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx");
            }
        }
    }
}
