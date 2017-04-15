<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="" CodeFile="sale.aspx.vb" Inherits="admin_store_sales_default"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
    <h4>Store Sales Administration</h4>
    
    <div id="tblFilter" runat="server">
	<table cellpadding="2" cellspacing="3" height="90">
		<TBODY>
			<tr>
				<th>
					From Date</th>
				<td class="field"><CC:DatePicker id="F_FromDate" runat="server" /></td>
				<th>
					To Date</th>
				<td class="field"><CC:DatePicker id="F_ToDate" runat="server" /></td>
			</tr>
			<tr>
				<th>
					Item #</th>
				<td class="field"><asp:textbox id="F_SKU" runat="server" /></td>
				<th>
					Export As</th>
				<td class="field">
					<asp:DropDownList ID="F_drpExportType" Runat="server">
					<asp:ListItem Value="HTML">HTML</asp:ListItem>
					<asp:ListItem Value="Excel">Excel</asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td><asp:Button ID="btnResetSearch" cssClass="btn" Text="Reset Search" Runat="server" /></td>
				<td colspan="3"><asp:Button ID="btnSearch" cssClass="btn" Text="Search" Runat="server" /></td>
			</tr>
		</TBODY>
	</table>
	<p></p>
	</div>
	
    <table cellSpacing="0" cellPadding="0" border="0" id="tblList" runat="server" width="650">
        <tr>
            <td><asp:datagrid OnItemDataBound="ComputeSum" id="dgList" runat="server" PageSize="30" AllowPaging="True" ShowFooter="True" AutoGenerateColumns="False"
                    CellSpacing="2" CellPadding="2" AllowSorting="True" BorderWidth="0" Width="100%">
                    <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
                    <ItemStyle CssClass="row"></ItemStyle>
                    <HeaderStyle CssClass="header"></HeaderStyle>
                    <FooterStyle CssClass="header"></FooterStyle>
                    <Columns>

                        <asp:TemplateColumn SortExpression="Y" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
								<HeaderTemplate>
									<asp:LinkButton enableviewstate="False" CommandArgument="SKU" CommandName="sort" id="Linkbutton1" runat="server">Item #</asp:LinkButton>
									<asp:LinkButton enableviewstate=False visible='<%#Viewstate("F_SortBy") = "SKU" and Viewstate("F_SortOrder") = "ASC" %>' CommandArgument="SKU" CommandName=sort id="Linkbutton2" runat="server">
										<img border="0" src="/includes/theme-admin/images/Asc3.gif"></asp:LinkButton>
									<asp:LinkButton enableviewstate=False visible='<%#Viewstate("F_SortBy") = "SKU" and Viewstate("F_SortOrder") = "DESC" %>' CommandArgument="SKU" CommandName=sort id="Linkbutton3" runat="server">
										<img border="0" src="/includes/theme-admin/images/Desc3.gif"></asp:LinkButton>
								</HeaderTemplate>
								<ItemTemplate>
									<asp:Label enableviewstate=False runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SKU") %>' ID="Label3" />
								</ItemTemplate>
						</asp:TemplateColumn>                 
								
                        <asp:TemplateColumn SortExpression="Y" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
								<HeaderTemplate>
									<asp:LinkButton enableviewstate="False" CommandArgument="ItemName" CommandName="sort" id="Linkbutton4" runat="server">Item Name</asp:LinkButton>
									<asp:LinkButton enableviewstate=False visible='<%#Viewstate("F_SortBy") = "ItemName" and Viewstate("F_SortOrder") = "ASC" %>' CommandArgument="ItemName" CommandName=sort id="Linkbutton5" runat="server">
										<img border="0" src="/includes/theme-admin/images/Asc3.gif"></asp:LinkButton>
									<asp:LinkButton enableviewstate=False visible='<%#Viewstate("F_SortBy") = "ItemName" and Viewstate("F_SortOrder") = "DESC" %>' CommandArgument="ItemName" CommandName=sort id="Linkbutton6" runat="server">
										<img border="0" src="/includes/theme-admin/images/Desc3.gif"></asp:LinkButton>
								</HeaderTemplate>
								<ItemTemplate>
									<asp:Label enableviewstate=False runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ItemName") %>' ID="Label1">
									</asp:Label>
								</ItemTemplate>
						</asp:TemplateColumn>     
						            
                        <asp:TemplateColumn SortExpression="Y" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
								<HeaderTemplate>
									<asp:LinkButton enableviewstate="False" CommandArgument="UnitPrice" CommandName="sort" id="Linkbutton7" runat="server">avg. Unit Price</asp:LinkButton>
									<asp:LinkButton enableviewstate=False visible='<%#Viewstate("F_SortBy") = "UnitPrice" and Viewstate("F_SortOrder") = "ASC" %>' CommandArgument="UnitPrice" CommandName=sort id="Linkbutton8" runat="server">
										<img border="0" src="/includes/theme-admin/images/Asc3.gif"></asp:LinkButton>
									<asp:LinkButton enableviewstate=False visible='<%#Viewstate("F_SortBy") = "UnitPrice" and Viewstate("F_SortOrder") = "DESC" %>' CommandArgument="UnitPrice" CommandName=sort id="Linkbutton9" runat="server">
										<img border="0" src="/includes/theme-admin/images/Desc3.gif"></asp:LinkButton>
								</HeaderTemplate>
								<ItemTemplate>
									<asp:Label enableviewstate=False runat="server" Text='<%# FormatCurrency(DataBinder.Eval(Container, "DataItem.UnitPrice")) %>' ID="Label2">
									</asp:Label>
								</ItemTemplate>
						</asp:TemplateColumn>   

                        <asp:TemplateColumn SortExpression="Y" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
								<HeaderTemplate>
									<asp:LinkButton enableviewstate="False" CommandArgument="itemdiscount" CommandName="sort" id="Linkbutton16" runat="server">Discount</asp:LinkButton>
									<asp:LinkButton enableviewstate=False visible='<%#Viewstate("F_SortBy") = "itemdiscount" and Viewstate("F_SortOrder") = "ASC" %>' CommandArgument="itemdiscount" CommandName=sort id="Linkbutton17" runat="server">
										<img border="0" src="/includes/theme-admin/images/Asc3.gif"></asp:LinkButton>
									<asp:LinkButton enableviewstate=False visible='<%#Viewstate("F_SortBy") = "itemdiscount" and Viewstate("F_SortOrder") = "DESC" %>' CommandArgument="itemdiscount" CommandName=sort id="Linkbutton18" runat="server">
										<img border="0" src="/includes/theme-admin/images/Desc3.gif"></asp:LinkButton>
								</HeaderTemplate>
								<ItemTemplate>
									<asp:Label enableviewstate=False runat="server" Text='<%# FormatCurrency(DataBinder.Eval(Container, "DataItem.itemdiscount")) %>' ID="Label6">
									</asp:Label>
								</ItemTemplate>
						</asp:TemplateColumn>   

                        <asp:TemplateColumn SortExpression="Y" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
								<HeaderTemplate>
									<asp:LinkButton enableviewstate="False" CommandArgument="TotalPrice" CommandName="sort" id="Linkbutton13" runat="server">Total Price</asp:LinkButton>
									<asp:LinkButton enableviewstate=False visible='<%#Viewstate("F_SortBy") = "TotalPrice" and Viewstate("F_SortOrder") = "ASC" %>' CommandArgument="TotalPrice" CommandName=sort id="Linkbutton14" runat="server">
										<img border="0" src="/includes/theme-admin/images/Asc3.gif"></asp:LinkButton>
									<asp:LinkButton enableviewstate=False visible='<%#Viewstate("F_SortBy") = "TotalPrice" and Viewstate("F_SortOrder") = "DESC" %>' CommandArgument="TotalPrice" CommandName=sort id="Linkbutton15" runat="server">
										<img border="0" src="/includes/theme-admin/images/Desc3.gif"></asp:LinkButton>
								</HeaderTemplate>
								<ItemTemplate>
									<asp:Label enableviewstate=False runat="server" Text='<%# FormatCurrency(DataBinder.Eval(Container, "DataItem.TotalPrice")) %>' ID="Label5">
									</asp:Label>
								</ItemTemplate>
								<FooterTemplate>
									<asp:label ID="lblPageSumTotal" Runat="server" />
								</FooterTemplate>
						</asp:TemplateColumn>   

                        <asp:TemplateColumn SortExpression="Y" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
								<HeaderTemplate>
									<asp:LinkButton enableviewstate="False" CommandArgument="NumSales" CommandName="sort" id="Linkbutton10" runat="server">Unit Sales</asp:LinkButton>
									<asp:LinkButton enableviewstate=False visible='<%#Viewstate("F_SortBy") = "NumSales" and Viewstate("F_SortOrder") = "ASC" %>' CommandArgument="NumSales" CommandName=sort id="Linkbutton11" runat="server">
										<img border="0" src="/includes/theme-admin/images/Asc3.gif"></asp:LinkButton>
									<asp:LinkButton enableviewstate=False visible='<%#Viewstate("F_SortBy") = "NumSales" and Viewstate("F_SortOrder") = "DESC" %>' CommandArgument="NumSales" CommandName=sort id="Linkbutton12" runat="server">
										<img border="0" src="/includes/theme-admin/images/Desc3.gif"></asp:LinkButton>
								</HeaderTemplate>
								<ItemTemplate>
									<asp:Label enableviewstate=False runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NumSales") %>' ID="Label4">
									</asp:Label>
								</ItemTemplate>
						</asp:TemplateColumn>   
                    </Columns>
                    <PagerStyle Visible="False"></PagerStyle>
                </asp:datagrid></td>
        </tr>
        <tr>
            <td colspan=4><CC:Navigator id="myNavigator" runat="server" PagerSize="10" /></td>
        </tr>
    </table>
    <asp:placeholder id="plcNoRecords" runat="server" visible="false">There are no records that match your search criteria</asp:placeholder>
</asp:content>