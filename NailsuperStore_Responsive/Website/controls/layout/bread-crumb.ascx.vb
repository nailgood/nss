
Option Strict Off

Imports Components
Imports DataLayer
Imports System.Data

Partial Class Controls_Breadcrumb
    Inherits ModuleControl

    Protected dbPage As ContentToolPageRow
    Protected dbSub As ContentToolNavigationRow
    Protected dbSubSub As ContentToolNavigationRow
    Protected dbNav As ContentToolNavigationRow
    Private m_Args As String = String.Empty
    Public Property DepartmentId() As Integer
        Get
            Return ViewState("DepartmentId")
        End Get
        Set(ByVal value As Integer)
            ViewState("DepartmentId") = value
        End Set
    End Property
    Public breadcrumbContent As String = "<ul class='content'><li class='home'><a href='/'>Home</a></li>"
    Public Overrides Property Args() As String
        Get
            Return m_Args
        End Get
        Set(ByVal Value As String)
            m_Args = Value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Utility.Common.IsViewFromAdmin() Then
            Me.Visible = False
            Exit Sub
        End If
        If Request.Path.Contains("/store/main-category.aspx") Or (Request.Url.ToString().Contains("?act=checkout") AndAlso Not Request.Url.ToString().Contains("login.aspx")) Then
            breadcrumbContent = String.Empty
        ElseIf Request.Path.Contains("store/default.aspx") Or Request.Path.Contains("store/sub-category.aspx") Or Request.Path.Contains("store/item.aspx") Or Request.Path.Contains("store/department-tab.aspx") Or Request.Path.Contains("store/collection.aspx") Then
            NavigationDepartment()
        ElseIf Request.Path.Contains("store/brand.aspx") Then
            NavigationBrand()
        ElseIf Request.RawUrl.Contains("news") Or Request.RawUrl.Contains("blog") Or Request.RawUrl.Contains("video") Or Request.RawUrl.Contains("shop-by-design") Or Request.RawUrl.Contains("media") Or Request.RawUrl.Contains("/tips") Or Request.RawUrl.Contains("gallery") Or Request.Path.Contains("/members/submission.aspx") Or Request.Path.Contains("/store/review/order-list.aspx") Or Request.Path.Contains("/store/review/product-list.aspx") Then
            NavigationCategory()
        ElseIf Request.RawUrl.Contains("/addressbook/") Then
            breadcrumbContent &= "<li class='bread-resource'><a href='/members/default.aspx'>My Account</a></li><li class='active'>Address Book</li>"
        ElseIf Request.Path.Contains("store/search-result.aspx") Then
            NavigationDepartmentSearch()
        ElseIf Request.Path.Contains("resource-center") Then
            breadcrumbContent &= "<li class='bread-resource active'>Resource Center</li>"
        ElseIf Request.Path.Contains("/store/shop-save.aspx") Then
            NavigationShopSave()
        ElseIf Request.Path.Contains("/store/recently-viewed.aspx") Then
            breadcrumbContent = "<ul class='content'><li class='home home-deal-center'><a href='/'>Home</a></li><li class='active'>Recently Viewed Items</li></ul>"
        ElseIf Request.Path.Contains("/store/free-gift.aspx") Then
            breadcrumbContent = "<ul class='content'><li class='home home-deal-center'><a href='/'>Home</a></li><li class='active'>Free Gift</li></ul>"
        ElseIf Request.Path.Contains("/store/free-sample.aspx") Then
            breadcrumbContent = "<ul class='content'><li class='home home-deal-center'><a href='/'>Home</a></li><li class='active'>Free Samples</li></ul>"
        ElseIf Request.RawUrl.Contains("/order-tracking/") Then
            breadcrumbContent &= "<li class='bread-resource'><a href='/members/default.aspx'>My Account</a></li><li class='active'>FedEx Tracking</li>"
        Else
            NavigationPageInfo()
        End If

        If Not String.IsNullOrEmpty(breadcrumbContent) Or Not breadcrumbContent.Contains("</ul>") Then
            breadcrumbContent &= "</ul>"
        End If
    End Sub

    Private Sub NavigationShopSave()

        Dim ShopSaveId As String = Request.QueryString("ShopSaveId")
        If (ShopSaveId > 0) Then
            Dim objShopSave As ShopSaveRow = ShopSaveRow.GetRow(DB, ShopSaveId)
            If (Not objShopSave Is Nothing AndAlso Not String.IsNullOrEmpty(objShopSave.Name)) Then
                breadcrumbContent = "<ul class='content'><li class='home'><a href='/'>Home</a></li><li class='active'>{0}</li></ul>"
                Dim name As String = String.Empty
                If (objShopSave.Type = 1) Then
                    name = "Deals Center"
                    breadcrumbContent = "<ul class='content'><li class='home home-deal-center'><a href='/'>Home</a></li><li><a href='/deals-center'>{0}</a></li></ul>"
                ElseIf (objShopSave.Type = 4) Then
                    name = SysParam.GetValue("ShopExploreOurStore")
                ElseIf (objShopSave.Type = 5) Then
                    name = SysParam.GetValue("TopRecommendedProducts")
                ElseIf (objShopSave.Type = 6) Then
                    name = "Promotion"
                End If
                If Not String.IsNullOrEmpty(name) Then
                    breadcrumbContent = String.Format(breadcrumbContent, name)
                End If
            End If
        
        End If
    End Sub

    Private Sub NavigationBrand()

        Dim BrandId As Integer = Request.QueryString("brandid")
        If (BrandId > 0) Then
            Dim obj As StoreBrandRow = StoreBrandRow.GetRow(BrandId)
            If (Not obj Is Nothing AndAlso Not String.IsNullOrEmpty(obj.BrandName)) Then
                breadcrumbContent = String.Format("<ul class='content'><li class='home'><a href='/'>Home</a></li><li class='active'>{0}</li></ul>", obj.BrandName)
            End If

        End If
    End Sub

  
    Private Sub NavigationCategory()
        Dim nav As String = String.Empty
        If Request.Path.Contains("shop-design") Then
            breadcrumbContent = "<ul class='content shop-by-design'><li><a href='/'>Home</a></li>"
            If Request.QueryString("cateId") <> Nothing Or Request.QueryString("Id") <> Nothing Then
                If Request.QueryString("cateId") <> Nothing Then
                    nav = CategoryRow.FullByCategoryId(CInt(Request.QueryString("cateId")))
                ElseIf Request.QueryString("Id") <> Nothing Then
                    nav = CategoryRow.FullByShopDesignId(CInt(Request.QueryString("Id")))
                End If

                breadcrumbContent &= "<li><a href='/shop-by-design'>Shop by Design</a></li>"
                If Not String.IsNullOrEmpty(nav) Then
                    For Each s In nav.Split("|")
                        If String.IsNullOrEmpty(s) Then
                            Continue For
                        End If
                        Dim name As String = s.Substring(0, s.IndexOf("["))
                        Dim id As Integer = CInt(s.Remove(s.IndexOf("]")).Substring(s.IndexOf("[") + 1))
                        If id = Request.QueryString("cateId") Then
                            breadcrumbContent &= "<li class='active'>" & name & "</li>"
                        Else
                            breadcrumbContent &= "<li><a href='" & URLParameters.ShopDesignListUrl(name, id) & "'>" & name & "</a></li>"
                        End If
                    Next
                End If
            Else
                breadcrumbContent &= "<li class='active'>Shop by Design</li>"
            End If
        Else
            breadcrumbContent = "<ul class='content'><li class='home'><a href='/'>Home</a></li><li class='bread-resource'><a href='/resource-center-summary'>Resource Center </a></li>"
            ''choose category va year
            If Not Request.QueryString("cateId") Is Nothing AndAlso Not Request.QueryString("y") Is Nothing Then
                nav = CategoryRow.FullByCategoryId(CInt(Request.QueryString("cateId")))
                Dim arr As String = Request.QueryString("y")
                If Not String.IsNullOrEmpty(nav) Then
                    For Each s In nav.Split("|")
                        If String.IsNullOrEmpty(s) Then
                            Continue For
                        End If
                        Dim name As String = s.Substring(0, s.IndexOf("["))
                        Dim id As Integer = CInt(s.Remove(s.IndexOf("]")).Substring(s.IndexOf("[") + 1))
                        breadcrumbContent &= "<li><a href='/news-topic'>News & Events</a></li><li><a href='/news-topic/" & HttpUtility.UrlEncode(RewriteUrl.ReplaceUrl(name.ToLower())) & "/" & id & "'>" & name & "</a></li>"
                        breadcrumbContent &= "<li class='active'>" & MonthName(CInt(Right(arr, arr.Length - 4))) & " " & CInt(Left(arr, 4)) & "</li>"
                    Next
                End If
                ''choose category or detail
            ElseIf Not Request.QueryString("cateId") Is Nothing Or Not Request.QueryString("newId") Is Nothing Or Not Request.QueryString("VideoId") Is Nothing Or Not Request.QueryString("MediaId") Is Nothing Or Not Request.QueryString("TipId") Is Nothing Then
                Dim cateID As Integer = IIf(Not Session("VideoCateId") Is Nothing, CInt(Session("VideoCateId")), IIf(Request.RawUrl.ToLower.Contains("media-detail"), CInt(Session("MediaCateId")), CInt(Session("NewsCateId"))))
                nav = IIf(cateID > 0, CategoryRow.FullByCategoryId(cateID), IIf(Not Request.QueryString("cateId") Is Nothing, CategoryRow.FullByCategoryId(CInt(Request.QueryString("cateId"))), TipCategoryRow.FullByCategoryId(Session("TipCateId"))))
                If Not String.IsNullOrEmpty(nav) Then
                    For Each s In nav.Split("|")
                        If String.IsNullOrEmpty(s) Then
                            Continue For
                        End If
                        Dim name As String = s.Substring(0, s.IndexOf("["))
                        Dim id As Integer = CInt(s.Remove(s.IndexOf("]")).Substring(s.IndexOf("[") + 1))
                        If Request.Path.Contains("video") Then
                            breadcrumbContent &= "<li><a href='/video-topic'>Video</a></li><li class='active'>" & name & "</li>"
                        ElseIf Request.Path.Contains("news") Then
                            breadcrumbContent &= IIf(name.ToLower = "blog", "<li class='active'>" & name & "</li>", "<li><a href='/news-topic'>News & Events</a></li><li class='active'>" & name & "</li>")
                        ElseIf Request.Path.Contains("media") Then
                            breadcrumbContent &= "<li><a href='/media-topic'>Media Press</a></li><li class='active'>" & name & "</li>"
                        Else
                            breadcrumbContent &= "<li><a href='/tips'>Expert Tips & Advice</a></li><li class='active'>" & name & "</li>"
                        End If
                    Next
                End If
                ''choose year
            ElseIf Not Request.QueryString("y") Is Nothing Then
                Dim arr As String = Request.QueryString("y")
                breadcrumbContent &= IIf(Request.RawUrl.Contains("blog"), "<li><a href='/blog'>Blog</a></li><li class='active'>" & MonthName(CInt(Right(arr, arr.Length - 4))) & " " & CInt(Left(arr, 4)) & "</li>", "<li><a href='/news-topic'>News & Events</a></li><li class='active'>" & MonthName(CInt(Right(arr, arr.Length - 4))) & " " & CInt(Left(arr, 4)) & "</li>")
            Else
                If Request.Path.Contains("video") Then
                    breadcrumbContent &= "<li class='active'>Video</li>"
                ElseIf Request.Path.Contains("news") Then
                    breadcrumbContent &= IIf(Request.RawUrl.Contains("blog"), "<li class='active'>Blog</li>", "<li class='active'>News & Events</li>")
                ElseIf Request.Path.Contains("media") Then
                    breadcrumbContent &= "<li class='active'>Media Press</li>"
                ElseIf Request.Path.Contains("tips") Then
                    breadcrumbContent &= "<li class='active'>Expert Tips & Advice</li>"
                ElseIf Request.Path.Contains("order-list") Then
                    breadcrumbContent &= "<li class='active'>Customer Feedback</li>"
                ElseIf Request.Path.Contains("product-list") Then
                    breadcrumbContent &= "<li class='active'>Customer Product Reviews</li>"
                Else
                    breadcrumbContent &= "<li class='active'>Customer Nail Art</li>"
                End If
            End If
        End If
        breadcrumbContent &= "</ul>"
    End Sub

    Private Sub NavigationPageInfo()
        If Request.RawUrl = "" Then
            Exit Sub
        End If

        Dim url As String = IIf(Request.RawUrl.Contains(Request.Path), Request.Path, Request.RawUrl)
        Dim nav As String = ContentToolPageRow.FullNavigationByPageUrl(url)

        If Not String.IsNullOrEmpty(nav) Then
            breadcrumbContent = "<ul class='content'>"
            For Each s In nav.Split("|")
                Try
                    If Not s.Contains("[") Then
                        breadcrumbContent += IIf(s.Contains("My Account"), "<li class='bread-resource active'>" & s & "</a></li>", "<li class='active'>" & s & "</a></li>")
                        Continue For
                    End If

                    Dim name As String = s.Substring(0, s.IndexOf("["))
                    Dim target As String = s.Substring(s.IndexOf("]") + 1)
                    Dim link As String = s.Remove(s.IndexOf("]")).Substring(s.IndexOf("[") + 1)

                    If String.IsNullOrEmpty(link) Then
                        breadcrumbContent += "<li class='active'>" & name & "</a></li>"
                    Else
                        If name = "Customer Service" Then
                            breadcrumbContent += "<li class='hidden-xs hidden-sm'><a href='" & link & "' target='" + target + "'>" & name & "</a></li>"
                        ElseIf name = "Home" Then
                            breadcrumbContent += "<li class='home'><a href='" & link & "' target='" + target + "'>" & name & "</a></li>"
                        Else
                            breadcrumbContent += IIf(name.Contains("Resource Center") Or name.Contains("My Account"), "<li class='bread-resource'><a href='" & link & "' target='" + target + "'>" & name & "</a></li>", "<li><a href='" & link & "' target='" + target + "'>" & name & "</a></li>")
                        End If

                    End If
                Catch ex As Exception

                End Try
            Next

            breadcrumbContent = breadcrumbContent & "</ul>"
        Else
            Me.Visible = False
        End If
    End Sub

    Private Sub NavigationDepartment()
        Dim nav As String = String.Empty
        If Request.Path.Contains("store/item.aspx") Then
            Dim ItemId As Integer = CInt(Request.QueryString("ItemId"))
            Dim DepartmentId As Integer = CInt(Session("DepartmentId"))
            nav = StoreDepartmentRow.FullByItemId(ItemId, DepartmentId)
        ElseIf Request.Path.Contains("store/department-tab.aspx") Then
            nav = DepartmentTabRow.FullByTabURLCode(Request.QueryString("TabURLCode"), CInt(Request.QueryString("DepartmentId")))
        Else
            Dim DepartmentId As Integer = CInt(Request.QueryString("DepartmentId"))
            nav = StoreDepartmentRow.FullByDeparmentId(DepartmentId)
        End If

        If Not String.IsNullOrEmpty(nav) Then

            Dim i As Integer = 0
            For Each s In nav.Split("^")
                If String.IsNullOrEmpty(s) Then
                    Continue For
                End If

                Try
                    If Not s.Contains("[") Then
                        breadcrumbContent += "<li itemscope itemtype=""http://data-vocabulary.org/Breadcrumb"" class='active' itemprop=""url""><span itemprop=""title"">" & s & "</span><a href='" & Utility.ConfigData.GlobalRefererName & Request.RawUrl & "' itemprop=""url"" class='show-snippet' >" & s & "</a></li>"
                        Continue For
                    End If

                    Dim name As String = s.Substring(0, s.IndexOf("["))
                    Dim url As String = s.Remove(s.IndexOf("]")).Substring(s.IndexOf("[") + 1)
                    Dim id As Integer = CInt(s.Substring(s.IndexOf("]") + 1))

                    If String.IsNullOrEmpty(url) Then
                        breadcrumbContent += "<li itemscope itemtype=""http://data-vocabulary.org/Breadcrumb"" Class='active' itemprop=""url""><span itemprop=""title"">" & name & "</span><a href='" & Utility.ConfigData.GlobalRefererName & Request.RawUrl & "' itemprop=""url"" class='show-snippet' >" & name & "</a></li>"
                    Else
                        If i = 0 Then
                            breadcrumbContent += "<li itemscope itemtype=""http://data-vocabulary.org/Breadcrumb""><a href='" & Utility.ConfigData.GlobalRefererName & URLParameters.MainDepartmentUrl(url, id) & "' itemprop=""url""><span itemprop=""title"">" & name & "</span></a></li>"
                            i += 1
                        Else
                            breadcrumbContent += "<li itemscope itemtype=""http://data-vocabulary.org/Breadcrumb""><a href='" & Utility.ConfigData.GlobalRefererName & URLParameters.DepartmentUrl(url, id) & "' itemprop=""url""><span itemprop=""title"">" & name & "</span></a></li>"
                        End If

                    End If
                Catch ex As Exception

                End Try
            Next

            If Request.RawUrl.Contains("?") Then
                'breadcrumbContent += NavigationDepartmentNarrow()
            End If
        ElseIf GetQueryString("brandid") <> Nothing Then
            breadcrumbContent += "<li class='active'><span property=""name"">Brand</span></a></li>"
            'breadcrumbContent += NavigationDepartmentNarrow()
        End If

    End Sub

    Public Function NavigationDepartmentNarrow() As String

        Dim breadcrumb As String = String.Empty
        Dim brand As String = GetQueryString("brand")
        Dim BrandId As String = GetQueryString("brandid")
        Dim collectionId As String = GetQueryString("collectionid")
        Dim toneId As String = GetQueryString("toneid")
        Dim shadeId As String = GetQueryString("shadeid")
        Dim price As String = GetQueryString("price")
        Dim rate As String = GetQueryString("rating")
        Dim urlRaw As String = Me.Request.RawUrl

        Dim oSpan As String = "<span class=""bcative"">"
        Dim cSpan As String = "</span>"
        Dim LinkReplace As String = "<a href=""{0}"" class=""bdcrmblnk"">"
        Dim cLink As String = "</a>"

        Dim urlRoot As String = URLParameters.AddParamaterToURL(urlRaw, "brand", "", False)
        urlRoot = URLParameters.AddParamaterToURL(urlRoot, "price", "", False)
        urlRoot = URLParameters.AddParamaterToURL(urlRoot, "rating", "", False)

        'kiem tra link co nail-brand ko add param brand
        Dim LinkBrand As Boolean = False
        If (urlRaw.Contains("nail-brand")) Then
            LinkBrand = True
        End If

        Dim url As String
        If (Not String.IsNullOrEmpty(brand) And Not LinkBrand) Then
            url = URLParameters.AddParamaterToURL(urlRaw, "brand", "", False)
            breadcrumb = breadcrumb & "<li class=""narrow""><a href=""" & url & """ class=""remove-filter"">" & StoreBrandRow.GetByURLCode(brand).BrandName & "</a></li>"
        ElseIf (Not String.IsNullOrEmpty(BrandId) AndAlso IsNumeric(BrandId) And Not LinkBrand) Then
            url = URLParameters.AddParamaterToURL(urlRaw, "brandId", "", False)
            breadcrumb = breadcrumb & "<li class=""narrow""><a href=""" & url & """ class=""remove-filter"">" & StoreBrandRow.GetRow(BrandId).BrandName & "</a></li>"
        End If

        If (ChecNumeric(collectionId)) Then
            Dim row As StoreCollectionRowBase = StoreCollectionRow.GetRow(DB, CInt(collectionId))
            url = URLParameters.AddParamaterToURL(urlRaw, "collectionid", "", False)
            breadcrumb = breadcrumb & "<li class=""narrow""><a href=""" & url & """ class=""remove-filter"">" & row.CollectionName & "</a></li>"
        End If

        If (ChecNumeric(toneId)) Then
            Dim row As StoreToneRowBase = StoreToneRow.GetRow(DB, CInt(toneId))
            url = URLParameters.AddParamaterToURL(urlRaw, "toneid", "", False)
            breadcrumb = breadcrumb & "<li class=""narrow""><a href=""" & url & """ class=""remove-filter"">" & row.Tone & "</a></li>"
        End If

        If (ChecNumeric(shadeId)) Then
            Dim row As StoreShadeRowBase = StoreShadeRow.GetRow(DB, CInt(shadeId))
            url = URLParameters.AddParamaterToURL(urlRaw, "shadeid", "", False)
            breadcrumb = breadcrumb & "<li class=""narrow""><a href=""" & url & """ class=""remove-filter"">" & row.Shade & "</a></li>"
        End If

        If (Not String.IsNullOrEmpty(price)) Then
            url = URLParameters.AddParamaterToURL(urlRaw, "price", "", False)
            If Not LinkBrand Then
                urlRoot = URLParameters.AddParamaterToURL(urlRoot, "brand", brand, False)
            End If
            breadcrumb = breadcrumb.Replace(oSpan, String.Format(LinkReplace, urlRoot)).Replace(cSpan, cLink)
            Dim min As Double = Utility.Common.GetMinMaxValue(price, False)
            Dim max As Double = Utility.Common.GetMinMaxValue(price, True)
            Dim strPrice As String = ""
            If min <= 0 And max > 0 Then
                strPrice = "Under $" & max
            ElseIf min > 0 And max <= 0 Then
                strPrice = "$" & min & "+"
            Else
                strPrice = "$" & min & " - $" & max
            End If

            breadcrumb = breadcrumb & "<li class=""narrow""><a href=""" & url & """ class=""remove-filter"">" & strPrice & "</a></li>"
        End If

        If (Not String.IsNullOrEmpty(rate)) Then
            url = URLParameters.AddParamaterToURL(urlRaw, "rating", "", False)
            If (Not String.IsNullOrEmpty(price)) Then
                urlRoot = URLParameters.AddParamaterToURL(urlRoot, "price", price, False)
            Else
                If (Not String.IsNullOrEmpty(brand) And Not LinkBrand) Then
                    urlRoot = URLParameters.AddParamaterToURL(urlRoot, "brand", brand, False)
                End If
            End If

            breadcrumb = breadcrumb.Replace(oSpan, String.Format(LinkReplace, urlRoot)).Replace(cSpan, cLink)
            Dim min As Double = Utility.Common.GetMinMaxValue(rate, False)
            Dim max As Double = Utility.Common.GetMinMaxValue(rate, True)
            Dim strRating As String = ""

            If min <= 0 And max > 0 Then
                strRating = "Under " & max
            ElseIf min > 0 And max <= 0 Then
                strRating = min & "+"
            Else
                strRating = min & " - " & max
            End If

            breadcrumb = breadcrumb & "<li class=""narrow""><a href=""" & url & """ class=""remove-filter"">" & strRating & "</a></li>"
        End If
        Return breadcrumb
    End Function

    Private Sub NavigationDepartmentSearch()

        Dim filterPrice As String = String.Empty
        Dim filterRating As String = String.Empty
        Dim filterbrand As String = String.Empty
        If Not Request.QueryString("price") Is Nothing Then
            filterPrice = Request.QueryString("price")
        End If

        If Not Request.QueryString("rating") Is Nothing Then
            filterRating = Request.QueryString("rating")
        End If

        If Not Request.QueryString("brandid") Is Nothing Then
            filterbrand = Request.QueryString("brandid")
        End If

        If String.IsNullOrEmpty(filterbrand) AndAlso String.IsNullOrEmpty(filterPrice) AndAlso String.IsNullOrEmpty(filterRating) Then
            breadcrumbContent &= "<li class='active'>All Items</li>"
        Else

            Dim Keywords As String = String.Empty
            Keywords = IIf(GetQueryString("kw") Is Nothing, String.Empty, GetQueryString("kw"))
            If String.IsNullOrEmpty(Keywords) AndAlso Not GetQueryString("F_Keywords") Is Nothing Then
                Keywords = GetQueryString("F_Keywords")
            End If

            breadcrumbContent &= "<li class='active'>All Items</li>"
            If Request.QueryString.Count > 0 Then
                'breadcrumbContent += NavigationDepartmentNarrow()
            End If
        End If

        divBC.Attributes.Add("class", "bread-crumb bread-crumb-search")
    End Sub
   
End Class
