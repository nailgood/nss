Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports Components
Imports DataLayer

Public Class admin_catalog_delete
    Inherits AdminPage

    Private CatalogId As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dbCatalog As StoreCatalogRow
        CatalogId = Convert.ToInt32(Request("CatalogId"))
        Try
            dbCatalog = StoreCatalogRow.GetRow(DB, CatalogId)
            StoreCatalogRow.RemoveRow(DB, CatalogId)
            If System.IO.File.Exists(Server.MapPath("/assets/catalog/" & dbCatalog.CatalogImage)) Then
                System.IO.File.Delete(Server.MapPath("/assets/catalog/" & dbCatalog.CatalogImage))
            End If
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        Finally
        End Try
    End Sub
End Class