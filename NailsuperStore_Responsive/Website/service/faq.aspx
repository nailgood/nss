<%@ Page Language="vb" AutoEventWireup="false" Inherits="faq" MasterPageFile="~/includes/masterpage/interior.master"
    CodeFile="~/service/faq.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <a name="top"></a>
    <div id="dContent">
        <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Repeater ID="rptTop" runat="server">
                    <ItemTemplate>
                        <div class="boxFAQ">
                            <h1>
                                <%# DataBinder.Eval(Container.DataItem, "CategoryName")%></h1>
                            <ul class="lstQuestion">
                                <asp:Repeater ID="rptInner" runat="server">
                                    <ItemTemplate>
                                        <li><a href="#<%# DataBinder.Eval(Container.DataItem, "FaqId")%>">
                                            <%# DataBinder.Eval(Container.DataItem, "Question")%></a></li></ItemTemplate>
                                </asp:Repeater>
                            </ul>
                            <asp:Panel runat="server" ID="pnlSend" Visible="false">
                                <b>Can't find what you're looking for in this section?</b>
                                <div class="lblAsk">
                                    <asp:LinkButton runat="server" ID="lnkAsk" CommandName="Ask" CommandArgument='<%#Eval("FaqCategoryId")%>' /></div>
                                <asp:PlaceHolder runat="server" ID="phAsk" Visible="false">
                                    <div class="form-horizontal panel-content" role="form" runat="server">
                                        <div class="content">
                                            <div class="form-group">
                                                <div class="col-sm-2 hidden-xs">
                                                </div>
                                                <div class="col-sm-6">
                                                    Please ask your question below.
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-sm-2 hidden-xs text-right">
                                                    Your Email</div>
                                                <div class="col-sm-6 txt-required">
                                                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" CssClass="form-control"
                                                        placeholder="username@hostname.com"></asp:TextBox>
                                                </div>
                                                <div class="div-error">
                                                    <asp:RequiredFieldValidator ID="rfvEmailAddress" runat="server" ControlToValidate="txtEmail"
                                                        CssClass="text-danger" ErrorMessage="Email is required" ValidationGroup="valFAQ"
                                                        Display="Dynamic"></asp:RequiredFieldValidator>
                                                    <asp:CustomValidator ID="cusEmail" runat="server" ClientValidationFunction="CheckValidEmail"
                                                        CssClass="text-danger" ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Email is invalid."
                                                        OnServerValidate="ServerCheckValidEmail" ValidateEmptyText="True" ValidationGroup="valFAQ"></asp:CustomValidator>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-sm-2 hidden-xs text-right">
                                                    Question</div>
                                                <div class="col-sm-6 txt-required">
                                                    <asp:TextBox ID="txtMessage" runat="server" MaxLength="30" CssClass="form-control"
                                                        TextMode="Multiline" placeholder="Question"></asp:TextBox>
                                                </div>
                                                <div class="div-error">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvQuestion" ControlToValidate="txtMessage"
                                                        Display="Dynamic" ValidationGroup="valFAQ" ErrorMessage="Question is required"
                                                        CssClass="text-danger"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-sm-2 text-right">
                                                </div>
                                                <div class="col-sm-8 content">
                                                    <asp:Button runat="server" ID="btnSubmit" data-btn="submit" Text="Submit" CssClass="btn btn-submit"
                                                        CausesValidation="true" />
                                                    <asp:Button runat="server" ID="btnCancel" data-btn="submit" Text="Cancel" CssClass="btn btn-cancel"
                                                        CausesValidation="true" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:PlaceHolder>
                                <asp:PlaceHolder runat="server" ID="phSubmitted" Visible="false">
                                    <div class="lblAsk">
                                        Thank you!<br />
                                        We have received your question and will provide an answer shortly.</div>
                                </asp:PlaceHolder>
                            </asp:Panel>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </ContentTemplate>
        </asp:UpdatePanel>
        <p>
            &nbsp;</p>
        <p>
            &nbsp;</p>
        <asp:UpdatePanel runat="server" ID="up2" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Repeater ID="rptMain" runat="server">
                    <ItemTemplate>
                        <div class="lstAnswer">
                            <h3>
                                <%#DataBinder.Eval(Container.DataItem, "CategoryName")%></h3>
                            <asp:Repeater ID="rptInner" runat="server">
                                <ItemTemplate>
                                    <div class="answer">
                                        <a name="<%# DataBinder.Eval(Container.DataItem, "FaqId")%>"></a><b class="secondarytxtc">
                                            Q:
                                            <%# DataBinder.Eval(Container.DataItem, "Question")%></b><br />
                                        A:
                                        <%# DataBinder.Eval(Container.DataItem, "Answer")%>
                                        <p class="text-right">
                                            <a class="smaller" href="#top">back to top
                                                <img src="../includes/theme/images/arrow.gif" border="0" /></a></p>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:Panel runat="server" ID="pnlSend" Visible="false">
                                <b>Can't find what you're looking for in this section?</b>
                                <div class="lblAsk">
                                    <asp:LinkButton runat="server" ID="lnkAsk2" CommandName="Ask" CommandArgument='<%#Eval("FaqCategoryId")%>' /></div>
                                <asp:PlaceHolder runat="server" ID="phAsk2" Visible="false">
                                    <div class="form-horizontal panel-content" role="form" runat="server">
                                        <div class="content">
                                            <div class="form-group">
                                                <div class="col-sm-2 hidden-xs">
                                                </div>
                                                <div class="col-sm-6">
                                                    Please ask your question below.
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-sm-2 hidden-xs text-right">
                                                    Your Email</div>
                                                <div class="col-sm-6 txt-required">
                                                    <asp:TextBox ID="txtEmail2" runat="server" MaxLength="50" CssClass="form-control"
                                                        placeholder="username@hostname.com"></asp:TextBox>
                                                </div>
                                                <div class="div-error">
                                                    <asp:RequiredFieldValidator ID="rfvEmailAddress2" runat="server" ControlToValidate="txtEmail2"
                                                        CssClass="text-danger" ErrorMessage="Email is required" ValidationGroup="valFAQ"
                                                        Display="Dynamic"></asp:RequiredFieldValidator>
                                                    <asp:CustomValidator ID="cusEmail2" runat="server" ClientValidationFunction="CheckValidEmail"
                                                        CssClass="text-danger" ControlToValidate="txtEmail2" Display="Dynamic" ErrorMessage="Email is invalid."
                                                        OnServerValidate="ServerCheckValidEmail" ValidateEmptyText="True" ValidationGroup="valFAQ"></asp:CustomValidator>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-sm-2 hidden-xs text-right">
                                                    Question</div>
                                                <div class="col-sm-6 txt-required">
                                                    <asp:TextBox ID="txtMessage2" runat="server" MaxLength="30" CssClass="form-control"
                                                        TextMode="Multiline" placeholder="Question"></asp:TextBox>
                                                </div>
                                                <div class="div-error">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvQuestion2" ControlToValidate="txtMessage2"
                                                        Display="Dynamic" ValidationGroup="valFAQ" ErrorMessage="Question is required"
                                                        CssClass="text-danger"></asp:RequiredFieldValidator>
                                                    <asp:Literal ID="ltrQuestion" runat="server"></asp:Literal>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-sm-2 text-right">
                                                </div>
                                                <div class="col-sm-8 content">
                                                    <asp:Button runat="server" ID="btnSubmit2" data-btn="submit" Text="Submit" CssClass="btn btn-submit"
                                                        CausesValidation="true" />
                                                    <asp:Button runat="server" ID="btnCancel2" data-btn="submit" Text="Cancel" CssClass="btn btn-cancel"
                                                        CausesValidation="true" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:PlaceHolder>
                                <asp:PlaceHolder runat="server" ID="phSubmitted2" Visible="false">
                                    <div style="margin: 10px 0;">
                                        Thank you!<br />
                                        We have received your question and will provide an answer shortly.</div>
                                </asp:PlaceHolder>
                            </asp:Panel>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        function CheckValidEmail(sender, args) {
            var email = document.getElementById('txtEmail').value;
            if (email != '') {
                var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                if (!re.test(email)) {
                    args.IsValid = false;
                    document.getElementById('cusEmail').innerHTML = 'Email is invalid';
                    return;
                }
            }
        }
    </script>
</asp:Content>
