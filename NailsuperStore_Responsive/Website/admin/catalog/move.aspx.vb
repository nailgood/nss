Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Controls
Imports Components

Public Class admin_catalog_move
    Inherits AdminPage

    Protected params As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim CatalogId As Integer = CType(Request("CatalogId"), Integer)
        Dim ACTION As String = Request("ACTION")
        params = GetPageParams(FilterFieldType.All)

        Try
            Core.ChangeSortOrder(DB, "CatalogId", "StoreCatalog", "SortOrder", "", CatalogId, ACTION)
            Response.Redirect("default.aspx?CatalogId=" & CatalogId & "&" & params)
        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class