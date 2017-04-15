Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_classifieds_categories_delete
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim ClassifiedCategoryId As Integer = Convert.ToInt32(Request("ClassifiedCategoryId"))
        Try
            ClassifiedCategoryRow.RemoveRow(DB, ClassifiedCategoryId)
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class

