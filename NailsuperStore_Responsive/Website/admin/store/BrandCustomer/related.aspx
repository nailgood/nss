<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="" CodeFile="related.aspx.vb" Inherits="admin_store_itemenable_related"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">    
	<script language="javascript">
	<!--
	
	function MyCallback(MemberId) {
		document.getElementById('MemberId').value = MemberId;
		GetItemEnableInfo();
	}
	function MyEmail(MemberId) {
		document.getElementById('MemberId').value = MemberId;
		GetItemEnableInfo();
	}

  
function SetType(Type1)
{     
    if (Type1 == "1")
    {
            document.getElementById('LookupEmail').value = "";
            InitQueryCode('LookupField', '/admin/ajax.aspx?f=DisplayItemEnable&Type=user&q=', 'varUser');
    }
    else
    {
            document.getElementById('LookupField').value = "";
           InitQueryCode('LookupEmail', '/admin/ajax.aspx?f=DisplayItemEnable&Type=mail&q=', 'varEmail');
    }
   
}
	function InitializeQuery() {
	
	InitQueryCode('LookupField', '/admin/ajax.aspx?f=DisplayItemEnable&Type=user&q=', 'varUser');
	
			}
	
		function InitializeQuery1() {
	
	InitQueryCode('LookupEmail', '/admin/ajax.aspx?f=DisplayItemEnable&Type=mail&q=', 'varEmail');
	}
	function GetItemEnableInfo() {
		var sItem, sConn;
		
		xml = getXMLHTTP();
		if(xml){
			xml.open("GET","/admin/Ajax.aspx?f=GetItemEnableInfo&MemberId=" + getValue(document.getElementById('MemberId')),true);
			xml.onreadystatechange = function() {
				if(xml.readyState==4 && xml.responseText) {
					if (xml.responseText.length > 0) {
						aData = xml.responseText.split('|');
						
						sItem = '';
						sItem += 'CustomerNo: ' + aData[0] + '<br>';
						sItem += 'MemberId: ' + aData[1];
											
						document.getElementById('ItemInfo').innerHTML = sItem;
					} else {
						sItem = '';
					}	
				}
			}	
			xml.send(null);
		}
		
		if (!isEmptyField(document.getElementById('MemberId'))) 
		{
			document.getElementById('<%=btnAdd.ClientID%>').disabled = false;
		} 
		else 
		{
			document.getElementById('<%=btnAdd.ClientID%>').disabled = true;
		}
	}	
	//-->
	</script>
	<h4>Permission buy brand <asp:label ID="lblItemName" Runat="server" /></h4>
	<br>
	<a href="default.aspx?<%=params%>"></a>
	<p><b>Add Customer and Brand Item</b>
		<table cellSpacing="2" cellPadding="3" border="0">
			<TBODY>
				<tr>
					<td class="optional" valign="top"><b>UserName search</b></td>
					<td class="field" width="400">
						<input type="text" id="LookupField" name="LookupField" onkeypress="javascript:SetType('1')" AutoCompleteType="Disabled" autocomplete="off" style="BORDER-RIGHT:#999999 1px solid; PADDING-RIGHT:4px; BORDER-TOP:#999999 1px solid; PADDING-LEFT:4px; BORDER-LEFT:#999999 1px solid; WIDTH:360px; BORDER-BOTTOM:#999999 1px solid">
						<input type="hidden" name="MemberId" id="MemberId">


					</td>
					<td valign="top">
					</td>
				</tr>
				<tr>
				    <td class="optional" valign="top"><b>Email search</b></td>
				    <td class="field" width="400">
				    <input type="text" id="LookupEmail" name="LookupEmail" onkeypress="javascript:SetType('0')" AutoCompleteType="Disabled" autocomplete="off" style="BORDER-RIGHT:#999999 1px solid; PADDING-RIGHT:4px; BORDER-TOP:#999999 1px solid; PADDING-LEFT:4px; BORDER-LEFT:#999999 1px solid; WIDTH:360px; BORDER-BOTTOM:#999999 1px solid">
				    
				     <input type="hidden" runat="server" value="" id="UserType" />
					   </td>
				    <td valign="top">
					</td>
				</tr>
				<tr>
				<td colspan = "3">
										<p>
							<span id="ItemInfo" style="FONT-WEIGHT:bold; WIDTH:300px; COLOR:red; HEIGHT:40px"></span>
				</td>
				</tr>
				<tr>
				<td class="optional" valign="top">Brand</td>
				<td class="field" width="400"><asp:DropDownList ID="drBrand" runat="server" /></td>
				</tr>
				<tr><td colspan="2" class="optional" valign="top">
				<p>
							<asp:button id="btnAdd" runat="server" Text="Add Customer and Brand" cssClass="btn" Enabled="True"></asp:button></p>
				</td></tr>
			</TBODY>
		</table>
	<p>
		<b>View/Update Existing Customer and Brand</b>
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
						    <HeaderTemplate>
									Delete
								</HeaderTemplate>
								<ItemTemplate>
									<CC:ConfirmLink enableviewstate=False Message="Are you sure that you want to remove this item?" runat="server" NavigateUrl= '<%# "delete.aspx?EnableId=" & DataBinder.Eval(Container.DataItem, "EnableId") & "&" & params %>' ImageUrl="/includes/theme-admin/images/delete.gif" ID="Confirmlink1"/>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn SortExpression="Y">
								<HeaderTemplate>
									Customer No
								</HeaderTemplate>
								<ItemTemplate>
									<asp:Label enableviewstate=False runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CustomerNo") %>' ID="Label3">
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>
							
					        <asp:TemplateColumn SortExpression="Y">
								<HeaderTemplate>
									Customer Name
								</HeaderTemplate>
								<ItemTemplate>
									<asp:Label enableviewstate=False runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Name") %>' ID="Label1">
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>              
							    <asp:TemplateColumn SortExpression="Y">
								<HeaderTemplate>
									Brand Name
								</HeaderTemplate>
								<ItemTemplate>
									<asp:Label enableviewstate=False runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BrandName") %>' ID="Label2">
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>  
														             
							    <asp:TemplateColumn SortExpression="Y">
								<HeaderTemplate>
									Email
								</HeaderTemplate>
								<ItemTemplate>
									<asp:Label enableviewstate=False runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Email") %>' ID="Label4">
									</asp:Label>
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
		<P>There are currently no coordinating items for this item.</asp:placeholder>
</asp:content>