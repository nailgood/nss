<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="" CodeFile="items.aspx.vb" Inherits="admin_store_salescategory_related"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	<%--
	<script type="text/javascript">
	<!--
	function MyCallback(ItemId) {
		document.getElementById('ItemId').value = ItemId;
		GetItemInfo();
	}
	
	if (window.addEventListener) {
		window.addEventListener('load', InitializeQuery, false);
	} else if (window.attachEvent) {
		window.attachEvent('onload', InitializeQuery);
	}

	function InitializeQuery() {
		InitQueryCode('LookupField', '/admin/ajax.aspx?f=DisplayItemsAndCollections&q=', MyCallback);
	}
	
	function GetItemInfo() {
		var sItem, sConn;
		
		xml = getXMLHTTP();
		if(xml){
			xml.open("GET","/admin/Ajax.aspx?f=GetItemInfo&ItemId=" + getValue(document.getElementById('ItemId')),true);
			xml.onreadystatechange = function() {
				if(xml.readyState==4 && xml.responseText) {
					if (xml.responseText.length > 0) {
						aData = xml.responseText.split('|');
						
						sItem = '';
						sItem += 'Item Number: ' + aData[0] + '<br>';
						sItem += 'Item: ' + aData[1];
											
						document.getElementById('ItemInfo').innerHTML = sItem;
					} else {
						sItem = '';
					}	
				}
			}	
			xml.send(null);
		}
		if (!isEmptyField(document.getElementById('ItemId'))) {
			document.getElementById('<%=btnAdd.ClientID%>').disabled = false;
		} else {
			document.getElementById('<%=btnAdd.ClientID%>').disabled = true;
		}
	}	
	//-->
	</script>--%>
	<h4>Items for sales category '<asp:label ID="lblItemName" Runat="server" />'</h4>
	<br>
	<a href="default.aspx?<%=params%>">лл Go Back To Sales Category List</a>
	<p><b>Add Item</b>
		<table cellSpacing="2" cellPadding="3" border="0">
			<TBODY>
				<tr>
					<td class="optional" valign="top"><b>Item search</b></td>
					<td class="field" width="400">
						Please enter the first few characters of the item name or SKU below<br>
						<%--<input type="text" id="LookupField" name="LookupField" style="BORDER-RIGHT:#999999 1px solid; PADDING-RIGHT:4px; BORDER-TOP:#999999 1px solid; PADDING-LEFT:4px; BORDER-LEFT:#999999 1px solid; WIDTH:360px; BORDER-BOTTOM:#999999 1px solid">
						<input type="hidden" name="ItemId" id="ItemId">--%>
						<%--<p>
							<span id="ItemInfo" style="FONT-WEIGHT:bold; WIDTH:300px; COLOR:red; HEIGHT:40px"></span>
							<br>
						Please click the "Add Item" button below to add the item selected as a sales category item
						<p>--%>
                          <input type="button" class="btn" id="Button1" value="Add Item" onclick="OpenPopUp();" />
                        <asp:TextBox ID="txtSku" runat="server" MaxLength="8" Columns="8" Enabled="false" Style="width: 67px;"></asp:TextBox>
					<%--		<asp:button id="btnAdd" runat="server" Text="Add Item" cssClass="btn" disable="true"></asp:button></p>--%>
					</td>
					<td valign="top">
					</td>
				</tr>
			</TBODY>
		</table>
	<p>
		<b>View/Update Existing Sales Category Items</b>
		<table cellpadding="0" cellspacing="0" border="0" id="tblList" runat="server">
			<tr>
				<td width="435">
					<asp:DataGrid BorderWidth="0" PageSize="20" AllowPaging="True" AllowSorting="True" CellPadding="2"
						CellSpacing="2" id="dgList" runat="server" AutoGenerateColumns="False" Width="488px">
						<AlternatingItemStyle CssClass="alternate"></AlternatingItemStyle>
						<ItemStyle CssClass="row" VerticalAlign="Top"></ItemStyle>
						<HeaderStyle CssClass="header"></HeaderStyle>
						<Columns>
							<asp:TemplateColumn>
								<ItemTemplate>
									<CC:ConfirmLink enableviewstate=False Message="Are you sure that you want to remove this item?" runat="server" NavigateUrl= '<%# "itemsdelete.aspx?SalesCategoryId=" & DataBinder.Eval(Container.DataItem, "SalesCategoryId") & "&Id=" & DataBinder.Eval(Container.DataItem, "ItemId") & "&" & params %>' ImageUrl="/includes/theme-admin/images/delete.gif" ID="Confirmlink1"/>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn SortExpression="Y">
								<HeaderTemplate>
									SKU
								</HeaderTemplate>
								<ItemTemplate>
									<asp:Label enableviewstate=False runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SKU") %>' ID="Label3">
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn SortExpression="Y">
								<HeaderTemplate>
									Item Name
								</HeaderTemplate>
								<ItemTemplate>
									<asp:Label enableviewstate=False runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ItemName") %>' ID="Label1">
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn SortExpression="Y">
								<HeaderTemplate>
									Active?
								</HeaderTemplate>
								<ItemTemplate>
									<asp:Label enableviewstate=False runat="server" Text='<%# iif(DataBinder.Eval(Container, "DataItem.IsActive") = 0,"no","<b>Yes</b>") %>' ID="Label2">
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn>
								<ItemTemplate>
									<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "items-move.aspx?ACTION=UP&Id=" & DataBinder.Eval(Container.DataItem, "Id") & "&SalesCategoryId=" & DataBinder.Eval(Container.DataItem, "SalesCategoryId")  & "&" & params %>' ImageUrl="/includes/theme-admin/images/moveup.gif" ID="Hyperlink2"/>
								</ItemTemplate>
							</asp:TemplateColumn>                                
							<asp:TemplateColumn>
								<ItemTemplate>
									<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "items-move.aspx?ACTION=DOWN&Id=" & DataBinder.Eval(Container.DataItem, "Id") & "&SalesCategoryId=" & DataBinder.Eval(Container.DataItem, "SalesCategoryId")  & "&" & params %>' ImageUrl="/includes/theme-admin/images/movedown.gif" ID="Hyperlink3"/>
								</ItemTemplate>
							</asp:TemplateColumn>                                
						</Columns>
						<PagerStyle Visible="False"></PagerStyle>
					</asp:DataGrid>
				</td>
			</tr>
			<tr>
				<td width="435">
					<CC:Navigator id="myNavigator" runat="server" PagerSize="10" />
				</td>
			</tr>
		</table>
		<asp:placeholder id="plcNoRecords" runat="server" visible="false">
		<P>There are currently no items for this sales category.</asp:placeholder>

        <div style="display: none">
        <CC:OneClickButton ID="AddNew" runat="server" Text="Add New" CssClass="btn">
        </CC:OneClickButton>
      
        </div>
                <input type="hidden" runat="server" value="" id="hidPopUpSKU" />
          <input type="hidden" runat="server" value="" id="hidSaveValue" />
