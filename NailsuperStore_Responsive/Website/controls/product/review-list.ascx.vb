

Imports DataLayer
Imports Components


Partial Class controls_product_review_list
    Inherits BaseControl
    Private filter As DepartmentFilterFields
    Protected m_ItemId As Integer
    Protected m_DepartmentId As Integer
    Private ItemsReviewsCollection As StoreItemReviewCollection
    Public Property ItemId() As Integer
        Get
            ''Return m_ItemId
            If (m_ItemId > 0) Then
                Return m_ItemId
            End If
            If Not String.IsNullOrEmpty(hidItemId.Value) Then
                Return CInt(hidItemId.Value)
            End If
            Return 0
        End Get
        Set(ByVal value As Integer)
            m_ItemId = value
            hidItemId.Value = value
        End Set
    End Property
    Public Property DepartmentId() As Integer
        Get
            If (m_DepartmentId > 0) Then
                Return m_DepartmentId
            End If
            Return 0
        End Get
        Set(ByVal value As Integer)
            m_DepartmentId = value
        End Set
    End Property
    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            '' InitPager()
            hidPageIndex.Value = 1
            ltrReviewList.Text = String.Empty
            BindData()
        End If
    End Sub

    Private Function GetReviewData(ByVal itemId As Integer, ByVal departmentId As Integer, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal sortField As String) As StoreItemReviewCollection
        Dim filter As DepartmentFilterFields = New DepartmentFilterFields()
        filter.pg = pageIndex
        filter.MaxPerPage = pageSize
        filter.ItemId = itemId
        filter.DepartmentId = departmentId
        filter.SortBy = sortField
        filter.SortOrder = "DESC"
        Dim DB As New Database()
        DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
        Dim result As StoreItemReviewCollection = StoreItemReviewRow.GetItemReviews1(DB, filter)
        DB.Close()
        Return result
    End Function
    Public Function GetMoreData(ByVal currentPageIndex As Integer, ByVal departmentId As Integer, ByVal itemId As Integer, ByVal sortField As String, ByVal isSort As Integer) As String()
        Dim reviewHTML As String = String.Empty
        Dim isAllowMore As Boolean = False
        Dim ItemsReviewsCollection As StoreItemReviewCollection = Nothing
        If isSort = 1 Then
            ItemsReviewsCollection = GetReviewData(itemId, departmentId, 1, currentPageIndex * Utility.ConfigData.ProductReviewPageSize(), sortField)
        Else
            ItemsReviewsCollection = GetReviewData(itemId, departmentId, currentPageIndex + 1, Utility.ConfigData.ProductReviewPageSize(), sortField)
        End If

        If ItemsReviewsCollection.Count > 0 Then
            Dim currentCount As Integer = 0
            If isSort <> 1 Then
                currentCount = currentPageIndex * Utility.ConfigData.ProductReviewPageSize()
            End If
            For i As Integer = 0 To ItemsReviewsCollection.Count - 1
                reviewHTML = reviewHTML & RenderReviewControl(ItemsReviewsCollection(i), i + currentCount)
            Next
            currentCount = currentCount + ItemsReviewsCollection.Count
            If (currentCount < ItemsReviewsCollection.TotalRecords) Then
                isAllowMore = True
            End If
        End If
        Dim result As String() = New String(3) {}
        result(0) = reviewHTML
        result(1) = isAllowMore
        If isSort = 1 Then
            result(2) = currentPageIndex
        Else
            result(2) = currentPageIndex + 1 'current page index'
        End If
        Return result
    End Function

    Public Sub BindData()
        Dim averageStars As Double = 0
        Dim reviewCount As Integer
        If DepartmentId > 0 Then
            countReview.Visible = False
        End If
        StoreItemReviewRow.GetReviewData(DB, ItemId, reviewCount, averageStars, DepartmentId)
        ltrReviewCount.Text = reviewCount.ToString() & " Review"
        If reviewCount = 0 Or reviewCount > 1 Then
            ltrReviewCount.Text &= "s"
        End If
        If (reviewCount < 1) Then
            'ltrReview.Text = "<img src=""" & Utility.ConfigData.CDNMediaPath & "/includes/theme/images/icon-star-empty.png"">"
            ltrReview.Text = "<i class=""fa fa-star-o fa-1""></i>"
        Else
            'ltrReview.Text = String.Format("<img src=""" & Utility.ConfigData.CDNMediaPath & "/includes/theme/images/star{0}.png"">", IIf(averageStars.ToString().Length > 1, averageStars.ToString().Replace(".", ""), averageStars.ToString() & "0"))
            ltrReview.Text = SitePage.BindIconStar(averageStars.ToString())
        End If

        divMoreReview.Visible = False
        ItemsReviewsCollection = GetReviewData(ItemId, DepartmentId, hidPageIndex.Value, Utility.ConfigData.ProductReviewPageSize(), drlSort.SelectedValue)
        If ItemsReviewsCollection.Count > 0 Then
            For i As Integer = 0 To ItemsReviewsCollection.Count - 1
                Dim htmlControl As String = RenderReviewControl(ItemsReviewsCollection(i), i)
                ltrReviewList.Text = ltrReviewList.Text & htmlControl
            Next
            If (ItemsReviewsCollection.Count < ItemsReviewsCollection.TotalRecords) Then
                divMoreReview.Visible = True
            End If
        Else
            divSort.Visible = False
        End If

    End Sub

    Public Shared Function RenderReviewControl(ByVal si As StoreItemReviewRow, ByVal itemIndex As Integer) As String
        Dim controlPath As String = "~/controls/product/review.ascx"
        Dim pageHolder As New Page()
        Dim viewControl As controls_product_review = DirectCast(pageHolder.LoadControl(controlPath), controls_product_review)
        viewControl.itemIndex = itemIndex
        viewControl.item = si
        pageHolder.Controls.Add(viewControl)
        Dim output As New System.IO.StringWriter()
        HttpContext.Current.Server.Execute(pageHolder, output, False)
        Return output.ToString()
    End Function



End Class
