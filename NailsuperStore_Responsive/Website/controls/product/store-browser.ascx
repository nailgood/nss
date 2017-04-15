<%@ Control Language="VB" AutoEventWireup="false" CodeFile="store-browser.ascx.vb"
    Inherits="controls_store_browser" %>
<%@ Register TagName="Resources" TagPrefix="uc" Src="~/controls/resource-center/menu.ascx" %>
<%@ Register TagName="ResourcesCenter" TagPrefix="ucResourcesCenter" Src="~/controls/layout/menu/resource-center.ascx" %>
<%@ Register TagName="NarrowSearch" TagPrefix="uc" Src="~/controls/product/narrow-search.ascx" %>
<asp:Panel ID="pnDepartmentBrand" runat="server" ClientIDMode="Static">
    <div id="leftmenu" class="<%=noneBorder %>">
        <div class="title-dept" id="divTitleDepartment" runat="server" clientidmode="Static">
            <asp:Literal EnableViewState="true" runat="server" ID="litBreadCrumb" /></div>
        <span class="glyphicon ic-minus ic-plus hidden" style="float: right"></span>
        <ul id="content-mn">
            <li>
                <asp:Repeater runat="server" ID="rptDepartments">
                    <HeaderTemplate>
                        <ul id="main-cat">
                            <asp:Literal runat="server" ID="litSales" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="lit" />
                        <asp:TreeView EnableClientScript="false" EnableViewState="false" runat="server" Visible="false"
                            ID="tree" ShowExpandCollapse="False" SkipLinkText="" NodeIndent="0" ShowLines="false"
                            NodeWrap="false" CssClass="tree" />
                        <asp:Literal runat="server" ID="ltrSubMenu" />
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>
                <%  If ShowNarrowSearchSubCate Then%>
                <uc:NarrowSearch runat="server" ID="nrs" />
                <%End If%>
                <%  If Not String.IsNullOrEmpty(brandHTML) Then%>
                <div id="departmentBrand">
                    <div class="title">
                        Shop by brand
                    </div>
                    <%=brandHTML%>
                </div>
                <%End If%>
                <ucResourcesCenter:ResourcesCenter runat="server" ID="ucResourcesCenter" />
            </li>
        </ul>
    </div>
<%--    <div class="ver-line pull-right">
        &nbsp;</div>--%>
</asp:Panel>
<asp:HiddenField ID="isRefineesult" runat="server" Value="1" />
<script language="javascript">
    $(window).resize(function () {
        fnReplateContentLeftMenu();
    });
    $(window).load(function () {
        fnRemoveRightLine();
        fnReplateContentLeftMenu();
        fnShowRefineResult();
        fnRemoveIconPlus();
        fnCheckboxNarrowSeach();
    });
    var pathname = window.location.pathname;
    fnRemoveRightLine = function () {
       
        var url = window.location.href;
       // alert(pathname + '---' + url);
        if (pathname.indexOf('product-reviews') != -1) 
        {
            $("#leftmenu").css('border-right', 'none');
        }

    }
    fnShowRefineResult = function () {
        if ($("#isRefineesult").val() == 0) {
            $("#narrowsearch .title").css("display", "none");
            //$("#main-cat").css("display", "none")
            $("#pnDepartmentBrand ul").css('padding-bottom','0');
        }
        //$("#narrowsearch").css("padding", "0");
    }
    fnRemoveIconPlus = function () {
        if ($('#narrowsearch').length <= 0 && $('#content-left').length > 0 && pathname.indexOf('product-reviews') == -1) {
            $('#leftmenu').css('border-right', 'none');
            //$('#content-left').addClass('hidden');
            $('#pnDepartmentBrand .ver-line').css('height', '0');
        }
        $('.testimonial-item').css('margin-top', '0');
    }
</script>