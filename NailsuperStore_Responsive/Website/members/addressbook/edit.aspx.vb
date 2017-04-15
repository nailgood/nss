Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System.Web.Services

Public Class Member_Addressbook_Edit
    Inherits SitePage
    Private dbMember As MemberRow
    Protected css As String = String.Empty
    Public Property ModuleType() As String
        Get
            Return CType(ViewState("ModuleType"), String)
        End Get
        Set(ByVal Value As String)
            ViewState("ModuleType") = Value
        End Set
    End Property
    Public ReadOnly Property IsShippingAddress() As Boolean?
        Get
            Dim value As String = GetQueryString("shippingAddress")
            If String.IsNullOrEmpty(value) Then
                Return Nothing
            End If
            If value.Trim() = "1" Then
                Return True
            ElseIf value.Trim() = "0"
                Return False
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public Property AddressId() As Integer
        Get
            Return CType(ViewState("AddressId"), String)
        End Get
        Set(ByVal value As Integer)
            ViewState("AddressId") = value
        End Set
    End Property
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        ModuleType = GetQueryString("mt")
        AddressId = GetQueryString("AddressId")
        If (ModuleType = "checkout") Then
            Me.MasterPageFile = "~/includes/masterpage/checkout.master"
        Else
            Me.MasterPageFile = "~/includes/masterpage/interior.master"
        End If
    End Sub
    Protected Sub ServerCheckPhoneUS(ByVal sender As Object, ByVal e As ServerValidateEventArgs)

        If Not String.IsNullOrEmpty(e.Value) Then
            e.IsValid = Utility.Common.CheckUSPhoneValid(e.Value)
        End If

    End Sub
    Protected Sub ServerCheckPhoneInternational(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        Utility.Common.CheckPhoneInternational(cusvPhoneInt, e)
    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim memberId As Integer = Utility.Common.GetCurrentMemberId()
        If memberId <= 0 And HasAccess() = False Then
            Response.Redirect("/members/login.aspx")
        End If

        dbMember = MemberRow.GetRow(memberId)
        If Not Page.IsPostBack Then
            LoadData()

            If (ModuleType = "checkout") Then
                divEmail.Visible = True
                rqtxtEmail.Visible = True
                cusvEmail.Visible = True

                Utility.Common.OrderLog(0, "Page Load", Nothing)
            End If
        End If
    End Sub

    Private Sub LoadData()
        LoadDropdowns()
        If AddressId > 0 Then
            ltrTitle.Text = " Edit Address Book Entry"
            Dim dbMemberAddress As MemberAddressRow = MemberAddressRow.GetRow(DB, AddressId)
            If dbMemberAddress Is Nothing Then
                Response.Redirect("/members/addressbook/")
            End If
            If Not dbMemberAddress.MemberId = dbMember.MemberId Then
                Response.Redirect("/members/addressbook/")
            End If
            If Not dbMemberAddress.AddressType = Utility.Common.MemberAddressType.AddressBook.ToString() Then
                Response.Redirect("/members/addressbook/")
            End If
            LoadFormData(dbMemberAddress)
        Else
            ltrTitle.Text = "Add New Address"
        End If

        EnableControl(drpCountry.SelectedValue, True)
    End Sub
    Private Sub LoadDropdowns()

        Dim listItem = StateRow.GetStateList()
        drpState.Items.AddRange(listItem.ToArray())
        drpState.DataBind()
        drpState.Items.Insert(0, New ListItem("", ""))

        drpState.Items.Remove(drpState.Items.FindByValue("PR"))
        drpState.Items.Remove(drpState.Items.FindByValue("VI"))

        drpCountry.Items.AddRange(CountryRow.GetCountries().ToArray())
        drpCountry.DataBind()
        drpCountry.Items.Insert(0, New ListItem("", ""))
        drpCountry.SelectedValue = "US"

        drpCountry.Items.Remove(drpCountry.Items.FindByValue("HI"))
        drpCountry.Items.Remove(drpCountry.Items.FindByValue("AK"))

        divState.ClientIDMode = ClientIDMode.Static
    End Sub
    Private Sub LoadFormData(ByVal dbMemberAddress As MemberAddressRow)
        If Not String.IsNullOrEmpty(dbMemberAddress.Label) Then
            txtLabel.Text = Trim(dbMemberAddress.Label)
        End If
        If Not String.IsNullOrEmpty(dbMemberAddress.FirstName) Then
            txtFirstName.Text = Trim(dbMemberAddress.FirstName)
        End If
        If Not String.IsNullOrEmpty(dbMemberAddress.LastName) Then
            txtLastName.Text = Trim(dbMemberAddress.LastName)
        End If
        If Not String.IsNullOrEmpty(dbMemberAddress.Company) Then
            txtCompany.Text = Trim(dbMemberAddress.Company)
        End If
        If Not String.IsNullOrEmpty(dbMemberAddress.Address1) Then
            txtAddress1.Text = Trim(dbMemberAddress.Address1)
        End If
        If Not String.IsNullOrEmpty(dbMemberAddress.Address2) Then
            txtAddress1.Text &= " " & dbMemberAddress.Address2.Trim()
        End If
        If Not String.IsNullOrEmpty(dbMemberAddress.City) Then
            txtCity.Text = Trim(dbMemberAddress.City)
        End If
        If Not String.IsNullOrEmpty(dbMemberAddress.Zip) Then
            txtZip.Text = Trim(dbMemberAddress.Zip)
        End If
        If Not String.IsNullOrEmpty(dbMemberAddress.Phone) Then
            txtBillingPhone.Text = Trim(dbMemberAddress.Phone)
            If Not String.IsNullOrEmpty(dbMemberAddress.PhoneExt) Then
                txtBillingPhone.Text &= " " & Trim(dbMemberAddress.PhoneExt)
            End If
        End If
        drpState.SelectedValue = dbMemberAddress.State
        drpCountry.SelectedValue = dbMemberAddress.Country
        If Not String.IsNullOrEmpty(dbMemberAddress.Region) Then
            txtRegion.Text = Trim(dbMemberAddress.Region)
        End If
    End Sub

    Protected Sub drpCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpCountry.SelectedIndexChanged
        EnableControl(drpCountry.SelectedValue, False)
    End Sub

    Private Sub EnableControl(ByVal country As String, ByVal isLoad As Boolean)
        If country <> "US" AndAlso country <> "" Then ''Int
            divState.Visible = False
            dvRegion.Visible = True
            cusvPhoneUS.Visible = False
            cusvPhoneInt.Visible = True
            rqtxtZip.Visible = False
            rqextxtZip.Visible = False
            txtZip.MaxLength = 20
            css = String.Empty
        Else ''US
            divState.Visible = True
            dvRegion.Visible = False
            cusvPhoneUS.Visible = True
            cusvPhoneInt.Visible = False
            rqtxtZip.Visible = True
            rqextxtZip.Visible = True
            txtZip.MaxLength = 5
            css = "required"
        End If
    End Sub

    Private Function CheckAddressMsg(ByVal Zip As String, ByVal City As String, ByVal State As String) As String
        Dim msg As String = ""
        Dim result As Integer = 0
        Dim collection As ZipCodeCollection = ZipCodeRow.CheckAddress(Zip, City, State, result)
        If result < 0 Then
            msg = String.Format("The address is not valid. Please verify State and Zipcode are correct.")
        ElseIf result = 0 Then
            For Each z As ZipCodeRow In collection
                msg &= ""
            Next
        End If

        Return msg
    End Function

    Private Sub ShowError(ByVal msg As String)
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "showError", "ShowError('" & msg & "');", True)
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim bError As Boolean = False

        If drpCountry.SelectedValue = "US" Then
            Dim strError As String = CheckAddressMsg(txtZip.Text, txtCity.Text, drpState.SelectedValue)
            If Not String.IsNullOrEmpty(strError) Then
                ShowError(strError)
                bError = True
            End If
        End If

        If bError = False And Page.IsValid Then
            Dim bSuccess As Boolean = False
            Dim DupId As Integer = DB.ExecuteScalar("select top 1 addressid from memberaddress where memberid = " & dbMember.MemberId & " and isdeleted = 0 and addressid <> " & AddressId & " and label = " & DB.Quote(txtLabel.Text))
            If Not DupId = Nothing Then
                ShowError("The specified label already exists in your address book, please choose a new label")
                Exit Sub
            End If
            Try
                Dim dbMemberAddress As MemberAddressRow = Nothing
                If AddressId > 0 Then
                    dbMemberAddress = MemberAddressRow.GetRow(DB, AddressId)
                Else
                    dbMemberAddress = New MemberAddressRow(DB)
                    dbMemberAddress.AddressType = "AddressBook"
                End If
                dbMemberAddress.Country = drpCountry.SelectedValue
                dbMemberAddress.AddressType = Utility.Common.MemberAddressType.AddressBook.ToString()
                dbMemberAddress.MemberId = dbMember.MemberId
                dbMemberAddress.IsDefault = False
                dbMemberAddress.FirstName = txtFirstName.Text.Trim()
                dbMemberAddress.LastName = txtLastName.Text.Trim()
                dbMemberAddress.Company = txtCompany.Text.Trim()
                dbMemberAddress.Address1 = txtAddress1.Text.Trim()
                dbMemberAddress.Address2 = String.Empty
                dbMemberAddress.City = txtCity.Text.Trim()
                dbMemberAddress.Zip = txtZip.Text.Trim()
                If ModuleType = "checkout" Then
                    dbMemberAddress.Email = txtEmail.Text.Trim()
                End If
                If dbMemberAddress.Country <> "US" Then
                    dbMemberAddress.Phone = txtBillingPhone.Text.Trim
                    dbMemberAddress.PhoneExt = String.Empty
                Else
                    Utility.Common.GetUSPhoneValueFromUI(txtBillingPhone.Text, dbMemberAddress.Phone, dbMemberAddress.PhoneExt)
                End If
                dbMemberAddress.State = drpState.SelectedValue
                dbMemberAddress.Region = txtRegion.Text.Trim()
                dbMemberAddress.Label = txtLabel.Text.Trim()
                dbMemberAddress.DoExport = True
                DB.BeginTransaction()
                If AddressId > 0 Then
                    dbMemberAddress.Update()
                Else
                    AddressId = dbMemberAddress.Insert()
                End If

                StoreOrderRow.CopyBillShipAddressFromMemberAddressBook(DB, AddressId, Utility.Common.GetCurrentMemberId(), IsShippingAddress)
                DB.CommitTransaction()

                If dbMemberAddress.Country <> "US" Or Utility.Common.CheckShippingSpecialUS(dbMemberAddress.Country, dbMemberAddress.State) Then
                    Dim SQL As String = "UPDATE StoreCartItem SET CarrierType = CASE WHEN isoversize = 1 and " & Utility.Common.USPSPriorityShippingId & " NOT IN (" & Utility.Common.PickupShippingId & ") THEN " & Utility.Common.TruckShippingId & " else " & Utility.Common.USPSPriorityShippingId & " END WHERE [type] = 'item' AND OrderId = " & Utility.Common.GetCurrentOrderId()
                    SQL &= "; UPDATE StoreOrder SET CarrierType = " & Utility.Common.USPSPriorityShippingId() & " WHERE OrderId = " & Utility.Common.GetCurrentOrderId()
                    DB.ExecuteSQL(SQL)
                End If

                bSuccess = True
            Catch ex As SqlException
                If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
                ShowError(ErrHandler.ErrorText(ex))
            End Try

            If bSuccess Then
                GoBack()
            End If
        End If

    End Sub
    Private Sub GoBack()
        Dim link As String = String.Empty
        If Me.MasterPageFile.Contains("/masterpage/checkout.master") Then
            link = "/store/payment.aspx?act=newaddress"
            If IsShippingAddress Then
                Session("ShippingAddressRedirect") = True
            Else
                Session("ShippingAddressRedirect") = False
            End If
        Else
            link = "/members/addressbook/"
        End If
        Response.Redirect(link)
    End Sub

    Protected Sub lbtnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnBack.Click
        GoBack()
    End Sub

    <WebMethod()>
    Public Shared Function SetZipCode(ByVal city As String, ByVal zipcode As String, ByVal state As String) As String
        Dim msg As String = ""
        Dim result As Integer = 0
        Dim collection As ZipCodeCollection = ZipCodeRow.GetRow(zipcode)

        Dim country As String = String.Empty
        If collection.Count > 0 Then
            For Each zip As ZipCodeRow In collection
                If zip.StateCode = "VI" Or zip.StateCode = "PR" Then
                    country = zip.StateCode
                Else
                    country = String.Empty
                    Exit For
                End If
            Next
        End If

        If country = String.Empty Then
            Return Newtonsoft.Json.JsonConvert.SerializeObject(collection)
        Else
            Return Newtonsoft.Json.JsonConvert.SerializeObject(country)
        End If
    End Function
End Class