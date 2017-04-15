<%@ Page Language="VB" AutoEventWireup="false" CodeFile="detail.aspx.vb" Inherits="Media_Detail"
    MasterPageFile="~/includes/masterpage/interior.master" EnableViewState="false" %>

<%@ Register Src="~/controls/layout/addthis.ascx" TagName="share" TagPrefix="usSocialShare" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <div id="page">
        <div id="media-detail" itemscope itemtype="http://schema.org/Article">
            <div class="title">
                <h1 itemprop="headline name">
                    <asp:Literal ID="ltTitle" runat="server"></asp:Literal>
                </h1>
            </div>
             <meta itemprop="author" content="The Nail Superstore" />
             <meta itemprop="datePublished" content="<%=mediaDate %>" />
            <div id="media-des"  itemprop="description">
                <asp:Literal ID="ltShortDes" runat="server"></asp:Literal>
            </div>
            <div id="share_social">
                <div id="linkImg">
                    <asp:Literal ID="ltrLinkImg" runat="server"></asp:Literal>
                </div>
                <span>Share: </span>
                <usSocialShare:share ID="ushare" runat="server" />
            </div>
            <!--Embeded Video-->
            <div id="mediaImg">
                <asp:Image ID="img" runat="server" itemprop="image" ImageUrl="~/assets/MediaPress/d85e99d5-e922-43ec-90f0-dac4e163d6ad.jpg">
                </asp:Image>
            </div>
        </div>
    </div>
</asp:Content>
