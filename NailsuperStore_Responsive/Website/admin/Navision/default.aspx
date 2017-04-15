<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Navision Import" CodeFile="default.aspx.vb" Inherits="admin_navision_index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
<script type="text/javascript">
<!--
var isFailed;

function loopStatus(fileNames,ids,buttons,texts) {
	var btn, txt;
	btn = buttons.split(",");
	txt = texts.split(",");
	for(var i = 0; i < btn.length; i++) {
		if(document.getElementById(btn[i])) {
			document.getElementById(btn[i]).disabled = true;
		}
	}

	GetImportStatus(fileNames,ids);

	if(typeof(isFailed) == 'undefined') {
		setTimeout("loopStatus('" + fileNames + "','" + ids + "','" + buttons + "','" + texts + "')", 3000); }
	else {
		for(var i = 0; i < btn.length; i++) {
			if(document.getElementById(btn[i]))  {
				document.getElementById(btn[i]).disabled = false;
				document.getElementById(btn[i]).innerText = txt[i];
			}
		}
	}
}

function GetImportStatus(fileNames,ids) {
	if(typeof(isFailed) != 'undefined') return;

	var xml,vals,vals2,divs,divs2,s;
	xml = getXMLHTTP();
	if(xml){
		xml.open("GET","/admin/Ajax.aspx?f=GetImportStatus&FileNames=" + fileNames,true);
		xml.onreadystatechange = function() {
			if(xml.readyState==4 && xml.responseText) {
				if (xml.responseText.length > 0) {
					s = xml.responseText;
					if(s.indexOf('Failed!') != -1) { isFailed = true; }
					if(s.indexOf('ImportSuccess') != -1) { isFailed = true; s = s.replace("ImportSuccess", ""); }

					vals = s.split('[~]');
					divs = ids.split(",");
					document.getElementById('tmpdiv').innerHTML = '';
					for(var i = 0; i < vals.length; i++) {
						divs2 = divs[i].split("|");
						vals2 = vals[i].split("|");
						for(var j = 0; j < vals2.length; j++) { 
							var div = document.getElementById(divs2[j]);
							var html = div.innerHTML + '';
							if(!(html.indexOf('Processing') != -1 && vals2[j].indexOf('Processing') != -1)) {
								document.getElementById('tmpdiv').innerHTML += div.id + ' - ' + vals2[j] + '<br />';
								div.innerHTML = vals2[j]; 
							}
						}
					}
				}
			}
		}
		xml.send(null);
	}
}
//-->
</script>

<h4>Navision Import</h4>

<p><asp:Button runat="server" ID="btnProduct" Text="Run Product Description Import" CssClass="btn" /></p>

<CC:FileUpload runat="server" ID="ccFU" PersistFileName="true" Width="320" style="width:320px;" Folder="/admin/navision/uploads/" /><br />
<CC:FileUploadExtensionValidator runat="server" Extensions="edi" EnableClientScript="false" ControlToValidate="ccFU" ErrorMessage="Invalid file type - EDI files only" Display="none" />
<asp:Literal runat="server" ID="lit" />

<p></p>
<table width="320" border="0" cellspacing="0" cellpadding="0">
<tr>
<td align="left">
<CC:OneClickButton runat="server" ID="btnSubmit" Text="Upload" CssClass="btn" />
</td>
<td align="right">
<asp:Button OnClientClick="return confirm('Are you sure you wish to begin processing all uploaded files?\nThe files will be processed in the order in which they were uploaded.');" runat="server" ID="btnProcess" Text="Process Import Files" CssClass="btn" />
</td>
</tr>
</table>

<p></p>
<CC:GridView id="gvList" CellSpacing="2" gridlines="none" CellPadding="2" runat="server" HeaderText="Last 25 File Uploads" EmptyDataText="No files have been processed." AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom" style="font-size:10px;">
	<AlternatingRowStyle CssClass="alternate"></AlternatingRowStyle>
	<RowStyle CssClass="row"></RowStyle>
	<Columns>
		<asp:BoundField DataField="FileName" HeaderText="File Name" />
		<asp:BoundField DataField="ImportDate" HeaderText="Import Date" Visible="false" />
		<asp:TemplateField HeaderText="Begin Process Time">
			<ItemTemplate>
				<div runat="server" id="divStart"></div>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="End Process Time">
			<ItemTemplate>
				<div runat="server" id="divDate"></div>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Status">
			<ItemTemplate>
				<div runat="server" id="divStatus"></div>
			</ItemTemplate>
		</asp:TemplateField>
	</Columns>
</CC:GridView>

<div id="tmpdiv" style="display:none;"></div>

</asp:content>