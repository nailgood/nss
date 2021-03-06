Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.Diagnostics
Imports System.Threading
Partial Class admin_store_items_edit
    Inherits AdminPage

    Private ItemId As Integer
    Dim Cart As ShoppingCart
    Private dbStoreItem As StoreItemRow
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        ItemId = Convert.ToInt32(Request("ItemId"))

        If ItemId = 0 Then
            Delete.Visible = False
            RelatedItems.Visible = False
            btnVideo.Visible = False
        End If
        If Not IsPostBack Then
            Page.Form.DefaultFocus = "ItemName"
            LoadFromDB()
            LoadDepartments()
            BindFeatures()
            CountVideo()
            CountRelatedItem()
            CountAlbum()
        Else
            If (hidIsRewardPoint.Value = "1") Then
                cusvRewardPoint.Enabled = True
                txtRewardPoint.CssClass = ""
            Else
                cusvRewardPoint.Enabled = False
                txtRewardPoint.CssClass = "txtDisable"
            End If
            If (hdEbayPrice.Value = "1") Then
                cusEbayPrice.Enabled = True
                txtEbayPrice.CssClass = ""
            Else
                cusEbayPrice.Enabled = False
                txtEbayPrice.CssClass = "txtDisable"
            End If
        End If

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "resetButton", "ResetButton();", True)
    End Sub

    Private Sub LoadDepartments()
        Dim Result As String = ""

        SQL = "SELECT DepartmentId FROM StoreDepartmentItem WHERE ItemId = " & DB.Quote(ItemId)
        Trace.Write(SQL)
        Dim sConn As String = ""
        Dim res As SqlDataReader = Nothing
        Try
            res = DB.GetReader(SQL)
            While res.Read
                Result = Result & sConn & res("DepartmentId")
                sConn = ","
            End While
        Catch ex As Exception

        End Try
        Core.CloseReader(res)
        treeDepartments.CheckedList = Result
    End Sub
    Protected Sub ServerCheckPointValid(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        If (String.IsNullOrEmpty(e.Value)) Then
            e.IsValid = False
            Exit Sub
        End If
        Dim point As Integer
        Try
            point = CInt(e.Value)
        Catch ex As Exception

        End Try
        If (point.ToString().Length <> e.Value.Length) Then
            e.IsValid = False
            Exit Sub
        End If
        If (point < 1) Then
            e.IsValid = False
        Else

            e.IsValid = True
        End If
    End Sub
    Protected Sub ServerCheckEbayPrice(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        If (String.IsNullOrEmpty(e.Value)) Then
            e.IsValid = False
            Exit Sub
        End If
        Dim EbayPrice As Double
        Try
            EbayPrice = CDbl(e.Value)
        Catch ex As Exception

        End Try
        If (EbayPrice.ToString().Length <> e.Value.Length) Then
            e.IsValid = False
            Exit Sub
        End If
        If (EbayPrice < 1) Then
            e.IsValid = False
        Else

            e.IsValid = True
        End If
    End Sub
    Private Sub LoadFromDB()
        cblPostingGroups.DataSource = CustomerPostingGroupRow.GetList(DB, "code")
        cblPostingGroups.DataTextField = "code"
        cblPostingGroups.DataValueField = "code"
        cblPostingGroups.DataBind()

        Status.DataSource = DB.GetDataSet("select * from storeitemstatus")
        Status.DataTextField = "Statusname"
        Status.DataValueField = "status"
        Status.DataBind()

        'cblBaseColor.DataSource = StoreBaseColorRow.GetAllRows(DB)
        'cblBaseColor.DataTextField = "basecolor"
        'cblBaseColor.DataValueField = "basecolorid"
        'cblBaseColor.DataBind()

        'cblCusionColor.DataSource = StoreCusionColorRow.GetAllRows(DB)
        'cblCusionColor.DataTextField = "cusioncolor"
        'cblCusionColor.DataValueField = "cusioncolorid"
        'cblCusionColor.DataBind()

        'cblLaminateTrim.DataSource = StoreLaminateTrimRow.GetAllRows(DB)
        'cblLaminateTrim.DataTextField = "LaminateTrim"
        'cblLaminateTrim.DataValueField = "LaminateTrimid"
        'cblLaminateTrim.DataBind()

        drpBrandId.DataSource = StoreBrandRow.GetAllStoreBrands(DB)
        drpBrandId.DataValueField = "BrandId"
        drpBrandId.DataTextField = "BrandName"
        drpBrandId.DataBind()
        drpBrandId.Items.Insert(0, New ListItem("", "0"))

        ddlCollections.DataSource = StoreCollectionRow.GetAllCollections(DB)
        ddlCollections.DataTextField = "CollectionName"
        ddlCollections.DataValueField = "CollectionId"
        ddlCollections.DataBind()
        ddlCollections.Items.Insert(0, New ListItem("-- Select --", ""))

        ddlTones.DataSource = StoreToneRow.GetAllTones(DB)
        ddlTones.DataTextField = "Tone"
        ddlTones.DataValueField = "ToneId"
        ddlTones.DataBind()
        ddlTones.Items.Insert(0, New ListItem("-- Select --", ""))

        ddlShades.DataSource = StoreShadeRow.GetAllShades(DB)
        ddlShades.DataTextField = "Shade"
        ddlShades.DataValueField = "ShadeId"
        ddlShades.DataBind()
        ddlShades.Items.Insert(0, New ListItem("-- Select --", ""))


        btnGroupItems.Visible = False

        Dim sPromotionid As String = ""
        If ItemId <> 0 Then
            btnAlbum2.Visible = True
            btnAlbum.Visible = True
            btnVideo.Visible = True
            btnVideo2.Visible = True
            btnAlbum.Visible = True
            hidItemId.Value = ItemId
            dbStoreItem = StoreItemRow.GetRow(DB, ItemId)
            If dbStoreItem.IsEbay = True Or dbStoreItem.IsEbayAllow Then
                trEbayShippingType.Visible = True
                drEbayShippingType.SelectedValue = Trim(dbStoreItem.EbayShippingType)
            End If
            If dbStoreItem.ItemGroupId <> Nothing Then
                btnAttributes.Visible = False
                btnGroupItems.Visible = True

                btnAttributes2.Visible = False
                btnGroupItems2.Visible = True

                'trBase.Visible = False
                'trCusion.Visible = False
                'trLaminate.Visible = False
                'Else
                '    cblBaseColor.SelectedValues = dbStoreItem.GetBaseColors()
                '    cblCusionColor.SelectedValues = dbStoreItem.GetCusionColors()
                '    cblLaminateTrim.SelectedValues = dbStoreItem.GetLaminateTrims()
            End If
            CloneItem.Message = "Are you sure you want to clone this item? The item name will be called " & dbStoreItem.ItemName & "_copy"
            CloneItem2.Message = CloneItem.Message

            ItemName.Text = dbStoreItem.ItemName
            ItemNameNew.Text = dbStoreItem.ItemNameNew
            txtPageTitle.Text = dbStoreItem.PageTitle
            txtOutsideUSPageTitle.Text = dbStoreItem.OutsideUSPageTitle
            txtMetaDescription.Text = dbStoreItem.MetaDescription
            txtOutsideUSMetaDescription.Text = dbStoreItem.OutsideUSMetaDescription
            txtMetaKeywords.Text = dbStoreItem.MetaKeywords
            txtImageAltTag.Text = dbStoreItem.ImageAltTag
            chkIsBestSeller.Checked = dbStoreItem.IsBestSeller
            IsActive.Checked = dbStoreItem.IsActive
            IsFirstClassPackage.Checked = dbStoreItem.IsFirstClassPackage
            If Not dbStoreItem.IsActive And dbStoreItem.URLCode = "" Then
                ltrLinkURLCodeDefault.Text = "<input type='button' name='btnGenerateUrlCode' id='btnGenerateUrlCode' onclick='GetItemURLCode(" & dbStoreItem.ItemId & ");' value='Generate' class='btn' />"
                ltrLinkURLCodeDefault.Visible = True
            Else
                ltrLinkURLCodeDefault.Visible = False
            End If

            SKU.Text = dbStoreItem.SKU
            PriceDesc.Text = dbStoreItem.PriceDesc
            txtLowStockMsg.Text = dbStoreItem.LowStockMsg
            txtLowStockThreshold.Text = dbStoreItem.LowStockThreshold
            txtInventoryStockNotification.Text = dbStoreItem.InventoryStockNotification
            If Not String.IsNullOrEmpty(dbStoreItem.URLCode) Then
                txtURLCode.Text = dbStoreItem.URLCode.ToLower()
            End If
            Price.Text = dbStoreItem.Price
            SalePrice.Text = dbStoreItem.SalePrice
            IsNew.Checked = dbStoreItem.IsNew
            chkIsOnSale.Checked = dbStoreItem.IsOnSale
            chkAllowPostEbay.Checked = dbStoreItem.IsEbayAllow
            chkIsEbay.Checked = dbStoreItem.IsEbay
            chkIsEbay.Enabled = False
            chkFlammable.Checked = dbStoreItem.IsFlammable
            Status.SelectedValue = dbStoreItem.Status
            NewUntil.Value = dbStoreItem.NewUntil.ToString("MM/dd/yyyy")
            ShipmentDate.Value = dbStoreItem.ShipmentDate
            If dbStoreItem.BODate <> Nothing Then BODate.Value = FormatDateTime(dbStoreItem.BODate, DateFormat.ShortDate)
            IsTaxFree.Checked = dbStoreItem.IsTaxFree
            Status.SelectedValue = dbStoreItem.Status
            QtyOnHand.Text = dbStoreItem.QtyOnHand
            ShortDesc.Text = dbStoreItem.ShortDesc
            If Not String.IsNullOrEmpty(dbStoreItem.LongDesc) Then

                LongDesc.Text = dbStoreItem.LongDesc.Replace("[br]", Environment.NewLine).Replace("/*", "*")
            End If

            ''dbStoreItem.ShortViet = "[color=#000000]This is description<br>[/color][b][color=#000000]Bold[/color][/b][color=#000000]<br>[/color][u][color=#000000]Underline[/color][/u][color=#000000]<br>[/color][i][color=#000000]Italic[/color][/i][color=#000000]<br>/*Bullet 1<br>/*Bullet 2<br>/*Bullet 3<br>[/color]"
            ShortVietDesc.Text = dbStoreItem.ShortViet
            ''ltrDemoBBCode.Text = BBCodeHelper.ConvertBBCodeToHTML(dbStoreItem.ShortViet)
            LongVietDesc.Text = BBCodeHelper.ConvertBBCodeToBBCodeEditor(dbStoreItem.LongViet)
            ''ltrLongDescViet.Text = BBCodeHelper.ConvertBBCodeToHTML(dbStoreItem.LongViet)
            ShortFrenchDesc.Text = dbStoreItem.ShortFrench
            LongFrenchDesc.Text = BBCodeHelper.ConvertBBCodeToBBCodeEditor(dbStoreItem.LongFrench)
            ShortSpanishDesc.Text = dbStoreItem.ShortSpanish
            LongSpanishDesc.Text = BBCodeHelper.ConvertBBCodeToBBCodeEditor(dbStoreItem.LongSpanish)
            AdditionalInfo.Text = dbStoreItem.AdditionalInfo
            Specifications.Text = dbStoreItem.Specifications
            ShippingInfo.Text = dbStoreItem.ShippingInfo
            HelpfulTips.Text = dbStoreItem.HelpfulTips
            drpBrandId.SelectedValue = dbStoreItem.BrandId
            ShipMethod.Text = dbStoreItem.CarrierType
            chkIsFeatured.Checked = dbStoreItem.IsFeatured
            ddlCollections.SelectedValue = dbStoreItem.GetCollections()
            ddlTones.SelectedValue = dbStoreItem.GetTones()
            ddlShades.SelectedValue = dbStoreItem.GetShades()
            drpItemType.SelectedValue = dbStoreItem.ItemType
            chkIsOversize.Checked = dbStoreItem.IsOversize
            chkIsHazMat.Checked = dbStoreItem.IsHazMat
            If chkIsHazMat.Checked Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "ChangeIsFlammable", "ChangeIsFlammable(" & IIf(chkIsHazMat.Checked, "true", "") & ");", True)
            End If

            chkIsRushDelivery.Checked = dbStoreItem.IsRushDelivery
            txtMaximumQuantity.Text = dbStoreItem.MaximumQuantity
            Weight.Text = dbStoreItem.Weight
            txtRushDeliveryCharge.Text = dbStoreItem.RushDeliveryCharge
            chkIsHot.Checked = dbStoreItem.IsHot
            txtLiftGateCharge.Text = dbStoreItem.LiftGateCharge
            txtScheduleDeliveryCharge.Text = dbStoreItem.ScheduleDeliveryCharge
            chkIsSpecialOrder.Checked = dbStoreItem.IsSpecialOrder
            If dbStoreItem.AcceptingOrder = Utility.Common.ItemAcceptingStatus.AcceptingOrder Then
                chkIsAcceptingOrder.Checked = True
            ElseIf dbStoreItem.AcceptingOrder = Utility.Common.ItemAcceptingStatus.InStock Then
                chkInStock.Checked = True
            End If
            '' chkIsAcceptingOrder.Checked = dbStoreItem.IsAcceptingOrder
            chkIsFreeShipping.Checked = dbStoreItem.IsFreeShipping
            chkIsFreeSample.Checked = dbStoreItem.IsFreeSample
            CheckDescFreeSample()
            cblPostingGroups.SelectedValues = dbStoreItem.GetSelectedPostingGroups
            'chkIsFlatFee.Checked = dbStoreItem.IsFlatFee
            btnSetupFeeTop.Visible = False
            btnSetupFeeBottom.Visible = False
            txtCasePrice.Text = dbStoreItem.CasePrice
            txtCaseQty.Text = dbStoreItem.CaseQty
            txthdItem.Text = dbStoreItem.HandlingFeeForItem
            txthdCase.Text = dbStoreItem.HandlingFeeForCase
            'txtChoiceName.Text = dbStoreItem.ChoiceName
            'If dbStoreItem.IsOversize Then
            '    If dbStoreItem.IsFlatFee Then
            '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "ShowPanelFlatFee", "ShowPanelFlatFee(1,1);", True)
            '        btnSetupFeeTop.Visible = True
            '        btnSetupFeeBottom.Visible = True
            '    Else
            '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "ShowPanelFlatFee", "ShowPanelFlatFee(1,0);", True)
            '    End If

            'Else
            '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "ShowPanelFlatFee", "ShowPanelFlatFee(0,0);", True)
            'End If

            If dbStoreItem.Measurement = 0 Then
                radLiquid.Checked = True
            Else
                radSolid.Checked = True
            End If

            If Not dbStoreItem.LastUpdated = Nothing Then
                ltlLastUpdated.Text = dbStoreItem.LastUpdated
            End If
            fuImage.CurrentFileName = dbStoreItem.Image
            sPromotionid = dbStoreItem.PromotionId
            If (dbStoreItem.IsRewardPoints) Then
                cusvRewardPoint.Enabled = True
                chkIsRewardPoint.Checked = True
                hidIsRewardPoint.Value = "1"
                txtRewardPoint.Text = dbStoreItem.RewardPoints
                txtRewardPoint.Enabled = True
                txtRewardPoint.CssClass = ""
            Else
                cusvRewardPoint.Enabled = False
                chkIsRewardPoint.Checked = False
                hidIsRewardPoint.Value = "0"
                txtRewardPoint.Text = String.Empty
                txtRewardPoint.CssClass = "txtDisable"
            End If
            If (dbStoreItem.IsEbay Or dbStoreItem.IsEbayAllow) Then
                cusEbayPrice.Enabled = True
                hdEbayPrice.Value = "1"
                txtEbayPrice.Text = dbStoreItem.EbayPrice
                txtEbayPrice.CssClass = ""
                txtEbayPrice.Enabled = True
                If (dbStoreItem.IsEbay) Then
                    chkAllowPostEbay.Attributes("onclick") = ""
                End If
            Else
                cusEbayPrice.Enabled = False
                hdEbayPrice.Value = "0"
                txtEbayPrice.Text = String.Empty
                txtEbayPrice.CssClass = "txtDisable"
            End If
            'If (dbStoreItem.IsEbayAllow) Then
            '    cusEbayPrice.Enabled = True
            '    chkAllowPostEbay.Checked = True
            '    hdEbayPrice.Value = "1"
            '    txtEbayPrice.CssClass = ""
            '    txtEbayPrice.Enabled = True
            '    'txtEbayPrice.Text = dbStoreItem.EbayPrice
            'Else
            '    cusEbayPrice.Enabled = False
            '    chkAllowPostEbay.Checked = False
            '    hdEbayPrice.Value = "0"
            '    'txtEbayPrice.Text = String.Empty
            '    txtEbayPrice.CssClass = "txtDisable"
            'End If

            txtMSDS.Text = dbStoreItem.MSDS
            '''Load Handling Fee
            Dim siProp As StoreItemRow = New StoreItemRow()
            siProp = siProp.GetPropertiesByItemId(dbStoreItem.ItemId)
            txthdCase.Text = siProp.HandlingFeeForCase
            txthdItem.Text = siProp.HandlingFeeForItem
            '''
        Else
            cusvRewardPoint.Enabled = False
            cusEbayPrice.Enabled = False
            txtRewardPoint.CssClass = "txtDisable"
            txtEbayPrice.CssClass = "txtDisable"
        End If



    End Sub

    Private Sub Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Save.Click, Save2.Click
        If Not Page.IsValid Then Exit Sub
        'If StoreItemRow.CheckURLCodeDuplicate(txtURLCode.Text, ItemId) Then
        '    AddError("URL Code is alredy exists. Please try again.")
        'Else

        SaveItemDetails("default.aspx?" & GetPageParams(FilterFieldType.All))
        'Cart.RecalculateTotalUpdate()
        '' End If

    End Sub

    Sub SaveItemDetails(ByVal sRedir As String)
        Dim List As String = String.Empty
        Dim sConn As String = String.Empty
        Dim dbStoreItem As New StoreItemRow
        Dim dbPromo As StorePromotionRow
        Dim handlingFeeItem, handlingFeeCase As Double
        Dim isGetHandlingFee As Boolean = False
        Dim dbStoreItemBeforUpdate As StoreItemRow = Nothing
        Try
            If ItemId <> 0 Then
                Dim currentItem As StoreItemRow = StoreItemRow.GetRow(DB, ItemId)
                dbStoreItem = CloneObject.Clone(currentItem)
                dbStoreItemBeforUpdate = CloneObject.Clone(currentItem)
                dbStoreItemBeforUpdate.ListDepartmentId = StoreItemRow.GetListDepartmentId(DB, ItemId)
                dbStoreItemBeforUpdate.ListPostingGroupCode = StoreItemRow.GetListPostingGroupCode(DB, ItemId)
                dbStoreItemBeforUpdate.ListCollectionId = dbStoreItemBeforUpdate.GetCollections()
                dbStoreItemBeforUpdate.ListToneId = dbStoreItemBeforUpdate.GetTones()
                dbStoreItemBeforUpdate.ListShapeId = dbStoreItemBeforUpdate.GetShades()
                dbStoreItemBeforUpdate.ListBaseColorId = StoreItemRow.GetListBaseColorIdByItemId(DB, ItemId)
                dbStoreItemBeforUpdate.ListCusionColorId = StoreItemRow.GetListCusionColorIdByItemId(DB, ItemId)
                dbStoreItemBeforUpdate.ListLaminateColorId = StoreItemRow.GetListLaminateColorIdByItemId(DB, ItemId)
            Else
                dbStoreItem = New StoreItemRow(DB)
            End If
            ''dbStoreItem.LsitDepartmtnt=Get

            ''dbStoreItemBeforUpdate = dbStoreItem
            DB.BeginTransaction()

            Dim OldItemType As String = dbStoreItem.ItemType
            Dim IsFeaturedOrig As Boolean = dbStoreItem.IsFeatured
            Dim CheckIsEbayAllow As Boolean = dbStoreItem.IsEbayAllow

            dbStoreItem.IsEbayAllow = chkAllowPostEbay.Checked
            dbStoreItem.ItemName = ItemName.Text
            dbStoreItem.ItemNameNew = ItemNameNew.Text
            dbStoreItem.PageTitle = txtPageTitle.Text
            dbStoreItem.OutsideUSPageTitle = txtOutsideUSPageTitle.Text
            dbStoreItem.MetaDescription = txtMetaDescription.Text
            dbStoreItem.OutsideUSMetaDescription = txtOutsideUSMetaDescription.Text
            dbStoreItem.MetaKeywords = txtMetaKeywords.Text
            dbStoreItem.IsActive = IsActive.Checked
            dbStoreItem.IsFirstClassPackage = IsFirstClassPackage.Checked
            dbStoreItem.LongDesc = LongDesc.Text
            dbStoreItem.SKU = SKU.Text
            If Not String.IsNullOrEmpty(txtURLCode.Text.Trim()) Then
                dbStoreItem.URLCode = txtURLCode.Text.Trim().ToLower()
            End If
            dbStoreItem.PriceDesc = PriceDesc.Text
            dbStoreItem.Price = Price.Text
            dbStoreItem.SalePrice = IIf(DB.IsEmpty(SalePrice.Text), Nothing, SalePrice.Text)
            dbStoreItem.IsNew = IsNew.Checked
            dbStoreItem.IsBestSeller = chkIsBestSeller.Checked
            dbStoreItem.IsTaxFree = IsTaxFree.Checked
            dbStoreItem.IsOnSale = chkIsOnSale.Checked
            dbStoreItem.BrandId = drpBrandId.SelectedValue
            If (chkIsRewardPoint.Checked) Then
                dbStoreItem.IsRewardPoints = True
                dbStoreItem.RewardPoints = CInt(txtRewardPoint.Text)
            Else
                dbStoreItem.IsRewardPoints = False
                dbStoreItem.RewardPoints = Nothing
            End If
            Try
                dbStoreItem.InventoryStockNotification = txtInventoryStockNotification.Text
            Catch ex As Exception
                dbStoreItem.InventoryStockNotification = 0
            End Try

            dbStoreItem.NewUntil = NewUntil.Value
            dbStoreItem.ShortDesc = ShortDesc.Text
            dbStoreItem.ShortViet = ShortVietDesc.Text
            dbStoreItem.LongViet = BBCodeHelper.ConvertBBCodeToDB(LongVietDesc.Text)
            dbStoreItem.ShortFrench = ShortFrenchDesc.Text
            dbStoreItem.LongFrench = BBCodeHelper.ConvertBBCodeToDB(LongFrenchDesc.Text)
            dbStoreItem.ShortSpanish = ShortSpanishDesc.Text
            dbStoreItem.LongSpanish = BBCodeHelper.ConvertBBCodeToDB(LongSpanishDesc.Text)
            dbStoreItem.Measurement = IIf(radSolid.Checked, 1, 0)

            dbStoreItem.AdditionalInfo = IIf(AdditionalInfo.Text = "<p>&#160;</p>", "", AdditionalInfo.Text)
            dbStoreItem.Specifications = IIf(Specifications.Text = "<p>&#160;</p>", "", Specifications.Text)
            dbStoreItem.ShippingInfo = IIf(ShippingInfo.Text = "<p>&#160;</p>", "", ShippingInfo.Text)

            dbStoreItem.HelpfulTips = HelpfulTips.Text
            dbStoreItem.ImageAltTag = txtImageAltTag.Text
            dbStoreItem.ShipmentDate = ShipmentDate.Value
            dbStoreItem.Status = Status.SelectedValue
            dbStoreItem.QtyOnHand = QtyOnHand.Text
            dbStoreItem.CarrierType = ShipMethod.Text
            dbStoreItem.BODate = BODate.Value
            dbStoreItem.PriceDesc = PriceDesc.Text
            dbStoreItem.IsHot = chkIsHot.Checked
            If Weight.Text <> dbStoreItem.Weight Then
                isGetHandlingFee = True
            End If
            dbStoreItem.Weight = Weight.Text
            dbStoreItem.LowStockMsg = txtLowStockMsg.Text
            If IsNumeric(txtLowStockThreshold.Text) Then dbStoreItem.LowStockThreshold = txtLowStockThreshold.Text Else dbStoreItem.LowStockThreshold = Nothing
            dbStoreItem.IsFeatured = chkIsFeatured.Checked
            dbStoreItem.MSDS = txtMSDS.Text.Trim()
            dbStoreItem.IsOversize = chkIsOversize.Checked
            dbStoreItem.IsHazMat = chkIsHazMat.Checked
            If dbStoreItem.IsHazMat = True Then
                dbStoreItem.IsFlammable = chkFlammable.Checked
            Else
                dbStoreItem.IsFlammable = False
            End If
            dbStoreItem.IsRushDelivery = chkIsRushDelivery.Checked
            dbStoreItem.ItemType = drpItemType.SelectedValue
            dbStoreItem.IsSpecialOrder = chkIsSpecialOrder.Checked
            If chkIsAcceptingOrder.Checked Then
                dbStoreItem.AcceptingOrder = Utility.Common.ItemAcceptingStatus.AcceptingOrder
            ElseIf chkInStock.Checked Then
                dbStoreItem.AcceptingOrder = Utility.Common.ItemAcceptingStatus.InStock
            Else
                dbStoreItem.AcceptingOrder = Utility.Common.ItemAcceptingStatus.None
            End If

            dbStoreItem.RushDeliveryCharge = IIf(IsNumeric(txtRushDeliveryCharge.Text), txtRushDeliveryCharge.Text, Nothing)
            dbStoreItem.LiftGateCharge = IIf(IsNumeric(txtLiftGateCharge.Text), txtLiftGateCharge.Text, Nothing)
            dbStoreItem.ScheduleDeliveryCharge = IIf(IsNumeric(txtScheduleDeliveryCharge.Text), txtScheduleDeliveryCharge.Text, Nothing)
            dbStoreItem.IsFreeShipping = chkIsFreeShipping.Checked
            dbStoreItem.IsFreeSample = IIf(chkIsFreeSample.Checked, 1, 0)
            dbStoreItem.CasePrice = IIf(String.IsNullOrEmpty(txtCasePrice.Text), 0, txtCasePrice.Text)
            dbStoreItem.CaseQty = IIf(String.IsNullOrEmpty(txtCaseQty.Text), 0, txtCaseQty.Text)
            dbStoreItem.HandlingFeeForCase = IIf(String.IsNullOrEmpty(txthdCase.Text), 0, txthdCase.Text)
            dbStoreItem.HandlingFeeForItem = IIf(String.IsNullOrEmpty(txthdItem.Text), 0, txthdItem.Text)
            'dbStoreItem.ChoiceName = txtChoiceName.Text
            If CheckDescFreeSample() = False Then
                DB.RollbackTransaction()
                Exit Sub

            End If
            If dbStoreItem.IsEbay = True Then
                If (dbStoreItem.EbayShippingType <> drEbayShippingType.SelectedValue.ToString) Or (dbStoreItemBeforUpdate.Weight <> dbStoreItem.Weight) Then
                    'Revise
                    DB.ExecuteSQL("update Ebay_ItemSell set QuantityRevise = " & dbStoreItem.QtyOnHand & " where sku = '" & dbStoreItem.SKU & "' and NailIsEbay = 1 ")
                    'Relist
                    'DB.ExecuteSQL("Update Ebay_Item Set [ExpireDate] = GETDATE() where  EbayId in ( SELECT ei.[EbayId]	FROM [Ebay_Item] ei INNER JOIN	[Ebay_ItemSell] eis ON ei.EbayId = eis.EbayId  WHERE(eis.[NailIsEbay] = 1)	AND	SKU ='" & dbStoreItem.SKU & "')")
                End If
                'dbStoreItem.EbayShippingType = IIf(drEbayShippingType.SelectedValue = "", String.Empty, Trim(drEbayShippingType.SelectedValue.ToString))
                'dbStoreItem.EbayPrice = 0
            End If

            If dbStoreItem.IsEbayAllow = False Then
                If dbStoreItem.IsEbayAllow <> CheckIsEbayAllow Then
                    'End Iem on Ebay
                    DB.ExecuteSQL("update Ebay_Item Set [ExpireDate] = '" & DateTime.Now & "' Where EbayId in (select EbayId from Ebay_ItemSell where NailIsEbay = 1 and SKU =  '" & dbStoreItem.SKU & "')")
                    'RunTask(System.Configuration.ConfigurationManager.AppSettings("RunTaskEbay"), "Run Task Ebay")
                    'Else
                    '    DB.ExecuteSQL("update Ebay_Item Set [ExpireDate] = '" & DateTime.Now & "' Where EbayId in (select EbayId from Ebay_ItemSell where NailIsEbay = 1 and SKU =  '" & dbStoreItem.SKU & "')")
                End If
            Else
                If dbStoreItem.IsActive = False Then
                    AddError("Post item to Ebay IsActive must = True")
                    DB.RollbackTransaction()
                    Exit Sub
                End If
                If dbStoreItem.QtyOnHand <= 0 Then
                    AddError("Post item to Ebay QtyOnhand must > 0")
                    DB.RollbackTransaction()
                    Exit Sub
                End If
                'dbStoreItem.EbayPrice = CDbl(txtEbayPrice.Text)
            End If
            If ((dbStoreItem.IsEbay AndAlso ItemId > 0) Or dbStoreItem.IsEbayAllow) Then
                dbStoreItem.EbayPrice = CDbl(txtEbayPrice.Text)
                dbStoreItem.EbayShippingType = IIf(drEbayShippingType.SelectedValue = "", String.Empty, Trim(drEbayShippingType.SelectedValue.ToString))
            End If
            If IsNumeric(txtMaximumQuantity.Text) Then dbStoreItem.MaximumQuantity = txtMaximumQuantity.Text Else dbStoreItem.MaximumQuantity = Nothing
            'If Not dbStoreItem.IsOversize Then
            '    dbStoreItem.IsFlatFee = False
            'Else
            '    dbStoreItem.IsFlatFee = IIf(chkIsFlatFee.Checked, 1, 0)
            'End If
            'If Not dbStoreItem.IsFlatFee Then
            '    dbStoreItem.FeeShipOversize = Nothing
            'End If


            Dim dvImages As DataView = DB.GetDataView("select value from sysparam where groupname = 'Item Image Size Settings' order by sortorder")
            Dim dvPaths As DataView = DB.GetDataView("select value from sysparam where groupname = 'Item Image Size Path Settings' order by sortorder")
            Dim Width As Integer, Height As Integer, vals() As String, ReferenceImage As String, NewImage As String

            Dim OldFileName As String = dbStoreItem.Image
            Dim arr As String()
            Dim ImagePath As String = Server.MapPath("/assets/items/")
            If fuImage.NewFileName <> String.Empty Then
                arr = fuImage.NewFileName.Split(".")
                fuImage.NewFileName = dbStoreItem.SKU & "." & arr(1)
                fuImage.SaveNewFile()
                ReferenceImage = fuImage.Folder & fuImage.NewFileName

                dbStoreItem.Image = fuImage.NewFileName
                Core.ResizeImage(ImagePath & "original/" & fuImage.NewFileName, ImagePath & "large/" & fuImage.NewFileName, 500, 500)
                Core.ResizeImage(ImagePath & "original/" & fuImage.NewFileName, ImagePath & fuImage.NewFileName, 230, 230)
                Core.ResizeImage(ImagePath & "original/" & fuImage.NewFileName, ImagePath & "medium/" & fuImage.NewFileName, 167, 167)
                Core.ResizeImage(ImagePath & "original/" & fuImage.NewFileName, ImagePath & "featured/" & fuImage.NewFileName, 115, 115)
                Core.ResizeImage(ImagePath & "original/" & fuImage.NewFileName, ImagePath & "related/" & fuImage.NewFileName, 80, 80)
                Core.ResizeImage(ImagePath & "original/" & fuImage.NewFileName, ImagePath & "small/" & fuImage.NewFileName, 77, 47) 'Polish
                Core.ResizeImage(ImagePath & "original/" & fuImage.NewFileName, ImagePath & "cart/" & fuImage.NewFileName, 58, 58)
                Core.ResizeImage(ImagePath & "original/" & fuImage.NewFileName, ImagePath & "free/" & fuImage.NewFileName, 35, 35)
            ElseIf fuImage.MarkedToDelete Then
                Utility.File.DeleteFile(ImagePath & "large/" & OldFileName)
                Utility.File.DeleteFile(ImagePath & OldFileName)
                Utility.File.DeleteFile(ImagePath & "medium/" & OldFileName)
                Utility.File.DeleteFile(ImagePath & "featured/" & OldFileName)
                Utility.File.DeleteFile(ImagePath & "related/" & OldFileName)
                Utility.File.DeleteFile(ImagePath & "small/" & OldFileName)
                Utility.File.DeleteFile(ImagePath & "cart/" & OldFileName)
                Utility.File.DeleteFile(ImagePath & "free/" & OldFileName)
                Utility.File.DeleteFile(ImagePath & "original/" & OldFileName)
                dbStoreItem.Image = String.Empty
            End If
            Dim isUpdate As Boolean = False
            If ItemId <> 0 Then
                isUpdate = True
                dbStoreItem.Update()
                DeleteItemCaseSaveCart(dbStoreItem)
                If dbStoreItemBeforUpdate.IsRewardPoints <> dbStoreItem.IsRewardPoints AndAlso dbStoreItem.IsRewardPoints Then
                    dbStoreItem.IPNUpdateItemPoint()
                End If
                If (hidIsDefaultURLCode.Value = "1") Then
                    Email.SendError("ToError500", "Generate URL Code", "SKU: " & dbStoreItem.SKU & "<br>Code: " & dbStoreItem.URLCode & "<br>Item Name: " & dbStoreItem.ItemName & "<br>Price Desc: " & dbStoreItem.PriceDesc)
                End If

            Else
                isUpdate = False
                ItemId = dbStoreItem.AutoInsert()
                dbStoreItem.ItemId = ItemId
                If dbStoreItem.IsRewardPoints Then
                    dbStoreItem.IPNUpdateItemPoint()
                End If

            End If


            dbStoreItem.RemoveAllPostingGroups()
            If Not cblPostingGroups.SelectedValues = String.Empty Then dbStoreItem.InsertPostingGroups(cblPostingGroups.SelectedValues)

            'Save Selected Departments
            dbStoreItem.RemoveDepartmentItems()
            dbStoreItem.InsertDepartmentItems(treeDepartments.CheckedList)

            dbStoreItem.RemoveAllCollections()
            dbStoreItem.InsertCollections(ddlCollections.SelectedValue)

            dbStoreItem.RemoveAllTones()
            dbStoreItem.InsertTones(ddlTones.SelectedValue)

            dbStoreItem.RemoveAllShades()
            dbStoreItem.InsertShades(ddlShades.SelectedValue)

            If isGetHandlingFee Then
                handlingFeeItem = StoreItemRow.GetHandlingFee(DB, dbStoreItem.ItemId, 0, 0)
                handlingFeeCase = StoreItemRow.GetHandlingFee(DB, dbStoreItem.ItemId, 1, 0)
                If dbStoreItem.HandlingFeeForItem < handlingFeeItem Then
                    dbStoreItem.HandlingFeeForItem = handlingFeeItem
                End If
                If dbStoreItem.HandlingFeeForCase < handlingFeeCase Then
                    dbStoreItem.HandlingFeeForCase = handlingFeeCase
                End If
            End If

            Dim siPro As StoreItemRow = StoreItemRow.GetPropertiesByItemId(dbStoreItem.ItemId)
            If siPro.ItemProId > 0 Then
                dbStoreItem.UpdateItemProperties(dbStoreItem)
            Else
                dbStoreItem.InsertItemProperties(dbStoreItem)
            End If


            'dbStoreItem.RemoveAllBaseColors()
            'dbStoreItem.InsertBaseColors(cblBaseColor.SelectedValues)

            'dbStoreItem.RemoveAllCusionColors()
            'dbStoreItem.InsertCusionColors(cblCusionColor.SelectedValues)

            'dbStoreItem.RemoveAllLaminateTrims()
            'dbStoreItem.InsertLaminateTrims(cblLaminateTrim.SelectedValues)

            Dim chkExcludeDiscount As HtmlInputCheckBox = CType(Page.FindControl("chkExcludeDiscount"), HtmlInputCheckBox)

            If dbStoreItem.IsFeatured AndAlso Not IsFeaturedOrig Then
                DB.ExecuteSQL("insert into wishlistupdateitems (itemid,purposeid,CustomerPriceGroupId,MemberId) values (" & ItemId & ",3,null,null)")
            End If

            DB.CommitTransaction()
            Dim logDetail As New AdminLogDetailRow
            logDetail.ObjectId = ItemId
            logDetail.ObjectType = Utility.Common.ObjectType.StoreItem.ToString()
            ''logSubject = "Insert"
            Dim changeLog As String = String.Empty
            If isUpdate Then
                dbStoreItem.ListDepartmentId = StoreItemRow.GetListDepartmentId(DB, ItemId)
                dbStoreItem.ListPostingGroupCode = StoreItemRow.GetListPostingGroupCode(DB, ItemId)
                dbStoreItem.ListCollectionId = dbStoreItemBeforUpdate.GetCollections()
                dbStoreItem.ListToneId = dbStoreItem.GetTones()
                dbStoreItem.ListShapeId = dbStoreItem.GetShades()
                dbStoreItem.ListBaseColorId = StoreItemRow.GetListBaseColorIdByItemId(DB, ItemId)
                dbStoreItem.ListCusionColorId = StoreItemRow.GetListCusionColorIdByItemId(DB, ItemId)
                dbStoreItem.ListLaminateColorId = StoreItemRow.GetListLaminateColorIdByItemId(DB, ItemId)
                changeLog = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.StoreItem, dbStoreItemBeforUpdate, dbStoreItem)
                logDetail.Message = changeLog
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            Else
                logDetail.Message = AdminLogHelper.ConvertObjectInsertToLogMesssageString(dbStoreItem, Utility.Common.ObjectType.StoreItem)
                logDetail.Action = Utility.Common.AdminLogAction.Insert.ToString()
            End If
            ''Chay tool ExportLuceneItem sau khi update/insert item
            ProcessCallTool(Utility.ConfigData.LuceneAppPath(), "ExportLuceneItem")
            ProcessCallTool(Utility.ConfigData.SendReminderAppPath(), "SendAvailabilityReminders")
            ''''''''''''''''''''
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
            If Request("act") = "FreeSample" Then

                Response.Redirect("../../promotions/freesamples/default.aspx?" & GetPageParams(FilterFieldType.All))
            Else
                If sRedir <> "" Then Response.Redirect(sRedir)
            End If


        Catch ex As ApplicationException
            Email.SendError("ToError500", "Error Edit Item: " & dbStoreItem.SKU & " - " & dbStoreItem.ItemName, "Message: " & ex.Message & ", Stack trace:" & ex.StackTrace)
            AddError(ex.Message)
        Catch ex As SqlException
            Email.SendError("ToError500", "Error Edit Item: " & dbStoreItem.SKU & " - " & dbStoreItem.ItemName, "Message: " & ex.Message & ", Stack trace:" & ex.StackTrace)
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
    'Private Sub WriteLog(ByVal subject As String, ByVal dbStoreItem As StoreItemRow)
    '    Dim logId As Integer = IIf(Session("LogId") Is Nothing, 0, Session("LogId"))
    '    Dim objLogDetail As New AdminLogDetailRow
    '    objLogDetail.LogId = logId
    '    objLogDetail.Message = Utility.Common.ObjectToString(dbStoreItem)
    '    objLogDetail.Subject = subject
    '    objLogDetail.CreatedDate = Now
    '    AdminLogDetailRow.Insert(DB, objLogDetail)
    'End Sub
    Private Sub DeleteItemCaseSaveCart(ByVal si As StoreItemRow)
        Try
            If si.CasePrice = 0 Or si.CaseQty = 0 Then
                Dim CountId As Integer = 0
                Try
                    CountId = DB.ExecuteScalar("select Count(Id) from SaveCart where ItemId = " & si.ItemId & " and [Type] = 'case'")
                Catch ex As Exception

                End Try
                If CountId > 0 Then
                    SaveCartRow.Delete(si.ItemId, 0, True)
                End If
            End If
        Catch

        End Try

    End Sub
    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click, Cancel2.Click
        If Request("act") = "FreeSample" Then
            Response.Redirect("../../promotions/freesamples/default.aspx?" & GetPageParams(FilterFieldType.All))
        Else
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        End If

    End Sub
    Private Sub Video_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVideo.Click, btnVideo2.Click
        Response.Redirect("video.aspx?Itemid=" & ItemId & "&" & GetPageParams(FilterFieldType.All))
        ''SaveItemDetails("video.aspx?Itemid=" & ItemId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
    Private Sub RelatedItems_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RelatedItems.Click, RelatedItems2.Click
        ''  If Not Page.IsValid Then Exit Sub
        Response.Redirect("related.aspx?Itemid=" & ItemId & "&" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Delete.Click, Delete2.Click
        Response.Redirect("delete.aspx?ItemId=" & ItemId & "&" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub btnGroupItems_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGroupItems.Click, btnGroupItems2.Click
        Response.Redirect("/admin/store/groups/items.aspx?ItemGroupId=" & DB.ExecuteScalar("select top 1 itemgroupid from storeitem where itemid = " & ItemId))
    End Sub

    Private Sub btnAttributes_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAttributes.Click, btnAttributes2.Click
        Response.Redirect("/admin/store/items/attributes.aspx?ItemId=" & ItemId)
    End Sub
    Private Sub btnAlbum_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAlbum.Click, btnAlbum2.Click
        Response.Redirect("album.aspx?ItemId=" & ItemId)
    End Sub
    Private Sub BindFeatures()
        If dbStoreItem Is Nothing Then
            dbStoreItem = StoreItemRow.GetRow(DB, ItemId)
        End If
        rptFeatures.DataSource = dbStoreItem.GetFeatureFilters()
        rptFeatures.DataBind()
    End Sub

    Private Sub lnkAddFeature_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkAddFeature.Click
        tblFeature.Visible = True
        txtFeature.Text = Nothing
        lblFeatureId.Text = 0
    End Sub

    Private Sub btnFeatureCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFeatureCancel.Click
        tblFeature.Visible = False
        lblFeatureId.Text = 0
    End Sub

    Public Sub ServerValidationItemURLCode(ByVal source As Object, ByVal args As ServerValidateEventArgs)
        args.IsValid = Not StoreItemRow.CheckURLCodeDuplicate(args.Value, ItemId)
    End Sub

    Public Sub ServerValidationItemSKU(ByVal source As Object, ByVal args As ServerValidateEventArgs)
        args.IsValid = Not StoreItemRow.CheckSKUDuplicate(args.Value, ItemId)
    End Sub

    Private Sub btnFeaturesave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFeaturesave.Click
        If Not IsValid Then Exit Sub
        Dim dbFeature As StoreItemFeatureFilterRow = StoreItemFeatureFilterRow.GetRow(DB, lblFeatureId.Text)
        dbFeature.ItemId = ItemId
        dbFeature.Name = txtFeature.Text
        If dbFeature.FeatureId = Nothing Then
            StoreItemFeatureFilterRow.InsertData(dbFeature)
        Else
            StoreItemFeatureFilterRow.UpdateData(dbFeature)
        End If
        txtFeature.Text = Nothing
        tblFeature.Visible = False
        lblFeatureId.Text = 0
        BindFeatures()
    End Sub

    Private Sub rptFeatures_ItemCommand(ByVal sender As Object, ByVal e As RepeaterCommandEventArgs) Handles rptFeatures.ItemCommand
        Select Case e.CommandName
            Case "Modify"
                tblFeature.Visible = True
                lblFeatureId.Text = e.CommandArgument
                Dim dbFeature As StoreItemFeatureFilterRow = StoreItemFeatureFilterRow.GetRow(DB, e.CommandArgument)
                txtFeature.Text = dbFeature.Name
            Case "Remove"
                StoreItemFeatureFilterRow.RemoveRow(DB, e.CommandArgument)
                BindFeatures()
        End Select
    End Sub
    Private Sub CountVideo()
        Dim dtVideo As DataTable = DB.GetDataTable("select si.*,siv.url, isnull(siv.description,'') as description from StoreItem si inner join storeitemvideo siv on si.ItemId = siv.itemid where  si.itemid = " & ItemId & " order by Arrange asc")
        If dtVideo.Rows.Count > 0 Then
            btnVideo.Text = btnVideo.Text & "(" + dtVideo.Rows.Count.ToString() + ")"
            btnVideo2.Text = btnVideo.Text
        End If
    End Sub
    Private Sub CountRelatedItem()
        Dim SQL As String = ""
        SQL &= " SELECT ri.Id, ri.ParentId, si.ItemId, si.SKU, si.ItemName, si.IsActive FROM StoreItem si, RelatedItem ri"
        SQL &= " where si.ItemId = ri.ItemId"
        SQL &= "   and ri.ParentId = " & DB.Quote(ItemId)
        SQL &= " ORDER BY ri.SortOrder"
        Dim res As DataSet = DB.GetDataSet(SQL)
        If res.Tables(0).Rows.Count > 0 Then
            RelatedItems.Text = RelatedItems.Text & "(" + res.Tables(0).Rows.Count.ToString() + ")"
            RelatedItems2.Text = RelatedItems.Text
        End If
    End Sub
    Private Sub CountAlbum()
        Dim CountAlbum As Integer = DB.ExecuteScalar("Select count(ItemId) from AlbumItem where ItemId = " & ItemId)
        btnAlbum2.Text = btnAlbum2.Text & "(" & CountAlbum & ")"
        btnAlbum.Text = btnAlbum.Text & "(" & CountAlbum & ")"

    End Sub
    Function CheckDescFreeSample() As Boolean
        Dim result As Boolean = True
        ''check short, long description is blank
        'If chkIsFreeSample.Checked = 0 Then
        '    If LongDesc.Text = Nothing Then
        '        lblLongDesc.Visible = True
        '        result = False
        '    End If
        '    If ShortDesc.Text = Nothing Then
        '        lblShortDesc.Visible = True
        '        result = False
        '    End If
        'End If
        Return result
    End Function
    Protected Sub RunTask(ByVal sPath As String, ByVal sErrorMessage As String)
        Dim pExportProcess As New Diagnostics.Process()
        Try
            pExportProcess.StartInfo.FileName = sPath
            pExportProcess.StartInfo.UseShellExecute = False
            pExportProcess.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(sPath)
            ' pExportProcess.StartInfo.WindowStyle = Diagnostics.ProcessWindowStyle.Hidden

            pExportProcess.Start()
            pExportProcess.WaitForExit()
            If pExportProcess.ExitCode = 0 Then
                'Response.Redirect("edit.aspx")
            Else
                AddError(sErrorMessage & ". Please try again.")
            End If
            pExportProcess.Close()
        Catch ex As ComponentModel.Win32Exception
            AddError("An error occur when processing: " & ex.Message)
        End Try
    End Sub

    Private Sub ProcessCallTool(ByVal PathFile As String, ByVal AppName As String)
        Dim filePath As String = PathFile 'Utility.ConfigData.LuceneAppPath()
        If (System.IO.File.Exists(filePath)) Then
            ''set duong dan LuceneApp va time cho de tool chay export 
            Dim info As New System.Diagnostics.ProcessStartInfo(filePath, "update|" & Utility.ConfigData.LuceneAppWaitMinute())
            Dim p As New System.Diagnostics.Process()
            p.StartInfo = info
            Dim sAppName As String = AppName '"ExportLuceneItem"
            If Not filePath.Contains(sAppName & ".exe") Then
                Dim i As Integer = filePath.LastIndexOf("\")
                If i < 1 Then
                    i = filePath.LastIndexOf("/")
                End If
                If i > 0 Then
                    sAppName = filePath.Substring(i + 1)
                End If
                Dim j As Integer = sAppName.LastIndexOf(".")
                If j > 0 Then
                    sAppName = sAppName.Substring(0, j)
                End If
            End If
            If Not IsProcessOpen(sAppName) Then
                p.Start()
            End If
        End If
    End Sub
    Private Function IsProcessOpen(ByVal name As String) As Boolean
        For Each clsProcess As Process In Process.GetProcesses()
            If clsProcess.ProcessName.Contains(name) Then
                Return True
            End If
        Next
        Return False
    End Function
End Class
