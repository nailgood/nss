<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master"
    Title="Homepage" CodeFile="default.aspx.vb" Inherits="Index" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>
        Free Samples</h4>
    <table border="0" cellspacing="1" cellpadding="2">
        <tr>
            <td colspan="2">
                <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
            </td>
        </tr>
        <tr>
            <td class="required">
                Free Samples Order Min:
            </td>
            <td class="field">
                <asp:TextBox ID="txtOrderMin" runat="server" MaxLength="50" Columns="50" Style="width: 100px;"
                    Enabled="true"></asp:TextBox>
            </td>
            <td>
                <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
            </td>
            <td>
                <CC:IntegerValidator ID="IntegerValidator2" runat="server" ControlToValidate="txtOrderMin"
                    CssClass="msgError" ErrorMessage="Please enter a valid Order Min" Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td class="required">
                Free Sample Qty
            </td>
            <td class="field">
                <asp:TextBox ID="txtQty" runat="server" MaxLength="50" Columns="50" Style="width: 100px;"
                    Enabled="true"></asp:TextBox>
            </td>
            <td>
                <CC:OneClickButton ID="btnSave1" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
            </td>
            <td>
                <CC:IntegerValidator ID="IntegerValidator1" runat="server" ControlToValidate="txtQty"
                    CssClass="msgError" ErrorMessage="Please enter a valid Quantity" Display="Dynamic" />
            </td>
        </tr>
        <tr style="display:none;">
            <td class="required">
                Free Sample Banner
                <br />
                <span class="smaller">1140 x auto </span>
            </td>
            <td class="field">
                <CC:FileUpload ID="fuImage" AutoResize="true" ImageDisplayFolder="/assets/dealday"
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
                <CC:FileUploadExtensionValidator Extensions="swf,swf,jpg,jpeg,gif,bmp" ID="feImage"
                    runat="server" Display="Dynamic" ControlToValidate="fuImage" CssClass="msgError"
                    ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
            </td>
        </tr>
       <tr style="display:none;">
            <td class="required">
                Free Sample Mobile Banner
                <br />
                <span class="smaller">768 x auto </span>
            </td>
            <td class="field">
                <CC:FileUpload ID="fuMobileImage" AutoResize="true" ImageDisplayFolder="/assets/dealday"
                    DisplayImage="False" runat="server" />
                <div runat="server" id="divMobileImg">
                    <b>Preview with Map:</b><map name="hpMobileimgmap"><asp:Literal runat="server" ID="litMobileMap" /></map>
                    <div>
                        <asp:Image runat="server" ID="hpMobileimg" usemap="#hpMobileimgmap" /></div>
                </div>
            </td>
            <td valign="top">
                <CC:OneClickButton ID="btnSaveMobileImage" runat="server" Text="Save" CssClass="btn">
                </CC:OneClickButton>
            </td>
            <td>
                <CC:FileUploadExtensionValidator Extensions="swf,swf,jpg,jpeg,gif,bmp" ID="feMobileImage"
                    runat="server" Display="Dynamic" ControlToValidate="fuMobileImage" CssClass="msgError"
                    ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
            </td>
        </tr>
    </table>
    <p>
    </p>
    <p>
    </p>
    <div>
        <input type="button" class="btn" id="Button1" value="Add Item Free Sample" onclick="OpenPopUp();" />  <CC:OneClickButton id="btnEditMetaTag" Runat="server" Text="Edit Meta Tags" cssClass="btn"></CC:OneClickButton> </div>
    <p>
    </p>
    <div style="display: none">
        <CC:OneClickButton ID="AddNew" runat="server" Text="Add New SalesPrice Image" CssClass="btn">
        </CC:OneClickButton>
      
        </div>
    <p>
    </p>
    <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50"
        AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links"
        EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False"
        BorderWidth="0" PagerSettings-Position="Bottom">
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
        <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink EnableViewState="False" runat="server" NavigateUrl='<%# "../../store/items/edit.aspx?act=FreeSample&ItemId=" & DataBinder.Eval(Container.DataItem, "ItemId") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                        ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <CC:ConfirmLink EnableViewState="False" Message="Are you sure that you want to remove this Image?"
                        runat="server" NavigateUrl='<%# "delete.aspx?ItemId=" & DataBinder.Eval(Container.DataItem, "ItemId") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                        ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="SKU" SortExpression="SKU" HeaderText="SKU" />
            <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Item Name" />
            <asp:BoundField DataField="QtyOnHand" SortExpression="QtyOnHand" HeaderText="Quantity" />
            <asp:CheckBoxField ItemStyle-HorizontalAlign="Center" SortExpression="IsActive" DataField="IsActive"
                HeaderText="Is Active" />
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Arrange">
                <ItemTemplate>
                    <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif"
                        CommandName="Up" />
                    <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif"
                        CommandName="Down" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </CC:GridView>
    <input type="hidden" runat="server" value="" id="hidPopUpSKU" />
    <input type="hidden" runat="server" value="" id="hidSaveValue" />
    <script type="text/javascript" src="../../../includes/theme-admin/scripts/Browser.js">
    </script>
    <script>
        function SetValue(save, value, isactive) {
            if (save == '1') {
                document.getElementById('<%=hidPopUpSKU.ClientID %>').value = value;
            }
            document.getElementById('<%=hidSaveValue.ClientID %>').value = save;
            $("#<%= AddNew.ClientID %>").click();
        }
        function OpenPopUp() {
            var item = document.getElementById('<%=hidPopUpSKU.ClientID %>').value;
            var url = '../ShopSave/SearchItem.aspx?Type=1&item=' + item
            var brow = GetBrowser();
            if (brow == 'ie') {
                var p = window.showModalDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
               // alert(p);
                document.getElementById('<%=hidPopUpSKU.ClientID %>').value = p;
                $("#<%= AddNew.ClientID %>").click();
            }
            else {
                ShowPopUpDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
            }
        }
    </script>
</asp:Content>
