<%@ Page Language="VB" AutoEventWireup="false" CodeFile="default.aspx.vb" MasterPageFile="~/includes/masterpage/interior.master"
    Inherits="Request" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    <div id="callback" class="form-horizontal" role="form">
        <h1 class="page-title">
            Request Call Back</h1>
 
            <ul class="sv-contact list-unstyled">
                <li><strong>Inside US: </strong>1.800.669.9430 or 1.847.260.4000</li>
                <li><strong>International: </strong>+1.847.260.4000</li>
                <li><strong>Business Hours: </strong>Monday to Friday<br />
                                    <span class="last">9:00 AM - 5:30 PM CST</span>
                </li>
            </ul>


        <div class="text col-sm-offset-3">
                To talk with us, please complete the form below:
        </div>
        <div class="form-group">
            <label for="drCountry" class="col-sm-3">
                Country:</label>
            <div class="n-dropdown col-sm-4">
                <asp:DropDownList ID="drCountry" CssClass="form-control" runat="server">
                </asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label for="drLang" class="col-sm-3">
                Preferred language:</label>
            <div class="n-dropdown col-sm-4">
                <asp:DropDownList ID="drLang" CssClass="form-control" runat="server">
                </asp:DropDownList>
            </div>
        </div>
        <div class="form-group" runat="server" id="dvEmail">
            <label for="txtEmail" class="col-sm-3  text-left">
                Email:</label>
            <div class="col-sm-4">
                <asp:TextBox runat="server" ID="txtEmail" type="text" CssClass="form-control col-sm-7"
                    placeholder="Enter your email here"></asp:TextBox>
            </div>
            <div class="div-error">
                <asp:RequiredFieldValidator runat="server" ID="rfvEmail" ControlToValidate="txtEmail"
                    ValidationGroup="valGroup" Display="Dynamic" Text="* Email is required" ErrorMessage="Email is requied"
                    CssClass="text-danger"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ValidationGroup="valGroup" runat="server" ID="regTxtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Email is invalid." CssClass="text-danger" />
           </div>
        </div>
        <div class="form-group" runat="server" id="dvName">
            <label for="txtName" class="col-sm-3">
                Name:</label>
            <div class="col-sm-4">
                <asp:TextBox runat="server" ID="txtName" type="text" CssClass="form-control" placeholder="Enter your name here"></asp:TextBox>
            </div>
            <div class="div-error">
                <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtName"
                    ValidationGroup="valGroup" Display="Dynamic" Text="* Name is required" ErrorMessage="Name is requied"
                    CssClass="text-danger"></asp:RequiredFieldValidator></div>
        </div>
        <div class="form-group" runat="server" id="dvNumber">
            <label for="txtNumber" class="col-sm-3">
                Phone number or Skype ID:</label>
            <div class="col-sm-4">
                <asp:TextBox runat="server" ID="txtNumber" type="text" CssClass="form-control" placeholder="Enter Phone number or Skype ID"></asp:TextBox>
            </div>
            <div class="div-error">
                <asp:RequiredFieldValidator runat="server" ID="rfvNumber" ControlToValidate="txtNumber"
                    ValidationGroup="valGroup" Display="Dynamic" Text="* Number is required" ErrorMessage="Number is requied"
                    CssClass="text-danger"></asp:RequiredFieldValidator></div>
        </div>
        <div class="form-group">
            <label for="drCallme" class="col-sm-3">
                When should we call you?:</label>
            <div class="n-dropdown col-sm-4">
                <asp:DropDownList ID="drCallme" CssClass="form-control" runat="server">
                    <asp:ListItem Selected="True">Now</asp:ListItem>
                    <asp:ListItem>5 minutes</asp:ListItem>
                    <asp:ListItem>10 minutes</asp:ListItem>
                    <asp:ListItem>30 minutes</asp:ListItem>
                    <asp:ListItem>60 minutes</asp:ListItem>
                    <asp:ListItem>Morning</asp:ListItem>
                    <asp:ListItem>Afternoon</asp:ListItem>
                    <asp:ListItem>Evening</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="form-group" runat="server" id="dvContent">
            <label for="txtNote" class="col-sm-3">
                How can we help?:</label>
            <div class="col-sm-4">
                <asp:TextBox runat="server" ID="txtNote" TextMode="MultiLine" type="text" CssClass="form-control"
                    Rows="3" placeholder="Enter your content"></asp:TextBox>
            </div>
            <div class="div-error">
                <asp:RequiredFieldValidator runat="server" ID="rfvNote" ControlToValidate="txtNote"
                    ValidationGroup="valGroup" Display="Dynamic" Text="* Note is required" ErrorMessage="Note is requied"
                    CssClass="text-danger"></asp:RequiredFieldValidator></div>
        </div>
        <div class="col-xs-offset-0 col-sm-offset-3 pn-btn">
            <asp:Button ID="btnRequest" data-btn="submit" runat="server" Text="Request Call Back" CssClass="btn btn-submit" />
            <%--                    <asp:Button ID="btnRequest2" data-btn="submit" runat="server"
                        Text="Request Call Back" CssClass="btn btn-warning btn-block btn-lg visible-xs" />--%>
        </div>
    </div>
</asp:Content>
