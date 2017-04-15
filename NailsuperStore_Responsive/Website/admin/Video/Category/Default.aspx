﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="admin_Video_Category_Default" MasterPageFile="~/includes/masterpage/admin.master" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">

    <script type="text/javascript">
    function ConfirmDelete() {
        if (!confirm('Are you sure to delete?')) {
            return false;
        }
    }

    </script>

    <h4>Category</h4>
    <span class="smaller">Please provide search criteria below</span>
    <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
        <table cellpadding="2" cellspacing="2">
            <tr style="display:none;">
                <th valign="top">
                    Name:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_Name" runat="server" Columns="50" MaxLength="256"></asp:TextBox>
                </td>
            </tr>
            <tr style="display:none;">
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
                <td class="optional">
                    Video Center Banner<br />
                    <span class="smaller">754 x 100</span>
                </td>
                <td class="field">
                    <CC:FileUpload ID="fuImage" AutoResize="true" ImageDisplayFolder="/assets/video/banner/"
                        DisplayImage="False" runat="server" />
                    <div runat="server" id="divImg">
                        <b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
                        <div>
                            <asp:Image runat="server" ID="hpimg" usemap="#hpimgmap" /></div>
                    </div>
                </td>
                <td valign="top">
                    <CC:OneClickButton ID="btnSaveImage" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
                </td>
                <td>
                    <CC:FileUploadExtensionValidator Extensions="swf,swf,jpg,jpeg,gif,bmp,png" ID="feImage"
                        runat="server" Display="Dynamic" ControlToValidate="fuImage" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
                </td>
            </tr>
            <tr style="display:none;">
                <td colspan="2" align="right">
                    <CC:OneClickButton ID="btnSearch" runat="server" Text="Search" CssClass="btn" />
                    <input class="btn" type="submit" value="Clear" onclick="window.location='default.aspx';return false;" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    <p></p>
    <CC:OneClickButton ID="AddNew" runat="server" Text="Add New Category" CssClass="btn"></CC:OneClickButton>   
    <p></p>
    
    <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50"
        AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria"
        AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
        <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
        <Columns>
           
            <asp:BoundField DataField="CategoryName" HeaderStyle-Width="250" HeaderText="Name"></asp:BoundField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active">
                <ItemTemplate>
                    <asp:ImageButton ID="imbActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png"
                        CommandName="Active" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CategoryId")%>' />
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Edit">
                <ItemTemplate>
                    <asp:HyperLink EnableViewState="False" runat="server" NavigateUrl='<%# "edit.aspx?id=" & DataBinder.Eval(Container.DataItem, "CategoryId") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                        ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete">
                <ItemTemplate>
                    <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif"
                        CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CategoryId")%>'
                        OnClientClick="return ConfirmDelete();" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Arrange">
                <ItemTemplate>
                    <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up"
                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CategoryId")%>' />
                    <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif"
                        CommandName="Down" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CategoryId")%>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </CC:GridView>
</asp:Content>