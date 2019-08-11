using System.Text;
using QuimiOSHub.Models;

namespace QuimiOSHub.Services
{
    /// <summary>
    /// Generates realistic ASP.NET WebForms HTML markup for mock QuimiOS endpoints.
    /// Mimics the legacy LIMS interface structure for testing and development.
    /// </summary>
    public class WebFormsHtmlGenerator
    {
        private const string ViewStateGenerator = "81A526C4";
        private const string EventValidation = "/wEWMgKl4pa4DwL66ZujDwKP962oBQLLz7SHAgKgodW7AwLEoJ7pDgLFoJ7pDgLGoJ7pDgLHoJ7pDgLBoJ7pDgLcoJ7pDgLlwMuZCQKOrqqlCALvr6H0BQLur8H3BQLur5H0BQLvr+H3BQL3k7+XDQLqr6n0BQKdqZFgAumvkfQFAuivkfQFAuqvmfQFAu+vmfQFArvAiv0GAu+vrfQFAu6vzfcFAuiv4fcFAuqvrfQFAvKv4fcFAuivzfcFAumvmfQFAuqvkfQFAsn8xb0HAuuvwfcFAuuvzfcFAu6vofQFAqHX7NYMAuyv4fcFAuuvlfQFAsirxowDAqPFp7ACAsbE7OIPAsXE7OIPAoa3nbABAr3K1bwLAvmowt8FAsy51WsC1KqatgUC4+nN4gw6S2SJyBOscYw4C1/0ibj3kG7oUQ==";

        public string GenerateInventoryGridPage(List<InventoryItem> inventoryItems, DateTime date)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("    <title>Consumo de Reactivos - QuimiOS</title>");
            sb.AppendLine("    <meta charset='utf-8' />");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("    <form name='form1' method='post' action='/Inventarios/ConsumoReacLabMasivo.aspx' id='form1'>");

            // ASP.NET WebForms state fields
            AppendWebFormsStateFields(sb);

            // Page header
            sb.AppendLine("        <div>");
            sb.AppendLine("            <h2>Consumo de Reactivos por Equipo</h2>");
            sb.AppendLine($"            <p>Fecha: {date:dd/MM/yyyy}</p>");
            sb.AppendLine("        </div>");

            // Grid container
            sb.AppendLine("        <div id='ctl00_ContentMasterPage_UpdatePanel1'>");
            sb.AppendLine("            <table class='gridview' id='ctl00_ContentMasterPage_grdConsumo' cellspacing='0' rules='all' border='1' style='border-collapse:collapse;'>");

            // Grid header
            AppendGridHeader(sb);

            // Grid rows
            AppendGridRows(sb, inventoryItems);

            sb.AppendLine("            </table>");
            sb.AppendLine("        </div>");

            // Hidden fields and buttons
            AppendHiddenFieldsAndButtons(sb, date);

