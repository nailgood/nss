<%@ Control Language="VB" AutoEventWireup="false" CodeFile="list-member-address.ascx.vb"
    Inherits="controls_checkout_list_member_address" %>
<div class="title">
    Select an address below:</div>
<asp:Literal ID="ltrListAddress" runat="server"></asp:Literal>

<div class="add-new">
    <a href="/members/addressbook/edit.aspx?mt=checkout&shippingAddress=<%= IIf(CurrentAddressType = Utility.Common.MemberAddressType.Shipping.ToString(), "1", "0") %>">Use a New Address</a>
</div>
<input type="hidden" id="hidListAddressBookId" runat="server" clientidmode="Static" />


