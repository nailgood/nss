Imports Components
Imports DataLayer
Imports System.Data
Imports System.Data.SqlClient

Public Class faq_default
    Inherits SitePage

    Private dtFaqCategory As DataTable
    Private dtFaq As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ''  LoadData()
            ''301 to /service/faq.aspx
            Utility.Common.Redirect301("/service/faq.aspx")
        End If
    End Sub



End Class
