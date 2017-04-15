<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false"
    CodeFile="shipvalid.aspx.vb" Inherits="admin_ShipValid" %>
    
<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
<h4>Shipping Validator</h4>
<style type="text/css">
.result
{
    padding:5px;
    margin:5px;
    border:solid Gray 1px;
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

</style>
<table cellpadding="2" cellspacing="2">
    <tr>
        <td style="width:150px">Full Name: </td>
        <td><asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td>Salon Name: </td>
        <td><asp:TextBox ID="txtSalonName" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td>Address: </td>
        <td><asp:TextBox ID="txtAddress" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td>City:</td>
        <td><asp:TextBox ID="txtCity" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td>State:</td>
        <td><asp:TextBox ID="txtState" runat="server" Text="CA"></asp:TextBox></td>
    </tr>
    <tr>
        <td>Zip:</td>
        <td><asp:TextBox ID="txtZip" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td>Country:</td>
        <td><asp:TextBox ID="txtCountry" runat="server" Text="US"></asp:TextBox></td>
    </tr>
    <tr>
        <td></td>
        <td>
            <asp:RadioButton ID="radUPS" runat="server" Text="UPS" GroupName="ship" Checked="True"></asp:RadioButton>
            <asp:RadioButton ID="radFedEx" runat="server" Text="FedEx" GroupName="ship"></asp:RadioButton>
        </td>
    </tr>
    <tr>
        <td></td>
        <td><asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btnSubmit_Click" /></td>
    </tr>
</table>
       
<div class="result"><asp:Literal ID="ltrResult" runat="server"></asp:Literal></div>
<div>
<asp:Label ID="lblocation" runat="server"></asp:Label>
</div>
</asp:Content>