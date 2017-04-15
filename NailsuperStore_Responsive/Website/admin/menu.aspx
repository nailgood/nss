<%@ Page Language="VB" AutoEventWireup="false" CodeFile="menu.aspx.vb" Inherits="MenuPage" %>
<HTML>
	<HEAD>
		<title>Welcome to Admin Section</title>
		<LINK href="/includes/theme-admin/css/admin.css?20120108" type="text/css" rel="stylesheet">
		<link href="/includes/theme-admin/css/menu.css.aspx" rel="stylesheet" type="text/css">
		<script src="/includes/theme-admin/scripts/menu.js"></script>
		<style>
		body,.body,.code{color:black;background:#B8CDE7;}
		</style>	
	</HEAD>
	<body>
		<a href="/admin" target="_parent"><img src="/includes/theme-admin/images/logo-sm.gif" width="140" height="80" style="border-style:none" alt="" /></a><br />
<%--		<div class=menutext>Welcome<br />
		<b><asp:label id="FullName" runat="server"></asp:label></b><br />
		</div>

        <p></p>
--%>
		<form id="main" runat="server">
		<asp:Label ID=lblMenu Runat=server></asp:Label>
		</form>
		
 <%--       <p></p>
        <% 
            If bSmartBug Then
%>
        <form method=post action="smartbug.aspx" target=main>
        <input type="submit" value="Edit Smart Bug" style="width:134px;" class="btn">
        </form>
<%
            End If
 %>--%>
       
	</body>
</HTML>
