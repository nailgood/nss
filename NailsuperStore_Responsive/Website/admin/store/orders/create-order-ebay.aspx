<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="create-order-ebay.aspx.vb" Inherits="admin_store_orders_create_order_ebay" %>
<%@ Register TagName="OrderDetail" TagPrefix="CC" Src="~/controls/orderebay.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">

<h4>eBay Quick Order</h4>

<span class="smaller">Please provide search criteria below</span>
<asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
<table cellpadding="2" cellspacing="2">

<tr>
		<td class="field">Username:</td>
		<td class="field"><asp:textbox id="F_UserName" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="field">Email:</td>
		<td class="field"><asp:textbox id="F_Email" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td></td>
	</tr>
	
<tr>
<td colspan="2" align="right">
<CC:OneClickButton id="btnSearch" Runat="server" Text="Search" cssClass="btn" />

</td>
</tr>
</table>
</asp:Panel>
<p></p>
<CC:OneClickButton id="Password" Runat="server" Text="Password" Visible="false" cssClass="btn"></CC:OneClickButton>

<p></p>
<div id="divCart" runat="server"  style="width:100%" visible="false">
   
    <div class="border" style="padding:10px 20px">
    <table cellpadding="0" cellspacing="0" >
        <tr>
            <td class="control" style="vertical-align:top">
                               
<!-- TABLE ITEM# -->
    <input type="hidden" id="hdnMore" runat="server"  />
	<table cellspacing="0" cellpadding="2" border="0">
	    <tr>
		    <td colspan="3" style="padding-bottom:10px;">
		     <p>1. Enter the item number, quantity, price, shipping cost, tax and ebay shipping type you want.<br />2. Click &quot;Add to Cart.&quot;<br /></p>
                <asp:Literal runat="server" id="litMsg" Text="<div style='margin:10px 0 0 0;' class='red'>One or more items require further customization. Please make your selection(s) and click the Add To Cart button.</div>" visible="false" />
		    </td>
	    </tr>

        <asp:Repeater runat="server" id="rpt">
	        <HeaderTemplate>
		        <tr>
			        <td style="width:100px;"><strong>Item</strong></td>
			        <td style="width:30px;"><strong>Qty</strong></td>
			        <td style="width:100px;"><strong>Price</strong></td>
			        <td>&nbsp;</td>
		        </tr>
	        </HeaderTemplate>
	        <ItemTemplate>
		        <asp:Literal runat="server" id="trOpen" />
		        <td class="" style="width:150px;padding:2px;">
		            <asp:TextBox runat="server" id="txtitem" CssClass="sbox" Width="150px" />
		         </td>
		        <td class="" style="width:30px;"><asp:TextBox runat="server" id="txtqty" CssClass="sbox-center" Width="30px" MaxLength="4" /></td>
		        <td class="" style="width:100px;"><asp:TextBox runat="server" id="txtPrice" CssClass="sbox-center" Width="100px" MaxLength="6" /></td>
		        <td>
			        <asp:Literal runat="server" id="lit" />
			        <asp:Repeater runat="server" id="rpt">
				        <HeaderTemplate><table><tr></HeaderTemplate>
				        <ItemTemplate><td style="padding-right:8px;"><asp:Label runat="server" id="lblId" Text='<%#Container.DataItem.Id%>' Visible="false" /><strong><%#CType(Container.DataItem, DataLayer.StoreAttributeRow).Name%>:</strong><br /><asp:DropDownList runat="server" id="ddlAttribute" /><br />&nbsp;</td></ItemTemplate>
				        <FooterTemplate></tr></table></FooterTemplate>
			        </asp:Repeater>
		        </td>
		        <asp:Literal runat="server" id="trClose" />
	        </ItemTemplate>
        </asp:Repeater>

    </table>
<!-- TABLE ITEM# -->
            </td>
           
        </tr>
    </table>
    
    <table>

        <tr>
            <td class="optional">Shipping Cost: </td>
            <td class="field"><asp:textbox id="txtShippingCost" runat="server" maxlength="50" columns="50" style="width: 240px;"></asp:textbox></td>
        </tr>
        <tr>
            <td class="optional">Tax: </td>
            <td class="field"><asp:textbox id="txtTax" runat="server" maxlength="50" columns="50" style="width: 240px;"></asp:textbox></td>
        </tr>
        <tr id="trEbayShippingType" runat="server">
                <td class="optional">
                    Ebay Shipping Type:
                </td>
                <td class="field" colspan="2">
                    <asp:DropDownList ID="drEbayShippingType" runat="server">
                        <asp:ListItem Value=""></asp:ListItem>
                        <asp:ListItem Value="USPSPriorityMailSmallFlatRateBox">USPS Priority Mail Small Flat Rate Box</asp:ListItem>
                        <asp:ListItem Value="USPSPriorityFlatRateBox">USPS Priority Mail Medium Flat Rate Box</asp:ListItem>
                        <asp:ListItem Value="USPSPriorityMailLargeFlatRateBox">USPS Priority Mail Large Flat Rate Box</asp:ListItem>
                        <asp:ListItem Value="UPSGround">USP Ground</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
         <tr>
                <td align="left"><br /><asp:Button runat="server" id="btnAdd2Cart" CssClass="btn" Text="Add to cart" /></td>
         </tr>
    </table>
    </div>
</div>

<br />
<asp:Literal runat="server" id="litJS" />


<table id="tblOrder" runat="server" cellpadding="0" cellspacing="0" class="form1" style="width:747px" visible="false">
    <tr>
        <td>
        <h4>Store Order Administration
		<asp:Literal runat="server" ID="litOrderNo"></asp:Literal>
	</h4>
        <CC:OrderDetail runat="server" ID="dtl" />
        </td>
    </tr>
</table>
</asp:Content>

