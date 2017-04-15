<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="" CodeFile="default.aspx.vb" Inherits="admin_store_statusmessage__default" ValidateRequest="false" %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	<h4>Modify Store Status Messages</h4>
	You may use the following markers inside the status messages to show dynamic content:<br><br>
	
	<span class=smaller>%%deliverytime%% - Delivery Time<br><br>
	%%shipmentdate%% - Shipment Date<br><br>
	%%qtyonhand%% - Qty. On Hand<br><br><br>(example: We currently only have %%qtyonhand%% items remaining in inventory)<br><br></span>
	<asp:Panel runat="server" ID="messagelbl">
	<table cellSpacing="2" cellPadding="3" border="0">
		<tr>
			<td class="required"><b>Dropship:</b></td>
			<td class="field" width=300><asp:textbox id="txtMessage1" runat="server" maxlength="150" columns="100"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator1" runat="server" ErrorMessage="Message is blank" ControlToValidate="txtMessage1" Display="Dynamic"></asp:requiredfieldvalidator></td>
		</tr>
		<tr><td colspan=3><b><u>Short Discontinued Status Messages</b></u></td></tr>
		<tr>
			<td class="required"><b>Qty. on hand > 0:</b></td>
			<td class="field" width=300><asp:textbox id="txtMessage2" runat="server" maxlength="150" columns="100"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Message is blank" ControlToValidate="txtMessage2" Display="Dynamic"></asp:requiredfieldvalidator></td>
		</tr>
		<tr>
			<td class="required"><b>Qty. on hand <= 0:</b></td>
			<td class="field" width=300><asp:textbox id="txtMessage3" runat="server" maxlength="150" columns="100"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="server" ErrorMessage="Message is blank" ControlToValidate="txtMessage3" Display="Dynamic"></asp:requiredfieldvalidator></td>
		</tr>
		<tr>
			<td class="required"><b>Qty. on hand with shipment date:</b></td>
			<td class="field" width=300><asp:textbox id="txtMessage4" runat="server" maxlength="150" columns="100"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator4" runat="server" ErrorMessage="Message is blank" ControlToValidate="txtMessage4" Display="Dynamic"></asp:requiredfieldvalidator></td>
		</tr>
		<tr><td colspan=3><b><u>Short Non-Discontinued Status Messages</b></u></td></tr>				
		<tr>
			<td class="required"><b>Qty. on hand <= 0<br>Shipment date after today:</b></td>
			<td class="field" width=300><asp:textbox id="txtMessage6" runat="server" maxlength="150" columns="100"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator6" runat="server" ErrorMessage="Message is blank" ControlToValidate="txtMessage6" Display="Dynamic"></asp:requiredfieldvalidator></td>
		</tr>
		<tr>
			<td class="required"><b>Qty. on hand <= 0<br>Shipment date before today:</b></td>
			<td class="field" width=300><asp:textbox id="txtMessage5" runat="server" maxlength="150" columns="100"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator5" runat="server" ErrorMessage="Message is blank" ControlToValidate="txtMessage5" Display="Dynamic"></asp:requiredfieldvalidator></td>
		</tr>
		<tr>
			<td class="required"><b>Qty. on hand > 0:</b></td>
			<td class="field" width=300><asp:textbox id="txtMessage7" runat="server" maxlength="150" columns="100"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator7" runat="server" ErrorMessage="Message is blank" ControlToValidate="txtMessage7" Display="Dynamic"></asp:requiredfieldvalidator></td>
		</tr>
		<tr><td colspan=3><b><u>Long Status Messages for Qty On Hand Available</b></u></td></tr>								
		<tr>
			<td class="required"><b>R1:</b></td>
			<td class="field" width=300><asp:textbox id="txtMessage8" runat="server" maxlength="150" columns="100"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator8" runat="server" ErrorMessage="Message is blank" ControlToValidate="txtMessage8" Display="Dynamic"></asp:requiredfieldvalidator></td>
		</tr>		
		<tr>
			<td class="required"><b>C3 with qty. on hand less then or exactly 10:</b></td>
			<td class="field" width=300><asp:textbox id="txtMessage9" runat="server" maxlength="150" columns="100"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator9" runat="server" ErrorMessage="Message is blank" ControlToValidate="txtMessage9" Display="Dynamic"></asp:requiredfieldvalidator></td>
		</tr>		
		<tr>
			<td class="required"><b>C3 with qty. on hand more than 10:</b></td>
			<td class="field" width=300><asp:textbox id="txtMessage10" runat="server" maxlength="150" columns="100"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator10" runat="server" ErrorMessage="Message is blank" ControlToValidate="txtMessage10" Display="Dynamic"></asp:requiredfieldvalidator></td>
		</tr>		
		<tr>
			<td class="required"><b>T1:</b></td>
			<td class="field" width=300><asp:textbox id="txtMessage11" runat="server" maxlength="150" columns="100"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator11" runat="server" ErrorMessage="Message is blank" ControlToValidate="txtMessage11" Display="Dynamic"></asp:requiredfieldvalidator></td>
		</tr>		
		<tr>
			<td class="required"><b>T2:</b></td>
			<td class="field" width=300><asp:textbox id="txtMessage12" runat="server" maxlength="150" columns="100"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator12" runat="server" ErrorMessage="Message is blank" ControlToValidate="txtMessage12" Display="Dynamic"></asp:requiredfieldvalidator></td>
		</tr>	
		<tr>
			<td class="required"><b>K1:</b></td>
			<td class="field" width=300><asp:textbox id="txtMessage13" runat="server" maxlength="150" columns="100"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator13" runat="server" ErrorMessage="Message is blank" ControlToValidate="txtMessage13" Display="Dynamic"></asp:requiredfieldvalidator></td>
		</tr>
		<tr><td colspan=3><b><u>Long Status Messages for Qty On Hand Unavailable</b></u></td></tr>								
		<tr>
			<td class="required"><b>R1 with shipment date after today:</b></td>
			<td class="field" width=300><asp:textbox id="txtMessage14" runat="server" maxlength="150" columns="100"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator14" runat="server" ErrorMessage="Message is blank" ControlToValidate="txtMessage14" Display="Dynamic"></asp:requiredfieldvalidator></td>
		</tr>													
		<tr>
			<td class="required"><b>R1 with shipment date on or before today:</b></td>
			<td class="field" width=300><asp:textbox id="txtMessage15" runat="server" maxlength="150" columns="100"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator15" runat="server" ErrorMessage="Message is blank" ControlToValidate="txtMessage15" Display="Dynamic"></asp:requiredfieldvalidator></td>
		</tr>													
		<tr>
			<td class="required"><b>C3 with shipment date after today:</b></td>
			<td class="field" width=300><asp:textbox id="txtMessage16" runat="server" maxlength="150" columns="100"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator16" runat="server" ErrorMessage="Message is blank" ControlToValidate="txtMessage16" Display="Dynamic"></asp:requiredfieldvalidator></td>
		</tr>													
		<tr>
			<td class="required"><b>C3 with shipment date on or before today:</b></td>
			<td class="field" width=300><asp:textbox id="txtMessage17" runat="server" maxlength="150" columns="100"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator17" runat="server" ErrorMessage="Message is blank" ControlToValidate="txtMessage17" Display="Dynamic"></asp:requiredfieldvalidator></td>
		</tr>													
		<tr>
			<td class="required"><b>K1 with shipment date after today:</b></td>
			<td class="field" width=300><asp:textbox id="txtMessage18" runat="server" maxlength="150" columns="100"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator18" runat="server" ErrorMessage="Message is blank" ControlToValidate="txtMessage18" Display="Dynamic"></asp:requiredfieldvalidator></td>
		</tr>													
		<tr>
			<td class="required"><b>K1 with shipment date on or before today:</b></td>
			<td class="field" width=300><asp:textbox id="txtMessage19" runat="server" maxlength="150" columns="100"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator19" runat="server" ErrorMessage="Message is blank" ControlToValidate="txtMessage19" Display="Dynamic"></asp:requiredfieldvalidator></td>
		</tr>													
		<tr>
			<td class="required"><b>T1, T2:</b></td>
			<td class="field" width=300><asp:textbox id="txtMessage20" runat="server" maxlength="150" columns="100"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator20" runat="server" ErrorMessage="Message is blank" ControlToValidate="txtMessage20" Display="Dynamic"></asp:requiredfieldvalidator></td>
		</tr>	
		<tr>
			<td class="required"><b>C1:</b></td>
			<td class="field" width=300><asp:textbox id="txtMessage21" runat="server" maxlength="150" columns="100"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator21" runat="server" ErrorMessage="Message is blank" ControlToValidate="txtMessage21" Display="Dynamic"></asp:requiredfieldvalidator></td>
		</tr>	
		<tr>
			<td class="required"><b>KC:</b></td>
			<td class="field" width=300><asp:textbox id="txtMessage22" runat="server" maxlength="150" columns="100"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator22" runat="server" ErrorMessage="Message is blank" ControlToValidate="txtMessage22" Display="Dynamic"></asp:requiredfieldvalidator></td>
		</tr>	
		<tr>
			<td class="required"><b>J1:</b></td>
			<td class="field" width=300><asp:textbox id="txtMessage23" runat="server" maxlength="150" columns="100"></asp:textbox></td>
			<td><asp:requiredfieldvalidator id="Requiredfieldvalidator23" runat="server" ErrorMessage="Message is blank" ControlToValidate="txtMessage23" Display="Dynamic"></asp:requiredfieldvalidator></td>
		</tr>	
	</table></asp:Panel>
	<p>
	<asp:button id="Save" runat="server" Text="Update Messages" cssClass="btn"></asp:button>
</asp:content>