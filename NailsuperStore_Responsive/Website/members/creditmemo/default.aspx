<%@ Page Language="vb" AutoEventWireup="false" Inherits="member_creditmemo_default" MasterPageFile="~/includes/masterpage/interior.master" CodeFile="default.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
<center>

<div id="page">



<div id="credit-memo" preventDefaultButton="ctl02_btnSearch">
<asp:repeater id="rptOrderHistory" EnableViewState="False" runat="server">
    <HeaderTemplate>
       <table cellspacing="1" cellpadding="1" class="tbl-order" summary="shopping cart table">
        <tr>
            <td class="header" >Details</td>
	        <td class="header" >Credit #</td>
	        <td class="header hidden-xs">Billing Name</td>	        
	        <td class="header hidden-xs">Total</td>
	        <td class="header">Return Date</td>
	        </tr>      
        </HeaderTemplate>
        <ItemTemplate>
            <!-- row-->
	        <tr valign="top" class="alternate">
	            <td class="">
		            <asp:Button id="btnDetails" runat="server" commandname="Details" CssClass="c-button list-button"  Text="Details" />
	            </td>
	            <td>
		            <asp:literal id="ltlOrderNo" runat="server" />
	            </td>
	            <td class="left hidden-xs">
		            <asp:literal id="ltlBillingName" runat="server" />
	            </td>
	            <td class="hidden-xs">
		            <asp:literal id="ltlTotal" runat="server" />
	            </td>
	            <td class="">
		            <asp:literal id="ltlPurchaseDate" runat="server" />
	            </td>
	        </tr>      
        </ItemTemplate>
        <FooterTemplate>
                
            </table>
        </FooterTemplate>
    </asp:repeater>

<div id="divEmpty" runat="server" visible="false" style="text-align:center">No records in this list</div>

</div>

</div>

</center>
</asp:Content>