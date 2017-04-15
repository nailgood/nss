<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/interior.master" AutoEventWireup="false" CodeFile="department-tab.aspx.vb" Inherits="store_department_tab" %>
<%@ Register src="~/controls/product/list.ascx" tagname="product" tagprefix="uc" %>
<%@ Register Src="~/controls/layout/bread-crumb.ascx" TagName="breadcrumb" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <h1 class="c-h1"><asp:Literal ID="ltrDepartmentName" runat="server"></asp:Literal></h1>
    <div class="line" style="clear: both"></div>
    <div id="cat-desc" class="dept-desc"><%= Description%></div>
    <uc:product ID="ucListProduct" runat="server" />
    <script language="javascript">
            $(window).load(function () {
                fnGetTopRowTitle($(".dept-desc"), 5, 20, 65, 60, 0, false); //1:container, 2:line show,  3:line height, 4: min space of word last line, 5: min left postion read more, 6: word add,7: end call function
            });
    </script>
</asp:Content>
