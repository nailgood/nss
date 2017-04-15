<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="edit.aspx.vb" Inherits="admin_NewsEvent_NewsFeed_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">
<h4>
     <h4>
       <% If NewsFeedId = 0 Then%>
        Add<% Else %>Edit<% End If %>
        News Feed</h4>
    <table border="0" cellspacing="1" cellpadding="2">
    <tr>
    <td colspan="2">
           <input type="button" name="Back" Class="btn" value="<< Back" onclick="window.history.back()" />
    </td>
    </tr>
         <tr>
            <td class="required">
                <b>Title:</b>
            </td>
            <td class="field">
                <asp:TextBox ID="txtTitle" runat="server" MaxLength="500" Width="376px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="refvTitle" runat="server" CssClass="msgError" ErrorMessage="Title # is blank"
                    ControlToValidate="txtTitle" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            
        </tr>
        <tr>
            <td class="optional">
               Short Content:
            </td>
            <td class="field" width="545">
               
                    <asp:TextBox ID="txtShortContent" runat="server" TextMode="MultiLine" Width="540px" Rows="5"></asp:TextBox>
                    <span class="smaller">Please use shift+enter to insert a line break instead of a paragraph.</span>
            </td>
            
        </tr>
        <tr>
            <td class="optional">
               Url:
            </td>
            <td class="field">
                <asp:TextBox ID="txtUrl" runat="server" MaxLength="500" Width="376px"></asp:TextBox>
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
              Is Active:
            </td>
            <td class="field">
                    <asp:CheckBox runat="server" ID="chkIsActive" Checked="true" />
            </td>
        </tr>
      <tr>
            <td class="optional">
                Image:<br />
               <%-- <span class="smaller">475 x 205</span>--%>
            </td>
            <td class="field">
                <CC:FileUpload ID="fuImage" Folder="/assets/NewsImage/newsfeed/fullupload" ImageDisplayFolder="/assets/NewsImage/newsfeed/fullupload"
                    DisplayImage="True" runat="server" Style="width: 475px;" />
              <br /><%--<img src="<%=ImagePath %>" />--%>
            </td>
           
        </tr>
      
                     <tr>
                        <td align="right" colspan="2">
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn"></asp:Button>
                        </td>
                    </tr>
    </table>

</asp:Content>