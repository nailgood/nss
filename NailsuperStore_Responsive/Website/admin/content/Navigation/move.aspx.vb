Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_content_navigation_move
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim NavigationId As Integer = Convert.ToInt32(Request("NavigationId"))
        Dim ParentId As Integer = Convert.ToInt32(Request("ParentId"))
        Dim Action As String = Request("ACTION")
        Try

            Dim Where As String
            If Not ParentId = 0 Then
                Where = "ParentId = " & ParentId
            Else
                Where = "ParentId is null"
            End If
            Core.ChangeSortOrder(DB, "NavigationId", "ContentToolNavigation", "SortOrder", Where, NavigationId, Action)
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class

