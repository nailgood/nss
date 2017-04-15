Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports Components
Imports DataLayer

Public Class admin_catalog_request_delete
    Inherits AdminPage

    Private CatalogId As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dbCatalog As StoreCatalogRequestRow
        CatalogId = Convert.ToInt32(Request("RequestId"))
        Try
            dbCatalog = StoreCatalogRequestRow.GetRow(DB, CatalogId)
            StoreCatalogRequestRow.RemoveRow(DB, CatalogId)
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        Finally
        End Try
    End Sub
End Class