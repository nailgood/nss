<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false"
    CodeFile="addpoint.aspx.vb" Inherits="admin_members_AddPoint" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ph" runat="Server">
<script type="text/javascript">
        function CheckPoint(source, arguments) {

            var value = arguments.Value;
            
            if (value != '') {

                if (isNaN(value)) {
                    arguments.IsValid = false;
                }
                else {
                    value = parseInt(value, 0);
                    if (value < 1)
                        arguments.IsValid = false;
                    else
                        arguments.IsValid = true;
                }

            }
            else arguments.IsValid = true;

          
        }
    </script>

    <div style="margin: 0">
        <h4>
            <asp:Literal ID="ltrHeader" runat="server" Text="Manual Adjustment Point for user: "></asp:Literal></h4>
            
            <CC:OneClickButton ID="btnCancel" runat="server" Text="Back to User" CssClass="btn" CausesValidation="False">
        </CC:OneClickButton>
        <span style="color: Red">
            <asp:Literal ID="ltrError" Text="Error" Visible="false" runat="server"></asp:Literal>
        </span>
        <table border="0" cellspacing="1" cellpadding="2">
            <tr>
                <td colspan="2">
                    <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
                </td>
            </tr>
            
            <tr>
                <td class="required">
                    Points Earned:
                </td>
                <td class="field" style="width: 500px">
                    <asp:TextBox ID="txtPointEarn" runat="server" MaxLength="100" Width="100px"></asp:TextBox>
                </td>
                <td>
                    <asp:CustomValidator ID="cusvPointEarn" ControlToValidate="txtPointEarn" EnableClientScript="true"
                        ValidationGroup="point" CssClass="msgError" ErrorMessage="Points Earned is invalid" ClientValidationFunction="CheckPoint" OnServerValidate="CheckSPoint"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <td class="required">
                    Points Debit:
                </td>
                <td class="field" style="width: 500px">
                    <asp:TextBox ID="txtPointDebit" runat="server" MaxLength="100" Width="100px"></asp:TextBox>
                    <i>Total points available: <%=totalPoint%></i>
                </td>
                <td>
                       <asp:CustomValidator ID="cusvPointDebit" ControlToValidate="txtPointDebit" EnableClientScript="true"
                        ValidationGroup="point" CssClass="msgError" ErrorMessage="Points Debit is invalid" ClientValidationFunction="CheckPoint" OnServerValidate="CheckSPoint"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <td class="required">
                    Status:
                </td>
                <td class="field" style="width: 500px">
                    <asp:DropDownList ID="drlStatus" runat="server">
                        <asp:ListItem Value="">- - -</asp:ListItem>
                        <asp:ListItem Value="0">Pending</asp:ListItem>
                        <asp:ListItem Value="1">Active</asp:ListItem>
                        <asp:ListItem Value="2">InActive</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="drlStatus" ValidationGroup="point"
                        CssClass="msgError" ErrorMessage="Status is required"></asp:RequiredFieldValidator>
                </td>
            </tr>
             <tr>
                <td class="optional">
                    Transaction No:
                </td>
                <td class="field" style="width: 500px">
                    <asp:TextBox ID="txtTransactionNo" runat="server" Enabled="true" MaxLength="100" Width="100px"></asp:TextBox>
                </td>
                <td>
                  
                </td>
            </tr>
            <tr>
                <td class="optional">
                    Description:
                </td>
                <td class="field">
                    <asp:TextBox ID="txtNote" runat="server" MaxLength="800" Width="380px" Height="150px"
                        TextMode="MultiLine"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
        </table>
         
        <CC:OneClickButton ID="btnSave" runat="server" Text="Save" ValidationGroup="point"
            CssClass="btn"></CC:OneClickButton>
         <CC:OneClickButton ID="btnAddNew" runat="server" Text="Clear"  CausesValidation="false"
            CssClass="btn"></CC:OneClickButton>
        <br />
        <br />
        <span class="smallest"><span class="red">red color</span> - status and point is pending</span>
        <br />
        <asp:Repeater ID="rptTrans" runat="server">
            <HeaderTemplate>
                <table id="pointTrans" rules="all" cellpadding="0" cellspacing="1">
                    <tr style="height: 25px">
                        <th style="width: 80px">
                            Date
                        </th>
                        <th style="width: 100px">
                            Trans ID
                        </th>
                        <th>
                            Description
                        </th>
                        <th style="width: 100px">
                            Amount
                        </th>
                        <th style="width: 100px">
                            Points Debit
                        </th>
                        <th style="width: 100px">
                            Points Earned
                        </th>
                         <th style="width: 50px">
                            Status
                        </th>
                         <th style="width: 25px">
                           
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr style="height: 25px">
                    <td class="row" align="center">
                        <%#DataBinder.Eval(Container.DataItem, "CreateDate", "{0:MM/dd/yyyy}")%>
                    </td>
                    <td class="row" align="center">
                        <asp:Literal ID="ltrLink" runat="server"></asp:Literal>
                    </td>
                    <td class="row" align="left" style="padding-left:5px;padding-right:5px;">
                        <%#Eval("Notes")%>
                    </td>
                    <td class="row" align="right" style="padding-right: 5px;">
                        <asp:Literal ID="ltrAmount" runat="server"></asp:Literal>
                    </td>
                    <td class="row" align="right" style="padding-right: 5px;">
                        <asp:Literal ID="ltrPointDebit" runat="server"></asp:Literal>
                    </td>
                    <td class="row" align="right" style="padding-right: 5px;">
                        <asp:Literal ID="ltrPointEarn" runat="server"></asp:Literal>
                    </td>
                      <td class="row" align="left" style="padding:0px 5px;">
                        <asp:Literal ID="ltrStaus" runat="server"></asp:Literal>
                    </td>
                     <td class="row" align="left" style="padding-left: 5px;">
                        <asp:Literal ID="ltrEdit" runat="server"></asp:Literal>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr style="height: 25px">
                    <td class="alternate" align="center">
                        <%#DataBinder.Eval(Container.DataItem, "CreateDate", "{0:MM/dd/yyyy}")%>
                    </td>
                    <td class="alternate" align="center">
                        <asp:Literal ID="ltrLink" runat="server"></asp:Literal>
                    </td>
                    <td class="alternate" align="left" style="padding-left: 5px;">
                        <%#Eval("Notes")%>
                    </td>
                    <td class="alternate" align="right" style="padding-right: 5px;">
                        <asp:Literal ID="ltrAmount" runat="server"></asp:Literal>
                    </td>
                    <td class="alternate" align="right" style="padding-right: 5px;">
                        <asp:Literal ID="ltrPointDebit" runat="server"></asp:Literal>
                    </td>
                    <td class="alternate" align="right" style="padding-right: 5px;">
                        <asp:Literal ID="ltrPointEarn" runat="server"></asp:Literal>
                    </td>
                    <td class="alternate" align="left" style="padding:0px 5px;">
                        <asp:Literal ID="ltrStaus" runat="server"></asp:Literal>
                    </td>
                    <td class="alternate" align="left" style="padding-left: 5px;">
                        <asp:Literal ID="ltrEdit" runat="server"></asp:Literal>
                    </td>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
</asp:Content>
