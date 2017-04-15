<%@ Control Language="VB" AutoEventWireup="false" CodeFile="free-item-list.ascx.vb"
    Inherits="controls_product_free_item_list" %>
    <%@ Register Src="free-sample-product.ascx" TagName="free" TagPrefix="uc1" %>
<section class="list" id="freeitemList">
    <div class="list-header">
    </div>
    <asp:Repeater ID="rptData" runat="server">
        <ItemTemplate>
            <uc1:free ID="ucFreeItem" runat="server" />
        </ItemTemplate>
    </asp:Repeater>
</section>
<input type="hidden" runat="server" id="hidListId" name="hidListId" clientidmode="Static" />