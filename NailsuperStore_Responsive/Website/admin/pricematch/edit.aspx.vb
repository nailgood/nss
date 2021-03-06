Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_pricematch_Edit
    Inherits AdminPage

    Protected PriceMatchId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        PriceMatchId = Convert.ToInt32(Request("PriceMatchId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If PriceMatchId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbPriceMatch As PriceMatchRow = PriceMatchRow.GetRow(DB, PriceMatchId)
        txtYourName.Text = dbPriceMatch.YourName
        txtPhoneNumber.Text = dbPriceMatch.PhoneNumber
        txtEmailAddress.Text = dbPriceMatch.EmailAddress
        txtCompetitorsCompanyName.Text = dbPriceMatch.CompetitorsCompanyName
        txtCompetitorsPhoneNumber.Text = dbPriceMatch.CompetitorsPhoneNumber
        txtCompetitorsWebsite.Text = dbPriceMatch.CompetitorsWebsite

        rpt.DataSource = DB.GetDataTable("select * from pricematchitem where pricematchid = " & PriceMatchId)
        rpt.DataBind()
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Response.Redirect("delete.aspx?PriceMatchId=" & PriceMatchId & "&" & GetPageParams(FilterFieldType.All))
    End Sub

End Class

