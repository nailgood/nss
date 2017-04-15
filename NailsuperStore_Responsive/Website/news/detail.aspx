<%@ Page Language="VB" AutoEventWireup="false" CodeFile="detail.aspx.vb" Inherits="News_Detail"
    MasterPageFile="~/includes/masterpage/interior.master"  %>
<%--EnableViewState="true"--%>
<%@ Register Src="~/controls/resource-center/review.ascx" TagName="ReviewNews" TagPrefix="rv" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
<script type="text/javascript" src="//s7.addthis.com/js/300/addthis_widget.js#pubid=ra-520b2d5b4eb4b59a"></script>
    <div id="newsdetail" itemscope itemtype="http://schema.org/Article">
            <h1 itemprop="headline name"><asp:Literal ID="litTitle" runat="server"></asp:Literal></h1>
             <meta itemprop="image" content="<%= GlobalSecureName & newsImg %>" />
             <meta itemprop="author" content="The Nail Superstore" />
             <meta itemprop="datePublished" content="<%=newsDate %>" />
             <meta itemprop="interactionCount" content="UserComments:<%=TotalComment %>" />
           <%-- <div class="shortdesc">
                <asp:Literal ID="litShortDes" runat="server"></asp:Literal>
            </div>--%>
            <div class="desc" itemprop="articleBody">              
                <div id="FlabellComponent" runat="server"></div>
                <div id="divContent" runat="server"></div>
            </div>
        </div>
        <div id="dvshare" style="display:none;"></div>
        <div id="comment">
            <div id="tabbox" class="tabbao">
                <nav>
                    <a id="tabnav_comment" class="selected">All Reviews (<%=TotalComment%>)<span class="arrow"></span></a>
                    <div id="stab_comment"  style="display:none;"></div>
                </nav>
            </div>
            <div id="tab_comment" class="boxtab">
                <rv:ReviewNews ID="rvComment" runat="server" />
            </div>
        </div>
        <script type="text/javascript">
            $(window).load(function () {
                ShowBoxShareNews();
                ShowTabOfNews();
            });
            $(window).resize(function () {
                ShowBoxShareNews();
                ShowTabOfNews();
            });
        </script>
    
</asp:Content>


