<%@ Page AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="admin_members_default" Language="VB"
    MasterPageFile="~/includes/masterpage/admin.master" Title="Member" %>

<asp:Content ID="Content" runat="server" ContentPlaceHolderID="ph">
    <h4>
        Member</h4>

<span class="smaller">Please provide search criteria below</span>
<asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
<table cellpadding="2" cellspacing="2">
<tr>
<th align="right" valign="top">Customer No:</th>
<td valign="top" class="field"><asp:textbox id="F_CustomerNo" runat="server"  Columns="20"  MaxLength="50"></asp:textbox>
<span style="font:italic 11px Arial;color:#444;">Enter multi customer no with commas</span></td>
 <th align="right" valign="top">
    Ship To Country/State:
</th>
<td valign="top" class="field">
   <div style="float:left"><asp:DropDownList ID="F_ShipToCountry" onchange="changeSelect('Sstate')" runat="server" /></div>
   <div id="dSstate" style="display:none;float:left;text-align:center;"> <asp:DropDownList ID="F_ShipToCounty" runat="server" /></div>
</td>    
</tr>
<tr>
<th align="right" valign="top">Username:</th>
<td valign="top" class="field"><asp:textbox id="F_Username" runat="server" Columns="20" MaxLength="50"></asp:textbox></td>
<th align="right" valign="top">Professional Status:</th>
<td valign="top" class="field"><asp:DropDownList ID="F_MemberTypeId" runat="server" /></td>
</tr>

<tr>
<th align="right" valign="top">Email :</th>
<td style="background:#E2EBF7; vertical-align:middle;"><asp:textbox id="F_EmailAddress" runat="server" Columns="50" MaxLength="50" ></asp:textbox></td>
<th align="right" valign="top">Create Date:</th>
<td valign="top" class="field">
<table border="0" cellpadding="0" cellspacing="0">
<tr><td class="smaller">From <CC:DatePicker id="F_CreateDateLbound" runat="server" /></td><td>&nbsp;</td><td class="smaller">To  <CC:DatePicker id="F_CreateDateUbound" runat="server" /></td>
 <td>
                                &nbsp;
                            </td>
                            <td valign="bottom">
                                <asp:DropDownList ID="F_OrderDate" runat="server">
                                    <asp:ListItem Value="">-- ALL --</asp:ListItem>
                                    <asp:ListItem Value="0">Today</asp:ListItem>
                                    <asp:ListItem Value="3">Last 3 days</asp:ListItem>
                                    <asp:ListItem Value="7">Last 7 days</asp:ListItem>
                                    <asp:ListItem Value="month">Last a month</asp:ListItem>
                                </asp:DropDownList>
                            </td>
</tr>
</table>
</td>
</tr>
<tr>
<th align="right" valign="top">Phone:</th>
<td valign="top" class="field"><asp:textbox id="F_Phone" runat="server" Columns="20" MaxLength="20"></asp:textbox></td>
<th align="right" valign="top"></th>
<td valign="top" class="field">
	<asp:DropDownList ID="F_IsActive" runat="server">
		<asp:ListItem Value="">-- Is Active Register? --</asp:ListItem>
		<asp:ListItem Value="1">Yes</asp:ListItem>
		<asp:ListItem Value="0">No</asp:ListItem>
	</asp:DropDownList>
	<asp:DropDownList ID="F_DeActive" runat="server">
		<asp:ListItem Value="">-- Is DeActive Account?--</asp:ListItem>
		<asp:ListItem Value="1">Yes</asp:ListItem>
		<asp:ListItem Value="0">No</asp:ListItem>
	</asp:DropDownList>
</td>
</tr>
<tr>
<th align="right" valign="top">Address:</th>
<td valign="top" class="field"><asp:textbox id="F_Address" runat="server" Columns="50" MaxLength="100"></asp:textbox></td>
<th align="right" valign="top">Customer Posting Group:</th>
<td valign="top" class="field">
	<asp:DropDownList runat="server" ID="F_CustomerPostingGroup"  />
</td>
</tr>
<tr>
<td colspan="4" align="right">
<CC:OneClickButton id="btnSearch" Runat="server" Text="Search" cssClass="btn" />
<input class="btn" type="submit" value="Clear" onclick="window.location='default.aspx';return false;" />
</td>
</tr>
</table>
</asp:Panel>
<p></p>

    <CC:GridView ID="gvList" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
        BorderWidth="0" CellPadding="2" CellSpacing="2" EmptyDataText="There are no records that match the search criteria"
        HeaderText="In order to change display order, please use header links" PagerSettings-Position="Bottom"
        PageSize="50">
