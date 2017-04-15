<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master"
    AutoEventWireup="false" CodeFile="edit.aspx.vb" Inherits="admin_Keyword_Synonym_Edit" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <script type="text/javascript">
	<!--
        function MyCallback() {
           
            if (document.getElementById('varKeyword')) {
                $('#varKeyword').hide();
            }
           // return;
            var keyword = document.getElementById('<%=txtMainKeyword.ClientID %>').value;
            if (keyword == '') {
                document.getElementById('<%=txtOneWay.ClientID %>').value = '';
                document.getElementById('<%=txtRoundTrip.ClientID %>').value = '';
            }
            else {
                //get from DB
                GetKeywordSynonymData(keyword);
            }
        }
        function BindKeywordSynonymData(keywordId) {
            var currentURl = window.location.href;
            var currentId = document.getElementById('<%=hidKeywordId.ClientID %>').value;
            currentId = parseInt(currentId);
            if (currentId > 0) {
                currentURl = currentURl.replace('KeywordId=' + currentId, 'KeywordId=' + keywordId);
            }
            else {
                var n = currentURl.indexOf("?");
                if (n > 1) {
                    currentURl = currentURl + '&KeywordId=' + keywordId;
                }
                else {
                    currentURl = currentURl + '?KeywordId=' + keywordId;
                }
            }
          //  alert(currentURl);
            window.location.href=currentURl;
        }
        if (window.addEventListener) {
            <%=IIf(SynonymType <> "buyinbulk", "window.addEventListener('load', InitializeQuery, false);", "")%>
            
        } else if (window.attachEvent) {
            window.attachEvent('onload', InitializeQuery);
        }

        function InitializeQuery() {
            var s = document.getElementById('<%=hidSearch.ClientID %>').value;
            InitQueryCode2('<%=txtMainKeyword.ClientID.Replace("_", "$") %>', '/admin/ajax.aspx?f=DisplayKeyword&q=', 'varKeyword', s);
        }
        function Delete(keywordId, KeywordSynonymId) {

            var yes = confirm('Are you sure?');
            if (yes) {
                window.location.href = 'delete.aspx?KeywordId=' + keywordId + '&KeywordSynonymId=' + KeywordSynonymId;
            }
            else {
                return false
            }
        }

        function ChangeSearchRoundTrip(keywordId) {
            if (document.getElementById('hidKeywordSynonymId')) {
                document.getElementById('hidKeywordSynonymId').value = keywordId;
                if (document.getElementById('lbtnChangeRoundTrip')) {
                    document.getElementById('lbtnChangeRoundTrip').click();
                }
            }

        }
        
       
	//-->
    </script>
    <h4>
        <%=IIf(String.IsNullOrEmpty(synonymType), "Add Keyword Synonym", "Add Tone Synonym") %>
    </h4>
    <br>
    <input type="hidden" id="hidSearch" runat="server" name="hidSearch" value="" />
    <a href="default.aspx?<%=params%>">«« Go Back To Keyword List</a>
    <p>
        <table cellspacing="2" cellpadding="3" border="0">
            <tbody>
                <tr>
                    <td class="required" valign="top">
                        Main keyword
                    </td>
                    <td class="field" width="301">
                        <asp:DropDownList runat="server" ID="ddlTone"></asp:DropDownList>
                        <asp:TextBox ID="txtMainKeyword" runat="server" Style="border-right: #999999 1px solid;
                            padding-right: 4px; border-top: #999999 1px solid; padding-left: 4px; border-left: #999999 1px solid;
                            width: 300px; border-bottom: #999999 1px solid"></asp:TextBox>
                        <asp:HiddenField ID="hidKeywordId" ClientIDMode="Static" runat="server" Value="" />
                          <input type="hidden" id="hidReset" value="0" runat="server" clientidmode="Static" />
                        <asp:HiddenField ID="hidF_KeywordName" ClientIDMode="Static" runat="server" Value="" />
                        <asp:HiddenField ID="hidKeywordSynonymId" ClientIDMode="Static" runat="server" Value="" />
                    </td>
                    <td valign="top">
                        <asp:RequiredFieldValidator ID="rfvMainKeywordDdl" runat="server" Display="Dynamic"
                            ControlToValidate="ddlTone" CssClass="msgError" ErrorMessage="Field 'Main keyword' is blank"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvMainKeyword" runat="server" Display="Dynamic"
                            ControlToValidate="txtMainKeyword" CssClass="msgError" ErrorMessage="Field 'Main keyword' is blank"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="optional" valign="top">
                        One way keyword
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtOneWay" runat="server" Width="302px" Height="100px" Value=""
                            TextMode="MultiLine">
                        </asp:TextBox>
                    </td>
                    <td valign="top">
                    </td>
                </tr>
                <tr>
                    <td class="optional" valign="top">
                        Round Trip keyword
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtRoundTrip" runat="server" Width="301px" Height="100px" Value=""
                            TextMode="MultiLine">
                        </asp:TextBox>
                    </td>
                    <td valign="top">
                    </td>
                </tr>
                <tr>
                    <td class="optional" valign="top">
                    </td>
                    <td class="field">
                  
                        <asp:Button ID="btnSave" CssClass="btn" runat="server" Text="Save" />
                    </td>
                    <td valign="top">
                    </td>
                </tr>
            </tbody>
        </table>
        <p>
</asp:Content>
