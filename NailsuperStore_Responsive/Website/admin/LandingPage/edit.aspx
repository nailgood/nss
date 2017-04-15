<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="edit.aspx.vb" Inherits="admin_LandingPage_edit" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">
<h4>
  <% If Id = 0 Then%>Add<% Else%>Edit<% End If%>
    Landing Page
</h4>

<table border="0" cellspacing="1" cellpadding="2">
    <tr>
        <td class="required">
            Title:
        </td>
        <td class="field">
            <asp:TextBox ID="txtTitle" runat="server"  MaxLength="255" Columns="50" Style="width: 419px;"></asp:TextBox>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="rfvTitle" runat="server" Display="Dynamic" ControlToValidate="txtTitle"
                CssClass="msgError" ErrorMessage="Field 'Title' is blank"></asp:RequiredFieldValidator>
        </td>
    </tr>        
    <tr>
        <td class="required">
            URL Code:
        </td>
        <td class="field">
            <asp:TextBox ID="txtURLCode" runat="server"  MaxLength="255" Columns="50" Style="width: 419px;"></asp:TextBox>
        </td>
        <td>
<%--            <asp:RequiredFieldValidator ID="rfvURLCode" runat="server" Display="Dynamic" ControlToValidate="txtURLCode"
                CssClass="msgError" ErrorMessage="Field 'URL Code' is blank"></asp:RequiredFieldValidator>
