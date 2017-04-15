Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Collections.Generic
Imports Utility
Imports Utility.Common

Public Class shop_design_detail
    Inherits SitePage
    Public AddthisAssociatedUrl As String = Utility.ConfigData.GlobalRefererName
    Public AddthisAssociatedName As String

    Public ShopDesignId As Integer = Integer.MinValue
    Public ZoomImagePath As String = String.Empty
    Public ImagePath As String = String.Empty

    Private CartItem As StoreCartItemRow
    Protected Item As StoreItemRow
    Protected ItemGroupId As Integer
    Protected Department As StoreDepartmentRow
    Private Shared Property isInternational As Boolean
    Public currentMemberId As Integer = 0

    Public Function GetFaceBookUrl() As String
        Return WebRoot & Me.Request.RawUrl
    End Function

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            ShopDesignId = GetQueryString("Id")
        Catch ex As Exception
        End Try
        If Not IsPostBack Then
            LoadShopDesignDetail()
            LoadListItem()
            LoadReview()
        Else
            ResetAfterSubmit()
        End If
        ucReview.ItemReviewId = ShopDesignId
    End Sub


#Region "Load Data"
    Private Sub LoadShopDesignDetail()
        Dim row As ShopDesignRow = ShopDesignRow.GetRow(DB, ShopDesignId)
        If row IsNot Nothing Then
            litShopDesignName.Text = row.Title
            ltrDescription.Text = BBCodeHelper.ConvertBBCodeToHTML(row.Description)
            If Not String.IsNullOrEmpty(row.Instruction) Then
                ltrInstruction.Text = BBCodeHelper.ConvertBBCodeToHTML(row.Instruction)
                ltrInstructionHeader.Text = "<a href='javascript:void(0)' onclick=""return ScrollToSection('#shop-design .instruction');"">Instructions</a>"
            End If
            divInstruction.Visible = Not String.IsNullOrEmpty(row.Instruction)

            LoadImage(row)

            SetMember()
            '''''''set MetaTag danh cho mang xa hoi khi share
            SetSocialNetwork(row)
        End If

    End Sub
    Private Sub SetSocialNetwork(ByVal objItem As ShopDesignRow)
        Dim objMetaTag As New MetaTag
        objMetaTag.MetaKeywords = objItem.MetaKeyword
        objMetaTag.PageTitle = objItem.PageTitle
        objMetaTag.MetaDescription = objItem.MetaDescription

        Dim shareURL As String = GlobalSecureName & URLParameters.ShopDesignUrl(objItem.Title, objItem.ShopDesignId)
        Dim shareTitle As String = objItem.Title
        Dim shareDesc As String = Utility.Common.RemoveHTMLTag(objItem.ShortDescription)
        objMetaTag.TypeShare = "news"
        objMetaTag.ImageName = objItem.Image
        objMetaTag.ImagePath = Utility.ConfigData.ShopDesignThumbPath
        objMetaTag.ImgHeight = 202
        objMetaTag.ImgWidth = 360
        objMetaTag.ShareDesc = shareDesc
        objMetaTag.ShareTitle = shareTitle
        objMetaTag.ShareURL = shareURL

        SetPageMetaSocialNetwork(Page, objMetaTag)
        ucShare.shareURL = shareURL
        'ushare.shareDescription = shareDesc
    End Sub
    Private Shared Sub SetMember()
        Dim CookieOrderId As Integer = 0
        Try
            CookieOrderId = Utility.Common.GetOrderIdFromCartCookie()
            Dim dbMember As MemberRow = MemberRow.GetRow(Utility.Common.GetCurrentMemberId())
            isInternational = dbMember.IsInternational
        Catch ex As Exception

        End Try
    End Sub

    Private Sub LoadListItem()
        Dim list As ShopDesignItemRowCollection = ShopDesignItemRow.ListItemByShopDesignId(ShopDesignId)
        If list.Count > 0 Then
            rptListItem.DataSource = list
            rptListItem.DataBind()
        End If
    End Sub

    Private Sub LoadReview()
        Dim totalReview As Integer = ReviewRow.GetTotalReviewByItemReviewId(ReviewType.ShopDesign, ShopDesignId)
        ltrHeaderReviewCount.Text = totalReview.ToString() & " Review"
        If totalReview > 1 Then
            ltrHeaderReviewCount.Text &= "s"
        End If
        ltrHeaderReview.Text = "<a  href='#' onclick=""return ScrollToSection('#shop-design .review-section');"">"
        ltrHeaderReview.Text &= "</a>"
    End Sub

    Public Sub LoadImage(ByVal obj As ShopDesignRow)

        Dim isAllowZoomImage As Boolean = False
        Dim imgFile As String = obj.Image
        If Not Core.FileExists(Server.MapPath(ConfigData.ShopDesignSmallPath) & imgFile) Then
            imgFile = Utility.ConfigData.CDNMediaPath & Utility.ConfigData.NoImageItem
            isAllowZoomImage = False
            ImagePath = Utility.ConfigData.CDNMediaPath & ConfigData.ShopDesignThumbPath
        Else
            ImagePath = Utility.ConfigData.CDNMediaPath & ConfigData.ShopDesignLargePath
        End If

        'Show image
        Dim imageHTML As String = "<div id='imgSource' style='display:{0}' allowzoom='{1}' ><img src='" & ImagePath & imgFile & "' alt='" & obj.Title & "' /></div>"
        If (isAllowZoomImage) Then
            imageHTML = String.Format(imageHTML, "none", "1")
            imageHTML = imageHTML & "<div id='wrap-zoom-image' style='top: 0px;  position: relative;'>"
        Else
            imageHTML = String.Format(imageHTML, "block", "0")
            imageHTML = imageHTML & "<div id='wrap-zoom-image' style='top: 0px;  position: relative;display:none;'>"
        End If
        imageHTML = imageHTML & "<a href='" & ZoomImagePath & imgFile & "' id='azoom' class='cloud-zoom' rel=""position:'right', zoomWidth:510, zoomHeight:510, adjustX:10, adjustY:-1"">"
        imageHTML = imageHTML & "<img style='display: block; border:none;' id='largeImage' src='" & ImagePath & imgFile & "'> </a></div>"
        divImage.InnerHtml = imageHTML

        'Load Alternate Image
        Dim TotalImage As Integer = 0
        Dim lstImage As ShopDesignMediaCollection = ShopDesignMediaRow.ListByShopDesignId(ShopDesignId, Utility.Common.ShopDesignMediaType.Image)

        'Load Video
        Dim lstVideo As ShopDesignMediaCollection = ShopDesignMediaRow.ListByShopDesignId(ShopDesignId, Utility.Common.ShopDesignMediaType.Video)

        Dim bReplaceImage As Boolean = False 'Neu co hinh ShopDesignMedia thi replace hinh default
        Dim liHTML As String = String.Empty
        If Not lstImage Is Nothing AndAlso lstImage.Count() > 0 Then
            For Each objImage As ShopDesignMediaRow In lstImage
                If Core.FileExists(Server.MapPath(ConfigData.ShopDesignSmallPath) & objImage.Url) Then
                    If Not bReplaceImage Then
                        divImage.InnerHtml = divImage.InnerHtml.Replace("src='" & ImagePath & imgFile & "'", "src='" & ConfigData.ShopDesignLargePath & objImage.Url & "'")
                        bReplaceImage = True
                    End If
                    liHTML &= "<li><a href='javascript:void(0)' onClick=""updateAlternateimg('" & Utility.ConfigData.CDNMediaPath & ConfigData.ShopDesignLargePath & "','" & Utility.ConfigData.CDNMediaPath & ConfigData.ShopDesignPath & "', '" & objImage.Url & "',0)""><img src=" & Utility.ConfigData.CDNMediaPath & ConfigData.ShopDesignSmallPath & objImage.Url & " alt='" & objImage.Tag & "' /></a></li>"
                End If
            Next
        End If


        If lstVideo.Count > 0 Then
            Dim videoImage As String = GetVideoImage(lstVideo.Item(0).Id & ".jpg", Utility.ConfigData.ShopDesignSmallPath)
            Dim liVideo As String = String.Empty
            liVideo &= "<li class='video'><a onclick=""{0}"" href='javascript:void(0)'>"
            liVideo &= "     <div class='img'><img src='" & videoImage & "'>"
            liVideo &= "       <div class='play'></div>"
            liVideo &= "    </div></a></li>"
            If lstVideo.Count > 1 Then
                liVideo = String.Format(liVideo, "return ScrollToSection('#shop-design #divVideo .label');")
                'divVideo.Visible = True
                rptVideo.DataSource = lstVideo
                rptVideo.DataBind()
            Else
                liVideo = String.Format(liVideo, "return PlayVideo(" & lstVideo.Item(0).Id & ");")
                divVideo.Visible = False
            End If
            liHTML = liHTML & liVideo
        Else
            divVideo.Visible = False
        End If

        If (Not String.IsNullOrEmpty(liHTML)) Then
            ltrImageList.Text = "<ul class='image-list'>" & liHTML & "</ul>"
        Else
            ltrImageList.Text = String.Empty
        End If

    End Sub
