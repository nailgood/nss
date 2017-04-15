<%@ Control Language="VB" AutoEventWireup="false" CodeFile="confirm-freight-option.ascx.vb"
    Inherits="controls_checkout_confirm_freight_option" %>
<tr>
    <td class='left'>
    </td>
    <td colspan='2' class="label-text">
        <ul class="freight-option">
            <li class="lift-gate" id="liLiftGate" runat="server">
                <div class="checkbox">
                    <label for="chkLiftGate">
                        <asp:CheckBox runat="server" ID="chkLiftGate" Checked="true" Enabled="false" />
                        
                        <i class="fa fa-check checkbox-font" ></i><span class="checkbox-label">
                            <asp:Literal ID="ltrLiftGate" runat="server"></asp:Literal>
                        </span>
                    </label>
                </div>
            </li>
            <li class="schedule-delivery" id="liScheduleDelivery" runat="server">
                <div class="checkbox">
                    <label for="chkScheduleDelivery">
                        <asp:CheckBox runat="server" ID="chkScheduleDelivery" Checked="true" Enabled="false" />
                        <i class="fa fa-check checkbox-font" ></i><span class="checkbox-label">
                            <asp:Literal ID="ltrScheduleDelivery" runat="server"></asp:Literal>
                        </span>
                    </label>
                </div>
            </li>
            <li class="inside-delivery" id="liInsideDelivery" runat="server">
                <div class="checkbox">
                    <label for="chkInsideDelivery">
                        <asp:CheckBox runat="server" ID="chkInsideDelivery" Checked="true" Enabled="false" />
                        <i class="fa fa-check checkbox-font" ></i><span class="checkbox-label">
                            <asp:Literal ID="ltrInsideDelivery" runat="server"></asp:Literal></span>
                    </label>
                </div>
            </li>
        </ul>
    </td>
    <td class='right'>
    </td>
</tr>
