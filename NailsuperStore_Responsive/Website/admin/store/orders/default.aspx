<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master"
    Title="Store Order" CodeFile="default.aspx.vb" Inherits="admin_store_orders_Index" %>

<%@ Register Src="~/controls/layout/pager.ascx" TagName="pager" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ph" ID="Content" runat="server">
    <script src="/includes/scripts/qtip/jquery.qtip.min.js" type="text/javascript"></script>
    <h4>
        Store Order</h4>
    <span class="smaller">Please provide search criteria below</span>
    <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
        <table cellpadding="2" cellspacing="2">
            <tr>
                <th align="right" valign="top">
                    Order No:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_OrderNo" runat="server" Columns="20" MaxLength="50"></asp:TextBox>
                    <span style="font: italic 11px Arial; color: #444;">Enter multi order no with commas</span>
                </td>
                <th align="right" valign="top">
                    Ship To Country/State:
                </th>
                <td valign="top" class="field">
                    <div style="float: left">
                        <asp:DropDownList ID="F_ShipToCountry" onchange="changeSelect('Sstate')" runat="server" />
                    </div>
                    <div id="dSstate" style="display: none; float: left; text-align: center;">
                        <asp:DropDownList ID="F_ShipToCounty" runat="server" />
                    </div>
                </td>
            </tr>
            <tr>
                <th align="right" valign="top">
                    Customer No:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_CustomerNo" runat="server" Columns="20" MaxLength="50"></asp:TextBox>
                    <span style="font: italic 11px Arial; color: #444;">Enter multi customer no with commas</span>
                </td>
                <th align="right" valign="top">
                    Ship To Salon Name:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_ShipToSalonName" runat="server" Columns="50" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right" valign="top">
                    Email:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_Email" runat="server" Columns="50" MaxLength="100"></asp:TextBox>
                </td>
                <th align="right" valign="top">
                    Ship To Name/Name 2:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_ShipToName" runat="server" Columns="20" MaxLength="50"></asp:TextBox>
                    <asp:TextBox ID="F_ShipToName2" runat="server" Columns="20" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right" valign="top">
                    Total:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_Total" runat="server" Columns="20" MaxLength="50"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="PasswordLenghtValidator" runat="server" ControlToValidate="F_Total"
                        Display="Dynamic" ErrorMessage="Please input decimal number!" ValidationExpression="^[-+]?[0-9]*\.?[0-9]*([?[0-9]+)?$"></asp:RegularExpressionValidator>
                </td>
                <th align="right" valign="top">
                    Ship To ZipCode:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_ShipToZipCode" runat="server" Columns="20" MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right" valign="top">
                    Order Date:
                </th>
                <td valign="top" class="field">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="smaller">
                                From
                                <CC:DatePicker ID="F_OrderDateLbound" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td class="smaller">
                                To
                                <CC:DatePicker ID="F_OrderDateUbound" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td valign="bottom">
                                <asp:DropDownList ID="F_OrderDate" runat="server">
                                    <asp:ListItem Value="">-- ALL --</asp:ListItem>
                                    <asp:ListItem Value="0">Today</asp:ListItem>
                                    <asp:ListItem Value="3">Last 3 days</asp:ListItem>
                                    <asp:ListItem Value="7">Last 7 days</asp:ListItem>
                                    <asp:ListItem Value="month">Last a month</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
                <th align="right" valign="top">
                    Bill To Country/State:
                </th>
                <td valign="top" class="field">
                    <div style="float: left">
                        <asp:DropDownList ID="F_BillToCountry" onchange="changeSelect('Bstate')" runat="server" />
                    </div>
                    <div id="dBState" style="display: none; float: left">
                        <asp:DropDownList ID="F_BillToCounty" runat="server" />
                    </div>
                </td>
            </tr>
            <tr>
                <th align="right" valign="top">
                    Order From:
                </th>
                <td valign="top" class="field">
                    <div style="float: left">
                        <asp:DropDownList ID="F_OrderFrom" onchange="changeSelect('date')" runat="server">
                            <asp:ListItem Value="">-- ALL --</asp:ListItem>
                            <asp:ListItem Value="0">Live</asp:ListItem>
                            <asp:ListItem Value="1">eBay</asp:ListItem>
                            <asp:ListItem Value="2">Amazon</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div id="dOrderdate" style="display: none;">
                        <asp:DropDownList ID="F_OrderFrom1" runat="server">
                            <asp:ListItem Value="">-- ALL --</asp:ListItem>
                            <asp:ListItem Value="0">Full Site</asp:ListItem>
                            <asp:ListItem Value="1">Mobile</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </td>
                <th align="right" valign="top">
                    Bill To Salon Name:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_BillToSalonName" runat="server" Columns="50" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right" valign="top">
                    Status:
                </th>
                <td valign="top" class="field">
                    <asp:CheckBox ID="chkgrey" runat="server" /><img style="border-width: 0px;" src="/includes/theme-admin/images/not_export.png"
                        title="Order is pending export
