<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Contact Email" CodeFile="default.aspx.vb" Inherits="admin_store_requestcallbacklanguage_Index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Request Call Back Language Administration</h4>

<span class="smaller">Please provide search criteria below</span>
<asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
<table cellpadding="2" cellspacing="2">
<tr>
<th valign="top"><b>Language Id:</b></th>
<td valign="top" class="field"><asp:DropDownList ID="F_LanguageId" AutoPostBack="true"  runat="server" /><CC:OneClickButton id="btnDeleteEmail" Runat="server" Text="Delete Email" cssClass="btn"></CC:OneClickButton><asp:textbox id="F_DetailID" runat="server" maxlength="50" columns="50" style="width: 0px;" Visible ="false"></asp:textbox></td>
</tr>
<tr>
<th valign="top"><b>Email:</b></th>
<td valign="top" class="field"><asp:DropDownList ID="F_EmailId" AutoPostBack="true"  runat="server" /></td>
</tr>
<tr>
		<td class="field">Language Name:</td>
		<td class="field"><asp:textbox id="F_LanguageName" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvName" runat="server" Display="Dynamic" ControlToValidate="F_LanguageName" ErrorMessage="Field 'Language Name' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="field">LanguageCode:</td>
		<td class="field"><asp:textbox id="F_LanguageCode" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvEmailAddress" runat="server" Display="Dynamic" ControlToValidate="F_LanguageCode" ErrorMessage="Field 'LanguageCode' is blank"></asp:RequiredFieldValidator></td>
	</tr>
<tr>
<td colspan="2" align="right">
<CC:OneClickButton id="btnSearch" Runat="server" Text="Search" cssClass="btn" Visible ="false" />

</td>
</tr>
</table>
</asp:Panel>
<p></p>
<CC:OneClickButton id="AddNew" CausesValidation="False" Runat="server" Text="Add New Email Language" cssClass="btn"></CC:OneClickButton>
<CC:OneClickButton id="AddNewEmail" Runat="server" Text="Add New Language" cssClass="btn"></CC:OneClickButton>
<p></p>

<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate="False" runat="server" NavigateUrl= '<%# "edit.aspx?DetailId=" & DataBinder.Eval(Container.DataItem, "DetailId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<CC:ConfirmLink enableviewstate="False" Message="Are you sure that you want to remove this Email Language?" runat="server" NavigateUrl= '<%# "delete.aspx?DetailId=" & DataBinder.Eval(Container.DataItem, "DetailId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField SortExpression="LanguageId" DataField="LanguageId" HeaderText="Language Id"></asp:BoundField>
		<asp:BoundField SortExpression="Language" DataField="Language" HeaderText="Language Name"></asp:BoundField>
		<asp:BoundField SortExpression="Name" DataField="Name" HeaderText="Name"></asp:BoundField>
		<asp:BoundField SortExpression="Email" DataField="Email" HeaderText="Email Address"></asp:BoundField>
		</Columns>
</CC:GridView>

</asp:content>

