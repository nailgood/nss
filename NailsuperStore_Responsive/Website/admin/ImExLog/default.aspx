<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Contact Email" CodeFile="default.aspx.vb" Inherits="admin_ImExLog_Index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Import/Export Log Administration</h4>

<span class="smaller">Please provide search criteria below</span>
<asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
<table cellpadding="2" cellspacing="2">
<tr>
<th valign="top"><b>Log Type:</b></th>
<td valign="top" class="field"><asp:DropDownList ID="F_LogTypeId" AutoPostBack ="true"  runat="server" /></td>
</tr>
<tr>
<th valign="top"><b>Process Date:</b></th>
<td valign="top" class="field">
<table border="0" cellpadding="0" cellspacing="0">
<tr><td class="smaller"><CC:DatePicker id="F_StartDateLbound" runat="server" /></td><td>&nbsp;</td>
</tr>
</table>
</td>
</tr>
<tr>
<td colspan="2" align="right">
<CC:OneClickButton id="btnSearch" Runat="server" Text="Search" cssClass="btn" />
<input class="btn" type="submit" value="Clear" onclick="window.location='default.aspx';return false;" />
</td>
</tr>

</table>
</asp:Panel>
<p></p>
<CC:OneClickButton id="AddNew" CausesValidation="False" Runat="server" Text="Add New Email Template" cssClass="btn" Visible ="false" ></CC:OneClickButton>

<p></p>
<div><table>
      <tr>
      <td></td>
      <td >Contents: File <asp:Label runat="server" ID="lblContents"  ></asp:Label> </td>
      </tr>
      <tr>
		<th valign="top"><CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom" SortOrder="DESC" SortBy="LogDate">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>		
		<asp:BoundField SortExpression="LogId" DataField="LogId" HeaderText="Log Id"></asp:BoundField>
		<asp:BoundField SortExpression="LogDate" DataField="LogDate" HeaderText="Log Date"></asp:BoundField>
		<asp:TemplateField HeaderText="Log File" SortExpression="LogFile">
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False NavigateUrl='<%# "Default.aspx?LogID=" & DataBinder.Eval(Container.DataItem, "LogID") & "&F_SortBy=" & ViewState("F_SortBy") & "&F_SortOrder=" & ViewState("F_SortOrder") %>' runat="server" text='<%# DataBinder.Eval(Container.DataItem, "LogFile") %>'></asp:HyperLink>
			
</ItemTemplate>
		</asp:TemplateField>
	</Columns>
            <HeaderStyle VerticalAlign="Top" />
</CC:GridView></th>
		<td class="field"><asp:textbox id="F_Contents" runat="server" maxlength="50" columns="50" style="width: 400px;" Height="400px" TextMode="MultiLine"  ></asp:textbox></td>
		<td></td>
</tr></table></div>


</asp:content>

