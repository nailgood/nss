<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master"
    Title="Item" CodeFile="default.aspx.vb" Inherits="admin_store_items_Index" %>

<%@ Register Src="~/controls/layout/pager.ascx" TagName="pager" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ph" ID="Content1" runat="server">

    <script language="javascript">
<!--
        function expandall() {
          
            var arr = document.getElementById('<%=hidItemIds.ClientID %>').value.split(',');
            var img = document.getElementById('IMGALL');
           
            if (img.src.indexOf("down") !== -1) {
                img.src = img.src.replace(/-down/i, "-up");
            }
            else {
                img.src = img.src.replace(/-up/i, "-down");
            }
          
           for (var i = 0; i < arr.length; i++) {
                try
                {
                    expandit(arr[i]);
                }
                catch(err)
                {
                    //alert(err.toString());
                }
                
            }
        }

        function expandit(objid) {
           
            var span = document.getElementById('SPAN' + objid).style;
            var img = document.getElementById('IMG' + objid);
            var imgtext = document.getElementById('imgtext' + objid);
            if (span.display == "none") {
                span.display = "block"
                img.src = img.src.replace(/-down/i, "-up");
                imgtext.innerHTML = 'Hide Image';
            } else {
                span.display = "none"
                img.src = img.src.replace(/-up/i, "-down");
                imgtext.innerHTML = 'View Image';
            }
      
            
        }
