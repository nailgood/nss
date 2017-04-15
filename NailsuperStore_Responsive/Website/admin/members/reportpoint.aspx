<%@ Page Language="VB" AutoEventWireup="false" CodeFile="reportpoint.aspx.vb" Inherits="admin_members_reportpoint" MasterPageFile="~/includes/masterpage/admin.master" %>
<%@ Register src="~/controls/layout/pager.ascx" tagname="pager" tagprefix="uc1" %>

<asp:Content ID="Content" runat="server" ContentPlaceHolderID="ph">
    <link href="/includes/theme/css/pager.css" rel="stylesheet" type="text/css" />
 <h4>Summary Point</h4>
<asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
<table>
<tr><td class="smaller field">Month <asp:DropDownList ID="drpMonth" runat="server"></asp:DropDownList></td>
<td class="smaller field">  Year <asp:DropDownList ID="drpYear" runat="server"></asp:DropDownList></td>
<td><CC:OneClickButton id="btnSearch" Runat="server" Text="Search" cssClass="btn" /></td>
</tr>
</table>
</asp:Panel>
<br /><br />
<table cellpadding="2" cellspacing="2">

<tr><th colspan="2">Total</th></tr>
<tr>
<td class="field bold">Point available</td><td class="field bold red"><%=TotalPavailable%> <%=Worth%></td>
</tr>
<tr>
<td class="field bold">Point pending</td><td class="field bold red"><%=PointPending%></td>
</tr>
<tr>
<td class="field bold">Points accumulated in <%=MonthN%> <%=Year%> </td><td class="field bold red"><%=PointsAccumulatedMonth%></td>
</tr>
<tr>
<td class="field bold">Points accumulated in <%=Year%></td><td class="field bold red"><%=PointsAccumulatedYear%></td>
</tr>
<tr>
<td class="field bold">Points earned up to date</td><td class="field bold red"><%=PointEarnedUptodate%></td>
</tr>
<tr>
<td class="field bold">Points debit in <%=MonthN%> <%=Year%></td><td class="field bold red"><%=PointDebitInMonth%></td>
</tr>
<tr>
<td class="field bold">Points debit up to date</td><td class="field bold red"><%=PointDebitUptodate%></td>
</tr>
</table>
<br />
<%--<CC:OneClickButton id="btnExportCVS" Runat="server" Text="Export excel" cssClass="btn" />--%>
<br />
	    <div>
	    <CC:GridView ID="gvList" runat="server" AllowSorting="True" AutoGenerateColumns="False" AllowPaging="true" 
        BorderWidth="0" CellPadding="2" CellSpacing="2" EmptyDataText="There are no records that match the search criteria"
        HeaderText="" PagerSettings-Position="Bottom"
        PageSize="50">
<HeaderStyle VerticalAlign="Top"></HeaderStyle>

        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top" />
        <RowStyle CssClass="row" VerticalAlign="Top" />
        <Columns>
        <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="sp">
		    <HeaderTemplate>Full Name</HeaderTemplate>
            <ItemTemplate>
		        <asp:Literal ID="ltName" runat="server"></asp:Literal>
		    </ItemTemplate>

<HeaderStyle CssClass="sp"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
        </asp:TemplateField>
		<asp:BoundField DataField="Address1" HeaderText="Address 1" ></asp:BoundField>
		<asp:BoundField DataField="Address2" HeaderText="Address 2" ></asp:BoundField>
		<asp:BoundField DataField="City" HeaderText="City" ></asp:BoundField>
		<asp:BoundField DataField="State" HeaderText="State" ></asp:BoundField>
		<asp:BoundField DataField="Country" HeaderText="Country" ></asp:BoundField>
		<asp:BoundField DataField="Zip" HeaderText="Zip" ></asp:BoundField>
		<asp:TemplateField ItemStyle-HorizontalAlign="Right" SortExpression="TotalPoint" HeaderText= "Point Available" HeaderStyle-Width="135px" HeaderStyle-CssClass="sp">
		    <ItemTemplate>
		        <asp:Literal ID="ltTotalPointavailable" runat="server"></asp:Literal>
		    </ItemTemplate>

<HeaderStyle CssClass="sp" Width="135px"></HeaderStyle>

<ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:TemplateField>
		<asp:BoundField DataField="PointPending" SortExpression="PendingPoint" HeaderText="Point Pending" HeaderStyle-Width="80px" HeaderStyle-CssClass="sp" ItemStyle-HorizontalAlign="Right">
<HeaderStyle CssClass="sp" Width="80px"></HeaderStyle>

<ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
		<asp:BoundField DataField="Pointsaccumulatedinmonth"  HeaderText="Point accumulated in month" HeaderStyle-CssClass="sp" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
<HeaderStyle CssClass="sp" Width="70px"></HeaderStyle>

<ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
		<asp:BoundField DataField="Pointsaccumulatedinyear"  HeaderText="Point accumulated in year" HeaderStyle-CssClass="sp" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
<HeaderStyle CssClass="sp" Width="70px"></HeaderStyle>

<ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
		<asp:BoundField DataField="Pointsearneduptodate"  HeaderText="Point earned up to date" HeaderStyle-CssClass="sp" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
<HeaderStyle CssClass="sp" Width="70px"></HeaderStyle>

<ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
		<asp:BoundField DataField="PointDebitinMonth"  HeaderText="Point debit in month" HeaderStyle-CssClass="sp" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
<HeaderStyle CssClass="sp" Width="70px"></HeaderStyle>

<ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
		<asp:BoundField DataField="Pointsdebituptodate"  HeaderText="Point debit up to date" HeaderStyle-CssClass="sp" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
<HeaderStyle CssClass="sp" Width="70px"></HeaderStyle>

<ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>
		
		</Columns>
    </CC:GridView>
	    </div>
 
 
     
</asp:Content>