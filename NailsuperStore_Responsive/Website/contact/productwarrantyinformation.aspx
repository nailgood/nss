<%@ Page Language="vb" AutoEventWireup="false" CodeFile="productwarrantyinformation.aspx.vb" MasterPageFile="~/includes/masterpage/interior.master" Inherits="Contact_productwarrantyinformation" %>

<%@ Register Src="~/controls/layout/form-contact.ascx" TagName="contact" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    <div id="contact_form" class="form-horizontal" role="form" runat="server">
        <div class="panel-content">
            <h1 class="formtitle">
                Product Warranty Information</h1>
            <div class="content">
                <div class="form-group">
                    <div class="col-sm-12">
                       Different manufacturers have diffirent warranty policies. To find out more about a specific product. Please fill out the form below. Thank You!
                    </div>
                </div>
                <uc1:contact ID="ucContact" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>