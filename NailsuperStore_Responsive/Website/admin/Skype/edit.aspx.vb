Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_skype_Edit
    Inherits AdminPage

    Protected SkypeId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        SkypeId = Convert.ToInt32(Request("SkypeId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()

        If SkypeId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If
        Dim dbContactSkype As ContactSkypeRow = ContactSkypeRow.GetRow(DB, SkypeId)
        If SkypeId <> 0 Then
            txtSkype.Text = dbContactSkype.Skype
            txtName.Text = dbContactSkype.Name
            'txtSort.Text = dbContactSkype.Sort
        Else
            txtSkype.Text = ""
            txtName.Text = ""
            'txtSort.Text = ""
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbContactSkype As ContactSkypeRow

            If SkypeId <> 0 Then
                dbContactSkype = ContactSkypeRow.GetRow(DB, SkypeId)
            Else
                dbContactSkype = New ContactSkypeRow(DB)
            End If
            dbContactSkype.Skype = txtSkype.Text
            dbContactSkype.Name = txtName.Text
            'dbContactSkype.Sort = txtSort.Text
            If SkypeId <> 0 Then
                dbContactSkype.SkypeID = SkypeId
                dbContactSkype.Update()
            Else
                SkypeId = dbContactSkype.Insert
            End If
            DB.CommitTransaction()

            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Response.Redirect("delete.aspx?SkypeId=" & SkypeId & "&" & GetPageParams(FilterFieldType.All))
    End Sub


End Class

