<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddAccion.aspx.cs" MasterPageFile="~/Site.Master" Inherits="Fintual_Task.Pages.AddAccion" EnableEventValidation="true" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>

    <script type="text/javascript">
        function abrirModalVender(nombreAccion) {
            document.getElementById('<%= HfNombreAccion.ClientID %>').value = nombreAccion;
            $('#modalVender').modal('show');
            return false;
        }
    </script>

    <div class="panel panel-default">
        <div class="panel-body">
            <div class="row">
                <div class="col-lg-6 col-md-6 col-xs-6">
                    <div class="row" style="margin-bottom: 10px">
                        <div class="col-lg-12 col-md-12 col-xs-12" style="text-align: left">
                            <asp:Label ID="LblAccionPortafolio" runat="server" Font-Bold="True" Font-Size="Large" Text="Agregar Acción al Portafolio"></asp:Label>
                        </div>
                    </div>

                    <!-- Nombre -->
                    <div class="row d-flex align-items-center" style="margin-top: 20px">
                        <div class="col-lg-3">
                            <asp:Label ID="LabelNombre" runat="server" Text="Nombre Acción" />
                        </div>
                        <div class="col-lg-9">
                            <asp:DropDownList ID="DdlNombreAccion" runat="server" CssClass="form-control" />
                        </div>
                    </div>

                    <!-- Precio -->
                    <div class="row d-flex align-items-center" style="margin-top: 20px">
                        <div class="col-lg-3">
                            <asp:Label ID="LabelPrecio" runat="server" Text="Precio Unitario" />
                        </div>
                        <div class="col-lg-9">
                            <asp:TextBox ID="TxtPrecio" runat="server" CssClass="form-control" />
                        </div>
                    </div>

                    <!-- Cantidad -->
                    <div class="row d-flex align-items-center" style="margin-top: 20px">
                        <div class="col-lg-3">
                            <asp:Label ID="LabelCantidad" runat="server" Text="Cantidad" />
                        </div>
                        <div class="col-lg-9">
                            <asp:TextBox ID="TxtCantidad" runat="server" CssClass="form-control" />
                        </div>
                    </div>

                    <!-- Mensaje -->
                    <asp:Panel ID="PanelMensaje" runat="server" Visible="false" Style="margin-top: 20px;">
                        <div class="row">
                            <div class="col-lg-12">
                                <asp:Label ID="LblMensaje" runat="server" CssClass="text-success" />
                            </div>
                        </div>
                    </asp:Panel>

                    <!-- Botón -->
                    <div class="row" style="margin-top: 20px">
                        <div class="col-lg-12" style="text-align: left">
                            <asp:Button ID="BtnGuardarAccion" runat="server" CssClass="boton_azul" OnClick="BtnGuardarAccion_Click" Text="Guardar" Width="150px" />
                        </div>
                    </div>
                </div>

                <div class="col-lg-6 col-md-6 col-xs-6" style="margin-top: 20px">
                    <asp:HiddenField ID="HfNombreAccion" runat="server" />
                    <asp:GridView ID="GridViewAcciones" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" Width="100%"
                        OnRowDataBound="GridViewAcciones_RowDataBound" OnRowCommand="GridViewAcciones_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="Nombre" HeaderText="Acción" />
                            <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:C}" />
                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                            <asp:BoundField DataField="ValorTotal" HeaderText="Valor Total" DataFormatString="{0:C}" />
                            <asp:TemplateField HeaderText="Acción">
                                <ItemTemplate>
                                    <asp:Button ID="BtnVender" runat="server" Text="Vender"
                                        CommandName="VenderAccion"
                                        CommandArgument='<%# Eval("Nombre") %>'
                                        Enabled='<%# Convert.ToDecimal(Eval("CantidadVender")) > 0 %>'
                                        CssClass="btn btn-danger btn-sm"
                                        UseSubmitBehavior="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <div class="modal fade" id="modalVender" tabindex="-1" role="dialog" aria-labelledby="modalVenderLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <asp:Panel runat="server" ID="PanelModal" CssClass="modal-content">
                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                <div class="modal-header">
                                    <h5>
                                        <asp:Label ID="LblAccionVender" runat="server" Text="" />
                                    </h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Cerrar">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body">
                                    <asp:Label runat="server" Text="Cantidad a vender:" />
                                    <asp:TextBox runat="server" ID="TxtCantidadVender" CssClass="form-control" /><br />
                                    <asp:Label runat="server" Text="Precio unitario:" />
                                    <asp:TextBox runat="server" ID="TxtPrecioVender" CssClass="form-control" /><br />
                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="BtnConfirmarVenta" CssClass="btn btn-primary" Text="Confirmar Venta" OnClick="BtnConfirmarVenta_Click" />
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
