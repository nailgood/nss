<%@ Page Language="VB" AutoEventWireup="false" CodeFile="editSong.aspx.vb" Inherits="admin_store_Album_editSong"
    MasterPageFile="~/includes/masterpage/admin.master" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>
        <% If Id = 0 Then%>Add<% Else%>Edit<% End If%>
        Song</h4>
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
        <tr>
            <td class="required">
                Name:
            </td>
            <td class="field">
                <asp:TextBox ID="txtName" runat="server" MaxLength="255" Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="msgError" Display="Dynamic" ControlToValidate="txtName"
                    ErrorMessage="Field 'Name' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                File Url:
            </td>
            <td class="field" width="400">
                <asp:TextBox ID="txtUrl" runat="server" MaxLength="255" Columns="50" Style="width: 419px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" CssClass="msgError"
                    ControlToValidate="txtUrl" ErrorMessage="Field 'File Url' is blank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                File Lenght:
            </td>
            <td class="field" width="400">
                <asp:TextBox ID="txtLenght" runat="server" MaxLength="255" Columns="50" Style="width: 100px;"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" CssClass="msgError"
                    ControlToValidate="txtLenght" ErrorMessage="Field 'File Lenght' is blank"></asp:RequiredFieldValidator>
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
                <b>Album</b>
            </td>
            <td class="optional">
                <asp:Literal ID="ltAlbum" runat="server"></asp:Literal>
            </td>
        </tr>
    </table>
    <p>
    </p>
    <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
    <CC:OneClickButton ID="btnSaveAdd" runat="server" Text="Save & Add" CssClass="btn">
    </CC:OneClickButton>
    <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
    </CC:OneClickButton>
    <input type="button" class="btn" id="Button1" value="Add Album" onclick="OpenPopUp();" />
    <div style="display: none">
        <CC:OneClickButton ID="btnAddAlbum" runat="server" Text="Add Album" CssClass="btn"
            CausesValidation="False"></CC:OneClickButton>
    </div>
    <input type="hidden" runat="server" value="" id="hidPopUpAlbum" />
    <input type="hidden" runat="server" value="" id="hidSaveValue" />

    <script type="text/javascript" src="../../../includes/theme-admin/scripts/Browser.js">
    </script>

    <script>
        function SetValue(save, value) {
            if (save == '1') {
                document.getElementById('<%=hidPopUpAlbum.ClientID %>').value = value;
            }
            document.getElementById('<%=hidSaveValue.ClientID %>').value = save;

        }
        function OpenPopUp() {
            var brow = GetBrowser();
            var Album = document.getElementById('<%=hidPopUpAlbum.ClientID %>').value;
            var url = 'MusicSearch.aspx?Type=1&Album=' + Album
            var p = window.showModalDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
            if (brow == 'ie') {
                if (typeof p != "undefined") {
                    if (p != '') {
                        document.getElementById('<%=hidPopUpAlbum.ClientID %>').value = p;
                        var button = document.getElementById('<%=btnAddAlbum.ClientID %>');
                        if (button)
                            button.click();
                    }
                }
            }
            else {

                var saveValue = document.getElementById('<%=hidSaveValue.ClientID %>').value;                
                if (saveValue == '1') {
                    var button = document.getElementById('<%=btnAddAlbum.ClientID %>');
                    if (button)
                        button.click();
                }
            }



        }
    </script>

</asp:Content>
