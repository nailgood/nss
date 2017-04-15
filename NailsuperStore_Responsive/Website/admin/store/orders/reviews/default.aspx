<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master"
    Title="Order Reviews" CodeFile="default.aspx.vb" Inherits="admin_store_orders_reviews_Index" %>

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

            document.getElementById('<%=OrderId1.ClientID %>').value = Id;

            //GetItemEnableInfo();
        }      
        function InitializeQuery() {

            InitQueryCode('ctl00$ph$LookupField', '/admin/ajax.aspx?f=DisplayItems&Type=user&q=', MyCallback);

        }

      
        function showPopup(url) {
  
        
            var p = window.showModalDialog(url, '', 'dialogHeight: 600px; dialogWidth: 620px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
            var brow = GetBrowser();        
            if (brow == 'ie')
            {  
                if(p)
                {
                    if(p=='0')
                        return;
                    if (p == '1') {
                        var button = document.getElementById('<%=btnSearch.ClientID %>');
                        if (button)
                            button.click();
                    }
                    else window.location = p;
                }
            }
            else 
            {
                var returnValue = document.getElementById('<%=hidPopupReturn.ClientID %>').value;               
                if(returnValue=='')
                    return;
                if(returnValue=='0') 
                    return;
                if (returnValue == '1') {
                    var button = document.getElementById('<%=btnSearch.ClientID %>');
                    if (button)
                        button.click();
                }
                else window.location = returnValue;                   
                
            }
       }
    </script>

    <h4>Order Reviews</h4>
      
    <span class="smaller">Please provide search criteria below</span>
    <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
        <table cellpadding="2" cellspacing="2">
           
            <tr>
                <th valign="top">
                    <b>First Name:</b>
                </th>
                <td valign="top" class="field">
                 <input type="hidden" name="OrderId1" id="OrderId1" runat="server" />
                    <asp:TextBox ID="F_txtFirstName" runat="server"></asp:TextBox>
                </td>
            </tr>
             <tr>
                <th valign="top">
                    <b>Customer No:</b>
                </th>
                <td valign="top" class="field">
                  <asp:TextBox ID="F_txtCustomerNo" runat="server"></asp:TextBox>
                </td>
            </tr>
             <tr>
                <th valign="top">
                    <b>Order No:</b>
                </th>
                <td valign="top" class="field">
                  <asp:TextBox ID="F_txtOrderNo" runat="server"></asp:TextBox>
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
                <th valign="top">
                    <b>Is Post:</b>
                </th>
                <td valign="top" class="field">
                    <asp:DropDownList ID="F_IsShared" runat="server">
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
   
    <p>
    </p>
    <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50"
        AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria"
        AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
        <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
        <Columns>
           
            <%--<asp:BoundField SortExpression="SKU" DataField="SKU" HeaderText="SKU"></asp:BoundField>--%>
            <asp:TemplateField HeaderText="Order No" ControlStyle-CssClass="">
                <ItemTemplate>
                    <asp:Literal ID="ltrOrderNo" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Stars">
                <ItemTemplate>
                    <asp:Literal ID="ltrStar" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField SortExpression="DateAdded" DataField="DateAdded" HeaderText="Date"
                DataFormatString="{0:d}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right">
            </asp:BoundField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active" HeaderStyle-Width="40px">
                <ItemTemplate>
                    <asp:ImageButton ID="imbActive" runat="server" ImageUrl="/includes/theme-admin/images/active.png"
                        CommandName="Active" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OrderId")%>' />
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Post" HeaderStyle-Width="40px" >
                <ItemTemplate>
                    <asp:ImageButton ID="imbShared" runat="server" ImageUrl="/includes/theme-admin/images/active.png"
                        CommandName="Shared" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OrderId")%>' />
                    <asp:Literal ID="imgSh" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Facebook">
                <ItemTemplate>
                    <asp:ImageButton ID="imbFacebook" runat="server" ImageUrl="/includes/theme-admin/images/active.png"
                        CommandName="Facebook" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OrderId")%>' />
                    <asp:Literal ID="imgFb" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
           
            <asp:TemplateField ItemStyle-HorizontalAlign="right">
                <ItemTemplate>
                     <img src="/includes/theme-admin/images/edit.gif" style="border:none;" onclick="Openpopup('view.aspx?OrderId=<%#DataBinder.Eval(Container.DataItem, "OrderId") %>');" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="right">
                <ItemTemplate>
                    <CC:ConfirmLink EnableViewState="False" Message="Are you sure that you want to remove this Order Review?"
                        runat="server" NavigateUrl='<%# "delete.aspx?OrderId=" & DataBinder.Eval(Container.DataItem, "OrderId") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                        ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </CC:GridView>
    <asp:HiddenField ID="hidCon" runat="server" />
    <input type="hidden" runat="server" value="" id="hidPopupReturn" />
    <script type="text/javascript">
        function SetValue(type, value) {
            document.getElementById('<%=hidPopupReturn.ClientID %>').value = value;
            if (type == 1) {
                location.reload();
            } else {
                window.location = value;
            }
        }
    </script>
</asp:Content>
