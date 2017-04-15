<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" Async="true"
    AutoEventWireup="false" CodeFile="video.aspx.vb" Inherits="admin_store_items_video" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <h4>
        Coordinating items for '<asp:Label ID="lblItemName" runat="server" />'</h4>
    <br>
    <a href="edit.aspx?<%=params%>">«« Go Back To Item </a>
    <p>
        <b>Add Coordinating Item</b>
        <asp:Panel ID="pnlSearch" DefaultButton="btnAdd" runat="server">
            <table cellpadding="2" cellspacing="2">
                <tr>
                    <td colspan="2">
                        <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <%--<span class="red">Errore Insert</span> --%>
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="required">
                        Url:
                    </td>
                    <td class="field" style="width: 120px;">
                        <asp:TextBox ID="txtUrl" runat="server" MaxLength="500" Width="520px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvTitle" CssClass="msgError" runat="server" Display="Dynamic" ControlToValidate="txtUrl"
                            ErrorMessage="Field 'Url' is blank"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="optional">
                        Thumb Image:<br />
                        <span class="smaller">220 x 130</span>
                    </td>
                    <td class="field">
                        <CC:FileUpload ID="fuThumb" AutoResize="false" EnableDelete="false" DisplayImage="False" runat="server"
                            Style="width: 200px;" />
                        <div>
                        </div>
                        <i><span class="smallest">Image available:
                            <asp:Label ID="lbUploadfile" runat="server" Text=""></asp:Label></span></i></div>
                        <div runat="server" id="divImg">
                            <b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
                            <div>
                                <asp:Image runat="server" ID="hpimg" usemap="#hpimgmap" /></div>
                        </div>
                    </td>
                    <td>
                        <CC:FileUploadExtensionValidator Extensions="jpg,gif" ID="feImage" runat="server"
                            Display="Dynamic" ControlToValidate="fuImage" CssClass="msgError" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="optional">
                        <b>Is Active?</b>
                    </td>
                    <td valign="top" class="field">
                        <asp:CheckBox ID="chkIsActive" runat="server" Checked="true" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="optional">
                        Description:
                    </td>
                    <td valign="top" class="field">
                        <asp:TextBox ID="txtDescription" runat="server" MaxLength="4000" Width="520px" Height="120px"
                            TextMode="MultiLine"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <CC:OneClickButton ID="btnAdd" runat="server" Text="Save" CssClass="btn" />
                        <CC:OneClickButton ID="btnClear" runat="server" Text="Clear" CausesValidation="false"
                            CssClass="btn"  />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <p>
            <asp:PlaceHolder ID="plcHasRecords" runat="server" Visible="true">
                <table cellpadding="0" cellspacing="0" border="0" id="tblList" runat="server">
                    <tr>
                        <td>
                            <asp:Repeater ID="rptList" runat="server">
                                <HeaderTemplate>
                                    <table cellpadding="1" cellspacing="1" border="0" style="border: solid 1px Black;
                                        margin: 10px 0">
                                        <tr style="height: 25px">
                                            <th style="width: 300px">
                                                Description
                                            </th>
                                              <th style="width: 200px">
                                                Image
                                            </th>
                                            <th style="width: 50px">
                                                Review
                                            </th>
                                            <th style="width: 50px">
                                                Active
                                            </th>
                                            <th style="width: 50px">
                                                Edit
                                            </th>
                                            <th style="width: 50px">
                                                Delete
                                            </th>
                                            <th style="width: 50px">
                                                Arrange
                                            </th>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr style="height: 25px">
                                        <td class="row">
                                            <%#Container.DataItem.Description%>
                                        </td>
                                         <td class="row">
                                            <asp:Literal ID="ltrImg" runat="server">
                                            </asp:Literal>
                                         </td>
                                        <td class="row" align="center">
                                            <a title="<%#Container.DataItem.Description%>" rel="prettyPhoto" href="<%#Container.DataItem.Url%>">
                                                <img alt="<%#Container.DataItem.Description%>" rel="prettyPhoto" style="border: none;"
                                                    src="/includes/theme-admin/images/preview.gif"></a>
                                        </td>
                                        <td class="row" align="center">
                                            <asp:ImageButton ID="imbActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png"
                                                CausesValidation="false" CommandName="Active" CommandArgument="<%#Container.DataItem.ItemVideoId%>" />
                                        </td>
                                        <td class="row" align="center">
                                            <asp:ImageButton ID="imbEdit" runat="server" ImageUrl="/includes/theme-admin/images/edit.gif" CommandName="Edit"
                                                CausesValidation="false" CommandArgument="<%#Container.DataItem.ItemVideoId%>" />
                                        </td>
                                        <td class="row" align="center">
                                            <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif"
                                                CausesValidation="false" CommandName="Delete" CommandArgument="<%#Container.DataItem.ItemVideoId%>"
                                                OnClientClick="return ConfirmDelete();" />
                                        </td>
                                        <td class="row" align="center">
                                            <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up"
                                                CausesValidation="false" CommandArgument="<%#Container.DataItem.ItemVideoId%>" />
                                            <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif"
                                                CausesValidation="false" CommandName="Down" CommandArgument="<%#Container.DataItem.ItemVideoId%>" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr style="height: 25px">
                                        <td class="alternate">
                                            <%#Container.DataItem.Description%>
                                        </td>
                                         <td class="row">
                                            <asp:Literal ID="ltrImg" runat="server">
                                            </asp:Literal>
                                         </td>
                                        <td class="alternate" align="center">
                                            <a title="<%#Container.DataItem.Description%>" rel="prettyPhoto" href="<%#Container.DataItem.Url%>">
                                                <img alt="<%#Container.DataItem.Description%>" rel="prettyPhoto" style="border: none;"
                                                    src="/includes/theme-admin/images/preview.gif"></a>
                                        </td>
                                        <td class="alternate" align="center">
                                            <asp:ImageButton ID="imbActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png"
                                                CausesValidation="false" CommandName="Active" CommandArgument="<%#Container.DataItem.ItemVideoId%>" />
                                        </td>
                                        <td class="alternate" align="center">
                                            <asp:ImageButton ID="imbEdit" runat="server" ImageUrl="/includes/theme-admin/images/edit.gif" CommandName="Edit"
                                                CausesValidation="false" CommandArgument="<%#Container.DataItem.ItemVideoId%>" />
                                        </td>
                                        <td class="alternate" align="center">
                                            <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif"
                                                CausesValidation="false" CommandName="Delete" CommandArgument="<%#Container.DataItem.ItemVideoId%>"
                                                OnClientClick="return ConfirmDelete();" />
                                        </td>
                                        <td class="alternate" align="center">
                                            <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up"
                                                CausesValidation="false" CommandArgument="<%#Container.DataItem.ItemVideoId%>" />
                                            <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif"
                                                CausesValidation="false" CommandName="Down" CommandArgument="<%#Container.DataItem.ItemVideoId%>" />
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                </table>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="plcNoRecords" runat="server" Visible="false">
                <p>
                There are currently no coordinating items for this item.</asp:PlaceHolder>

            <script type="text/javascript">
                function ClearForm() {
                    document.getElementById('<%=txtUrl.ClientID %>').value = '';
                    document.getElementById('<%=chkIsActive.ClientID %>').checked = false;
                    document.getElementById('<%=txtDescription.ClientID %>').value = '';
                    return false;
                }
            </script>

            <script type="text/javascript" charset="utf-8">
                $(document).ready(function() {
                    $("a[rel^='prettyPhoto']").prettyPhoto();
                });
                
            </script>
</asp:Content>
