Imports Components
Imports DataLayer
Imports Newtonsoft.Json
Partial Class controls_product_product_list
    Inherits BaseControl
    Private m_ItemsCollection As StoreItemCollection
    Private m_isInternational As Boolean
    Private m_XmlData As String
    Private m_ItemsCollectionCount As Integer
    Public ItemIndex As Integer = 0
    Public msg As String = String.Empty
    Public PageSize As Integer = Utility.ConfigData.PageSizeScroll
    'Public Description As String = String.Empty
    Public Property ItemsCollection() As StoreItemCollection
        Get
            Return m_ItemsCollection
        End Get
        Set(ByVal value As StoreItemCollection)
            m_ItemsCollection = value
        End Set
    End Property

    Public Property isInternational() As Boolean
        Get
            Return m_isInternational
        End Get
        Set(ByVal value As Boolean)
            m_isInternational = value
        End Set
    End Property
    Public Property XmlData() As String
        Get
            Return m_XmlData
        End Get
        Set(ByVal value As String)
            m_XmlData = value
        End Set
    End Property
    Public ItemsCollectionCount As Integer
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ItemIndex = StoreItemRow.counter
            If ItemsCollectionCount > 0 Then
                rpListItem.DataSource = ItemsCollection
                rpListItem.DataBind()
                'LoadListData(ItemsCollection)
            Else
                emtyList.Text = Resources.Msg.EmtyList
            End If

        End If

    End Sub
    'Private Sub LoadListData(ByVal sc As StoreItemCollection)
    '    For i As Integer = 0 To sc.Count - 1
    '        Dim controlPath As String = "~/controls/product/product.ascx"
    '        Dim pageHolder As New Page()
    '        Dim ucproduct As New controls_product_product
    '        ucproduct = DirectCast(pageHolder.LoadControl(controlPath), controls_product_product)
    '        ucproduct.IsFirstLoad = True
    '        ucproduct.Fill(i + 1, sc(i), "", 0, isInternational)
    '        phListItem.Controls.Add(ucproduct)
    '    Next
    'End Sub
    Protected Sub rpListItem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rpListItem.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim si As StoreItemRow = e.Item.DataItem
            Dim ucItem As New controls_product_product
            ucItem = CType(e.Item.FindControl("ucItem"), controls_product_product)
            ucItem.IsFirstLoad = True
            ucItem.Fill(e.Item.ItemIndex + 1, si, "", 0, isInternational)
        End If
    End Sub
    Public Function GetXmlData(ByVal filter As DepartmentFilterFields, ByVal isInternational As String, ByVal pgIndex As Integer, ByVal pgSize As Integer, ByVal beginRecord As Integer) As String
        ' ItemIndex = StoreItemRow.counter
        Dim ItemsCollectionCount As Integer = 0
        Dim ItemsCollection As New StoreItemCollection
        GetCollection(filter, ItemsCollection, ItemsCollectionCount)


        Dim controlPath As String = "~/controls/product/product.ascx"
        Dim pageHolder As New Page()
        Dim XmlData As String = ""
        Dim ucproduct As New controls_product_product
        ucproduct = DirectCast(pageHolder.LoadControl(controlPath), controls_product_product)

        If ItemsCollection.Count > 0 Then
            XmlData = StoreItemRow.BindList(ItemsCollection, ItemsCollectionCount, ucproduct, isInternational, False) 'BindList(ItemsCollection, Countdata, ucproduct)
            'ItemsCollection = Nothing
            Dim items As New Item()
            items.ContentData = XmlData
            items.PageIndex = pgIndex
            items.PageSize = pgSize
            items.Beginrecord = beginRecord
            XmlData = JsonConvert.SerializeObject(items)
        End If
        Return XmlData
    End Function
    Private Sub GetCollection(ByVal filter As DepartmentFilterFields, ByRef ItemsCollection As StoreItemCollection, ByRef ItemsCollectionCount As Integer)
        Try
            Dim url As String = HttpContext.Current.Request.RawUrl.ToString()
            If url.Contains("store/default.aspx") Then
                ItemsCollection = GetParam(filter, isInternational)
            End If
            If url.Contains("store/search-result.aspx") Then
                Dim luceneHelper As New LuceneHelper
                'ItemsCollection = luceneHelper.SearchItemInLucene(filter.Keyword, filter.SortBy, filter.SortOrder, filter.pg, filter.MaxPerPage, ItemsCollectionCount, filter.BrandId, filter.PriceRange, filter.RatingRange, True)
            End If
            If url.Contains("store/main.aspx") Then
                Dim TabId As Integer = 0
                Dim DepartmentTab As DepartmentTabRow
                If GetQueryString("TabURLCode") <> Nothing Then
                    DepartmentTab = DepartmentTabRow.GetByURLCode(GetQueryString("TabURLCode"), filter.DepartmentId)
                End If
                TabId = DepartmentTab.DepartmentTabId
                ItemsCollection = StoreItemRow.ListByDepartmentTabId(TabId, filter)
            End If
            If url.Contains("store/category.aspx") Then
                Dim xmlData As String = ""
                If filter.HasPromotion = True And filter.SalesCategoryId = 0 Then
                    ItemsCollection = StoreItemRow.GetListItems(filter, ItemsCollectionCount)
                Else
                    ItemsCollection = StoreItemRow.ListCategoryItem(filter.SalesCategoryId, filter)
                    'ItemsCollectionCount = ItemsCollection.TotalRecords
                End If
            End If
            If url.Contains("store/shop-save") Then
                Dim ShopSaveId As Integer = GetQueryString("ShopSaveId")
                ItemsCollection = StoreItemRow.ListByShopSaveId(ShopSaveId, filter)
            End If
            If url.Contains("store/department-tab.aspx") Then
                Dim tabId As Integer = tabId = GetQueryString("DepartmentTabId")
                ItemsCollection = StoreItemRow.ListByDepartmentTabId(tabId, filter)
            End If
            If ItemsCollectionCount = 0 Then
                ItemsCollectionCount = ItemsCollection.TotalRecords
            End If
        Catch ex As Exception

        End Try

    End Sub
    Private Function GetParam(ByRef filter As DepartmentFilterFields, ByVal isInter As Boolean) As StoreItemCollection
        Dim ItemCollection As New StoreItemCollection
        Dim m_BrandID As Integer = 0
        StoreItemRow.SetMember(filter, isInter)
        If String.IsNullOrEmpty(GetQueryString("brandid")) = False And GetQueryString("brandid") <> "default.aspx" Then
            m_BrandID = GetQueryString("brandid")
        End If
        filter.SortOrder = GetQueryString("SortOrder")
        filter.Feature = GetQueryString("featureid")
        filter.IsOnSale = GetQueryString("F_Sale") <> Nothing
        filter.IsNew = GetQueryString("F_IsNew") <> Nothing
        filter.Sale24Hour = GetQueryString("F_24Hour") <> Nothing
        filter.SaleBuy1Get1 = GetQueryString("F_Buy1Get1") <> Nothing
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
        'total = ItemCollection.TotalRecords
        Return ItemCollection
    End Function
    Public Class Item
        Public Property ContentData() As String
            Get
                Return m_ContentData
            End Get
            Set(ByVal value As String)
                m_ContentData = value
            End Set
        End Property
        Public Property PageIndex() As Integer
            Get
                Return m_PageIndex
            End Get
            Set(ByVal value As Integer)
                m_PageIndex = value
            End Set
        End Property
        Public Property PageSize() As Integer
            Get
                Return m_PageSize
            End Get
            Set(ByVal value As Integer)
                m_PageSize = value
            End Set
        End Property
        Public Property Beginrecord() As Integer
            Get
                Return m_Beginrecord
            End Get
            Set(ByVal value As Integer)
                m_Beginrecord = value
            End Set
        End Property
        Private m_ContentData As String
        Private m_PageSize As Integer = Utility.ConfigData.PageSizeScroll
        Private m_PageIndex As Integer = 0
        Private m_Beginrecord As Integer = 0
    End Class
End Class
