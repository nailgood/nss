<%@ Control Language="VB" AutoEventWireup="false" CodeFile="shipping-option.ascx.vb"
    Inherits="controls_checkout_shipping_option" %>
<ul id="ulOption" runat="server">
    <li class="title">Option</li>
    <li class="insurance" id="pnlInsurance" clientidmode="Static" visible="false" runat="server">
        <div class="checkbox">
            <label for="chkInsurance">
                <input type="checkbox" id="chkInsurance" onchange="CheckShippingInsurance(this.checked);"
                    clientidmode="Static" runat="server" />
                <i class="fa fa-check checkbox-font" ></i>
                <asp:Label ID="lblInsurance" runat="server"></asp:Label>
                per <span class="number">$100 </span>
            </label>
        </div>
    </li>
    <li class="signature-confirm" id="pnlSignature" clientidmode="Static" runat="server">
        <div class="checkbox">
            <label for="chkSignature" <%=styleLabelDisable %> >
                <asp:CheckBox runat="server" ID="chkSignatureDisable"   AutoPostBack="false" clientidmode="Static" />
                    <input type="checkbox" id="chkSignature" onchange="CheckShippingSignature(this.checked);" clientidmode="Static" runat="server" />
                    <i class="fa fa-check checkbox-font" ></i>
                <asp:Literal ID="ltrSignature" runat="server"></asp:Literal></label>
        </div>
        <div style="padding-top: 10px;"><span class="question" onclick="ShowTipSignatureConfirm();">&nbsp; </span></div>
    </li>
</ul>
