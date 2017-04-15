<%@ Page Language="VB" AutoEventWireup="false" Debug="true" CodeFile="default.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_NewsEvent_Blog_default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <script type="text/javascript">
        function ConfirmDelete() {
            if (!confirm('Are you sure to delete?')) {
                return false;
            }
        }
    </script>
    <script type="text/javascript" src="/includes/scripts/tinymce/tinymce.min.js"></script>
    <script type="text/javascript">

        //path vao filemanager chua dialog.aspx
        tfm_path = '/includes/scripts/tinymce/plugins/tinyfilemanager.net/tinyfilemanager.net';
        tinymce.init({
            //content_css: "/includes/tinymce_4/css/content.css",
            selector: "#<%=txtContent.ClientID %>",
            theme: "modern",
            menubar: true, //on/off menu editor
            plugins: [
                        "advlist autolink lists link image charmap print preview hr anchor pagebreak",
                        "searchreplace wordcount visualblocks visualchars code fullscreen",
                        "insertdatetime media nonbreaking save table contextmenu directionality",
                        "emoticons template paste textcolor spellchecker insertdatetime tinyfilemanager.net"
                    ],
            // Theme options
            toolbar1: "newdocument,|,bold,italic,underline,strikethrough,|,visualchars,nonbreaking,template,pagebreak,",
            toolbar2: "styleselect,formatselect,fontselect,fontsizeselect",
            toolbar3: "cut,copy,paste,pastetext,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,|,insertdatetime,|,forecolor,backcolor,|,hr,removeformat,|,spellchecker",
            toolbar4: "charmap,emoticons,media,image,tinyfilemanager.net,|,preview,print,|,ltr,rtl,|,code,fullscreen",
            image_advtab: true,
            //mac dinh da remove cac the dac biet tu worđ, khai bao de remove them mot so the khac khi copy tu word sang
            paste_word_valid_elements: "b,strong,i,em,h1,h2"
        });
        
    </script>

    <div style="margin: 0 20px">
        <h4>
            <asp:Literal ID="ltrHeader" runat="server" Text="List Blog"></asp:Literal></h4>
        <asp:Panel ID="pnNew" runat="server" Visible="false">
            <table border="0" cellspacing="1" cellpadding="2">
                <tr>
                    <td colspan="2">
                        <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
                    </td>
                </tr>
                <tr>
                    <td class="required">
                        Title:
                    </td>
                    <td class="field" style="width: 710px">
                        <asp:TextBox ID="txtTitle" runat="server" MaxLength="255" Width="700px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" Display="Dynamic" ControlToValidate="txtTitle"
                            CssClass="msgError" ErrorMessage="* Please input Title"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="required">
                        Page Title:
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtPageTitle" runat="server" MaxLength="255" Width="700px" TextMode="SingleLine"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvPageTitle" runat="server" Display="Dynamic" ControlToValidate="txtPageTitle"
                            CssClass="msgError" ErrorMessage="* Please input Page Title"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="required">
                        Meta Keyword:
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtMetaKeyword" runat="server" MaxLength="2000" Width="700px" TextMode="MultiLine"></asp:TextBox><%= Resources.Admin.lenPageTitle%>
                    </td>
                    <td>
                    <asp:RequiredFieldValidator ID="refvMetaKeyword" runat="server" Display="Dynamic" ControlToValidate="txtMetaKeyword"
                            CssClass="msgError" ErrorMessage="* Please input Meta Keyword"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="required">
                        Meta Description:
                    </td>
                    <td class="field">
                        <asp:TextBox ID="txtMetaDescription" runat="server" MaxLength="2000" Width="700px" Height="120"
                            TextMode="MultiLine"></asp:TextBox><%= Resources.Admin.lenMetaDesc%>
                    </td>
                    <td>
                      <asp:RequiredFieldValidator ID="refvMetaDescription" runat="server" Display="Dynamic" ControlToValidate="txtMetaDescription"
                            CssClass="msgError" ErrorMessage="* Please input Meta Description"></asp:RequiredFieldValidator>
                    </td>
                </tr>    
                <tr>
                    <td class="optional">
                        Image File:<br />
                        <span class="smaller">770 x 433</span>
                    </td>
                    <td class="field">
                        <CC:FileUpload ID="fuImage" AutoResize="true" Folder="/assets/blog/" ImageDisplayFolder="/assets/blog/thumb/"
                            DisplayImage="False" runat="server" Style="width: 475px;" />
                        <div runat="server" id="divImg">
                            <b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
                            <div>
                                <asp:Image runat="server" ID="hpimg" usemap="#hpimgmap" /></div>
                        </div>
                    </td>
                    <td>
                        <CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp" ID="feImage"
                            runat="server" Display="Dynamic" ControlToValidate="fuImage" CssClass="msgError" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
                        <asp:Label runat="server" ID="lblImgError" Visible="false" style="color: Red; display: inline;">Must be at least 770x433</asp:Label>
                    </td>
                </tr>           
                <tr>
                    <td class="optional">
                        <b>Is Active?</b>
                    </td>
                    <td class="field">
                        <asp:CheckBox runat="server" ID="chkIsActive" Checked="true" />
                    </td>
                </tr>                
                <asp:Panel ID="pnHtml" runat="server" Visible="true">
                    <tr>
                        <td class="optional">
                            Short Content:
                        </td>
                        <td class="field">
                            <asp:TextBox ID="txtShortContent" runat="server" Height="100px" Width="700px" TextMode="MultiLine"></asp:TextBox>
                            <td>
                            </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Content:
                        </td>
                        <td class="field">
                            <asp:TextBox ID="txtContent" runat="server" Height="400px" Width="700px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                </asp:Panel>
            </table>
            <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
            <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
            </CC:OneClickButton>
        </asp:Panel>
        <asp:Panel ID="pnList" runat="server">
            <table cellpadding="2" cellspacing="2">
                <tr>
                    <th valign="top">
                        Title:
                    </th>
                    <td valign="top" class="field">
                        <asp:TextBox ID="F_Title" runat="server" Columns="50" MaxLength="256"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th valign="top">
                        <b>Is Active:</b>
                    </th>
                    <td valign="top" class="field">
                        <asp:DropDownList ID="F_IsActive" runat="server">
                            <asp:ListItem Value="">-- ALL --</asp:ListItem>
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="0">No</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>            
                <tr>
                    <td colspan="2" align="right">
                        <CC:OneClickButton ID="btnSearch" runat="server" Text="Search" CssClass="btn" />
                        <input class="btn" type="submit" value="Clear" onclick="window.location='default.aspx';return false;" />
                    </td>
                </tr>
            </table>
            <p>
                <CC:OneClickButton ID="btnAddNew" runat="server" Text="Add new" CssClass="btn"></CC:OneClickButton></p>
            <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50"
                AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria"
                AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
                <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
                <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
                <Columns>
                    <asp:BoundField DataField="Title" HeaderStyle-Width="250" HeaderText="Title"></asp:BoundField>
                    <%--<asp:CheckBoxField ItemStyle-HorizontalAlign="Center" DataField="IsActive" HeaderText="Is Active" />--%>                  
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active">
                        <ItemTemplate>
                            <asp:ImageButton ID="imbActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png"
                                CommandName="Active" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "NewsId")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Facebook">
                        <ItemTemplate>
                            <asp:ImageButton ID="imbFacebook" runat="server" ImageUrl="/includes/theme-admin/images/active.png"
                                CommandName="Facebook" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "NewsId")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Edit">
                        <ItemTemplate>
                            <asp:ImageButton ID="imbEdit" runat="server" ImageUrl="/includes/theme-admin/images/edit.gif" CommandName="Edit"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "NewsId")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete">
                        <ItemTemplate>
                            <asp:ImageButton ID="imbDelete" runat="server" ImageUrl="/includes/theme-admin/images/delete.gif"
                                CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "NewsId")%>'
                                OnClientClick="return ConfirmDelete();" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Arrange">
                        <ItemTemplate>
                            <asp:ImageButton ID="imbUp" runat="server" ImageUrl="/includes/theme-admin/images/MoveUp.gif" CommandName="Up"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "NewsId")%>' />
                            <asp:ImageButton ID="imbDown" runat="server" ImageUrl="/includes/theme-admin/images/MoveDown.gif"
                                CommandName="Down" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "NewsId")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </CC:GridView>
            <asp:HiddenField ID="hidCon" runat="server" />
        </asp:Panel>
    </div>
    <script src="/includes/theme-admin/scripts/checkcharacters.js" type="text/javascript"></script>
</asp:Content>
