Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Collections.Generic

Public Class class_item
    Inherits SitePage

    Public reviewCount As Integer = 0
    Private ItemId As Integer = Integer.MinValue
    Public ZoomImagePath As String = String.Empty
    Public ImagePath As String = String.Empty
    Protected Item As StoreItemRow
    Protected ItemGroupId As Integer
    Protected MediaUrl As String = Utility.ConfigData.MediaUrl
    Public AcceptingOrderMessage As String = String.Empty
    Public AcceptingOrderName As String = String.Empty
    Public isAcceptingOrder As Boolean = False
    Public isAllowAddWishlist As Boolean = False
    Private MemberId As Integer = 0
    Public strPriceDesc As String = String.Empty
    Public strLongDescription As String = String.Empty
    Protected AggregateRating As String = "itemprop=""aggregateRating"" itemscope itemtype=""http://schema.org/AggregateRating"""
    Public Function GetFaceBookUrl() As String
        Return WebRoot & Me.Request.RawUrl
    End Function
    Public isMusicItem As Boolean = False
    Private Function CheckMusicItem(ByVal itemId As Integer) As Boolean
        If AlbumRow.CountByItemId(DB, itemId) > 0 Then
            Return True
        End If
        Return False
    End Function

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Not IsPostBack Then

            Try
                ItemId = GetQueryString("ItemId")
            Catch ex As Exception
                Response.Redirect("/deals-center")
            End Try

            If ItemId < 1 Then
                Response.Redirect("/deals-center")
            End If

            MemberId = Utility.Common.GetCurrentMemberId()
            Item = StoreItemRow.GetRow(DB, ItemId, MemberId)
            If Item Is Nothing Then
                Response.Redirect("/deals-center")
            End If

            If String.IsNullOrEmpty(Item.URLCode) And Not String.IsNullOrEmpty(Item.ItemName) Then
                Utility.Common.CheckValidURLCode(URLParameters.ReplaceUrl(HttpUtility.UrlEncode(Item.ItemName.ToLower())))
            Else
                Utility.Common.CheckValidURLCode(Item.URLCode)
            End If

            If Item.IsActive = False Then
                'Check has related item
                Dim firstrelatedURLCode As String = DB.ExecuteScalar("Select top 1 it.URLCode FROM RelatedItem rli left join StoreItem it on (it.ItemId=rli.ItemId)   where ParentId = " & Item.ItemId & " and IsActive=1 and  IsRedirect=1")
                If Not String.IsNullOrEmpty(firstrelatedURLCode) Then
                    Utility.Common.Redirect301(URLParameters.ProductUrl(firstrelatedURLCode, ItemId))
                End If

                AddError("It is no longer available. Please select a different item.")
            End If

            Session("DepartmentId") = Item.GetDefaultDepartmentId()
            If Utility.Common.IsItemAcceptingOrder(Item.AcceptingOrder) OrElse Item.IsSpecialOrder Then
                Item.QtyOnHand = 9999
            End If
            BindData()

            Dim addCartRelated As String = String.Empty
            If (Not Session("AddCartRelated") Is Nothing) Then
                addCartRelated = Session("AddCartRelated")
            End If

            If (addCartRelated = "1") Then
                Session("AddCartRelated") = Nothing
            Else
                Utility.Common.AddPointKeyword(ItemId, Utility.Common.ItemAction.Detail, SysParam.GetValue("SearchPointDetail"))
            End If

            Dim averageStars As Double = 0
            StoreItemReviewRow.GetReviewData(DB, Item.ItemId, reviewCount, averageStars)
            If reviewCount > 0 Then
                ltrHeaderReviewCount.Text = "<span itemprop=""reviewCount"">" & reviewCount.ToString() & "</span> Review"
            Else
                ltrHeaderReviewCount.Text = reviewCount.ToString() & " Review"
                AggregateRating = String.Empty
            End If
            If reviewCount = 0 Or reviewCount > 1 Then
                ltrHeaderReviewCount.Text &= "s"
            End If

            ''ltrReviewCount.Text = ltrHeaderReviewCount.Text
            ltrHeaderReview.Text = "<a  href='#' onclick=""return ScrollToSection('#item-detail .data .review-section');"">"
            If (reviewCount < 1) Then
                'ltrHeaderReview.Text &= "<img src=""" & Utility.ConfigData.CDNmediapath & "/includes/theme/images/icon-star-empty.png"" /><span style='display:none;' itemprop='ratingValue'>0</span>"
                'ltrHeaderReview.Text &= "<img src=""" & Utility.ConfigData.CDNMediaPath & "/includes/theme/images/icon-star-empty.png"" />"
                ltrHeaderReview.Text &= "<i class=""fa fa-star-o fa-1""></i>"
            Else
                'ltrHeaderReview.Text &= String.Format("<img src=""" & Utility.ConfigData.CDNMediaPath & "/includes/theme/images/star{0}.png"" /><span style='display:none;' itemprop='ratingValue'>{1}</span>", IIf(averageStars.ToString().Length > 1, averageStars.ToString().Replace(".", ""), averageStars.ToString() & "0"), averageStars.ToString())
                ltrHeaderReview.Text &= SitePage.BindIconStar(averageStars.ToString())
            End If
            ltrHeaderReview.Text &= "</a>" & IIf(reviewCount > 0, String.Format("<span class=""show-snippet"" itemprop=""ratingValue"">{0}</span>", averageStars.ToString()), "")

            ''  ltrReview.Text = ltrHeaderReview.Text
            Dim countRelated As Integer = StoreItemRow.CountRelatedItem(Item.ItemId)
            If (countRelated > 0) Then
                divRelated.Visible = True
                ltrHeaderRelated.Text = "<a href='javascript:void(0)' onclick=""return ScrollToSection('#item-detail .data .related');"">Customers Also Bought</a>"
                ucRelatedItem.ItemId = Item.ItemId
                ucRelatedItem.ItemGroupId = Item.ItemGroupId
            Else
                ltrHeaderRelated.Text = String.Empty
                divRelated.Visible = False
            End If

        End If

    End Sub


