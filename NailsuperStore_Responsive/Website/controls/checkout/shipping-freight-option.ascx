<%@ Control Language="VB" AutoEventWireup="false" CodeFile="shipping-freight-option.ascx.vb"
    Inherits="controls_checkout_shipping_freight_option" %>
<ul>
    <li class="title">Freight Delivery Option</li>
    <li class="lift-gate">
        <div class="checkbox">
            <label for="chkLiftGate">
                <input type="checkbox" onchange="RequestOversizeFee(this.checked,1);" id="chkLiftGate"
                    runat="server" clientidmode="Static" />
                <i class="fa fa-check checkbox-font" ></i><span class="checkbox-label" id="lblLiftGate"
                    runat="server">Click here to request a lift gate. A single surcharge of ${0} will
                    be added to your order. </span>
            </label>
        </div>
    </li>
    <li class="schedule-delivery">
        <div class="checkbox">
            <label for="chkScheduleDelivery">
                <input type="checkbox" onchange="RequestOversizeFee(this.checked,2);" id="chkScheduleDelivery"
                    runat="server" clientidmode="Static" />
                <i class="fa fa-check checkbox-font" ></i><span class="checkbox-label" id="lblScheduleDelivery" runat="server">
                    Click here to request a scheduled delivery. A single surcharge of {0} will be added
                    to your order. </span>
            </label>
        </div>
    </li>
    <li class="inside-delivery">
        <div class="checkbox">
            <label for="chkInsideDelivery">
                <input type="checkbox" onchange="RequestOversizeFee(this.checked,3);" id="chkInsideDelivery"
                    runat="server" clientidmode="Static" />
                <i class="fa fa-check checkbox-font" ></i></span><span class="checkbox-label" id="lblInsideDelivery" runat="server">
                    Click here to request an inside delivery. A single surcharge of ${0} will be added to
                    your order. </span>
            </label>
        </div>
    </li>
</ul>
