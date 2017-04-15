<%@ Control Language="VB" AutoEventWireup="false" CodeFile="store-department-list.ascx.vb" Inherits="modules_StoreDepartmentList" %>

<asp:Repeater runat="server" ID="rptDepartments">
	<ItemTemplate>
			<asp:Literal runat="server" ID="lit" />
			<%--<asp:TreeView runat="server" ID="tree1" ShowExpandCollapse="False" SkipLinkText="" ShowLines="false" NodeIndent="17" />--%>
	</ItemTemplate>
</asp:Repeater>