<%@ Page Language="VB" AutoEventWireup="false" CodeFile="quickorder.aspx.vb" MasterPageFile="~/includes/masterpage/interior.master" Inherits="quickorder_default" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    <title id="PageTitle" enableviewstate="False" runat="server"></title>
        <center>

            <div id="quick-order">
                    <div class="panel-content bo-qo"  preventdefaultbutton="ctl02_btnSearch">
                        <div class="title">
                            Catalog quick order</div>
                        <div class="border" style="padding: 10px 20px">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="control" style="vertical-align: top">
                                        <p>Using this feature, you can order the products you want by using the item number
                                        shown in The Nail Superstore catalogue. It&#39;s <strong>simple</strong> as 1-2-3.
                                        </p>
                                        <p>
                                            1. Enter the item number listed in the catalog and quantity you want.<br />
                                            2. Click &quot;Add to Cart.&quot;<br />
                                        </p>
                                        You can enter up to 20 items. Sale prices will be automatically applied to your
                                        order.<br />
                                        <div class="visible-xs" style="padding:15px 0"><img alt="Catalog Quick Order" src="<%= Utility.ConfigData.CDNMediaPath %>/includes/theme/images/quickorder.jpg" style="border: none" /></div>
                                        <!-- TABLE ITEM# -->
                                        <input id="hdnMore" runat="server" type="hidden" />
                                        <table border="0" cellpadding="2" cellspacing="0">
                                            <tr>
                                                <td colspan="3" style="padding-bottom: 10px;">
                                                    <asp:Literal ID="litMsg" runat="server" Text="&lt;div style='margin:10px 0 0 0;' class='red'&gt;One or more items require further customization. Please make your selection(s) and click the Add To Cart button.&lt;/div&gt;"
                                                        Visible="false" />
                                                </td>
                                            </tr>
                                             <asp:Repeater ID="rpt" runat="server">
                        <headertemplate><tr><td style="width:100px;"><strong>Item</strong></td><td style="width:30px;"><strong>Qty</strong></td><td>&#160;</td></tr></headertemplate>
                         <itemtemplate><asp:Literal ID="trOpen" runat="server" />
                         
                         <td>
                         <div id="dvItem" runat="server">
                             <asp:TextBox ID="txtitem" runat="server" CssClass="form-control"/>
                         </div>
                         </td>
		                <td style="width:30px;">
                            <div id="dvQty" runat="server">
                            <asp:TextBox ID="txtqty" runat="server" CssClass="form-control" MaxLength="4" Width="40px"/>
                            </div>
                        </td>
		                <td>
                 <div class="has-error"><span id="spclose" runat="server"></span></div>
        <asp:Literal ID="lit" runat="server" />
        <asp:Repeater ID="rpt" runat="server">
            <HeaderTemplate>
            </HeaderTemplate>
				<ItemTemplate>
                    <td><asp:Label ID="lblId" runat="server" Text="<%#Container.DataItem.Id%>" Visible="false" />
                    <strong><%#CType(Container.DataItem, DataLayer.StoreAttributeRow).Name%>:</strong><br />
                    <asp:DropDownList ID="ddlAttribute" runat="server" /><br />&#160;</td></ItemTemplate>
                <FooterTemplate>
                   
            </FooterTemplate>
        </asp:Repeater> 
                     </td>
                            <asp:Literal ID="trClose" runat="server" />
        </ItemTemplate> 
        </asp:Repeater> </td>
                            <asp:Literal ID="trClose" runat="server" /></ItemTemplate> </asp:Repeater>
                            <tr>
                                <td align="left" style="padding-top:15px">
                                    <asp:Button ID="btnAdd2Cart" runat="server" CssClass="btn btn-submit" Text="Add to Cart" />
                                </td>
                            </tr>
                            </table>
                            <!-- TABLE ITEM# -->
                            </td>
                            <td style="padding-left: 20px; vertical-align: top" class="hidden-xs">
                                <img alt="Catalog Quick Order" src="/includes/theme/images/quickorder.jpg" style="border: none" />
                            </td>
                            </tr></table></div>
         
                    <asp:Literal runat="server" ID="litJS" />
                   
                </div>
               
            </div>
           
           
        </center>
 </asp:Content>

