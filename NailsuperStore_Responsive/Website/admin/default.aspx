<%@ Page Language="VB" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="admin_index" %>
<%@ Import Namespace="DataLayer" %>
<HTML>
	<HEAD>
		<TITLE><%=SysParam.GetValue("SiteName")%> Admin Section</TITLE>
	</HEAD>
    <frameset rows="24px,*" cols="*" frameborder=no border="0" framespacing="0">
    <frame src="top.aspx" name="top_menu" scrolling="no" target="">
    <frameset cols="188,*" border=0 frameborder=0 framespacing=0>
    <frame src="menu.aspx" name="menu" noresize scrolling="auto">
    <frame src="<%=FrameURL%>" name="main" noresize>
    </frameset>
    </frameset>
</HTML>
