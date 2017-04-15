<%@ Control Language="VB" AutoEventWireup="false" CodeFile="barstatus.ascx.vb" Inherits="controls_layout_barstatus" %>
<div id="bar-status">
       <%--<div class="h-invite hide-sm">
            <div class="header">
               <%=link%> 
            </div>
           <div class="header">
                <div class="h-text">Friend registers</div> 
           </div>
           <div class="header">
                  <span class="pull-left h-text">Friend orders</span>
                 <span class="pull-right end-text">You get <%=PointReferFriend%> points</span>
           </div>
            
        </div>--%>
        <div class="invite"> 
            <div class="content">
                 <span class="bar-circle">1</span>
                 <span class="txt"><%=link%></span>
               <%--<hr />--%>
            </div>
            <div class="content">
                 <span class="bar-circle">2</span>
                 <span class="txt">Friend registers</span>
                <%-- <hr />--%>
                 
            </div>
            <div class="content">
             <span class="bar-circle">3</span>
             <span class="txt">Friend orders</span>
            <%--<hr />--%>
            </div>
            <div class="content end-text">
                <span class="glyphicon glyphicon-ok"><span class="txt s-txt">You get <%=PointReferFriend%> points</span></span>
                
            </div>
        </div>
</div>