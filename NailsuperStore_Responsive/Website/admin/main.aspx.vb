Imports Components
Imports System.Diagnostics
Imports DataLayer
Imports System.Linq

Partial Class admin_main
    Inherits AdminPage

    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lbServerTime.Text = Now.ToString("M/d/yyyy HH:mm")
        lbDBName.Text = DB.Connection.Database
        lblNoConnections.Text = DB.ExecuteScalar("SELECT COUNT(dbid) as NumberOfConnections FROM sys.sysprocesses WHERE dbid > 0 AND DB_NAME(dbid) = '" & DB.Connection.Database & "' AND loginame = 'Nail' Group BY dbid, loginame")

        Try
            If Utility.ConfigData.USerClearCache.Split(",").Any(Function(i) i = Session("Track_AdminName")) Then
                btnClearCache.Visible = True
            End If
        Catch ex As Exception

        End Try

    End Sub


    Protected Sub ClearIndexSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ClearIndexSearch.Click
        Dim filePath As String = Utility.ConfigData.LuceneAppPath()
        If (System.IO.File.Exists(filePath)) Then
            Dim info As New System.Diagnostics.ProcessStartInfo(filePath, "")
            Dim p As New System.Diagnostics.Process()
            p.StartInfo = info
            Dim sAppName As String = "ExportLuceneItem"
            If Not filePath.Contains(sAppName & ".exe") Then
                Dim i As Integer = filePath.LastIndexOf("\")
                If i < 1 Then
                    i = filePath.LastIndexOf("/")
                End If
                If i > 0 Then
                    sAppName = filePath.Substring(i + 1)
                End If
                Dim j As Integer = sAppName.LastIndexOf(".")
                If j > 0 Then
                    sAppName = sAppName.Substring(0, j)
                End If
            End If
            If Not IsProcessOpen(sAppName) Then
                p.Start()
            End If
        End If
    End Sub
    Public Function IsProcessOpen(ByVal name As String) As Boolean
        For Each clsProcess As Process In Process.GetProcesses()
            If clsProcess.ProcessName.Contains(name) Then
                Return True
            End If
        Next
        Return False
    End Function

    Protected Sub UpdateCss_Click(sender As Object, e As EventArgs)
        Try
            Dim _param As SysparamRow = SysparamRow.GetRow(Me.DB, "CssScriptVersion")
            If _param Is Nothing Or String.IsNullOrEmpty(_param.Value) Then
                _param = New SysparamRow(Me.DB, "CssScriptVersion")
                _param.Value = DateTime.Now.ToString("yyyyMMddhhmmss")
                _param.Type = "string"
                _param.GroupName = "CSS"
                _param.Comments = "CSS Scripts."
                _param.Insert()
            Else
                _param.Value = DateTime.Now.ToString("yyyyMMddhhmmss")
                _param.Update()
                Utility.CacheUtils.RemoveCache(SysparamRowBase.cachePrefixKey & "_ListAll")


            End If
        Catch ex As Exception
            Response.Write("<script>alert('Refesh Css Script Error');</script>")
            Email.SendError("ToError500", "hplUpdateCss_Click", ex.Message)

        End Try
    End Sub
    Protected Sub clearCache_Click(sender As Object, e As EventArgs)
        Try
            For Each entry As System.Collections.DictionaryEntry In Cache
                Cache.Remove(entry.Key)
            Next
            Response.Write("<script>alert('Clear cache completed.');</script>")
        Catch ex As Exception
            Response.Write("<script>alert('Clear cache error')</script>")
            Email.SendError("ToError500", "clearCache_Click", ex.Message)

        End Try
    End Sub
End Class
