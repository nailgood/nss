<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_admins_AdminIPAccess_Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
<asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>
<script type="text/javascript">
    function ConfirmDelete() {
        if (!confirm('Are you sure to delete?')) {
            return false;
        }
    }

</script>

<div style="margin:0 20px">
<h4><asp:Literal ID="ltrHeader" runat="server" Text="List IP Access"></asp:Literal></h4>

<asp:Panel ID="pnList" runat="server">
<div style="padding-bottom:10px">   
    <CC:OneClickButton id="btnAdd" runat="server" Text="Add New IP" cssClass="btn" ValidationGroup="val1" CausesValidation="False"></CC:OneClickButton>
    <CC:OneClickButton id="btnBack" runat="server" Text="Back" cssClass="btn" ValidationGroup="val1" CausesValidation="False"></CC:OneClickButton>
    <asp:Label ID="ltrMsg" runat="server" CssClass="red"></asp:Label>
</div>    

<CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50"
        AllowPaging="false" AllowSorting="false" EmptyDataText="There are no records that match the search criteria"
        AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
        <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
        <Columns>
        <asp:TemplateField>
            <HeaderTemplate>No.</HeaderTemplate>
            <ItemTemplate>
                <%#Container.DataItemIndex + 1%>
            </ItemTemplate>
        </asp:TemplateField>               
        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Username">
        <ItemTemplate>
            <%=UserName()%>
        </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="IP Address">
            <ItemTemplate>
                <asp:Literal ID="ltrIP" runat="server"></asp:Literal>
            </ItemTemplate>
        </asp:TemplateField>
           
        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete">
            <ItemTemplate>
                <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif"
                    CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>'
                    OnClientClick="return ConfirmDelete();" />
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
    </CC:GridView>

</asp:Panel>
</div>


</asp:content>

