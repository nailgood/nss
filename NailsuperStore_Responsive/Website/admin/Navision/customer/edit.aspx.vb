Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_navision_customer_Edit
    Inherits AdminPage

    Protected CustomerId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        CustomerId = Convert.ToInt32(Request("CustomerId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        drpCustomerPriceGroupId.Datasource = CustomerPriceGroupRow.GetAllCustomerPriceGroups(DB)
        drpCustomerPriceGroupId.DataValueField = "CustomerPriceGroupId"
        drpCustomerPriceGroupId.DataTextField = "CustomerPriceGroupCode"
        drpCustomerPriceGroupId.Databind()
        drpCustomerPriceGroupId.Items.Insert(0, New ListItem("", ""))

        If CustomerId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbCustomer As CustomerRow = CustomerRow.GetRow(DB, CustomerId)
        txtCustomerNo.Text = dbCustomer.CustomerNo
        txtName.Text = dbCustomer.Name
        txtName2.Text = dbCustomer.Name2
        txtAddress.Text = dbCustomer.Address
        txtAddress2.Text = dbCustomer.Address2
        txtCity.Text = dbCustomer.City
        txtZipcode.Text = dbCustomer.Zipcode
        txtCounty.Text = dbCustomer.County
        txtPhone.Text = dbCustomer.Phone
        txtContact.Text = dbCustomer.Contact
        txtEmail.Text = dbCustomer.Email
        txtWebsite.Text = dbCustomer.Website
        txtSalesTaxExemptionNumber.Text = dbCustomer.SalesTaxExemptionNumber
        txtCurrencyCode.Text = dbCustomer.CurrencyCode
        txtCustomerDiscountGroup.Text = dbCustomer.CustomerDiscountGroup
        txtLanguageCode.Text = dbCustomer.LanguageCode
        txtPaymentTermsCode.Text = dbCustomer.PaymentTermsCode
        dtLastDateModified.Value = dbCustomer.LastDateModified
        dtLastImport.Value = dbCustomer.LastImport
        dtLastExport.Value = dbCustomer.LastExport
        drpCustomerPriceGroupId.SelectedValue = dbCustomer.CustomerPriceGroupId
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbCustomer As CustomerRow

            If CustomerId <> 0 Then
                dbCustomer = CustomerRow.GetRow(DB, CustomerId)
            Else
                dbCustomer = New CustomerRow(DB)
            End If
            dbCustomer.CustomerNo = txtCustomerNo.Text
            dbCustomer.Name = txtName.Text
            dbCustomer.Name2 = txtName2.Text
            dbCustomer.Address = txtAddress.Text
            dbCustomer.Address2 = txtAddress2.Text
            dbCustomer.City = txtCity.Text
            dbCustomer.Zipcode = txtZipcode.Text
            dbCustomer.County = txtCounty.Text
            dbCustomer.Phone = txtPhone.Text
            dbCustomer.Contact = txtContact.Text
            dbCustomer.Email = txtEmail.Text
            dbCustomer.Website = txtWebsite.Text
            dbCustomer.SalesTaxExemptionNumber = txtSalesTaxExemptionNumber.Text
            dbCustomer.CurrencyCode = txtCurrencyCode.Text
            dbCustomer.CustomerDiscountGroup = txtCustomerDiscountGroup.Text
            dbCustomer.LanguageCode = txtLanguageCode.Text
            dbCustomer.PaymentTermsCode = txtPaymentTermsCode.Text
            dbCustomer.LastDateModified = dtLastDateModified.Value
            dbCustomer.LastImport = dtLastImport.Value
            dbCustomer.LastExport = dtLastExport.Value
            dbCustomer.CustomerPriceGroupId = drpCustomerPriceGroupId.SelectedValue

            If CustomerId <> 0 Then
                dbCustomer.Update()
            Else
                CustomerId = dbCustomer.Insert
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
        Response.Redirect("delete.aspx?CustomerId=" & CustomerId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

