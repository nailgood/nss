<%@ Page Language="vb" AutoEventWireup="false" Inherits="store_ViewOrder" MasterPageFile="~/includes/masterpage/main.master" CodeFile="ViewOrder.aspx.vb" %>

<%@ Register TagName="OrderDetail" TagPrefix="CC" Src="~/controls/product/order-detail.ascx" %>
<%@ Register TagPrefix="CT" Namespace="MasterPages" Assembly="Common" %>

<%--<%@ Register Src="../modules/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc2" %>
<%@ Register Src="../modules/Menu.ascx" TagName="Menu" TagPrefix="uc3" %>
<%@ Register src="../modules/NeedAssistance.ascx" tagname="NeedAssistance" tagprefix="uc4" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    <title id="PageTitle" enableviewstate="False" runat="server"></title>
        <center>
            <div id="header-page">
             <%--   <uc2:SearchBar ID="SearchBar1" runat="server" />
                <uc3:Menu ID="Menu1" runat="server" />--%>
            </div>
            <div id="page">
                <div id="left-page">
                    
                   <%-- <uc4:NeedAssistance ID="NeedAssistance1" runat="server" />--%>
                </div>
                <div id="content-page" preventdefaultbutton="ctl02_btnSearch">
                    <CT:ContentRegion runat="server" ID="ContentRegion1" Width="744" />
                    <CT:ErrorMessage ID="ErrorPlaceHolder" runat="server" />
                    <CT:ContentRegion runat="server" ID="CT_Top">
                        <!--div class="bc">
	<a class="bdcrmblnk" href="/home.aspx">Home</a> <img src="/App_Themes/Default/images/icon_stepmenu.gif"> <a class="bdcrmblnk" href="/members/">My Account</a> <img src="/App_Themes/Default/images/icon_stepmenu.gif"> <a class="bdcrmblnk" href="/members/orderhistory/">Order History</a> <img src="/App_Themes/Default/images/icon_stepmenu.gif"> <span class="bcative">View Order</span>
</div-->
                    </CT:ContentRegion>
                    <div class="form1" style="padding-top: 10px; width: 100%;">
                        <div class="title">
                            <asp:Literal ID="ltlPageTitle" runat="server" /></div>
                        <div class="border">
                            <table style="width: 100%;" cellspacing="0" cellpadding="0" border="0" summary="product">
                                <tr>
                                    <td style="padding: 10px 10px 10px 20px">
                                        <b>Order Status: <span class="mag">
                                            <asp:Literal runat="server" ID="ltl" /></span><asp:Literal runat="server" ID="ltl2" /></b>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <CC:OrderDetail runat="server" ID="dtl" />
                    <div>
                        &nbsp;</div>
                    <CT:ContentRegion runat="server" ID="CT_Bottom" Width="744" />
                </div>
                <div style="clear: both">
                </div>
                
            </div>
            
            <div>
                <CT:NavigationRegion runat="server" ID="NavigationRegion" />
            </div>
        </center>
     
        
</asp:Content>