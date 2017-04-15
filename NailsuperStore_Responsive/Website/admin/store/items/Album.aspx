<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Album.aspx.vb" Inherits="admin_store_items_Album" MasterPageFile="~/includes/masterpage/admin.master" %>


<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">

    <script type="text/javascript">
    function ConfirmDelete() {
        if (!confirm('Are you sure to delete?')) {
            return false;
        }
    }

    </script>

    <h4>Album</h4>
         
    <p></p>
            <input type="button" class="btn" id="Button1" value="Add Album" onclick="OpenPopUp();" />
            <CC:OneClickButton ID="btnBack" runat="server" Text="Back" CssClass="btn" CausesValidation="False">
    </CC:OneClickButton>
     <div style="display: none">
      <CC:OneClickButton ID="btnAddAlbum" runat="server" Text="Add Album" CssClass="btn" CausesValidation="False">
    </CC:OneClickButton>
    </div>
    <p></p>
    
    <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50"
        AllowPaging="false" AllowSorting="True" EmptyDataText="There are no records that match the search criteria"
        AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
        <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
        <Columns>
           
            <asp:BoundField DataField="Name" HeaderStyle-Width="250" HeaderText="Album Name"></asp:BoundField>
           
          
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete">
                <ItemTemplate>
                    <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif"
                        CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "AlbumId")%>'
                        OnClientClick="return ConfirmDelete();" />
                </ItemTemplate>
            </asp:TemplateField>
       <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Arrange">
                <ItemTemplate>
                    <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up"
                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "AlbumId")%>' />
                    <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif"
                        CommandName="Down" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "AlbumId")%>' />
                </ItemTemplate>
            </asp:TemplateField>
    
        </Columns>
    </CC:GridView>
          <input type="hidden" runat="server" value="" id="hidPopUpAlbum" />
  
     <div style="display: none">
        <input type="button" class="btn" id="Button" value="Add Album" onclick="OpenPopUp();" />
        <input type="hidden" runat="server" value="" id="hidSaveValue" />
   </div>
   <script type="text/javascript" src="../../../includes/theme-admin/scripts/Browser.js">
    </script>

    <script type="text/javascript">
        function SetValue(save, value) {
            if (save == '1') {
                document.getElementById('<%=hidPopUpAlbum.ClientID %>').value = value;
            }
            document.getElementById('<%=hidSaveValue.ClientID %>').value = save;

        }
        function OpenPopUp() {
            var brow = GetBrowser();
            var Album = document.getElementById('<%=hidPopUpAlbum.ClientID %>').value;
            var url = '../Album/MusicSearch.aspx?Type=1&Album=' + Album
            var p = window.showModalDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
            if (brow == 'ie') {
                if (typeof p != "undefined") {
                    if (p != '') {
                        document.getElementById('<%=hidPopUpAlbum.ClientID %>').value = p;
                        var button = document.getElementById('<%=btnAddAlbum.ClientID %>');
                        if (button)
                            button.click();
                    }
                }
            }
            else {
                var saveValue = document.getElementById('<%=hidSaveValue.ClientID %>').value;
                if (saveValue == '1') {
                    var button = document.getElementById('<%=btnAddAlbum.ClientID %>');
                    if (button)
                        button.click();
                }
            }            
           

        }
    </script>
</asp:Content>