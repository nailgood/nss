<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="" CodeFile="image.aspx.vb" Inherits="admin_store_items_images_image"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
    <h4>Image Preview - Alternate Images</h4>
    <p>
        <asp:Label Runat="server" ID="lblThumb"></asp:Label><br>
        <asp:Image ID="Thumb" Runat="server" />
    <p>
        <asp:Button ID="btnBack" Runat="server" CausesValidation="False" CssClass="btn" Text="&laquo;&laquo; Back to List" />
</asp:content>