//-->
    </script>
    <asp:HiddenField ID="hidItemIds" runat="server" />
    <asp:Panel runat="server" ID="pnItem" DefaultButton="btnSearch">
    <h4>Item</h4>
    <span class="smaller">Please provide search criteria below</span>
    <table cellpadding="2" cellspacing="2">
        <tr>
            <th align="right" valign="top">
                Department:
            </th>
            <td valign="top" class="field">
                <asp:DropDownList ID="F_DepartmentId" runat="server" />
            </td>
            <th align="right"  valign="top">
                Item Name:
            </th>
            <td valign="top" class="field">
                <asp:TextBox ID="F_ItemName" runat="server" Columns="50" MaxLength="255"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th  align="right" valign="top">
                Brand:
            </th>
            <td valign="top" class="field">
                <asp:DropDownList ID="F_BrandId" runat="server" />
            </td>            
            <th  align="right" valign="top">
                SKU:
            </th>
            <td valign="top" class="field">
                <asp:TextBox ID="F_SKU" runat="server" Columns="20" MaxLength="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th  align="right" valign="top">
                Group Name:
            </th>
            <td valign="top" class="field">
                <asp:DropDownList ID="F_GroupName" runat="server" />
            </td>
             <th  align="right" valign="top">
                Amazon & eBay:
            </th>
            <td valign="top" class="field">
                <asp:DropDownList ID="F_IsItemSelling" runat="server">
                        <asp:ListItem Value="">-- Is Selling? --</asp:ListItem>
                        <asp:ListItem Value="1">Yes</asp:ListItem>
                        <asp:ListItem Value="0">No</asp:ListItem>
                    </asp:DropDownList>
                    
                     <asp:DropDownList ID="F_IsEbay" runat="server">
                        <asp:ListItem Value="">-- Is Allow? --</asp:ListItem>
                        <asp:ListItem Value="1">Yes</asp:ListItem>
                        <asp:ListItem Value="0">No</asp:ListItem>
                    </asp:DropDownList>
                
            </td>
        </tr>
        <tr>
            <th  align="right" valign="top">
                Item Type:
            </th>
            <td valign="top" class="field">
                <asp:RadioButtonList ID="F_ItemType" runat="server" RepeatDirection="Horizontal" >
                    <asp:ListItem Text="All items" Value="0" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Buy in Bulk items" Value="3"></asp:ListItem>
                </asp:RadioButtonList>
                <%--<asp:DropDownList ID="F_ItemType" runat="server">
                    <asp:ListItem Text="-- ALL --" Value="" />
                    <asp:ListItem Text="Single Items" Value="0" />
                    <asp:ListItem Text="Item Group Items" Value="1" />
                </asp:DropDownList>--%>
            </td>            
            <th valign="top">
               <%-- Amazon & eBay:--%>
            </th>
            <td valign="top" class="field">
                
                <asp:DropDownList ID="F_IsFlammableUS" runat="server">
                    <asp:ListItem Value="">-- Is  Hazardous Material? --</asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:DropDownList>
                
                 <asp:DropDownList ID="F_IsFlammableInternational" runat="server">
                    <asp:ListItem Value="">-- Is Block air shipments? --</asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:DropDownList>
             </td>
            
        </tr>
        <tr>
            <th valign="top">
            </th>
            <td valign="top" class="field">
                <asp:DropDownList ID="F_IsActive" runat="server">
                    <asp:ListItem Value="">-- Is Active? --</asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:DropDownList>
                
                 <asp:DropDownList ID="F_IsNew" runat="server">
                    <asp:ListItem Value="">-- Is New? --</asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:DropDownList>
                 <asp:DropDownList ID="F_BestSeller" runat="server">
                    <asp:ListItem Value="">-- Is Best Seller? --</asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:DropDownList>
                   <asp:DropDownList ID="F_IsHot" runat="server">
                    <asp:ListItem Value="">-- Is Hot? --</asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:DropDownList>            
                              
              
            </td>            
            <th valign="top">
              <%-- Amazon Selling:--%>
            </th>
            <td valign="top" class="field">
                    <asp:DropDownList ID="F_PromotionCode" runat="server">
                    <asp:ListItem Value="">-- Is Has Coupon? --</asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:DropDownList>
                
                
                 <asp:DropDownList ID="F_IsRewardPoint" runat="server">
                    <asp:ListItem Value="">-- Is Rewards Points?--</asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:DropDownList>   
                        
                
                <asp:DropDownList ID="F_HasSalesPrice" runat="server">
                    <asp:ListItem Value="">-- Is Has Sale Price? --</asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:DropDownList>
                
             </td>
        </tr>
        <tr>
            <th valign="top"></th>
            <td valign="top" class="field">  
             <asp:DropDownList ID="F_IsSpecialOrder" runat="server">
                    <asp:ListItem Value="">-- Is Special Order? --</asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:DropDownList> 
                <asp:DropDownList ID="F_IsFreeSample" runat="server">
                    <asp:ListItem Value="">-- Is Free Sample? --</asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:DropDownList>
                
                <asp:DropDownList ID="F_IsFreeShipping" runat="server">
                    <asp:ListItem Value="">-- Is Free Shipping? --</asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:DropDownList>
                
            </td>
            <th valign="top">
            </th>
            <td valign="top" class="field">  
            <asp:DropDownList ID="F_IsFeatured" runat="server">
                    <asp:ListItem Value="">-- Is Feature? --</asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:DropDownList>      
               
                
                <asp:DropDownList ID="F_IsAcceptingOrder_InStock" runat="server">
                    <asp:ListItem Value="0">-- Is Accepting Order/In Stock? --</asp:ListItem>
                    <asp:ListItem Value="1">Accepting Order</asp:ListItem>
                    <asp:ListItem Value="2">In Stock</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
        <td>
         <CC:OneClickButton ID="AddNew" runat="server" Text="Add New Item" CssClass="btn">
    </CC:OneClickButton>
        </td>
            <td colspan="3" align="right">
                <CC:OneClickButton ID="btnSearch" runat="server" Text="Search" CssClass="btn" />
                <input class="btn" type="submit" value="Clear" onclick="window.location='default.aspx';return false;" />
                <CC:OneClickButton ID="btnEndEbay" runat="server" Text="" CssClass="btn" Style="visibility: hidden;" />
                <input type="hidden" runat="server" value="" id="hidEndEbay" />
            </td>
        </tr>
    </table>
   
   </asp:Panel>
    
    <table>
        <tr>
            <td>
                <uc1:pager ID="pagerTop" runat="server" ViewMode="1" OnPageSizeChanging="pagerTop_PageSizeChanging"
                    OnPageIndexChanging="pagerTop_PageIndexChanging" />
            </td>
        </tr>
        <tr>
            <td>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="up" runat="server">
