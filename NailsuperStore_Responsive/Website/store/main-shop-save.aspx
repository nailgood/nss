<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/main.master" AutoEventWireup="false" CodeFile="main-shop-save.aspx.vb" Inherits="store_main_shop_save" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">

    <div id="main-shop-save">
    <div class="ver-line">&nbsp;</div>
        <div class="ver-line-group">&nbsp;</div>
        <ul>
              <asp:Repeater runat="server" ID="rptShopSave">
                     <ItemTemplate>
                           <li>
                           <hr />
                                    
                                 
                               <div>
                                 <div class="ver-line">&nbsp;</div> 
                                    <asp:Literal runat="server" ID="ltrLink"></asp:Literal>
                
                                    
                               </div>
                     
                           </li>
                    </ItemTemplate>
              </asp:Repeater>
        </ul>
    </div>
</asp:Content>

