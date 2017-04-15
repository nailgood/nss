<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="admin_store_shippingcountry_default" MasterPageFile="~/includes/masterpage/admin.master" %>

<asp:Content ID="Content" runat="server" ContentPlaceHolderID="ph">
    <h4>Import Shipping Country</h4>
    <div>
   
        <table>
            <tr>
            <td align="left" colspan ="2" ><asp:Label id="lblMsg" runat ="server" ForeColor ="blue" Font-Italic="true"  ></asp:Label></td>
            </tr>
            <tr>
                <td><input type="file" runat="server" id="ctlCSVFile" />
                    
                <asp:TextBox ID="txtDataRow" runat ="server" TextMode="MultiLine" Height="100px" Width="200px" Visible ="false"   ></asp:TextBox> 
                </td>
                
            </tr>
            <tr>
                <td><CC:OneClickButton id="ctlImportButton" runat="server" text="Import" cssClass="btn"  />
                    <CC:ConfirmButton id="btnRemove1" runat="server" Message="Are you sure want to Remove this EmailList ?" Text="Remove" cssClass="btn" CausesValidation="False" Visible="false"></CC:ConfirmButton>
                    <CC:OneClickButton id="btnRemove" runat="server" Text="Save" cssClass="btn" Visible ="false"></CC:OneClickButton> </td>
            </tr>
        </table>
        
    </div>
    <hr />  
       
        <asp:GridView id="ctlGridView" runat="server" AllowPaging="false">
        <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top" />
        <RowStyle CssClass="row" VerticalAlign="Top" /> 
        </asp:GridView>     
    
    
</asp:Content> 