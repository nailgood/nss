<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="fedex.aspx.vb" Inherits="admin_fedex" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
<h4>Shipping Calculating</h4>
<style type="text/css">
    
.result
{
    padding:5px;
    margin:5px;
    border:solid Gray 1px;
    width:auto;
    font:12px/18px Arial;
    width:500px;
    background-color:#dedede;
}

.can
{
	border:solid Black 1px;
	background-color:Silver;
	padding:5px;
	margin-bottom:10px;
}

ul.item
{
	border:solid Black 1px;
	background-color:#b7cde6;
	padding:5px;
	margin-left:10px;
	list-style-position:inside;
}
.hide
{
    display:none
    }
.show
{
    display:block;
    }
</style> 
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="up" runat="server">
<ContentTemplate>

<table cellpadding="2" cellspacing="2">
    <tr><td style="width:150px">Shipping Type: </td>
    <td>
        <asp:DropDownList ID="drpShippingType" runat="server">
                    <asp:ListItem Value="16">Ground</asp:ListItem>
                    <asp:ListItem Value="17">Two Day</asp:ListItem>
                    <asp:ListItem Value="18">Next Day</asp:ListItem>
                    <%--<asp:ListItem Value="19">Freight Delivery</asp:ListItem>--%>
        </asp:DropDownList></td>
    </tr>
    <tr class="account-nav">
        <td>Weight <span class="red">*</span>:</td>
        <td><asp:TextBox ID="txtWeight" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Weight is required" ControlToValidate="txtWeight" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cv1" runat="server" ControlToValidate="txtWeight" Type="Double" Operator="DataTypeCheck" ErrorMessage="Weight must be numeric" />
        </td>
    </tr>
    <tr>
        <td>Zip <span class="red">*</span>:</td>
        <td><asp:TextBox ID="txtZip" runat="server"></asp:TextBox>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Zipcode is required" ControlToValidate="txtZip" Display="Dynamic"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr><td>Country:</td><td><asp:TextBox ID="txtCountry" runat="server" Text="US" Enabled="false"></asp:TextBox></td></tr>
    <tr>
        <td></td>
        <td>
            <asp:CheckBox ID="chkResidentialAddress" runat="server" Text="Residential Address" AutoPostBack="true" OnCheckedChanged="chkResidentialAddress_Change" />
            <asp:CheckBox ID="chkResidentialFee" runat="server" Text="Residential Fee" Visible="false" />
        </td>
    </tr>
    <tr>
        <td></td>
        <td><asp:CheckBox ID="chkSignature" runat="server" Text="Signature Confirmation" /></td>
    </tr>
    <tr>
        <td></td>
        <td><asp:CheckBox ID="chkInsurance" runat="server" OnClick="return ShowSubtotal();" Text="Shipping Insurance" /></td>
    </tr>
    <tr>
        <td colspan="2"> <div id="tr2" class="hide">
            <div style="float:left;width:155px">SubTotal order:</div>
            <div><asp:TextBox ID="txtSubTotal" runat="server" text="0" Width="50px"></asp:TextBox> <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtSubTotal" Type="Double" Operator="DataTypeCheck" ErrorMessage="Value must be a number!" /></div>
        </td>
    </tr>
   
  
   
    <tr id="tr1" runat="server" visible="false">
        <td>Total:</td>
        <td><asp:Label ID="lbTotal" runat="server"></asp:Label></td>
    </tr>
    <tr>
    <td></td>
    <td><asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btnSubmit_Click" /></td>
    </tr>
</table>
<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="up">
    <ProgressTemplate>
        <center>
        Please wait...<br /><img src="/includes/theme-admin/images/loader.gif" alt="" />
        </center>
    </ProgressTemplate>
</asp:UpdateProgress>
     <div style="clear:both" class="result">
    <h1>UPS</h1>
    <asp:Literal ID="ltrResult1" runat="server"></asp:Literal>
    </div>
    <div class="result">
    <div><h1>Fedex</h1></div>
    <asp:Literal ID="ltrResult" runat="server"></asp:Literal>
    </div>
       <script type="text/javascript">
           var chkInsurance = document.getElementById('<%=chkInsurance.ClientId%>');
           if (chkInsurance.checked) {
               document.getElementById('tr2').className = 'show';
           }
           function ShowSubtotal() {

               if (chkInsurance.checked) {
                   document.getElementById('tr2').className = 'show';
               }
               else {
                   document.getElementById('tr2').className = 'hide';
               }

           }
</script>
</ContentTemplate>
</asp:UpdatePanel>



    </asp:Content>
