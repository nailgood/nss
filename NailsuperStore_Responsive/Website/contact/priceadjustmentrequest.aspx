<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/interior.master"
    CodeFile="priceadjustmentrequest.aspx.vb" Inherits="contact_priceadjustmentrequest" %>

<%@ Register Src="~/controls/layout/form-contact.ascx" TagName="contact" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div id="contact_form" class="form-horizontal" role="form" runat="server">
        <div class="panel-content">
            <h1 class="formtitle">
                Price Adjustment Request</h1>
            <div class="content">
                <uc1:contact ID="ucContact" runat="server" />
                <asp:Literal ID="litScript" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
</asp:Content>