#End Region

#Region "Video List"
    Protected Sub rptVideo_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptVideo.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim ltrVideo As System.Web.UI.WebControls.Literal = DirectCast(e.Item.FindControl("ltrVideo"), System.Web.UI.WebControls.Literal)
            If Not ltrVideo Is Nothing Then
                Dim row As ShopDesignMediaRow = e.Item.DataItem
                ltrVideo.Text = "<li class='video-item {0}'>"
                If ((e.Item.ItemIndex + 1) Mod 2 <> 0) Then
                    ltrVideo.Text = String.Format(ltrVideo.Text, "smallfirst")
                Else
                    ltrVideo.Text = String.Format(ltrVideo.Text, "smalllast")
                End If
                ltrVideo.Text &= "<a onclick=""return PlayVideo(" & row.Id & ");"" href='javascript:void(0)'><div class='img'>"
                ltrVideo.Text &= " <img src='" & GetVideoImage(row.Id & ".jpg", Utility.ConfigData.ShopDesignVideoThumbPath) & "' />"
                ltrVideo.Text &= "<div class='play'></div></div></a>"
                ltrVideo.Text &= "<div class='name'><a onclick=""return PlayVideo(" & row.Id & ");"" href='javascript:void(0)'>" & row.Tag & "</a></div></li>"
            End If
        End If
    End Sub

    Public Function GetVideoImage(ByVal thumbimage As String, ByVal pathImage As String) As String
        Dim path As String = Server.MapPath("~" & pathImage) & thumbimage
        If System.IO.File.Exists(path) Then
            Return Utility.ConfigData.CDNMediaPath & pathImage & thumbimage
        Else
            Return Utility.ConfigData.CDNMediaPath & Utility.ConfigData.ShopDesignVideoThumbNoImage
        End If
    End Function