Cart items are pending export" id="ctl00_ph_gvList_ctl03_imExportStatus">
                    <asp:CheckBox ID="chkblue" runat="server" /><img style="border-width: 0px;" src="/includes/theme-admin/images/reexport.png"
                        title="Order is pending exported
Cart items are exported" id="Img1">
                    <asp:CheckBox ID="chkred" runat="server" /><img style="border-width: 0px;" src="/includes/theme-admin/images/exported.png"
                        title="Order is exported
Cart items are exported" id="ctl00_ph_gvList_ctl04_imExportStatus">
                    <asp:CheckBox ID="chkblack" runat="server" /><img style="border-width: 0px;" src="/includes/theme-admin/images/downloaded.png"
                        title="Order is downloaded
Cart items are downloaded" id="ctl00_ph_gvList_ctl46_imExportStatus">
                </td>
                <th align="right" valign="top">
                    Bill To Name/Name 2:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_BillToName" runat="server" Columns="20" MaxLength="30"></asp:TextBox>
                    <asp:TextBox ID="F_BillToName2" runat="server" Columns="20" MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right" valign="top">
                    Payment:
                </th>
                <td valign="top" class="field">
                    <asp:DropDownList ID="F_PaymentType" runat="server">
                        <asp:ListItem Value="">-- ALL --</asp:ListItem>
                        <asp:ListItem Value="0">Credit Card</asp:ListItem>
                        <asp:ListItem Value="1">PayPal</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <th align="right" valign="top">
                    Bill To Zipcode:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_BillToZipcode" runat="server" Columns="20" MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right" valign="top">
                    PayPal:
                </th>
                <td valign="top" class="field">
                    <asp:DropDownList ID="F_PayPalAddress" runat="server">
                        <asp:ListItem Value="">-- PayPal Address --</asp:ListItem>
                        <asp:ListItem Value="0">Match</asp:ListItem>
                        <asp:ListItem Value="1">Unmatch</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="F_PayPalType" runat="server">
                        <asp:ListItem Value="">-- PayPal Type --</asp:ListItem>
                        <asp:ListItem Value="0">Instant</asp:ListItem>
                        <asp:ListItem Value="1">eCheck</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <th align="right" valign="top">
                    Transaction ID:
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="txtTransactionID" runat="server" Columns="20" MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <CC:OneClickButton ID="btnSearch" runat="server" Text="Search" CssClass="btn" />
                    <input class="btn" type="submit" value="Clear" onclick="window.location='default.aspx';return false;" />
                </td>
            </tr>
        </table>
    </asp:Panel>
   <p>
    <table>
        <tr>
            <td>
                <uc1:pager ID="pagerTop" PageSize="36" runat="server" ViewMode="1" OnPageSizeChanging="pagerTop_PageSizeChanging"
                    OnPageIndexChanging="pagerTop_PageIndexChanging" />
            </td>
        </tr>
        <tr>
            <td>
                <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" AllowPaging="false"
                    AllowSorting="True" HeaderText="In order to change display order, please use header links"
                    EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False"
                    BorderWidth="0" PagerSettings-Position="Bottom">
                    <AlternatingRowStyle CssClass="alt-member" VerticalAlign="Top"></AlternatingRowStyle>
                    <RowStyle CssClass="rowmember" VerticalAlign="Top"></RowStyle>
                    <Columns>
                        <asp:TemplateField ControlStyle-Width="20">
                             
                            <ItemTemplate>
                                <asp:HyperLink EnableViewState="False" runat="server" NavigateUrl='<%# "edit.aspx?OrderId=" & DataBinder.Eval(Container.DataItem, "OrderId") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                                    ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit" ToolTip="Edit"></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ControlStyle-Width="20">
                        
                            <ItemTemplate>
                                <asp:HyperLink EnableViewState="False" Target="_blank" runat="server" NavigateUrl='<%# "../orders/default.aspx?MemberId=" & DataBinder.Eval(Container.DataItem, "MemberId")  %>'
                                    ImageUrl="/includes/theme-admin/images/list-order.png" ID="lnkOrder" ToolTip="Others Orders"></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ControlStyle-Width="80">
                            <HeaderTemplate>
                                <a href="javascript:void(0);" onclick="Sort('OrderNo')">Order No</a>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.OrderNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="BillToSalonName" HeaderStyle-Width="100"  HeaderText="Salon Name"></asp:BoundField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80" ItemStyle-VerticalAlign="Top"
                            HeaderText="Name">
                            <ItemTemplate>
                                <asp:Literal ID="lnkMember" runat="server"></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="BillToName2" HeaderText="Name 2"></asp:BoundField>
                        <asp:BoundField DataField="BillToCounty" HeaderText="State"></asp:BoundField>
                        <asp:BoundField DataField="BillToCountry" HeaderText="Country" ItemStyle-HorizontalAlign="Center">
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Email">
                            <HeaderTemplate>
                                <a href="javascript:void(0);" onclick="Sort('Email')">Email</a>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="ltrEmail" runat="server"></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <a href="javascript:void(0);" onclick="Sort('ProcessDate')">Order Date</a>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblProcessDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ProcessDate") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right">
                        </asp:BoundField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"
                            HeaderText="Status">
                            <ItemTemplate>
                                <asp:Image ID="imExportStatus" runat="server" ImageUrl="/includes/theme-admin/images/downloaded.png" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"
                            HeaderText="Ship Via">
                            <ItemTemplate>
                                <%#ShowShipVia(IIf(IsDBNull(DataBinder.Eval(Container.DataItem, "OrderId")), String.Empty, DataBinder.Eval(Container.DataItem, "OrderId")))%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Transaction ID">
                            <ItemTemplate>
                                <%#ShowTransaction(IIf(IsDBNull(DataBinder.Eval(Container.DataItem, "OrderNo")), String.Empty, DataBinder.Eval(Container.DataItem, "OrderNo")), IIf(IsDBNull(DataBinder.Eval(Container.DataItem, "EbayOrderId")), String.Empty, DataBinder.Eval(Container.DataItem, "EbayOrderId")), IIf(IsDBNull(DataBinder.Eval(Container.DataItem, "AmazonOrderId")), String.Empty, DataBinder.Eval(Container.DataItem, "AmazonOrderId")), DataBinder.Eval(Container.DataItem, "TransactionID"), DataBinder.Eval(Container.DataItem, "PaypalStatus"), IIf(IsDBNull(DataBinder.Eval(Container.DataItem, "PaypalShipToAddress")), String.Empty, DataBinder.Eval(Container.DataItem, "PaypalShipToAddress")))%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </CC:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <uc1:pager ID="pagerBottom" PageSize="36" runat="server" ViewMode="1" OnPageSizeChanging="pagerTop_PageSizeChanging"
                    OnPageIndexChanging="pagerTop_PageIndexChanging" />
            </td>
        </tr>
    </table>
    </p>
    <CC:OneClickButton ID="btnSort" runat="server" Text="Save" CssClass="btnHidden">
    </CC:OneClickButton>
    <input type="hidden" id="hidSortField" runat="server" />
    <asp:HiddenField ID="hidCon" runat="server" />
    <div runat="server" id="divDownload" visible="false">
        <asp:HyperLink ID="lnkDownload" runat="server">Download File</asp:HyperLink>
    </div>
    <script type="text/javascript">
        function Sort(field) {

            document.getElementById('<%=hidSortField.ClientID %>').value = field;
            var btn = document.getElementById('<%=btnSort.ClientID %>');
            if (btn) {

                btn.click();
            }
        }
        $(document).ready(function () {
            var arr = document.getElementsByTagName("label");
            for (var t = 0; t < arr.length; t++) {
                if (arr[t].id.indexOf('tip') >= 0) {
                    ShowTip(arr[t].id);
                }
            }
        });

        function ShowTip(id) {
            var titletext = $('#' + id).attr("paramTitle");
            var desc = $('#' + id).attr("paramDes");
            $('#' + id).qtip({
                content: {
                    title: titletext,
                    text: desc
                },
                style: {

                    backgroundColor: 'White',
                    border: {
                        width: 3,
                        radius: 5,
                        color: 'Silver'
                    },
                    width: 650,
                    padding: 5,
                    textAlign: 'left',
                    tip: true, // Give it a speech bubble tip with automatic corner detection
                    name: 'light' // Style it according to the preset 'cream' style
                },
                position: {
                    corner: {
                        target: 'bottomLeft',
                        tooltip: 'rightTop'
                    }
                }
            });
        }

        function ConfirmReExport() {
            return confirm('Are you sure to re-export this order?');
        }
        var dprOrder1 = $('#<%= F_OrderFrom.ClientID %> option:selected').val();
        if (dprOrder1 == "0")
            document.getElementById("dOrderdate").style.display = "block";

        var dprShipCountry = $('#<%= F_ShipToCountry.ClientID %> option:selected').val();
        if (dprShipCountry == "US")
            document.getElementById("dSstate").style.display = "block";

        var dprBillCountry = $('#<%= F_BillToCountry.ClientID %> option:selected').val();
        if (dprBillCountry == "US")
            document.getElementById("dBState").style.display = "block";

        function changeSelect(val) {
            if (val == "date") {
                var dprOrder = $('#<%= F_OrderFrom.ClientID %> option:selected').val();
                if (dprOrder == "0")
                    document.getElementById("dOrderdate").style.display = "block";
                else
                    document.getElementById("dOrderdate").style.display = "none";
            }
            else if (val == "Sstate") {
                var dprShip = $('#<%= F_ShipToCountry.ClientID %> option:selected').val();
                if (dprShip == "US")
                    document.getElementById("dSstate").style.display = "block";
                else {

                    document.getElementById("dSstate").style.display = "none";

                }

            }
            else if (val == "Bstate") {
                var dprBill = $('#<%= F_BillToCountry.ClientID %> option:selected').val();
                if (dprBill == "US")
                    document.getElementById("dBState").style.display = "block";
                else
                    document.getElementById("dBState").style.display = "none";
            }

        }
       
        
    </script>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
</asp:Content>
