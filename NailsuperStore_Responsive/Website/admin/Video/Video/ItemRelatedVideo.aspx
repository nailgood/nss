<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ItemRelatedVideo.aspx.vb" Inherits="admin_Video_Video_ItemRelatedVideo" MasterPageFile="~/includes/masterpage/admin.master" %>
    
<asp:Content ID="Content" ContentPlaceHolderID="ph" Runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<script type="text/javascript" src="../../../includes/theme-admin/scripts/Browser.js">
    </script>
<div style="margin:0 20px">
<h4><asp:Literal ID="ltrHeader" runat="server" Text="List Item"></asp:Literal></h4>
<input type="button" class="btn" id="btnSearch" value="Add Item" onclick="OpenPopUp();"   />
<CC:OneClickButton id="btnBack" runat="server" Text="Back" cssClass="btn" ValidationGroup="val1" CausesValidation="False"></CC:OneClickButton>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btnHidden"></CC:OneClickButton>
    
<asp:UpdatePanel ID="up" runat="server">
<ContentTemplate>    
    
<asp:Repeater ID="rptItem" runat="server">
    <HeaderTemplate>
        
        <table cellpadding="1" cellspacing="1" border="0" style="border:solid 1px Black;margin:5px 0">
            <tr style="height:25px">
                <th style="width:30px">#</th>
                <th>SKU</th>
                <th>Item Name</th> 
                <th style="width:50px">Active</th> 
                <th style="width:50px">Delete</th>
                <th style="width:70px">Arrange</th>                
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr style="height:25px">
            <td class="row" align="center"><%#Container.ItemIndex + 1%></td>
            <td class="row" style="padding:0 5px"><asp:Literal ID="ltrSKU" runat="server"></asp:Literal> </td> 
            <td class="row" style="padding:0 5px"><asp:Literal ID="ltrName" runat="server"></asp:Literal> </td>            
            <td class="row" align="center"><asp:CheckBox ID="chkIsActive" runat="server" Enabled="false" /> </td>  
            <td class="row" align="center"><asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif" CommandName="Delete" CommandArgument="<%#Container.DataItem.ItemId%>" OnClientClick="return ConfirmDelete();" /></td>
           
             <td class="row" align="center">
                <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up" CommandArgument="<%#Container.DataItem.ItemId%>" />
                <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif" CommandName="Down" CommandArgument="<%#Container.DataItem.ItemId%>" />
            </td>
        </tr>
    </ItemTemplate>
   <AlternatingItemTemplate>
        <tr style="height:25px">
            <td class="alternate" align="center"><%#Container.ItemIndex + 1%></td>
            <td class="alternate" style="padding:0 5px"><asp:Literal ID="ltrSKU" runat="server"></asp:Literal> </td> 
            <td class="alternate" style="padding:0 5px"><asp:Literal ID="ltrName" runat="server"></asp:Literal> </td>    
            <td class="alternate" align="center"><asp:CheckBox ID="chkIsActive" runat="server" Enabled="false" /> </td>         
            <td class="alternate" align="center"><asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif" CommandName="Delete" CommandArgument="<%#Container.DataItem.ItemId%>" OnClientClick="return ConfirmDelete();" /></td>
           
             <td class="alternate" align="center">
                <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up" CommandArgument="<%#Container.DataItem.ItemId%>" />
                <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif" CommandName="Down" CommandArgument="<%#Container.DataItem.ItemId%>" />
            </td>
        </tr>
   </AlternatingItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>

 <input type="hidden" runat="server" value="" id="hidPopUpVideoId" />
   <input type="hidden" runat="server" value="" id="hidSaveValue" />
</ContentTemplate>
</asp:UpdatePanel>

<div id="divEmpty" runat="server" visible="false" style="padding:10px"><i>List item is empty</i></div>
<CC:OneClickButton id="btnAddNew" runat="server" Text="Add Item" cssClass="btn" Visible="false"></CC:OneClickButton>

</div>
<script>
    function SetValue(save, value) {
        if (save == '1') {
            document.getElementById('<%=hidPopUpVideoId.ClientID %>').value = value;
        }
        document.getElementById('<%=hidSaveValue.ClientID %>').value = save;

    }
    function OpenPopUp() {
        var brow = GetBrowser();
        var item = document.getElementById('<%=hidPopUpVideoId.ClientID %>').value;
        var url = 'PopupItemRelatedVideo.aspx?item=' + item
        var p = window.showModalDialog(url, '', 'dialogHeight: 700px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
        //        document.getElementById('<%=hidPopUpVideoId.ClientID %>').value = p;
        //        var button = document.getElementById('<%=btnSave.ClientID %>');
        //        alert(p);
        //        if (button)
        //            button.click();
        if (brow == 'ie') {
            if (typeof p != "undefined") {
                if (p != '') {
                    document.getElementById('<%=hidPopUpVideoId.ClientID %>').value = p;
                    var button = document.getElementById('<%=btnSave.ClientID %>');
                    if (button)
                        button.click();
                }
            }
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

</script>
</asp:Content>

