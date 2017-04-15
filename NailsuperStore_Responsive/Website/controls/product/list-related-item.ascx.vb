


Imports DataLayer
Imports Components
Imports System.Data.SqlClient
Partial Class controls_layout_list_related_item
    Inherits ModuleControl
    Private m_Item As StoreItemRow
    Private m_ItemCollection As StoreItemCollection
    Private m_ItemHas As Hashtable
    Private filter As New DepartmentFilterFields()
    Protected ReWriteURL As RewriteUrl
    Private NoRecords As String = "<div style='padding:20px 0px 0px 20px'>There are no related items for this item.</div>"
    Protected NofRecords As Int16
    Protected NofPages As Integer = 1
    Public errorMessage As String
    Dim Cart As ShoppingCart
    Dim p As SitePage
    Private m_ItemGroup As Integer = 0
    Private m_ItemId As Integer = 0
    Private m_VideoId As Integer = 0

    Private isInternational As Boolean = False

    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property
    Public Property ItemCollection() As StoreItemCollection
        Get
            Return m_ItemCollection
        End Get
        Set(ByVal value As StoreItemCollection)
            m_ItemCollection = value
        End Set
    End Property
    Public Property Item() As StoreItemRow
        Get
            Return m_Item
        End Get
        Set(ByVal value As StoreItemRow)
            m_Item = value
        End Set
    End Property
    Public Property ItemId() As Integer
        Get
            Return m_ItemId
        End Get
        Set(ByVal value As Integer)
            m_ItemId = value
        End Set
    End Property
    Public Property VideoId() As Integer
        Get
            Return m_VideoId
        End Get
        Set(ByVal value As Integer)
            m_VideoId = value
        End Set
    End Property
    Public Property ItemHas() As Hashtable
        Get
            Return ViewState("ItemHas")
        End Get
        Set(ByVal value As Hashtable)
            ViewState("ItemHas") = value
        End Set
    End Property
    Public Property ItemGroup() As Integer
        Get
            Return m_ItemGroup
        End Get
        Set(ByVal value As Integer)
            m_ItemGroup = value
        End Set
    End Property
    Public currentOrderId As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If TypeOf Me.Page Is SitePage Then
            p = CType(Me.Page, SitePage)
            Cart = p.Cart

            'Check order is commit
            If Not Cart Is Nothing AndAlso Not Cart.Order.OrderNo Is Nothing AndAlso Cart.Order.OrderNo.Length > 0 Then

                Session("OrderId") = Nothing
                Cart = Nothing
            End If
        Else
            Cart = Nothing
        End If

        If Not IsPostBack Then
            ItemHas = New Hashtable()
            BindControl()
        End If

    End Sub
    Dim uc As New UserControl
    Public Sub BindControl()
        Dim itemCount As Integer = 0
        Dim ViewAll As Integer = 0
        currentOrderId = Utility.Common.GetOrderIdFromCartCookie()
        filter = New DepartmentFilterFields()
        filter.pg = 1
        filter.MaxPerPage = Integer.MaxValue
        filter.OrderId = IIf(Session("OrderId") <> Nothing, Session("OrderId"), 0)
        filter.MemberId = Utility.Common.GetCurrentMemberId()
        'filter.SortBy = "related.Arrange"
        'filter.SortOrder = "ASC"

        If filter.MemberId > 0 Then
            isInternational = MemberRow.GetRow(Utility.Common.GetCurrentMemberId()).IsInternational
        End If
        Dim RecordCount As Integer = 0
        Dim lstRelated As StoreItemCollection = New StoreItemCollection

        If VideoId > 0 Then
            hidTotal.Value = ItemCollection.Count()
            ucListProduct.ItemsCollection = ItemCollection
            RecordCount = ItemCollection.Count()
        Else
            lstRelated = StoreItemRow.GetRelatedItemsColection(DB, ItemId, ItemGroup, filter, RecordCount)
            hidTotal.Value = lstRelated.Count()
            ucListProduct.ItemsCollection = lstRelated
        End If
        If RecordCount = 0 Then
            Exit Sub
        End If
        ucListProduct.ItemsCollectionCount = RecordCount
        ucListProduct.isInternational = isInternational
        'ucListProduct.PageSize = filter.MaxPerPage
    End Sub
End Class
