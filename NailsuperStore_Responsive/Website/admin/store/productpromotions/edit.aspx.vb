Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class Edit
    Inherits AdminPage

    Protected PromotionId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If hidPopUpSKU.Value <> "" Then
            txtSKU.Text = hidPopUpSKU.Value

            'Dim arr As Array = Split(hidPopUpSKU.Value.Trim(), ";")
            'Dim dt As DataTable = DB.GetDataTable("select spi.* from salesprice spi inner join StoreItem si on spi.ItemId=si.ItemId where EndingDate >= GETDATE()" & " and si.sku = '" & arr(0).ToString() & "'")
            'If dt.Rows.Count > 0 Then
            '    Response.Redirect("edit.aspx?SalesPriceId=" & dt.Rows(0)("SalesPriceId"))
            'End If

        End If
        PromotionId = Convert.ToInt32(Request("PromotionId"))
        If Not IsPostBack Then
            mixmatchName.Visible = False
            Utility.Common.BindPromotionType(drpPromotionType, True)

            drlCustomerPriceGroup.DataSource = CustomerPriceGroupRow.GetAllCustomerPriceGroups(DB)
            drlCustomerPriceGroup.DataValueField = "CustomerPriceGroupId"
            drlCustomerPriceGroup.DataTextField = "CustomerPriceGroupCode"
            drlCustomerPriceGroup.DataBind()
            drlCustomerPriceGroup.Items.Insert(0, New ListItem("- - -", ""))

            ' btnSave.Attributes.Add("OnClick", "ReloadFrame('load');")
            LoadFromDB()
            ShowFormByType()
        End If
    End Sub
    Protected Sub lblGetMMDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblGetMMDetail.Click
        If Not String.IsNullOrEmpty(txtMixMatchNo.Text) Then
            Dim mixmatch As MixMatchRow = MixMatchRow.GetByMixMatchNo(txtMixMatchNo.Text)
            If Not mixmatch Is Nothing Then
                mixmatchName.InnerText = mixmatch.Description
                mixmatchName.Visible = True
                ''get SKU 
                Dim sku As String = DB.ExecuteScalar("Select SKU from StoreItem where ItemId=(Select top 1 ItemId from MixMatchLine where MixMatchId=" & mixmatch.Id & " and Value=0)")
                txtSKU.Text = sku
                btnAddSKU.Visible = False
                If mixmatch.CustomerPriceGroupId > 0 Then
                    drlCustomerPriceGroup.SelectedValue = mixmatch.CustomerPriceGroupId
                Else
                    drlCustomerPriceGroup.SelectedValue = String.Empty
                End If
                txtMinimumQuantityPurchase.Text = mixmatch.Mandatory
                txtMinimumQuantityPurchase.Enabled = False
                dtStartDate.Value = mixmatch.StartingDate
                dtEndDate.Value = mixmatch.EndingDate
                'drlCustomerPriceGroup.Enabled = False
                'dtStartDate.Enabled = False
                'dtEndDate.Enabled = False
            End If

        End If
    End Sub
    Private Sub LoadFromDB()
        'trSingleUse.Visible = False

        trUsed.Visible = False
		If PromotionId = 0 Then
			trUsed.Visible = False
			btnDelete.Visible = False
			Exit Sub
        End If
        If PromotionId > 0 Then
            Dim drv As DataView = DB.GetDataView("select * from storeitem where promotionid = " & PromotionId)
            If drv.Count > 0 Then
                txtSKU.Text = drv(0)("SKU").ToString
                hidOldSKU.Value = txtSKU.Text
            End If
        End If
        Dim dbStorePromotion As StorePromotionRow = StorePromotionRow.GetRow(DB, PromotionId)
        txtPromotionName.Text = dbStorePromotion.PromotionName
        txtPromotionCode.Text = dbStorePromotion.PromotionCode
        drpPricingType.SelectedValue = dbStorePromotion.PricingType
        drpPromotionType.SelectedValue = dbStorePromotion.PromotionType
        drlCustomerPriceGroup.SelectedValue = dbStorePromotion.CustomerPriceGroupId
        dtStartDate.Value = dbStorePromotion.StartDate
        dtEndDate.Value = dbStorePromotion.EndDate
        txtMessage.Text = dbStorePromotion.Message
        If Utility.Common.IsPromotionFreeItem(dbStorePromotion.PromotionType) Then
            Dim objMixMatch As MixMatchRow = MixMatchRow.GetRow(DB, dbStorePromotion.MixmatchId)
            If Not objMixMatch Is Nothing Then
                txtMixMatchNo.Text = objMixMatch.MixMatchNo
                mixmatchName.InnerText = objMixMatch.Description
                mixmatchName.Visible = True
                drlCustomerPriceGroup.SelectedValue = objMixMatch.CustomerPriceGroupId
                dtStartDate.Value = objMixMatch.StartingDate
                dtEndDate.Value = objMixMatch.EndingDate
               
            End If
        Else
            txtDiscount.Text = dbStorePromotion.Discount
        End If


        If dbStorePromotion.MinimumPurchase <> Nothing Then txtMinimumPurchase.Text = dbStorePromotion.MinimumPurchase
        If dbStorePromotion.MaximumPurchase <> Nothing Then txtMaximumPurchase.Text = dbStorePromotion.MaximumPurchase
        If dbStorePromotion.MinimumQuantityPurchase <> Nothing Then txtMinimumQuantityPurchase.Text = dbStorePromotion.MinimumQuantityPurchase
       
        chkIsItemSpecific.Checked = dbStorePromotion.IsItemSpecific
        chkIsFreeShipping.Checked = dbStorePromotion.IsFreeShipping
		chkIsActive.Checked = dbStorePromotion.IsActive
        chkIsOneUse.Checked = dbStorePromotion.IsOneUse
        chkIsRegisterSend.Checked = dbStorePromotion.IsRegisterSend
        chkIsProductCoupon.Checked = dbStorePromotion.IsProductCoupon
        chkIsTotalProduct.Checked = dbStorePromotion.IsTotalProduct
        If chkIsActive.Checked = True Then
            chkIsRegisterSend.Enabled = True
        Else
            chkIsRegisterSend.Enabled = False
        End If

        'chkIsSingleUse.Enabled = False
        'If chkIsSingleUse.Checked Then trUsed.Visible = True Else trUsed.Visible = False
		If dbStorePromotion.IsUsed = True Then
			lblUsed.Text = "<a href=""/admin/store/orders/edit.aspx?OrderId=" & DB.ExecuteScalar("select top 1 orderid from storeorder where promotioncode = " & DB.Quote(dbStorePromotion.PromotionCode)) & """>View Order</a>"
			chkIsUsed.Enabled = False
            chkIsOneUse.Enabled = False
			btnSave.Visible = False
			btnDelete.Visible = False
		End If
        chkIsUsed.Checked = dbStorePromotion.IsUsed
        fuImage.CurrentFileName = dbStorePromotion.Image
        hpimg.ImageUrl = ConfigurationManager.AppSettings("PathCoupon") & dbStorePromotion.Image

        If fuImage.CurrentFileName <> Nothing Then
            divImg.Visible = True
            If Right(fuImage.CurrentFileName.ToLower, 4) <> ".swf" Then  Else divImg.InnerHtml = "<script type=""text/javascript"" src=""/includes/flash/homepage.swf.js.aspx?SWF=" & Server.UrlEncode(dbStorePromotion.Image) & """></script>"
        End If
	End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        ltrError.Text = ""
        If Not IsValid Then Exit Sub
        Dim isUpdate As Boolean = False
        Dim objMixMatch As MixMatchRow = Nothing
        Dim customerPriceGroupId As Integer
        Dim isGetFree As Boolean = Utility.Common.IsPromotionFreeItem(drpPromotionType.SelectedValue)
        If Not String.IsNullOrEmpty(drlCustomerPriceGroup.SelectedValue) Then
            customerPriceGroupId = drlCustomerPriceGroup.SelectedValue
        Else
            customerPriceGroupId = 0
        End If
        Dim itemId As Integer = 0
        Try
           
            DB.BeginTransaction()
            If isGetFree Then
                objMixMatch = MixMatchRow.GetByMixMatchNo(txtMixMatchNo.Text)
                If objMixMatch Is Nothing Then
                    ltrError.Text = "Mixmatch No:" & txtMixMatchNo.Text & " is not valid"
                    DB.RollbackTransaction()
                    Exit Sub
                End If
                If String.IsNullOrEmpty(txtSKU.Text) Then
                    ltrError.Text = "Mixmatch No:" & txtMixMatchNo.Text & " is not valid"
                    DB.RollbackTransaction()
                    Exit Sub
                End If
                SQL = " UPDATE MixMatch SET " _
                     & "CustomerPriceGroupId = " & DB.Number(customerPriceGroupId) _
                   & ",StartingDate = " & DB.NullQuote(dtStartDate.Value) _
                   & ",EndingDate = " & DB.NullQuote(dtEndDate.Value) _
                   & " WHERE [Id] = " & DB.Quote(objMixMatch.Id)
                DB.ExecuteSQL(SQL)
                itemId = DB.ExecuteScalar("Select ItemId from StoreItem where ItemId=(Select top 1 ItemId from MixMatchLine where MixMatchId=" & objMixMatch.Id & " and Value=0)")
                Dim validMM As Integer = DB.ExecuteScalar("Select [dbo].[fc_MixMatchLine_CheckItemValid](" & objMixMatch.Id & "," & itemId & ")")
                If (validMM = 0) Then
                    ltrError.Text = "Item #" & txtSKU.Text & " is active at the same time in other mixmatch . Please check again"
                    DB.RollbackTransaction()
                    Exit Sub
                End If

            End If

            Dim dbStorePromotion As StorePromotionRow

            Dim dbStorePromotionOld As New StorePromotionRow

            If PromotionId <> 0 Then
                dbStorePromotion = StorePromotionRow.GetRow(DB, PromotionId)
                dbStorePromotionOld = CloneObject.Clone(dbStorePromotion)

            Else
                dbStorePromotion = New StorePromotionRow(DB)
            End If
            dbStorePromotion.PromotionName = txtPromotionName.Text
            dbStorePromotion.PromotionCode = txtPromotionCode.Text.Trim().ToUpper()
            dbStorePromotion.PromotionType = drpPromotionType.SelectedValue
            dbStorePromotion.PricingType = drpPricingType.SelectedValue
            dbStorePromotion.Message = txtMessage.Text
            dbStorePromotion.CustomerPriceGroupId = customerPriceGroupId

            If Not isGetFree Then
                dbStorePromotion.Discount = txtDiscount.Text
                dbStorePromotion.MixmatchId = 0
            Else
                dbStorePromotion.Discount = 0
                dbStorePromotion.MixmatchId = objMixMatch.Id
            End If

            If txtMinimumPurchase.Text <> "" Then dbStorePromotion.MinimumPurchase = txtMinimumPurchase.Text Else dbStorePromotion.MinimumPurchase = Nothing
            If txtMaximumPurchase.Text <> "" Then dbStorePromotion.MaximumPurchase = txtMaximumPurchase.Text Else dbStorePromotion.MaximumPurchase = Nothing
            If txtMinimumQuantityPurchase.Text <> "" Then dbStorePromotion.MinimumQuantityPurchase = txtMinimumQuantityPurchase.Text Else dbStorePromotion.MinimumQuantityPurchase = Nothing
            dbStorePromotion.StartDate = dtStartDate.Value
            dbStorePromotion.EndDate = dtEndDate.Value
            dbStorePromotion.IsItemSpecific = chkIsItemSpecific.Checked
            dbStorePromotion.IsFreeShipping = chkIsFreeShipping.Checked
            If Not dbStorePromotion.IsUsed = True Then dbStorePromotion.IsUsed = chkIsUsed.Checked
            dbStorePromotion.IsActive = chkIsActive.Checked
            dbStorePromotion.IsTotalProduct = chkIsTotalProduct.Checked
            dbStorePromotion.IsOneUse = chkIsOneUse.Checked
            dbStorePromotion.IsRegisterSend = chkIsRegisterSend.Checked
            dbStorePromotion.IsProductCoupon = chkIsProductCoupon.Checked
            fuImage.Height = 160
            fuImage.Width = 430
            fuImage.AutoResize = True
            Dim arr As String()
            If PromotionId <> 0 Then

                dbStorePromotion.Update()
                isUpdate = True
            Else
                dbStorePromotion.IsOneUse = chkIsOneUse.Checked
                PromotionId = dbStorePromotion.Insert
                isUpdate = False
            End If
            'If isGetFree Then
            '    DB.ExecuteSQL("exec sp_MixMatch_UpdateFromProductCoupon " & PromotionId)
            'End If
            If txtSKU.Text <> Nothing Then
                If itemId < 1 Then
                    itemId = DB.ExecuteScalar("Select ItemId from StoreItem where SKU='" & txtSKU.Text.Trim() & "'")

                End If
                DB.ExecuteScalar("Update storeItem set promotionid = 0 where promotionid = " & PromotionId)
                DB.ExecuteScalar("Update storeItem set promotionid = " & PromotionId & " where ItemId = " & itemId)
                'dbStoreItem = StoreItemRow.GetRow(DB, txtSKU.Text)
                'dbStoreItem.PromotionId = PromotionId
                'dbStoreItem.Update()
                StoreItemRow.ClearItemCache(itemId)
            End If
            DB.ExecuteScalar("Update StorePromotion set MixmatchId=0 where MixmatchId=" & dbStorePromotion.MixmatchId & " and promotionid<>" & dbStorePromotion.PromotionId)

            If PromotionId <> 0 Then
                If fuImage.NewFileName <> String.Empty Then
                    arr = fuImage.NewFileName.Split(".")
                    fuImage.NewFileName = PromotionId.ToString & "." & arr(1)
                    fuImage.SaveNewFile()
                    dbStorePromotion.Image = fuImage.NewFileName
                    dbStorePromotion.Update()
                ElseIf fuImage.MarkedToDelete Then
                    fuImage.RemoveOldFile()
                    dbStorePromotion.Image = Nothing
                    dbStorePromotion.Update()
                    'ElseIf dbStorePromotion.Image = "" Then
                    '    ltMssImage.Text = "Image is required."
                    '    Exit Sub
                End If
                Dim ImagePath As String = Server.MapPath("/assets/coupon/")
                Core.ResizeImage(ImagePath & fuImage.NewFileName, ImagePath & fuImage.NewFileName, 430, 160)
            End If
            'If fuImage.NewFileName <> String.Empty Or fuImage.MarkedToDelete Then fuImage.RemoveOldFile()
            DB.CommitTransaction()
            Dim logDetail As New AdminLogDetailRow
            logDetail.ObjectType = Utility.Common.ObjectType.ProductCoupon.ToString()
            logDetail.ObjectId = PromotionId
            If isUpdate Then
                logDetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.ProductCoupon, dbStorePromotionOld, dbStorePromotion)
                If (hidOldSKU.Value <> txtSKU.Text) Then
                    logDetail.Message = logDetail.Message & "SKU|" & hidOldSKU.Value & "|" & txtSKU.Text & "[br]"
                End If
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            Else
                logDetail.Message = AdminLogHelper.ConvertObjectInsertToLogMesssageString(dbStorePromotion, Utility.Common.ObjectType.ProductCoupon)
                If Not (String.IsNullOrEmpty(txtSKU.Text)) Then
                    logDetail.Message = logDetail.Message & "SKU|" & txtSKU.Text & "[br]"
                End If

                logDetail.Action = Utility.Common.AdminLogAction.Insert.ToString()
            End If
            AdminLogHelper.WriteLuceneLogDetail(logDetail)

            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            ltrError.Text = ErrHandler.ErrorText(ex)
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Response.Redirect("delete.aspx?PromotionId=" & PromotionId & "&" & GetPageParams(FilterFieldType.All))
    End Sub

    'Protected Sub AddSKUtoTextbox_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddSKUtoTextbox.Click
    '    txtSKU.Text = hidPopUpSKU.Value
    'End Sub

    Protected Sub drpPromotionType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpPromotionType.SelectedIndexChanged
        ''Freeitem
        
        ShowFormByType()
    End Sub
    Private Sub ShowFormByType()
        If Utility.Common.IsPromotionFreeItem(drpPromotionType.SelectedValue) Then
            trMixMatchNo.Visible = True
            trDiscount.Visible = False
            rfvMixMatchNo.Enabled = True
            rfvDiscount.Enabled = False
            btnAddSKU.Visible = False
            'drlCustomerPriceGroup.Enabled = False
            'dtStartDate.Enabled = False
            'dtEndDate.Enabled = False
            txtMinimumQuantityPurchase.Enabled = False
        Else
            trMixMatchNo.Visible = False
            trDiscount.Visible = True
            rfvMixMatchNo.Enabled = False
            rfvDiscount.Enabled = True
            btnAddSKU.Visible = True
            'drlCustomerPriceGroup.Enabled = True
            'dtStartDate.Enabled = True
            'dtEndDate.Enabled = True
            txtMinimumQuantityPurchase.Enabled = True
        End If
    End Sub
End Class
