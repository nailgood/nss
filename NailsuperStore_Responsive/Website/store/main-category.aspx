<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/main.master"
    AutoEventWireup="false" CodeFile="main-category.aspx.vb" Inherits="store_main_category" %>

<%@ Register Src="../controls/layout/menu/main-category.ascx" TagName="main" TagPrefix="uc1" %>
<%@ Register Src="~/controls/product/testimonial-item.ascx" TagName="testimonial"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div id="main-cate-page">
        <section class="list-cate">
            <div class="left-col">
                <uc1:main ID="ucLeftMenu" runat="server" />
                <uc2:testimonial ID="ucTestimonial" runat="server" />                
            </div>
            <div class="main-col">
                <h1>
                    <asp:Literal ID="ltrDepartmentName" runat="server">
                    </asp:Literal>
                </h1>
                <div id="cat-desc"  class="dept-desc"><%=strDescription%></div>
               
                <div class="data">
                 <div class="top-line">&nbsp;</div>
                    <asp:Repeater ID="rptSubCate" runat="server">
                        <ItemTemplate>
                            <div id="divItem" runat="server">
                                <div class="hidden-left">
                                    &nbsp;</div>
                                <div class="hidden-top">
                                    &nbsp;</div>
                                <div class="box">
                                    <asp:Literal ID="ltrImage" runat="server">
                                    </asp:Literal>
                                </div>
                                <div class="name">
                                    <asp:Literal ID="ltrName" runat="server">
                                    </asp:Literal></div>
                                <div class="empty">
                                    &nbsp;
                                </div>
                                <div class="hidden-bottom">
                                    &nbsp;</div>
                                <div class="hidden-right">
                                    &nbsp;</div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </section>
        <section class="banner-list">
        </section>
    </div>
    <script type="text/javascript">

        $(window).load(function () {
            $(".dept-desc").css("display", "block");
            fnGetTopRowTitle($(".dept-desc"), 5, 20, 65, 60, 0, false);
            var lstchild = $('#main-cate-page .list-cate .data').children('.item');
            ResetHeightList(lstchild, 'main-cate');
            SetArrowPosition();
            //var row = getRows('.main-col .desc');
            // alert(row);
        });
        $(window).resize(function () {

           // if (window.ViewPortVidth > 767) {
                SetArrowPosition();
            //}
            fnGetTopRowTitle($(".dept-desc"), 5, 20, 65, 60, 0, false);
            var lstchild = $('#main-cate-page .list-cate .data').children('.item');
            ResetHeightList(lstchild, 'main-cate');
     
            

        });
        $(window).scroll(function () {
           // if (window.ViewPortVidth < 992 && window.ViewPortVidth > 767)
           // {
                SetArrowPosition();
         
            //}
        });
        
        
    </script>
</asp:Content>
