<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="UPS Fee" CodeFile="default.aspx.vb" Inherits="admin_store_UPSFee_default" %>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
<h4>Edit <%If Subject IsNot String.Empty Then %> <%=Subject %> <%Else %>Shipping<%End If %> Fee</h4>

<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" AutoGenerateEditButton="true" BorderWidth="0" PagerSettings-Position="Bottom" OnRowEditing="gvList_RowEditing" OnRowUpdating="gvList_RowUpdating" OnRowCancelingEdit="gvList_RowCancelingEdit">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		    <asp:TemplateField HeaderText="Shipment Method">
		        <ItemTemplate>
		        <%#Eval("Name")%>
		           <span style="float:left;padding:0 5px"><%#ShowIcon(Eval("Code"), Eval("MethodId"), Eval("Name")) %></span> <asp:Label runat="server" ID="lblMethodId" Text='<%#Eval("MethodId") %>' Visible="false"></asp:Label>
		        </ItemTemplate>
		    </asp:TemplateField>
			<asp:TemplateField HeaderText="Residential">
		    <ItemTemplate>
		        <%#FormatCurrency(Eval("Residential"))%>
		    </ItemTemplate>
		    <EditItemTemplate>
		        <asp:TextBox ID="txtResidential" runat="server" Text='<%# FormatCurrency(Eval("Residential")) %>'></asp:TextBox>
		    </EditItemTemplate>
		    </asp:TemplateField>
		    <asp:TemplateField HeaderText="Signature">
		    <ItemTemplate>
		        <%#FormatCurrency(Eval("Signature"))%>
		    </ItemTemplate>
		    <EditItemTemplate>
		        <asp:TextBox ID="txtSignature" runat="server" Text='<%#FormatCurrency(Eval("Signature")) %>'></asp:TextBox>
		    </EditItemTemplate>
		    </asp:TemplateField>
			<asp:TemplateField HeaderText="Insurance">
		    <ItemTemplate>
		        <%#FormatCurrency(Eval("Insurance"))%>
		    </ItemTemplate>
		    <EditItemTemplate>
		        <asp:TextBox ID="txtInsurance" runat="server" Text='<%#FormatCurrency(Eval("Insurance")) %>'></asp:TextBox>
		    </EditItemTemplate>
		    </asp:TemplateField>
		    <asp:TemplateField HeaderText="DASResidential">
		    <ItemTemplate>
		        <%#FormatCurrency(Eval("DASResidential"))%>
		    </ItemTemplate>
		    <EditItemTemplate>
		        <asp:TextBox ID="txtDASResidential" runat="server" Text='<%#FormatCurrency(Eval("DASResidential")) %>'></asp:TextBox>
		    </EditItemTemplate>
		    </asp:TemplateField>
		    <asp:TemplateField HeaderText="DASCommercial">
		    <ItemTemplate>
		        <%#FormatCurrency(Eval("DASCommercial"))%>
		    </ItemTemplate>
		    <EditItemTemplate>
		        <asp:TextBox ID="txtDASCommercial" runat="server" Text='<%#FormatCurrency(Eval("DASCommercial")) %>'></asp:TextBox>
		    </EditItemTemplate>
		    </asp:TemplateField>
		    <asp:TemplateField HeaderText="FuelRate">
		    <ItemTemplate>
		        <%#FormatPercent(Eval("FuelRate") / 100)%>
		    </ItemTemplate>
		    <EditItemTemplate>
		        <asp:TextBox ID="txtFuelRate" runat="server" Text='<%#FormatPercent(Eval("FuelRate")/100) %>'></asp:TextBox>
		    </EditItemTemplate>
		    </asp:TemplateField>
		
            <asp:TemplateField HeaderText="Hazardous Material">
		    <ItemTemplate>
		        <%#FormatCurrency(Eval("HazMatFee"))%>
		    </ItemTemplate>
		    <EditItemTemplate>
		        <asp:TextBox ID="txtHazMatFee" runat="server" Text='<%#FormatCurrency(Eval("HazMatFee")) %>'></asp:TextBox>
		    </EditItemTemplate>
		    </asp:TemplateField>
		</Columns>
</CC:GridView>
</asp:content>