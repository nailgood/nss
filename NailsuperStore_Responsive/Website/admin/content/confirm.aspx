<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="Confirm.aspx.vb" Inherits="admin_content_Confirm" title="Confirmation Page" %>
<asp:Content ID="Content" ContentPlaceHolderID="ph" Runat="Server">

<p>
<font size=4><b>Confirmation Page</b></font>

<p>
The page has been published.

<p>
<% If Not dbPage.PageURL = String.Empty Then%>
<li>Click <a href="<%=dbPage.PageURL %>" target=_blank>here</a> to see it.
<li>Click <a href="/admin/content/edit.aspx?PageId=<%=dbPage.PageId %>">here</a> to edit this page.
<li>Click <a href="/admin/content/pages/">here</a> to go back to edit another page.
<% End If%>


</asp:Content>

