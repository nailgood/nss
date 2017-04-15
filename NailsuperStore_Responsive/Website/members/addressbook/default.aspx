<%@ Page Language="vb" AutoEventWireup="false" Inherits="member_addressbook_default" MasterPageFile="~/includes/masterpage/interior.master"  CodeFile="default.aspx.vb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
<h1></h1>
<CT:ErrorMessage id="ErrorPlaceHolder" runat="server"/>


<!-- cart table -->
<p>
    <asp:Button id="btnAdd" runat="server" CssClass="btn btn-submit" alternatetext="Retrieve Password" Text="Add New Entry" />
</p>
<div id="addressbook" runat="server">

<asp:repeater id="rptAddressBook" EnableViewState="False" runat="server">
    <HeaderTemplate>
        <table cellspacing="1" cellpadding="1" class="tbl-order" summary="shopping cart table">
            <tr>
                <td class="header">Edit</td>
	            <td class="header">Delete</td>
	            <td class="header hidden-xs">Label</td>
	            <td class="header hidden-xs">Name</td>	        
	            <td class="header">Address</td>
	        </tr>      
    </HeaderTemplate>
    <ItemTemplate>
        <tr valign="top">
	        <td class="center">
		         <asp:Button id="btnEdit" runat="server" CssClass="c-button list-button" alternatetext="Edit" Text="Edit" commandname="Edit" />
	        </td>
	        <td>
		        <asp:Button id="btnDelete" runat="server" commandname="Remove" OnClientClick="if (!confirm('Are you sure you want delete?')) return false;" CssClass="c-button list-button" alternatetext="Delete" Text="Delete" />
	        </td>
	        <td class="hidden-xs">
		        <asp:literal id="ltlLabel" runat="server" />
	        </td>
	        <td class="hidden-xs">
		        <asp:literal id="ltlName" runat="server" />
	        </td>
	        <td>
		        <asp:literal id="ltlAddress" runat="server" />
	        </td>
	    </tr>    
	     
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
    </asp:repeater>
</div>
     
<div id="divNoItems" runat="server">
    <asp:literal id="ltlNoItems" runat="server" />
</div>
</asp:Content>