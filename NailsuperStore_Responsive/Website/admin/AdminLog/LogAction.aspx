<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false"
    CodeFile="LogAction.aspx.vb" Inherits="admin_AdminLog_LogAction" %>

<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
   <h4>
        Admin Log</h4>
    <span class="smaller">Please provide search criteria below</span>
    <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
        <table cellpadding="2" cellspacing="2">
            <tr>
                <th valign="middle" Width="118px" align="left">
                    User Name:
                </th>
                <td valign="top" class="field">
                    <asp:DropDownList ID="drpUsername" runat="server" Width="118px"  AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th valign="middle" align="left"> 
                    Date:
                </th>
                <td valign="top" class="field">
                    <table>
                        <tr>
                            <td>
                                <CC:DatePicker ID="F_FromDate" runat="server" />
                            </td>
                            <td>
                                ~
                            </td>
                            <td>
                                <CC:DatePicker ID="F_ToDate" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <th valign="middle" align="left">
                    Action:
                </th>
                <td valign="top" class="field">
                <asp:DropDownList ID="drpAction" runat="server" AutoPostBack="false">
                        <asp:ListItem Text="Insert" Value="Insert"></asp:ListItem>
                        <asp:ListItem Text="Update" Value="Update"></asp:ListItem>
                        <asp:ListItem Text="Delete" Value="Delete"></asp:ListItem>
                </asp:DropDownList>
                    
                </td>
            </tr>
            <tr>
                <th valign="middle" align="left">
                    Type:
                </th>
                <td valign="top" class="field">
                    <asp:DropDownList ID="drlObject" onChange="changeObject(this.value)" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
             <tr id="trSKU" style="display:none;"  runat="server">
                <th valign="middle" align="left">
                    Item SKU:
                </th>
                <td valign="top" class="field">
                  <asp:TextBox ID="txtSKU" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr id="trOrderNo" style="display:none;"  runat="server">
                <th valign="middle" align="left">
                    Order No:
                </th>
                <td valign="top" class="field">
                  <asp:TextBox ID="txtOrderNo" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr id="trTransID" style="display:none;"  runat="server">
                <th valign="middle" align="left">
                    Trans ID:
                </th>
                <td valign="top" class="field">
                  <asp:TextBox ID="txtTransID" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <CC:OneClickButton ID="btnSearch" runat="server" Text="Search" CssClass="btn" />
                    <input class="btn" type="submit" value="Clear" onclick="window.location='LogAction.aspx';return false;" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <p>
    </p>
    <p>
    </p>
    <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="30"  RowStyle-Height="25px"  
        AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria"
        AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
        
        <RowStyle CssClass="rowLog" VerticalAlign="Top"></RowStyle>
        <Columns>
<%--            <asp:BoundField DataField="Username" HeaderStyle-Width="70" HeaderText="Username" SortExpression="UserName">
            </asp:BoundField>--%>
             <asp:TemplateField HeaderText="Username" HeaderStyle-Width="70" SortExpression="UserName">
                <ItemTemplate>
                    <asp:Literal ID="ltrUsername" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Action" HeaderStyle-Width="50">
                <ItemTemplate>
                    <asp:Literal ID="ltrAction" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Type" HeaderStyle-Width="100">
                <ItemTemplate>
                    <asp:Literal ID="ltrObjectType" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
           
            <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Name" HeaderStyle-Width="190">
                <ItemTemplate>
                    <asp:Literal ID="ltrObjectName" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Detail">
                <ItemTemplate>
                    <asp:Literal ID="ltrActionDetail" runat="server" Visible="true"></asp:Literal>
                    <table cellspacing="0" cellpadding="0" class="tblLog" style="display:none;" >
                        <tbody>
                            <tr>
                                  <td  width="180px" class="field">
                                    Short Vietnamese Description:
                                </td>
                                <td width="200px">
                                    Ty Nuyen
                                </td>
                                <td  width="200px" class="lastRight">
                                    TyVan
                                </td>
                            </tr>
                            <tr>
                                 <td  class="lastBottom field">
                                   User name:
                                </td>
                                <td  width="200px" class="lastBottom">
                                    tynv
                                </td>
                                <td width="200px" class="lastBottom lastRight">
                                    nguyenty
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CreatedDate" HeaderText="Date" HeaderStyle-Width="70" SortExpression="CreatedDate" 
                HtmlEncode="False"></asp:BoundField>
            <asp:BoundField DataField="IP" HeaderStyle-Width="70" HeaderText="IP" SortExpression="IP">
            </asp:BoundField>
            <asp:BoundField DataField="Browser" HeaderStyle-Width="70" HeaderText="Browser" >
            </asp:BoundField>
        </Columns>
    </CC:GridView>
    <script type="text/javascript">
        function changeObject(value) {
            if (value == 'StoreItem') {
                if (document.getElementById('<%=trSKU.ClientID %>')) {
                    document.getElementById('<%=trSKU.ClientID %>').style.display = '';
                    document.getElementById('<%=trTransID.ClientID %>').style.display = 'none';
                }
            }
            else if (value == 'CashPoint')
            {
                if (document.getElementById('<%=trTransID.ClientID %>')) {
                    document.getElementById('<%=trTransID.ClientID %>').style.display = '';
                    document.getElementById('<%=trSKU.ClientID %>').style.display = 'none';
                    }
            }
            else if (value == 'TrackingNumber')
            {
               
                if (document.getElementById('<%=trOrderNo.ClientID %>')) {
                    document.getElementById('<%=trOrderNo.ClientID %>').style.display = '';
                    document.getElementById('<%=trSKU.ClientID %>').style.display = 'none';
                }
            }
            else {
                if (document.getElementById('<%=trSKU.ClientID %>')) {
                    document.getElementById('<%=trSKU.ClientID %>').style.display = 'none';
                }
                if (document.getElementById('<%=trTransID.ClientID %>')) {
                    document.getElementById('<%=trTransID.ClientID %>').style.display = 'none';
                }
                 if (document.getElementById('<%=trOrderNo.ClientID %>')) {
                    document.getElementById('<%=trOrderNo.ClientID %>').style.display = 'none';
                }
            }
        }
    </script>
</asp:Content>
