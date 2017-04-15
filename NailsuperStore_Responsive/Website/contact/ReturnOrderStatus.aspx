<%@ Page Language="vb" AutoEventWireup="false" CodeFile="ReturnOrderStatus.aspx.vb" MasterPageFile="~/includes/masterpage/interior.master" Inherits="contact_ReturnOrderStatus" %>
<%@ Register Src="~/controls/layout/form-contact.ascx" TagName="contact" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    <div id="contact_form" class="form-horizontal" role="form" runat="server">
        <div class="panel-content">
            <h1 class="formtitle">
                Return Order Status</h1>
            <div class="content">
                <div class="form-group">
                    <div class="col-sm-12">
                       Want to know the status of a return? Please fill out the form below and we'll get back to you within 24 hours. Thank You!
                    </div>
                </div>
                <uc1:contact ID="ucContact" runat="server" />
                <asp:Literal ID="litScript" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
</asp:Content>