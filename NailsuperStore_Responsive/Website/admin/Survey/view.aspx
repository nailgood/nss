<%@ Page Language="VB" AutoEventWireup="false" CodeFile="view.aspx.vb" Inherits="admin_Survey_view" title="Untitled Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>View Customer Survey</title>

    <script language="jscript">
        window.name = "modal";
    </script>

      <script type="text/javascript" src="../../../../includes/theme-admin/scripts/Browser.js">
    </script>

   <link href="../../../../includes/theme-admin/css/admin.css" rel="stylesheet" type="text/css" />
   

</head>
<body onload="CheckTarget();">
    <form id="form1" runat="server">
    <table style="width:800px;">
        <tr>
            <td valign="top" style="width:100px;"><b>Customer Name:</b></td>
            <td><asp:label id="lbCustName" runat="server"></asp:label></td>
        </tr>
        <tr>
            <td valign="top"><b>Customer Email:</b></td>
            <td ><asp:label id="lbCustEmail" runat="server"></asp:label></td>
        </tr>
        <tr runat="server" ID="trMember">
            <td><b>Memeber:</b></td>
            <td><asp:Literal runat="server" ID="ltrMember"></asp:Literal></td>
        </tr>
        <tr>
            <td><b>Posted Date:</b></td>
            <td><asp:Label runat="server" ID="lbDate"></asp:Label></td>
        </tr>        
        <tr>
            <td valign="top"><b>Questions:</b></td>
            <td>
                <asp:DataList runat="server" id="dlQuestion" CssClass="tblQuestion">
		            <ItemTemplate>
		                <span class="question"><asp:Literal runat="server" Id="ltrQuestion"></asp:Literal></span><br />
		                <asp:RadioButtonList Enabled="false" runat="server" id="rdlAnswer" class="rdlAnswer" RepeatColumns="5" RepeatLayout="Flow"></asp:RadioButtonList>
		                <asp:Literal runat="server" Id="ltrNote"></asp:Literal>
		            </ItemTemplate>
		        </asp:DataList>
            </td>
        </tr>
        <tr runat="server" id="trCommnent">
            <td valign="top"><b>Comments:</b></td>
            <td><asp:Label runat="server" ID="lbComment"></asp:Label></td>
        </tr>
    </table>
    <div style="margin-top:20px;">
    <asp:Button ID="btnClose" OnClientClick="ClosePopup();" runat="server" Text="Close" CssClass="btn" />
    </div>
      <script type="text/javascript">
         
          function ClosePopup() {
              window.close();
          }
    </script>
    
    </form>
</body>
</html>
