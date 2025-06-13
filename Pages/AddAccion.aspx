<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddAccion.aspx.cs" MasterPageFile="~/Site.Master" Inherits="Fintual_Task.Pages.AddAccion" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-default">
        <div class="panel-body">
            <div class="row" style="margin-bottom: 10px">
                <div class="col-lg-12 col-md-12 col-xs-12" style="text-align: left">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Large" Text="Agregar Acción al Portafolio"></asp:Label>
                </div>
            </div>

            <!-- Nombre -->
            <div class="row d-flex align-items-center" style="margin-top: 20px">
                <div class="col-lg-3">
                    <asp:Label ID="LabelNombre" runat="server" Text="Nombre Acción" Font-Size="Small" />
                </div>
                <div class="col-lg-9">
                    <asp:TextBox ID="TxtNombre" runat="server" CssClass="form-control" />
                </div>
            </div>

            <!-- Precio -->
            <div class="row d-flex align-items-center" style="margin-top: 20px">
                <div class="col-lg-3">
                    <asp:Label ID="LabelPrecio" runat="server" Text="Precio Unitario" Font-Size="Small" />
                </div>
                <div class="col-lg-9">
                    <asp:TextBox ID="TxtPrecio" runat="server" CssClass="form-control" />
                </div>
            </div>

            <!-- Cantidad -->
            <div class="row d-flex align-items-center" style="margin-top: 20px">
                <div class="col-lg-3">
                    <asp:Label ID="LabelCantidad" runat="server" Text="Cantidad" Font-Size="Small" />
                </div>
                <div class="col-lg-9">
                    <asp:TextBox ID="TxtCantidad" runat="server" CssClass="form-control" />
                </div>
            </div>

            <!-- Mensaje -->
            <asp:Panel ID="PanelMensaje" runat="server" Visible="false" style="margin-top: 20px;">
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
    </div>
</asp:Content>
