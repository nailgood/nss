<%@ Page Language="vb" AutoEventWireup="false" Inherits="member_reminders_default" CodeFile="default.aspx.vb" MasterPageFile="~/includes/masterpage/interior.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
<p>
    <asp:Button id="btnAdd" runat="server" CssClass="btn btn-submit" alternatetext="Add new entry" Text="Add New Entry" ></asp:Button>
</p>
<!-- cart table -->
<div id="reminder" runat="server">
<asp:repeater id="rptReminders" EnableViewState="False" runat="server">
    <HeaderTemplate>
       <table cellspacing="1" cellpadding="1" border="0" class="tbl-order" summary="shopping cart table">
        <tr>
            <td class="header">Edit</td>
	        <td class="header">Delete</td>
	        <td class="header h-sm">Name</td>
	        <td class="header">Recurs?</td>	        
	        <td class="header h-sm">1st Reminder</td>
	        <td class="header h-sm">2nd Reminder</td>
            <td class="header">Date</td>
	        </tr>      
        </HeaderTemplate>
        <ItemTemplate>
            <!-- row-->
	        <tr valign="top">
	            <td>
		            <asp:Button id="btnEdit" runat="server" commandname="Edit" CssClass="c-button list-button"  Text="Edit" />
	            </td>
	            <td>
		            <asp:Button id="btnDelete" runat="server" commandname="Remove" OnClientClick="if (!confirm('Are you sure you want delete?')) return false;" CssClass="c-button list-button"  Text="Delete" />
	            </td>
	            <td class="h-sm">
		            <asp:literal id="ltlName" runat="server" />
	            </td>
	            <td>
		            <asp:literal id="ltlRecurs" runat="server" />
	            </td>
	            <td class="h-sm">
		            <asp:literal id="ltlFirstReminder" runat="server" />
	            </td>
	            <td class="h-sm">
		            <asp:literal id="ltlSecondReminder" runat="server" />
	            </td>
                <td>
		            <asp:literal id="ltlDate" runat="server" />
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