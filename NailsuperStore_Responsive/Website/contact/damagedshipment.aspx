<%@ Page Language="vb" AutoEventWireup="false" CodeFile="damagedshipment.aspx.vb" MasterPageFile="~/includes/masterpage/interior.master" Inherits="contact_damagedshipment" %>

<%@ Register Src="~/controls/layout/form-contact.ascx" TagName="contact" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
  <div id="contact_form" class="form-horizontal" role="form" runat="server">
        <div class="panel-content">
            <h1 class="formtitle">Damaged Shipment</h1>
            <div class="content">
                <div class="form-group">
                    <div class="col-sm-12">
                        We try every thing possible to ship your order without any damaged. Unfortunately, accidents do happen. Please fill out the form below. We will get back to you within 2 business days. Thank You!
                    </div>
                </div>
                <uc1:contact ID="ucContact" runat="server" />
                <asp:Literal ID="litScript" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
</asp:Content>