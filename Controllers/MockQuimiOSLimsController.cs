using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuimiOSHub.Data;
using QuimiOSHub.Models;
using QuimiOSHub.Services;
using System.Text.RegularExpressions;

namespace QuimiOSHub.Controllers
{
    /// <summary>
    /// Mock QuimiOS LIMS endpoints for inventory management.
    /// Simulates ASP.NET WebForms interface for development and testing.
    /// </summary>
    [ApiController]
    [Route("mock-quimios/inventarios")]
    public class MockQuimiOSLimsController : ControllerBase
    {
        private readonly QuimiosDbContext _context;
        private readonly WebFormsHtmlGenerator _htmlGenerator;

        public MockQuimiOSLimsController(QuimiosDbContext context, WebFormsHtmlGenerator htmlGenerator)
        {
            _context = context;
            _htmlGenerator = htmlGenerator;
        }

        /// <summary>
        /// GET: Generates inventory grid HTML from database.
        /// Simulates ConsumoReacLabMasivo.aspx page with current stock levels.
        /// </summary>
        [HttpGet("ConsumoReacLabMasivo.aspx")]
        [Produces("text/html")]
        public async Task<IActionResult> GetInventoryGrid(
            [FromQuery] string? txtFecDesde,
            [FromQuery] string? txtFecHasta,
            [FromQuery] string? cmbEquipo,
            [FromQuery] string? ddlSuc)
        {
            DateTime date = DateTime.Now;
            if (!string.IsNullOrEmpty(txtFecDesde))
            {
                DateTime.TryParseExact(txtFecDesde, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out date);
            }

            var inventoryItems = await _context.InventoryItems
                .Include(i => i.Reagent)
                .Where(i => i.IsActive && i.Reagent != null)
                .OrderBy(i => i.Reagent!.Code)
                .ToListAsync();

            var html = _htmlGenerator.GenerateInventoryGridPage(inventoryItems, date);
            return Content(html, "text/html");
        }

        /// <summary>
        /// POST: Processes consumption submission, updates inventory.
        /// Validates WebForms fields, creates consumption records and movements.
        /// </summary>
        [HttpPost("ConsumoReacLabMasivo.aspx")]
        [Produces("text/html")]
        public async Task<IActionResult> SubmitConsumption([FromForm] IFormCollection formData)
        {
            var errors = new List<string>();

            // Validate ASP.NET WebForms required fields
            ValidateWebFormsFields(formData, errors);

            // Validate business context
            ValidateBusinessContext(formData, errors);

            // Parse and validate grid data
            var gridConsumptions = ParseGridData(formData, errors);

            if (errors.Any())
            {
                var errorHtml = _htmlGenerator.GenerateErrorPage(errors);
                return Content(errorHtml, "text/html");
            }

            // Process consumption and update inventory
            try
            {
                await ProcessConsumptionAsync(gridConsumptions, formData);
                var successHtml = _htmlGenerator.GenerateSuccessPage("Consumo guardado exitosamente");
                return Content(successHtml, "text/html");
            }
            catch (Exception ex)
            {
                var errorHtml = _htmlGenerator.GenerateErrorPage(new List<string> { ex.Message });
                return Content(errorHtml, "text/html");
            }
        }

        private void ValidateWebFormsFields(IFormCollection formData, List<string> errors)
        {
            // ViewState validation key
            if (!formData.ContainsKey("__VIEWSTATEGENERATOR"))
                errors.Add("Missing ViewState generator");

            // Event routing fields
            if (!formData.ContainsKey("__EVENTTARGET"))
                errors.Add("Missing event target");

            // ScriptManager for AJAX partial postback
            if (!formData.ContainsKey("ctl00$ContentMasterPage$ScriptManager1"))
                errors.Add("Missing ScriptManager (AJAX postback required)");

            // Button that triggered the save
            if (!formData.ContainsKey("ctl00$ContentMasterPage$btnGuardaMasivo"))
                errors.Add("Missing save button trigger");
        }

        private void ValidateBusinessContext(IFormCollection formData, List<string> errors)
        {
            // Branch/location validation
            if (!formData.ContainsKey("ctl00$ContentMasterPage$ddlSuc"))
                errors.Add("Missing branch/location (ddlSuc)");

            // Equipment ID validation
            if (!formData.ContainsKey("ctl00$ContentMasterPage$cmbEquipo"))
                errors.Add("Missing equipment ID");
            else if (formData["ctl00$ContentMasterPage$cmbEquipo"] != "6")
                errors.Add("Invalid equipment ID (expected ARCHITECT c8000)");

            // Workstation ID
            if (!formData.ContainsKey("ctl00$ContentMasterPage$cmbMesaOrdenac"))
                errors.Add("Missing workstation ID");

            // Date validation
            if (!formData.ContainsKey("ctl00$ContentMasterPage$txtDesdeB"))
                errors.Add("Missing consumption date");
            else
            {
                var dateStr = formData["ctl00$ContentMasterPage$txtDesdeB"].ToString();
                if (!Regex.IsMatch(dateStr, @"^\d{2}/\d{2}/\d{4}$"))
                    errors.Add("Invalid date format (expected dd/MM/yyyy)");
            }
        }

