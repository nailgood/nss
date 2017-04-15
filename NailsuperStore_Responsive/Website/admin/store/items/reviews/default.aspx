<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master"
    Title="Item Reviews" CodeFile="default.aspx.vb" Inherits="admin_store_items_reviews_Index" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
 <script type="text/javascript" src="../../../../includes/theme-admin/scripts/Browser.js">
    </script>
    <script type="text/javascript">

        if (window.addEventListener) {
            window.addEventListener('load', InitializeQuery, false);
        } else if (window.attachEvent) {
            window.attachEvent('onload', InitializeQuery);
        }
        function MyCallback(Id) {

            document.getElementById('<%=ItemId1.ClientID %>').value = Id;

            //GetItemEnableInfo();
        }
        function SetType() {
            InitQueryCode('ctl00$ph$LookupField', '/admin/ajax.aspx?f=DisplayItems&Type=user&q=', MyCallback);
        }
        function InitializeQuery() {

            InitQueryCode('ctl00$ph$LookupField', '/admin/ajax.aspx?f=DisplayItems&Type=user&q=', MyCallback);

        }
    </script>

    <h4>
        Item Reviews</h4>
    <span class="smaller">Please provide search criteria below</span>
    <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
        <table cellpadding="2" cellspacing="2">
            <tr>
                <th valign="top">
                    <b>Sku:</b>
                </th>
                <td valign="top" class="field">
                    <asp:DropDownList ID="F_ItemId" runat="server" Visible="false" />
                    <input type="text" id="LookupField" name="LookupField" onkeypress="javascript:SetType()"
                        onmousedown="javascript:ResetType()" autocompletetype="Disabled" autocomplete="off"
                        runat="server" style="width: 280px" />
                    <input type="hidden" name="ItemId1" id="ItemId1" runat="server" />
                </td>
            </tr>
            <tr>
                <th valign="top">
                    <b>Customer username:</b>
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_txtUserName" runat="server"></asp:TextBox>
                </td>
            </tr>
             <tr>
                <th valign="top">
                    <b>Customer no:</b>
                </th>
                <td valign="top" class="field">
                  <asp:TextBox ID="F_txtCustomerNo" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    Review Title:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_ReviewTitle" runat="server" Columns="50" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    First Name:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_FirstName" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
             <tr>
                <th valign="top">
                    Last Name:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_LastName" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    <b>Num Stars:</b>
                </th>
                <td valign="top" class="field">
                    <table border="0" cellpadding="cellspacing="0">
                        <tr>
                            <td>
                                From<asp:TextBox ID="F_NumStarsLBound" runat="server" Columns="5" MaxLength="10" />
                            </td>
                            <td>
                                To<asp:TextBox ID="F_NumStarsUBound" runat="server" Columns="5" MaxLength="10" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    <b>Date Added:</b>
                </th>
                <td valign="top" class="field">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="smaller">
                                From
                                <CC:DatePicker ID="F_DateAddedLbound" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td class="smaller">
                                To
                                <CC:DatePicker ID="F_DateAddedUbound" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    <b>Status:</b>
                </th>
                <td valign="top" class="field">
                    <asp:DropDownList ID="F_Status" runat="server">
                        <asp:ListItem Value="">-- ALL --</asp:ListItem>
                        <asp:ListItem Value="0">Un-Approved</asp:ListItem>
                        <asp:ListItem Value="2">Added Point</asp:ListItem>
                         <asp:ListItem Value="1">Actived</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    <b>Is Featured:</b>
                </th>
                <td valign="top" class="field">
                    <asp:DropDownList ID="F_IsFeatured" runat="server">
                        <asp:ListItem Value="">-- ALL --</asp:ListItem>
                        <asp:ListItem Value="1">Yes</asp:ListItem>
                        <asp:ListItem Value="0">No</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    <b>Has bought this item?:</b>
                </th>
                <td valign="top" class="field">
                    <asp:DropDownList ID="F_Bought" runat="server">
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
    </asp:Panel>
    <p>
    </p>
    <CC:OneClickButton ID="AddNew" Visible="false" runat="server" Text="Add New Item Review"
        CssClass="btn"></CC:OneClickButton>
    <p>
    </p>
    <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50"
        AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria"
        AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
        <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
        <Columns>
            <asp:TemplateField HeaderText="Reviewer">
                <ItemTemplate>
                    <asp:Literal ID="ltrReviewName" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ReviewTitle" HeaderText="Title"></asp:BoundField>
            <%--<asp:BoundField SortExpression="SKU" DataField="SKU" HeaderText="SKU"></asp:BoundField>--%>
            <asp:TemplateField HeaderText="Item" ControlStyle-CssClass="">
                <ItemTemplate>
                    <asp:Literal ID="ltrItemName" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="Order" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="">
                <ItemTemplate>
                    <asp:Literal ID="ltrOrderNo" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Stars" SortExpression="NumStars">
                <ItemTemplate>
                    <asp:Literal ID="ltrStar" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField SortExpression="DateAdded" DataField="DateAdded" HeaderText="Date"
                DataFormatString="{0:d}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right">
            </asp:BoundField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Points">
                <ItemTemplate>
                    <asp:ImageButton ID="imbPoint" runat="server" ImageUrl="/includes/theme-admin/images/plus.png"
                        CommandName="AddPoint" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ReviewId")%>' />
                        <asp:Literal ID="ltrPoint" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active">
                <ItemTemplate>
                    <asp:ImageButton ID="imbActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png"
                        CommandName="Active" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ReviewId")%>' />
                        
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Facebook">
                <ItemTemplate>
                    <asp:ImageButton ID="imbFacebook" runat="server" ImageUrl="/includes/theme-admin/images/active.png"
                        CommandName="Facebook" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ReviewId")%>' />
                    <asp:Literal ID="imgFb" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="right">
                <ItemTemplate>
                   <img src="/includes/theme-admin/images/edit.gif" style="border:none;"  onclick="Openpopup('view.aspx?ReviewId=<%#DataBinder.Eval(Container.DataItem, "ReviewId") %>');" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="right">
                <ItemTemplate>
                    <CC:ConfirmLink EnableViewState="False" Message="Are you sure that you want to remove this Item Review?"
                        runat="server" NavigateUrl='<%# "delete.aspx?ReviewId=" & DataBinder.Eval(Container.DataItem, "ReviewId") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                        ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </CC:GridView>
    <asp:HiddenField ID="hidCon" runat="server" />
      <input type="hidden" runat="server" value="" id="hidPopupReturn" />
    <script type="text/javascript">
        function SetValue(type,value) {
            document.getElementById('<%=hidPopupReturn.ClientID %>').value = value;
            if (type == 1) {
                location.reload();
            } else {
                window.location = value;
            }
        }
    </script>
</asp:Content>
