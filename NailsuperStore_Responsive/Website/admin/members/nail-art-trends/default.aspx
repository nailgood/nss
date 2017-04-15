<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="admin_members_nail_art_trends_default" %>

<asp:Content ID="Content" runat="server" ContentPlaceHolderID="ph">
    <script type="text/javascript">
    function ConfirmDelete() {
        if (!confirm('Are you sure to delete?')) {
            return false;
        }
    }

    </script>
    <h4>
        Member Submmission</h4>

<span class="smaller">Please provide search criteria below</span>
<asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
<table cellpadding="2" cellspacing="2">
<tr>
<th valign="top"><b>Type:</b></th>
<td valign="top" class="field">
	<asp:DropDownList ID="F_Type" runat="server">
		<asp:ListItem Value="">-- ALL --</asp:ListItem>
		<asp:ListItem Value="0">Nail Art Trends</asp:ListItem>
	</asp:DropDownList>
</td>
</tr>
<tr>
<th valign="top"><b>Status:</b></th>
<td valign="top" class="field">
	<asp:DropDownList ID="F_Status" runat="server">
		<asp:ListItem Value="">-- ALL --</asp:ListItem>
		<asp:ListItem Value="1">Yes</asp:ListItem>
		<asp:ListItem Value="0">No</asp:ListItem>
	</asp:DropDownList>
</td>
</tr>
<tr>
<th valign="top"><b>Submited Date:</b></th>
<td valign="top" class="field">
<table border="0" cellpadding="0" cellspacing="0">
<tr><td class="smaller">From <CC:DatePicker id="F_SubmitedDateLbound" runat="server" /></td><td>&nbsp;</td><td class="smaller">To  <CC:DatePicker id="F_SubmitedDateUbound" runat="server" /></td>
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
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "edit.aspx?SubmissionId=" & DataBinder.Eval(Container.DataItem, "SubmissionId") %>' ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit"></asp:HyperLink>
			</itemtemplate>
            </asp:TemplateField>
     
              <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete">
                <ItemTemplate>
                    <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif"
                        CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "SubmissionId")%>'
                        OnClientClick="return ConfirmDelete();" />
                </ItemTemplate>
            </asp:TemplateField>
            
		<asp:TemplateField ItemStyle-HorizontalAlign="Center">
		    <HeaderTemplate>
		        Name
		    </HeaderTemplate>
            <ItemTemplate>
		        <asp:Literal ID="ltrMemberId" runat="server"></asp:Literal>
		    </ItemTemplate>
        </asp:TemplateField>
		<asp:BoundField DataField="Email" HeaderText="Email"></asp:BoundField>
		<asp:BoundField DataField="ArtName" HeaderText="ArtName"></asp:BoundField>
		<asp:TemplateField ItemStyle-HorizontalAlign="Center">
		    <HeaderTemplate>
		        Files
		    </HeaderTemplate>
            <ItemTemplate>
            <asp:HyperLink ID="hblSave" runat="server"></asp:HyperLink>
		        <asp:Literal ID="ltrFile" runat="server"></asp:Literal>
		    </ItemTemplate>
        </asp:TemplateField>
		<asp:BoundField DataField="SubmittedDate" HeaderText="Submitted Date" DataFormatString="{0:d}" HTMLEncode="False"></asp:BoundField>
		   <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Status">
                <ItemTemplate>
                    <asp:ImageButton ID="imbStatus" runat="server" ImageUrl="/includes/theme-admin/images/active.png"
                        CommandName="Active" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "SubmissionId")%>' />
                </ItemTemplate>
            </asp:TemplateField>
	
        </Columns>
    </CC:GridView>
    <asp:HiddenField ID="hidCon" runat="server" />
</asp:Content>
