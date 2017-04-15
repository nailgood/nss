<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="" CodeFile="ArrangeItems.aspx.vb" Inherits="admin_store_Departments_ArrangeItems"  %>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
<h4>Upate Arrange: <%=deptName%></h4>

<div class="u-list">
<CC:OneClickButton id="Update" Runat="server" Text="Update" cssClass="btn u-list-w-btn"></CC:OneClickButton>
</div>


<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" 
        PageSize="1000" AllowPaging="false" AllowSorting="True" 
        HeaderText="" 
        EmptyDataText="There are no records that match the search criteria" 
        AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom" 
        SortOrder="Asc" CssClass="u-list-w-fix">
<HeaderStyle VerticalAlign="Top"></HeaderStyle>

	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		
		<asp:BoundField SortExpression="ItemName" DataField="Itemname" HeaderText="Item"></asp:BoundField>
		
		<asp:Checkboxfield ItemStyle-HorizontalAlign="Center" SortExpression="IsActive" DataField="IsActive" HeaderText="Is Active">
<ItemStyle HorizontalAlign="Center"></ItemStyle>
        </asp:Checkboxfield>
     
    <asp:TemplateField HeaderText="Arrange" SortExpression="Arrange" HeaderStyle-CssClass="u-list-w-arrange">
        <ItemTemplate>
        <asp:Label ID="lb_<%#DataBinder.Eval(Container.DataItem, "ItemId") %>" Style="display:none"><%#DataBinder.Eval(Container.DataItem, "Arrange") %></asp:Label>
            <input type="text" id="<%#DataBinder.Eval(Container.DataItem, "ItemId") %>" onchange="changeval('<%#DataBinder.Eval(Container.DataItem, "ItemId") %>')" value='' style="width:98%" />
        </ItemTemplate>
    </asp:TemplateField>
	</Columns>
  
</CC:GridView>
<asp:HiddenField ID="hlistItem" runat="server" />
<asp:HiddenField ID="hlistVal" runat="server" />
<script language="javascript">

    $(document).ready(function () {
        fnBindArrang();
//        $("#ctl00_ph_Update").click(function () {
//            var listItem = "";
//            $('#ctl00_ph_gvList input[type="text"]').each(function () {
//                var id = this.id,
//                val = $("#" + id).val();
//                    if (val > 0) {
//                        listItem += id + "-" + val + "|";
//                    }
//                    alert(listItem);
//        
//            });
//            $("#<%=hlistItem.ClientID %>").val(listItem);
//        });
    });
    var listItem = "";
    function changeval(id) {
        var val = $("#" + id).val();
        listItem += id + "-" + val + "|";
       //alert(listItem);
        $("#<%=hlistItem.ClientID %>").val(listItem);
    }
    fnBindArrang = function () {
        $('#ctl00_ph_gvList input[type="text"]').each(function () {
            var id = this.id;
           // alert($("#lb_" + id).text());
            $("#" + id).val($("#lb_" + id).text());

        });
    }
</script>
</asp:content>
