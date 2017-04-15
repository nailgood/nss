<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="" CodeFile="order-view.aspx.vb" Inherits="admin_store_orders_order_view"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	<h4>Store Order Administration - View Order#
		<%= DisplayOrderNo %> <% if dbOrder.IsReturned then %>- <span class="red">Return Order</span><%end if %>
	</h4>
	<table width="753" cellspacing="0" cellpadding="0" border="0">
		<tr valign="top">
			<td width="667">
				<br>
				<table width="667" cellspacing="0" cellpadding="0" border="0">
					<tr valign="top">
						<td width="272">
							<img src="/images/assets/co-billing-details.gif" width="272" height="26" border="0" alt="billing details"><br>
							<!-- biling details -->
							<div style="MARGIN-TOP:5px;MARGIN-LEFT:14px">
								<div class="bldblktlv">bill to:</div>
								<div class="blkten"><%=dbOrder.BillToName & " " & dbOrder.BillToName2%></div>
								<div class="blkten"><%=dbOrder.BillToAddress%></div>
								<% 	If Not dbOrder.BillToAddress2 = Nothing Then%>
								<div class="blkten"><%=dbOrder.BillToAddress2%></div>
								<% end if %>
								<div class="blkten"><%=dbOrder.BillToCity%>
									,
									<%=dbOrder.BillToCounty%>
									<%=dbOrder.BillToZipcode%>
								</div>
								<div class="blkten"><%=dbOrder.BillToPhone%></div>
								<div class="blkten"><%=dbOrder.Email%></div>
							</div>
						</td>
						<td width="395" valign="top">
							<!-- reminder box -->
							<div align="right" class="bldblktlv">Order#:
								<%=DisplayOrderNo%>
							</div>
							<br>
						</td>
					</tr>
				</table>
				<asp:Repeater id="rptRecipients" Runat="server">
					<ItemTemplate>
						<!-- this group of items is for... -->
						<table height="34" cellspacing="0" cellpadding="0" border="0">
							<tr>
								<td class="forlinename">
									<nobr>order details:
										<%#DataBinder.Eval(Container.DataItem, "NickName") %>
									</nobr>
								</td>
								<td>
								</td>
								<td class="blkten">
									<%#DataBinder.Eval(Container.DataItem, "Address1") %>
									<%# iif(IsDBNull(DataBinder.Eval(Container, "DataItem.Address2")),", ", " " & DataBinder.Eval(Container, "DataItem.Address2") & ", ") %>
									<%#DataBinder.Eval(Container.DataItem, "City") %>
									,
									<%#DataBinder.Eval(Container.DataItem, "State") %>
									<%#DataBinder.Eval(Container.DataItem, "Zip") %>
								</td>
							</tr>
						</table>
						<table width="667" cellspacing="0" cellpadding="0" border="0" style="margin-bottom:10px">
							<!-- table header -->
							<tr>
								<td width="84" class="tblehdr">
									&nbsp;
								</td>
								<td width="226" class="tblehdr">
									item
								</td>
								<td width="99" class="tblehdr">
									qty.
								</td>
								<td width="143" class="tblehdr">
									price
								</td>
								<td width="115" class="tblehdr">
									total
								</td>
							</tr>
							<!-- item line -->
							<tr>
								<td colspan="5" width="667" height="10">
									<img src="/includes/theme-admin/images/spacer.gif" width="667" height="10" border="0" alt="" /><br />
								</td>
							</tr>
							<asp:Repeater id="rptBag" Runat="server">
								<ItemTemplate>
									<tr valign="top">
										<td align="center">
											<input type=hidden runat=server id=hdnId value='<%# DataBinder.Eval(Container, "DataItem.CartItemId")%>' NAME="hdnId"/>
											<a href='/store/item.aspx?i=<%# DataBinder.Eval(Container, "DataItem.CartItemId") %>'>
												<img width=58 height=58 border=0 src='/assets/items/thumbnails/<%# DataBinder.Eval(Container, "DataItem.Image")%>' class='imgbdr' /><br />
											</a>
										</td>
										<td>
											<div class="blkten">
												<span class="bldblkten"><a href='/store/item.aspx?i=<%# DataBinder.Eval(Container, "DataItem.CartItemId") %>'>
														<%# DataBinder.Eval(Container, "DataItem.ItemName") %>
													</a></span>
											</div>
											<div class="blkten">
												product id <span class="bldblkten">
													<%# DataBinder.Eval(Container, "DataItem.SKU") %>
													<asp:Literal ID="ltlBackorderDate" Runat="server"/>
												</span>
											</div>
											<div id="divGiftWrap" class="blkten" style="margin:12px 0 0 18px;" runat="server" visible="false">
												gift wrap item
											</div>
										</td>
										<td class="bldblkelv">
											<%# DataBinder.Eval(Container, "DataItem.Quantity") %>
										</td>
										<td class="bldblkten">
											<%# FormatCurrency(DataBinder.Eval(Container, "DataItem.Price")) %>
										</td>
										<td class="bldblkten">
											<%# FormatCurrency(DataBinder.Eval(Container, "DataItem.Quantity") * DataBinder.Eval(Container, "DataItem.Price")) %>
										</td>
									</tr>
									<%# iif(NOT Container.DataItem("GiftCardToName") Is DBNull.Value OR NOT Container.DataItem("GiftCardFromName") Is DBNull.Value OR NOT Container.DataItem("GiftCardMessage1") Is DBNull.Value OR NOT Container.DataItem("GiftCardMessage2") Is DBNull.Value,"<tr><td>&nbsp;</td><td colspan=6><div class=""blkten"" style=""margin-top:6px; margin-bottom:4px; ""><b>To</b>: " & Container.DataItem("GiftCardToName") & " - <b>From</b>: " & Container.DataItem("GiftCardFromName") & "<br><b>Line 1:</b> " & Container.DataItem("GiftCardMessage1") & " - <b>Line 2:</b> " & Container.DataItem("GiftCardMessage2") & "</div></td></tr>","") %>
								</ItemTemplate>
								<SeparatorTemplate>
									<!-- thin divider -->
									<tr>
										<td colspan="5" width="667" height="21">
											<img src="/images/assets/graypix.gif" width="667" height="1" border="0" vspace="10" alt="" /><br />
										</td>
									</tr>
								</SeparatorTemplate>
								<footertemplate>
								</footertemplate>
							</asp:Repeater>
							<!-- thick divider -->
							<tr runat="server" id="trDivider">
								<td colspan="5" width="667" height="22">
									<img src="/images/assets/graypix.gif" width="667" height="2" border="0" vspace="10" alt="" /><br />
								</td>
							</tr>
							<!-- subtotal line -->
							<tr runat="server" id="trSubtotal">
								<td width="552" colspan="4" align="right" class="bldblkelv" style="padding-right:6px;">
									sub total:&nbsp;
								</td>
								<td width="115" class="bldblkelv">
									<%#FormatCurrency(DataBinder.Eval(Container.DataItem, "SubTotal")) %>
								</td>
							</tr>
							<tr id="trGiftWrapping" runat="server">
								<td width="552" colspan="4" align="right" class="blkelv" style="padding-right:6px;">
									gift wrapping:&nbsp;
								</td>
								<td width="115" class="bldblkelv">
									<%#FormatCurrency(DataBinder.Eval(Container.DataItem, "GiftWrapping")) %>
								</td>
							</tr>
							<tr runat="server" id="trShipping">
								<td width="552" colspan="4" align="right" class="blkelv" style="padding-right:6px;">
									delivery and handling:&nbsp;
								</td>
								<td width="115" class="bldblkelv">
									<%#FormatCurrency(DataBinder.Eval(Container.DataItem, "Shipping")) %>
								</td>
							</tr>
							<tr id="trDeliveryUpgrade" runat="server">
								<td width="552" colspan="4" align="right" class="blkelv" style="padding-right:6px;">
									optional delivery upgrade:&nbsp;
								</td>
								<td width="115" class="bldblkelv">
									<%#FormatCurrency(DataBinder.Eval(Container.DataItem, "DeliveryUpgrade")) %>
								</td>
							</tr>
							<tr id="trDeliverySurcharge" runat="server">
								<td width="552" colspan="4" align="right" class="blkelv" style="padding-right:6px;">
									delivery surcharge:&nbsp;
								</td>
								<td width="115" class="bldblkelv">
									<%#FormatCurrency(DataBinder.Eval(Container.DataItem, "DeliverySurcharge")) %>
								</td>
							</tr>
							<tr id="trOffshoreShipping" runat="server">
								<td width="552" colspan="4" align="right" class="blkelv" style="padding-right:6px;">
									offshore surcharge:&nbsp;
								</td>
								<td width="115" class="bldblkelv">
									<%#FormatCurrency(DataBinder.Eval(Container.DataItem, "OffshoreShipping")) %>
								</td>
							</tr>
							<tr runat="server" id="trTax">
								<td width="552" colspan="4" align="right" class="blkelv" style="padding-right:6px;">
									tax:&nbsp;
								</td>
								<td width="115" class="bldblkelv">
									<%#FormatCurrency(DataBinder.Eval(Container.DataItem, "Tax")) %>
								</td>
							</tr>
							<tr runat="server" id="trGrandTotal">
								<td width="552" colspan="4" align="right" class="bldblkelv" style="padding-right:6px;">
									grand total:&nbsp;
								</td>
								<td width="115" class="bldblkelv">
									<%#FormatCurrency(DataBinder.Eval(Container.DataItem, "Total")) %>
								</td>
							</tr>
						</table>
					</ItemTemplate>
				</asp:Repeater>
				<!-- grand total table (careful here, there's a lot going on to make these columns line up properly) -->
				<img src="/images/assets/blackpix.gif" width="667" height="2" border="0" vspace="3" alt=""><br>
				<table cellspacing="0" cellpadding="0" border="0" style="MARGIN-BOTTOM:10px">
					<tr>
						<td width="550">
							<img src="/includes/theme-admin/images/spacer.gif" width="550" height="1" alt=""><br>
						</td>
						<td>
						</td>
					</tr>
					<tr>
						<td align="right" class="bldblkten">
							total:&nbsp;
						</td>
						<td align="right" class="bldblkten">
							<%=FormatCurrency(dbOrder.BaseSubTotal) %>
						</td>
					</tr>
					<tr>
						<td align="right" class="blkten">
							delivery and handling:&nbsp;
						</td>
						<td align="right" class="blkten">
							<%=FormatCurrency(dbOrder.Shipping) %>
						</td>
					</tr>
					<tr>
						<td align="right" class="blkten">
							tax:&nbsp;
						</td>
						<td align="right" class="blkten">
							<%=FormatCurrency(dbOrder.Tax) %>
						</td>
					</tr>
					<tr>
						<td colspan="2" class="bldblkthr" align="right">
							<span class="thkblklntop" style="PADDING-TOP:4px">grand total:
								<%=FormatCurrency(dbOrder.Total) %>
							</span>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<table width="667" cellspacing="0" cellpadding="0" border="0" class="graybox">
		<tr>
			<td colspan="5" width="667" height="7">
				<img src="/includes/theme-admin/images/spacer.gif" width="667" height="7" alt=""><br>
			</td>
		</tr>
		<tr valign="top">
			<td width="274">
				<img src="/images/assets/co-payment-details.gif" width="274" height="28" border="0" alt="payment details"><br>
				<!-- cc card -->
				<table width="240" cellspacing="6" cellpadding="0" border="0" style="MARGIN-LEFT:9px" class="blkten">
					<tr>
						<td width="100" class="bldblkten">
							<span id="labelCardTypeId" runat="server" class="bldblkten">credit card type</span>
						</td>
						<td width="140">
							<asp:Literal id="ltlCardType" runat="server" />
						</td>
					</tr>
					<tr>
						<td class="bldblkten">
							<span id="labelCardHolderName" runat="server" class="bldblkten">name on card</span>
						</td>
						<td>
							<asp:Literal id="ltlCardHolderName" runat="server" />
						</td>
					</tr>
					<tr>
						<td class="bldblkten">
							<span id="labelCardNumber" runat="server" class="bldblkten">card number</span>
						</td>
						<td>
							<asp:Literal id="ltlCardNumber" runat="server" />
						</td>
					</tr>
					<tr>
						<td class="bldblkten">
							<span id="labelCIDNumber" runat="server" class="bldblkten">CID number</span>
						</td>
						<td>
							<asp:Literal id="ltlCIDNumber" runat="server" />
						</td>
					</tr>
					<tr>
						<td class="bldblkten">
							<span id="labelExpirationDate" runat="server" class="bldblkten">expiration</span>
						</td>
						<td>
							<asp:Literal id="ltlExpirationDate" runat="server" />
						</td>
					</tr>
				</table>
			</td>
			<td width="7" class="drkgray">
				<img src="/includes/theme-admin/images/spacer.gif" width="1" height="7" alt=""><br>
			</td>
		</tr>
		<tr>
			<td colspan="5" width="667" height="10">
				<img src="/includes/theme-admin/images/spacer.gif" width="667" height="10" alt=""><br>
			</td>
		</tr>
	</table>
	<h4>Change Billing Details</h4>
	<table cellSpacing="2" cellPadding="3" border="0">
		<tr>
			<td colSpan="3"><span class="red">red color</span> - required fields</td>
		</tr>
		<tr>
			<td class="required"><b>First name:</b></td>
			<td class="field"><asp:textbox id="BillingFirstName" runat="server" Width="140" maxlength="42" columns="20" cssclass="sbox"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ErrorMessage="First name is required"
					ControlToValidate="BillingFirstName" Display="Dynamic" /></td>
		</tr>
		<tr>
			<td class="required"><b>Last name:</b></td>
			<td class="field"><asp:textbox id="BillingLastName" runat="server" Width="140" maxlength="42" columns="20" cssclass="sbox" /></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="server" ErrorMessage="Last name is required"
					ControlToValidate="BillingLastName" Display="Dynamic" /></td>
		</tr>
		<tr>
			<td class="required"><b>Company:</b></td>
			<td class="field"><asp:textbox id="BillingCompany" runat="server"  maxlength="42" columns="40" cssclass="sbox" /></td>
			<td>&nbsp;</td>
		</tr>
		<tr>
			<td class="required"><b>Address 1</b></td>
			<td class="field"><asp:textbox id="BillingAddress1" runat="server"  maxlength="42" columns="40" cssclass="sbox"></asp:textbox><br>
			</td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator4" runat="server" ErrorMessage="Address 1 is required"
					ControlToValidate="BillingAddress1" Display="Dynamic" /></td>
		</tr>
		<tr>
			<td class="optional"><b>Address 2</b></td>
			<td class="field"><asp:textbox id="BillingAddress2" runat="server"  maxlength="42" columns="40" cssclass="sbox"></asp:textbox></td>
			<td>&nbsp;</td>
		</tr>
		<tr>
			<td class="required"><b>City:</b></td>
			<td class="field"><asp:textbox id="BillingCity" runat="server" maxlength="42" columns="40" cssclass="sbox"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator5" runat="server" cssClass="blkten" ErrorMessage="City is required"
					ControlToValidate="BillingCity" Display="Dynamic" /></td>
		</tr>
		<tr>
			<td class="required"><b>State</b></td>
			<td class="field"><CC:DropDownListEx runat="server" id="BillingState" /></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator6" runat="server" cssClass="blkten" ErrorMessage="State is required"
					ControlToValidate="BillingState" Display="Dynamic" /></td>
		</tr>
		<tr>
			<td class="required"><b>Zip:</b></td>
			<td class="field"><CC:Zip id="BillingZip" runat="server" cssclass="sbox"></CC:Zip></td>
			<td><CC:ZipValidator id="BillingZipValidator" runat="server" cssClass="blkten" ErrorMessage="Zip is required"
					ControlToValidate="BillingZip" Display="Dynamic" />
			</td>
		</tr>
		<tr>
			<td class="optional"><b>Phone:</b></td>
			<td class="field"><CC:Phone id="BillingPhone" runat="server" cssclass="sbox"></CC:Phone></td>
			<td><CC:PhoneValidator id="PhoneValidator1" runat="server" cssClass="blkten" ErrorMessage="Phone number is required"
					ControlToValidate="BillingPhone" Display="Dynamic" /></td>
		</tr>
		<tr>
			<td class="required"><b>Email:</b></td>
			<td class="field"><asp:textbox id="Email" runat="server" maxlength="142" columns="40" /></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator7" runat="server" cssClass="blkten" ErrorMessage="Email is required"
					ControlToValidate="Email" Display="Dynamic" />
				<CC:EMAILVALIDATOR id="EmailValidator" runat="server" cssClass="blkten" ErrorMessage="Valid email address required"
					ControlToValidate="Email" Display="Dynamic" /></td>
		</tr>
		<tr>
			<td colspan="3">
			<asp:Button ID="btnCancel" Runat="server" Text="Return to Order List" CssClass="btn" CausesValidation="False" /> &nbsp;&nbsp; 
			<CC:Confirmbutton Message="Are you sure want to update the billing information for this order?" ID="btnSave" Runat="server" CssClass="btn" text="Update Billing Details" />
			</td>
		</tr>
	</table>
</asp:content>