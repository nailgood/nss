<%@ Page Language="vb" AutoEventWireup="false" Inherits="member_creditmemo_view"
    MasterPageFile="~/includes/masterpage/interior.master" CodeFile="view.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <script type="text/javascript">
        $(window).load(function () {

        });
    </script>
    <div id="orderconfirm" class="memo-detail">
        <div class="title">
            <asp:Literal ID="ltrPageTitle" runat="server"></asp:Literal>
        </div>
        <ul class="address">
            <li class="billing">
                <ul>
                    <li class="title">Billing Address</li>
                    <asp:Literal ID="ltrBilling" runat="server"></asp:Literal>
                </ul>
            </li>
            <li class="shipping">
                <ul>
                    <li class="title">Shipping Address</li>
                    <asp:Literal ID="ltrShipping" runat="server"></asp:Literal>
                </ul>
            </li>
        </ul>
        <div class="cart-item-list">
            <div class="header">
                <div class="image">
                    Item</div>
                <div class="name">
                </div>
                <div class="qty">
                    Qty</div>
                <div class="unit">
                    Unit</div>
                <div class="unitprice">
                    Unit Price</div>
                <div class="restock">
                    Restock Charge</div>
                <div class="total">
                    Total</div>
            </div>
            <asp:Repeater runat="server" ID="rptLine">
                <ItemTemplate>
                    <div class="cart-row" id="divRow" runat="server">
                        <div class="image cart-cell">
                            <asp:Literal ID="ltrImage" runat="server"></asp:Literal>
                        </div>
                        <div class="name cart-cell">
                            <div class="item-name">
                                <asp:Literal ID="ltrName" runat="server"></asp:Literal>
                            </div>
                            <div class="sku">
                                Item#
                                <asp:Literal ID="ltrSKU" runat="server"></asp:Literal>
                            </div>
                            <div class="smallqty">
                                Qty:
                                <asp:Literal ID="ltrSmallQty" runat="server"></asp:Literal>
                            </div>
                            <div class="smallunitprice ">
                                Unit Price :
                                <asp:Literal ID="ltrSmallUnitPrice" runat="server"></asp:Literal>
                            </div>
                            <div class="smallrestock">
                                Restock Charge:
                                <asp:Literal ID="ltrSmallRestock" runat="server"></asp:Literal>
                            </div>
                        </div>
                        <div class="qty cart-cell ">
                            <asp:Literal ID="ltrQty" runat="server"></asp:Literal></div>
                        <div class="unit cart-cell">
                            <asp:Literal ID="ltrUnit" runat="server">
                            </asp:Literal>
                        </div>
                        <div class="unitprice cart-cell">
                            <asp:Literal ID="ltrUnitPrice" runat="server"></asp:Literal>
                        </div>
                        <div class="restock cart-cell">
                            <asp:Literal ID="ltrRestock" runat="server"></asp:Literal>
                        </div>
                        <div class="total cart-cell">
                            <asp:Literal ID="ltrTotal" runat="server"></asp:Literal>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
          
        </div>
        <div id="memo-summary">
            <div class="title">
                Credit Memo Summary
            </div>
            <div class="detail">
                <div class="des">
                    <asp:Literal ID="ltrDesc" runat="server"></asp:Literal>
                </div>
                <ul>
                    <li id="liSubtotal" visible="false" runat="server">
                        <div class="text">
                            Merchandise Subtotal</div>
                        <div class="value">
                            <asp:Literal ID="ltrSubTotal" runat="server"></asp:Literal>
                        </div>
                    </li>
                    <li visible="false" id="liSaleTax" runat="server">
                        <div class="text">
                            Sales Tax</div>
                        <div class="value">
                            <asp:Literal ID="ltrTax" runat="server"></asp:Literal></div>
                    </li>
                    <li visible="false" id="liLessShipping" runat="server">
                        <div class="text">
                            Less Shipping</div>
                        <div class="value">
                            <asp:Literal ID="ltrLessShipping" runat="server"></asp:Literal></div>
                    </li>
                    <li visible="false" id="liCreditTotal" runat="server">
                        <div class="text">
                            Credit Total</div>
                        <div class="value">
                            <asp:Literal ID="ltrCreditTotal" runat="server"></asp:Literal></div>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
