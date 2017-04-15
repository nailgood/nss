<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AdminLastLoginActivity.ascx.vb" Inherits="AdminLastLoginActivity" %>
<h4>Top 10 Last Logins</h4>
<asp:repeater id="LastActivity" runat="server">
	<HeaderTemplate>
		<table border="0" cellspacing="2" cellpadding="3" width=500>
			<tr>
				<th>
					Username</th>
				<th>
					Full Name</th>
				<th>
					IP Address</th>
				<th>
					Entry Date/Time</th>
			</tr>
	</HeaderTemplate>
	<ItemTemplate>
		<tr valign="top" class="row">
			<td><%# DataBinder.Eval(Container.DataItem, "Username") %></td>
			<td><%# DataBinder.Eval(Container.DataItem, "FullName") %></td>
			<td><%# DataBinder.Eval(Container.DataItem, "RemoteIP") %></td>
			<td><%# DataBinder.Eval(Container.DataItem, "LoginDate") %></td>
		</tr>
	</ItemTemplate>
	<AlternatingItemTemplate>
		<tr valign="top" class="alternate">
			<td><%# DataBinder.Eval(Container.DataItem, "Username") %></td>
			<td><%# DataBinder.Eval(Container.DataItem, "FullName") %></td>
			<td><%# DataBinder.Eval(Container.DataItem, "RemoteIP") %></td>
			<td><%# DataBinder.Eval(Container.DataItem, "LoginDate") %></td>
		</tr>
	</AlternatingItemTemplate>
	<FooterTemplate>
		</table>
	</FooterTemplate>
</asp:repeater>