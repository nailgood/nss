<%@ Page Language="vb" AutoEventWireup="false" CodeFile="billinginquiry.aspx.vb"
    MasterPageFile="~/includes/masterpage/interior.master" Inherits="Contact_billinginquiry" %>

<%@ Register Src="~/controls/layout/form-contact.ascx" TagName="contact" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div id="contact_form" class="form-horizontal" role="form" runat="server">
        <div class="panel-content">
            <h1 class="formtitle">Billing Inquiry</h1>
            <div class="content">
                <div class="form-group">
                    <div class="col-sm-12">
                        Your statement does not match with what you paid for? To prevent fraud, please fill out the form below. We will get back to you in 2 business days. Thank You!
                    </div>
                </div>
                <uc1:contact ID="ucContact" runat="server" />
                <asp:Literal ID="litScript" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
</asp:Content>
