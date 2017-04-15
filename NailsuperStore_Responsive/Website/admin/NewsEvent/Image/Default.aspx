<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_NewsEvent_Image_Default" %>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
<script>
    function ConfirmDelete() {
        return confirm('Are you sure to delete?');
        
    }
</script>
 <h4>
        Image</h4>
    <span class="smaller">Please provide search criteria below</span>
    <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
        <table cellpadding="2" cellspacing="2">
            <tr>
                <th valign="top">
                    Name:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_Name" runat="server" Columns="50" MaxLength="256"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    <b>Is Active:</b>
                </th>
                <td valign="top" class="field">
                    <asp:DropDownList ID="F_IsActive" runat="server">
                        <asp:ListItem Value="">-- ALL --</asp:ListItem>
                        <asp:ListItem Value="1">Yes</asp:ListItem>
                        <asp:ListItem Value="0">No</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <CC:OneClickButton ID="btnSearch" runat="server" Text="Search" CssClass="btn" />
                    <input class="btn" type="submit" value="Clear" onclick="window.location='default.aspx';return false;" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <p></p>
    <CC:OneClickButton ID="AddNew" runat="server" Text="Add New Images" CssClass="btn"></CC:OneClickButton>
    <p></p>

<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="4" AllowPaging="True" AllowSorting="True"  EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	
	 <Columns>
            
            <asp:BoundField DataField="ImageName" HeaderStyle-Width="250" HeaderText="Name" ItemStyle-VerticalAlign="Middle"></asp:BoundField>
             <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Image" ItemStyle-VerticalAlign="Middle">
                <ItemTemplate>
                   <asp:Literal ID="litImage" runat="server"></asp:Literal>
                </ItemTemplate>
                </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active" ItemStyle-VerticalAlign="Middle">
                <ItemTemplate>
                    <asp:ImageButton ID="imbActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png"
                        CommandName="Active" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ImageId")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" HeaderText="Edit">
                <ItemTemplate>
                    <asp:HyperLink EnableViewState="False" runat="server" NavigateUrl='<%# "edit.aspx?id=" & DataBinder.Eval(Container.DataItem, "ImageId") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                        ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center"  HeaderText="Delete" ItemStyle-VerticalAlign="Middle">
                <ItemTemplate>
                    <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif"
                        CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ImageId")%>'
                        OnClientClick="return ConfirmDelete();" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
</CC:GridView>




</asp:content>

