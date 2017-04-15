Imports Components
Imports DataLayer
Partial Class controls_product_product_list_collection
    Inherits BaseControl
    Private Shared ItemsCollection As StoreItemCollection
    Private Shared isInternational As Boolean
    Private Shared m_XmlData As String
    Protected Shared ItemsCollectionCount As Integer = 0
    Private Shared m_DepartmentId As Integer
    Private Shared m_ItemId As Integer
    Private Shared m_PageSize As Integer = Utility.ConfigData.PageSizeCollection
    Public filter As DepartmentFilterFields
    Private dbMember As New MemberRow
    Public Shared ItemIndex As Integer = 0
   
    Public Shared Property XmlData() As String
        Get
            Return m_XmlData
        End Get
        Set(ByVal value As String)
            m_XmlData = value
        End Set
    End Property
    Public Shared PageSize As Integer
    Public Shared Property DepartmentId() As Integer
        Get
            Return m_DepartmentId
        End Get
        Set(ByVal value As Integer)
            m_DepartmentId = value
        End Set
    End Property
    Public Shared Property ItemId() As Integer
        Get
            Return m_ItemId
        End Get
        Set(ByVal value As Integer)
            m_ItemId = value
        End Set
    End Property
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            PageSize = m_PageSize
            LoadData()
        End If
    End Sub
    Private Sub LoadData()
        Try
            If Not String.IsNullOrEmpty(GetQueryString("DepartmentId")) And m_DepartmentId = 0 Then
                m_DepartmentId = CInt(GetQueryString("DepartmentId"))
            End If
        Catch ex As Exception

        End Try
        If (m_ItemId > 0) Then
            m_DepartmentId = 0
        End If
        'StoreItemRow.counter = 0
        filter = New DepartmentFilterFields()
        filter.DepartmentId = m_DepartmentId
        filter.ItemId = m_ItemId
        filter.pg = 1
        filter.MaxPerPage = PageSize
        StoreItemRow.SetMember(filter, isInternational)
        filter.MaxPerPage = m_PageSize
        filter.pg = 1
        ItemsCollection = GetItemNarrowSearch(ItemsCollectionCount, filter)
        fltr.Filter = filter
        ItemIndex = StoreItemRow.counter
        If ItemsCollectionCount > 0 Then
            rpListItem.DataSource = ItemsCollection
            rpListItem.DataBind()
            'LoadListData(ItemsCollection)
        Else
            emtyList.Text = Resources.Msg.EmtyList
        End If
    End Sub
    Private Shared Function GetItemNarrowSearch(ByRef total As Integer, ByRef filter As DepartmentFilterFields) As StoreItemCollection
        Dim ItemCollection As New StoreItemCollection
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
        ItemCollection = New StoreItemCollection
        ItemCollection = StoreItemRow.GetListColection(filter, total)
        Return ItemCollection
    End Function
    'Private Sub LoadListData(ByVal sc As StoreItemCollection)
    '    For i As Integer = 0 To sc.Count - 1
    '        Dim controlPath As String = "~/controls/product/product-collection.ascx"
    '        Dim pageHolder As New Page()
    '        Dim ucproduct As New controls_product_product_collection
    '        ucproduct = DirectCast(pageHolder.LoadControl(controlPath), controls_product_product_collection)
    '        ucproduct.IsFirstLoad = True
    '        ucproduct.Fill(i + 1, sc(i), "", 0, isInternational)
    '        phListItem.Controls.Add(ucproduct)
    '    Next

    'End Sub
    Protected Sub rpListItem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rpListItem.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim si As StoreItemRow = e.Item.DataItem
            Dim ucItem As New controls_product_product_collection
            ucItem = CType(e.Item.FindControl("ucItem"), controls_product_product_collection)
            ucItem.IsFirstLoad = True
            ucItem.Fill(e.Item.ItemIndex + 1, si, "", 0, isInternational)
        End If
    End Sub
    Public Shared Function GetXmlData(ByVal pgIndex As Integer) As String
        'ItemIndex = StoreItemRow.counter
        Dim filter As New DepartmentFilterFields
        Dim controlPath As String = "~/controls/product/product-collection.ascx"
        Dim pageHolder As New Page()
        Dim ucproduct As New controls_product_product_collection
        ucproduct = DirectCast(pageHolder.LoadControl(controlPath), controls_product_product_collection)
        filter.MaxPerPage = PageSize
        filter.pg = pgIndex
        StoreItemRow.counter = ((pgIndex * PageSize) - PageSize)
        Try
            filter.DepartmentId = GetQueryString("DepartmentId")
        Catch
        End Try
        Try
            filter.ItemId = GetQueryString("ItemId")
        Catch
        End Try
        StoreItemRow.SetMember(filter, isInternational)
        ItemsCollection = New StoreItemCollection
        ItemsCollection = GetItemNarrowSearch(ItemsCollectionCount, filter)
        If ItemsCollection.Count > 0 Then
            XmlData = StoreItemRow.BindList(ItemsCollection, ItemsCollectionCount, ucproduct, isInternational, True) 'BindList(ItemsCollection, Countdata, ucproduct)
        End If
        Return XmlData
    End Function
End Class
