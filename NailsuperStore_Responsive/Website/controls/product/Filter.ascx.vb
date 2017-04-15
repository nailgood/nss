Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Collections.Generic

Partial Class control_Filter
    Inherits BaseControl

    Protected PageParams As String

    Private m_Department, m_ParentDepartment, m_Polish, m_TopCoat, m_PediSpas As StoreDepartmentRow
    Private m_Filter As DepartmentFilterFields
    Private m_FilterBrands As Boolean = True
    Private m_FilterCollections As Boolean = False
    Private m_FilterTones As Boolean = False
    Private m_FilterShades As Boolean = False
    Private m_IsFilter As Boolean = False
    Private m_Title As String = "Refine Results"
    Public ItemCollection As StoreItemCollection
    Private m_CountDr As Integer = 0
    Protected css, css1, css2 As String
    Private m_ItemCollectionCount As Integer
    Public Property Title() As String
        Get
            Return m_Title
        End Get
        Set(ByVal value As String)
            m_Title = value
        End Set
    End Property
    Public Property ItemCollectionCount() As Integer
        Get
            Return m_ItemCollectionCount
        End Get
        Set(ByVal value As Integer)
            m_ItemCollectionCount = value
        End Set
    End Property
    Public Property FilterBrands() As Boolean
        Get
            Return m_FilterBrands
        End Get
        Set(ByVal value As Boolean)
            m_FilterBrands = value
        End Set
    End Property

    Public Property FilterCollections() As Boolean
        Get
            Return m_FilterCollections
        End Get
        Set(ByVal value As Boolean)
            m_FilterCollections = value
        End Set
    End Property
    Public Property IsFilter() As Boolean
        Get
            Return m_IsFilter
        End Get
        Set(ByVal value As Boolean)
            m_IsFilter = value
        End Set
    End Property

    Public Property FilterTones() As Boolean
        Get
            Return m_FilterTones
        End Get
        Set(ByVal value As Boolean)
            m_FilterTones = value
        End Set
    End Property

    Public Property FilterShades() As Boolean
        Get
            Return m_FilterShades
        End Get
        Set(ByVal value As Boolean)
            m_FilterShades = value
        End Set
    End Property

    Public Property Filter() As DepartmentFilterFields
        Get
            Return m_Filter
        End Get
        Set(ByVal value As DepartmentFilterFields)
            m_Filter = value
        End Set
    End Property

    Public Property Polish() As StoreDepartmentRow
        Get
            Return m_Polish
        End Get
        Set(ByVal value As StoreDepartmentRow)
            m_Polish = value
        End Set
    End Property

    Public Property TopCoat() As StoreDepartmentRow
        Get
            Return m_TopCoat
        End Get
        Set(ByVal value As StoreDepartmentRow)
            m_TopCoat = value
        End Set
    End Property

    Public Property PediSpas() As StoreDepartmentRow
        Get
            Return m_PediSpas
        End Get
        Set(ByVal value As StoreDepartmentRow)
            m_PediSpas = value
        End Set
    End Property

    Public Property Department() As StoreDepartmentRow
        Get
            Return m_Department
        End Get
        Set(ByVal value As StoreDepartmentRow)
            m_Department = value
        End Set
    End Property

    Public Property ParentDepartment() As StoreDepartmentRow
        Get
            Return m_ParentDepartment
        End Get
        Set(ByVal value As StoreDepartmentRow)
            m_ParentDepartment = value
        End Set
    End Property
    Public Property CountDr() As Integer
        Get
            Return m_CountDr
        End Get
        Set(ByVal value As Integer)
            m_CountDr = value
        End Set
    End Property
    Private Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        'If Polish Is Nothing Then Polish = StoreDepartmentRow.GetRow(DB, 186)
        'If TopCoat Is Nothing Then TopCoat = StoreDepartmentRow.GetRow(DB, 56)
        'If PediSpas Is Nothing Then PediSpas = StoreDepartmentRow.GetRow(DB, 54)

    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load


        If Not IsPostBack Then
            If IsSearchPage() Then

                If ItemCollectionCount < 1 Then
                    Me.Visible = False
                    Exit Sub
                End If

            End If
            Try
                BindData()
            Catch ex As Exception

            End Try

        End If
    End Sub

    Private Function IsSearchPage() As Boolean
        If Me.Request.RawUrl.Contains("search-result.aspx") Then
            Return True
        End If
        Return False
    End Function

    Private Sub BindData()

        Dim CustomerPriceGroupId As Integer = 0
        If Not IsNumeric(System.Web.HttpContext.Current.Session("CustomerPriceGroupId")) Then
            CustomerPriceGroupId = StoreItemRow.GetCustomerPriceGroupByMember(Filter.MemberId)
        End If

        If IsSearchPage() Then
            sFeatures.Visible = False
        Else
            sFeatures.Visible = False

            'Khoa tat vi ko co data
            'Try
            '    ddlFeatures.DataSource = StoreFeatureRow.GetAllFeatures(DB, Filter, Request.RawUrl, CustomerPriceGroupId)
            '    ddlFeatures.DataTextField = "name"
            '    ddlFeatures.DataValueField = "FeatureId"
            '    ddlFeatures.DataBind()
            '    If ddlFeatures.Items.Count > 0 Then
            '        sFeatures.Visible = True
            '        ddlFeatures.Items.Insert(0, New ListItem("All Features", ""))
            '        ddlFeatures.SelectedValue = Request("featureid")
            '        m_CountDr += 1
            '    Else
            '        sFeatures.Visible = False
            '    End If
            'Catch ex As Exception
            '    sFeatures.Visible = False
            'End Try

        End If

        If Not Request.RawUrl.Contains("/nail-brand") AndAlso FilterBrands Then
            PageParams = CType(MyBase.Page, BasePage).GetPageParams(FilterFieldType.All)
            If PageParams.Contains("F_Promotion=Y") Then
                Dim i As Integer
                Dim strBrand As String = ""
                Dim filter1 As DepartmentFilterFields = Filter
                filter1.MaxPerPage = Int16.MaxValue
                Dim total As Integer
                Dim si As StoreItemCollection = StoreItemRow.GetActiveItems(DB, filter1, total)
                For i = 0 To si.Count - 1
                    strBrand &= IIf(strBrand = "", si(i).BrandId, "," & si(i).BrandId)
                Next
                If Session("ListBrandId") Is Nothing Then
                    Filter.ListBrandId = strBrand
                    Session("ListBrandId") = strBrand
                Else
                    Filter.ListBrandId = Session("ListBrandId")
                End If
            Else
                Session.Remove("ListBrandId")
            End If

            Dim tmp As Boolean = Filter.All
            Filter.All = True
            'If Me.Request.RawUrl.Contains("search-result.aspx") Then
            '    Dim lstBrandId As String = String.Empty
            '    If Not Session("searchBrandId") Is Nothing Then
            '        lstBrandId = Session("searchBrandId")
            '        If Not String.IsNullOrEmpty(lstBrandId) Then
            '            If lstBrandId.Substring(0, 1) = "," Then
            '                lstBrandId = lstBrandId.Substring(1, lstBrandId.Length - 1)
            '                If lstBrandId.Length > 1 Then
            '                    If lstBrandId.Substring(lstBrandId.Length - 1, 1) = "," Then
            '                        lstBrandId = lstBrandId.Substring(0, lstBrandId.Length - 1)
            '                    End If
            '                End If
            '            End If
            '        End If
            '    End If

            '    If (String.IsNullOrEmpty(lstBrandId)) Then
            '        ddlBrands.DataSource = StoreBrandRow.GetAllStoreBrands(DB, Filter)
            '    Else
            '        ddlBrands.DataSource = StoreBrandRow.GetListBrandFilterSearchKeyword(Filter.Keyword, lstBrandId)
            '    End If
            'Else
            '    ddlBrands.DataSource = StoreBrandRow.GetAllStoreBrands(DB, Filter)
            'End If

            Filter.All = tmp
            'ddlBrands.DataTextField = "BrandName"
            'ddlBrands.DataValueField = "BrandId"
            'ddlBrands.DataBind()

            'If ddlBrands.Items.Count > 0 Then
            '    ddlBrands.Items.Insert(0, New ListItem("All Brands", ""))
            '    If Request("BrandId") <> Nothing Then
            '        ddlBrands.SelectedValue = Request("brandId")
            '    End If
            'Else
            '    sBrand.Visible = False
            'End If

            'If ddlBrands.Items.Count > 2 Then
            '    sBrand.Visible = True
            'Else
            '    sBrand.Visible = False
            'End If
            If IsSearchPage() Then
                Dim list As Generic.List(Of ListItem) = New Generic.List(Of ListItem)()
                If Not Session("searchCollectionId") Is Nothing Then 'Khong co collection
                    Dim countCollections As String = Session("searchCountCollection").ToString()
                    If countCollections = "," And GetQueryString("collectionid") = Nothing Then
                        sCollections.Visible = False
                    Else
                        Dim collectionReal = countCollections.Split(",").Where(Function(i) i.Trim().Length > 0).Select(Function(j) j.Split(":")(0))
                        If collectionReal.Count() = 1 And GetQueryString("collectionid") = Nothing Then
                            sCollections.Visible = False
                        Else

                            'Dim lstCollectionId = Session("searchCollectionId")
                            list = StoreCollectionRow.GetCollectionFilter(String.Join(",", collectionReal))

                            If GetQueryString("toneid") <> Nothing Or GetQueryString("shadeid") <> Nothing Then
                                list.RemoveAll(Function(i) Not collectionReal.Any(Function(col) col.Trim() = i.Value.Trim()))
                            End If

                            If list.Count <= 1 And GetQueryString("collectionid") = Nothing Then
                                sCollections.Visible = False
                            ElseIf GetQueryString("collectionid") <> Nothing And list.Count = 1 Then
                                ddlCollections.Items.Add(New ListItem("Collection", ""))
                                For Each item As ListItem In list
                                    ddlCollections.Items.Add(item)
                                Next
                                ddlCollections.SelectedValue = Request("collectionid")
                                CountDr += 1
                                sCollections.Visible = True
                            End If

                            If list IsNot Nothing AndAlso list.Count > 1 Then
                                ddlCollections.Items.Add(New ListItem("Collection", ""))
                                For Each item As ListItem In list
                                    ddlCollections.Items.Add(item)
                                Next
                                ddlCollections.SelectedValue = Request("collectionid")
                                CountDr += 1
                                sCollections.Visible = True
                            Else
                                If GetQueryString("collectionid") = Nothing Then
                                    sCollections.Visible = False
                                End If

                            End If
                        End If

                    End If

                End If
                'Tone
                If Not Session("searchToneId") Is Nothing Then
                    Dim countTones As String = Session("searchCountTone").ToString()
                    If countTones = ", " And GetQueryString("toneid") = Nothing Then
                        sTones.Visible = False
                    Else
                        Dim toneReal = countTones.Split(", ").Where(Function(i) i.Trim().Length > 0).Select(Function(j) j.Split(": ")(0))
                        If toneReal.Count() = 1 And GetQueryString("toneid") = Nothing Then
                            sTones.Visible = False
                        Else
                            Dim lstToneId = Session("searchToneId")
                            list = StoreToneRow.GetToneFilter(String.Join(",", toneReal))

                            If GetQueryString("collectionid") <> Nothing Or GetQueryString("shadeid") <> Nothing Then
                                list.RemoveAll(Function(i) Not toneReal.Any(Function(col) col.Trim() = i.Value.Trim()))
                            End If

                            If list.Count <= 1 And GetQueryString("toneid") = Nothing Then
                                sTones.Visible = False
                            ElseIf GetQueryString("toneid") <> Nothing And list.Count = 1 Then
                                ddlTones.Items.Add(New ListItem("Tones", ""))
                                For Each item As ListItem In list
                                    ddlTones.Items.Add(item)
                                Next

                                ddlTones.SelectedValue = Request("toneid")
                                CountDr += 1
                                sTones.Visible = True
                            End If

                            If list IsNot Nothing AndAlso list.Count > 1 Then
                                ddlTones.Items.Add(New ListItem("Tones", ""))
                                For Each item As ListItem In list
                                    ddlTones.Items.Add(item)
                                Next
                                'ddlTones.DataSource = list
                                'ddlTones.DataBind()

                                ddlTones.SelectedValue = Request("toneid")
                                CountDr += 1
                                sTones.Visible = True
                            Else
                                If GetQueryString("toneid") = Nothing Then
                                    sTones.Visible = False
                                End If
                            End If
                        End If
                    End If
                End If

                'Shades
                If Not Session("searchShadeId") Is Nothing Then
                    Dim countshades As String = Session("searchCountShade").ToString()
                    If countshades = "," And GetQueryString("shadeid") = Nothing Then
                        sShades.Visible = False
                    Else
                        Dim shadeReal = countshades.Split(",").Where(Function(i) i.Trim().Length > 0).Select(Function(j) j.Split(":")(0))
                        If shadeReal.Count() = 1 And GetQueryString("shadeid") = Nothing Then
                            sShades.Visible = False
                        Else
                            Dim lstShadeId = Session("searchShadeId")
                            list = StoreShadeRow.GetShadeFilter(String.Join(",", shadeReal))

                            If GetQueryString("collectionid") <> Nothing Or GetQueryString("toneid") <> Nothing Then
                                list.RemoveAll(Function(i) Not shadeReal.Any(Function(col) col.Trim() = i.Value.Trim()))
                            End If

                            If list.Count <= 1 And GetQueryString("shadeid") = Nothing Then
                                sShades.Visible = False
                            ElseIf GetQueryString("shadeid") <> Nothing And list.Count = 1
                                ddlShades.Items.Add(New ListItem("Shades", ""))
                                For Each item As ListItem In list
                                    ddlShades.Items.Add(item)
                                Next

                                ddlShades.SelectedValue = Request("Shadeid")
                                CountDr += 1
                                sShades.Visible = True
                            End If

                            If list IsNot Nothing AndAlso list.Count > 1 Then
                                ddlShades.Items.Add(New ListItem("Shades", ""))
                                For Each item As ListItem In list
                                    ddlShades.Items.Add(item)
                                Next

                                ddlShades.SelectedValue = Request("Shadeid")
                                CountDr += 1
                                sShades.Visible = True
                            Else
                                If GetQueryString("shadeid") = Nothing Then
                                    sShades.Visible = False
                                End If
                            End If
                        End If
                    End If
                End If

            Else
                'Collection
                Dim list = StoreCollectionRow.GetAllCollections(Filter, Request.RawUrl, CustomerPriceGroupId)

                If list IsNot Nothing AndAlso list.Count > 1 Then
                    ddlCollections.Items.Add(New ListItem("Collection", ""))
                    For Each item As ListItem In list
                        ddlCollections.Items.Add(item)
                    Next
                    'ddlCollections.DataSource = list
                    'ddlCollections.DataBind()

                    ddlCollections.SelectedValue = Request("collectionid")
                    m_CountDr += 1
                    sCollections.Visible = True
                Else

                    sCollections.Visible = False
                End If

                'Tone
                list = StoreToneRow.GetAllTones(Filter, Request.RawUrl, CustomerPriceGroupId)
                If list IsNot Nothing AndAlso list.Count > 1 Then
                    ddlTones.Items.Add(New ListItem("Tones", ""))
                    For Each item As ListItem In list
                        ddlTones.Items.Add(item)
                    Next
                    'ddlTones.DataSource = list
                    'ddlTones.DataBind()

                    ddlTones.SelectedValue = Request("toneid")
                    sTones.Visible = True
                Else
                    sTones.Visible = False
                End If

                'Shades
                list = StoreShadeRow.GetAllShades(Filter, Request.RawUrl, CustomerPriceGroupId)
                If list IsNot Nothing AndAlso list.Count > 1 Then
                    ddlShades.Items.Add(New ListItem("Shades", ""))
                    For Each item As ListItem In list
                        ddlShades.Items.Add(item)
                    Next
                    'ddlShades.DataSource = list
                    'ddlShades.DataBind()

                    ddlShades.SelectedValue = Request("Shadeid")
                    sShades.Visible = True
                Else
                    sShades.Visible = False
                End If
            End If

        Else
            'sBrand.Visible = False
            sCollections.Visible = False
            sTones.Visible = False
            sShades.Visible = False
        End If

        Dim isCollection As Boolean = LCase(Request.RawUrl).Contains("nail-collection")
        If isCollection = False Then
            css2 = " visible-lg visible-md"
        Else
            css2 = ""
        End If
        If m_CountDr = 0 And isCollection = False Then
            css = "hidden"
            css1 = "d-inline"
        Else
            If isCollection = False Then
                css = ""
            Else
                css = " hidden-xs hidden-sm "
            End If

            css1 = "tb-cell"
        End If

        Dim HasHotItem As Boolean = False
        Dim HasFeaturedItem As Boolean = False
        Dim HasNewItem As Boolean = False
        Dim HasBestSeller As Boolean = False
        Dim hasToprate As Boolean = False
        If IsSearchPage() Then
            Dim isSearchKW As String = SitePage.GetQueryString("searchkw")
            If isSearchKW = "1" Then
                Filter.IsSearchKeyWord = True
            Else
                Filter.IsSearchKeyWord = False
            End If
            If Not Filter.IsSearchKeyWord Then
                HasNewItem = HttpContext.Current.Session("searchHasNew")
                HasHotItem = False '' HttpContext.Current.Session("searchHasHot")
                HasBestSeller = False '' HttpContext.Current.Session("searchHasBestSale")
                hasToprate = HttpContext.Current.Session("searchHasToprate")
            Else
                StoreItemRow.CheckShowSortFieldInSearchKeyword(SitePage.GetQueryString("kw"), HasNewItem, HasHotItem, HasBestSeller, hasToprate)
            End If

        Else
            If Not ItemCollection Is Nothing Then
                hasToprate = True
                For Each item As StoreItemRow In ItemCollection
                    If item.IsHot Then
                        HasHotItem = True
                    End If
                    If item.IsFeatured Then
                        HasFeaturedItem = True
                    End If
                    If item.IsNew Then
                        HasNewItem = True
                    End If
                    If item.IsBestSeller Then
                        HasBestSeller = True
                    End If
                Next
            End If
        End If

        If Not Request.RawUrl.Contains("/nail-supply") And Not Request.RawUrl.Contains("/nail-collection") Then
            If Not HasHotItem Then ddlSortBy.Items.Remove(ddlSortBy.Items.FindByValue("hot-items"))
            If Not HasNewItem Then ddlSortBy.Items.Remove(ddlSortBy.Items.FindByValue("new-items"))
            If Not HasBestSeller Then ddlSortBy.Items.Remove(ddlSortBy.Items.FindByValue("best-sellers"))
            If Not hasToprate Then
                ddlSortBy.Items.Remove(ddlSortBy.Items.FindByValue("top-rated"))
                ddlSortBy.Items.Remove(ddlSortBy.Items.FindByValue("most-popular-review"))
            End If

        End If

        ddlSortBy.SelectedValue = Request("sort")

        'Collection hidden field Sort
        hidSortBy.Value = Request.RawUrl
        If Request("sort") IsNot Nothing Then
            hidSortBy.Value = hidSortBy.Value.Replace("sort=" & GetQueryString("sort"), "").Replace("&&", "&")
        End If
        Dim s As String = hidSortBy.Value.Substring(hidSortBy.Value.Length - 1)
        If s <> "?" And s <> "&" Then
            hidSortBy.Value &= IIf(hidSortBy.Value.Contains("?"), "&", "?")
        End If
    End Sub
    Private Function GetLink(ByVal paraName As String, ByVal paraValue As String, ByVal addEmptyValue As Boolean, ByVal resetPageIndex As Boolean) As String
        Dim url As String = Request.UrlReferrer.ToString
        url = URLParameters.AddParamaterToURL(url, paraName, paraValue, addEmptyValue)
        If resetPageIndex Then
            url = URLParameters.AddParamaterToURL(url, "pg", "", False)
        End If
        '' Response.Redirect(url)
        Return url
    End Function
    Public Sub ddlShades_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlShades.SelectedIndexChanged
        Dim url As String = GetLink("shadeid", ddlShades.SelectedValue, False, True)
        If ddlFeatures.Items.Count < 1 Then
            url = URLParameters.AddParamaterToURL(url, "featureid", "", False)
        End If
        If ddlCollections.Items.Count < 1 Then
            url = URLParameters.AddParamaterToURL(url, "collectionid", "", False)
        End If
        'If ddlBrands.Items.Count < 1 Then
        '    url = URLParameters.AddParamaterToURL(url, "BrandId", "", False)
        'End If
        If ddlRange.Items.Count < 1 Then
            url = URLParameters.AddParamaterToURL(url, "price", "", False)
        End If
        If ddlTones.Items.Count < 1 Then
            url = URLParameters.AddParamaterToURL(url, "toneid", "", False)
        End If
        Response.Redirect(GetURLEnableViewtextPaging(url))
    End Sub
    Public Sub ddlTones_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlTones.SelectedIndexChanged
        Dim url As String = GetLink("toneid", ddlTones.SelectedValue, False, True)
        If ddlFeatures.Items.Count < 1 Then
            url = URLParameters.AddParamaterToURL(url, "featureid", "", False)
        End If
        If ddlCollections.Items.Count < 1 Then
            url = URLParameters.AddParamaterToURL(url, "collectionid", "", False)
        End If
        'If ddlBrands.Items.Count < 1 Then
        '    url = URLParameters.AddParamaterToURL(url, "BrandId", "", False)
        'End If
        If ddlRange.Items.Count < 1 Then
            url = URLParameters.AddParamaterToURL(url, "price", "", False)
        End If
        If ddlTones.Items.Count < 1 Then
            url = URLParameters.AddParamaterToURL(url, "shadeid", "", False)
        End If
        Response.Redirect(GetURLEnableViewtextPaging(url))
    End Sub
    Public Sub ddlRange_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlRange.SelectedIndexChanged
        Dim PriceRange As String = ddlRange.SelectedValue
        Dim url As String = GetLink("price", ddlFeatures.SelectedValue, False, True)
        If ddlFeatures.Items.Count < 1 Then
            url = URLParameters.AddParamaterToURL(url, "featureid", "", False)
        End If
        If ddlCollections.Items.Count < 1 Then
            url = URLParameters.AddParamaterToURL(url, "collectionid", "", False)
        End If
        'If ddlBrands.Items.Count < 1 Then
        '    url = URLParameters.AddParamaterToURL(url, "BrandId", "", False)
        'End If
        If ddlShades.Items.Count < 1 Then
            url = URLParameters.AddParamaterToURL(url, "shadeid", "", False)
        End If
        If ddlTones.Items.Count < 1 Then
            url = URLParameters.AddParamaterToURL(url, "toneid", "", False)
        End If
        Response.Redirect(GetURLEnableViewtextPaging(url))
    End Sub
    Public Sub ddlFeatures_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlFeatures.SelectedIndexChanged
        Dim url As String = GetLink("featureid", ddlFeatures.SelectedValue, False, True)
        If ddlCollections.Items.Count < 1 Then
            url = URLParameters.AddParamaterToURL(url, "collectionid", "", False)
        End If
        'If ddlBrands.Items.Count < 1 Then
        '    url = URLParameters.AddParamaterToURL(url, "BrandId", "", False)
        'End If
        If ddlRange.Items.Count < 1 Then
            url = URLParameters.AddParamaterToURL(url, "price", "", False)
        End If
        If ddlShades.Items.Count < 1 Then
            url = URLParameters.AddParamaterToURL(url, "shadeid", "", False)
        End If
        If ddlTones.Items.Count < 1 Then
            url = URLParameters.AddParamaterToURL(url, "toneid", "", False)
        End If
        Response.Redirect(GetURLEnableViewtextPaging(url))
    End Sub
    Public Sub ddlCollections_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCollections.SelectedIndexChanged
        '' Dim CollectionId As Integer = IIf(IsNumeric(ddlCollections.SelectedValue), ddlCollections.SelectedValue, Nothing)
        Dim url As String = GetLink("collectionid", ddlCollections.SelectedValue, False, True)
        If ddlFeatures.Items.Count < 1 Then
            url = URLParameters.AddParamaterToURL(url, "featureid", "", False)
        End If
        'If ddlBrands.Items.Count < 1 Then
        '    url = URLParameters.AddParamaterToURL(url, "BrandId", "", False)
        'End If
        If ddlRange.Items.Count < 1 Then
            url = URLParameters.AddParamaterToURL(url, "price", "", False)
        End If
        If ddlShades.Items.Count < 1 Then
            url = URLParameters.AddParamaterToURL(url, "shadeid", "", False)
        End If
        If ddlTones.Items.Count < 1 Then
            url = URLParameters.AddParamaterToURL(url, "toneid", "", False)
        End If
        Response.Redirect(GetURLEnableViewtextPaging(url))
    End Sub
    Private Function GetURLEnableViewtextPaging(ByVal url As String) As String
        If (Me.Request.RawUrl.Contains("/nail-sales-promotion") Or Me.Request.RawUrl.Contains("/store/category.aspx")) Then
            'Dim brandFilter As String = ddlBrands.SelectedValue
            Dim collectionFilter As String = ddlCollections.SelectedValue
            Dim toneFilter As String = ddlTones.SelectedValue
            Dim shadeFilter As String = ddlShades.SelectedValue
            'If Not (String.IsNullOrEmpty(brandFilter) And String.IsNullOrEmpty(collectionFilter) And String.IsNullOrEmpty(toneFilter) And String.IsNullOrEmpty(shadeFilter)) Then
            If Not (String.IsNullOrEmpty(collectionFilter) And String.IsNullOrEmpty(toneFilter) And String.IsNullOrEmpty(shadeFilter)) Then
                url = url.Replace("&evpg=1", "")
                url = url.Replace("?evpg=1", "")
                Session("disableViewPaging") = "1"
            Else
                Dim disableViewPaging As String = ""
                If Not (Session("disableViewPaging")) Is Nothing Then
                    disableViewPaging = Session("disableViewPaging")
                    If (disableViewPaging = "1") Then
                        url = URLParameters.AddParamaterToURL(url, "evpg", "1", False)
                        Session("disableViewPaging") = "0"
                    End If
                End If
            End If
        End If
        Return url
    End Function
    'Public Sub ddlBrands_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlBrands.SelectedIndexChanged
    '    Dim url As String = ""
    '    If Me.Request.RawUrl.Contains("/nail-brand/") Then
    '        Dim code As String = Request.QueryString("BrandId")
    '        If Not code Is Nothing Then
    '            url = Me.Request.RawUrl.Replace(code, ddlBrands.SelectedValue)
    '        Else
    '            url = Me.Request.RawUrl.Replace("/nail-brand/", "/nail-brand/" & ddlBrands.SelectedValue)

    '        End If
    '        'If ddlFeatures.Items.Count < 1 Then
    '        '    url = URLParameters.AddParamaterToURL(url, "feature", "", False)
    '        'End If
    '        'If ddlCollections.Items.Count < 1 Then
    '        '    url = URLParameters.AddParamaterToURL(url, "collection", "", False)
    '        'End If
    '        'If ddlRange.Items.Count < 1 Then
    '        '    url = URLParameters.AddParamaterToURL(url, "price", "", False)
    '        'End If
    '        'If ddlShades.Items.Count < 1 Then
    '        '    url = URLParameters.AddParamaterToURL(url, "shade", "", False)
    '        'End If
    '        'If ddlTones.Items.Count < 1 Then
    '        '    url = URLParameters.AddParamaterToURL(url, "tone", "", False)
    '        'End If
    '        url = URLParameters.AddParamaterToURL(url, "pg", "", False)
    '    Else
    '        url = GetLink("BrandId", ddlBrands.SelectedValue, False, True)
    '    End If
    '    Response.Redirect(GetURLEnableViewtextPaging(url))
    'End Sub
    Public Sub ddlSortBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlSortBy.SelectedIndexChanged
        Dim url As String = GetLink("sort", ddlSortBy.SelectedValue, True, False)
        'If ddlFeatures.Items.Count < 1 Then
        '    url = URLParameters.AddParamaterToURL(url, "feature", "", False)
        'End If
        'If ddlCollections.Items.Count < 1 Then
        '    url = URLParameters.AddParamaterToURL(url, "collection", "", False)
        'End If
        'If ddlBrands.Items.Count < 1 Then
        '    url = URLParameters.AddParamaterToURL(url, "BrandId", "", False)
        'End If
        'If ddlRange.Items.Count < 1 Then
        '    url = URLParameters.AddParamaterToURL(url, "price", "", False)
        'End If
        'If ddlShades.Items.Count < 1 Then
        '    url = URLParameters.AddParamaterToURL(url, "shade", "", False)
        'End If
        'If ddlTones.Items.Count < 1 Then
        '    url = URLParameters.AddParamaterToURL(url, "tone", "", False)
        'End If
        Response.Redirect(url)
    End Sub

End Class
