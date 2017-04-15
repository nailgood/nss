<%@ Page Language="vb" AutoEventWireup="false" CodeFile="generalquestion.aspx.vb"
    MasterPageFile="~/includes/masterpage/interior.master" Inherits="contact_generalquestion" %>

<%@ Register Src="~/controls/layout/form-contact.ascx" TagName="contact" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div id="contact_form" class="form-horizontal" role="form" runat="server">
        <div class="panel-content">
            <h1 class="formtitle">
                General Question</h1>
            <div class="content">
                <div class="form-group">
                    <div class="col-sm-12">
                        We are here to help. We would like to hear from you. Just drop us a line for whatever question you might have. Thank you!
                    </div>
                </div>
                <uc1:contact ID="ucContact" runat="server" />
                <asp:Literal ID="litScript" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
</asp:Content>
