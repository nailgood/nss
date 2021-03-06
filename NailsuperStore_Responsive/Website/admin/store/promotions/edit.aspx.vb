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


        PromotionId = Convert.ToInt32(Request("PromotionId"))
        If Not IsPostBack Then
            ' btnSave.Attributes.Add("OnClick", "ReloadFrame('load');")
            Utility.Common.BindPromotionType(drpPromotionType, True)
            drlCustomerPriceGroup.DataSource = CustomerPriceGroupRow.GetAllCustomerPriceGroups(DB)
            drlCustomerPriceGroup.DataValueField = "CustomerPriceGroupId"
            drlCustomerPriceGroup.DataTextField = "CustomerPriceGroupCode"
            drlCustomerPriceGroup.DataBind()
            drlCustomerPriceGroup.Items.Insert(0, New ListItem("- - -", ""))
            LoadFromDB()
            ShowFormByType()
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

        Dim dbStorePromotion As StorePromotionRow = StorePromotionRow.GetRow(DB, PromotionId)
        txtPromotionName.Text = dbStorePromotion.PromotionName
        txtPromotionCode.Text = dbStorePromotion.PromotionCode
        drpPricingType.SelectedValue = dbStorePromotion.PricingType
        drpPromotionType.SelectedValue = dbStorePromotion.PromotionType
        drlCustomerPriceGroup.SelectedValue = dbStorePromotion.CustomerPriceGroupId
        If Utility.Common.IsPromotionFreeItem(dbStorePromotion.PromotionType) Then
            Dim lstPromotionItem As StorePromotionItemCollection = StorePromotionItemRow.GetListByPromotion(DB, PromotionId)
            If Not lstPromotionItem Is Nothing Then
                If lstPromotionItem.Count > 0 Then
                    Dim si As StoreItemRow
                    Dim html As String = ""
                    For Each pitem As StorePromotionItemRow In lstPromotionItem
                        si = StoreItemRow.GetRow(DB, pitem.ItemId)
                        If Not hidSelectSKU.Value.Contains(si.SKU & ";") Then
                            html = html & "<tr><td>" & si.SKU & " - " & si.ItemName & "</td><td><img style='padding-left:10px;cursor:pointer;' onClick=""return DeleteFreeItem('" & si.SKU & "')"" style='border-width:0px; cursor:pointer;' alt='Delete' src='/includes/theme-admin/images/icon-remove.png'></td></tr>"
                            hidSelectSKU.Value = hidSelectSKU.Value & si.SKU & ";"
                        End If
                    Next
                    If Not String.IsNullOrEmpty(html) Then
                        html = "<table>" & html & "</table>"
                    End If
                    ''End If
                    ltrFreeItem.Text = html
                    hidDeleteSKU.Value = ""
                End If
            End If
        Else
            txtDiscount.Text = dbStorePromotion.Discount
        End If
        txtMessage.Text = dbStorePromotion.Message

        If dbStorePromotion.MinimumPurchase <> Nothing Then txtMinimumPurchase.Text = dbStorePromotion.MinimumPurchase
        If dbStorePromotion.MaximumPurchase <> Nothing Then txtMaximumPurchase.Text = dbStorePromotion.MaximumPurchase
        dtStartDate.Value = dbStorePromotion.StartDate
        dtEndDate.Value = dbStorePromotion.EndDate
        chkIsItemSpecific.Checked = dbStorePromotion.IsItemSpecific
        chkIsFreeShipping.Checked = dbStorePromotion.IsFreeShipping
        chkIsActive.Checked = dbStorePromotion.IsActive
        chkIsOneUse.Checked = dbStorePromotion.IsOneUse
        chkIsRegisterSend.Checked = dbStorePromotion.IsRegisterSend
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
    Private Sub GetSKU_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetSKU.Click
        If hidPopUpSKU.Value <> "" Or hidDeleteSKU.Value <> "" Then
            Dim source As String = hidSelectSKU.Value & hidPopUpSKU.Value.Trim()
            hidSelectSKU.Value = ""
            Dim arr As Array = Split(source.Trim(), ";")
            ''If arr(0) <> "thisForm" Then
            Dim si As StoreItemRow
            Dim html As String = ""
            For i As Integer = 0 To arr.Length - 1
                If (arr(i).ToString() <> "") Then

                    si = StoreItemRow.GetRowSku(DB, arr(i).ToString())
                    If Not (si.SKU.Equals(hidDeleteSKU.Value)) Then
                        If Not hidSelectSKU.Value.Contains(si.SKU & ";") Then
                            html = html & "<tr><td>" & si.SKU & " - " & si.ItemName & "</td><td><img style='padding-left:10px;cursor:pointer;' onClick=""return DeleteFreeItem('" & si.SKU & "')"" style='border-width:0px; cursor:pointer;' alt='Delete' src='/includes/theme-admin/images/icon-remove.png'></td></tr>"
                            hidSelectSKU.Value = hidSelectSKU.Value & si.SKU & ";"
                        End If
                    End If
                End If
            Next
            If Not String.IsNullOrEmpty(html) Then
                html = "<table>" & html & "</table>"
            End If
            ''End If
            ltrFreeItem.Text = html
            hidDeleteSKU.Value = ""
        End If

    End Sub

    Private Function CheckPromotionCode(ByVal PromotionCode As String) As Boolean
        Dim dt As DataTable = DB.GetDataTable("select * from StorePromotion WHERE promotionid is not null and PromotionCode = '" & PromotionCode & "'")
        If dt.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function
    Protected Sub drpPromotionType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpPromotionType.SelectedIndexChanged
        ''Freeitem

        ShowFormByType()
    End Sub
    Private Sub ShowFormByType()
        If Utility.Common.IsPromotionFreeItem(drpPromotionType.SelectedValue) Then
            trFreeItem.Visible = True
            trDiscount.Visible = False
            rfvDiscount.Enabled = False
           

        Else
            trFreeItem.Visible = False
            trDiscount.Visible = True
            rfvDiscount.Enabled = True
           
        End If
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Dim isUpdate As Boolean = False
        Dim lstSKUFree As New System.Collections.Generic.List(Of String)
        rfvFreeItem.Visible = False
        Try
            Dim customerPriceGroupId As Integer = 0
            If Not String.IsNullOrEmpty(drlCustomerPriceGroup.SelectedValue) Then
                customerPriceGroupId = drlCustomerPriceGroup.SelectedValue
            End If
            Dim discount As Double = 0
            If Utility.Common.IsPromotionFreeItem(drpPromotionType.SelectedValue) Then
                Dim source As String = hidSelectSKU.Value
                Dim arrSKU As Array = Split(source.Trim(), ";")
                Dim html As String = ""
                For i As Integer = 0 To arrSKU.Length - 1
                    If (arrSKU(i).ToString() <> "") Then
                        lstSKUFree.Add(arrSKU(i))
                    End If
                Next
                If lstSKUFree.Count < 1 Then
                    rfvFreeItem.Visible = True
                    Exit Sub
                End If
            Else
                discount = txtDiscount.Text
            End If
            DB.BeginTransaction()

            Dim dbStorePromotion As StorePromotionRow
            Dim dbStorePromotionOld As New StorePromotionRow
            If PromotionId <> 0 Then
                dbStorePromotion = StorePromotionRow.GetRow(DB, PromotionId)
                dbStorePromotionOld = CloneObject.Clone(dbStorePromotion)
            Else
                dbStorePromotion = New StorePromotionRow(DB)
            End If
            dbStorePromotion.CustomerPriceGroupId = customerPriceGroupId
            dbStorePromotion.PromotionName = txtPromotionName.Text
            dbStorePromotion.PromotionType = drpPromotionType.SelectedValue
            dbStorePromotion.PricingType = drpPricingType.SelectedValue
            dbStorePromotion.Message = txtMessage.Text
            dbStorePromotion.Discount = discount
            If txtMinimumPurchase.Text <> "" Then dbStorePromotion.MinimumPurchase = txtMinimumPurchase.Text Else dbStorePromotion.MinimumPurchase = Nothing
            If txtMaximumPurchase.Text <> "" Then dbStorePromotion.MaximumPurchase = txtMaximumPurchase.Text Else dbStorePromotion.MaximumPurchase = Nothing
            dbStorePromotion.StartDate = dtStartDate.Value
            dbStorePromotion.EndDate = dtEndDate.Value
            dbStorePromotion.IsItemSpecific = chkIsItemSpecific.Checked
            dbStorePromotion.IsFreeShipping = chkIsFreeShipping.Checked
            If Not dbStorePromotion.IsUsed = True Then dbStorePromotion.IsUsed = chkIsUsed.Checked
            dbStorePromotion.IsActive = chkIsActive.Checked
            dbStorePromotion.IsOneUse = chkIsOneUse.Checked
            dbStorePromotion.IsRegisterSend = chkIsRegisterSend.Checked
            fuImage.Height = 160
            fuImage.Width = 430
            fuImage.AutoResize = True
            Dim arr As String()
            If PromotionId <> 0 Then
                If CheckPromotionCode(txtPromotionCode.Text) = True And dbStorePromotion.PromotionCode.Trim().ToUpper() <> txtPromotionCode.Text.Trim().ToUpper() Then
                    ltrError.Text = "Duplicated promotion code: " & txtPromotionCode.Text
                    Exit Sub
                End If
                dbStorePromotion.PromotionCode = txtPromotionCode.Text.Trim().ToUpper()
                dbStorePromotion.Update()
                DB.ExecuteSQL("Delete from StorePromotionItem where PromotionId=" & PromotionId)
                isUpdate = True
            Else
                If CheckPromotionCode(txtPromotionCode.Text) = True Then
                    ltrError.Text = "Duplicated promotion code: " & txtPromotionCode.Text
                    Exit Sub
                End If
                dbStorePromotion.PromotionCode = txtPromotionCode.Text.Trim().ToUpper()
                dbStorePromotion.IsOneUse = chkIsOneUse.Checked
                PromotionId = dbStorePromotion.Insert
                isUpdate = False
            End If

            If PromotionId <> 0 Then
                ''Update Promotion Item
                ''DB.ExecuteSQL("Delete from StorePromotionItem where PromotionId=" & PromotionId)
                If Utility.Common.IsPromotionFreeItem(drpPromotionType.SelectedValue) Then
                    StorePromotionItemRow.InsertListPromotionItem(DB, lstSKUFree, PromotionId)
                End If
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
            ' If fuImage.NewFileName <> String.Empty Or fuImage.MarkedToDelete Then fuImage.RemoveOldFile()
            DB.CommitTransaction()
            Dim logDetail As New AdminLogDetailRow
            logDetail.ObjectType = Utility.Common.ObjectType.OrderCoupon.ToString()
            logDetail.ObjectId = PromotionId
            If isUpdate Then
                logDetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.OrderCoupon, dbStorePromotionOld, dbStorePromotion)
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            Else
                logDetail.Message = AdminLogHelper.ConvertObjectInsertToLogMesssageString(dbStorePromotion, Utility.Common.ObjectType.OrderCoupon)
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
End Class
