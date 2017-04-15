<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="admin_report_order_default" title="Untitled Page" %>


<%@ Register Assembly="System.Web.DataVisualization" Namespace="System.Web.UI.DataVisualization.Charting"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">
    <h4>Order Report</h4>
    <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
        <table cellpadding="2" cellspacing="2">
            <tr>
                
                <th style="text-align: right;">
                    From Date:
                </th>
                <td class="field">
                    <CC:DatePicker ID="dtpFromDate" runat="server" />
                </td>
                   <th style="text-align: right;">
                    To Date:
                </th>
                <td class="field">
                    <CC:DatePicker ID="dtpToDate" runat="server" />
                </td>
            </tr>     
            <tr>
                <td colspan="4">
                   <CC:OneClickButton ID="btnSearch" runat="server" Text="Search" CssClass="btn" />
                </td>
            </tr>
        </table>
        <br />
    </asp:Panel>
     <br />
     <div id="dvChart">
         <cc1:Chart ID="ChartReport" runat="server" Width="1000px"  height="320" ImageType="Png" >        
              <Series>                    
                    <cc1:Series Name="Series1" MarkerSize="7" MarkerColor="#028dc6"  BorderColor="#028dc6" LabelFormat="C" BorderWidth="4" Color="#e6f3f9"   ShadowColor="Transparent" >
                    </cc1:Series>
              </Series>
                <ChartAreas>
                    <cc1:ChartArea Name="ChartArea1">                                               
                    </cc1:ChartArea>
                </ChartAreas>
        </cc1:Chart>
     </div>
     
     <br />
    <div>
        <table cellspacing="2" cellpadding="2" border-width="0"  rules="all">
            <tr>
                <th style="white-space:nowrap;"  align="right">Page Total:</th>
                <th style="white-space:nowrap;" align="right"><asp:literal runat="server" ID="ltrOrder"></asp:literal></th>
                <th style="white-space:nowrap;" align="right"><asp:literal runat="server" ID="ltrOrderWeb"></asp:literal></th>
                <th style="white-space:nowrap;" align="right"><asp:literal runat="server" ID="ltrOrderMobile"></asp:literal></th>
                <th style="white-space:nowrap;" align="right"><asp:literal runat="server" ID="ltrOrderAmazon"></asp:literal></th>
                <th style="white-space:nowrap;" align="right"><asp:literal runat="server" ID="ltrOrderEbay"></asp:literal></th>
                <th style="white-space:nowrap;" align="right"><asp:literal runat="server" ID="ltrTotal"></asp:literal></th>
                <th></th>
                <th></th>
            </tr>
             <tr>
                <th style="white-space:nowrap;width:80px;">Date</th>
                <th style="white-space:nowrap;width:70px;">Orders</th>
                <th style="white-space:nowrap;width:70px;">Desktop</th>
                <th style="white-space:nowrap;width:70px;">Mobile</th>
                 <th style="white-space:nowrap;width:70px;">Amazon</th>
                 <th style="white-space:nowrap;width:70px;">Ebay</th>
                <th style="white-space:nowrap;width:100px;">Total</th>
                <th style="white-space:nowrap;width:130px;">Shipping</th>
                <th style="white-space:nowrap;width:70px;">Tax</th>
            </tr>
            <asp:Repeater ID="rptReportList" runat="server" >
            <ItemTemplate>
                 <tr class="row" valign="top">
                    <td align="center">
                        <a href='<%# "/admin/store/orders/default.aspx?F_OrderDateLBound=" & DataBinder.Eval(Container, "DataItem.Date", "{0:MM/dd/yyyy}") & "&F_OrderDateUBound=" & DataBinder.Eval(Container, "DataItem.Date", "{0:MM/dd/yyyy}")%>'><%# DataBinder.Eval(Container, "DataItem.Date", "{0:MM/dd/yyyy}")%></a>
                    </td>
                    <td align="right">
                       <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CountOrder") %>'></asp:Label>
                    </td>
                    <td align="right">
                       <asp:Label ID="Label5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CountWeb") %>'></asp:Label>
                    </td>
                      <td align="right">
                       <asp:Label ID="Label6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CountMobile") %>'></asp:Label>
                    </td>
                      <td align="right">
                       <asp:Label ID="Label7" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CountAmazon") %>'></asp:Label>
                    </td>
                      <td align="right">
                       <asp:Label ID="Label8" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CountEbay") %>'></asp:Label>
                    </td>
                    <td align="right">
                       <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Total","{0:C}") %>'></asp:Label>
                    </td>
                    <td align="right">
                       <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Shipping","{0:C}") %>'></asp:Label>
                    </td>
                    <td align="right">
                       <asp:Label ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Tax","{0:C}") %>'></asp:Label>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="alternate" valign="top">
                   <td align="center">
                        <a href='<%# "/admin/store/orders/default.aspx?F_OrderDateLBound=" & DataBinder.Eval(Container, "DataItem.Date", "{0:MM/dd/yyyy}") & "&F_OrderDateUBound=" & DataBinder.Eval(Container, "DataItem.Date", "{0:MM/dd/yyyy}")%>'><%# DataBinder.Eval(Container, "DataItem.Date", "{0:MM/dd/yyyy}")%></a>
                    </td>
                    <td align="right">
                       <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CountOrder") %>'></asp:Label>
                    </td>
                    <td align="right">
                       <asp:Label ID="Label5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CountWeb") %>'></asp:Label>
                    </td>
                      <td align="right">
                       <asp:Label ID="Label6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CountMobile") %>'></asp:Label>
                    </td>
                     <td align="right">
                       <asp:Label ID="Label7" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CountAmazon") %>'></asp:Label>
                    </td>
                      <td align="right">
                       <asp:Label ID="Label8" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CountEbay") %>'></asp:Label>
                    </td>
                    <td align="right">
                       <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Total","{0:C}") %>'></asp:Label>
                    </td>
                    <td align="right">
                       <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Shipping","{0:C}") %>'></asp:Label>
                    </td>
                    <td align="right">
                       <asp:Label ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Tax","{0:C}") %>'></asp:Label>
                    </td>
                </tr>
            </AlternatingItemTemplate>
               
            </asp:Repeater>           

        </table>
        <br />           
    </div>
</asp:Content>

