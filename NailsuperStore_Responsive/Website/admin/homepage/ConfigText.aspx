﻿<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="ConfigText.aspx.vb" Inherits="admin_homepage_ConfigText" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">
<h4>Section Text</h4>
<table cellSpacing="2" cellPadding="3" border="0">
	<asp:repeater enableviewstate="True" id="configtext" Runat="server">
		<ItemTemplate>
			<tr valign="middle" id="headerRow" runat="server">
				<th colspan="3">
					<asp:Label ID="headerLabel" Runat="server" />
				</th>
			</tr>
			<tr valign="top">
				<td class="row" width=300><asp:Label ID="sysparamName" Runat="server" /></td>
				<td class="row"><asp:PlaceHolder ID="editPlace" Runat="server" /></td>
				<td class="row"><CC:ConfirmButton ValidationGroup="Group" Message="Are you sure you want to save this value?" ID="saveButton" Runat="server" CssClass="btn" Text="Save" CommandName="Save"/></td>
				<td><asp:PlaceHolder ID="validatePlace" Runat="server" /></td>
			</tr>
		</ItemTemplate>
		<AlternatingItemTemplate>
			<tr valign="middle" id="headerRowALT" runat="server">
				<th colspan="3">
					<asp:Label ID="headerLabelALT" Runat="server" />
				</th>
			</tr>
			<tr valign="top">
				<td class="alternate" width=300><asp:Label ID="sysparamNameALT" Runat="server" /></td>
				<td class="alternate"><asp:PlaceHolder ID="editPlaceALT" Runat="server" /></td>
				<td class="alternate"><CC:ConfirmButton ValidationGroup="Group" Message="Are you sure you want to save this value?" ID="saveButtonALT" Runat="server" CssClass="btn" Text="Save" CommandName="Save"/></td>
				<td><asp:PlaceHolder ID="validatePlaceALT" Runat="server" /></td>
			</tr>
		</AlternatingItemTemplate>
	</asp:repeater>
</table>
</asp:Content>

