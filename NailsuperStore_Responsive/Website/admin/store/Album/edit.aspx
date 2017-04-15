<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" Inherits="admin_store_Album_edit"
    MasterPageFile="~/includes/masterpage/admin.master" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">

    <script type="text/javascript">
        function ConfirmDelete() {
            if (!confirm('Are you sure to delete?')) {
                return false;
            }
        }

    </script>

    <h4>
        <% If Id = 0 Then%>Add<% Else%>Edit<% End If%>
        Album</h4>
    <table border="0" cellspacing="1" cellpadding="2">
        <tr>
            <td colspan="2">
                <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <%--<span class="red">Errore Insert</span> --%>
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </td>
        </tr>
        <%--   <tr>
            <td class="required">
                Item Sku:
            </td>
            <td class="field" width="400">
                <asp:TextBox ID="txtSku" runat="server" MaxLength="255" Columns="50" Style="width: 200px;"></asp:TextBox>
						
            </td>
            <td>
            <asp:Label ID="lbMsgSku" runat="server" Visible="false" CssClass="red"></asp:Label>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="txtSku"
                    ErrorMessage="Field 'Item Sku' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>--%>
        <tr>
            <td class="required">
                Name:
            </td>
            <td class="field">
                <asp:TextBox ID="txtName" runat="server" MaxLength="255" Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvName" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="txtName"
                    ErrorMessage="Field 'Name' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Thumb Image:<br />
                <span class="smaller">176 x 99</span>
            </td>
            <td class="field">
                <CC:FileUpload ID="fuThumb" AutoResize="true" DisplayImage="False" runat="server"
                    Style="width: 475px;" />
                <div runat="server" id="divImg">
                    <b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
                    <div>
                        <asp:Image runat="server" ID="hpimg" usemap="#hpimgmap" /></div>
                </div>
            </td>
            <td>
                <CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp" ID="feImage" runat="server" CssClass="msgError"
                    Display="Dynamic" ControlToValidate="fuImage" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
            </td>
        </tr>
        <tr id="trAlbum" visible="false" runat="server">
            <td class="optional">
            </td>
            <td id="Td1" colspan="2" runat="server">
                <asp:Literal ID="ltrAlbum" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="optional">
                <b>Is Active?</b>
            </td>
            <td class="field">
                <asp:CheckBox runat="server" ID="chkIsActive" Checked="true" />
            </td>
        </tr>
        <tr>
            <td class="optional">
                Description
            </td>
            <td class="field">
                <asp:TextBox ID="txtDescription" runat="server" MaxLength="2000" TextMode="MultiLine"
                    Width="419px" Height="120px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="required">
                Page Title:
            </td>
            <td class="field">
                <asp:TextBox ID="txtPageTitle" runat="server" MaxLength="255" TextMode="SingleLine"
                    Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvPageTitle" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="txtPageTitle"
                    ErrorMessage="Field 'Page Title' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Meta Keywords:
            </td>
            <td class="field">
                <asp:TextBox ID="txtMetaKeyword" runat="server" MaxLength="1000" TextMode="MultiLine"
                    Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvMetaKeyword" runat="server" Display="Dynamic" CssClass="msgError"
                    ControlToValidate="txtMetaKeyword" ErrorMessage="Field 'Meta Keyword' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                Meta Description:
            </td>
            <td class="field">
                <asp:TextBox ID="txtMetaDescription" runat="server" MaxLength="1000" TextMode="MultiLine"
                    Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvMetaDescription" runat="server" Display="Dynamic" CssClass="msgError"
                    ControlToValidate="txtMetaDescription" ErrorMessage="Field 'Meta Description' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="optional">
                Song:
            </td>
            <td class="optional" colspan="2">
                <asp:Literal ID="ltSong" runat="server"></asp:Literal>
            </td>
        </tr>
    </table>
    <p>
    </p>
    <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
    <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
    </CC:OneClickButton>
    <span id="sp1" runat="server" visible="false">
        <input type="button" class="btn" runat="server" id="Button1" value="Add Song" onclick="OpenPopUp();" /></span>
    <br />
    &nbsp;
    <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50"
        AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria"
        AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
        <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
        <Columns>
            <asp:BoundField DataField="Name" HeaderStyle-Width="250" HeaderText="Album Name">
            </asp:BoundField>
            <%-- <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active">
                <ItemTemplate>
                    <asp:ImageButton ID="imbActive" runat="server" ImageUrl="/images/admin/active.png"
                        CommandName="Active" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "AlbumId")%>' />
                </ItemTemplate>
            </asp:TemplateField>--%>
            <%--  <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Edit">
                <ItemTemplate>
                    <asp:HyperLink EnableViewState="False" runat="server" NavigateUrl='<%# "../album/edit.aspx?id=" & DataBinder.Eval(Container.DataItem, "AlbumId") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                        ImageUrl="/images/admin/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>--%>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete">
                <ItemTemplate>
                    <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/images/admin/delete.gif"
                        CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "SongId")%>'
                        OnClientClick="return ConfirmDelete();" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Arrange">
                <ItemTemplate>
                    <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/images/admin/MoveUp.gif" CommandName="Up"
                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "SongId")%>' />
                    <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/images/admin/MoveDown.gif"
                        CommandName="Down" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "SongId")%>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </CC:GridView>
    <div style="display: none">
        <CC:OneClickButton ID="btnAddSong" runat="server" Text="Add Song" CssClass="btn"
            CausesValidation="False"></CC:OneClickButton>
    </div>
    <input type="hidden" runat="server" value="" id="hidPopUpSong" name="hidPopUpSong" />
    <input type="hidden" runat="server" value="" id="hidSaveValue" />

    <script type="text/javascript" src="../../../includes/theme-admin/scripts/Browser.js">
    </script>

    <script>
        function SetValue(save, value) {
            if (save == '1') {
                document.getElementById('<%=hidPopUpSong.ClientID %>').value = value;
            }
            document.getElementById('<%=hidSaveValue.ClientID %>').value = save;

        }

        function OpenPopUp() {
            var brow = GetBrowser();
            var Song = document.getElementById('<%=hidPopUpSong.ClientID %>').value;
            var url = 'MusicSearch.aspx?Type=2&Song=' + Song;
            var p = window.showModalDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');

            if (brow == 'ie') {
                if (typeof p != "undefined") {
                    if (p != '') {
                        document.getElementById('<%=hidPopUpSong.ClientID %>').value = p;
                        var button = document.getElementById('<%=btnAddSong.ClientID %>');
                        if (button)
                            button.click();
                    }
                }
            }
            else {
                var saveValue = document.getElementById('<%=hidSaveValue.ClientID %>').value;
                if (saveValue == '1') {
                    var button = document.getElementById('<%=btnAddSong.ClientID %>');
                    if (button)
                        button.click();
                }
            }

        }
    </script>

</asp:Content>
