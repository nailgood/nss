Imports Components
Imports System.Configuration.ConfigurationManager
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports DataLayer
Imports System.Threading

''' <summary>
''' NAVISION TASKS
''' Add: Trung Nguyen - 1/6/2010
''' </summary>
''' <remarks></remarks>
Partial Class admin_NavisionTasks_Default
    Inherits AdminPage

    Private sExport As String = AppSettings.Get("NavisionExportPath")
    Private sImages As String = AppSettings.Get("NavisionImagesPath")
    Private sProductImport As String = AppSettings.Get("NavisionProductImportPath")
    Private sSendReminders As String = AppSettings.Get("SendRemindersPath")
    Private sWishListEmails As String = AppSettings.Get("WishListEmailsPath")
    Private sViewedItems As String = AppSettings.Get("ViewedItemsPath")
    Private sPurgeCC As String = AppSettings.Get("PurgeCCPath")

    Private sReturnURL As String = "/admin/navisiontasks/"

    ''' <summary>
    ''' Run external application
    ''' </summary>
    ''' <param name="sPath">Path of application</param>
    ''' <param name="sMessage">Error Message</param>
    ''' <remarks></remarks>
    Protected Sub RunTask(ByVal sPath As String, ByVal sErrorMessage As String)
        Dim pExportProcess As New Diagnostics.Process()
        Try
            pExportProcess.StartInfo.FileName = sPath
            pExportProcess.StartInfo.UseShellExecute = False
            pExportProcess.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(sPath)
            pExportProcess.StartInfo.WindowStyle = Diagnostics.ProcessWindowStyle.Hidden

            pExportProcess.Start()
            pExportProcess.WaitForExit()
            If pExportProcess.ExitCode = 0 Then
                Response.Redirect(sReturnURL)
            Else
                AddError(sErrorMessage & ". Please try again.")
            End If
            pExportProcess.Close()
        Catch ex As ComponentModel.Win32Exception
            AddError("An error occur when processing: " & ex.Message)
        End Try
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        RunTask(sExport, "Export data fail")
    End Sub

    Protected Sub btnImages_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImages.Click
        RunTask(sImages, "Import images fail")
    End Sub

    Protected Sub btnProductImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProductImport.Click
        RunTask(sProductImport, "Import product fail")
    End Sub

    Protected Sub btnReminders_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReminders.Click
        RunTask(sSendReminders, "Send reminders fail")
    End Sub

    Protected Sub btnWishListEmails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWishListEmails.Click
        RunTask(sWishListEmails, "Send wish list emails fail")
    End Sub

    Protected Sub btnPurgeViewedItems_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPurgeViewedItems.Click
        RunTask(sViewedItems, "Purge viewed items fail")
    End Sub

    Protected Sub tbnPurgeCC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbnPurgeCC.Click
        RunTask(sPurgeCC, "Purge CC fail")
    End Sub
End Class