        private List<GridConsumption> ParseGridData(IFormCollection formData, List<string> errors)
        {
            var consumptions = new List<GridConsumption>();

            // Find all grid rows by looking for product ID fields
            var gridPattern = new Regex(@"^ctl00\$ContentMasterPage\$grdConsumo\$ctl(\d{2})\$hfIDProducto$");

            foreach (var key in formData.Keys)
            {
                var match = gridPattern.Match(key);
                if (!match.Success) continue;

                var rowNumber = match.Groups[1].Value;
                var prefix = $"ctl00$ContentMasterPage$grdConsumo$ctl{rowNumber}";

                var consumption = new GridConsumption
                {
                    RowNumber = rowNumber,
                    ProductId = int.Parse(formData[$"{prefix}$hfIDProducto"].ToString())
                };

                // Parse consumption values with validation
                if (formData.ContainsKey($"{prefix}$txtPacientes") &&
                    int.TryParse(formData[$"{prefix}$txtPacientes"], out int px))
                    consumption.Patients = px;

                if (formData.ContainsKey($"{prefix}$txtRepeticiones") &&
                    int.TryParse(formData[$"{prefix}$txtRepeticiones"], out int rep))
                    consumption.Repeats = rep;

                if (formData.ContainsKey($"{prefix}$txtControlCapMGrd") &&
                    int.TryParse(formData[$"{prefix}$txtControlCapMGrd"], out int qc))
                    consumption.QC = qc;

                if (formData.ContainsKey($"{prefix}$txtCalibracionCapMGrd") &&
                    int.TryParse(formData[$"{prefix}$txtCalibracionCapMGrd"], out int cal))
                    consumption.Calibrations = cal;

                if (formData.ContainsKey($"{prefix}$txtCancelacionCapMGrd") &&
                    int.TryParse(formData[$"{prefix}$txtCancelacionCapMGrd"], out int canc))
                    consumption.Cancellations = canc;

                if (formData.ContainsKey($"{prefix}$cmbMotCancelacionGrd"))
                    consumption.CancellationReason = formData[$"{prefix}$cmbMotCancelacionGrd"].ToString();

                consumptions.Add(consumption);
            }

            if (!consumptions.Any())
                errors.Add("No consumption data found in grid");

            return consumptions;
        }

        private async Task ProcessConsumptionAsync(List<GridConsumption> gridConsumptions, IFormCollection formData)
        {
            var dateStr = formData["ctl00$ContentMasterPage$txtDesdeB"].ToString();
            var consumptionDate = DateTime.ParseExact(dateStr, "dd/MM/yyyy", null);

            foreach (var gridConsumption in gridConsumptions)
            {
                var inventoryItem = await _context.InventoryItems
                    .Include(i => i.Reagent)
                    .FirstOrDefaultAsync(i => i.Id == gridConsumption.ProductId);

                if (inventoryItem?.Reagent == null)
                    continue;

                // Calculate total consumption
                var totalConsumption = gridConsumption.Patients + gridConsumption.Repeats +
                                     gridConsumption.QC + gridConsumption.Calibrations;

                if (totalConsumption == 0)
                    continue;

                // Check sufficient stock
                if (inventoryItem.CurrentStock < totalConsumption)
                    throw new InvalidOperationException($"Insufficient stock for {inventoryItem.Reagent.Code}: {inventoryItem.CurrentStock} available, {totalConsumption} required");

                // Create consumption record
                var consumptionRecord = new ConsumptionRecord
                {
                    ReagentId = inventoryItem.Reagent.Id,
                    ConsumptionDate = consumptionDate,
                    ResearchConsumption = gridConsumption.Patients,
                    RepeatConsumption = gridConsumption.Repeats,
                    QCConsumption = gridConsumption.QC,
                    ManualConsumption = 0,
                    CalibrationConsumption = gridConsumption.Calibrations,
                    TotalConsumption = totalConsumption
                };

                _context.ConsumptionRecords.Add(consumptionRecord);
                await _context.SaveChangesAsync();

                // Create inventory movement
                var movement = new InventoryMovement
                {
                    InventoryItemId = inventoryItem.Id,
                    MovementType = "OUT",
                    Quantity = totalConsumption,
                    Reference = $"Consumption {consumptionDate:yyyy-MM-dd}",
                    Notes = $"Px:{gridConsumption.Patients} Rep:{gridConsumption.Repeats} QC:{gridConsumption.QC} Cal:{gridConsumption.Calibrations}",
                    MovementDate = consumptionDate,
                    ConsumptionRecordId = consumptionRecord.Id
                };

                _context.InventoryMovements.Add(movement);

                // Update current stock
                inventoryItem.CurrentStock -= totalConsumption;
            }

            await _context.SaveChangesAsync();
        }

        private class GridConsumption
        {
            public string RowNumber { get; set; } = string.Empty;
            public int ProductId { get; set; }
            public int Patients { get; set; }
            public int Repeats { get; set; }
            public int QC { get; set; }
            public int Calibrations { get; set; }
            public int Cancellations { get; set; }
            public string CancellationReason { get; set; } = "[Seleccione]";
        }
    }
}