--%>        </td>
    </tr>        
    <tr>
		<td class="optional">Item</td>
		<td class="field">
		    <asp:textbox id="txtSKU" runat="server" maxlength="8" columns="8" style="width: 67px;" Enabled="false" ></asp:textbox> <input type="button" class="btn" id="btnAddSKu" value="Add SKU" onclick="OpenPopUp();" />
		    <input type="button" class="btn" id="btnRemove" value="Remove SKU" onclick="RemoveItem();" />
		</td>
		<td>
                <asp:Label ID="lbError" runat="server" CssClass="red"></asp:Label><%--<asp:RequiredFieldValidator
                    ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="txtSKU"
                    CssClass="msgError" ErrorMessage="Please add Item for Landing Page"></asp:RequiredFieldValidator>--%>
        </td>
	</tr>
	<tr>
		<td class="optional">Customer Price Group:</td>
		<td class="field"><asp:dropdownlist id="drpCustomerPriceGroupId" runat="server" /></td>
		<td></td>
	</tr>
	<tr>
        <td class="required">
            File Location:
        </td>
        <td class="field">
            <asp:TextBox ID="txtFileLocation" runat="server"  MaxLength="255" Columns="50" Style="width: 419px;"></asp:TextBox>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="rfvFileLocation" runat="server" Display="Dynamic" ControlToValidate="txtFileLocation"
                CssClass="msgError" ErrorMessage="Field 'File Location' is blank"></asp:RequiredFieldValidator>
        </td>
    </tr>      
    <tr>
        <td class="required">
            Start Date:
        </td>
        <td class="field">
            <CC:DatePicker ID="dprStartDate" runat="server" />
        <td>
            <CC:RequiredDateValidator ID="rfvStartDate" runat="server" Display="Dynamic" ControlToValidate="dprStartDate"
                CssClass="msgError" ErrorMessage="Field 'Start Date' is blank"></CC:RequiredDateValidator>
            <CC:DateValidator Display="Dynamic" runat="server" id="dtvStartDate" ControlToValidate="dprStartDate" 
                CssClass="msgError" ErrorMessage="Date Field 'Start Date' is invalid" />
        </td>
     </tr>
     <tr>
        <td class="required">
            End Date:
        </td>
        <td class="field">
            <CC:DatePicker ID="dprEndDate" runat="server" />
        </td>
        <td>
            <CC:RequiredDateValidator ID="rfvEndDate" runat="server" Display="Dynamic" ControlToValidate="dprEndDate"
                CssClass="msgError" ErrorMessage="Field 'End Date' is blank"></CC:RequiredDateValidator>
            <CC:DateValidator Display="Dynamic" runat="server" id="dtvEndDate" ControlToValidate="dprEndDate" 
                CssClass="msgError" ErrorMessage="Date Field 'End Date' is invalid" />
        </td>
     </tr>     
     <tr>
        <td class="optional">
            URL Return:
        </td>
        <td class="field">
            <asp:TextBox ID="txtUrlReturn" runat="server"  MaxLength="500" Columns="50" Style="width: 419px;"></asp:TextBox>
        </td>
        <td>
        </td>
    </tr>     
     <tr>
        <td class="required">
            Page Title:
        </td>
        <td class="field">
            <asp:TextBox ID="txtPageTitle" runat="server" MaxLength="1000" TextMode="SingleLine"
                Columns="50" Style="width: 419px;"></asp:TextBox>
            <div class="smaller" style="margin-top: 2px">
                <b>Page Titles</b> help your store items' rankings in search engines like <b style="border-right: #666666 1px solid;
                    border-top: #666666 1px solid; background: #ffffff; border-left: #666666 1px solid;
                    border-bottom: #666666 1px solid">&nbsp;<b style="color: #1111ff">G</b><b style="color: #ff1111">o</b><b
                        style="color: #ddbb00">o</b><b style="color: #1111ff">g</b><b style="color: #00ad00">l</b><b
                            style="color: #ff1111">e</b>&nbsp;</b> and <b style="border-right: #666666 1px solid;
                                border-top: #666666 1px solid; background: #ffffff; border-left: #666666 1px solid;
                                border-bottom: #666666 1px solid">&nbsp;<b style="color: #ff0000">Yahoo!</b>&nbsp;</b></div>

        </td>
        <td>
         <asp:RequiredFieldValidator ID="refvPageTitle" runat="server" Display="Dynamic" ControlToValidate="txtPageTitle"
                CssClass="msgError" ErrorMessage="Field 'Page Title' is blank"></asp:RequiredFieldValidator>
        </td>
    </tr>    
    <tr>
        <td class="required">
            Meta Description:
        </td>
        <td class="field">
            <asp:TextBox ID="txtMetaDescription" runat="server" MaxLength="2000" TextMode="MultiLine"
                Columns="50" Style="width: 419px;"></asp:TextBox>
        </td>
        <td>
         <asp:RequiredFieldValidator ID="refvMetaDescription" runat="server" Display="Dynamic" ControlToValidate="txtMetaDescription"
                CssClass="msgError" ErrorMessage="Field 'Meta Description' is blank"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="required">
            Meta Keywords:
        </td>
        <td class="field">
            <asp:TextBox ID="txtMetaKeyword" runat="server" MaxLength="2000" TextMode="MultiLine"
                Columns="50" Style="width: 419px;"></asp:TextBox>
        </td>
        <td>
         <asp:RequiredFieldValidator ID="refvMetaKeyword" runat="server" Display="Dynamic" ControlToValidate="txtMetaKeyword"
                CssClass="msgError" ErrorMessage="Field 'Meta Keyword' is blank"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="optional">
            <b>Google Analytics Experiment Code</b>
        </td>
        <td class="field">
           <asp:TextBox ID="txtGoogleABCode" runat="server" MaxLength="2000" TextMode="MultiLine"
                Columns="50" Style="width: 419px;height:130px;"></asp:TextBox>
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
</table>
<p id="pnControlBottom">    
    <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
    <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
    </CC:OneClickButton>
</p>
    <input type="hidden" runat="server" value="" id="hidPopUpSKU" />
    <input type="hidden" runat="server" value="" id="hidItemId" />
<script>
        function OpenPopUp() {

            var item = document.getElementById('<%=hidPopUpSKU.ClientID %>').value;
            var itemid = document.getElementById('<%=hidItemId.ClientID %>').value;
            var sku = document.getElementById('<%=txtSKU.ClientID %>').value;
            var url = '../promotions/ShopSave/SearchItem.aspx?Type=0&item=' + item
            var p = window.showModalDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
            var brow = GetBrowser();
            if (brow == 'ie') {
                if (typeof p != "undefined") {
                    if (p != '') {
                        document.getElementById('<%=hidPopUpSKU.ClientID %>').value = p;
                        document.getElementById('<%=txtSKU.ClientID %>').value = p;
                    }
                }
            }
            
          
        }
        function SetValue(save, value) {
            if (save == '1') {
                document.getElementById('<%=hidPopUpSKU.ClientID %>').value = value;
                document.getElementById('<%=txtSKU.ClientID %>').value = value;
            }      
        }
        function RemoveItem() {
            document.getElementById('<%=hidPopUpSKU.ClientID %>').value = '';
            document.getElementById('<%=txtSKU.ClientID %>').value = '';
        }
</script>
</asp:Content>

