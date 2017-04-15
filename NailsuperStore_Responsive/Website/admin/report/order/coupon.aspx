<%@ Page Language="VB" AutoEventWireup="false" validateRequest="false" CodeFile="coupon.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_store_report_coupon" %>
<asp:content ContentPlaceHolderId="ph" ID="Content1" runat="server">

    <script language="javascript">
<!--
	function expandit(objid){
		var span = document.getElementById('SPAN' + objid).style;
		var img  = document.getElementById('IMG' + objid);
		var imgtext = document.getElementById('imgtext');
		if (span.display=="none") {
			span.display="block"
			img.src = img.src.replace(/down/i, "up");
			imgtext.innerText = 'Hide Image';
		} else {
			span.display="none"
			img.src = img.src.replace(/up/i, "down");
			imgtext.innerText = 'View Image';
		}
	}
	function exportXls()
    {	
	 document.getElementById("ctl00_ph_hidexport").value = document.getElementById("Excel").innerHTML
     return true;
    }
    
//-->
function ViewPromotionCustomer(PromotionCode,CustomerNo,Name,UsedTimes)
{
	document.getElementById("div789").style.display = "block";	
	document.getElementById("div789").style.left = parseInt(window.screen.availWidth/2)-parseInt(503/2);
	document.getElementById("div789").style.top = parseInt(window.screen.availHeight/2)-parseInt(7/2);
		
	var htmlTable = "<table width='100%'>"
	htmlTable += "<tr>"
	htmlTable += "<td align='right'>"
	htmlTable += "<u>Promotion Code</u> : "
	htmlTable += "</td>"
	htmlTable += "<td><b>"
	htmlTable += PromotionCode
	htmlTable += "</b></td>"
	htmlTable += "</tr>"
	htmlTable += "<tr>"
	htmlTable += "<td align='right' width='35%'>"
	htmlTable += "<u>Customer No</u> : "
	htmlTable += "</td>"
	htmlTable += "<td>"
	htmlTable += CustomerNo
	htmlTable += "</td>"
	htmlTable += "</tr>"
	htmlTable += "<tr>"
	htmlTable += "<td align='right'>"
	htmlTable += "<u>Customer Name</u> : "
	htmlTable += "</td>"
	htmlTable += "<td>"
	htmlTable += Name
	htmlTable += "</td>"
	htmlTable += "</tr>"	
	htmlTable += "<tr>"
	htmlTable += "<td align='right'>"
	htmlTable += "<u>Used Times</u> : "
	htmlTable += "</td>"
	htmlTable += "<td>"
	htmlTable += UsedTimes
	htmlTable += "</td>"
	htmlTable += "</tr>"	
	
	htmlTable += "</table>"
	document.getElementById("div7899").innerHTML = htmlTable
	
	return false;
}
function Hide_DIV789()
			{      				
										        			        			
        		document.getElementById("div123").style.display='none';      
        		document.getElementById("div789").style.display='none';      			 		        			
			}

</script>


<h4>Order Coupon Report</h4>

<p></p>
<table cellpadding="2" cellspacing="2">
<tr>
<th valign="top">Promotion Code:</th>
<td valign="top" class="field"><asp:textbox id="F_PromotionCode" runat="server" Columns="20" MaxLength="20"></asp:textbox></td>
</tr>


<tr><td style="padding:10px 0px 10px 0px;"><CC:OneClickButton id="btnSearch" Runat="server" Text="Search" cssClass="btn" /></td>
<td style="padding:10px 0px 10px 0px;"><CC:OneClickButton id="btnExcel" Runat="server" Text="Excel" cssClass="btn" /></td>

</tr>
</table>
<div id="Excel">
<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="30" AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>	
		<asp:TemplateField HeaderText="Promotion Code" SortExpression="PromotionCode">
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False NavigateUrl='<%# "coupon-customer.aspx?PromotionCode=" & DataBinder.Eval(Container.DataItem, "PromotionCode") %>' runat="server" text='<%# DataBinder.Eval(Container.DataItem, "PromotionCode") %>'></asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		
        <asp:BoundField SortExpression="UsedTimes" DataField="UsedTimes" HeaderText="Used Times"></asp:BoundField>

	</Columns>
    <HeaderStyle VerticalAlign="Top" />
</CC:GridView>
</div> 
<INPUT id="hidexport" type="hidden" name="hidexport" runat="server"/>
<div onmousedown="dragStart(event, 'div789')" id="div789" style="DISPLAY: none; POSITION: absolute">
	<table cellSpacing="0" cellPadding="0" width="500" align="center" border="0">
		<tr>
			<td><IMG height="7" src="../../store/PromotionsReport/SkinTemplet/images/top.gif" 
                    width="503"></td>
		</tr>
		<tr>
			<td background="../../store/PromotionsReport/SkinTemplet/images/bgr.gif" 
                height="100%">
				<table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
					<tr>
						<td vAlign="top">
							<table height="100%" cellSpacing="0" cellPadding="5" width="100%" border="0">
								<tr>
									<td>
										<table height="100%" cellSpacing="0" cellPadding="2" width="100%" border="0">
											<tr>
												<td width="20" style="WIDTH: 20px"></td>
												<td width="500">
													<DIV id="div7899" style="COLOR: #ff0000"></DIV>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td style="COLOR: #ff3300" vAlign="top" align="right">
										<DIV><SPAN onmouseover="this.style.cursor='hand';this.style.textDecoration='underline'" onclick="Hide_DIV789()"
												onmouseout="this.style.textDecoration='none'"><b>close </b></SPAN>&nbsp; 
											&nbsp; &nbsp;
										</DIV>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td><IMG height="7" 
                    src="../../store/PromotionsReport/SkinTemplet/images/bottom.gif" width="503"></td>
		</tr>
	</table>
</div>
</asp:content>