<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="AmazonHistory.aspx.vb" Inherits="admin_store_items_AmazonHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">
<div style="margin: 0 20px">
        <h4>
            <asp:Literal ID="ltrHeader" runat="server" Text="List amazon items of Nail item: "></asp:Literal></h4>        
            <asp:Button ID="Cancel2" runat="server" Text="Back" CssClass="btn" CausesValidation="False">
                </asp:Button>
                <br />
                <br />
        <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="20"
            AllowPaging="false" AllowSorting="False" EmptyDataText="There are no records that match the search criteria"
            AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
            <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
            <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
            <Columns>
               
                 <asp:BoundField  DataField="ASIN" HeaderText="Amazon ASIN">   </asp:BoundField>
                <asp:BoundField  DataField="Title" HeaderText="Amazon Title">
                </asp:BoundField>
                <asp:TemplateField>
              <HeaderTemplate>Title</HeaderTemplate>
                <ItemTemplate>
			        <asp:Literal ID="ltTitle" runat="server"></asp:Literal>
			    </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField>
              <HeaderTemplate>Weight</HeaderTemplate>
                <ItemTemplate>
			        <asp:Label ID="lbWeightPound" runat="server"></asp:Label>
			    </ItemTemplate>
              </asp:TemplateField>
             
                <asp:BoundField  DataField="Quantity" HeaderText="Qty">
                </asp:BoundField>
                <asp:TemplateField>
                <HeaderTemplate>Price</HeaderTemplate>
                <ItemTemplate>
			        <asp:Literal ID="ltPrice" runat="server"></asp:Literal>
			    </ItemTemplate>
              </asp:TemplateField>
                <asp:BoundField  DataField="LauchDate" HeaderText="Create Date">
                </asp:BoundField>
                 <asp:BoundField  DataField="DiscontinueDate" HeaderText="Expire Date">
                </asp:BoundField>
                <asp:Checkboxfield ItemStyle-HorizontalAlign="Center"   DataField="IsActive" HeaderText="Sell In Amazon"/>
            </Columns>
        </CC:GridView>
    </div>
</asp:Content>

