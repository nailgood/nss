Imports Components
Imports DataLayer
Imports System.Web.Services
Imports System.Collections.Generic
Imports Utility.Common

Partial Class Store_Category
    Inherits SitePage
    Protected m_DepartmentId As Integer = Nothing
    Private filter As DepartmentFilterFields
    Private SalesCategory As SalesCategoryRow
    Protected Shared ItemsCollectionCount As Integer
    Protected ReWriteURL As RewriteUrl
    Protected promotion As String = ""
    Private Shared m_BrandID As Integer = 0
    Protected Shared Brand As StoreBrandRow
    Private strMetaDescription As String = String.Empty
    Private Shared isInternational As Boolean = False
    Private pagesize As Integer = Utility.ConfigData.PageSizeScroll

    Private Shared Sub getRequestBrandID()
        m_BrandID = 0
        If Not String.IsNullOrEmpty(GetQueryString("brandid")) Then
            m_BrandID = CInt(GetQueryString("brandid"))
        End If
        If m_BrandID > 0 Then
            HttpContext.Current.Session("DepartmentId") = Nothing
        End If
    End Sub
    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
       
        If Not Page.IsPostBack Then
            filter = New DepartmentFilterFields
            filter.MaxPerPage = pagesize
            filter.pg = 1
            filter.All = True
            filter.MaxPerPage = pagesize
            Dim Department As StoreDepartmentRow = StoreDepartmentRow.GetRow(DB, 23)
            Dim ParentDepartment As StoreDepartmentRow = StoreDepartmentRow.GetMainLevelDepartment(DB, 23)
            filter.DepartmentId = 23
            If promotion <> Nothing Then
                getRequestBrandID()
                filter.BrandId = m_BrandID
                filter.Field1Name = "sales"
                filter.SortBy = GetQueryString("sort")
                filter.HasPromotion = promotion <> Nothing
                BindData()
            Else
                SubSalesCategory()
            End If
        End If
    End Sub
    Private Sub SubSalesCategory()
        Dim sCategoryCode As String = ""
        Dim sCategoryID As Integer = 0
        If String.IsNullOrEmpty(GetQueryString("SaleCategoryId")) = False Then
            sCategoryID = CInt(GetQueryString("SaleCategoryId"))
        End If
        SalesCategory = SalesCategoryRow.GetRow(DB, sCategoryID)
        Utility.Common.CheckValidURLCode(SalesCategory.URLCode)
        litDepTitle.Text = SalesCategory.Category
        filter.SalesCategoryId = SalesCategory.SalesCategoryId
        filter.All = False

        filter.HasPromotion = True
        strMetaDescription = SetMetaDescription(strMetaDescription, SalesCategory.Category)
        Page.Title = SalesCategory.Category
        BindData()
        Session("SalseCategoryId") = SalesCategory.SalesCategoryId
        If LCase(GetQueryString("SaleCategoryId")) <> "category.aspx" Then

            If Request.Url.PathAndQuery <> Nothing Then
                Session("LastCategoryUrl") = Request.Url.PathAndQuery.Replace("SaleCategoryId=category", "SaleCategoryId=" & sCategoryID)
            End If
        End If
    End Sub
    Private Sub BindData()
        Dim data As StoreItemCollection
        StoreItemRow.SetMember(filter, isInternational)
        If filter.HasPromotion = True And filter.SalesCategoryId = 0 Then
            filter.CollectionId = IIf(String.IsNullOrEmpty(GetQueryString("collectionid")), 0, GetQueryString("collectionid"))
            filter.ToneId = IIf(String.IsNullOrEmpty(GetQueryString("toneid")), 0, GetQueryString("toneid"))
            filter.ShadeId = IIf(String.IsNullOrEmpty(GetQueryString("shadeid")), 0, GetQueryString("shadeid"))
            data = StoreItemRow.GetListItems(filter, ItemsCollectionCount)
        Else
            data = StoreItemRow.ListCategoryItem(filter.SalesCategoryId, filter)
            ItemsCollectionCount = data.TotalRecords
        End If
        If ItemsCollectionCount = 0 Then
            strMetaDescription = String.Empty
        Else
            LoadControlList(data, isInternational, ItemsCollectionCount, pagesize)
            strMetaDescription = SetMetaDescription(strMetaDescription, data(0).ItemName)
            Page.MetaDescription = strMetaDescription
        End If
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
        phListItem.Controls.Add(ucListProduct)
    End Sub
    <WebMethod()> _
    Public Shared Function GetData(ByVal pageIndex As Integer, ByVal pageSize As Integer) As String
        'Dim result As String() = New String(3) {}
        Dim filter As New DepartmentFilterFields
        Dim isInter As Boolean = False
        filter.MaxPerPage = pageSize
        filter.pg = pageIndex
        StoreItemRow.SetMember(filter, isInter)
        StoreItemRow.counter = ((pageIndex * pageSize) - pageSize)
        filter.CollectionId = IIf(String.IsNullOrEmpty(GetQueryString("collectionid")), 0, GetQueryString("collectionid"))
        filter.ToneId = IIf(String.IsNullOrEmpty(GetQueryString("toneid")), 0, GetQueryString("toneid"))
        filter.ShadeId = IIf(String.IsNullOrEmpty(GetQueryString("shadeid")), 0, GetQueryString("shadeid"))
        filter.All = False
        filter.HasPromotion = True
        Try
            filter.SalesCategoryId = GetQueryString("SaleCategoryId")
        Catch
        End Try
        ' Dim ItemsCollection As New StoreItemCollection
        Dim xmlData As String = ""
        Dim ucproduct As New controls_product_product_list
        ' If ItemsCollection.Count > 0 Then
        ' ucproduct.ItemsCollection = ItemsCollection
        ' ucproduct.isInternational = isInternational
        'ucproduct.ItemsCollectionCount = ItemsCollectionCount
        'ucproduct.PageSize = pageSize
        If pageSize < Utility.ConfigData.PageSizeScroll Then
            pageIndex = 1
            pageSize = Utility.ConfigData.PageSizeScroll + pageSize
        End If
        'ucproduct.PageSize = pageSize
        xmlData = ucproduct.GetXmlData(filter, isInter, pageIndex, pageSize, pageIndex * pageSize - pageSize + 1)
        ' End If
        'result(0) = xmlData
        'result(1) = pageIndex
        'result(2) = pageIndex * pageSize - pageSize + 1
        Return xmlData
    End Function

End Class
