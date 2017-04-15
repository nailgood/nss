<%@ Page Language="vb" AutoEventWireup="false" Inherits="members_classifieds" CodeFile="classifieds.aspx.vb" MasterPageFile="~/includes/masterpage/main.master" %>
<%--<%@ Register Src="~/modules/SearchBar.ascx" TagPrefix="uc" TagName="SearchBar" %>
<%@ Register Src="~/modules/Menu.ascx" TagPrefix="uc" TagName="Menu" %>
<%@ Register Src="~/modules/EmailSignup.ascx" TagPrefix="uc" TagName="EmailSignup" %>
<%@ Register Src="~/modules/NeedAssistance.ascx" TagPrefix="uc" TagName="NeedAssistance" %>
<%@ Register Src="~/modules/CustomerServiceMenu.ascx" TagPrefix="uc" TagName="CustomerServiceMenu" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
<center>

<div id="header-page">
<%--<uc:SearchBar runat="server" ID="ucSearchBar" />
<uc:Menu runat="server" ID="ucMenu" /> --%> 
</div>

<div id="page">

<div id="left-page">
<%--<uc:CustomerServiceMenu runat="server" ID="ucCustomerServiceMenu" />
<uc:NeedAssistance runat="server" ID="ucNeedAssistance" />
<uc:EmailSignup runat="server" ID="ucEmailSignup" />--%>
</div>

<div id="content-page" preventDefaultButton="ctl02_btnSearch">
<div class="bc" style="text-align:left;">
	<a class="bdcrmblnk" href="/home.aspx">Home</a> <img src="/includes/theme/images/icon_stepmenu.gif"> <a class="bdcrmblnk" href="/members/">My Account</a> <img src="/includes/theme/images/icon_stepmenu.gif"> <span class="bcative">My Classifieds</span>
</div>
<CT:ErrorMessage id="ErrorPlaceHolder" runat="server"/>
<div id="Div1" runat="server" class="form" style="width:100%;padding-top:10px;">
    <div class="title">My Classifieds</div>
    <div class="border">
    
   

<div style="padding-top:10px">
<CC:GridView id="gvList" Width="100%" CellSpacing="1" BorderColor="#dadada" CellPadding="1" runat="server" PageSize="25" AllowPaging="True" AllowSorting="True" HeaderText="" EmptyDataText="You have no current classified ads." AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top" ></RowStyle>
	<Columns>
		<asp:TemplateField>
			<HeaderTEmplate></HeaderTEmplate>
			<ItemTemplate>
			<asp:Button id="btndelete" runat="server" CssClass="btng60"  Text="Delete"  onclientclick="return confirm('Are you sure you wish to remove this classified ad?');" runat="server" CommandName="Remove" CommandArgument='<%#Container.DataItem("ClassifiedId")%>' /></ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<HeaderTEmplate><b>Title</b></HeaderTEmplate>
			<ItemTemplate><a href='/services/classifieds/detail.aspx?Member=Y&ClassifiedId=<%#Container.DataItem("ClassifiedId")%>' class="lgry"><%#Container.DataItem("Title")%></a></ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField DataField="category" HeaderText="Category"></asp:BoundField>
	</Columns>
</CC:GridView></div>
    </div>
</div>

</div>

</div>
</center>

</asp:Content>