<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" CodeFile="Edit.aspx.vb" Inherits="admin_AutoRespond_Edit" %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan=2><h4><% IF DayId = 0 Then %> Add New <% Else %> EDit <% End If%></h4></td></tr>
	
<tr>
<td class="required" style="color:Black">Day Name:</td>
		<td class="field"><asp:textbox id="txtDayName" runat="server" MaxLength="30" Columns="50" style="width: 419px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvDayName" runat="server" Display="Dynamic" ControlToValidate="txtDayName" CssClass="msgError" ErrorMessage="Field 'Day Name' is blank"></asp:RequiredFieldValidator></td>
</tr>
	<tr>
		<td class="optional" align="center" style="padding-top:5px">Start:</td>
		<td class="field">
		<table width="100%">
		
		<tr>
		    <td width="40px">Hour: </td>
		    <td width="100px" align="left"><asp:DropDownList runat="server" ID="ddlStart" AutoPostBack="false" /></td>
		    <td width="40px">Date: </td>
		    <td width="100px" align="left"><CC:DatePicker ID="dtStartingDate" runat="server"></CC:DatePicker></td>
		</tr>
		</table>
		
		</td>
		<td><CC:DateValidator Display="Dynamic" runat="server" id="dtvStartingDate" ControlToValidate="dtStartingDate" CssClass="msgError" ErrorMessage="Date Field 'Starting Date' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional" align="center" style="padding-top:5px">End </td>
		<td class="field">
		<table width="100%">
		<tr>
		    <td width="40px">Hour: </td>
		    <td width="100px" align="left"><asp:DropDownList runat="server" ID="ddlEnd" AutoPostBack="false" /></td>
		    <td width="40px">Date: </td>
		    <td width="100px" align="left"><CC:DatePicker ID="dtEndingDate" runat="server"></CC:DatePicker></td>
		</tr>
		</table>
		</td>
		<td><CC:DateValidator Display="Dynamic" runat="server" id="dtvEndingDate" ControlToValidate="dtEndingDate" CssClass="msgError" ErrorMessage="Date Field 'Ending Date' is invalid" /></td>
	</tr>

</table>

<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
</asp:content>