<HeaderStyle VerticalAlign="Top"></HeaderStyle>

        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top" />
        <RowStyle CssClass="row" VerticalAlign="Top" />
        <Columns>
            <asp:TemplateField>
                <itemtemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "edit.aspx?MemberId=" & DataBinder.Eval(Container.DataItem, "MemberId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <itemtemplate>
			<CC:ConfirmLink enableviewstate=False Message="Are you sure that you want to remove this Member?" runat="server" NavigateUrl= '<%# "delete.aspx?MemberId=" & DataBinder.Eval(Container.DataItem, "MemberId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
			</itemtemplate>
            </asp:TemplateField>
		<asp:BoundField SortExpression="MemberType" DataField="ProfessionalStatus" HeaderText="Professional Status"></asp:BoundField>
		<asp:BoundField DataField="CustomerPostingGroup" HeaderText="Group" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
		<%--<asp:BoundField SortExpression="Username" DataField="Username" HeaderText="Username"></asp:BoundField>--%>
		<asp:TemplateField SortExpression="Username" HeaderText="Username">
		     <ItemTemplate>
		        <asp:Literal ID="ltrUserName" runat="server"></asp:Literal>
		    </ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField DataField="Country" HeaderText="Country" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
		<asp:BoundField SortExpression="m.CreateDate" DataField="CreateDate" HeaderText="Create Date" DataFormatString="{0:G}" HTMLEncode="False"></asp:BoundField>
		<asp:TemplateField>
		    <HeaderTemplate>
		        Last Login Date
		    </HeaderTemplate>
		     <ItemTemplate>
		        <asp:Label ID="lblastDate" runat="server"></asp:Label>
		    </ItemTemplate>
		</asp:TemplateField>
		<asp:Checkboxfield ItemStyle-HorizontalAlign="Center" SortExpression="IsActive" 
                DataField="IsActive" HeaderText="Active<br>Register">
<ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:Checkboxfield>
		<asp:Checkboxfield ItemStyle-HorizontalAlign="Center" SortExpression="DeActive" 
                DataField="DeActive" HeaderText="Deactivate<br>Account" 
                ControlStyle-Width="100">
<ControlStyle Width="100px"></ControlStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:Checkboxfield>
		<asp:TemplateField ItemStyle-HorizontalAlign="Center">
		    <HeaderTemplate>
		        Password
		    </HeaderTemplate>
            <ItemTemplate>
		        <asp:HyperLink enableviewstate="False" runat="server" NavigateUrl='<%# string.Format("password.aspx?MemberId={0}&F_SortBy=Email&F_SortOrder=ASC", DataBinder.Eval(Container.DataItem, "MemberId")) %>' ImageUrl="/includes/theme-admin/images/preview.gif" ID="lnkPassword">Edit</asp:HyperLink>
		    </ItemTemplate>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:TemplateField>
		<asp:TemplateField ItemStyle-HorizontalAlign="Center">
		    <HeaderTemplate>
		        Orders
		    </HeaderTemplate>
            <ItemTemplate>
		        <asp:HyperLink enableviewstate="False" runat="server" NavigateUrl='<%# string.Format("../store/orders/default.aspx?MemberId={0}", DataBinder.Eval(Container.DataItem, "MemberId")) %>' ImageUrl="/includes/theme-admin/images/preview.gif" ID="lnkOrder">Order</asp:HyperLink>
		    </ItemTemplate>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:TemplateField>
		<%--<asp:BoundField SortExpression="ValidCashPoint" DataField="ValidCashPoint" HeaderText="Total points"></asp:BoundField>--%>
		<asp:TemplateField ItemStyle-HorizontalAlign="Center">
		    <HeaderTemplate>
		        Total<br>Points
		    </HeaderTemplate>
            <ItemTemplate>
		        <asp:Literal ID="ltrTotalPoint" runat="server"></asp:Literal>
		    </ItemTemplate>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:TemplateField>
        </Columns>
    </CC:GridView>
    <asp:HiddenField ID="hidCon" runat="server" />
    <script language="javascript">
        var dprShipCountry = $('#<%= F_ShipToCountry.ClientID %> option:selected').val();
        if (dprShipCountry == "US")
            document.getElementById("dSstate").style.display = "block";
        function changeSelect(val) {
           if (val == "Sstate") {
                var dprShip = $('#<%= F_ShipToCountry.ClientID %> option:selected').val();
                if (dprShip == "US")
                    document.getElementById("dSstate").style.display = "block";
                else {
                  document.getElementById("dSstate").style.display = "none";
                }

            }
        }
    </script>
</asp:Content>
