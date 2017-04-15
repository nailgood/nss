﻿<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="edit.aspx.vb" Inherits="admin_shopdesign_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">
    <h4>
        <% If IdShop = 0 Then%>Add<% Else%>Edit<% End If%>
        Shop Design</h4>
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
                    <td class="field" style="width: 500px">
                        <asp:TextBox ID="txtTitle" runat="server" MaxLength="255" Width="300px"></asp:TextBox>
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
                        <asp:TextBox ID="txtPageTitle" runat="server" MaxLength="255" Width="300px" TextMode="SingleLine"></asp:TextBox>
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
                        <asp:TextBox ID="txtMetaKeyword" runat="server" MaxLength="2000" Width="300px" TextMode="MultiLine"></asp:TextBox>
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
                        <asp:TextBox ID="txtMetaDescription" runat="server" MaxLength="2000" Width="300px"
                            TextMode="MultiLine"></asp:TextBox>
                    </td>
                    <td>
                      <asp:RequiredFieldValidator ID="refvMetaDescription" runat="server" Display="Dynamic" ControlToValidate="txtMetaDescription"
                            CssClass="msgError" ErrorMessage="* Please input Meta Description"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td valign="top" class="required">
                        <b>Category:</b>
                    </td>
                    <td class="field">
                        Please select below all category.<br>
                      <%--  <CC:CheckBoxList runat="server" ID="cblCategory" RepeatColumns="1" CellPadding="0" CellSpacing="1" />--%>
                        <asp:TreeView ID="tvCategory" runat="server" ShowCheckBoxes="All" CollapseImageToolTip="Collapse this Department" 
                        ExpandImageToolTip="Expand this Department" AutoGenerateDataBindings="false">
                            <SelectedNodeStyle Font-Names="Arial,Lucinda,Verdana,Helvetica" Font-Size="12px" ForeColor="white" />
                            <HoverNodeStyle Font-Names="Arial,Lucinda,Verdana,Helvetica" Font-Size="12px" ForeColor="black" />
                            <NodeStyle CssClass="NodeStyle" HorizontalPadding="0" />
                        </asp:TreeView>
                    </td>
                    <td>
                        <asp:Label ID="lblCategory" runat="server" CssClass="red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="optional">
                        Image File:<br />
                        <span class="smaller">150 x 150</span>
                    </td>
                    <td class="field">
                        <CC:FileUpload ID="fulImage" AutoResize="true" Folder="/assets/NewsImage" ImageDisplayFolder="/assets/NewsImage/MainThumb"
                            DisplayImage="False" runat="server" Style="width: 475px;" />
                        <div runat="server" id="divImg">
                            <b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
                            <div>
                                <asp:Image runat="server" ID="hpimg" usemap="#hpimgmap" /></div>
                        </div>
                    </td>
                    <td>
                        <CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp" ID="feImage"
                            runat="server" Display="Dynamic" ControlToValidate="fulImage" CssClass="msgError" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator>
                        <asp:Label runat="server" ID="lblImgError" Visible="false" style="color: Red; display: inline;">Must be at least 150x150</asp:Label>
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
                <tr>
                        <td class="optional">
                            Short Description:
                        </td>
                        <td class="field">
                            <asp:TextBox ID="txtShortDesc" runat="server" Height="100px" Width="400px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                            <td>
                            </td>
                    </tr>
                <asp:Panel ID="pnHtml" runat="server" Visible="true">
                    <tr>
                        <td class="optional">
                            Instruction:
                        </td>
                        <td class="field">
                            <asp:TextBox ID="txtInstruction" runat="server" Height="200px" Width="400px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                            <td>
                            </td>
                    </tr>
                    <tr>
                        <td class="optional">
                            Description:
                        </td>
                        <td class="field">
                            <asp:TextBox ID="txtDesc" runat="server" Height="200px" Width="400px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                </asp:Panel>
            </table>
            <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
            <CC:OneClickButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False">
            </CC:OneClickButton>
</asp:Content>

