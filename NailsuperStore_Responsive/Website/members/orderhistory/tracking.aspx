<%@ Page Language="VB" AutoEventWireup="false" CodeFile="tracking.aspx.vb" MasterPageFile="~/includes/masterpage/interior.master" Inherits="members_orderhistory_tracking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
<link href="../../includes/theme/css/tracking.css" rel="stylesheet" type="text/css" />
<center>
<div id="page">
<CT:ErrorMessage id="ErrorPlaceHolder" runat="server"/>
<asp:UpdatePanel ID="up" runat="server"  >
<ContentTemplate>
<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="up">
    <ProgressTemplate>
        <center>
        Please wait...<br /><img src="/includes/theme/images/loader.gif" alt="" />
        </center>
    </ProgressTemplate>
</asp:UpdateProgress>
   
     <div id="dvError" runat="server" visible="false">
        <div id="error" style="padding-top:0px">
        <div class="right"></div>
        <div class="left"></div>
        <div class="title">
            <img src="/includes/theme/images/icon_error.gif"> 
            <div class="txt"><asp:Label id="lbExceptionDescription" runat="server"></asp:Label></div>
        </div>
        <div class="content"><img style="border:0px" src="/includes/theme/images/icon_aerror.gif"> <b>Recommended action:</b><br />
        Delivery will be re-attempted the next business day.</div>
        <div class="bot"><div class="rightbot"></div>
        <div class="leftbot"></div>
        </div>
        </div>
     </div>
<div id="dNorecord" runat="server" visible="false" class="no-record"><asp:Label ID="lbmsgError" runat="server"></asp:Label></div>
<div id="ordertracking" class="form" runat="server">
    <div class="title">
        <div style="float:left"><asp:Label id="lbNumber" runat="server"></asp:Label></div>
        <div class="view-fedex"><asp:Label id="lbviewfedex" runat="server"></asp:Label></div>
    </div>
    <div class="border h-info">
    
        <div class="h-left">
            <div>Ship (P/U) date :</div>
            <div class="line-track"><asp:Label id="lbShipTimestamp" runat="server"></asp:Label></div>
            <div>FRANKLIN PARK, IL US</div>
       
        </div>
        <div class="h-middle"> 
            <asp:Literal id="ltStatus" runat="server"></asp:Literal> 
        </div>
        <div class="h-right">
            <div><asp:Label id="lbStatus" runat="server"></asp:Label></div>
            <div class="line-track"><asp:Label id="lbActualDeliveryTimestamp" runat="server"></asp:Label></div>
            <div><asp:Label id="lbActualAddress" runat="server"></asp:Label></div>
        </div>
     </div>
 

    <div style="clear:both">
        <div class="title">Travel History</div>
        <div class="border">
        <div style="width:100%;padding:10px 9px 10px 11px">
              <asp:Literal id="lthistory" runat="server"></asp:Literal>
        </div>
        </div>
    </div>
    <div style="clear:both">
        <div class="title boder">Shipment Facts</div>
         <div class="border" style="padding-left:10px;padding-right:10px;vertical-align:middle; height:auto; overflow:hidden">
        <div style="width:25%;float:left">
            <ul>
                <li class="info">Tracking number</li>
                <li class="info">Reference</li>
                <li class="info" id="liDimension" runat="server">Dimensions</li>
            <li class="info">Purchase order number</li>
            <li class="info">Shipment ID</li>
            <li id="liDoorTagNumber" runat="server" class="info">Door Tag Number</li>
        </ul>
    </div>
    <div style="width:25%;float:left">
         <ul>
            <li><asp:Label id="lbNumer2" runat="server"></asp:Label></li>
            <li><asp:Label id="lbReference" runat="server"></asp:Label></li>
            <li id="liDimension1" runat="server"><asp:Label id="lbDimensions" runat="server"></asp:Label></li>
            <li><asp:Label id="lbordernumber" runat="server"></asp:Label></li>
            <li><asp:Label id="lbShipmentID" runat="server"></asp:Label></li>
            <li id="liDoorTagNumber1" runat="server"><asp:Label id="lbDoorTagNumber" runat="server"></asp:Label></li>
        </ul>
    </div>
    <div style="width:25%;float:left">
         <ul>
            <li class="info">Service</li>
            <li class="info">Weight</li>
            <li class="info">Total pieces</li>
            <li class="info" id="liInvoice" runat="server">Invoice number</li>
            <li class="info">Packaging</li>
        </ul>
    </div>
    <div>
         <ul>
            <li><asp:Label id="lbService" runat="server"></asp:Label></li>
            <li><asp:Label id="lbWeight" runat="server"></asp:Label></li>
            <li><asp:Label id="lbTotalpieces" runat="server"></asp:Label></li>
            <li id="liInvoice1" runat="server"><asp:Label id="lbInvoicenumber" runat="server"></asp:Label></li>
            <li><asp:Label id="lbPackaging" runat="server"></asp:Label></li>
        </ul>
    </div>
    </div>
</div>          

<asp:Literal id="ltContent" runat="server"></asp:Literal>

</div>
</ContentTemplate>
</asp:UpdatePanel>

</div>

</center>
</asp:Content>
