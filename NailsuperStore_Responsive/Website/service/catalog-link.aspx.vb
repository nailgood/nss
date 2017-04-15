Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Collections.Generic
Imports Utility.Common

Partial Class service_catalog_link
    Inherits SitePage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadList()
        End If
    End Sub

    Private Sub LoadList()
        'Lấy querystring bỏ vô biên
        Try
            Dim sku As String = GetQueryString("SKU")
            Dim regexItem = New Regex("([^a-zA-Z0-9,]+)|(^,)")
            If regexItem.IsMatch(sku) OrElse sku.Contains("%") Then
                Response.Redirect("/service/catalog")
            End If

        Catch ex As Exception
            Response.Redirect("/service/catalog")
        End Try

    End Sub

End Class
