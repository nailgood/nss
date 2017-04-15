Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports Utility.Common

Public Class Admin_EditPolicyItem
    Inherits AdminPage
    Private TotalRecords As Integer
    Protected PolicyId As Integer
    Protected Type As Integer = 1
    Private PathFolderImage As String = Utility.ConfigData.ShopSaveBannerFolder

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("id") <> Nothing AndAlso Request.QueryString("id").Length > 0 Then
            PolicyId = CInt(Request.QueryString("id"))
        End If


        If Not IsPostBack Then
            If PolicyId > 0 Then
                LoadDetail()
            End If

        End If
    End Sub

    Private Sub LoadDetail()
        ltrHeader.Text = "Edit Item Policy"
        Dim row As PolicyRow = PolicyRow.GetRow(PolicyId)
        txtTitle.Text = row.Title
        txtMessage.Text = row.Message
        txtTextLink.Text = row.TextLink
        txtContent.Text = row.Content
        chkIsActive.Checked = row.IsActive

        If row.IsPopup Then
            radPopup.Checked = True
        Else
            radPage.Checked = True
        End If

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx")
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub

        Dim p As New PolicyRow()

        Try

            p.IsActive = chkIsActive.Checked
            p.Content = txtContent.Text
            p.Title = txtTitle.Text.Trim()
            p.Message = txtMessage.Text.Trim()
            p.TextLink = txtTextLink.Text.Trim()
            p.IsPopup = radPopup.Checked
            'Dim logSubject As String = ""
            'If (hidId.Value.Length > 0) Then
            '    ShopSaveId = hidId.Value
            '    shopsave.ShopSaveId = ShopSaveId
            '    logSubject = "Update Shop Save"
            '    shopsave.Update()
            'Else
            '    logSubject = "Insert Shop Save"
            '    ShopSaveId = p.Insert()
            'End If
            'WriteLogDetail(logSubject, shopsave)
            Dim bSuccess As Boolean = False
            If PolicyId > 0 Then
                p.PolicyId = PolicyId
                bSuccess = p.Update()
            Else
                bSuccess = p.Insert()
            End If

            If bSuccess Then
                Response.Redirect("default.aspx")
            Else

            End If
        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

End Class
