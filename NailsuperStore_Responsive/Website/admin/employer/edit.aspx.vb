Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_employer_Edit
    Inherits AdminPage

    Protected EmployerId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        EmployerId = Convert.ToInt32(Request("EmployerId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If EmployerId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbEmployer As EmployerRow = EmployerRow.GetRow(DB, EmployerId)
        txtEmployerName.Text = dbEmployer.EmployerName
        fuImage.CurrentFileName = dbEmployer.Image
        chkIsFeatured.Checked = dbEmployer.IsFeatured
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbEmployer As EmployerRow

            If EmployerId <> 0 Then
                dbEmployer = EmployerRow.GetRow(DB, EmployerId)
            Else
                dbEmployer = New EmployerRow(DB)
            End If
            dbEmployer.EmployerName = txtEmployerName.Text
            If fuImage.NewFileName <> String.Empty Then
                fuImage.SaveNewFile()
                Core.ResizeImage(Server.MapPath(fuImage.Folder) & fuImage.NewFileName, Server.MapPath(fuImage.Folder) & fuImage.NewFileName, 120, 120)
                dbEmployer.Image = fuImage.NewFileName
            ElseIf fuImage.MarkedToDelete Then
                dbEmployer.Image = Nothing
            End If
            dbEmployer.IsFeatured = chkIsFeatured.Checked

            If EmployerId <> 0 Then
                dbEmployer.Update()
            Else
                EmployerId = dbEmployer.Insert
            End If

            DB.CommitTransaction()

            If fuImage.NewFileName <> String.Empty OrElse fuImage.MarkedToDelete Then fuImage.RemoveOldFile()

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
        Response.Redirect("delete.aspx?EmployerId=" & EmployerId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

