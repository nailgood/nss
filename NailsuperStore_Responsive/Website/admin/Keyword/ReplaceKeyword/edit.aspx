<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master"
    AutoEventWireup="false" CodeFile="edit.aspx.vb" Inherits="admin_Keyword_ReplaceKeyword_edit" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <script type="text/javascript">
	<!--
        function MyCallback() {
            if (document.getElementById('divSearch')) {
                $('#divSearch').hide();
            }
        }

        if (window.addEventListener) {
            window.addEventListener('load', InitializeQuery, false);
        } else if (window.attachEvent) {
            window.attachEvent('onload', InitializeQuery);
        }

        function InitializeQuery() {
           
            var keywordId = '';
            if (document.getElementById('hidKeywordId')) {
                keywordId = document.getElementById('hidKeywordId').value;
            }
           
            InitQueryCode('LookupField', '/admin/ajax.aspx?keywordid=' + keywordId + '&f=DisplayKeywordSynonym&q=', 'divSearch');
            
        }

        function ResetKeyword(keyword) {
         
            $('#hidReset').val('1');
            $('#LookupField').val(keyword);
        }
       
	//-->
    </script>
    <h4>
        Coordinating keyword for '<asp:Label ID="lblItemName" runat="server" />'</h4>
        <input type="hidden" id="hidReset" value="0" />
    <br>
    <a href="default.aspx?<%=params%>">«« Go Back To Keyword List</a>
    <p>
        <b>Add Coordinating Keyword</b>
        <table cellspacing="2" cellpadding="3" border="0">
            <tbody>
                <tr>
                    <td class="optional" valign="top">
                        <b>Keyword search</b>
                    </td>
                    <td class="field" width="400">
                        Please enter the first few characters of the item name that belongs to the family/collection
                        below<br>
                        <br />
                        <input type="text" id="LookupField" name="LookupField" style="border-right: #999999 1px solid;
                            padding-right: 4px; border-top: #999999 1px solid; padding-left: 4px; border-left: #999999 1px solid;
                            width: 360px; border-bottom: #999999 1px solid">
                        <br />
                        <br>
                        <br />
                        Please click the "Add Coordinating Keyword" button below to add the keyword selected
                        as a coordinating keyword"
                        <p>
                            <asp:Button ID="btnAdd" runat="server" Text="Add Coordinating Keyword" CssClass="btn">
                            </asp:Button></p>
                        <asp:HiddenField ID="hidKeywordId" ClientIDMode="Static" runat="server" Value="" />
                    </td>
                    <td valign="top">
                    </td>
                </tr>
            </tbody>
        </table>
</asp:Content>
