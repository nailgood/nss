<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="" CodeFile="related-swatch.aspx.vb" Inherits="admin_store_items_relatedswatch"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
	<script language="javascript">
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
		InitQueryCode('LookupField', '/admin/ajax.aspx?f=DisplaySwatches&q=', MyCallback);
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
	</script>
	<h4>Related Swatches for '<asp:label ID="lblItemName" Runat="server" />'</h4>
	<br>
	<a href="default.aspx?<%=params%>">лл Go Back To Item List</a>
	<p><b>Add Related Swatch</b>
		<table cellSpacing="2" cellPadding="3" border="0">
			<TBODY>
				<tr>
					<td class="optional" valign="top"><b>Item search</b></td>
					<td class="field" width="400">
						Please enter the first few characters of the swatch name that belongs to this item<br>
						<input type="text" id="LookupField" name="LookupField" style="BORDER-RIGHT:#999999 1px solid; PADDING-RIGHT:4px; BORDER-TOP:#999999 1px solid; PADDING-LEFT:4px; BORDER-LEFT:#999999 1px solid; WIDTH:360px; BORDER-BOTTOM:#999999 1px solid">
						<input type="hidden" name="ItemId">
						<p>
							<span id="ItemInfo" style="FONT-WEIGHT:bold; WIDTH:300px; COLOR:red; HEIGHT:40px"></span>
							<br>
						Please click the "Add Related Swatch" button below to add the swatch selected to this item"
						<p>
							<asp:button id="btnAdd" runat="server" Text="Add Related Swatch" cssClass="btn" Enabled="False"></asp:button></p>
					</td>
					<td valign="top">
					</td>
				</tr>
			</TBODY>
		</table>
	<p>
		<b>View/Update Existing Related Swatches</b>
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
									<CC:ConfirmLink enableviewstate=False Message="Are you sure that you want to remove this item?" runat="server" NavigateUrl= '<%# "CollectionDelete.aspx?sType=Swatch&ParentId=" & DataBinder.Eval(Container.DataItem, "ParentId") & "&ItemId=" & DataBinder.Eval(Container.DataItem, "ItemId") & "&" & params %>' ImageUrl="/includes/theme-admin/images/delete.gif" ID="Confirmlink1"/>
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
									<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "move.aspx?sTable=RelatedSwatch&ACTION=UP&ParentId=" & DataBinder.Eval(Container.DataItem, "ParentId") & "&Id=" & DataBinder.Eval(Container.DataItem, "Id")  & "&" & params %>' ImageUrl="/includes/theme-admin/images/moveup.gif" ID="Hyperlink2"/>
								</ItemTemplate>
							</asp:TemplateColumn>                                
							<asp:TemplateColumn>
								<ItemTemplate>
									<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "move.aspx?sTable=RelatedSwatch&ACTION=DOWN&ParentId=" & DataBinder.Eval(Container.DataItem, "ParentId") & "&Id=" & DataBinder.Eval(Container.DataItem, "Id")  & "&" & params %>' ImageUrl="/includes/theme-admin/images/movedown.gif" ID="Hyperlink3"/>
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
		<P>There are currently no related swatches for this item.</asp:placeholder>
</asp:content>