            sb.AppendLine("    </form>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        private void AppendWebFormsStateFields(StringBuilder sb)
        {
            sb.AppendLine($"        <input type='hidden' name='__VIEWSTATEGENERATOR' id='__VIEWSTATEGENERATOR' value='{ViewStateGenerator}' />");
            sb.AppendLine($"        <input type='hidden' name='__EVENTVALIDATION' id='__EVENTVALIDATION' value='{EventValidation}' />");
            sb.AppendLine("        <input type='hidden' name='__EVENTTARGET' id='__EVENTTARGET' value='' />");
            sb.AppendLine("        <input type='hidden' name='__EVENTARGUMENT' id='__EVENTARGUMENT' value='' />");
            sb.AppendLine("        <input type='hidden' name='ctl00_treePrincipal_ExpandState' value='eunnnnnnnnnnnnnunnnnnnnnnnnnnnunnnnunnnnnnnnnnnennnunnun' />");
            sb.AppendLine("        <input type='hidden' name='ctl00_treePrincipal_SelectedNode' value='ctl00_treePrincipalt48' />");
        }

        private void AppendGridHeader(StringBuilder sb)
        {
            sb.AppendLine("                <tr>");
            sb.AppendLine("                    <th scope='col'><input name='ctl00$ContentMasterPage$grdConsumo$ctl01$chkProvAll' type='checkbox' checked='checked' /></th>");
            sb.AppendLine("                    <th scope='col'>ID Producto</th>");
            sb.AppendLine("                    <th scope='col'>Código</th>");
            sb.AppendLine("                    <th scope='col'>Producto</th>");
            sb.AppendLine("                    <th scope='col'>Pacientes</th>");
            sb.AppendLine("                    <th scope='col'>Repeticiones</th>");
            sb.AppendLine("                    <th scope='col'>Control</th>");
            sb.AppendLine("                    <th scope='col'>Calibración</th>");
            sb.AppendLine("                    <th scope='col'>Cancelación</th>");
            sb.AppendLine("                    <th scope='col'>Motivo Cancel</th>");
            sb.AppendLine("                    <th scope='col'>Validación</th>");
            sb.AppendLine("                    <th scope='col'>Sin Identificar</th>");
            sb.AppendLine("                    <th scope='col'>Motivo Sin ID</th>");
            sb.AppendLine("                    <th scope='col'>Existencia Inicial</th>");
            sb.AppendLine("                    <th scope='col'>Existencia Final</th>");
            sb.AppendLine("                    <th scope='col'>Usuario</th>");
            sb.AppendLine("                </tr>");
        }

        private void AppendGridRows(StringBuilder sb, List<InventoryItem> inventoryItems)
        {
            int rowIndex = 2; // Start at ctl02 (ctl01 is header)

            foreach (var item in inventoryItems.OrderBy(i => i.Reagent!.Code))
            {
                var rowNum = rowIndex.ToString("D2");

                sb.AppendLine($"                <tr class='gridrow'>");

                // Row selection checkbox (first column)
                sb.AppendLine($"                    <td><input name='ctl00$ContentMasterPage$grdConsumo$ctl{rowNum}$chkQueProveedor' type='checkbox' checked='checked' /></td>");

                // Product ID (second column, visible)
                sb.AppendLine($"                    <td>{item.ReagentId}</td>");

                // Reagent code with proper ID pattern for parsing
                sb.AppendLine($"                    <td><span class='wd090' id='ctl00_ContentMasterPage_grdConsumo_ctl{rowNum}_lblCodProducto'>{item.Reagent!.Code}</span></td>");

                // Product name
                sb.AppendLine($"                    <td>{item.Reagent.Name}</td>");

                // Consumption input fields
                sb.AppendLine($"                    <td><input name='ctl00$ContentMasterPage$grdConsumo$ctl{rowNum}$txtPacientes' type='text' value='0' /></td>");
                sb.AppendLine($"                    <td><input name='ctl00$ContentMasterPage$grdConsumo$ctl{rowNum}$txtRepeticiones' type='text' value='0' /></td>");
                sb.AppendLine($"                    <td><input name='ctl00$ContentMasterPage$grdConsumo$ctl{rowNum}$txtControlCapMGrd' type='text' value='0' /></td>");
                sb.AppendLine($"                    <td><input name='ctl00$ContentMasterPage$grdConsumo$ctl{rowNum}$txtCalibracionCapMGrd' type='text' value='0' /></td>");
                sb.AppendLine($"                    <td><input name='ctl00$ContentMasterPage$grdConsumo$ctl{rowNum}$txtCancelacionCapMGrd' type='text' value='0' /></td>");
                sb.AppendLine($"                    <td><select name='ctl00$ContentMasterPage$grdConsumo$ctl{rowNum}$cmbMotCancelacionGrd'><option value='[Seleccione]'>[Seleccione]</option></select></td>");
                sb.AppendLine($"                    <td><input name='ctl00$ContentMasterPage$grdConsumo$ctl{rowNum}$txtValidacionCapMGrd' type='text' value='0' /></td>");
                sb.AppendLine($"                    <td><input name='ctl00$ContentMasterPage$grdConsumo$ctl{rowNum}$txtSinIdentificarCapMGrd' type='text' value='0' /></td>");
                sb.AppendLine($"                    <td><select name='ctl00$ContentMasterPage$grdConsumo$ctl{rowNum}$cmbMotSinIdentificarGrd'><option value='[Seleccione]'>[Seleccione]</option></select></td>");

                // Stock tracking (last 3 columns)
                sb.AppendLine($"                    <td><span id='ctl00_ContentMasterPage_grdConsumo_ctl{rowNum}_lblExistInicial'>{item.CurrentStock}</span></td>");
                sb.AppendLine($"                    <td><span id='ctl00_ContentMasterPage_grdConsumo_ctl{rowNum}_lblExistFinal'>{item.CurrentStock}</span></td>");
                sb.AppendLine($"                    <td><span id='ctl00_ContentMasterPage_grdConsumo_ctl{rowNum}_lblUsuario'>admin</span></td>");

                // Hidden product ID
                sb.AppendLine($"                    <input type='hidden' name='ctl00$ContentMasterPage$grdConsumo$ctl{rowNum}$hfIDProducto' id='ctl00_ContentMasterPage_grdConsumo_ctl{rowNum}_hfIDProducto' value='{item.ReagentId}' />");

                sb.AppendLine("                </tr>");

                rowIndex++;
            }
        }

        private void AppendHiddenFieldsAndButtons(StringBuilder sb, DateTime date)
        {
            sb.AppendLine("        <div>");
            sb.AppendLine($"            <input type='hidden' name='ctl00$ContentMasterPage$txtDesdeB' value='{date:dd/MM/yyyy}' />");
            sb.AppendLine("            <input type='hidden' name='ctl00$ContentMasterPage$ddlSuc' value='2' />");
            sb.AppendLine("            <input type='hidden' name='ctl00$ContentMasterPage$cmbEquipo' value='6' />");
            sb.AppendLine("            <input type='hidden' name='ctl00$ContentMasterPage$cmbMesaOrdenac' value='2' />");
            sb.AppendLine("            <input type='hidden' name='ctl00$ContentMasterPage$hfActivo' value='0' />");
            sb.AppendLine("            <input type='hidden' name='ctl00$ContentMasterPage$hfCalcAuto' value='1' />");
            sb.AppendLine("            <input type='submit' name='ctl00$ContentMasterPage$btnGuardaMasivo' value='Guardar Consumo' />");
            sb.AppendLine("        </div>");
        }

        public string GenerateSuccessPage(string message = "Consumo guardado exitosamente")
        {
            var sb = new StringBuilder();

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("    <title>QuimiOS - Operación Exitosa</title>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("    <div>");
            sb.AppendLine($"        <h2>{message}</h2>");
            sb.AppendLine("        <p>La operación se completó correctamente.</p>");
            sb.AppendLine("    </div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        public string GenerateErrorPage(List<string> errors)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("    <title>QuimiOS - Error</title>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("    <div>");
            sb.AppendLine("        <h2>Error al procesar la solicitud</h2>");
            sb.AppendLine("        <ul>");

            foreach (var error in errors)
            {
                sb.AppendLine($"            <li>{error}</li>");
            }

            sb.AppendLine("        </ul>");
            sb.AppendLine("    </div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }
    }
}
