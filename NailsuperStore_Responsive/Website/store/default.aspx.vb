Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Collections.Generic
Imports Utility.Common
Partial Class Store_Default
    Inherits SitePage
    Private Shared m_DepartmentID As Integer
    Private Shared m_BrandID As Integer
    Private iPolish As Integer = 99
    Private iQuickOrder As Integer = 99
    Private iNormal As Integer = 99
    Protected Shared Brand As StoreBrandRow
    Protected Department As StoreDepartmentRow
    Protected ParentDepartment As StoreDepartmentRow
    Protected ItemsCollectionCount As Integer = 0
    Private ItemsCollection As StoreItemCollection
    Private Const MaxFeaturedPage As Integer = 12
    Private Const MaxPolishPage As Integer = 24
    Private filter As DepartmentFilterFields
    Private Polish, PediSpas As StoreDepartmentRow
    Protected ItemType As ItemDisplayType
    Private Shared IsSalesDepartment As Boolean = False
    Private strMetaDescription As String = String.Empty
    Private strPageTitle As String = String.Empty
    Dim uc As New UserControl
    Protected IsQuickOrder As Boolean
    Private PageSize As Integer = Utility.ConfigData.PageSizeScroll
    Private Shared Property isInternational As Boolean
    Protected Shared list As String = ""
    Protected Shared DepTitle As String = String.Empty
    Protected pageId As String = String.Empty
    Protected countdr As Integer = 0
    Public Enum ItemDisplayType
        Normal
        Polish
        Pedicure
        Featured
        Promotion
        QuickOrder
    End Enum
    Public Property Url() As String
        Get
            Return ViewState("Url")
        End Get
        Set(ByVal value As String)
            ViewState("Url") = value
        End Set
    End Property
    Public Property listProduct() As String
        Get
            Return ViewState("listProduct")
        End Get
        Set(ByVal value As String)
            ViewState("listProduct") = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            list = ""

            Dim kw = GetQueryString("kw")
            If Not String.IsNullOrEmpty(kw) Then
                Dim redirectURL As String = "/store/searchresult.aspx?kw=" & kw.Replace("-", " ")
                Utility.Common.Redirect301(redirectURL)
                Exit Sub
            End If

            Dim brand = GetQueryString("brand")
            If Not String.IsNullOrEmpty(brand) Then
                Dim redirectURL As String = "/deals-center"
                Utility.Common.Redirect301(redirectURL)
                Exit Sub
            End If

            Dim brandname = GetQueryString("brandname")
            If Not String.IsNullOrEmpty(brandname) Then
                Dim redirectURL As String = "/deals-center"
                Utility.Common.Redirect301(redirectURL)
                Exit Sub
            End If

            Dim departmentCode As String = GetQueryString("DepartmentURLCode")
            If Not String.IsNullOrEmpty(departmentCode) Then
                If departmentCode = "nail-table-accessories" Then
                    Dim redirectURL As String = Me.Request.RawUrl.Replace("nail-table-accessories", "manicure-table-accessories")
                    Utility.Common.Redirect301(redirectURL)
                Else
                    Response.Redirect("/301.aspx?DepartmentURLCode=" & departmentCode)
                End If

                Exit Sub
            End If

            Dim departmentId = GetQueryString("departmentId")
            If Not String.IsNullOrEmpty(departmentId) Then
                Response.Redirect("/301.aspx?departmentId=" & departmentId)
                Exit Sub
            End If

            Dim brandId = GetQueryString("brandid")
            If Not String.IsNullOrEmpty(brandId) Then
                Response.Redirect("/301.aspx?brandid=" & brandId)
                Exit Sub
            End If

            'Email.SendError("ToError500", "DEFAULT URL", "Url=" & Request.Url.ToString() & "<br>rawURL:" & Request.RawUrl)
            Utility.Common.Redirect301("/deals-center")
            'LoadData()
        End If

    End Sub
    Private Sub getRequestDepartmentID()
        m_DepartmentID = 0
        If GetQueryString("DepartmentId") <> Nothing Then
            m_DepartmentID = GetQueryString("DepartmentId")
        Else
            Session.Remove("DepartmentId")
        End If

        If m_DepartmentID > 0 Then
            Session("DepartmentId") = m_DepartmentID
        End If
    End Sub
    Private Shared Sub getRequestBrandID()
        If Not GetQueryString("F_BrandId") Is Nothing Then
            HttpContext.Current.Response.Redirect("/301.aspx?brandid=" & GetQueryString("F_BrandId"))
        End If
        m_BrandID = 0

        If String.IsNullOrEmpty(GetQueryString("brandid")) = False And GetQueryString("brandid") <> "default.aspx" Then
            m_BrandID = GetQueryString("brandid")
        End If

        If m_BrandID > 0 Then
            HttpContext.Current.Session.Remove("DepartmentId")
            Brand = StoreBrandRow.GetRow(m_BrandID)
        End If
    End Sub

    'Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
    '    PediSpas = StoreDepartmentRow.GetRow(DB, 234)
    '    fltr.PediSpas = PediSpas
    'End Sub

    Private Sub LoadData()
        getRequestDepartmentID()
        getRequestBrandID()
        Dim Keywords As String = IIf(GetQueryString("kw") Is Nothing, String.Empty, GetQueryString("kw"))
        Dim departmentID As Integer = 0
        If m_DepartmentID <= 0 Then
            departmentID = StoreDepartmentRow.GetDefaultDepartment(DB)
        Else
            departmentID = m_DepartmentID
        End If
        Department = StoreDepartmentRow.GetRow(DB, m_DepartmentID)
        If Department.IsQuickOrder Then
            Utility.Common.Redirect301(URLParameters.DepartmentCollectionUrl(Department.URLCode, departmentID))
        End If
        'Check valid URLCode
        'If m_DepartmentID > 0 Or m_BrandID = 0 Then
        '    Utility.Common.CheckValidURLCode(Department.URLCode)
        'Else
        '    Utility.Common.CheckValidURLCode(Brand.URLCode)
        'End If
        ParentDepartment = StoreDepartmentRow.GetMainLevelDepartment(DB, departmentID)
        fltr.Department = Department
        fltr.ParentDepartment = ParentDepartment
        filter = New DepartmentFilterFields()
        filter.DepartmentId = Department.DepartmentId
        filter.pg = IIf(GetQueryString("pg") = String.Empty Or IsNumeric(GetQueryString("pg")) = False, 1, GetQueryString("pg"))
        filter.SortBy = GetQueryString("Sort")
        filter.BrandId = m_BrandID
        Keywords = Utility.Common.CheckSearchKeyword(Keywords)
        Dim isSearchKW As String = GetQueryString("searchkw")
        If isSearchKW = "1" Then
            filter.IsSearchKeyWord = True
            filter.Keyword = Keywords
        Else
            filter.IsSearchKeyWord = False
            filter.Keyword = IIf(Keywords.IndexOf(" ") <> -1, Core.SplitSearchAND(Keywords), Keywords)
        End If
        'Dim promotion As String = IIf(GetQueryString("F_Promotion") <> Nothing, IIf(GetQueryString("F_Promotion") = "Promotion.aspx", "Y", GetQueryString("F_Promotion")), GetQueryString("F_Promotion"))
        'filter.HasPromotion = promotion <> Nothing
        'filter.PromotionId = IIf(GetQueryString("PromotionId") <> Nothing AndAlso IsNumeric(GetQueryString("PromotionId")), GetQueryString("PromotionId"), Nothing)
        'If Request.Path.ToLower = "/store/default.aspx" AndAlso promotion <> Nothing AndAlso (GetQueryString("F_24Hour") <> Nothing OrElse GetQueryString("F_Buy1Get1") <> Nothing OrElse GetQueryString("F_IsNew") <> Nothing) Then
        '    IsSalesDepartment = True
        'End If
        'If m_DepartmentID = Utility.ConfigData.RootDepartmentID AndAlso filter.BrandId <> Nothing Then
        '    'litDepTitle.Text = Brand.BrandName
        '    DepTitle = Brand.BrandName
        'ElseIf Not IsSalesDepartment Then
        '    If m_DepartmentID = Utility.ConfigData.RootDepartmentID Then
        '        'litDepTitle.Text = "All Items"
        '        DepTitle = "All Items"
        '    End If
        'Else
        '    'litDepTitle.Text = IIf(GetQueryString("F_24Hour") <> Nothing, "Today's 24 Hour Sales", IIf(GetQueryString("F_Buy1Get1") <> Nothing, "Bonus Buy Sales", "New Product Sales"))
        '    DepTitle = IIf(GetQueryString("F_24Hour") <> Nothing, "Today's 24 Hour Sales", IIf(GetQueryString("F_Buy1Get1") <> Nothing, "Bonus Buy Sales", "New Product Sales"))
        'End If

        Dim isUSCustomer As Boolean = Utility.Common.IsUSCustomer()
        strPageTitle = Utility.Common.GetPageTitleByCustomerType(isUSCustomer, Department.PageTitle, Department.OutsideUSPageTitle, Department.Name)

        If (Me.Request.RawUrl.Contains("nail-promotions")) Then
            strPageTitle = "On Sale " & strPageTitle
        End If

        strMetaDescription = Utility.Common.GetMetaDescriptionByCustomerType(isUSCustomer, Department.MetaDescription, Department.OutsideUSMetaDescription, String.Empty)
        strMetaDescription = SetMetaDescription(strMetaDescription, Department.Name, IIf(filter.HasPromotion, Resources.Msg.NailPromotion, String.Empty))

        fltr.Filter = filter
        fltr.Visible = filter.PromotionId = Nothing AndAlso (Not filter.IsFeatured OrElse filter.DepartmentId = 23)
        If IsNumeric(GetQueryString("ps")) Then PageSize = CInt(GetQueryString("ps"))

        If PageSize = Nothing OrElse PageSize Mod 4 <> 0 Then
            If m_DepartmentID = ParentDepartment.DepartmentId AndAlso IIf(GetQueryString("F_BrandId") <> Nothing, "Y", GetQueryString("F_All")) <> "Y" AndAlso Not filter.HasPromotion AndAlso filter.PromotionId = Nothing Then
                PageSize = MaxFeaturedPage
            Else
                PageSize = IIf(filter.IsFeatured, MaxFeaturedPage, IIf(m_DepartmentID = 186, MaxPolishPage, PageSize))
            End If
        End If

        If Not IsPostBack Then
            iPolish = PageSize
            iQuickOrder = PageSize
            iNormal = PageSize
        End If
        If GetQueryString("DepartmentId") Is Nothing = False Or GetQueryString("brandid") <> Nothing Or GetQueryString("kw") <> Nothing Then
            Session("LastDepartmentUrl") = Request.RawUrl
        End If
        BindData()
    End Sub
    Private Sub BindData()
        If String.IsNullOrEmpty(Department.NameRewriteUrl) = False Then
            If Not (m_BrandID > 0 And Department.DepartmentId = 23) Then
                'litDepTitle.Text = Department.NameRewriteUrl
                DepTitle = Department.NameRewriteUrl
            Else
                'litDepTitle.Text = Department.Name
                DepTitle = Department.Name
            End If
        Else
            'litDepTitle.Text = Department.Name
            DepTitle = Department.Name
        End If

        If m_DepartmentID = ParentDepartment.DepartmentId AndAlso IIf(GetQueryString("F_BrandId") <> Nothing, "Y", GetQueryString("F_All")) <> "Y" AndAlso Not filter.HasPromotion AndAlso filter.PromotionId = Nothing Then
            ItemType = ItemDisplayType.Featured
            'filter.IsFeatured = True
            filter.MaxPerPage = PageSize 'drpPageSize.SelectedValue 'MaxFeaturedPage
            ''ItemsCollectionCount = 0
            ''ItemsCollection = GetItemNarrowSearch(ItemsCollectionCount, filter)
            ''fltr.ItemCollection = ItemsCollection
        Else
            If filter.PromotionId <> Nothing Then
                ItemType = ItemDisplayType.Promotion
                filter.MaxPerPage = -1
                Dim Promotion As PromotionRow = PromotionRow.GetRow(DB, filter.PromotionId, False)
                filter.GetItems = True
                filter.GetItems = False
            Else
            End If
            'filter.IsFeatured = False
            If ItemType <> ItemDisplayType.Promotion Then filter.MaxPerPage = PageSize
            filter.pg = 1
            filter.MaxPerPage = PageSize
            fltr.FilterCollections = ItemType = ItemDisplayType.Polish
            fltr.FilterTones = ItemType = ItemDisplayType.Polish
            fltr.FilterShades = ItemType = ItemDisplayType.Polish
            If ItemType = ItemDisplayType.QuickOrder Then
                fltr.FilterCollections = ItemType = ItemDisplayType.QuickOrder
                fltr.FilterTones = ItemType = ItemDisplayType.QuickOrder
                fltr.FilterShades = ItemType = ItemDisplayType.QuickOrder
            End If
            fltr.IsFilter = Department.IsFilter
            'filter.MemberId = Utility.Common.GetCurrentMemberId()
            'ItemsCollection = New StoreItemCollection
            'ItemsCollection = GetItemNarrowSearch(ItemsCollectionCount, filter)
            'fltr.ItemCollection = ItemsCollection
            'If ItemsCollection.Count = 1 AndAlso filter.Keyword <> Nothing Then
            '    Dim url As String = URLParameters.ProductUrl(ItemsCollection(0).URLCode, ItemsCollection(0).ItemId)
            '    Session("DepartmentId") = Nothing
            '    Response.Redirect(url)
            'End If
        End If
        ItemsCollection = New StoreItemCollection
        ItemsCollection = GetItemNarrowSearch(ItemsCollectionCount, filter, isInternational)
        fltr.ItemCollection = ItemsCollection

        countdr = fltr.CountDr
        If ItemsCollection.Count = 1 AndAlso filter.Keyword <> Nothing Then
            Dim url As String = URLParameters.ProductUrl(ItemsCollection(0).URLCode, ItemsCollection(0).ItemId)
            Session("DepartmentId") = Nothing
            Response.Redirect(url)
        End If
        ViewState("ItemsCollectionCount") = ItemsCollectionCount
        If ItemsCollection.Count > 0 Then
            LoadControlList(ItemsCollection, isInternational, ItemsCollectionCount, PageSize)
            strMetaDescription = SetMetaDescription(strMetaDescription, ItemsCollection(0).ItemName)
        Else
            If (Not filter.HasPromotion) Then
                strMetaDescription = String.Empty
            End If
        End If
        ''set MetaTag danh cho mang xa hoi khi share
        Dim objMetaTag As New MetaTag
        objMetaTag.MetaKeywords = Department.MetaKeywords
        objMetaTag.PageTitle = strPageTitle
        objMetaTag.MetaDescription = strMetaDescription
        Dim shareURL As String = GlobalSecureName & URLParameters.DepartmentUrl(Department.URLCode, m_DepartmentID)
        objMetaTag.TypeShare = "category"
        objMetaTag.ImageName = Department.LargeImage
        objMetaTag.ImagePath = Utility.ConfigData.DepartmentMainImageFolder
        objMetaTag.ImgHeight = 202
        objMetaTag.ImgWidth = 360
        objMetaTag.ShareDesc = IIf(String.IsNullOrWhiteSpace(Department.Description), strMetaDescription, Department.Description)
        objMetaTag.ShareTitle = strPageTitle
        objMetaTag.ShareURL = shareURL
        SetPageMetaSocialNetwork(Page, objMetaTag)
    End Sub
    Private Sub LoadControlList(ByVal sc As StoreItemCollection, ByVal isInt As Boolean, ByVal Count As Integer, ByVal pagesize As Integer)
        Dim controlPath As String = "~/controls/product/product-list.ascx"
        Dim pageHolder As New Page()
        Dim ucListProduct As New controls_product_product_list
        ucListProduct = DirectCast(pageHolder.LoadControl(controlPath), controls_product_product_list)
        ucListProduct.ItemsCollection = sc
        ucListProduct.isInternational = isInt
        ucListProduct.ItemsCollectionCount = Count
        ucListProduct.PageSize = pagesize
        'ucListProduct.Description = Department.Description
        phListItem.Controls.Add(ucListProduct)
    End Sub
    Private Function GetItemNarrowSearch(ByRef total As Integer, ByRef filter As DepartmentFilterFields, ByVal isInter As Boolean) As StoreItemCollection
        Dim ItemCollection As New StoreItemCollection
        StoreItemRow.SetMember(filter, isInter)
        filter.SortOrder = GetQueryString("SortOrder")
        filter.Feature = GetQueryString("featureid")
        filter.IsOnSale = GetQueryString("F_Sale") <> Nothing
        filter.IsNew = GetQueryString("F_IsNew") <> Nothing
        filter.Sale24Hour = GetQueryString("F_24Hour") <> Nothing
        filter.SaleBuy1Get1 = GetQueryString("F_Buy1Get1") <> Nothing
        Dim promotion As String = IIf(GetQueryString("F_Promotion") <> Nothing, IIf(GetQueryString("F_Promotion") = "Promotion.aspx", "Y", GetQueryString("F_Promotion")), GetQueryString("F_Promotion"))
        filter.HasPromotion = promotion <> Nothing
        filter.PromotionId = IIf(GetQueryString("PromotionId") <> Nothing AndAlso IsNumeric(GetQueryString("PromotionId")), GetQueryString("PromotionId"), Nothing)
        If HttpContext.Current.Request.Path.ToLower = "/store/default.aspx" AndAlso promotion <> Nothing AndAlso (GetQueryString("F_24Hour") <> Nothing OrElse GetQueryString("F_Buy1Get1") <> Nothing OrElse GetQueryString("F_IsNew") <> Nothing) Then
            IsSalesDepartment = True
        End If
        If (m_DepartmentID = Utility.ConfigData.RootDepartmentID Or m_DepartmentID = 0) AndAlso filter.BrandId <> Nothing Then
            'litDepTitle.Text = Brand.BrandName
            DepTitle = Brand.BrandName
        ElseIf Not IsSalesDepartment Then
            If m_DepartmentID = Utility.ConfigData.RootDepartmentID Then
                'litDepTitle.Text = "All Items"
                DepTitle = "All Items"
            End If
        Else
            'litDepTitle.Text = IIf(GetQueryString("F_24Hour") <> Nothing, "Today's 24 Hour Sales", IIf(GetQueryString("F_Buy1Get1") <> Nothing, "Bonus Buy Sales", "New Product Sales"))
            DepTitle = IIf(GetQueryString("F_24Hour") <> Nothing, "Today's 24 Hour Sales", IIf(GetQueryString("F_Buy1Get1") <> Nothing, "Bonus Buy Sales", "New Product Sales"))
        End If
        If (Not GetQueryString("sort") Is Nothing) Then

            filter.SortBy = GetQueryString("sort")
        Else
            filter.SortBy = Nothing
        End If
        If (Not GetQueryString("price") Is Nothing) Then
            filter.PriceRange = GetQueryString("price")
        Else
            filter.PriceRange = Nothing
        End If
        If (Not GetQueryString("pricehigh") Is Nothing) Then
            filter.PriceHigh = GetQueryString("pricehigh")
        Else
            filter.PriceHigh = Nothing
        End If
        If (Not GetQueryString("rating") Is Nothing) Then
            filter.RatingRange = GetQueryString("rating")
        Else
            filter.RatingRange = Nothing
        End If
        If (Not GetQueryString("brandid") Is Nothing) Then
            filter.BrandId = GetQueryString("brandid")
        Else
            filter.BrandId = 0
        End If
        If (Not GetQueryString("collectionid") Is Nothing) Then
            filter.CollectionId = GetQueryString("collectionid")
        Else
            filter.CollectionId = 0
        End If
        If (Not GetQueryString("toneid") Is Nothing) Then
            filter.ToneId = GetQueryString("toneid")
        Else
            filter.ToneId = 0
        End If
        If (Not GetQueryString("shadeid") Is Nothing) Then
            filter.ShadeId = GetQueryString("shadeid")
        Else
            filter.ShadeId = 0
        End If
        Dim si As New StoreItemRow
        ItemCollection = si.GetListDefaultItem(filter) 'StoreItemRow.GetActiveItemsNarrowSearch(DB, filter, total)
        total = ItemCollection.TotalRecords
        Return ItemCollection
    End Function
    <WebMethod()> _
    Public Shared Function GetData(ByVal pageIndex As Integer, ByVal pageSize As Integer) As String
        ' Dim result As String() = New String(3) {}
        'System.Threading.Thread.Sleep(2000)
        Dim isInter As Boolean = False
        Dim ItemsCollecCount As Integer = 0
        Dim filter As New DepartmentFilterFields
        Dim pg As Integer = filter.pg
        getRequestBrandID()
        StoreItemRow.counter = ((pageIndex * pageSize) - pageSize)
        filter.MaxPerPage = pageSize
        filter.pg = pageIndex
        filter.BrandId = m_BrandID

        If String.IsNullOrEmpty(GetQueryString("DepartmentId")) = False Then
            filter.DepartmentId = GetQueryString("DepartmentId")
        End If
        'Dim ItemsCollection As New StoreItemCollection
        Dim ucproduct As New controls_product_product_list
        'ItemsCollection = GetItemNarrowSearch(ItemsCollecCount, filter, isInter)
        'If ItemsCollection.Count > 0 Then
        'ucproduct.ItemsCollection = ItemsCollection
        'ucproduct.isInternational = isInternational
        'ucproduct.ItemsCollectionCount = ItemsCollectionCount
        If pageSize < Utility.ConfigData.PageSizeScroll Then
            pageIndex = 1
            pageSize = Utility.ConfigData.PageSizeScroll + pageSize
        End If
        'ucproduct.PageSize = pageSize
        Dim XmlData As String = ucproduct.GetXmlData(filter, isInter, pageIndex, pageSize, pageIndex * pageSize - pageSize + 1)
        'End If
        'result(0) = XmlData
        'result(1) = pageIndex
        'result(2) = pageIndex * pageSize - pageSize + 1
        'result(3) = pageSize
        'result(3) = filter.DepartmentId
        Return XmlData
    End Function
End Class
