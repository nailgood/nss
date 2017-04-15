Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_shipping_Edit
    Inherits AdminPage

    Protected ShippingRangeId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        ShippingRangeId = Convert.ToInt32(Request("ShippingRangeId"))
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

        If ShippingRangeId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbShippingRange As ShippingRangeRow = ShippingRangeRow.GetRow(DB, ShippingRangeId)
        txtLowValue.Text = dbShippingRange.LowValue
        txtHighValue.Text = dbShippingRange.HighValue
        txtOverUnderValue.Text = dbShippingRange.OverUnderValue
        txtFirstPoundOver.Text = dbShippingRange.FirstPoundOver
        txtFirstPoundUnder.Text = dbShippingRange.FirstPoundUnder
        txtAdditionalPound.Text = dbShippingRange.AdditionalPound
        txtAdditionalThreshold.Text = dbShippingRange.AdditionalThreshold
        drpMethodId.SelectedValue = dbShippingRange.MethodId
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbShippingRange As ShippingRangeRow

            If ShippingRangeId <> 0 Then
                dbShippingRange = ShippingRangeRow.GetRow(DB, ShippingRangeId)
            Else
                dbShippingRange = New ShippingRangeRow(DB)
            End If
            dbShippingRange.LowValue = txtLowValue.Text
            dbShippingRange.HighValue = txtHighValue.Text
            dbShippingRange.OverUnderValue = txtOverUnderValue.Text
            dbShippingRange.FirstPoundOver = txtFirstPoundOver.Text
            dbShippingRange.FirstPoundUnder = txtFirstPoundUnder.Text
            dbShippingRange.AdditionalPound = txtAdditionalPound.Text
            dbShippingRange.AdditionalThreshold = txtAdditionalThreshold.Text
            dbShippingRange.MethodId = drpMethodId.SelectedValue

            If Not DB.IsEmpty(DB.ExecuteScalar("select top 1 coalesce(lowvalue,'') from shippingrange where methodid = " & dbShippingRange.MethodId & " and (" & DB.Quote(dbShippingRange.LowValue) & " between lowvalue and highvalue or " & DB.Quote(dbShippingRange.HighValue) & " between lowvalue and highvalue) and ShippingRangeId <> " & ShippingRangeId)) Then
                DB.RollbackTransaction()
                AddError("The zipcode range specified overlaps an existing zipcode range for the selected shipment method.")
                Exit Sub
            End If

            If ShippingRangeId <> 0 Then
                WriteLogDetail("Update ShippingRange", dbShippingRange)
                dbShippingRange.Update()
            Else
                ShippingRangeId = dbShippingRange.Insert
                WriteLogDetail("Update InsertRange", dbShippingRange)
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
        Response.Redirect("delete.aspx?ShippingRangeId=" & ShippingRangeId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

