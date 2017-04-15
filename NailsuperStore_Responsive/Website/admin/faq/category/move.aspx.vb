Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_faq_category_move
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim FaqCategoryId As Integer = Convert.ToInt32(Request("FaqCategoryId"))
        Dim Action As String = Request("ACTION")
        Try
            Core.ChangeSortOrder(DB, "FaqCategoryId", "FaqCategory", "SortOrder", "", FaqCategoryId, Action) 
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class