#Region "Load Data"
    Private Sub SetSocialNetwork(ByVal objItem As StoreItemRow)
        Dim objMetaTag As New MetaTag

        Dim isUSCustomer As Boolean = Utility.Common.IsUSCustomer()
        objMetaTag.PageTitle = Utility.Common.GetPageTitleByCustomerType(isUSCustomer, Item.PageTitle, Item.OutsideUSPageTitle, Item.ItemName)

        objMetaTag.MetaKeywords = Item.MetaKeywords
        objMetaTag.MetaDescription = Utility.Common.GetMetaDescriptionByCustomerType(isUSCustomer, Item.MetaDescription, Item.OutsideUSMetaDescription, BBCodeHelper.ReplaceBBCode(RewriteUrl.StripTags(Item.LongDesc)))

        If objMetaTag.PageTitle = Item.ItemName And isUSCustomer Then
            objMetaTag.PageTitle &= strPriceDesc ' & " - " & SysParam.GetValue("PageTitle")
            'Else
            '    objMetaTag.PageTitle &= " - " & SysParam.GetValue("PageTitle")
        End If

        Dim shareURL As String = GlobalSecureName & URLParameters.ProductUrl(objItem.URLCode, objItem.ItemId)
        Dim shareTitle As String = objItem.ItemName & strPriceDesc
        Dim shareDesc As String = Utility.Common.RemoveHTMLTag(strLongDescription)
        Dim strPrice As Double = 0
        If objItem.LowSalePrice > 0 Then
            strPrice = objItem.LowSalePrice
        ElseIf objItem.LowPrice > 0 Then
            strPrice = objItem.LowPrice
        Else
            strPrice = objItem.Price
        End If
        'link MSDS
        ucPriceCart.linkMSDS = objItem.MSDS
        ''''''''''''
        objMetaTag.TypeShare = "item"
        objMetaTag.ShareTitle = shareTitle
        objMetaTag.ShareDesc = shareDesc
        objMetaTag.Price = strPrice
        objMetaTag.ShareURL = shareURL
        objMetaTag.ImageName = objItem.Image
        objMetaTag.ImagePath = MediaUrl & "/items/"
        objMetaTag.ImgHeight = 500
        objMetaTag.ImgWidth = 500
        objMetaTag.Canonical = shareURL
        SetPageMetaSocialNetwork(Page, objMetaTag)
        '' SetPageMetaSocialNetwork("item", shareTitle, shareDesc, strPrice.ToString(), shareURL, objItem.Image, MediaUrl & "/items/", 500, 500, "", "")
        ucShare.shareURL = shareURL
        ucShare.shareDescription = shareDesc
    End Sub


    Private Sub BindData()
        isMusicItem = CheckMusicItem(Item.ItemId)
        ViewedItemRow.Add(DB, Session.SessionID, Item.ItemId, MemberId)
        ucProductReview.ItemId = Item.ItemId
        LoadCommonInfor(Item)
        LoadInstruction(Item)
        LoadImage(Item)
        ltrImageSrc.Text = String.Format("<link rel=""image_src"" href=""{0}"" />", WebRoot & MediaUrl & "/items/" & Item.Image)

        '' SetMetaTag()
        SetSocialNetwork(Item)
        If Item.IsFreeSample = False Then
            ucPriceCart.Item = Item
        Else
            ucPriceCart.Visible = False
        End If

    End Sub


    Public Sub LoadImage(ByVal objItem As StoreItemRow)
        Dim dtVideo As DataTable = StoreItemVideoRow.ListByItemId(DB, objItem.ItemId)
        divVideo.Visible = False
        If Not dtVideo Is Nothing AndAlso dtVideo.Rows.Count > 0 Then
            divVideo.Visible = True
        End If
        ucItemImage.Item = objItem
        ucItemImage.ListVideo = dtVideo

        ucItemVideo.Item = objItem
        ucItemVideo.ListVideo = dtVideo
        If isMusicItem Then
            lblHowToVideoTitle.InnerText = "Albums"
        End If

    End Sub

    Public Sub LoadInstruction(ByVal objItem As StoreItemRow)
        ltrInstructionHeader.Text = String.Empty
        ltrInstruction.Text = BBCodeHelper.ConvertBBCodeToHTML(objItem.Specifications)
        If (String.IsNullOrEmpty(ltrInstruction.Text)) Then
            divInstruction.Visible = False
        Else
            ltrInstructionHeader.Text = "<a href='javascript:void(0)' onclick=""return ScrollToSection('#item-detail .data .instruction');"">Instructions</a>"
        End If
    End Sub

    Public Sub LoadCommonInfor(ByVal objItem As StoreItemRow)
        strPriceDesc = IIf(objItem.PriceDesc <> Nothing, " - " & objItem.PriceDesc, "")
        Dim measure As String = ShowMeasurement(objItem.PriceDesc, objItem.Measurement)
        strPriceDesc &= IIf(measure.Length > 0, " (" & measure & ")", "")
        litItemName.Text = objItem.ItemName & strPriceDesc
        If System.IO.File.Exists((Server.MapPath(MediaUrl & "/items/related/") & objItem.Image)) Then
            divThumbImage.Text = "<div class='thumb-image'><img src='" & Utility.ConfigData.CDNMediaPath & MediaUrl & "/items/related/" & objItem.Image & "' alt='' /></div>"
        Else
            divThumbImage.Text = String.Empty
        End If
        litSKU.Text = "Item #" & objItem.SKU
        Dim strLongDes As String = objItem.LongDesc
        strLongDescription = BBCodeHelper.ConvertBBCodeToHTML(strLongDes)
        ''
        ucDescription.SKU = objItem.SKU
        ucDescription.Description = strLongDescription
        ucDescription.LoadDescription()
    End Sub
#End Region





    Private Sub ShowError(ByVal msg As String)
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "showError", "ShowError('" & msg & "');", True)
    End Sub


#Region "Load more review"
    <WebMethod()> _
    Public Shared Function GetMoreData(ByVal pageIndex As Integer, ByVal itemId As Integer, ByVal sortField As String, ByVal isSort As Integer) As String()
        Dim reviewControl As New controls_product_review_list
        Dim result As String() = reviewControl.GetMoreData(pageIndex, 0, itemId, sortField, isSort)
        Return result
    End Function
#End Region

End Class
