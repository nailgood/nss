
Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Collections.Generic
Imports Utility.Common
Partial Class controls_layout_addthis
    Inherits BaseControl
    Public shareURL As String = String.Empty
    Public shareDescription As String
    

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

        Dim sb As StringBuilder = New StringBuilder()
        sb.AppendLine("var addthis_shareconfig_toolBox = {")
        sb.AppendLine("url:'" & shareURL & "',")
        sb.AppendLine("title:'" + shareDescription + "',")
        sb.AppendLine("data_track_addressbar:0,")
        sb.AppendLine("passthrough: {")
        sb.AppendLine("   twitter: {")
        sb.AppendLine("       via: '" & shareURL & "'")
        sb.AppendLine("   }")
        sb.AppendLine("}")
        sb.AppendLine("}")
        sb.AppendLine("if (toolBoxShareConfigs == null) var toolBoxShareConfigs = {};")
        sb.AppendLine("toolBoxShareConfigs['toolBox'] = addthis_shareconfig_toolBox;")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "shareToolBox", sb.ToString(), True)

    End Sub
End Class
