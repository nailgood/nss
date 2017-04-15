Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_classifieds_categories_Edit
    Inherits AdminPage

    Protected ClassifiedCategoryId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        ClassifiedCategoryId = Convert.ToInt32(Request("ClassifiedCategoryId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If ClassifiedCategoryId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbClassifiedCategory As ClassifiedCategoryRow = ClassifiedCategoryRow.GetRow(DB, ClassifiedCategoryId)
        txtCategory.Text = dbClassifiedCategory.Category
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            Dim dbClassifiedCategory As ClassifiedCategoryRow

            If ClassifiedCategoryId <> 0 Then
                dbClassifiedCategory = ClassifiedCategoryRow.GetRow(DB, ClassifiedCategoryId)
            Else
                dbClassifiedCategory = New ClassifiedCategoryRow(DB)
            End If
            dbClassifiedCategory.Category = txtCategory.Text

            If ClassifiedCategoryId <> 0 Then
                dbClassifiedCategory.Update()
            Else
                ClassifiedCategoryId = dbClassifiedCategory.Insert
            End If

            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Response.Redirect("delete.aspx?ClassifiedCategoryId=" & ClassifiedCategoryId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