<script type="text/javascript" src="../../../../../includes/theme-admin/scripts/Browser.js"></script>
    <script>
        function SetValue(save, value, isactive) {
            if (save == '1') {
                document.getElementById('<%=hidPopUpSKU.ClientID %>').value = value;
            }
            document.getElementById('<%=hidSaveValue.ClientID %>').value = save;
            $("#<%= AddNew.ClientID %>").click();
        }
        function OpenPopUp() {

            var item = document.getElementById('<%=hidPopUpSKU.ClientID %>').value;
            var url = '../../promotions/ShopSave/SearchItem.aspx?Type=0&item=' + item

            var brow = GetBrowser();
            if (brow == 'ie') {
                var p = window.showModalDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
                document.getElementById('<%=hidPopUpSKU.ClientID %>').value = p;
                document.getElementById('<%=txtSku.ClientID %>').value = p;
                $("#<%= AddNew.ClientID %>").click();
            }
            else {
                ShowPopUpDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
            }
        }
        function SetValue(save, value) {

            if (save == '1') {
                document.getElementById('<%=hidPopUpSKU.ClientID %>').value = value;
                document.getElementById('<%=txtSKU.ClientID %>').value = value;
                $("#<%= AddNew.ClientID %>").click();
            }

        }
    
    </script>
</asp:content>