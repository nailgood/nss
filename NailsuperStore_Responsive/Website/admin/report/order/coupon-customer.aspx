<%@ Page Language="VB" AutoEventWireup="false" validateRequest="false" CodeFile="coupon-customer.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_report_order_coupon_customer" %>
<asp:content ContentPlaceHolderId="ph" ID="Content1" runat="server">

    <script language="javascript">
<!--
	function expandit(objid){
		var span = document.getElementById('SPAN' + objid).style;
		var img  = document.getElementById('IMG' + objid);
		var imgtext = document.getElementById('imgtext');
		if (span.display=="none") {
			span.display="block"
			img.src = img.src.replace(/down/i, "up");
			imgtext.innerText = 'Hide Image';
		} else {
			span.display="none"
			img.src = img.src.replace(/up/i, "down");
			imgtext.innerText = 'View Image';
		}
	}
	function exportXls()
    {	
	 document.getElementById("ctl00_ph_hidexport").value = document.getElementById("Excel").innerHTML
     return true;
    }
    
//-->
</script>


<h4>Promotions Report</h4>

<p></p>
<table cellpadding="2" cellspacing="2">
<tr>
<th valign="top">Promotion Code:</th>
<td valign="top" class="field"><asp:textbox id="F_PromotionCode" runat="server" Columns="20" MaxLength="20"></asp:textbox></td>
</tr>
<tr>
<th valign="top">Customer No:</th>
<td valign="top" class="field"><asp:textbox id="F_CustomerNo" runat="server" Columns="20" MaxLength="20"></asp:textbox></td>
</tr>
<tr>
<th valign="top"><b>Customer Name:</b></th>
<td valign="top" class="field"><asp:textbox id="F_CustomerName" runat="server" Columns="20" MaxLength="20"></asp:textbox></asp:dropdownlist></td>

</tr>

<tr><td style="padding:10px 0px 10px 0px;"><CC:OneClickButton id="btnSearch" Runat="server" Text="Search" cssClass="btn" /></td>
<td style="padding:10px 0px 10px 0px;"><CC:OneClickButton id="btnExcel" Runat="server" Text="Excel" cssClass="btn" /></td>

</tr>
</table>
<div id="Excel">
<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>	
		
		<asp:BoundField SortExpression="PromotionCode" DataField="PromotionCode" HeaderText="Promotion Code"></asp:BoundField>
       <asp:BoundField SortExpression="CustomerNo" DataField="CustomerNo" HeaderText="Customer No"></asp:BoundField>
       <asp:BoundField SortExpression="Name" DataField="Name" HeaderText="Customer Name"></asp:BoundField>
	   <asp:BoundField SortExpression="UsedTimes" DataField="UsedTimes" HeaderText="Used Times"></asp:BoundField>

	</Columns>
    <HeaderStyle VerticalAlign="Top" />
</CC:GridView>
</div> 
<INPUT id="hidexport" type="hidden" name="hidexport" runat="server"/>
</asp:content>