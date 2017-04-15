<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/interior.master" AutoEventWireup="false" CodeFile="returnauthorizationrequest.aspx.vb" Inherits="Contact_returnauthorizationrequest" %>

<%@ Register Src="~/controls/layout/form-contact.ascx" TagName="contact" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    <div id="contact_form" class="form-horizontal" role="form" runat="server">
        <div class="panel-content">
            <h1 class="formtitle">Return Authorization Number Request</h1>
            <div class="content">
                <div class="form-group">
                    <div class="col-sm-12">
                       Need to return an item to The Nail Superstore? Simply fill out the form bellow. Within 48 hours after submitting your request, you will receive a response by email. Thank You!
                    </div>
                </div>
                <uc1:contact ID="ucContact" runat="server" />
                <asp:Literal ID="litScript" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
</asp:Content>