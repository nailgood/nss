Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports Utility.Common
Imports System.Globalization.CultureInfo
Partial Class controls_product_narrow_search
    Inherits ModuleControl
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal value As String)
        End Set
    End Property

    Private m_Filter As DepartmentFilterFields

    Public Property Filter() As DepartmentFilterFields
        Get
            Return m_Filter
        End Get
        Set(ByVal value As DepartmentFilterFields)
            m_Filter = value
        End Set
    End Property

    Private Keywords As String
    Private bBrand As Boolean = False
    Private bCategory As Boolean = False
    Private bPrice As Boolean = False
    Private bRating As Boolean = False
    Private MaxPerPage As Integer = 12
    Private Const MaxFeaturedPage As Integer = 12
    Private Const MaxPolishPage As Integer = 24
    Private lstCountCategory As String
    Private lstCountBrand As String
    Private lstCountPrice As String
    Private lstCountRating As String
    Private DisplayNarrowSearch As Boolean = False
    Private queryBrand As String = String.Empty
    Private queryPrice As String = String.Empty
    Private queryRating As String = String.Empty
    Private queryPros As String = String.Empty
    Private queryCons As String = String.Empty
    Private queryExp As String = String.Empty

    Private queryDepartmentId As String = String.Empty

    Private qrTone, qrShade, qrCollection As String
    Protected CountPrice As Integer = 0
    Protected CountRating As Integer = 0
    Private ckChecked As String = String.Empty
    Private hideDiv As Integer = 0
    Private firstRangePrice, firstRating As List(Of String)
    Private lstChkact As String = " <li class=""checkbox""><label for=""{0}""><input type=""checkbox"" id=""{0}"" onclick=""window.location='{1}'"" {2}><i class=""fa fa-check checkbox-font"" ></i><a href=""{1}"">{3}</a><span class=""count""> ({4})</span></label></li>"
    Private lstChkinact As String = " <li class=""checkbox""><label for=""{0}"" class=""disable"">" & vbCrLf &
                             "<i class=""fa fa-check checkbox-font bg-disable""></i>{1}<span class=""count""> ({2})</span></label></li>"
    Protected isReviewRating As Boolean = False

    Private countCategoryRender As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Filter Is Nothing Then
            Filter = New DepartmentFilterFields
        End If
        Keywords = IIf(GetQueryString("kw") Is Nothing, String.Empty, GetQueryString("kw"))
        'SortBy = IIf(GetQueryString("sort") Is Nothing, String.Empty, GetQueryString("sort"))
        queryBrand = IIf(GetQueryString("brandid") Is Nothing, String.Empty, GetQueryString("brandid")) 'IIf(GetQueryString("brand") Is Nothing, String.Empty, GetQueryString("brand"))
        queryPrice = IIf(GetQueryString("price") Is Nothing, String.Empty, GetQueryString("price"))
        queryRating = IIf(GetQueryString("rating") Is Nothing, String.Empty, GetQueryString("rating"))
        queryPros = IIf(GetQueryString("pros") Is Nothing, String.Empty, GetQueryString("pros"))
        queryCons = IIf(GetQueryString("cons") Is Nothing, String.Empty, GetQueryString("cons"))
        queryExp = IIf(GetQueryString("exp") Is Nothing, String.Empty, GetQueryString("exp"))

        If Filter.DepartmentId <= 0 Then
            queryDepartmentId = IIf(GetQueryString("DepartmentId") Is Nothing, String.Empty, GetQueryString("DepartmentId"))
            If Not String.IsNullOrEmpty(queryDepartmentId) Then
                Filter.DepartmentId = queryDepartmentId
            End If
        End If

        qrCollection = IIf(String.IsNullOrEmpty("CollectionId"), String.Empty, GetQueryString("CollectionId"))
        qrTone = IIf(String.IsNullOrEmpty("ToneId"), String.Empty, GetQueryString("ToneId"))
        qrShade = IIf(String.IsNullOrEmpty("ShadeId"), String.Empty, GetQueryString("ShadeId"))
        If Not IsPostBack Then
            BindData()
            If Not DisplayNarrowSearch Then
                narrowsearch.Visible = False
            End If
        End If

    End Sub
    Private Function IsSearchPage() As Boolean
        If Me.Request.RawUrl.Contains("search-result.aspx") Then
            Return True
        End If
        Return False
    End Function
    Private Sub BindData()
        isReviewRating = IIf(Me.Request.Url.PathAndQuery.Contains("order-list.aspx") Or Me.Request.Url.PathAndQuery.Contains("product-list.aspx"), True, False)
        If IsSearchPage() Or Me.Request.Url.PathAndQuery.Contains("store/brand.aspx") Or Me.Request.Url.PathAndQuery.Contains("sub-category.aspx") Or isReviewRating Then

            If Me.Request.Url.PathAndQuery.Contains("store/review/product-list.aspx") Then
                lbRateTitle.Text = "Customer Rating"
            Else
                lbRateTitle.Text = "Customer Review"
            End If
            Try
                If IsSearchPage() Then
                    If (Me.Request.RawUrl.Contains("/store/search-result.aspx")) Then
                        dvCategories.Visible = Utility.ConfigData.UseSolr
                        Dim lstCategoryId As String = String.Empty
                        If Not Session("searchDepartmentId") Is Nothing Then
                            lstCategoryId = Session("searchDepartmentId")
                            If Not String.IsNullOrEmpty(lstCategoryId) Then
                                If lstCategoryId.Substring(0, 1) = "," Then
                                    lstCategoryId = lstCategoryId.Substring(1, lstCategoryId.Length - 1)
                                    If lstCategoryId.Length > 1 Then
                                        If lstCategoryId.Substring(lstCategoryId.Length - 1, 1) = "," Then
                                            lstCategoryId = lstCategoryId.Substring(0, lstCategoryId.Length - 1)
                                        End If
                                    End If
                                End If
                            End If
                        End If
                        If (Not Session("searchCountCategory") Is Nothing) Then
                            lstCountCategory = Session("searchCountCategory")
                            If (Not String.IsNullOrEmpty(lstCategoryId)) Then
                                Dim categoryList As String = String.Empty
                                Dim categories As String = String.Empty
                                If lstCategoryId.Contains("_") Then
                                    Dim temp As String() = lstCategoryId.Split("_")
                                    Dim filterDepartment As String() = temp(0).Replace("OR", "|").Split("|")
                                    categoryList = temp(1)
                                    If filterDepartment.Length > 0 Then
                                        For idx As Int16 = 0 To filterDepartment.Length - 1
                                            If (filterDepartment(idx).Contains("departmentid:")) Then
                                                categories += filterDepartment(idx).Split(":")(1) + ","
                                            End If
                                        Next
                                End If
                            Else
                                    categoryList = lstCategoryId
                                End If
                                Dim dt As DataTable = StoreDepartmentRow.GetListCategoryFilterSearchKeyword(Keywords, categoryList, categories)
                                CreateCategoy(dt, lstCountCategory, IIf(lstCategoryId.Contains("_"), True, False))
                            End If
                        End If
                    Else
                        dvCategories.Visible = False
                    End If
                    Dim lstBrandId As String = String.Empty
                    If Not Session("searchBrandId") Is Nothing Then
                        lstBrandId = Session("searchBrandId")
                        If Not String.IsNullOrEmpty(lstBrandId) Then
                            If lstBrandId.Substring(0, 1) = "," Then
                                lstBrandId = lstBrandId.Substring(1, lstBrandId.Length - 1)
                                If lstBrandId.Length > 1 Then
                                    If lstBrandId.Substring(lstBrandId.Length - 1, 1) = "," Then
                                        lstBrandId = lstBrandId.Substring(0, lstBrandId.Length - 1)
                                    End If
                                End If
                            End If
                        End If
                    End If
                    If (Not Session("searchCountBrand") Is Nothing) Then
                        lstCountBrand = Session("searchCountBrand")
                        If (Not String.IsNullOrEmpty(lstBrandId)) Then
                            Dim dt As DataTable = StoreBrandRow.GetListBrandFilterSearchKeyword(Keywords, lstBrandId)
                            CreateSubBrand(dt)
                        End If
                    End If

                    If (Not Session("searchCountPrice") Is Nothing) And Not isReviewRating Then
                        lstCountPrice = Session("searchCountPrice")
                        LoadPrice()
                    End If

                    If (Not Session("searchCountRating") Is Nothing) Then
                        lstCountRating = Session("searchCountRating")
                        LoadRating()
                    End If
                Else
                    footer.Visible = False
                    Dim id As Integer = 0
                    If GetQueryString("DepartmentId") <> 0 Then
                        ' Dim DeptUrlCode As String = GetQueryString("DepartmentURLCode")
                        id = CInt(GetQueryString("DepartmentId"))  'StoreDepartmentRow.GetDepartmentIdByDepertmentCode(DeptUrlCode)
                    End If
                    'LoadCategory()
                    LoadBrand(id)
                    LoadPrice()
                    LoadRating()
                End If
            Catch ex As Exception
                Email.SendError("ToError500", "NarrowSearch", "Url: " & Request.RawUrl & "<br>Exception:" & ex.ToString())
            End Try
            If (Not bCategory) Or isReviewRating Then
                dvCategories.Visible = False
            Else
                DisplayNarrowSearch = True
            End If
            If (Not bBrand) Or isReviewRating Then
                dvBrand.Visible = False
            Else
                DisplayNarrowSearch = True
            End If

            If (Not bPrice) Or isReviewRating Then
                dvPrice.Visible = False
            Else
                DisplayNarrowSearch = True
            End If

            If (Not bRating) Then
                dvRating.Visible = False
            Else
                DisplayNarrowSearch = True
            End If
        Else
            DisplayNarrowSearch = False
            narrowsearch.Visible = False
        End If

    End Sub
    Private Sub LoadBrand(ByVal departmentID As Integer)
        Filter = New DepartmentFilterFields()
        Filter.All = True
        Filter.IsNew = GetQueryString("F_IsNew") <> Nothing
        Filter.Sale24Hour = GetQueryString("F_24Hour") <> Nothing
        Filter.SaleBuy1Get1 = GetQueryString("F_Buy1Get1") <> Nothing
        Dim promotion As String = IIf(GetQueryString("F_Promotion") <> Nothing, IIf(GetQueryString("F_Promotion") = "Promotion.aspx", "Y", GetQueryString("F_Promotion")), GetQueryString("F_Promotion"))
        Filter.HasPromotion = promotion <> Nothing
        Filter.PromotionId = IIf(GetQueryString("PromotionId") <> Nothing AndAlso IsNumeric(GetQueryString("PromotionId")), GetQueryString("PromotionId"), Nothing)
        If Session("OrderId") <> Nothing Then Filter.OrderId = Session("OrderId")
        If Not String.IsNullOrEmpty(qrCollection) Then
            Filter.CollectionId = CInt(qrCollection)
        End If
        If Not String.IsNullOrEmpty(qrShade) Then
            Filter.ShadeId = CInt(qrShade)
        End If
        If Not String.IsNullOrEmpty(qrTone) Then
            Filter.ToneId = CInt(qrTone)
        End If

        If Not String.IsNullOrEmpty(queryBrand) Then
            Filter.BrandId = CInt(queryBrand)
        End If

        Filter.PriceRange = queryPrice
        Filter.RatingRange = queryRating

        If departmentID <= 0 Then
            bBrand = False
        Else
            Filter.DepartmentId = departmentID 'Department.DepartmentId
            Filter.pg = -1
            Dim sp As SitePage = Me.Page

            If Not Me.Request.Url.PathAndQuery.Contains("store/review/product-list.aspx") Then
                SolrHelper.SearchItem(Filter, 1, 1, GetQueryString("sort"), SitePage.GetCurrentFilterActive(Request.RawUrl.Substring(Request.RawUrl.LastIndexOf("?") + 1)))
            End If

            Dim lstBrandId As String = String.Empty
            If Not Session("searchBrandId") Is Nothing Then
                lstBrandId = Session("searchBrandId")
                If Not String.IsNullOrEmpty(lstBrandId) Then
                    If lstBrandId.Substring(0, 1) = "," Then
                        lstBrandId = lstBrandId.Substring(1, lstBrandId.Length - 1)
                        If lstBrandId.Length > 1 Then
                            If lstBrandId.Substring(lstBrandId.Length - 1, 1) = "," Then
                                lstBrandId = lstBrandId.Substring(0, lstBrandId.Length - 1)
                            End If
                        End If
                    End If
                End If
            End If
            If (Not Session("searchCountBrand") Is Nothing) Then
                lstCountBrand = Session("searchCountBrand")
                If (Not String.IsNullOrEmpty(lstBrandId)) Then
                    Dim dt As DataTable = StoreBrandRow.GetListBrandFilterSearchKeyword(Keywords, lstBrandId)
                    CreateSubBrand(dt)
                End If
            End If
            If (Not Session("searchCountPrice") Is Nothing) And Not isReviewRating Then
                lstCountPrice = Session("searchCountPrice")
            End If

            If (Not Session("searchCountRating") Is Nothing) Then
                lstCountRating = Session("searchCountRating")
            End If
        End If

    End Sub
    Private Sub LoadPrice()
        Dim arr As Array = [Enum].GetValues(GetType(Utility.Common.Price))
        Dim lstPrice As New List(Of String)
        If String.IsNullOrEmpty(lstCountPrice) Then
            If (Not String.IsNullOrEmpty(queryBrand) Or Not String.IsNullOrEmpty(queryRating)) And Not IsSearchPage() Then
                Filter.BrandId = IIf(Not String.IsNullOrEmpty(queryBrand), queryBrand, Nothing)
                Filter.RatingRange = IIf(Not String.IsNullOrEmpty(queryRating), queryRating, Nothing)
            End If

            lstPrice = StoreItemRow.CountNarrowSearch(Filter, "Price")
        End If

        If lstPrice Is Nothing And String.IsNullOrEmpty(lstCountPrice) Then
            Exit Sub
        End If

        Dim max As Integer = arr.Length - 1
        Dim x As Integer = 0 'Count 
        For i As Integer = 0 To max
            Dim t As String = String.Empty
            If (i = 0) Then
                t = String.Format(",{0}", CType(arr(i + 1), Integer))
            ElseIf (i = max) Then
                t = String.Format("{0},", CType(arr(i), Integer))
            Else
                t = String.Format("{0},{1}", CType(arr(i), Integer), CType(arr(i + 1), Integer))
            End If

            Dim y As Integer = 0
            If String.IsNullOrEmpty(lstCountPrice) Then
                y = lstPrice(i)
            Else
                For Each s As String In lstCountPrice.Split(";")
                    If s.Trim().Contains(":") Then
                        If s.Contains(t) Then
                            y = CInt(s.Substring(s.IndexOf(":") + 1))
                            Exit For
                        End If
                    End If
                Next
            End If

            ltPriceContent.Text &= ShowPrice(t, y)
            x += IIf(y > 0, 1, 0)
        Next

        bPrice = IIf(String.IsNullOrEmpty(queryPrice), x > 1, x > 0)
    End Sub
    Private Sub LoadRating()

        Dim arr As Array = [Enum].GetValues(GetType(Utility.Common.Rating))
        Dim lstRating As New List(Of String)
        If String.IsNullOrEmpty(lstCountRating) Then
            If (Not String.IsNullOrEmpty(queryBrand) Or Not String.IsNullOrEmpty(queryPrice)) And Not IsSearchPage() Then
                Filter.BrandId = IIf(Not String.IsNullOrEmpty(queryBrand), queryBrand, Nothing)
                Filter.PriceRange = IIf(Not String.IsNullOrEmpty(queryPrice), queryPrice, Nothing)
            End If
            If isReviewRating Then
                Filter.PageParams = Me.Request.Url.PathAndQuery
                Dim typeReview As String = IIf(Filter.PageParams.Contains("order-list"), "order", "product")
                lstRating = StoreItemReviewRow.CountRating(Filter, typeReview)
            Else
                lstRating = StoreItemRow.CountNarrowSearch(Filter, "Rating")
            End If

        End If

        If lstRating Is Nothing And String.IsNullOrEmpty(lstCountRating) Then
            Exit Sub
        End If

        Dim max As Integer = arr.Length - 1
        Dim x As Integer = 0 'Count 
        For i As Integer = 0 To max
            Dim t As String = String.Empty
            If (i = 0) Then
                t = String.Format(",{0}", CType(arr(i + 1), Integer))
            ElseIf (i = max) Then
                t = String.Format("{0},", CType(arr(i), Integer))
            Else
                t = String.Format("{0},{1}", CType(arr(i), Integer), CType(arr(i + 1), Integer))
            End If

            Dim y As Integer = 0
            If String.IsNullOrEmpty(lstCountRating) Then
                y = lstRating(i)
            Else
                For Each s As String In lstCountRating.Split(";")
                    If s.Trim().Contains(":") Then
                        If s.Contains(t) Then
                            y = CInt(s.Substring(s.IndexOf(":") + 1))
                            Exit For
                        End If
                    End If
                Next
            End If

            ltRatingContent.Text &= ShowRating(t, y)
            x += IIf(y > 0, 1, 0)
        Next

        bRating = IIf(String.IsNullOrEmpty(queryRating), x > 1, x > 0)
    End Sub

    Private Function GetLink(ByVal paraName As String, ByVal paraValue As String, ByVal addEmptyValue As Boolean, ByVal resetPageIndex As Boolean) As String
        Dim url As String = Me.Request.RawUrl
        url = URLParameters.AddParamaterToURL(url, "tab", "", False)
        url = URLParameters.AddParamaterToURL(url, paraName, paraValue, addEmptyValue)
        If resetPageIndex Then
            url = URLParameters.AddParamaterToURL(url, "pg", "", False)
        End If
        Return url
    End Function

    Private Function ShowPrice(ByVal price As String, ByVal count As Integer) As String
        Dim result As String = String.Empty

        CountPrice = CountPrice + 1
        Dim countparam As Integer

        If (Not String.IsNullOrEmpty(lstCountPrice)) Then
            Dim check As Integer = lstCountPrice.IndexOf(";(" & price & "):")
            Dim tmp As Integer = 0
            Dim lsttmp As String = String.Empty
            If (check >= 0) Then
                lsttmp = lstCountPrice.Substring(check + price.Length + 4)
                tmp = lsttmp.IndexOf(";")
                If (tmp > 0) Then
                    count = Convert.ToInt32(lsttmp.Substring(0, tmp))
                    countparam += 1
                End If
            End If
        Else
            countparam = count
        End If

        Dim url As String = String.Empty
        ckChecked = String.Empty
        If (Not String.IsNullOrEmpty(price) Or countparam > 0) Then
            hideDiv += 1

            If (queryPrice = "(" & price & ")") Then
                ckChecked = "checked"
                url = GetLink("price", "", True, True)
            Else
                url = GetLink("price", "(" & price & ")", True, True)
            End If

            bPrice = True

            'show price
            Dim sprice As String = String.Empty
            Try
                If price(0) = "," Then
                    sprice = String.Format("Under ${0}", price.Replace(",", ""))
                ElseIf price(price.Length - 1) = "," Then
                    sprice = String.Format("${0}+", price.Replace(",", ""))
                Else
                    sprice = String.Format("${0}", price.Replace(",", " - $"))
                End If
            Catch ex As Exception

            End Try


            If count > 0 Then
                result = String.Format(lstChkact, "chkPrice" & CountPrice, url, ckChecked, sprice, count)
            Else
                'If Request.RawUrl.Contains("?") Then
                result = String.Format(lstChkinact, "chkPrice" & CountPrice, sprice, count)
                'End If
            End If
        End If

        If hideDiv <= 1 Then
            bPrice = False
        End If

        Return result
    End Function

    Private Function ShowRating(ByVal rating As String, ByVal count As Integer) As String
        Dim result As String = String.Empty

        CountRating = CountRating + 1
        Dim countparam As Integer

        If (Not String.IsNullOrEmpty(lstCountRating)) Then
            Dim check As Integer = lstCountRating.IndexOf(";(" & rating & "):")
            Dim tmp As Integer = 0
            Dim lsttmp As String = String.Empty
            If (check >= 0) Then
                lsttmp = lstCountRating.Substring(check + rating.Length + 4)
                tmp = lsttmp.IndexOf(";")
                If (tmp > 0) Then
                    count = Convert.ToInt32(lsttmp.Substring(0, tmp))
                    countparam += 1
                End If
            End If
        Else
            countparam = count
        End If

        Dim url As String = String.Empty
        ckChecked = String.Empty
        If (Not String.IsNullOrEmpty(rating) Or countparam > 0) Then
            hideDiv += 1

            'show Rating
            Dim sRating As String = String.Empty
            Try
                If rating(0) = "," Then
                    sRating = String.Format("Under {0}", rating.Replace(",", ""))

                ElseIf rating(rating.Length - 1) = "," Then
                    sRating = String.Format("{0}+", rating.Replace(",", ""))
                Else
                    sRating = String.Format("{0}", rating.Replace(",", " - "))
                End If
            Catch ex As Exception

            End Try

            If rating(0) = "," Then
                rating = "0" & rating
            End If

            If (queryRating = "(" & rating & ")") Then
                ckChecked = "checked"
                url = GetLink("rating", "", True, True)
            Else
                url = GetLink("rating", "(" & rating & ")", True, True)
            End If

            bRating = True

            If count > 0 Then
                result = String.Format(lstChkact, "chkRating" & CountRating, url, ckChecked, sRating, count)
            Else
                result = String.Format(lstChkinact, "chkRating" & CountRating, sRating, count)
            End If
        End If

        If hideDiv <= 1 Then
            bRating = False
        End If

        Return result
    End Function

    Private Sub CreateSubBrand(ByVal dt As DataTable)
        Dim isShow As Boolean = False
        Dim checkShow As Boolean = False
        Dim count As Integer = dt.DefaultView.Count() - 1
        If count = 0 Then
            dvBrand.Visible = False
            Exit Sub
        End If
        CountPrice = 0

        Dim lstBrandId As String = "Brand"
        For Each dr As DataRow In dt.Rows
            lstBrandId &= "," & dr("BrandId").ToString()
        Next

        If Not String.IsNullOrEmpty(queryRating) Or Not String.IsNullOrEmpty(queryPrice) Then
            Filter.RatingRange = IIf(String.IsNullOrEmpty(queryRating), Nothing, queryRating)
            Filter.PriceRange = IIf(String.IsNullOrEmpty(queryPrice), Nothing, queryPrice)
        End If
        Dim lstBrand As List(Of String) = StoreItemRow.CountNarrowSearch(Filter, lstBrandId)

        Dim visible As Int16 = 0
        For i As Integer = 0 To count
            ckChecked = String.Empty
            CountPrice = CountPrice + 1
            bBrand = True
            Dim dr As DataRow = dt.Rows(i)
            Dim url As String = String.Empty
            If Me.Request.RawUrl.Contains("nail-brand") And Not String.IsNullOrEmpty(queryBrand) Then
                url = Me.Request.RawUrl.Replace(queryBrand, dr("BrandId").ToString())
            Else
                url = GetLink("brandid", dr("BrandId").ToString(), True, True)
            End If

            Dim count1 As Integer = 0
            Dim CountBrand As String = ""
            If (Not String.IsNullOrEmpty(lstCountBrand)) Then
                Dim id As String = dr("BrandId").ToString()
                Dim check As Integer = lstCountBrand.IndexOf("," & id & ":")
                Dim tmp As Integer = 0
                Dim lsttmp As String = String.Empty
                If (check >= 0) Then
                    lsttmp = lstCountBrand.Substring(check + id.Length + 2)
                    tmp = lsttmp.IndexOf(",")
                    If (tmp > 0) Then
                        ' CountBrand = "(" & lsttmp.Substring(0, tmp) & ")"
                        count1 = lsttmp.Substring(0, tmp)
                    End If
                End If
            Else
                If Not String.IsNullOrEmpty(queryRating) Or Not String.IsNullOrEmpty(queryPrice) Then
                    'Filter.RatingRange = IIf(String.IsNullOrEmpty(queryRating), Nothing, queryRating)
                    'Filter.PriceRange = IIf(String.IsNullOrEmpty(queryPrice), Nothing, queryPrice)
                    'Filter.BrandId = dr("BrandId")
                    count1 = lstBrand(i) 'StoreItemRow.GetCountActiveItemsNarrowSearch(DB, Filter)
                    If count1 > 0 Then
                        visible = True
                    End If
                Else
                    count1 = dr("CountBrand")
                End If

                'CountBrand = "(" & IIf(count1 <> dr("CountBrand"), count1, dr("CountBrand").ToString()) & ")"
            End If
            CountBrand = "(" & count1 & ")"
            If (i = 0) Then
                ltBrand.Text = "<ul class=""parent"">"
            End If
            ' Dim cssClass As String = String.Empty
            If (queryBrand = dr("BrandId").ToString()) Then
                'cssClass = "class=""select"""
                ckChecked = "checked"
                url = GetLink("brandid", "", True, True)
                checkShow = True
            End If
            hideDiv += 1
            If count1 > 0 Then
                visible = visible + 1
                ltBrand.Text = ltBrand.Text & "<li class=""checkbox""><label for=""chkBrand" & CountPrice & """>" & vbCrLf &
                "<input type=""checkbox"" id=""chkBrand" & CountPrice & """ onclick=""window.location='" & url & "'""" & ckChecked & ">" & vbCrLf &
              "<i class=""fa fa-check checkbox-font"" ></i><a href=""" & url & """>" & dr("BrandName").ToString() & "</a><span class=""count""> " & CountBrand & "</span></label></li>"

                'ltBrand.Text = String.Format(lstChkact, "chkBrand" & CountPrice, url, ckChecked, dr("BrandName").ToString(), CountBrand)
            Else
                ltBrand.Text = ltBrand.Text & "<li class=""checkbox""><label for=""chkBrand" & CountPrice & """>" & vbCrLf &
                "<i class=""fa fa-check checkbox-font  bg-disable""></i>" & dr("BrandName").ToString() & "<span class=""count""> " & CountBrand & "</span></label></li>"
                'ltBrand.Text = String.Format(lstChkinact, "chkBrand" & CountPrice, dr("BrandName").ToString(), CountBrand)
            End If

            If (count > 10 And i = 10) Then
                If Not String.IsNullOrEmpty(queryBrand) And Not checkShow Then
                    ltBrand.Text = ltBrand.Text & "</ul><ul id=""more-brand"" class=""parent"">"
                Else
                    ltBrand.Text = ltBrand.Text & "</ul><ul id=""more-brand"" style=""display:none;"" class=""parent"">"
                    'ltBrand.Text = ltBrand.Text & "</ul><ul id=""more-brand"" class=""parent hidden"">"
                    isShow = True
                End If
            End If
            If (i = count) Then
                ltBrand.Text = ltBrand.Text & "</ul>"
            End If
        Next
        If hideDiv <= 1 Or (visible <= 1 And String.IsNullOrEmpty(queryBrand)) Then
            bBrand = False
        End If
        If (isShow) Then
            ltBrand.Text = ltBrand.Text & "<div class=""dvmore""><span onclick=""showAllBrand();"" id=""show-brand"" class=""more"">Show more<b class=""arrow-down""></b></span></div>"
        End If
        hideDiv = 0
    End Sub

    Private Function ListPros() As List(Of String)
        Dim lst As List(Of String) = New List(Of String)
        lst.Add("Easy To Use")
        lst.Add("High Quality")
        lst.Add("Good Value")
        lst.Add("Long Lasting")
        Return lst
    End Function

    Private Function ListCons() As List(Of String)
        Dim lst As List(Of String) = New List(Of String)
        lst.Add("Difficult To Use")
        lst.Add("Poor Quality")
        lst.Add("Expensive")
        lst.Add("Does Not Working")
        Return lst
    End Function

    Private Function ListExpLevel() As List(Of String)
        Dim lst As List(Of String) = New List(Of String)
        lst.Add("Student")
        lst.Add("1+ years")
        lst.Add("3+ years")
        lst.Add("5+ years")
        Return lst
    End Function


    Protected Sub rpPros_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rpPros.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim hlPros As HyperLink = CType(e.Item.FindControl("hlPros"), HyperLink)
            Dim pros As String = e.Item.DataItem()
            hlPros.Text = pros
            Dim url As String = String.Empty
            If (queryPros.Equals(Escape(pros))) Then
                hlPros.CssClass = "select"
                url = GetLink("pros", "", True, True)
            Else
                url = GetLink("pros", Escape(pros), True, True)
            End If
            hlPros.NavigateUrl = url
        End If
    End Sub

    Protected Sub rpCons_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rpCons.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim hlCons As HyperLink = CType(e.Item.FindControl("hlCons"), HyperLink)
            Dim cons As String = e.Item.DataItem()
            hlCons.Text = cons
            Dim url As String = String.Empty
            If (queryCons.Equals(Escape(cons))) Then
                hlCons.CssClass = "select"
                url = GetLink("cons", "", True, True)
            Else
                url = GetLink("cons", Escape(cons), True, True)
            End If
            hlCons.NavigateUrl = url
        End If
    End Sub

    Protected Sub rpExpLevel_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rpExpLevel.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim hlExpLevel As HyperLink = CType(e.Item.FindControl("hlExpLevel"), HyperLink)
            Dim ExpLevel As String = e.Item.DataItem()
            hlExpLevel.Text = ExpLevel
            Dim url As String = String.Empty
            If (queryExp.Equals(Escape(ExpLevel))) Then
                hlExpLevel.CssClass = "select"
                url = GetLink("exp", "", True, True)
            Else
                url = GetLink("exp", Escape(ExpLevel), True, True)
            End If
            hlExpLevel.NavigateUrl = url
        End If
    End Sub

    Private Sub LoadCategory()
        ddlCategory.DataSource = StoreDepartmentRow.GetFilterProductReview()
        ddlCategory.DataTextField = "Name"
        ddlCategory.DataValueField = "DepartmentId"
        ddlCategory.DataBind()
        If ddlCategory.Items.Count > 0 Then
            dvCategory.Visible = True
            ddlCategory.Items.Insert(0, New ListItem("--- All ---", ""))
            ddlCategory.SelectedValue = Request("DepartmentId")
        Else
            dvCategory.Visible = False
        End If
    End Sub

    Protected Sub ddlCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCategory.SelectedIndexChanged
        Dim url As String = GetLink("DepartmentId", ddlCategory.SelectedValue, True, True)
        If ddlCategory.Items.Count < 1 Then
            url = URLParameters.AddParamaterToURL(url, "DepartmentId", "", True)
        End If
        Response.Redirect(url)
    End Sub

    Private Function Escape(ByVal src As String) As String
        src = src.Trim() ''.ToLower()
        src = src.Replace(" ", "-")
        src = src.Replace("+", "")
        Return src
    End Function

    Private Function EscapePrice(ByVal PriceDesc As String) As String
        PriceDesc = PriceDesc.Trim().Replace(" ", "")
        PriceDesc = PriceDesc.Replace("$", "")
        PriceDesc = PriceDesc.Replace("-", ",")
        PriceDesc = PriceDesc.Replace("+", ",")
        PriceDesc = PriceDesc.ToLower().Replace("under", ",")
        Return PriceDesc
    End Function
    Private Sub render(ByVal dr As DataRow, ByVal lstCategory As List(Of String), ByVal firstCategory As Boolean, ByRef checkShow As Boolean, ByVal disableCount0 As Boolean, ByRef isShow As Boolean, ByRef visible As Integer)
        ckChecked = String.Empty
        CountPrice = CountPrice + 1
        bCategory = True
        Dim url As String = String.Empty
        If Me.Request.RawUrl.Contains("nail-brand") And Not String.IsNullOrEmpty(queryDepartmentId) Then
            url = Me.Request.RawUrl.Replace(queryDepartmentId, dr("DepartmentId").ToString())
        Else
            url = GetLink("DepartmentId", dr("DepartmentId").ToString(), True, True)
        End If

        Dim count1 As Integer = 0
        Dim CountCategory As String = ""
        If (Not String.IsNullOrEmpty(lstCountCategory)) Then
            Dim id As String = dr("DepartmentId").ToString()
            Dim check As Integer = lstCountCategory.IndexOf("," & id & ":")
            Dim tmp As Integer = 0
            Dim lsttmp As String = String.Empty
            If (check >= 0) Then
                lsttmp = lstCountCategory.Substring(check + id.Length + 2)
                tmp = lsttmp.IndexOf(",")
                If (tmp > 0) Then
                    ' CountBrand = "(" & lsttmp.Substring(0, tmp) & ")"
                    count1 = lsttmp.Substring(0, tmp)

                End If
            End If
        Else
            If Not String.IsNullOrEmpty(queryRating) Or Not String.IsNullOrEmpty(queryPrice) Then
                'count1 = lstCategory(i) 'StoreItemRow.GetCountActiveItemsNarrowSearch(DB, Filter)
            Else
                count1 = dr("CountCategory")
            End If

            'CountBrand = "(" & IIf(count1 <> dr("CountBrand"), count1, dr("CountBrand").ToString()) & ")"
        End If

        CountCategory = "(" & count1 & ")"
        If (firstCategory) Then
            ltCategory.Text = "<ul class=""parent"">"
        End If
        ' Dim cssClass As String = String.Empty
        If (queryDepartmentId = dr("DepartmentId").ToString()) Then
            'cssClass = "class=""select"""
            ckChecked = "checked"
            url = GetLink("DepartmentId", "", True, True)
            checkShow = True
        End If
        hideDiv += 1
        If count1 > 0 Then
            countCategoryRender = countCategoryRender + 1
            visible = visible + 1
            ltCategory.Text = ltCategory.Text & "<li class=""checkbox""" & IIf(disableCount0 AndAlso dr("ParentId") <> 23, "style='margin-left: 22px;'", "") & "><label for=""chkCategory" & CountPrice & """>" & vbCrLf &
            "<input type=""checkbox"" id=""chkCategory" & CountPrice & """ onclick=""window.location='" & url & "'""" & ckChecked & ">" & vbCrLf &
          "<i class=""fa fa-check checkbox-font"" ></i><a href=""" & url & """>" & dr("DepartmentName").ToString() & "</a><span class=""count""> " & CountCategory & "</span></label></li>"

            'ltBrand.Text = String.Format(lstChkact, "chkBrand" & CountPrice, url, ckChecked, dr("BrandName").ToString(), CountBrand)
        ElseIf Not disableCount0
            countCategoryRender = countCategoryRender + 1
            ltCategory.Text = ltCategory.Text & "<li class=""checkbox""><label for=""chkCategory" & CountPrice & """>" & vbCrLf &
            "<i class=""fa fa-check checkbox-font bg-disable"" ></i>" & dr("DepartmentName").ToString() & "<span class=""count""> " & CountCategory & "</span></label></li>"
            'ltBrand.Text = String.Format(lstChkinact, "chkBrand" & CountPrice, dr("BrandName").ToString(), CountBrand)
        End If

        If (countCategoryRender = 11) Then
            If Not String.IsNullOrEmpty(queryDepartmentId) And Not checkShow Then
                ltCategory.Text = ltCategory.Text & "</ul><ul id=""more-category"" class=""parent"">"
            Else
                ltCategory.Text = ltCategory.Text & "</ul><ul id=""more-category"" style=""display:none;"" class=""parent"">"
                'ltBrand.Text = ltBrand.Text & "</ul><ul id=""more-brand"" class=""parent hidden"">"
                isShow = True
            End If
        End If

    End Sub
    Private Sub CreateCategoy(dt As DataTable, categoryCount As String, Optional ByVal disableCount0 As Boolean = False)
        Dim isShow As Boolean = False
        Dim checkShow As Boolean = False
        Dim count As Integer = dt.DefaultView.Count() - 1
        If count = 0 Then
            dvCategories.Visible = False
            Exit Sub
        End If
        CountPrice = 0

        Dim lstCategoryId As String = "Category"
        For Each dr As DataRow In dt.Rows
            lstCategoryId &= "," & dr("DepartmentId").ToString()
        Next

        Dim lstCategory As List(Of String) = New List(Of String)

        Dim visible As Int16 = 0

        If disableCount0 Then
            Dim dtParentCategory = dt.Select("ParentId = 23")
            If dtParentCategory.Length > 0 Then
                For i As Integer = 0 To dtParentCategory.Length - 1
                    render(dtParentCategory(i), lstCategory, IIF(i = 0, True, False), checkShow, disableCount0, isShow, visible)
                    Dim dtChild = dt.Select(String.Format("ParentId = {0}", dtParentCategory(i)("DepartmentId")))
                    If dtChild.Length > 0 Then
                        For index = 0 To dtChild.Length - 1
                            render(dtChild(index), lstCategory, False, checkShow, disableCount0, isShow, visible)
                        Next
                    End If
                Next
            End If
        Else
            For i As Integer = 0 To count
                render(dt.Rows(i), lstCategory, IIF(i = 0, True, False), checkShow, disableCount0, isShow, visible)
            Next
        End If

        ltCategory.Text = ltCategory.Text & "</ul>"
        If hideDiv <= 1 Or (visible <= 1 And String.IsNullOrEmpty(queryDepartmentId)) Then
            bCategory = False
        End If
        If (isShow) Then
            ltCategory.Text = ltCategory.Text & "<div class=""dvmoreCategory""><span onclick=""showAllCategory();"" id=""show-category"" class=""more"">Show more<b class=""arrow-down""></b></span></div>"
        End If
        hideDiv = 0
    End Sub
End Class
