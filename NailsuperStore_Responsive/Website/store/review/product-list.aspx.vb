Imports Components
Imports DataLayer
Imports System.Text
Imports System.IO
Imports Utility
Imports System.Globalization
Imports System.Web.Services
Imports System.Collections.Generic
Partial Class store_productreviews

    Inherits SitePage
    Private Shared filter As DepartmentFilterFields
    Private Shared ItemsReviewsCollection As StoreItemReviewCollection
    ''Private m_Item As StoreItemRow
    Protected TotalPages As Integer
    Protected NoRecords As String = "<div style='padding:15px 0px 0px 15px'>There are no reviews for this item.</div>"
    Protected m_ItemId As Integer
    Protected Shared PageSize As Integer = Utility.ConfigData.PageSizeScroll
    Protected Shared pgIndex As Integer = 1
    Protected Shared TotalRecords As Integer = 0
    Public Property ItemId() As Integer
        Get
            Return m_ItemId
        End Get
        Set(ByVal value As Integer)
            m_ItemId = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (GetQueryString("DepartmentId") <> Nothing) Then
            IsIndexed = False
            IsFollowed = False
        End If
        If Not IsPostBack Then
            BindData()
        End If
    End Sub


    Public Sub BindData()
        filter = New DepartmentFilterFields()
        filter.pg = pgIndex
        filter.MaxPerPage = PageSize
        filter.ItemId = m_ItemId
        filter.RatingRange = IIf(GetQueryString("rating") Is Nothing, String.Empty, GetQueryString("rating"))
        filter.DepartmentId = IIf(GetQueryString("DepartmentId") Is Nothing, 0, Convert.ToInt32(GetQueryString("DepartmentId")))
        Dim queryPros As String = IIf(GetQueryString("pros") Is Nothing, String.Empty, GetQueryString("pros"))
        Dim queryCons As String = IIf(GetQueryString("cons") Is Nothing, String.Empty, GetQueryString("cons"))
        Dim queryExp As String = IIf(GetQueryString("exp") Is Nothing, String.Empty, GetQueryString("exp"))
        ' Trinh them lay PageTitle khi chon Category

        Dim ltCanonical As Literal = Me.Master.FindControl("ltrCanonical")
        If (GetQueryString("DepartmentId") <> Nothing) Then
            Dim objMetaTag As New MetaTag
            Dim row As ContentToolPageRow = ContentToolPageRow.GetRowByURL(Request.Path)
            Dim Department As StoreDepartmentRow = StoreDepartmentRow.GetRow(DB, filter.DepartmentId)
            objMetaTag.MetaDescription = row.MetaDescription
            objMetaTag.MetaKeywords = row.MetaKeywords
            objMetaTag.PageTitle = row.Title + " - " + Department.PageTitle
            objMetaTag.Canonical = Utility.ConfigData.GlobalRefererName & URLParameters.ProductReviewUrl(Department.URLCode, Department.DepartmentId)
            SetPageMetaSocialNetwork(Page, objMetaTag)
            'ltCanonical.Text = URLParameters.ProductReviewUrl(Department.URLCode, Department.DepartmentId)
        Else
            ltCanonical.Text = String.Format(Resources.Msg.Canonical, Utility.ConfigData.GlobalRefererName & Request.RawUrl.ToString().Split("?")(0))
        End If


        'Hien them vao
        Dim queryUrlCode As String = IIf(GetQueryString("t") Is Nothing, String.Empty, GetQueryString("t"))
        ItemsReviewsCollection = StoreItemReviewRow.GetListItemReviewsNarrowSearch(filter, ConvertParam("Pros", queryPros), ConvertParam("Cons", queryCons), ConvertParam("exp", queryExp), queryUrlCode)
        If ItemsReviewsCollection.Count > 0 Then
            TotalRecords = ItemsReviewsCollection.TotalRecords
            For i As Integer = 0 To ItemsReviewsCollection.Count - 1
                Dim htmlControl As String = RenderReviewControl(ItemsReviewsCollection(i), i + 1)
                ltrReviewList.Text = ltrReviewList.Text & htmlControl
            Next
            If (ItemsReviewsCollection.Count < ItemsReviewsCollection.TotalRecords) Then
                divMoreReview.Visible = True
            Else
                divMoreReview.Visible = False
            End If
        Else
            lblNoRecords.Text = Resources.Msg.review_no_record
            lblNoRecords.Visible = True
            divMoreReview.Visible = False
        End If
    End Sub
    Private Shared Function ConvertParam(ByVal ParamName As String, ByVal value As String) As String
        Dim result As String = String.Empty
        If (Not String.IsNullOrEmpty(value)) Then
            If ParamName.ToUpper().Equals("EXP") Then
                If (value.ToUpper().Equals("STUDENT")) Then
                    result = "ExperienceLevel" & value.Trim().Replace("-", "")
                Else
                    Dim i As Integer = value.ToUpper().LastIndexOf("-YEARS")
                    If (i > 0) Then
                        result = "ExperienceLevel" & value.Trim().Substring(0, i)
                    End If
                End If
            Else
                result = ParamName.Trim() & value.Trim().Replace("-", "")
            End If
            result = "#" & result & "#=on"
        End If
        Return result
    End Function
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
#Region "Load more review"
    <WebMethod()> _
    Public Shared Function GetMoreData(ByVal pageIndex As Integer, ByVal pageSize As Integer) As String()
        Dim result As String() = New String(2) {}
        Dim xml As String = ""
        Dim isAllowMore As Boolean = False
        filter.pg = pageIndex
        filter.MaxPerPage = pageSize
        Dim queryPros As String = IIf(GetQueryString("pros") Is Nothing, String.Empty, GetQueryString("pros"))
        Dim queryCons As String = IIf(GetQueryString("cons") Is Nothing, String.Empty, GetQueryString("cons"))
        Dim queryExp As String = IIf(GetQueryString("exp") Is Nothing, String.Empty, GetQueryString("exp"))
        Dim queryUrlCode As String = IIf(GetQueryString("t") Is Nothing, String.Empty, GetQueryString("t"))
        ItemsReviewsCollection = New StoreItemReviewCollection
        ItemsReviewsCollection = StoreItemReviewRow.GetListItemReviewsNarrowSearch(filter, ConvertParam("Pros", queryPros), ConvertParam("Cons", queryCons), ConvertParam("exp", queryExp), queryUrlCode)
        If ItemsReviewsCollection.Count > 0 Then
            Dim bg As Integer = (pageSize * pageIndex) - pageSize + 1
            For i As Integer = 0 To ItemsReviewsCollection.Count - 1
                xml &= RenderReviewControl(ItemsReviewsCollection(i), bg + i)
            Next
            TotalRecords = ItemsReviewsCollection.TotalRecords
            If pageIndex * pageSize < TotalRecords Then
                isAllowMore = True
            Else
                isAllowMore = False
            End If
        End If
        result(0) = xml
        result(1) = isAllowMore
        'result(2) = pageIndex + 1
        Return result
    End Function
#End Region
End Class
