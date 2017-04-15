Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_customers_Edit
    Inherits AdminPage

    Protected CustomerId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        CustomerId = Convert.ToInt32(Request("CustomerId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If CustomerId = 0 Then
            btnDelete.Visible = False
            txtCustomerNo.Enabled = True
            Exit Sub
        Else
            txtCustomerNo.Enabled = False
        End If

        Dim dbCustomer As CustomerRow = CustomerRow.GetRow(DB, CustomerId)
        txtCustomerNo.Text = Trim(dbCustomer.CustomerNo)
        txtName.Text = Trim(dbCustomer.Name)
        txtName2.Text = Trim(dbCustomer.Name2)
        txtAddress.Text = Trim(dbCustomer.Address)
        txtAddress2.Text = Trim(dbCustomer.Address2)
        txtCity.Text = Trim(dbCustomer.City)
        txtZipcode.Text = Trim(dbCustomer.Zipcode)
        txtCounty.Text = Trim(dbCustomer.County)
        txtPhone.Text = Trim(dbCustomer.Phone)
        txtContact.Text = Trim(dbCustomer.Contact)
        txtEmail.Text = Trim(dbCustomer.Email)
        txtWebsite.Text = Trim(dbCustomer.Website)
        dtLastExport.Text = IIf(dbCustomer.LastExport = Nothing, "N/A", Trim(dbCustomer.LastExport))

        Dim dbCustomerContact As CustomerContactRow = CustomerContactRow.GetRow(DB, Trim(dbCustomer.ContactNo))
        If dbCustomerContact.ContactId <> Nothing Then
            txtContactNo.Enabled = False
            txtContactNo.Text = Trim(dbCustomerContact.ContactNo)
            txtContactName.Text = Trim(dbCustomerContact.CustName)
            txtContactName2.Text = Trim(dbCustomerContact.CustName2)
            txtContactAddress.Text = Trim(dbCustomerContact.Address)
            txtContactAddress2.Text = Trim(dbCustomerContact.Address2)
            txtContactCity.Text = Trim(dbCustomerContact.City)
            txtContactZipcode.Text = Trim(dbCustomerContact.PostCode)
            txtContactCounty.Text = Trim(dbCustomerContact.County)
            txtContactPhone.Text = Trim(dbCustomerContact.Phone)
            txtContactEmail.Text = Trim(dbCustomerContact.Email)
            txtContactWebsite.Text = Trim(dbCustomerContact.HomePage)
            dtLastExport.Text = IIf(dbCustomerContact.LastExport = Nothing, "N/A", Trim(dbCustomerContact.LastExport))
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbCustomer As CustomerRow
            Dim dbCustomerContact As CustomerContactRow

            If CustomerId <> 0 Then
                dbCustomer = CustomerRow.GetRow(DB, CustomerId)
            Else
                dbCustomer = New CustomerRow(DB)
                dbCustomer.CustomerNo = txtCustomerNo.Text
            End If
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
            dbCustomer.DoExport = True

            If CustomerId <> 0 Then
                dbCustomer.Update()
            Else
                CustomerId = dbCustomer.Insert
            End If

            If Trim(dbCustomer.ContactNo) <> String.Empty Then
                dbCustomerContact = CustomerContactRow.GetRow(DB, dbCustomer.ContactNo)
            End If
            If dbCustomerContact.ContactId = 0 Then
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
            dbCustomerContact.Phone = txtContactPhone.Text
            dbCustomerContact.Email = txtContactEmail.Text
            dbCustomerContact.HomePage = txtContactWebsite.Text
            dbCustomerContact.DoExport = True

            If dbCustomerContact.ContactId <> 0 Then
                dbCustomerContact.Update()
            Else
                dbCustomerContact.ContactId = dbCustomerContact.Insert
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

