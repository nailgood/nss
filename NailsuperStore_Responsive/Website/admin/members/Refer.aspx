<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="Refer.aspx.vb" Inherits="admin_members_Refer" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">
    <h4>
        Refer Friend</h4>
    <span class="smaller">Please provide search criteria below</span>
    <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
        <table cellpadding="2" cellspacing="2">
            <tr>
                <th valign="top">
                    Member Refer:
                </th>
                <td valign="top" class="field">
                   <asp:TextBox ID="F_MemberRefer" runat="server" Width="131px" MaxLength="50"></asp:TextBox>
                </td>
                <th valign="top">
                    Date:
                </th>
                <td valign="top" class="field">                 
                    <CC:DatePicker ID="F_CreatedDate" runat="server" />
                </td>
            </tr>
            <tr>
                <th valign="top">
                    Refer Code:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_ReferCode" runat="server" Width="131px" MaxLength="50"></asp:TextBox>
                </td>
                <th valign="top">
                    Type:
                </th>
                <td valign="top" class="field">
	                <asp:RadioButtonList runat="server" ID="F_Type" RepeatDirection="Horizontal">                    
<%--                        <asp:ListItem Value="0">All</asp:ListItem>
                        <asp:ListItem Value="1">Email</asp:ListItem>
                        <asp:ListItem Value="2">Facebook</asp:ListItem>
                        <asp:ListItem Value="3">Twitter</asp:ListItem>--%>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    Username:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_Username" runat="server" Columns="50" MaxLength="50"></asp:TextBox>
                </td>
                 <th valign="top">
                    Email:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_Email" runat="server" Columns="50" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    Status:
                </th>
                <td valign="top" class="field">
                    <asp:DropDownList ID="F_Status" runat="server">
<%--	                    <asp:ListItem Value="">---</asp:ListItem>		                    
	                    <asp:ListItem Value="0">Invitation sent</asp:ListItem>
	                    <asp:ListItem Value="1">Registered</asp:ListItem>
	                    <asp:ListItem Value="2">Activated Account</asp:ListItem>
	                    <asp:ListItem Value="3">Ordered</asp:ListItem>
	                    <asp:ListItem Value="4">Ordere shipped</asp:ListItem>--%>
                    </asp:DropDownList>
                </td>
            </tr>            
            <tr>
                <td colspan="2" align="right">
                    <CC:OneClickButton ID="btnSearch" runat="server" Text="Search" CssClass="btn" />
                    <input class="btn" type="submit" value="Clear" onclick="window.location='refer.aspx';return false;" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <p>
    </p>

 <CC:GridView ID="gvList" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
        BorderWidth="0" CellPadding="2" CellSpacing="2" EmptyDataText="There are no records that match the search criteria"
        HeaderText="In order to change display order, please use header links" PagerSettings-Position="Bottom"
        PageSize="20">
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top" />
        <RowStyle CssClass="row" VerticalAlign="Top" />
        <Columns>
        <asp:TemplateField HeaderText="Member Refer" HeaderStyle-Width="90"  SortExpression="MemberRefer">
            <ItemTemplate>
		        <asp:HyperLink enableviewstate="False" runat="server" NavigateUrl='<%# string.Format("/admin/members/edit.aspx?MemberId={0}", DataBinder.Eval(Container.DataItem, "MemberReferId")) %>' ID="lnkMemberRefer"><%#DataBinder.Eval(Container.DataItem, "MemberRefer")%></asp:HyperLink>
		    </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Username" HeaderStyle-Width="90">
            <ItemTemplate>
		        <asp:HyperLink enableviewstate="False" runat="server" NavigateUrl='<%# string.Format("/admin/members/edit.aspx?MemberId={0}", DataBinder.Eval(Container.DataItem, "MemberUseReferId")) %>' ID="lnkMemberUseRefer"><%#DataBinder.Eval(Container.DataItem, "MemberUseRefer")%></asp:HyperLink>
		    </ItemTemplate>
        </asp:TemplateField>
		<asp:BoundField SortExpression="Email" DataField="Email" HeaderStyle-Width="180" HeaderText="Email"></asp:BoundField>
		<asp:BoundField SortExpression="CreatedDate" DataField="CreatedDate" HeaderText="Created Date" HeaderStyle-Width="90" DataFormatString="{0:d}" HTMLEncode="False"></asp:BoundField>
		<asp:BoundField DataField="ReferCode" HeaderText="Refer Code" HeaderStyle-Width="65"></asp:BoundField>
		<asp:TemplateField HeaderText="Status"  SortExpression="Status">
            <ItemTemplate>
                <asp:Literal runat="server" ID="ltrStatus"></asp:Literal>
		    </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Type" HeaderStyle-Width="30" ItemStyle-HorizontalAlign="Center" SortExpression="Source">
            <ItemTemplate>
                <asp:Literal runat="server" ID="ltrSource"></asp:Literal>
		    </ItemTemplate>
        </asp:TemplateField>
        </Columns>
    </CC:GridView>
<asp:HiddenField ID="hidCon" runat="server" />
</asp:Content>

