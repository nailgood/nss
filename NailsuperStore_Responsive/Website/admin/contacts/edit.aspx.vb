Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_contacts_Edit
    Inherits AdminPage

    Protected ContactId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        ContactId = Convert.ToInt32(Request("ContactId"))

        If ContactId <> 0 Then
            PASSWORDVALIDATOR1.Enabled = False
            PASSWORDVALIDATOR2.Enabled = False
        Else
            trPassword.Visible = False
        End If

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
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not PASSWORD1.Text = String.Empty Or Not PASSWORD2.Text = String.Empty Then
            PASSWORDVALIDATOR1.Enabled = True
            PASSWORDVALIDATOR2.Enabled = True
            PasswordLenghtValidator.Enabled = True

            'Force Page Validation	
            Page.Validate()
        End If
        If Not IsValid Then Exit Sub

        Try
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
            dbCustomerContact.Phone = txtContactPhone.Text
            dbCustomerContact.Email = txtContactEmail.Text
            dbCustomerContact.HomePage = txtContactWebsite.Text
            dbCustomerContact.DoExport = True

            If ContactId <> 0 Then
                dbCustomerContact.Update()
            Else
                ContactId = dbCustomerContact.Insert
            End If

            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
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

