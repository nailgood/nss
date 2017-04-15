<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" Async="true" AutoEventWireup="false" CodeFile="media.aspx.vb" Inherits="admin_shopdesign_media" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">
    <script type="text/javascript">
    function ConfirmDelete() {
        if (!confirm('Are you sure to delete?')) {
            return false;
        }
    }
    </script>
 <h4><asp:Literal ID="ltrHeader" runat="server" Text="List Media "></asp:Literal></h4>

 <asp:Panel ID="pnAddNew" runat="server">
    <table border="0" cellspacing="1" cellpadding="2">
        <tr>
            <td colspan="2">
                <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
            </td>
        </tr>
         <tr>            
            <%=lblFile %>
            <td class="field" style="width: 500px">
                <CC:FileUpload ID="file" AutoResize="true" Folder="/assets/shopdesign/" ImageDisplayFolder="/assets/shopdesign/small/" EnableDelete="false"
                    DisplayImage="False" runat="server" Style="width: 475px;" />
                <div runat="server" id="divImg">
                    <b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
                    <div>
                        <asp:Image runat="server" ID="hpimg" usemap="#hpimgmap" /></div>
                </div>
            </td>
            <td>
                <CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp" ID="feImage"
                    runat="server" Display="Dynamic" ControlToValidate="fuImage" CssClass="msgError" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
                <asp:Label runat="server" ID="lblImgError" Visible="false" style="color: Red; display: inline;"></asp:Label>
            </td>
        </tr>
        <% If TypeMedia = Utility.Common.ShopDesignMediaType.Video Then %>
        <tr>
            <td class="required">
                Video URL:
            </td>
            <td class="field" style="width: 500px">
                <asp:TextBox ID="txtUrl" runat="server" MaxLength="255" Width="400px"></asp:TextBox>
                <div id="divVideo" runat="server" visible="false">
                    <asp:Literal ID="ltrVideo" runat="server"></asp:Literal>
                </div>
            </td>
            <td>
                <asp:Label runat="server" ID="lblUrlError" Visible="false" style="color: Red; display: inline;"></asp:Label>
                <%--<asp:RequiredFieldValidator ID="rfvUrl" runat="server" Display="Dynamic" ControlToValidate="txtUrl" Visible="false"
                    CssClass="msgError" ErrorMessage="* Please input URL"></asp:RequiredFieldValidator>--%>
            </td>
        </tr>
        <tr>
            <td class="optional">
               Description:
            </td>
            <td class="field" style="width: 500px">
                <asp:TextBox ID="txtDesc" runat="server" MaxLength="255" Width="400px" TextMode="MultiLine"></asp:TextBox>
            </td>
            <td></td>
        </tr>
        <% End If%>
        <tr>
            <td class="optional">
                Media Tag:
            </td>
            <td class="field" style="width: 500px">
                <asp:TextBox ID="txtTag" runat="server" MaxLength="255" Width="400px" ></asp:TextBox>
            </td>
            <td></td>
        </tr>
    </table><br />
    <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
    <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False"></CC:OneClickButton>
    <CC:OneClickButton ID="btnBack" runat="server" Text="Back" CssClass="btn" ValidationGroup="val1" CausesValidation="False"></CC:OneClickButton>
 </asp:Panel><br />
 <asp:Panel ID="pnList" runat="server">
    <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50"
        AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria"
        AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom" CausesValidation="false" >
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
        <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
        <Columns>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="130" HeaderText="File">
                <ItemTemplate>
                    <asp:Literal ID="lrtUrl" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Tag" HeaderStyle-Width="250" HeaderText="Tag"></asp:BoundField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Edit">
                <ItemTemplate>
                    <a href="media.aspx?Id=<%#DataBinder.Eval(Container.DataItem, "Id") & "&ShopDesignId=" & ShopDesignId & "&Type=" & TypeMedia & "&" & GetPageParams(Components.FilterFieldType.All)%>">
                        <img src="/includes/theme-admin/images/edit.gif" style="border: none;" /></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete">
                <ItemTemplate>
                    <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif"
                        CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>'
                        OnClientClick="return ConfirmDelete();" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="" >
                <ItemTemplate>
                    <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up" CausesValidation="false"
                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                    <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif" CausesValidation="false"
                        CommandName="Down" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </CC:GridView>
    <asp:HiddenField ID="hidCon" runat="server" />
 </asp:Panel>
</asp:Content>

