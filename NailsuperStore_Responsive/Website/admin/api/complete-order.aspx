<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="complete-order.aspx.vb" Inherits="admin_complete_order" %>
    
<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
<div style="float:left;width:380px">

<h4>Complete Order</h4>
<table cellpadding="2" cellspacing="2" width="100%">
    <tr>
        <td style="background:Silver;text-align:center;padding:5px;" colspan="2"><strong>Checkout by Paypal</strong></td>
    </tr>
    <tr>
        <th style="width:150px">Order Id: </th>
        <td class="field"><asp:TextBox ID="txtOrderId" RunAt="Server" Width="150px"></asp:TextBox></td>
    </tr>
    <tr>
        <th>Member Id:</th>
        <td class="field"><asp:TextBox ID="txtMemberId" RunAt="Server" Width="150px"></asp:TextBox></td>
    </tr>
    <tr>
        <th>Payment Status:</th>
        <td class="field"><asp:DropDownList ID="drpPaymentStatus" RunAt="Server" Width="150px">
            <asp:ListItem Text="Completed" Value="Completed"></asp:ListItem>
            <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
        </asp:DropDownList></td>
    </tr>
    <tr>
        <th>Payment Type:</th>
        <td class="field"><asp:DropDownList ID="drpPaymentType" RunAt="Server" Width="150px">
            <asp:ListItem Text="instant" Value="Completed"></asp:ListItem>
            <asp:ListItem Text="echeck" Value="Pending"></asp:ListItem>
        </asp:DropDownList></td>
    </tr>
    <tr>
        <th>Purchase Point:</th>
        <td class="field"><asp:TextBox ID="txtPurchasePoint" Runat="Server" Width="150px"></asp:TextBox> pts</td>
    </tr>
    <tr>
        <th></th>
        <td class="field"><asp:CheckBox ID="chkSend" runat="server" Checked="true" Text="Send email confirmation" /></td>
    </tr>
    <tr>
        <th></th>
        <td class="field"><asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btnSubmit_Click" /></td>
    </tr>
</table>

<br /><br />
<table cellpadding="2" cellspacing="2" width="100%">
    <tr>
        <td style="background:Silver;padding:5px;text-align:center" colspan="2"><strong>Resend email confirmation order</strong></td>
    </tr>
    <tr>
        <th style="width:150px">Order No: </th>
        <td class="field"><asp:TextBox ID="txtOrderNo" RunAt="Server" Width="150px"></asp:TextBox></td>
    </tr>
    <tr>
        <th></th>
        <td class="field"><asp:Button ID="btnSubmitCon" Text="Submit" runat="server" OnClick="btnSubmitCon_Click" /></td>
    </tr>
</table>

<br /><br />
<table cellpadding="2" cellspacing="2" width="100%">
    <tr>
        <td style="background:Silver;text-align:center;padding:5px;" colspan="2"><strong>Checkout by Credit Card</strong></td>
    </tr>
     <tr>
        <th style="width:150px">Order Id: </th>
        <td class="field"><asp:TextBox ID="txtOrderIdCr" RunAt="Server" Width="150px"></asp:TextBox></td>
    </tr>
    <tr>
        <th>Member Id:</th>
        <td class="field"><asp:TextBox ID="txtMemberIdCr" RunAt="Server" Width="150px"></asp:TextBox></td>
    </tr>
     <tr>
        <th>Card Name:  </th>
        <td class="field"><asp:TextBox ID="txtCardName" RunAt="Server" Width="150px"></asp:TextBox></td>
    </tr>
    <tr>
        <th>Card Number:  </th>
        <td class="field"><asp:TextBox ID="txtCardNum" RunAt="Server" Width="150px"></asp:TextBox></td>
    </tr>
      <tr>
        <th>Card Type: </th>
        <td class="field"><asp:DropDownList ID="drType" runat="server" Width="150px">
        <asp:ListItem Selected="True">--Select--</asp:ListItem>
        <asp:ListItem Value="V">Visa</asp:ListItem>
        <asp:ListItem Value="M">MasterCard</asp:ListItem>
        <asp:ListItem Value="D">Discover</asp:ListItem>
        </asp:DropDownList></td>
    </tr>
      <tr>
        <th>Exp Date: </th>
        <td class="field"> 
        <asp:DropDownList ID="drMonth" runat="server">
        <asp:ListItem Selected="True"></asp:ListItem>
        <asp:ListItem Value="1">January</asp:ListItem>
        <asp:ListItem Value="2">February</asp:ListItem>
        <asp:ListItem Value="3">March</asp:ListItem>
        <asp:ListItem Value="4">April</asp:ListItem>
        <asp:ListItem Value="5">May</asp:ListItem>
        <asp:ListItem Value="6">June</asp:ListItem>
        <asp:ListItem Value="7">July</asp:ListItem>
        <asp:ListItem Value="8">August</asp:ListItem>
        <asp:ListItem Value="9">September</asp:ListItem>
        <asp:ListItem Value="10">October</asp:ListItem>
        <asp:ListItem Value="11">November</asp:ListItem>
        <asp:ListItem Value="12">December</asp:ListItem></asp:DropDownList>
        <asp:DropDownList ID="drYear" runat="server">
        <asp:ListItem Selected="True"></asp:ListItem>
        <asp:ListItem Value="2012">2012</asp:ListItem>
        <asp:ListItem Value="2013">2013</asp:ListItem>
        <asp:ListItem Value="2014">2014</asp:ListItem>
        <asp:ListItem Value="2015">2015</asp:ListItem>
        <asp:ListItem Value="2016">2016</asp:ListItem>
        <asp:ListItem Value="2017">2017</asp:ListItem>
        <asp:ListItem Value="2018">2018</asp:ListItem>
        <asp:ListItem Value="2019">2019</asp:ListItem>
        <asp:ListItem Value="2020">2020</asp:ListItem>
        <asp:ListItem Value="2021">2021</asp:ListItem>
        <asp:ListItem Value="2022">2022</asp:ListItem>
        <asp:ListItem Value="2023">2023</asp:ListItem>
        </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th>CVV2:  </th>
        <td class="field"><asp:TextBox ID="txtCardID" RunAt="Server" Width="150px"></asp:TextBox></td>
    </tr>
    <tr>
        <th>Purchase Point:</th>
        <td class="field"><asp:TextBox ID="txtPurchasePointCr" Runat="Server" Width="150px" ></asp:TextBox> pts</td>
    </tr>
     <tr>
        <th></th>
        <td class="field"><asp:Button ID="btnCredit" Text="Submit" runat="server" /></td>
    </tr>
    
 </table>
</div>   
<div class="result" style="float:left;"><asp:Label ID="lblResult" RunAt="Server"></asp:Label></div> 
</asp:Content>