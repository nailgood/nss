Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_navision_customer_contact_Edit
    Inherits AdminPage

    Protected ContactId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        ContactId = Convert.ToInt32(Request("ContactId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If ContactId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbCustomerContact As CustomerContactRow = CustomerContactRow.GetRow(DB, ContactId)
        txtContactNo.Text = dbCustomerContact.ContactNo
        txtContactName.Text = dbCustomerContact.CustName
        txtContactName2.Text = dbCustomerContact.CustName2
        txtContactAddress.Text = dbCustomerContact.Address
        txtContactAddress2.Text = dbCustomerContact.Address2
        txtContactCity.Text = dbCustomerContact.City
        txtContactZipcode.Text = dbCustomerContact.PostCode
        txtContactCounty.Text = dbCustomerContact.County
        txtContactCountry.Text = dbCustomerContact.CountryCode
        txtContactPhone.Text = dbCustomerContact.Phone
        txtContactEmail.Text = dbCustomerContact.Email
        txtContactWebsite.Text = dbCustomerContact.HomePage
        txtSalesTaxExemptionNumber.Text = dbCustomerContact.VATRegistrationNo
        dtLastExport.Value = dbCustomerContact.LastExport
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbCustomerContact As CustomerContactRow

            If ContactId <> 0 Then
                dbCustomerContact = CustomerContactRow.GetRow(DB, ContactId)
            Else
                dbCustomerContact = New CustomerContactRow(DB)
            End If
            dbCustomerContact.ContactNo = txtContactNo.Text
            dbCustomerContact.CustName = txtContactName.Text
            dbCustomerContact.CustName2 = txtContactName2.Text
            dbCustomerContact.Address = txtContactAddress.Text
            dbCustomerContact.Address2 = txtContactAddress2.Text
            dbCustomerContact.City = txtContactCity.Text
            dbCustomerContact.PostCode = txtContactZipcode.Text
            dbCustomerContact.County = txtContactCounty.Text
            dbCustomerContact.CountryCode = txtContactCountry.Text
            dbCustomerContact.Phone = txtContactPhone.Text
            dbCustomerContact.Email = txtContactEmail.Text
            dbCustomerContact.HomePage = txtContactWebsite.Text
            dbCustomerContact.VATRegistrationNo = txtSalesTaxExemptionNumber.Text
            dbCustomerContact.LastExport = dtLastExport.Value

            If ContactId <> 0 Then
                dbCustomerContact.Update()
            Else
                ContactId = dbCustomerContact.Insert
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
        Response.Redirect("delete.aspx?ContactId=" & ContactId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

