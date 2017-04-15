<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SearchReview.aspx.vb" Inherits="admin_store_departments_testimonial_SearchReview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../../includes/theme-admin/css/admin.css" rel="stylesheet" type="text/css" />
    <script src="../../../../includes/scripts/admin.js" type="text/javascript"></script>
     <script src="../../../../includes/scripts/XmlHttpLookupAdmin.js" type="text/javascript"></script>
    <script src="../../../../includes/scripts/ajaxQueue.js" type="text/javascript"></script>
    <script src="../../../../includes/theme-admin/css/session.js.aspx" type="text/javascript"></script>
     <script type="text/javascript" src="../../../../includes/theme-admin/scripts/Browser.js"></script>
    <%-- <script language="jscript">
         window.name = "modal";
    </script>--%>
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

            InitQueryCode('LookupField', '/admin/ajax.aspx?f=DisplayItems&Type=user&q=', MyCallback);

        }
        function InitializeQuery() {

            InitQueryCode('LookupField', '/admin/ajax.aspx?f=DisplayItems&Type=user&q=', MyCallback);

        }         
    </script>
</head>
<body onload="CheckTarget();">
    <form id="form1" runat="server">
    <span class="smaller">Please provide search criteria below</span>
       <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
     <table cellpadding="2" cellspacing="2">
        <tr>
            <th valign="top">
                Department:
            </th>
            <td valign="top" class="field">
                <asp:DropDownList ID="F_DepartmentId" runat="server" />
            </td>
        </tr>
        <tr>
            <th valign="top">
                SKU:
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
            <td colspan="2" align="right">                
                <CC:OneClickButton ID="btnSearch" runat="server" Text="Search" CssClass="btn" ></CC:OneClickButton>
                <CC:OneClickButton ID="btnClear" runat="server" Text="Clear" CssClass="btn" ></CC:OneClickButton>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <table>
        <tr>
            <td align="left">
                <input type="button" value="Save" class="btn" onclick="Save();" />
                <input type="button" value="Close" class="btn" onclick="ClosePopup();" />
            </td>
        </tr>
        <tr>
            <td>
                <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50"
        AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria"
        AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
        <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
        <Columns>
        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
        <asp:CheckBox ID="chk_ReviewId" runat="server" />
        </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField SortExpression="ReviewerName" DataField="ReviewerName" HeaderText="Reviewer" ItemStyle-Width="120" />
         <asp:BoundField SortExpression="ReviewTitle" DataField="ReviewTitle" HeaderText="Title" ItemStyle-Width="250" />
         <asp:TemplateField HeaderText="Comment"  ItemStyle-Width="400">
                <ItemTemplate>
                    <asp:Literal ID="ltrComment" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
          <asp:BoundField SortExpression="Itemname" DataField="Itemname" HeaderText="Item" ItemStyle-Width="250" />
            <asp:TemplateField SortExpression="NumStars" HeaderText="Stars">
                <ItemTemplate>
                    <asp:Literal ID="ltrStar" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField SortExpression="DateAdded" DataField="DateAdded" HeaderText="Date"
                DataFormatString="{0:d}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right">
            </asp:BoundField>  
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Facebook">
                <ItemTemplate>
                    <asp:Image ID="imbFacebook" runat="server" ImageUrl="/includes/theme-admin/images/active.png" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </CC:GridView>
    <input type="hidden" runat="server" value="" id="hidCon" />
    <input type="hidden" runat="server" value="" id="hidReviewIdSelect" />
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        function CheckItem(id, status) {
            var idSelect = document.getElementById('<%=hidReviewIdSelect.ClientID %>').value;
            if (status) {
                idSelect += id + ';';
            }
            else idSelect = idSelect.replace(id + ';', '');
            document.getElementById('<%=hidReviewIdSelect.ClientID %>').value = idSelect;
        }

        function ClosePopup() {
            SetParentData('0', '');
            window.close();
        }

        function Save() {

            var id = document.getElementById('<%=hidReviewIdSelect.ClientID %>').value;
            if (id != '') {
                SetParentData('1', id);
                //window.opener.SetValue('1',id)           
                window.close();
            }
            else {
                alert('Please select at least an item.');
            }
        }
        function SetParentData(save, data) {
            var brow = GetBrowser();
            if (brow == 'ie' || brow == 'safari') {
                opener.ButtonClick(data);
                window.returnValue = data;
            }
            else {
                alert('2---------' + data);
                window.opener.SetValue(save, data)
            }
        }
        function CheckTarget() {
            var frm = document.getElementById('form1')

            if (frm) {
                var brow = GetBrowser();
                if (brow == 'ie') {
                    frm.target = "modal"
                }
                else frm.target = ""

            }
        }
    </script>
    </form>
</body>
</html>
