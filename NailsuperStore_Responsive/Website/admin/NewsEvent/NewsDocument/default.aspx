<%@ Page Language="VB" AutoEventWireup="false" CodeFile="default.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_news"  Title="News" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>


<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
<script type="text/javascript">
    function ConfirmDelete() {
        if (!confirm('Are you sure to delete?')) {
            return false;
        }
    }

</script>

<div style="margin:0 20px">
<h4><asp:Literal ID="ltrHeader" runat="server" Text="List News Document"></asp:Literal></h4>

<asp:Panel ID="pnList" runat="server">
<div style="padding-bottom:10px">   
<input type="button" class="btn" id="btnSearch" value="Add Document" onclick="OpenPopUp();" />
      &nbsp;  <CC:OneClickButton id="btnBack" runat="server" Text="Back" cssClass="btn" ValidationGroup="val1" CausesValidation="False"></CC:OneClickButton>
                <CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btnHidden"></CC:OneClickButton>
        <asp:Label ID="ltrMsg" runat="server" CssClass="red"></asp:Label>
</div>    

<CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50"
        AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria"
        AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
        <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
        <Columns>
       <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="STT">
           <ItemTemplate>
                <asp:Literal ID="ltrIndex" runat="server"></asp:Literal>
           </ItemTemplate>
       </asp:TemplateField>                                 
             <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="News Title">
                <ItemTemplate>
                    <asp:Literal ID="ltNewsTitle" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Document">
                <ItemTemplate>
                    <asp:Literal ID="ltDocument" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active">
                <ItemTemplate>
                    <asp:ImageButton ID="imbActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png"
                        CommandName="Active" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete">
                <ItemTemplate>
                    <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif"
                        CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>'
                        OnClientClick="return ConfirmDelete();" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Arrange">
                <ItemTemplate>
                    <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up"
                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                    <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif"
                        CommandName="Down" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </CC:GridView>

</asp:Panel>
</div>
 <input type="hidden" runat="server" value="" id="hidPopUpDoc" />
     <input type="hidden" runat="server" value="" id="hidSaveValue" />
     <script type="text/javascript" src="../../../includes/theme-admin/scripts/Browser.js"></script>
<script type="text/javascript">
    function SetValue(save, id) {
        if (save == '1') {
            document.getElementById('<%=hidPopUpDoc.ClientID %>').value = id;
        }
        document.getElementById('<%=hidSaveValue.ClientID %>').value = save;
    }
    function OpenPopUp() {
        var DocId = document.getElementById('<%=hidPopUpDoc.ClientID %>').value;
        var url = 'AddDocument.aspx?DocId=' + DocId
        var p = window.showModalDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
        var brow = GetBrowser();
        var button = document.getElementById('<%=btnSave.ClientID %>');
        if (brow == 'ie') {
            document.getElementById('<%=hidPopUpDoc.ClientID %>').value = p;
            if (button)
                button.click();
        }
        else {
            var saveValue = document.getElementById('<%=hidSaveValue.ClientID %>').value;
            if (saveValue == '1') {
                if (button)
                    button.click();
            }
        }
    }
</script>
</asp:content>

