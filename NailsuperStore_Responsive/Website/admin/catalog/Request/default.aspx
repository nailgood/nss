<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="admin_catalog_request_default" title="" %>
<asp:Content ID="Content" ContentPlaceHolderID="ph" Runat="Server">
	<h4>Catalog Administration</h4>
	<table cellpadding="2" cellspacing="3" height="90">
		<TBODY>
			<tr>
				<th>
					First name</th>
				<td class="field"><asp:textbox id="F_Firstname" runat="server" /></td>
				<th>
					Last name</th>
				<td class="field"><asp:textbox id="F_LastName" runat="server"></asp:textbox></td>
			</tr>
			<tr>
			  <th>Date Requested</th>
			  <td colspan=3 class="field"><table border=0><tr><td><CC:DatePicker id="F_DateRequestedStart" runat="server" /></td><td>to</td><td><CC:DatePicker id="F_DateRequestedEnd" runat="server" /></td></tr></table></td>
			</tr>
			<tr>
			<th valign="top"><b>Output As:</b></th>
			<td valign="top" colspan=3 class="field">
			<asp:DropDownList ID="F_OutputAs" runat="server">
			<asp:ListItem Value="HTML" Text="HTML Page"></asp:ListItem>
			<asp:ListItem Value="Excel" Text="Excel Document"></asp:ListItem>
			</asp:DropDownList>
			</td>
			</tr>
			<tr>
				<td colspan=4><asp:Button ID="btnSearch" cssClass="btn" Text="Search" Runat="server" /> &nbsp;&nbsp; <asp:Button ID="ResetSearch" cssClass="btn" Text="Reset Search" Runat="server" /></td>
			</tr>
		</TBODY>
	</table>
	<p><asp:button id="AddNew" Runat="server" Text="Add New Request" cssClass="btn"></asp:button>
	<p></p>
	<table cellSpacing="0" cellPadding="0" border="0" id="tblList" runat="server">
		<tr>
			<td><asp:datagrid id="dgList" runat="server" PageSize="20" AllowPaging="True" AutoGenerateColumns="False"
					CellSpacing="2" CellPadding="2" AllowSorting="True" BorderWidth="0" Width="100%">
					<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
					<ItemStyle CssClass="row"></ItemStyle>
					<HeaderStyle CssClass="header"></HeaderStyle>
					<Columns>
						<asp:TemplateColumn>
							<ItemTemplate>
								<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "edit.aspx?RequestId=" & DataBinder.Eval(Container.DataItem, "RequestId")  & "&" & params %>' ImageUrl="/includes/theme-admin/images/edit.gif" ID="Hyperlink1">Edit</asp:HyperLink>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn>
							<ItemTemplate>
								<CC:ConfirmLink enableviewstate=False Message="Are you sure that you want to remove this request?" runat="server" NavigateUrl= '<%# "delete.aspx?RequestId=" & DataBinder.Eval(Container.DataItem, "RequestId") & "&" & params %>' ImageUrl="/includes/theme-admin/images/delete.gif" ID="Confirmlink1">Delete</CC:ConfirmLink>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn SortExpression="Y">
							<HeaderTemplate>
								<asp:LinkButton enableviewstate="False" CommandArgument="Name" CommandName="sort" id="Linkbutton4"
									runat="server">Name</asp:LinkButton>
								<asp:LinkButton enableviewstate=False visible='<%#Viewstate("F_SortBy") = "Name" and Viewstate("F_SortOrder") = "ASC" %>' CommandArgument="Name" CommandName=sort id="Linkbutton5" runat="server">
									<img border="0" src="/includes/theme-admin/images/Asc3.gif"></asp:LinkButton>
								<asp:LinkButton enableviewstate=False visible='<%#Viewstate("F_SortBy") = "Name" and Viewstate("F_SortOrder") = "DESC" %>' CommandArgument="Name" CommandName=sort id="Linkbutton6" runat="server">
									<img border="0" src="/includes/theme-admin/images/Desc3.gif"></asp:LinkButton>
							</HeaderTemplate>
							<ItemTemplate>
								<asp:Label enableviewstate=False runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Name") %>' ID="Label3">
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn SortExpression="Y">
							<HeaderTemplate>
								<asp:LinkButton enableviewstate="False" CommandArgument="DateRequested" CommandName="sort" id="Linkbutton1"
									runat="server">Date Requested</asp:LinkButton>
								<asp:LinkButton enableviewstate=False visible='<%#Viewstate("F_SortBy") = "DateRequested" and Viewstate("F_SortOrder") = "ASC" %>' CommandArgument="DateRequested" CommandName=sort id="Linkbutton2" runat="server">
									<img border="0" src="/includes/theme-admin/images/Asc3.gif"></asp:LinkButton>
								<asp:LinkButton enableviewstate=False visible='<%#Viewstate("F_SortBy") = "DateRequested" and Viewstate("F_SortOrder") = "DESC" %>' CommandArgument="DateRequested" CommandName=sort id="Linkbutton3" runat="server">
									<img border="0" src="/includes/theme-admin/images/Desc3.gif"></asp:LinkButton>
							</HeaderTemplate>
							<ItemTemplate>
								<asp:Label enableviewstate=False runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DateRequested") %>' ID="Label1">
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn SortExpression="Y">
							<HeaderTemplate>
								<asp:LinkButton enableviewstate="False" CommandArgument="State" CommandName="sort" id="Linkbutton7"
									runat="server">State</asp:LinkButton>
								<asp:LinkButton enableviewstate=False visible='<%#Viewstate("F_SortBy") = "State" and Viewstate("F_SortOrder") = "ASC" %>' CommandArgument="State" CommandName=sort id="Linkbutton8" runat="server">
									<img border="0" src="/includes/theme-admin/images/Asc3.gif"></asp:LinkButton>
								<asp:LinkButton enableviewstate=False visible='<%#Viewstate("F_SortBy") = "State" and Viewstate("F_SortOrder") = "DESC" %>' CommandArgument="State" CommandName=sort id="Linkbutton9" runat="server">
									<img border="0" src="/includes/theme-admin/images/Desc3.gif"></asp:LinkButton>
							</HeaderTemplate>
							<ItemTemplate>
								<asp:Label enableviewstate=False runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.State") %>' ID="Label2">
								</asp:Label>
							</ItemTemplate>
						</asp:TemplateColumn>														
					</Columns>
					<PagerStyle Visible="False"></PagerStyle>
				</asp:datagrid>
				<asp:HiddenField ID="hidCon" runat="server" /></td>
		</tr>
		<tr>
			<td><CC:Navigator id="myNavigator" runat="server" PagerSize="10" /></td>
		</tr>
	</table>
	<asp:placeholder id="plcNoRecords" runat="server" visible="false">There are no records that match the search criteria</asp:placeholder>
	
<div runat="server" id="divDownload" visible="false">
<asp:HyperLink id="lnkDownload" runat="server">Download File</asp:HyperLink>
</div>

</asp:Content>