<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="edit.aspx.vb" Inherits="admin_members_nail_art_trends_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">
<h4>
       View Member Submission</h4>
    <table border="0" cellspacing="1" cellpadding="2">
    <tr>
    <td colspan="2">
       
        <input type="button" name="Back" Class="btn" value="<< Back" onclick="window.history.back()" />
    </td>
    </tr>
        <tr>
            <td class="required">
                Name:
            </td>
            <td class="field">
                <asp:TextBox ID="txtName" runat="server" MaxLength="50" Width="376px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="refvName1" runat="server" CssClass="msgError" ErrorMessage="Name # is blank" ControlToValidate="txtName" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="required">
               Email:
            </td>
            <td class="field">
                  <asp:TextBox ID="txtEmail" runat="server" MaxLength="255" Width="376px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="msgError" ErrorMessage="Email # is blank"
                    ControlToValidate="txtEmail" Display="Dynamic"></asp:RequiredFieldValidator>
                   <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmail" CssClass="msgError" ErrorMessage="Invalid Email Format"></asp:RegularExpressionValidator>
            </td>
            
        </tr>
        <tr>
            <td class="optional">
               Country:
            </td>
            <td class="field">
                <asp:DropDownList ID="drCountry" runat="server"></asp:DropDownList>
             </td>
         </tr>
             <tr>
            <td class="required">
               Art Name:
            </td>
            <td class="field">
                  <asp:TextBox ID="txtArtName" runat="server" MaxLength="100" Width="376px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="msgError" ErrorMessage="Art Name # is blank"
                    ControlToValidate="txtArtName" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            
        </tr>
        <tr>
            <td class="optional">
               Salon Name:
            </td>
            <td class="field">
                 <asp:TextBox ID="txtSalonName" runat="server" MaxLength="255" Width="376px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="optional">
              Instruction:
            </td>
            <td class="field">
                 <asp:TextBox style="WIDTH: 349px" id="txtInstruction" runat="server" Columns="55" rows="5" TextMode="Multiline"></asp:TextBox>
            </td>
        </tr>
          <tr>
		<td class="optional">Submit Date:</td>
		<td class="field">
		    <CC:DatePicker ID="dtSubmitdate" runat="server"></CC:DatePicker>
		    <CC:DateValidator Display="Dynamic" runat="server" id="dtvSubmitdate" ControlToValidate="dtSubmitdate" CssClass="msgError" ErrorMessage="Date Field 'Submit date' is invalid" />
		</td>
	
	</tr>
        <tr>
            <td class="optional">
              Status:
            </td>
            <td class="field">
                    <asp:CheckBox runat="server" ID="chkStatus" Checked="true" />
            </td>
        </tr>
        <tr>
            <td class="optional">
                File Member Upload:
            </td>
            <td class="field">
                <asp:Literal ID="ltImg" runat="server"></asp:Literal>
            </td>
            
        </tr>
        <tr>
            <td class="optional">
                File Admin Upload:
            </td>
            <td class="field">
              <table>
                            <tr>
                            <td><asp:Label id="lbmsg" runat="server" CssClass="msgError"></asp:Label> <CC:FileUpload runat="server" ID="fuImage" Folder="/upload/nail-art-trends/fullupload" DisplayImage="true"
                        ImageDisplayFolder="/upload/nail-art-trends/admin" /></td>
                        </tr>
                        <tr>
                            <td>  <CC:FileUpload runat="server" ID="fuImage1" Folder="/upload/nail-art-trends/fullupload" DisplayImage="true"
                        ImageDisplayFolder="/upload/nail-art-trends/admin" /></td>
                        </tr>
                         <tr>
                            <td>  <CC:FileUpload runat="server" ID="fuImage2" Folder="/upload/nail-art-trends/fullupload" DisplayImage="true"
                        ImageDisplayFolder="/upload/nail-art-trends/admin" /></td>
                        </tr>
              </table>
            </td>
            
        </tr>
        <tr>
            <td colspan="2">
               
                  <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn"></asp:Button>
            </td>
        </tr>
    </table>

</asp:Content>

