<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master"
    AutoEventWireup="false" CodeFile="edit.aspx.vb" Inherits="admin_promotions_FreeGift_level_edit" %>

<asp:Content ID="Content" runat="server" ContentPlaceHolderID="ph">
    <h4>
        Free Gift Level</h4>
    <table>
        <tr>
            <td valign="top" colspan="2">
                <font class="red">red color</font>- required fields
            </td>
        </tr>
        <tr>
            <td class="required" style="width: 80px;">
                <strong>Name:</strong>
            </td>
            <td class="field">
                <asp:TextBox ID="txtName" runat="server" Width="300px"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="refvName" ValidationGroup="level" runat="server"
                    CssClass="msgError" ErrorMessage="Name cannot be blank" ControlToValidate="txtName"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
                <strong>Min Value:</strong>
            </td>
            <td class="field">
                <asp:TextBox ID="txtMinValue" runat="server" Width="100px"></asp:TextBox>
            </td>
            <td>
                <asp:CustomValidator ID="cusvMinValue" ControlToValidate="txtMinValue" EnableClientScript="true"
                    ValidationGroup="level" ErrorMessage="Min Value is not valid" CssClass="msgError"
                    ValidateEmptyText="true" ClientValidationFunction="CheckMinValue" runat="server"> 
                </asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td class="optional">
                <strong>Max Value:</strong>
            </td>
            <td class="field">
                <asp:TextBox ID="txtMaxValue" runat="server" Width="100px"></asp:TextBox>
            </td>
            <td>
                <asp:CustomValidator ID="cusvMaxValue" ControlToValidate="txtMaxValue" EnableClientScript="true"
                    ValidationGroup="level" ErrorMessage="Max Value is not valid" CssClass="msgError"
                    ValidateEmptyText="true" ClientValidationFunction="CheckMaxValue" runat="server"> 
                </asp:CustomValidator>
            </td>
        </tr>
     <%--   <tr>
            <td class="optional">
                Image File:<br />
                150X88
            </td>
            <td class="field">
                <CC:FileUpload ID="fuImage" EnableDelete="false" AutoResize="true" Folder="/assets/Banner/"
                    ImageDisplayFolder="/assets/Banner/" DisplayImage="false" runat="server" />
                <div runat="server" id="divImg">
                    <b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
                    <div>
                        <asp:Image runat="server" ID="hpimg" usemap="#hpimgmap" /></div>
                </div>
            </td>
            <td>
                <CC:FileUploadExtensionValidator Extensions="swf,swf,jpg,jpeg,gif,bmp" ID="feImage"
                    runat="server" Display="Dynamic" ValidationGroup="inforbanner" ControlToValidate="fuImage"
                    CssClass="msgError" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
                <span style="color: Red">
                    <asp:Literal ID="ltMssImage" runat="server"></asp:Literal></span>
            </td>
        </tr>--%>
        <tr>
            <td class="optional">
                Is Active
            </td>
            <td class="field">
                <asp:CheckBox ID="chkIsActive" runat="server" AutoPostBack="True" Text="" />
            </td>
            <td>
            </td>
        </tr>
    </table>
    <p>
        <CC:OneClickButton ID="btnSave" runat="server" CausesValidation="true" ValidationGroup="level"
            CssClass="btn" Text="Save"></CC:OneClickButton>&nbsp;
        <CC:OneClickButton ID="btnCancel" runat="server" CssClass="btn" Text="Cancel" CausesValidation="False">
        </CC:OneClickButton>
    </p>
    <script type="text/javascript">

        function CheckMinValue(source, arguments) {
            var number = arguments.Value;
            if (number == '') {
                arguments.IsValid = false;
                source.innerText = source.innerHTML = "Min Value cannot be blank";
                return;
            }
            if (!isDoubleValid(arguments.Value)) {
                arguments.IsValid = false;
                source.innerText = source.innerHTML = "Min Value is not valid";
                return;
            }
            arguments.IsValid = true
        }

        function CheckMaxValue(source, arguments) {
            var number = arguments.Value;
            if (number == '') {
                arguments.IsValid = true;
                return;
            }
            if (!isDoubleValid(arguments.Value)) {
                arguments.IsValid = false;
                source.innerText = source.innerHTML = "Max Value is not valid";
                return;
            }
            arguments.IsValid = true;
        }
        function isDoubleValid(value) {

            var doubleFormat = /^[0-9.]*$/;
            if (!doubleFormat.test(value)) {
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
