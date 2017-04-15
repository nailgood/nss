Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Collections.Generic
Imports Utility.Common
Imports Newtonsoft.Json

Partial Class Store_Collection
    Inherits SitePage
    Private filter As DepartmentFilterFields
    Protected Department As StoreDepartmentRow
    Public reviewCount As Integer = 0
    Private Shared Property isInternational As Boolean
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadData()
        End If
    End Sub

    Private Sub LoadData()

        Dim DepartmentId As Integer = 0
        Try
            If GetQueryString("DepartmentId") <> Nothing Then
                DepartmentId = CInt(GetQueryString("DepartmentId"))
            End If
        Catch ex As Exception

        End Try

        StoreItemRow.counter = 0
        Department = StoreDepartmentRow.GetRow(DB, DepartmentId)
        Utility.Common.CheckValidURLCode(Department.URLCode)

        If Department.IsQuickOrder = False Or DepartmentId = 0 Then
            Response.Redirect(URLParameters.DepartmentUrl(Department.URLCode, DepartmentId))
        End If

        'Review info
        Dim averageStars As Double = 0
        StoreItemReviewRow.GetReviewData(DB, 0, reviewCount, averageStars, DepartmentId)
        ltrHeaderReviewCount.Text = reviewCount.ToString() & " Review"
        If reviewCount > 1 Then
            ltrHeaderReviewCount.Text &= "s"
        End If

        If (reviewCount < 1) Then
            'ltrHeaderReview.Text = "<img src=""/includes/theme/images/icon-star-empty.png"">"
            ltrHeaderReview.Text = "<i class=""fa fa-star-o fa-1""></i>"
        Else
            'ltrHeaderReview.Text = String.Format("<img src=""/includes/theme/images/star{0}.png"">", IIf(averageStars.ToString().Length > 1, averageStars.ToString().Replace(".", ""), averageStars.ToString() & "0"))
            ltrHeaderReview.Text = SitePage.BindIconStar(averageStars.ToString())
        End If
        ucProductReview.DepartmentId = DepartmentId
        ucProductReview.ItemId = 0

        'Department Info
        If IsDBNull(Department.NameRewriteUrl) = False Then
            If Department.NameRewriteUrl <> "" Then
                litDepTitle.Text = Department.NameRewriteUrl
            Else
                litDepTitle.Text = Department.Name
            End If
        Else
            litDepTitle.Text = Department.Name
        End If

        Dim objMetaTag As New MetaTag
        Dim shareURL As String = GlobalSecureName & URLParameters.DepartmentCollectionUrl(Department.URLCode, DepartmentId)
        Dim isUSCustomer As Boolean = Utility.Common.IsUSCustomer()
        If Department.PageTitle = Nothing Then
            objMetaTag.PageTitle = Department.Name
        Else
            objMetaTag.PageTitle = GetPageTitleByCustomerType(isUSCustomer, Department.PageTitle, Department.OutsideUSPageTitle, Department.Name)
        End If

        objMetaTag.MetaDescription = GetMetaDescriptionByCustomerType(isUSCustomer, Department.MetaDescription, Department.OutsideUSMetaDescription, String.Empty)
        objMetaTag.MetaKeywords = Department.MetaKeywords
        objMetaTag.MetaKeywords = MetaKeywords
        objMetaTag.ShareURL = shareURL
        objMetaTag.Canonical = shareURL
        SetPageMetaSocialNetwork(Page, objMetaTag)

        Dim strDescription As String = String.Empty
        If Not String.IsNullOrEmpty(Department.Description) Then
            strDescription = Department.Description
        Else
            strDescription = Department.MetaDescription
        End If
        ucDescription.SKU = String.Empty
        ucDescription.Description = strDescription
        ucDescription.LoadDescription()

        filter = New DepartmentFilterFields()
        filter.DepartmentId = DepartmentId
        filter.pg = 1
        ucFilter.Filter = filter
        ucListProduct.DepartmentId = DepartmentId
    End Sub

#Region "Load more review"
    <WebMethod()> _
    Public Shared Function GetMoreData(ByVal pageIndex As Integer, ByVal itemId As Integer, ByVal sortField As String, ByVal isSort As Integer) As String()
        Dim reviewControl As New controls_product_review_list
        Dim filter As New DepartmentFilterFields
        If String.IsNullOrEmpty(GetQueryString("DepartmentId")) = False Then
            filter.DepartmentId = GetQueryString("DepartmentId")
            filter.ItemId = 0
        End If
        Dim result As String() = reviewControl.GetMoreData(pageIndex, filter.DepartmentId, itemId, sortField, isSort)
        Return result
    End Function
#End Region
End Class