#End Region


#Region "Add Cart"
    Protected Sub rptListItem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptListItem.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim sdi As ShopDesignItemRow = e.Item.DataItem
            Dim OrderId As Integer = 0
            If Common.GetOrderIdFromCartCookie() > 0 Then
                OrderId = CInt(Common.GetOrderIdFromCartCookie())
            End If

            Dim MemberId As Integer = 0
            If Common.GetMemberIdFromCartCookie() > 0 Then
                MemberId = CInt(Common.GetMemberIdFromCartCookie())
            End If

            Dim si As StoreItemRow = StoreItemRow.GetRowInList(DB, sdi.ItemId, OrderId, MemberId)
            Dim litItemName As Literal = CType(e.Item.FindControl("litItemName"), Literal)
            litItemName.Text = String.Format("<a href=""{0}"">{1} - {2}</a>", URLParameters.ProductUrl(si.URLCode, si.ItemId), si.ItemName, si.PriceDesc)

            Dim litItemImage As Literal = CType(e.Item.FindControl("litItemImage"), Literal)
            litItemImage.Text = String.Format("<img src=""" & Utility.ConfigData.CDNMediaPath & "/assets/items/featured/{0}"" alt=""{1}"" />", si.Image, si.ImageAltTag)

            'Price
            Dim lblPrice As Literal = CType(e.Item.FindControl("lblPrice"), Literal)
            Dim ltsave As Literal = CType(e.Item.FindControl("ltsave"), Literal)
            Dim litPromotionText As Literal = CType(e.Item.FindControl("litPromotionText"), Literal)
            Dim objItemPrice As ItemPrice = BaseShoppingCart.GetItemPrice(DB, si, 1, 0, 0, False)
            lblPrice.Text = StoreItemRow.ReturnPrice(objItemPrice)
            If (objItemPrice.SalePrice > 0) Then
                ltsave.Text = "<li class='save'>You Save <span>-" & Utility.Common.ViewCurrency(objItemPrice.YouSave) & " (" & objItemPrice.PercentSave & "%)</span></li>"
            Else
                ltsave.Visible = False
            End If

            If si.Promotion IsNot Nothing Then
                litPromotionText.Text = si.Promotion.Description
            End If

            Dim divAddCart As HtmlGenericControl = CType(e.Item.FindControl("divAddCart"), HtmlGenericControl)
            divAddCart.Visible = False


            If si.IsFlammable = True And isInternational Then
                Dim divFlammable As HtmlGenericControl = CType(e.Item.FindControl("divFlammable"), HtmlGenericControl)
                divFlammable.Visible = True
            ElseIf si.PermissionBuyBrand = False AndAlso Utility.Common.GetCurrentMemberId > 0 Then
                Dim divFlammable As HtmlGenericControl = CType(e.Item.FindControl("divFlammable"), HtmlGenericControl)
                divFlammable.Visible = True
                divFlammable.InnerHtml = "<span class=""red"">Available only in store or by phone order</span>"
            ElseIf si.QtyOnHand <= 0 And Not Utility.Common.IsItemAcceptingOrder(si.AcceptingOrder) And Not si.IsSpecialOrder Then
                Dim divCart As Literal = CType(e.Item.FindControl("divCart"), Literal)
                divCart.Text = String.Format("<div id=""divCart"" class=""red addcart"">Currently Out of Stock <br><a href=""#"" onclick=""NotifyInStock('{0}');"">Notify me when in stock</a></div>", si.ItemId)
            Else
                divAddCart.Visible = True
                Dim litQtyInput As Literal = CType(e.Item.FindControl("litQtyInput"), Literal)
                litQtyInput.Text = String.Format("<input type=""tel"" class=""txt-qty"" name=""txtQtyItem{0}"" id=""txtQtyItem{0}"" value=""{1}"" onkeypress=""return numbersonly();"" maxlength=""4"">", si.ItemId, sdi.QtyDefault)

                Dim litQtyArrow As Literal = CType(e.Item.FindControl("litQtyArrow"), Literal)
                litQtyArrow.Text = String.Format("<a href=""javascript:void(Increase('txtQtyItem{0}'))""><li><b class=""arrow-up arrow-down-act"" id=""imgIntxtQty{0}""></b></li></a><a href=""javascript:void(Decrease('txtQtyItem{0}',1))""><li class=""bd-qty""><b class=""arrow-down arrow-down-act"" id=""imgDetxtQty{0}""></b></li></a>", si.ItemId)

                Dim litQtyPlusMin As Literal = CType(e.Item.FindControl("litQtyPlusMin"), Literal)
                litQtyPlusMin.Text = String.Format("<div class=""plus""><a href=""javascript:void(Increase('txtQtyItem{0}'))"">+</a></div><div class=""min""><a href=""javascript:void(Decrease('txtQtyItem{0}',1))"">&ndash;</a></div>", si.ItemId)

                Dim litAction As Literal = CType(e.Item.FindControl("litAction"), Literal)
                litAction.Text = String.Format("<div style=""clear:both""><input value=""Add to Cart"" type=""button"" class=""bt-add-cart bg-add-cart"" onclick=""mainScreen.ExecuteCommand('AddCart', 'methodHandlers.ShowCart', ['{0}', GetQty('{0}'),false]);"" /></div>", si.ItemId)
                litAction.Text &= String.Format("<div name=""lblInCart{0}"" id=""lblInCart{0}"" class=""incart"">[text]</div>", si.ItemId)
                litAction.Text = IIf(si.IsInCart, litAction.Text.Replace("[text]", "Added to your cart"), litAction.Text.Replace("[text]", ""))
            End If
        End If
    End Sub

    Private Sub ShowError(ByVal msg As String)
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "showError", "ShowError('" & msg & "');", True)
    End Sub

    Public Sub ResetAfterSubmit()
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "resetPageSubmit", "ResetPageSubmit();", True)
    End Sub
#End Region

End Class
