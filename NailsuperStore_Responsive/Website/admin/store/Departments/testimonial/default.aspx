<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="admin_store_departments_testimonial_default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">
<h4>Department Testimonial</h4> 
<asp:Panel ID="pnlAddNew" runat="server">
<table>
<tr>
    <th valign="top">
                <b>Department:</b>
            </th>
            <td valign="top" class="field">
                <asp:DropDownList ID="F_DepartmentId" runat="server" AutoPostBack="true" ></asp:DropDownList>
            </td>
</tr>
<tr>
<td><br />
    <input type="button" class="btn" id="btnAddNew" value="Add Review" onclick="OpenPopUp();" />           
    <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btnHidden" />
</td>
</tr>
</table>    
</asp:Panel>
<br />

<CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" 
        AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria"
        AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
        <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
        <Columns>  
            <asp:TemplateField HeaderText="Reviewer" ItemStyle-Width="120">
                <ItemTemplate>
                    <asp:Literal ID="ltrReviewName" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:BoundField DataField="ReviewTitle" HeaderText="Title" ItemStyle-Width="250"></asp:BoundField>
              <asp:TemplateField HeaderText="Comment" ItemStyle-Width="400">
                <ItemTemplate>
                    <asp:Literal ID="ltrComment" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
               <asp:TemplateField HeaderText="Item" ItemStyle-Width="250">
                <ItemTemplate>
                    <asp:Literal ID="ltrItemName" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Stars">
                <ItemTemplate>
                    <asp:Literal ID="ltrStar" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="Date" ItemStyle-Width="140">
                <ItemTemplate>
                    <asp:Literal ID="ltrDate" runat="server"></asp:Literal>
                </ItemTemplate>        
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Edit">
                <ItemTemplate>
                    <img src="/includes/theme-admin/images/edit.gif" style="border:none;" alt="" onclick="Openpopup('/admin/store/items/reviews/view.aspx?ReviewId=<%#DataBinder.Eval(Container.DataItem, "ItemReviewId") %>');" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete">
                <ItemTemplate>
                    <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif"
                        CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemReviewId")%>'
                        OnClientClick="return ConfirmDelete();" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                     <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up" CommandArgument="<%#Container.DataItem.ItemReviewId%>" />
                    <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif" CommandName="Down" CommandArgument="<%#Container.DataItem.ItemReviewId%>" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </CC:GridView>
    <input type="hidden" runat="server" value="" id="hidPopUpReviewId" />
    <input type="hidden" runat="server" value="" id="hidSaveValue" />
    <input type="hidden" runat="server" value="" id="hidPopupReturn" />
    <script type="text/javascript" src="../../../../includes/theme-admin/scripts/Browser.js"></script>
     <script type="text/javascript">
         function ConfirmDelete() {
             if (!confirm('Are you sure to delete?')) {
                 return false;
             }
         }

         function SetValue(save, value) {
             if (save == '1') {
                 document.getElementById('<%=hidPopUpReviewId.ClientID %>').value = value;
             }
             else {
                 
                 window.location = value;
                 
             }
             document.getElementById('<%=hidSaveValue.ClientID %>').value = save;
         }

         function OpenPopUp() {
             var departmentId = document.getElementById('<%=F_DepartmentId.ClientID %>').value;
             var item = document.getElementById('<%=hidPopUpReviewId.ClientID %>').value;
             var url = 'SearchReview.aspx?F_DepartmentId=' + departmentId + '&ReviewId=' + item

             var brow = GetBrowser();
             if (brow == 'safari') {
                 var p = window.open(url, "SearchReview", "width=1200, height=700");
           
             }
             else 
             {
                 var p = window.showModalDialog(url, '', 'dialogHeight: 700px; dialogWidth: 1200px; dialogTop: 100px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');

                 if (brow == 'ie') {
                     document.getElementById('<%=hidPopUpReviewId.ClientID %>').value = p;
                     var button = document.getElementById('<%=btnSave.ClientID %>');
                     if (button)
                         button.click();
                 }
                 else {
                     var saveValue = document.getElementById('<%=hidSaveValue.ClientID %>').value;
                     if (saveValue == '1') {
                         var button = document.getElementById('<%=btnSave.ClientID %>');
                         if (button)
                             button.click();
                     }
                 }
             }
         }

         function ButtonClick(value) {
             document.getElementById('<%=hidPopUpReviewId.ClientID %>').value = value;
             var button = document.getElementById('<%=btnSave.ClientID %>');
             if (button)
                 button.click();
         }
         function showPopup(url) {
             var p = window.showModalDialog(url, '', 'dialogHeight: 600px; dialogWidth: 620px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
             var brow = GetBrowser();
             if (brow == 'ie') {
                 if (p) {
                     if (p == '0')
                         return;
                     window.location = p;
                 }
             }
             else {
                 var returnValue = document.getElementById('<%=hidPopupReturn.ClientID %>').value;
                 if (returnValue == '')
                     return;
                 if (returnValue == '0')
                     return;
                 window.location = returnValue;
             }
         }
     </script>   
</asp:Content>

