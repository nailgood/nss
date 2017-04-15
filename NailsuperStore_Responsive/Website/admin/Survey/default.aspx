<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="admin_Survey_default" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ph" Runat="Server">
<script type="text/javascript">
    function showPopup(id) {
        var url = 'view.aspx?Id=' + id;
        var p = window.showModalDialog(url, '', 'dialogHeight: 500px; dialogWidth: 800px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
    }
</script>
    <h4><asp:Literal runat="server" ID="ltrTitle"></asp:Literal></h4>
      
    <span class="smaller">Please provide search criteria below</span>
    <asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
        <table cellpadding="2" cellspacing="2">
           <tr runat="server" visible="false">
                <th valign="top">
                    <b>Survey Code:</b>
                </th>
                <td valign="top" class="field">
                    <asp:DropDownList runat="server" ID="drpSurveyCode"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th valign="top">
                    <b>Customer Name:</b>
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_txtName" runat="server"></asp:TextBox>
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
                    <b>Email:</b>
                </th>
                <td valign="top" class="field">
                    <asp:TextBox ID="F_txtEmail" runat="server"></asp:TextBox>
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
                                <CC:DatePicker ID="F_DateLbound" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td class="smaller">
                                To
                                <CC:DatePicker ID="F_DateUbound" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>           
            <tr>
                <td colspan="2" align="right">
                    <CC:OneClickButton ID="btnSearch" runat="server" Text="Search" CssClass="btn" />
                    <CC:OneClickButton ID="btnClear" runat="server" Text="Clear" CssClass="btn" />
<%--                    <input class="btn" type="submit" value="Clear" onclick="window.location='default.aspx';return false;" />
--%>                </td>
            </tr>
        </table>
    </asp:Panel>
    <p>
    </p>
   
    <p>
    </p>
    <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="30"
        AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria"
        AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
        <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
        <Columns>
            <asp:TemplateField HeaderText="Order" Visible="false">
                <ItemTemplate>
                        <asp:Literal runat="server" ID="ltrOrder"></asp:Literal>
<%--                        <asp:Label ID="lbOrderId" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OrderId") %>'></asp:Label>
--%>                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField SortExpression="cus.CustomerNo" HeaderText="Customer No"  HeaderStyle-Width="80px" ControlStyle-CssClass="">
                <ItemTemplate>
                    <asp:HyperLink ID="lblCustomerNo" runat="server" NavigateUrl='<%# "/admin/members/edit.aspx?MemberId=" & DataBinder.Eval(Container.DataItem, "MemberId") %>' ><%# DataBinder.Eval(Container.DataItem, "CustomerNo") %></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField SortExpression="sr.CustomerName" HeaderStyle-Width="100px" HeaderText="Name">
                <ItemTemplate>
                        <asp:Label ID="lbName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CustomerName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField SortExpression="sr.CustomerEmail" HeaderText="Email">
                <ItemTemplate>
                        <asp:Label ID="lblEmail" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CustomerEmail") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField SortExpression="CreatedDate" DataField="CreatedDate" HeaderText="Date"  HeaderStyle-Width="80px" 
                DataFormatString="{0:d}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right">
            </asp:BoundField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Points" ItemStyle-Width="60" >
                <ItemTemplate>
                    <asp:ImageButton ID="imbPoint" runat="server" ImageUrl="/includes/theme-admin/images/plus.png" CommandName="AddPoint"
                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>' />
                          <asp:Literal ID="ltrPoint" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
           <asp:TemplateField>
                <ItemTemplate>
                   <a href="javascript:void(0)" onclick="showPopup('<%#DataBinder.Eval(Container.DataItem, "Id") %>');return(false);">View</a>
               </ItemTemplate>
            </asp:TemplateField>            
        </Columns>
    </CC:GridView>  
<asp:HiddenField ID="hidCon" runat="server" />
</asp:Content>

