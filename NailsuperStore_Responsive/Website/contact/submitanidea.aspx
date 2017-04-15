<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/interior.master" CodeFile="submitanidea.aspx.vb" Inherits="Contact_submitanidea" %>

<%@ Register Src="~/controls/layout/form-contact.ascx" TagName="contact" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
   <div id="contact_form" class="form-horizontal" role="form" runat="server">
        <div class="panel-content">
            <h1 class="formtitle">
                Submit An Idea</h1>
            <div class="content">
                <div class="form-group">
                    <div class="col-sm-12">
                       We would love to have your ideas for new enhancements and features you would like to see. Please submit your suggestions so we can make The Nail Superstore more effective for you
                    </div>
                </div>
                <uc1:contact ID="ucContact" runat="server" />
                <asp:Literal ID="litScript" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
</asp:Content>