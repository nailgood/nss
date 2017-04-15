<%@ Page Language="vb" AutoEventWireup="false" CodeFile="wholesaleinquiry.aspx.vb" MasterPageFile="~/includes/masterpage/interior.master" Inherits="contact_wholesaleinquiry" %>
<%@ Register Src="~/controls/layout/form-contact.ascx" TagName="contact" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    <div id="contact_form" class="form-horizontal" role="form" runat="server">
        <div class="panel-content">
            <h1 class="formtitle">
                Wholesale Inquiry</h1>
            <div class="content">
                <div class="form-group">
                    <div class="col-sm-12">
                       Do you have a nail or beauty supply store? Interested to buy in bulk at wholesale price. Please fill out the form. Thank You!
                    </div>
                </div>
                <uc1:contact ID="ucContact" runat="server" />
                <asp:Literal ID="litScript" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
</asp:Content>