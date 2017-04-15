Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.Data.SqlClient
Partial Class admin_emailLog_Index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            Dim strPath As String = System.Configuration.ConfigurationManager.AppSettings("sLogMailPath")
            Dim filename As String = "EmailLog_" & Date.Now().Year & "_" & Date.Now().Month & "_" & Date.Now().Day & ".txt"
            Dim sContents As String = ""
            Try
                If Core.FileExists(strPath & filename) = True Then
                    sContents = Core.OpenFile(strPath & filename)
                End If
                'sContents = sContents & "Date: " & Date.Now() & " Send Email: " & sEmail & " Subject: " & sSubject & " Status: "
                F_Contents.Text = sContents
            Catch ex As Exception
                F_Contents.Text = sContents
            End Try
        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub
        Dim strPath As String = System.Configuration.ConfigurationManager.AppSettings("sLogMailPath")
        Dim filename As String = "EmailLog_" & Date.Now().Year & "_" & Date.Now().Month & "_" & Date.Now().Day & ".txt"
        Dim sContents As String = ""
        Dim sfilechose As String = ""
        Dim dDate As Date
        Try
            dDate = CDate(F_StartDateLbound.Text)
            sfilechose = "EmailLog_" & dDate.Year & "_" & dDate.Month & "_" & dDate.Day & ".txt"
            If Core.FileExists(strPath & sfilechose) = True Then
                sContents = Core.OpenFile(strPath & sfilechose)
            End If
            'sContents = sContents & "Date: " & Date.Now() & " Send Email: " & sEmail & " Subject: " & sSubject & " Status: "
            F_Contents.Text = sContents
        Catch ex As Exception
            F_Contents.Text = sContents
        End Try


    End Sub

End Class

