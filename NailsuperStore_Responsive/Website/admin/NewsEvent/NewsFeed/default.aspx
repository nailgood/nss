<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="admin_NewsEvent_NewsFeed_default" %>


<asp:Content ID="Content" runat="server" ContentPlaceHolderID="ph">
    <script type="text/javascript">
    function ConfirmDelete() {
        if (!confirm('Are you sure to delete?')) {
            return false;
        }
    }

    </script>
    <h4>
        News Feed</h4>

<span class="smaller">Please provide search criteria below</span>
<asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
<table cellpadding="2" cellspacing="2">
<tr>
<th valign="top"><b>Title:</b></th>
<td valign="top" class="field">
	<asp:TextBox ID="F_txtTitle" runat="server"></asp:TextBox>
</td>
</tr>
<tr>
<th valign="top"><b>Is Active:</b></th>
<td valign="top" class="field">
	<asp:DropDownList ID="F_IsActive" runat="server">
		<asp:ListItem Value="">-- ALL --</asp:ListItem>
		<asp:ListItem Value="1">Yes</asp:ListItem>
		<asp:ListItem Value="0">No</asp:ListItem>
	</asp:DropDownList>
</td>
</tr>
<tr>
<th valign="top"><b>Date:</b></th>
<td valign="top" class="field">
<table border="0" cellpadding="0" cellspacing="0">
<tr><td class="smaller">From <CC:DatePicker id="F_CreatedDateLbound" runat="server" /></td><td>&nbsp;</td><td class="smaller">To  <CC:DatePicker id="F_CreatedDateUbound" runat="server" /></td>
</tr>
</table>
</td>
</tr>
<tr>
<td colspan="2" align="right">
<CC:OneClickButton id="btnSearch" Runat="server" Text="Search" cssClass="btn" />
<input class="btn" type="submit" value="Clear" onclick="window.location='default.aspx';return false;" />
</td>
</tr>
<tr>
   <td colspan ="2" align="left"><CC:OneClickButton ID="btnAddNew" runat="server" Text="Add new" CssClass="btn"></CC:OneClickButton></td> 
</tr>
</table>
</asp:Panel>
<p></p>

    <CC:GridView ID="gvList" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
        BorderWidth="0" CellPadding="2" CellSpacing="2" EmptyDataText="There are no records that match the search criteria"
        HeaderText="In order to change display order, please use header links" PagerSettings-Position="Bottom"
        PageSize="50">
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top" />
        <RowStyle CssClass="row" VerticalAlign="Top" />
        <Columns>
         <asp:TemplateField>
          
                <itemtemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "edit.aspx?NewsFeedId=" & DataBinder.Eval(Container.DataItem, "NewsFeedId") %>' ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit"></asp:HyperLink>
			</itemtemplate>
            </asp:TemplateField>
     
              <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete">
                <ItemTemplate>
                    <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif"
                        CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "NewsFeedId")%>'
                        OnClientClick="return ConfirmDelete();" />
                </ItemTemplate>
            </asp:TemplateField>
    
		<asp:BoundField DataField="Title" HeaderText="Title"></asp:BoundField>
		<asp:BoundField DataField="Url" HeaderText="Url"></asp:BoundField>
		<asp:BoundField DataField="CreatedDate" HeaderText="Date" DataFormatString="{0:d}" HTMLEncode="False"></asp:BoundField>
		   <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Status">
                <ItemTemplate>
                    <asp:ImageButton ID="imbStatus" runat="server" ImageUrl="/includes/theme-admin/images/active.png"
                        CommandName="Active" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "NewsFeedId")%>' />
                </ItemTemplate>
            </asp:TemplateField>
	
        </Columns>
    </CC:GridView>
    <asp:HiddenField ID="hidCon" runat="server" />
</asp:Content>
