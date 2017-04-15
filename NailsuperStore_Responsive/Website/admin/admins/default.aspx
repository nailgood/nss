<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="NailSuperstore.com" CodeFile="default.aspx.vb" Inherits="admin_admins_Index"  %>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
<h4>Admin User Administration</h4>

<table>
<tr>
    <th>Username</th>
    <td class="field"><asp:textbox id="F_Login" runat="server"></asp:textbox></td>
</tr>
<tr>
    <th>First Name</th>
    <td class="field"><asp:textbox id="F_FirstName" runat="server"></asp:textbox></td>
</tr>
<tr>
    <th>Last Name</th>
    <td class="field"><asp:textbox id="F_LastName" runat="server"></asp:textbox></td>
</tr>
<tr>
    <th style="text-align: right;">Is Active</th>
    <td valign="top" class="field">
        <asp:DropDownList ID="F_IsActive" runat="server">
            <asp:ListItem Value="">-- ALL --</asp:ListItem>
            <asp:ListItem Value="1">Yes</asp:ListItem>
            <asp:ListItem Value="0">No</asp:ListItem>
        </asp:DropDownList>	
    </td>
</tr>
<tr>
    <td colspan="2" align="right" valign="top">
       <asp:button id="btnSearch" Runat="server" Text="Search" cssClass="btn"></asp:button>
    </td>
</tr>
</table>

<p></p>
<asp:button id="AddNew" Runat="server" Text="Add New Admin" cssClass="btn"></asp:button>
    
<p></p>
<table cellspacing="0" cellpadding="0" border="0" id="tblList" runat="server">
<tr><td>

    <asp:DataGrid id="dgList" runat="server" PageSize="20" AllowPaging="True" AutoGenerateColumns="False" CellSpacing="2" CellPadding="2" AllowSorting="True" BorderWidth="0" Width="100%">
    <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
    <ItemStyle CssClass="row"></ItemStyle>
    <HeaderStyle CssClass="header"></HeaderStyle>
    <Columns>
        <asp:TemplateColumn>
            <ItemTemplate>
                <asp:HyperLink enableviewstate="False" runat="server" NavigateUrl= '<%# "edit.aspx?AdminId=" & DataBinder.Eval(Container.DataItem, "AdminId")   & params %>' ImageUrl="/includes/theme-admin/images/edit.gif" ID="Hyperlink1">Edit</asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
           <ItemTemplate>
                <CC:ConfirmLink enableviewstate="False" Message="Are you sure that you want to remove this user?" runat="server" NavigateUrl= '<%# "delete.aspx?AdminId=" & DataBinder.Eval(Container.DataItem, "AdminId")  & params %>' ImageUrl="/includes/theme-admin/images/delete.gif" ID="ConfirmDelete">Delete</CC:ConfirmLink>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn SortExpression="Y">
            <HeaderTemplate>
                    <asp:LinkButton enableviewstate="False" CommandArgument="a.Username" CommandName="sort" id="sortLogin" runat="server">Username</asp:LinkButton>
                    <asp:LinkButton enableviewstate="False" visible='<%#Viewstate("F_SortBy") = "a.Username" and Viewstate("F_SortOrder") = "ASC" %>' CommandArgument="a.Username" CommandName="sort" id="Linkbutton4" runat="server"><img border="0" src="/includes/theme-admin/images/Asc3.gif" alt=""></asp:LinkButton>
                    <asp:LinkButton enableviewstate="False" visible='<%#Viewstate("F_SortBy") = "a.Username" and Viewstate("F_SortOrder") = "DESC" %>' CommandArgument="a.Username" CommandName="sort" id="sortLoginDesc" runat="server"><img border="0" src="/includes/theme-admin/images/Desc3.gif" alt=""></asp:LinkButton>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label enableviewstate="False" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Username") %>' ID="Label1"></asp:Label>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn SortExpression="Y">
            <HeaderTemplate>
                <asp:LinkButton enableviewstate="False" CommandArgument="a.LastName" CommandName="sort" id="Linkbutton1" runat="server">Last Name</asp:LinkButton>
                <asp:LinkButton enableviewstate="False" visible='<%#Viewstate("F_SortBy") = "a.LastName" and Viewstate("F_SortOrder") = "ASC" %>' CommandArgument="a.LastName" CommandName="sort" id="Linkbutton2" runat="server"><img border="0" src="/includes/theme-admin/images/Asc3.gif" alt=""></asp:LinkButton>
                <asp:LinkButton enableviewstate="False" visible='<%#Viewstate("F_SortBy") = "a.LastName" and Viewstate("F_SortOrder") = "DESC" %>' CommandArgument="a.LastName" CommandName="sort" id="Linkbutton3" runat="server"><img border="0" src="/includes/theme-admin/images/Desc3.gif" alt=""></asp:LinkButton>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label enableviewstate="False" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LastName") %>' ID="Label2">
                </asp:Label>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:BoundColumn DataField="FirstName" HeaderText="First Name"></asp:BoundColumn>    
        <asp:TemplateColumn HeaderText="IP Acsess" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <a href="AdminIPAccess/Default.aspx?username=<%#DataBinder.Eval(Container, "DataItem.Username")%>"><%#getCountIPAccess(DataBinder.Eval(Container, "DataItem.Username"))%></a>
                
            </ItemTemplate>
        </asp:TemplateColumn>
                 <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderText="Active">
            <ItemTemplate>
                <asp:ImageButton ID="imbActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png"
                    CommandName="Active" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "AdminId")%>' />
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <asp:HyperLink enableviewstate="False" runat="server" NavigateUrl= '<%# "password.aspx?AdminId=" & DataBinder.Eval(Container.DataItem, "AdminId")  &  params %>' ID="ChangePass">Change password</asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
    <PagerStyle Visible="False"></PagerStyle>
    </asp:datagrid>
    <asp:HiddenField ID="hidCon" runat="server" />
    </td>
</tr>
<tr>
<td><CC:Navigator id="myNavigator" runat="server" PagerSize="10" /></td>
</tr>
</table>

<asp:placeholder id="plcNoRecords" runat="server" visible="false">There are no records that match the search criteria</asp:placeholder>

</asp:content>