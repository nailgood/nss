Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_shippingint_Edit
    Inherits AdminPage

    Protected ShippingRangeIntId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        ShippingRangeIntId = Convert.ToInt32(Request("ShippingRangeIntId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        drpMethodId.Datasource = ShipmentMethodRow.GetAllShipmentMethods(DB)
        drpMethodId.DataValueField = "MethodId"
        drpMethodId.DataTextField = "Name"
        drpMethodId.Databind()
        drpMethodId.Items.Insert(0, New ListItem("", ""))

        drpCountryCode.Items.AddRange(CountryRow.GetCountries().ToArray())
        drpCountryCode.DataBind()
        drpCountryCode.Items.Insert(0, New ListItem("", ""))

        If ShippingRangeIntId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbShippingRangeInt As ShippingRangeIntRow = ShippingRangeIntRow.GetRow(DB, ShippingRangeIntId)
        txtOverUnderValue.Text = dbShippingRangeInt.OverUnderValue
        txtFirstPoundOver.Text = dbShippingRangeInt.FirstPoundOver
        txtFirstPoundUnder.Text = dbShippingRangeInt.FirstPoundUnder
        txtAdditionalPound.Text = dbShippingRangeInt.AdditionalPound
        txtAdditionalThreshold.Text = dbShippingRangeInt.AdditionalThreshold
        drpMethodId.SelectedValue = dbShippingRangeInt.MethodId
        drpCountryCode.SelectedValue = dbShippingRangeInt.CountryCode
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbShippingRangeInt As ShippingRangeIntRow

            If ShippingRangeIntId <> 0 Then
                dbShippingRangeInt = ShippingRangeIntRow.GetRow(DB, ShippingRangeIntId)
            Else
                dbShippingRangeInt = New ShippingRangeIntRow(DB)
            End If
            dbShippingRangeInt.OverUnderValue = txtOverUnderValue.Text
            dbShippingRangeInt.FirstPoundOver = txtFirstPoundOver.Text
            dbShippingRangeInt.FirstPoundUnder = txtFirstPoundUnder.Text
            dbShippingRangeInt.AdditionalPound = txtAdditionalPound.Text
            dbShippingRangeInt.AdditionalThreshold = txtAdditionalThreshold.Text
            dbShippingRangeInt.MethodId = drpMethodId.SelectedValue
            dbShippingRangeInt.CountryCode = drpCountryCode.SelectedValue

            If ShippingRangeIntId <> 0 Then
                dbShippingRangeInt.Update()
            Else
                ShippingRangeIntId = dbShippingRangeInt.Insert
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
        Response.Redirect("delete.aspx?ShippingRangeIntId=" & ShippingRangeIntId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

