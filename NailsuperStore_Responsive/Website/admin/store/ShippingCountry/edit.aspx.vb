Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_ShippingCountry_Edit
    Inherits AdminPage

    Protected CountryId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        CountryId = Convert.ToInt32(Request("CountryId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        drpCountryId.Items.AddRange(CountryRow.GetCountries().ToArray())
        drpCountryId.DataBind()
        drpCountryId.Items.Insert(0, New ListItem("", ""))
        drpCountryId.Items.RemoveAt(drpCountryId.Items.IndexOf(drpCountryId.Items.FindByText("United States")))

        If CountryId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbCountry As CountryRow = CountryRow.GetRow(DB, CountryId)
        txtShippingCode.Text = dbCountry.ShippingCode
        chkIsActive.Checked = dbCountry.IsShippingActive
        drpCountryId.SelectedValue = dbCountry.CountryId
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbCountry As CountryRow

            If CountryId <> 0 Then
                dbCountry = CountryRow.GetRow(DB, CountryId)
            Else
                dbCountry = New CountryRow(DB)
            End If
            dbCountry.ShippingCode = txtShippingCode.Text
            dbCountry.CountryId = drpCountryId.SelectedValue
            dbCountry.IsShippingActive = chkIsActive.Checked

            If CountryId <> 0 Then
                dbCountry.Update()
            Else
                CountryId = dbCountry.Insert
            End If

            DB.CommitTransaction()


            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Response.Redirect("delete.aspx?CountryId=" & CountryId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

