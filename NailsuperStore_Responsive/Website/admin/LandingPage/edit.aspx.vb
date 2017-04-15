Imports Components
Imports DataLayer

Partial Class admin_LandingPage_edit
    Inherits AdminPage

    Protected Id As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Id = Convert.ToInt32(Request("Id"))
        'If hidPopUpSKU.Value <> "" Then
        txtSKU.Text = hidPopUpSKU.Value
        ' End If
        If Not IsPostBack Then
            LoadFromDB()
        End If

    End Sub

    Private Sub LoadFromDB()
        drpCustomerPriceGroupId.DataSource = CustomerPriceGroupRow.GetAllCustomerPriceGroups(DB)
        drpCustomerPriceGroupId.DataTextField = "codewithcount"
        drpCustomerPriceGroupId.DataValueField = "CustomerPriceGroupId"
        drpCustomerPriceGroupId.DataBind()
        drpCustomerPriceGroupId.Items.Insert(0, New ListItem("", "0"))

        If Id <= 0 Then
            txtFileLocation.Text = "/assets/LandingPage/"
            Return
        End If
        Dim dbLandingPage As LandingPageRow = LandingPageRow.GetRow(DB, Id)
        If Not dbLandingPage Is Nothing Then
            txtTitle.Text = dbLandingPage.Title
            txtPageTitle.Text = dbLandingPage.PageTitle
            txtMetaKeyword.Text = dbLandingPage.MetaKeywords
            txtMetaDescription.Text = dbLandingPage.MetaDescription
            dprStartDate.Text = dbLandingPage.StartingDate.ToString("MM/dd/yyyy")
            dprEndDate.Text = dbLandingPage.EndingDate.ToString("MM/dd/yyyy")
            drpCustomerPriceGroupId.SelectedValue = dbLandingPage.CustomerPriceGroupId
            txtFileLocation.Text = dbLandingPage.FileLocation
            txtURLCode.Text = dbLandingPage.UrlCode
            txtUrlReturn.Text = dbLandingPage.UrlReturn
            chkIsActive.Checked = dbLandingPage.IsActive
            txtGoogleABCode.Text = dbLandingPage.GoogleABCode
            hidItemId.Value = dbLandingPage.ItemId
            Dim si As StoreItemRow = StoreItemRow.GetRow(DB, dbLandingPage.ItemId)
            txtSKU.Text = si.SKU
            hidPopUpSKU.Value = si.SKU
        End If

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If String.IsNullOrEmpty(txtURLCode.Text) Then
            txtURLCode.Text = txtTitle.Text.Trim().ToLower().Replace(" ", "-")
        End If
        Dim bCheckUrlCode As Boolean = LandingPageRow.CheckURLCode(DB, Id, txtURLCode.Text)
        If bCheckUrlCode Then
            AddError("Duplicated URL Code: " & txtURLCode.Text)
            Exit Sub
        End If
        Dim startDate As DateTime = Convert.ToDateTime(dprStartDate.Text)
        Dim endDate As DateTime = Convert.ToDateTime(dprEndDate.Text)
        If (startDate > endDate) Then
            AddError("End Date (" & endDate.ToString("MM/dd/yyyy") & ") must be greater than Start date (" & startDate.ToString("MM/dd/yyyy") & ").")
            Exit Sub
        End If

        Dim i As Integer = txtFileLocation.Text.LastIndexOf(".")
        If i > 0 Then
            Dim sCompare As String = txtFileLocation.Text.ToLower().Substring(i + 1)
            If Not sCompare.Equals("htm") And Not sCompare.Equals("html") Then
                AddError("File Location incorrect. (htm, html) ")
                Exit Sub
            End If
        Else
            AddError("File Location incorrect. (htm, html) ")
            Exit Sub
        End If

        If Page.IsValid Then
            Try
                Dim dbLandingPage As LandingPageRow
                Dim dbLandingPageBefore As New LandingPageRow
                Dim logDetail As New AdminLogDetailRow
                If Id <> 0 Then
                    dbLandingPage = LandingPageRow.GetRow(DB, Id)
                    dbLandingPageBefore = CloneObject.Clone(dbLandingPage)
                Else
                    dbLandingPage = New LandingPageRow(DB)
                End If
                dbLandingPage.Title = txtTitle.Text
                dbLandingPage.UrlCode = txtURLCode.Text

                Dim si As StoreItemRow = StoreItemRow.GetRowSku(DB, txtSKU.Text)
                dbLandingPage.ItemId = si.ItemId

                dbLandingPage.CustomerPriceGroupId = drpCustomerPriceGroupId.SelectedValue
                dbLandingPage.StartingDate = Convert.ToDateTime(dprStartDate.Text)
                dbLandingPage.EndingDate = Convert.ToDateTime(dprEndDate.Text)

                dbLandingPage.IsActive = chkIsActive.Checked
                dbLandingPage.FileLocation = txtFileLocation.Text
                dbLandingPage.PageTitle = txtPageTitle.Text
                dbLandingPage.MetaDescription = txtMetaDescription.Text
                dbLandingPage.MetaKeywords = txtMetaKeyword.Text
                dbLandingPage.GoogleABCode = txtGoogleABCode.Text
                dbLandingPage.UrlReturn = txtUrlReturn.Text
                If Id <> 0 Then
                    LandingPageRow.Update(DB, dbLandingPage)
                    logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                    dbLandingPage.GoogleABCode = String.Empty
                    dbLandingPageBefore.GoogleABCode = String.Empty
                    logDetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.LandingPage, dbLandingPageBefore, dbLandingPage)
                Else
                    Id = LandingPageRow.Insert(DB, dbLandingPage)
                    logDetail.Action = Utility.Common.AdminLogAction.Insert.ToString()
                    dbLandingPage.GoogleABCode = String.Empty
                    logDetail.Message = AdminLogHelper.ConvertObjectInsertToLogMesssageString(dbLandingPage, Utility.Common.ObjectType.LandingPage)
                End If
                logDetail.ObjectId = Id
                logDetail.ObjectType = Utility.Common.ObjectType.LandingPage.ToString()
                AdminLogHelper.WriteLuceneLogDetail(logDetail)

                Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

            Catch ex As Exception
                AddError(ErrHandler.ErrorText(ex))

            End Try
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
End Class
