Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports Components
Imports DataLayer

Public Class admin_catalog_request_edit
    Inherits AdminPage

    Private CatalogId As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        CatalogId = Convert.ToInt32(Request("RequestId"))
        CatalogRequest.MemberId = 0
        CatalogRequest.CatalogId = CatalogId
        CatalogRequest.IsAdmin = True
        CatalogRequest.DB = DB
    End Sub
End Class