<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_pricematch_Edit"  Title="Price Match"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If PriceMatchId = 0 Then %>Add<% Else %>Edit<% End If %> Price Match</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan=2><span class=smallest><span class=red>red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Your Name:</td>
		<td class="field"><asp:Label id="txtYourName" runat="server"></asp:Label></td>
	</tr>
	<tr>
		<td class="required">Phone Number:</td>
		<td class="field"><asp:Label id="txtPhoneNumber" runat="server"></asp:Label></td>
	</tr>
	<tr>
		<td class="required">Email Address:</td>
		<td class="field"><asp:Label id="txtEmailAddress" runat="server"></asp:Label></td>
	</tr>
	<tr>
		<td class="required">Competitor's Company Name:</td>
		<td class="field"><asp:Label id="txtCompetitorsCompanyName" runat="server"></asp:Label></td>
	</tr>
	<tr>
		<td class="required">Competitor's Phone Number:</td>
		<td class="field"><asp:Label id="txtCompetitorsPhoneNumber" runat="server"></asp:Label></td>
	</tr>
	<tr>
		<td class="optional">Competitor's Website:</td>
		<td class="field"><asp:Label id="txtCompetitorsWebsite" runat="server"></asp:Label></td>
	</tr>
	<asp:Repeater runat="server" ID="rpt">
	<HeaderTemplate>
		<tr><td colspan="2" class="bold largest" style="padding-top:10px;">ITEMS</td></tr>
		<tr><td colspan="2">
		<table border="0"><tr><th>Item # or Product Name</th><th>Competitor Price</th></tr>
	</HeaderTemplate>
	<ItemTemplate>
		<tr>
		<td><%#Container.DataItem("ItemNumberProductName")%></td>
		<td><%#FormatCurrency(Container.DataItem("CompetitorPrice"))%></td>
		</tr>
	</ItemTemplate>
	<FooterTemplate>
		</table>
		</td>
		</tr>
	</FooterTemplate>
	</asp:Repeater>
</table>


<p></p>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Price Match?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

