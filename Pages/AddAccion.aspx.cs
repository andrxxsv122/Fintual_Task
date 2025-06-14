using Fintual_Task.Models;
using Fintual_Task.Negocio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fintual_Task.Pages
{
    public partial class AddAccion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAccionesDdl();
            }
            BindGrid();
        }

        protected void BindGrid()
        {
            var portafolio = Session["Portafolio"] as Portafolio;
            if (portafolio != null)
            {
                var rebalanceo = portafolio.Rebalancear();

                var dataSource = portafolio.Acciones.Select(a => new
                {
                    Nombre = a.Nombre,
                    Precio = a.PrecioActual,
                    Cantidad = a.Cantidad,
                    ValorTotal = a.PrecioActual * a.Cantidad,
                    CantidadVender = rebalanceo.ContainsKey(a.Nombre) && rebalanceo[a.Nombre] < 0 ? (int)Math.Floor(Math.Abs(rebalanceo[a.Nombre] / a.PrecioActual)) : 0
                }).ToList();

                GridViewAcciones.DataSource = dataSource;
                GridViewAcciones.DataBind();
            }
        }


        protected void Rebalanceo()
        {
            if (Session["Portafolio"] != null)
            {
                var portafolio = (Portafolio)Session["Portafolio"];

                portafolio.DistribucionObjetivo = new Dictionary<string, decimal>
                {
                    { "META", 0.4m},
                    { "AAPL", 0.6m}
                };

                var rebalanceo = portafolio.Rebalancear();

                Session["Rebalanceo"] = rebalanceo;

                StringBuilder sb = new StringBuilder();

                foreach (var item in rebalanceo)
                {
                    var accion = portafolio.Acciones.FirstOrDefault(a => a.Nombre == item.Key);
                    decimal precioActual = accion?.PrecioActual ?? 1;

                    decimal cantidad = Math.Round(item.Value / precioActual, 0, MidpointRounding.AwayFromZero);

                    if (cantidad > 0)
                        sb.AppendLine($"Comprar {cantidad} acciones de {item.Key} por ${precioActual} cada una<br />");
                    else if (cantidad < 0)
                        sb.AppendLine($"Vender {Math.Abs(cantidad)} acciones de {item.Key} por ${precioActual} cada una<br />");
                    else
                        sb.AppendLine($"No hacer nada con {item.Key}<br />");
                }

                Mensaje(sb.ToString(), 1);
            }
        }

        protected void BtnGuardarAccion_Click(object sender, EventArgs e)
        {
            string nombre = DdlNombreAccion.SelectedValue;
            decimal precio = decimal.TryParse(TxtPrecio.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal p) ? p : 0;
            int cantidad = int.TryParse(TxtCantidad.Text, out int c) ? c : 0;

            if (nombre == "-1" || precio <= 0 || cantidad <= 0)
            {
                Mensaje("Todos los campos deben contener valores válidos.", 0);
                return;
            }

            var portafolio = Session["Portafolio"] as Portafolio ?? new Portafolio();

            Accion accion = new Accion(nombre, precio, cantidad);
            bool accionAñadida = portafolio.AgregarAccion(accion);

            Session["Portafolio"] = portafolio;

            if (accionAñadida)
            {
                Mensaje("Acción añadida.", 1);
                ClearInputs();

                Rebalanceo();
                BindGrid();
            }
            else
            {
                Mensaje("Error al agregar la acción.", 0);
            }
        }
        protected void BtnConfirmarVenta_Click(object sender, EventArgs e)
        {
            string nombre = HfNombreAccion.Value;
            if (string.IsNullOrEmpty(nombre))
            {
                Mensaje("Error al recuperar la acción.", 0);
                return;
            }

            if (!int.TryParse(TxtCantidadVender.Text, out int cantidad) || cantidad <= 0)
            {
                Mensaje("Ingrese una cantidad válida.", 0);
                return;
            }

            var portafolio = Session["Portafolio"] as Portafolio;
            if (portafolio == null)
                return;

            var accion = portafolio.Acciones.FirstOrDefault(a => a.Nombre == nombre);
            if (accion == null)
                return;

            if (cantidad > accion.Cantidad)
            {
                Mensaje("No puede vender más acciones de las que posee.", 0);
                return;
            }

            accion.Cantidad -= cantidad;

            if (accion.Cantidad == 0)
            {
                portafolio.Acciones.Remove(accion);
            }

            Session["Portafolio"] = portafolio;
            Mensaje($"Se vendieron {cantidad} acciones de {nombre}.", 1);

            Rebalanceo();
            BindGrid();
        }

        protected void GridViewAcciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var rebalanceo = Session["Rebalanceo"] as Dictionary<string, decimal>;
                var portafolio = Session["Portafolio"] as Portafolio;
                if (rebalanceo == null || portafolio == null) return;

                string nombreAccion = DataBinder.Eval(e.Row.DataItem, "Nombre").ToString();

                if (rebalanceo.TryGetValue(nombreAccion, out decimal valor))
                {
                    var accion = portafolio.Acciones.FirstOrDefault(a => a.Nombre == nombreAccion);

                    if (accion != null)
                    {
                        int cantidadAVender = (int)Math.Ceiling(Math.Abs(valor) / accion.PrecioActual);

                        Button btnVender = e.Row.FindControl("BtnVender") as Button;
                        if (btnVender != null)
                        {
                            btnVender.Enabled = valor < 0 && cantidadAVender > 0;
                        }
                    }
                }
            }
        }

        protected void GridViewAcciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "VenderAccion")
            {
                string nombreAccion = e.CommandArgument.ToString();

                HfNombreAccion.Value = nombreAccion;

                ViewState["Accion"] = nombreAccion;
                LblAccionVender.Text = $"Vender acción: {nombreAccion}";
                TxtCantidadVender.Text = "";
                TxtPrecioVender.Text = "";

                MostrarModalVender();
            }
        }

        private void MostrarModalVender()
        {
            string script = @"$('#modalVender').modal('show');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MostrarModal", script, true);
        }

        protected void ClearInputs()
        {
            DdlNombreAccion.SelectedValue = "-1";
            TxtPrecio.Text = String.Empty;
            TxtCantidad.Text = String.Empty;
        }

        protected void Mensaje(string mensaje, int tipo)
        {
            PanelMensaje.Visible = true;
            LblMensaje.Text = mensaje;

            if (tipo == 1) LblMensaje.CssClass = "text-success";
            else LblMensaje.CssClass = "text-danger";
        }

        protected void LoadAccionesDdl()
        {
            List<string> acciones = new List<string> { "META", "AAPL" };
            DdlNombreAccion.DataSource = acciones;
            DdlNombreAccion.DataBind();
            DdlNombreAccion.Items.Insert(0, new ListItem("Seleccione una acción", "-1"));
        }
    }
}