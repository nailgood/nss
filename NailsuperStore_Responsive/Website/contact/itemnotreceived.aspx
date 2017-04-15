<%@ Page Language="vb" CodeFile="itemnotreceived.aspx.vb" MasterPageFile="~/includes/masterpage/interior.master"  AutoEventWireup="false" Inherits="contact_itemnotreceived" %>
<%@ Register Src="~/controls/layout/form-contact.ascx" TagName="contact" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    <div id="contact_form" class="form-horizontal" role="form" runat="server">
        <div class="panel-content">
            <h1 class="formtitle">
                Item Not Received</h1>
            <div class="content">
                <div class="form-group">
                    <div class="col-sm-12">
                       We want you to receive your order complete. We try our best but mistakes can happen. Please fill out the form below. We will get back to you within 2 business days. Thank You!
                    </div>
                </div>
                <uc1:contact ID="ucContact" runat="server" />
                <asp:Literal ID="litScript" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
</asp:Content>