<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="" CodeFile="departments.aspx.vb" Inherits="admin_store_tips_categories_departments"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
	<script language="javascript">
	<!--
	    function MyCallback(DepartmentId) {
		document.getElementById('DepartmentId').value = DepartmentId;
		GetDepartmentInfo();
		
	}
	
	if (window.addEventListener) {
		window.addEventListener('load', InitializeQuery, false);
	} else if (window.attachEvent) {
		window.attachEvent('onload', InitializeQuery);
	}

	function InitializeQuery() {
		InitQueryCode('LookupField', '/admin/ajax.aspx?f=DisplayDepartments&q=', MyCallback);
	}
	
	function GetDepartmentInfo() {
		var sDepartment, sConn;
		xml = getXMLHTTP();
		if(xml){
			xml.open("GET","/admin/Ajax.aspx?f=GetDepartmentInfo&DepartmentId=" + getValue(document.getElementById('DepartmentId')),true);
			xml.onreadystatechange = function() {
			    if (xml.readyState == 4 && xml.responseText) {
			        if (xml.responseText.length > 0) {
			            aData = xml.responseText.split('|');

			            sDepartment = '';
			            sDepartment += 'Department Name: ' + aData[1];
			            document.getElementById('DepartmentInfo').innerHTML = sDepartment;
			        } else {
			            sDepartment = '';
			        }
			    }
			}	
			xml.send(null);
		}
		if (!isEmptyField(document.getElementById('DepartmentId'))) {
			document.getElementById('<%=btnAdd.ClientID%>').disabled = false;
		} else {
			document.getElementById('<%=btnAdd.ClientID%>').disabled = true;
		}
	}	
	//-->
	</script>
	<h4>Tip category departments for tip category '<asp:label ID="lblItemName" Runat="server" />'</h4>
	<br>
	<a href="default.aspx?<%=params%>">лл Go Back To Tip Category List</a>
	<p><b>Add Tip Category Department</b>
		<table cellSpacing="2" cellPadding="3" border="0">
			<TBODY>
				<tr>
					<td class="optional" valign="top"><b>Department search</b></td>
					<td class="field" width="400">
						Please enter the first few characters of the department name<br>
						<input type="text" id="LookupField" name="LookupField" style="BORDER-RIGHT:#999999 1px solid; PADDING-RIGHT:4px; BORDER-TOP:#999999 1px solid; PADDING-LEFT:4px; BORDER-LEFT:#999999 1px solid; WIDTH:360px; BORDER-BOTTOM:#999999 1px solid">
						<input type="hidden" name="DepartmentId" id="DepartmentId">
						<p>
							<span id="DepartmentInfo" style="FONT-WEIGHT:bold; WIDTH:300px; COLOR:red; HEIGHT:40px"></span>
							<br>
						Please click the "Add Department" button below to add the department selected as a tip category department
						<p>
							<asp:button id="btnAdd" runat="server" Text="Add Department" cssClass="btn"></asp:button></p>
					</td>
					<td valign="top">
					</td>
				</tr>
			</TBODY>
		</table>
	<p>
		<b>View/Update Existing Tip Category Departments</b>
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
									<CC:ConfirmLink enableviewstate=False Message="Are you sure that you want to remove this item?" runat="server" NavigateUrl= '<%# "itemsdelete.aspx?TipCategoryId=" & DataBinder.Eval(Container.DataItem, "TipCategoryId") & "&Id=" & DataBinder.Eval(Container.DataItem, "DepartmentId") & "&sType=Department&" & params %>' ImageUrl="/images/admin/delete.gif" ID="Confirmlink1"/>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn SortExpression="Y">
								<HeaderTemplate>
									Department Name
								</HeaderTemplate>
								<ItemTemplate>
									<asp:Label enableviewstate=False runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Name") %>' ID="Label1">
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
		<P>There are currently no tip category departments for this tip category.</asp:placeholder>
</asp:content>