<ContentTemplate>            
                <CC:GridView ID="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50"
                    AllowPaging="false" AllowSorting="True" HeaderText="In order to change display order, please use header links"
                    EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False"
                    BorderWidth="0" PagerSettings-Position="Bottom">
                    <AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
                    <RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink EnableViewState="False" runat="server" NavigateUrl='<%# "edit.aspx?ItemId=" & DataBinder.Eval(Container.DataItem, "ItemId") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                                    ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <CC:ConfirmLink EnableViewState="False" Message="Are you sure that you want to remove this Item?"
                                    runat="server" NavigateUrl='<%# "delete.aspx?ItemId=" & DataBinder.Eval(Container.DataItem, "ItemId") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                                    ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField  ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <img src="/includes/theme-admin/images/alter-image.png" alt="Alternate Image" title="Alternate Image" />
                            </HeaderTemplate>
                            <ItemTemplate >
                               <asp:HyperLink EnableViewState="False" runat="server" NavigateUrl='<%# "/admin/store/items/images/default.aspx?F_ItemId=" & DataBinder.Eval(Container.DataItem, "ItemId") & "&" & GetPageParams(Components.FilterFieldType.All) %>'
                                    ID="lnkImages"></asp:HyperLink>
                            </ItemTemplate>                            
                        </asp:TemplateField>                        
                         <asp:TemplateField>
                         <HeaderStyle />
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="litNewHotBestSeller" />
                               <asp:Literal runat="server" ID="litFreeShip_Sample" />
                            </ItemTemplate>                            
                        </asp:TemplateField>                        
                        <%--<asp:TemplateField>
                            <HeaderStyle />
                            <HeaderTemplate>
                                <a href="#" onclick="Sort('GroupName')">Group Name</a>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="litType" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField>
                          <HeaderStyle />
                            <HeaderTemplate>
                                <a href="#" onclick="Sort('ItemName')">Item Name</a>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="litItemName" />
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                         <HeaderStyle Width="60px" />
                            <HeaderTemplate>
                                <a href="#" onclick="Sort('SKU')">SKU</a>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="litSKU" />
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                         <HeaderTemplate>Case Sales Price</HeaderTemplate>
                            <ItemTemplate>
                             <asp:HyperLink EnableViewState="False" runat="server" NavigateUrl='<%# "/admin/store/items/salesprice/default.aspx?cprice=" & DataBinder.Eval(Container.DataItem,"CasePrice")& "&cqty=" & DataBinder.Eval(Container.DataItem,"CaseQty") & "&salestype=3&ItemId=" & DataBinder.Eval(Container.DataItem,"ItemId")%>'
                                    ID="lnkCaseSalePrice"></asp:HyperLink>
                               <%-- <input type="button" class="btn" style="font-size: 11px;" value=""
                                    onclick="window.location='/admin/store/items/salesprice/default.aspx?ItemId=<%#Container.DataItem("itemid")%>'" />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                         <HeaderTemplate>Sales Price</HeaderTemplate>
                            <ItemTemplate>
                             <asp:HyperLink EnableViewState="False" runat="server" NavigateUrl='<%# "/admin/store/items/salesprice/default.aspx?salestype=2&ItemId=" & DataBinder.Eval(Container.DataItem,"ItemId")%>'
                                    ID="lnkSalePrice"></asp:HyperLink>
                               <%-- <input type="button" class="btn" style="font-size: 11px;" value=""
                                    onclick="window.location='/admin/store/items/salesprice/default.aspx?ItemId=<%#Container.DataItem("itemid")%>'" />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                         <HeaderStyle />
                            <HeaderTemplate>
                                <a href="#" onclick="Sort('Price')">Price</a>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltlPrice" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                         <HeaderStyle />
                            <HeaderTemplate>
                                <a>Policies</a>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HyperLink EnableViewState="False" runat="server" 
                                    NavigateUrl='<%# "/admin/store/items/policies.aspx?pSize=20&pIndex=0&ItemId=" & DataBinder.Eval(Container.DataItem, "ItemId") & "&itemName=" & DataBinder.Eval(Container.DataItem, "ItemName")%>'
                                    Text='<%# DataBinder.Eval(Container.DataItem, "Policy")%>'
                                    ID="hplPoliciesTitle"></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Department(s)">
                         <HeaderStyle Width="250px" />
                            <ItemTemplate>
                                <ul>
                                <asp:Repeater ID="Departments" runat="server">                                    
                                    <ItemTemplate>
                                        <li style="list-style-image: url(/includes/theme-admin/images/minifolder.gif)">
                                            <%#Container.DataItem("NAME")%>
                                            </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                                </ul>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <a href="#" onclick="expandall()">Image Preview <img id='IMGALL' src="/includes/theme-admin/images/detail-down.gif" width="8" height="8" hspace="2" border="0" alt="Expand" align=absmiddle></a>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="imglink"><a href='javascript:expandit(<%#DataBinder.Eval(Container.DataItem,"ItemId")%>);'><span class="smaller" id="imgtext<%#DataBinder.Eval(Container.DataItem,"ItemId")%>">View Image</span><img id='IMG<%#DataBinder.Eval(Container.DataItem,"ItemId")%>' src="/includes/theme-admin/images/detail-down.gif" width="8" height="8" hspace="2" border="0" alt="Expand" align=absmiddle></a></asp:Label><asp:Label
                                    runat="server" ID="noimg" Text="" CssClass="smaller" /><span style="display: none"
                                        id='SPAN<%#DataBinder.Eval(Container.DataItem,"ItemId")%>'><asp:Literal ID="img" runat="server"></asp:Literal></span></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <a href="#" onclick="Sort('BrandName')">Brand</a>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="ltlBrandName" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField  ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <a href="#" onclick="Sort('si.IsActive')">Active</a>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsActive" runat="server" AutoPostBack="true" OnCheckedChanged="chkIsActive_Checked" ToolTip='<%#DataBinder.Eval(Container.DataItem,"ItemId")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <HeaderTemplate>
                                <a href="#" onclick="Sort('si.IsFeatured')">Featured</a>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsFeatured" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <a href="#" onclick="Sort('PromotionCode')">Coupon Code</a>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="ltrPromotionCode" runat="server"></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                      
                        <asp:TemplateField HeaderText="End this item in Ebay" Visible="false">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkEndEbay" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </CC:GridView>
                 <asp:HiddenField ID="hidCon" runat="server" />
</ContentTemplate>
</asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <uc1:pager ID="pagerBottom" runat="server" ViewMode="1" OnPageSizeChanging="pagerTop_PageSizeChanging"
                    OnPageIndexChanging="pagerTop_PageIndexChanging" />
            </td>
        </tr>
    </table>
    <CC:OneClickButton ID="btnSort" runat="server" Text="Save" CssClass="btnHidden">
    </CC:OneClickButton>
    <input type="hidden" id="hidSortField" runat="server" />
   

    <script type="text/javascript">
        function Sort(field) {

            document.getElementById('<%=hidSortField.ClientID %>').value = field;
            var btn = document.getElementById('<%=btnSort.ClientID %>');
            if (btn) {

                btn.click();
            }
        }
        function SetEbayEndItem(itemid, value) {

            document.getElementById('<%=hidEndEbay.ClientId %>').value = value + "," + itemid;
            var btn = document.getElementById('<%=btnEndEbay.ClientId %>');
            if (btn)
                btn.click();
        }
    
    </script>

</asp:Content>
