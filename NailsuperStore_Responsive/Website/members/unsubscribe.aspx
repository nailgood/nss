<%@ Page Language="VB" AutoEventWireup="false" CodeFile="unsubscribe.aspx.vb" Inherits="members_unsubscribe"
    MasterPageFile="~/includes/masterpage/interior.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
<div id="contact_form" class="form-horizontal" role="form" runat="server">
<h1>Unsubcribe Email</h1>
   <div class="panel-content">
            <div class="title">Email Offers & Promotions</div>
            <div class="content">
            <div class="form-group">
                    <div class="col-sm-12">
                Get the latest on The Nail Superstores many exclusive benefits including promotional offers, coupons and discounts, and notification of new brands and products via email.
                </div>
            </div>
             <div class="form-group">
                    <div class="col-sm-12">
                 <div id="group-radio">
                <asp:RadioButtonList ID="rbtnNewsletter" runat="server" RepeatDirection="Vertical"
                    CssClass="radio-node">
                    <asp:ListItem Value="1"><i class="ico-radio"></i>Yes, I would like to sign-up The Nail Superstore's emails.</asp:ListItem>
                    <asp:ListItem Value="0"><i class="ico-radio"></i>No, thanks.</asp:ListItem>
                </asp:RadioButtonList>
            </div>
              </div>
            </div>
            <div class="form-group">
                    <div class="col-sm-12">
                     <asp:Button runat="server" ID="btnSubmit" data-btn="submit" Text="Submit" CssClass="btn btn-submit" CausesValidation="true" />
                    </div>
              </div>
            </div>
        </div>
    </div>
</asp:Content>
