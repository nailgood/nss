<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="edit.aspx.vb" Inherits="admin_catalog_request_edit" title="" %>
<%@Register TagPrefix="AE" TagName="RequestCatalog" Src="~/Controls/RequestCatalog.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ph" Runat="Server">
	<h4>Catalog - Add / Edit Catalog Request</h4>
	<AE:RequestCatalog id="CatalogRequest" runat="server" />
</asp:Content>