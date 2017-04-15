Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports System.IO
Imports System.Collections.Generic

Public Class admin_members_edit
    Inherits AdminPage

    Protected MemberId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request("act") IsNot Nothing AndAlso Request("act") = "email" Then
        Else

        End If

        MemberId = Convert.ToInt32(Request("MemberId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()

        drpCustomerPriceGroupId.DataSource = CustomerPriceGroupRow.GetAllCustomerPriceGroups(DB)
        drpCustomerPriceGroupId.DataValueField = "CustomerPriceGroupId"
        drpCustomerPriceGroupId.DataTextField = "CustomerPriceGroupCode"
        drpCustomerPriceGroupId.DataBind()
        drpCustomerPriceGroupId.Items.Insert(0, New ListItem("- - -", ""))

        MemberTypeId.DataSource = MemberTypeRow.GetAllMemberTypes1(DB)
        MemberTypeId.DataValueField = "MemberTypeId"
        MemberTypeId.DataTextField = "MemberType"
        MemberTypeId.DataBind()
        MemberTypeId.Items.Insert(0, New ListItem("-- ALL --", ""))

        If MemberId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbMember As MemberRow = MemberRow.GetRow(MemberId)
        MemberTypeId.SelectedValue = dbMember.MemberTypeId
        txtUsername.Text = dbMember.Username
        txtCustomerNo.Text = dbMember.Customer.CustomerNo
        txtLicenseNumber.Text = dbMember.LicenseNumber
        txtLicenseState.Text = dbMember.LicenseState
        txtStudentNumber.Text = dbMember.StudentNumber
        txtSchoolName.Text = dbMember.SchoolName
        txtContactName.Text = dbMember.ContactName
        txtContactPhone.Text = dbMember.ContactPhone
        txtSalesTaxExemptionNumber.Text = dbMember.SalesTaxExemptionNumber
        txtAuthorizedSignatureName.Text = dbMember.AuthorizedSignatureName
        txtAuthorizedSignatureTitle.Text = dbMember.AuthorizedSignatureTitle
        dtLicenseExpirationDate.Value = dbMember.LicenseExpirationDate
        dtExpectedGraduationDate.Value = dbMember.ExpectedGraduationDate
        dtAuthorizedSignatureDate.Value = dbMember.AuthorizedSignatureDate
        chkIsSameDefaultAddress.Checked = dbMember.IsSameDefaultAddress
        chkIsActive.Checked = dbMember.IsActive
        chkDeActive.Checked = dbMember.DeActive
        chkDeptOfRevenueRegistered.Checked = dbMember.DeptOfRevenueRegistered
        chkResaleAcceptance.Checked = dbMember.ResaleAcceptance
        chkInformationAccuracyAgreement.Checked = dbMember.InformationAccuracyAgreement
        chkCanPostJob.Checked = dbMember.CanPostJob
        chkCanPostClassified.Checked = dbMember.CanPostClassified

        Dim dbCustomer As CustomerRow = dbMember.Customer
        Dim MemberBillingAddress As MemberAddressRow = MemberAddressRow.GetAddressByType(DB, dbMember.MemberId, Utility.Common.MemberAddressType.Billing.ToString())
        ''MemberAddressRow.GetRow(DB, DB.ExecuteScalar("SELECT AddressId FROM MemberAddress WHERE MemberId=" & dbMember.MemberId & " AND Label = 'Default Billing Address'"))
        lblSalonName.Text = MemberBillingAddress.Company
        lblCustomerNo2.Text = dbCustomer.CustomerNo
        txtName.Text = dbCustomer.Name
        txtName2.Text = dbCustomer.Name2
        txtCustomerPostingGroup.Text = dbCustomer.CustomerPostingGroup
        txtAddress.Text = dbCustomer.Address
        txtAddress2.Text = dbCustomer.Address2
        txtCity.Text = dbCustomer.City
        txtZipcode.Text = dbCustomer.Zipcode
        txtCounty.Text = dbCustomer.County
        If (String.IsNullOrEmpty(dbCustomer.PhoneExt)) Then
            txtPhone.Text = dbCustomer.Phone
        Else
            txtPhone.Text = dbCustomer.Phone & " ext " & dbCustomer.PhoneExt
        End If

        drpCustomerPriceGroupId.SelectedValue = dbCustomer.CustomerPriceGroupId
        txtContact.Text = dbCustomer.Contact
        txtEmail.Text = dbCustomer.Email
        txtWebsite.Text = dbCustomer.Website
        txtContactNo.Text = dbCustomer.ContactNo
        lblSalesTaxExemptionNumber.Text = dbCustomer.SalesTaxExemptionNumber
        txtCurrencyCode.Text = dbCustomer.CurrencyCode
        txtCustomerPriceGroup.Text = CustomerPriceGroupRow.GetRow(DB, dbCustomer.CustomerPriceGroupId).CustomerPriceGroupCode
        txtCustomerDiscountGroup.Text = dbCustomer.CustomerDiscountGroup
        txtLanguageCode.Text = dbCustomer.LanguageCode
        txtPaymentTermsCode.Text = dbCustomer.PaymentTermsCode
        dtLastDateModified.Text = dbCustomer.LastDateModified
        dtLastImport.Text = dbCustomer.LastImport
        dtLastExport.Text = dbCustomer.LastExport
        'chkDoExport.Checked = dbCustomer.DoExport

        Dim dbCustomerContact As CustomerContactRow = dbMember.CustomerContact
        txtContactNo.Text = dbCustomerContact.ContactNo
        lblContactName.Text = dbCustomerContact.CustName
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
        lblSalesTaxExemptionNumber2.Text = dbCustomerContact.VATRegistrationNo
        lblLastExport.Text = dbCustomerContact.LastExport
        If (MemberId > 0) Then
            btnViewOrder.Visible = True
        Else
            btnViewOrder.Visible = False
        End If
        LoadExportCustomerLog(dbCustomer.CustomerId)
    End Sub
    Protected Sub btnViewOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnViewOrder.Click
        Response.Redirect("/admin/store/orders/default.aspx?MemberId=" & MemberId)
    End Sub


    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click


        Dim CustomerId As Integer = DB.ExecuteScalar("select top 1 coalesce(customerid,0) from customer where customerno = " & DB.Quote(txtCustomerNo.Text) & " and not exists (select top 1 memberid from member where customerid = customer.customerid and memberid <> " & MemberId & ")")
        Dim dbMember As MemberRow
        Dim dbMemberBefore As New MemberRow
        If MemberId <> 0 Then
            dbMember = MemberRow.GetRow(MemberId)
            dbMemberBefore = CloneObject.Clone(dbMember)
            If txtPassword.Text <> "" Then dbMember.Password = txtPassword.Text
        Else
            dbMember = New MemberRow(DB)
            dbMember.Password = txtPassword.Text()
        End If

        If txtCustomerNo.Text <> String.Empty AndAlso CustomerId <> dbMember.CustomerId Then
            If CustomerId <> 0 AndAlso MemberId <> 0 Then
                dbMember = MemberRow.GetRow(MemberId)
                dbMember.CustomerId = CustomerId
            Else
                AddError("Customer Number not found or already linked to another member.")
                Exit Sub
            End If
        End If

        If MemberId > 0 And txtPassword.Text = "" Then
            rqtxtPassword.Enabled = False
            pvtxtPassword.Enabled = False
            cvtxtPassword.Enabled = False
        End If

        Page.Validate()

        If Not IsValid Then Exit Sub
        Try
            Dim logDetail As New AdminLogDetailRow
            DB.BeginTransaction()

            dbMember.Username = txtUsername.Text
            dbMember.Customer.CustomerNo = txtCustomerNo.Text
            dbMember.LicenseNumber = txtLicenseNumber.Text
            dbMember.LicenseState = txtLicenseState.Text
            dbMember.StudentNumber = txtStudentNumber.Text
            dbMember.SchoolName = txtSchoolName.Text
            If String.IsNullOrEmpty(MemberTypeId.SelectedValue) Then
                dbMember.MemberTypeId = 0
            Else
                dbMember.MemberTypeId = MemberTypeId.SelectedValue
            End If

            dbMember.ContactName = txtContactName.Text
            dbMember.ContactPhone = txtContactPhone.Text
            dbMember.SalesTaxExemptionNumber = txtSalesTaxExemptionNumber.Text
            dbMember.AuthorizedSignatureName = txtAuthorizedSignatureName.Text
            dbMember.AuthorizedSignatureTitle = txtAuthorizedSignatureTitle.Text
            dbMember.LicenseExpirationDate = dtLicenseExpirationDate.Value
            dbMember.ExpectedGraduationDate = dtExpectedGraduationDate.Value
            dbMember.AuthorizedSignatureDate = dtAuthorizedSignatureDate.Value
            dbMember.IsSameDefaultAddress = chkIsSameDefaultAddress.Checked
            dbMember.IsActive = chkIsActive.Checked
            dbMember.DeActive = chkDeActive.Checked
            dbMember.DeptOfRevenueRegistered = chkDeptOfRevenueRegistered.Checked
            dbMember.ResaleAcceptance = chkResaleAcceptance.Checked
            dbMember.InformationAccuracyAgreement = chkInformationAccuracyAgreement.Checked
            dbMember.CanPostJob = chkCanPostJob.Checked
            dbMember.CanPostClassified = chkCanPostClassified.Checked
            Dim customerPriceGroupId As Integer = 0
            If String.IsNullOrEmpty(drpCustomerPriceGroupId.SelectedValue) Then
                customerPriceGroupId = 0
            Else
                customerPriceGroupId = CInt(drpCustomerPriceGroupId.SelectedValue)
            End If
            Dim sql As String = "Update Customer set CustomerPriceGroupId=" & customerPriceGroupId & " where  CustomerId=(Select CustomerId from Member where MemberId=" & dbMember.MemberId & ")"
            DB.ExecuteSQL(sql)
            Dim logSubject As String = ""
            If MemberId <> 0 Then
                logSubject = "Update member"
                dbMember.Update(DB)
                If (dbMember.IsActive) Then ''check and add point if register by refer friend
                    Dim res As DataTable = DB.GetDataTable("select [dbo].[fc_Member_UseReferCode](" & dbMember.MemberId & ",'" & dbMember.Customer.Email & "') as UseReferCode ")
                    If Not (res Is Nothing) Then
                        If res.Rows.Count > 0 Then
                            Dim UseReferCode As String = res.Rows(0)("UseReferCode")
                            If Not (String.IsNullOrEmpty(UseReferCode)) Then
                                ''Add point for this user if has refer code
                                Utility.Common.AddPointReferActiveAccount(DB, dbMember.MemberId, dbMember.Customer.Email, UseReferCode, dbMember.Username)
                            End If
                        End If
                    End If
                End If
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                logDetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.Member, dbMemberBefore, dbMember)
                logDetail.Message &= AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.Member, dbMemberBefore.Customer, dbMember.Customer)

            Else
                logSubject = "Insert member"
                MemberId = dbMember.Insert(False)
                logDetail.Action = Utility.Common.AdminLogAction.Insert.ToString()
                logDetail.Message = AdminLogHelper.ConvertObjectInsertToLogMesssageString(dbMember, Utility.Common.ObjectType.Member)
                logDetail.Message &= AdminLogHelper.ConvertObjectInsertToLogMesssageString(dbMember.Customer, Utility.Common.ObjectType.Member)
            End If
            AddPointRegister(dbMember)
            DB.CommitTransaction()

            logDetail.ObjectId = MemberId
            logDetail.ObjectType = Utility.Common.ObjectType.Member.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
            WriteLogDetail(logSubject, dbMember)
            Cache.Remove("CheckWHS_result_" & dbMember.MemberId)
            'LoadFromDB()
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


        Response.Redirect("delete.aspx?MemberId=" & MemberId & "&" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub LoadExportCustomerLog(ByVal CustomerId As Integer)
        ''lay thong tin cac file da export cua customer
        Dim ex As ExportCustomerLogRow = ExportCustomerLogRow.GetRow(DB, CustomerId)
        If Not ex Is Nothing Then
            If Not String.IsNullOrEmpty(ex.CustomerFile) Or Not String.IsNullOrEmpty(ex.AddressFile) Or Not String.IsNullOrEmpty(ex.NoteCustomerFile) Or Not String.IsNullOrEmpty(ex.NoteAddressFile) Then ''
                lbCustomerFile.Text = ex.CustomerFile
                lbAddressFile.Text = ex.AddressFile
                tblExport.Visible = True
                Dim filePath As String = Utility.ConfigData.ArchivePath
                If Not String.IsNullOrEmpty(ex.CustomerFile) Then
                    ltrCustomerStatus.Text = ex.CustomerStatus
                    If (Not File.Exists(filePath & ex.CustomerFile)) Then
                        lbCustomerFile.Enabled = False
                        spPendingCustomer.Visible = True
                    End If
                End If
                If (Not String.IsNullOrEmpty(ex.AddressFile)) Then
                    ltrAddressStatus.Text = ex.AddressStatus
                    If Not File.Exists(filePath & ex.AddressFile) Then
                        lbAddressFile.Enabled = False
                        spPendingAddress.Visible = True
                    End If
                End If
                If (ex.ModifiedDate = DateTime.MinValue) Then
                    ltrDate.Text = ex.CreatedDate
                Else
                    ltrDate.Text = ex.ModifiedDate
                End If
                If (Not String.IsNullOrEmpty(ex.NoteCustomerFile) Or Not String.IsNullOrEmpty(ex.NoteAddressFile)) Then
                    Dim lstNoteCustFile As String() = ex.NoteCustomerFile.Split(",")
                    Dim lstNoteAddrFile As String() = ex.NoteAddressFile.Split(",")
                    Dim lst As New List(Of NoteFile)

                    ''so dong lstNoteCustFile >= lstNoteAddrFile, thi cho vong lap theo lstCustNoteFile va nguoc lai
                    If (lstNoteCustFile.Length >= lstNoteAddrFile.Length) Then
                        For i As Integer = 0 To lstNoteCustFile.Length - 1
                            Dim note As New NoteFile
                            If Not String.IsNullOrEmpty(lstNoteCustFile(i)) Then
                                Dim index As Integer = lstNoteCustFile(i).IndexOf(":")
                                If index >= 0 Then
                                    note.CustFile = lstNoteCustFile(i).Substring(0, index)
                                    note.CustStatus = lstNoteCustFile(i).Substring(index + 1)
                                End If
                            End If

                            If (i < lstNoteAddrFile.Length) Then
                                If Not String.IsNullOrEmpty(lstNoteAddrFile(i)) Then
                                    Dim index As Integer = lstNoteAddrFile(i).IndexOf(":")
                                    If index >= 0 Then
                                        note.AddrFile = lstNoteAddrFile(i).Substring(0, index)
                                        note.AddrStatus = lstNoteAddrFile(i).Substring(index + 1)
                                    End If
                                End If
                            End If
                            If (Not String.IsNullOrEmpty(note.CustFile) Or Not String.IsNullOrEmpty(note.AddrFile)) Then
                                lst.Add(note)
                            End If
                        Next
                    Else
                        For i As Integer = 0 To lstNoteAddrFile.Length - 1
                            Dim note As New NoteFile
                            If Not String.IsNullOrEmpty(lstNoteAddrFile(i)) Then
                                Dim index As Integer = lstNoteAddrFile(i).IndexOf(":")
                                If index >= 0 Then
                                    note.AddrFile = lstNoteAddrFile(i).Substring(0, index)
                                    note.AddrStatus = lstNoteAddrFile(i).Substring(index + 1)
                                End If
                            End If
                            If (i < lstNoteCustFile.Length) Then
                                If Not String.IsNullOrEmpty(lstNoteCustFile(i)) Then
                                    Dim index As Integer = lstNoteCustFile(i).IndexOf(":")
                                    If index >= 0 Then
                                        note.CustFile = lstNoteCustFile(i).Substring(0, index)
                                        note.CustStatus = lstNoteCustFile(i).Substring(index + 1)
                                    End If
                                End If
                            End If
                            If (Not String.IsNullOrEmpty(note.CustFile) Or Not String.IsNullOrEmpty(note.AddrFile)) Then
                                lst.Add(note)
                            End If
                        Next
                    End If
                    If lst.Count > 0 Then
                        rptNoteFile.Visible = True
                        rptNoteFile.DataSource = lst
                        rptNoteFile.DataBind()
                    Else
                        rptNoteFile.Visible = False
                    End If
                Else
                    rptNoteFile.Visible = False
                End If
            Else
                tblExport.Visible = False
            End If
        End If
    End Sub

    Private Sub DownloadExport(ByVal sFileName As String)
        sFileName = sFileName.Trim()
        Dim filePath As String = Utility.ConfigData.ArchivePath & sFileName
        Dim PathDes As String = Request.PhysicalApplicationPath & Utility.ConfigData.FolderCopyArchivePath
        Try
            If (File.Exists(filePath)) Then
                If (Not Directory.Exists(PathDes)) Then
                    Directory.CreateDirectory(PathDes)
                End If
                File.Copy(filePath, PathDes & sFileName, True)
                Dim context As HttpContext = HttpContext.Current
                context.Response.Buffer = True
                context.Response.Clear()
                context.Response.AddHeader("content-disposition", "attachment; filename=" + sFileName)
                context.Response.ContentType = "application/octet-stream"
                context.Response.WriteFile("~" & Utility.ConfigData.FolderCopyArchivePath + sFileName)
                context.Response.Flush()
                context.Response.Close()
                File.Delete(PathDes & sFileName)
            Else
                ltrMsgError.Text = "<span class=""red"">" & sFileName & " can't download.</span>"
            End If
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub lbCustomerFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCustomerFile.Click
        DownloadExport(lbCustomerFile.Text)
    End Sub

    Protected Sub lbAddressFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddressFile.Click
        DownloadExport(lbAddressFile.Text)
    End Sub

    Protected Sub rptNoteFile_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptNoteFile.ItemCommand
        If e.CommandName.Equals("DownloadExport") Then
            DownloadExport(e.CommandArgument)
        End If
    End Sub

    Protected Sub rptNoteFile_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptNoteFile.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim note As NoteFile = DirectCast(DirectCast(e.Item.DataItem(), System.Object), NoteFile)

            Dim lbNoteCustomerFile As LinkButton = CType(e.Item.FindControl("lbNoteCustomerFile"), LinkButton)
            Dim ltrNoteCustStatus As Literal = CType(e.Item.FindControl("ltrNoteCustStatus"), Literal)

            Dim lbNoteAddressFile As LinkButton = CType(e.Item.FindControl("lbNoteAddressFile"), LinkButton)
            Dim ltrNoteAddrStatus As Literal = CType(e.Item.FindControl("ltrNoteAddrStatus"), Literal)

            lbNoteCustomerFile.Text = note.CustFile
            lbNoteCustomerFile.CommandArgument = note.CustFile
            ltrNoteCustStatus.Text = note.CustStatus

            lbNoteAddressFile.Text = note.AddrFile
            lbNoteAddressFile.CommandArgument = note.AddrFile
            ltrNoteAddrStatus.Text = note.AddrStatus
        End If
    End Sub
    Private Sub AddPointRegister(dbMember As MemberRow) 'add point when active
        Dim count As Integer = 0
        Try
            count = DB.ExecuteScalar("select COUNT(cashpointid) from CashPoint where MemberId = " & dbMember.MemberId & " and Notes = 'Member Register'")
        Catch
            count = 0
        End Try
        If count = 0 And dbMember.IsActive Then
            BaseShoppingCart.AwardedPoint(DB, dbMember.Customer.CustomerNo, dbMember.MemberId)
        End If
    End Sub
End Class

Public Class NoteFile
    Public CustFile As String
    Public CustStatus As String
    Public AddrFile As String
    Public AddrStatus As String
End Class

