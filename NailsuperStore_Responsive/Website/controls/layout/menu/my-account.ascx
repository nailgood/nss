<%@ Control Language="VB" AutoEventWireup="false" CodeFile="my-account.ascx.vb" Inherits="controls_layout_menu_my_account" %>
<nav class="left-nav">
    <div id="divMyAccountMenu" class="titleroot">My Account</div>
    <ul id="customer-service">
        <% If String.IsNullOrEmpty(Args) Then%>
        <%= GenerateGroupMenu("Account Details", "/members/default.aspx")%>
        <%= GenerateGroupMenu("Edit Account", "/members/address.aspx")%>
         <%= GenerateGroupMenu("Change Password", "/members/account.aspx")%>
          <%= GenerateGroupMenu("Unsubscribe Email ", "/members/unsubscribe.aspx")%>
            <%= GenerateGroupMenu("Order History", "/members/orderhistory/")%>
          <%= GenerateGroupMenu("Purchased Product History", "/members/purchased-product.aspx")%>
          <%= GenerateGroupMenu("Order / Product Review", "/members/leavereview.aspx")%>
          <%= GenerateGroupMenu("Credit Memo", "/members/creditmemo/")%>
          <%= GenerateGroupMenu("Refer Friends", "/members/referfriend/manager.aspx")%>
         <%If Not DataLayer.MemberRow.MemberInGroupWHS(memberID) Then%> <%=GenerateGroupMenu("Cash Reward Points Balance", "/members/pointbalance.aspx") %><% End If%>
          <%= GenerateGroupMenu("Address Book", "/members/addressbook/")%>
          <%= GenerateGroupMenu("Sign Out", "/members/logout.aspx")%>
         
       <%End If%>  
    </ul>
</nav>
<script type="text/javascript">
    $(document).ready(function () {
        CheckShowBreadCrumbMenuPopup('divMyAccountMenu', 'My Account');
    });
    $(window).resize(function () {
        CheckShowBreadCrumbMenuPopup('divMyAccountMenu', 'My Account');
    });
</script>