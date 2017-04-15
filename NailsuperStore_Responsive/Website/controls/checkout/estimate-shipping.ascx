<%@ Control Language="VB" AutoEventWireup="false" CodeFile="estimate-shipping.ascx.vb"
    Inherits="controls_checkout_estimate_shipping" %>
<section class="estimate-shipping" id="secEstimateShipping">
    <h2>
        Estimate Shipping Charges
    </h2>

    <table border="0">
        <tr>
            <td class="label-text">
                Your country
            </td>
            <td class="control">
                <div class="nf-dropdown">
                    <asp:DropDownList ID="drpCountry" onChange="ChangeCountry(this.value);" ClientIDMode="Static" CssClass="form-control" runat="server">
                    </asp:DropDownList>
                </div>
            </td>
        </tr>
        <tr id="trZipCode" runat="server"  clientidmode="Static">
            <td class="label-text">
                Zip code
            </td>
            <td class="control">
                <asp:TextBox ID="txtZipCode" ClientIDMode="Static" CssClass="form-control zip" runat="server" MaxLength="5"></asp:TextBox>
                <input type="hidden" id="hidZipCode" />
            </td>
        </tr>
    </table>
    <div id="ResultZipCode"></div>
<%--
    <div class="cal" id="divCalculateShipping" runat="server" clientidmode="Static">
         <input type="button" class="btnGreen" value="Estimate Shipping" onclick="CalculateShipping();" />
    </div>
--%>
<%--    <div class="change" >
        <a href="javascript:void(0)" onclick="ChangeMethod()" id="lnkChangeMethod" runat="server" clientidmode="Static">Change shipping method</a>
    </div>--%>
    
</section>

<script type="text/javascript">
    $(document).ready(function () {
        var country = $('#drpCountry').val();
        var zipCode = $('#txtZipCode').val();

        if (country.length > 0) {
            if (country == 'US' && zipCode.length > 0) {
                mainScreen.ExecuteCommand('CalEstimateShipping', 'methodHandlers.CalEstimateShippingCallBack', [country, zipCode]);
            }
            else if (country != 'US') {
                ChangeCountry(country);
            }
        }

        $('#txtZipCode').live("keypress", function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
            }
        });
    });

    
</script>

<div class="estimate-result" id="divEstimateShippingResult"></div>
