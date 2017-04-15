<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="USPS.aspx.vb" Inherits="admin_api_USPS" %>

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
</style> <table cellpadding="2" cellspacing="2">
  
    <tr class="account-nav">
        <td>Weight <span class="red">*</span>:</td>
        <td><asp:TextBox ID="txtWeight" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Weight is blank" ControlToValidate="txtWeight" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cv1" runat="server" ControlToValidate="txtWeight" Type="Double" Operator="DataTypeCheck" ErrorMessage="Value must be a number!" />
        </td>
    </tr>
    <tr><td>Country Code:</td><td><asp:TextBox ID="txtCountry" runat="server" Text="VI"></asp:TextBox></td></tr>
    <tr>
    <td></td>
    <td><asp:Button ID="btnSubmit" Text="Submit" runat="server" /></td>
    </tr>
</table>
    
     <div style="clear:both" class="result">
    <h1>UPSP</h1>
    <asp:Literal ID="ltrResult1" runat="server"></asp:Literal>
    </div>



    </asp:Content>
