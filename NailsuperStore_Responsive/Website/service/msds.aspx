<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/interior.master" AutoEventWireup="false" CodeFile="msds.aspx.vb" Inherits="services_msds" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
     <h1>MSDS</h1>
    <div class="pageinfo">
          <%-- <asp:Literal ID="ltMsds" runat="server"></asp:Literal>--%>
           <asp:Repeater ID="rpMSDS" runat="server">
                <HeaderTemplate>
                    <div class="msds">
                      <div class="rowHeader">
                            <div class="col1"><asp:LinkButton ID="lnkSKU" runat="server" CommandName="SKU">Item#</asp:LinkButton></div>
                            <div class="col2"><asp:LinkButton ID="LinkButton1" runat="server" CommandName="ItemName">Item Name</asp:LinkButton></div>
                            <div class="col3"></div>
                      </div>
                </HeaderTemplate>
                <ItemTemplate>
                  <div class="rowItem">
                  <div class="col1"><%# Eval("SKU") %> </div>
                  <div class="col2"><%# Eval("ItemName") %></div>
                  <div class="col3"><a href="<%=cdn %>/upload/msds/<%# Eval("MSDS") %>" target="_blank"><i class="fa fa-download fa-2"></i></a></div>
                  </div>
                </ItemTemplate>
                <FooterTemplate>
                   </div>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>

