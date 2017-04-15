<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="admin_WishListEmailTemplate_Default" %>
<asp:Content ID="Content" ContentPlaceHolderID="ph" Runat="Server">
 <h4>Wishlist Email Template Administration</h4>
     <table cellSpacing="0" cellPadding="0" border="0" id="tblList" runat="server">
        <tr>
            <td><asp:datagrid id="dgList" runat="server"  AutoGenerateColumns="False" CellSpacing="2" CellPadding="2" AllowSorting="True" BorderWidth="0" Width="100%">
                    <AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
                    <ItemStyle CssClass="row"></ItemStyle>
                    <HeaderStyle CssClass="header"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "edit.aspx?EmailTemplateId=" & DataBinder.Eval(Container.DataItem, "EmailTemplateId")  %>' ImageUrl="/includes/theme-admin/images/edit.gif" ID="Hyperlink1">Edit</asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                                       
						<asp:BoundColumn DataField="EmailSubject" HeaderText="Email Subject"></asp:BoundColumn>
                        <asp:BoundColumn DataField="EmailPurpose" HeaderText="Email Purpose"></asp:BoundColumn>                                
                        <asp:BoundColumn DataField="EmailBodyText" HeaderText="Email Body"></asp:BoundColumn> 
                                                    
                    </Columns>
                   
                </asp:datagrid></td>
        </tr>       
    </table>
    </asp:Content>

