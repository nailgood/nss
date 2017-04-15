Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Collections.Generic
Imports Newtonsoft.Json
Imports Utility.Common
Imports Humanizer

Partial Class Store_SubCategory
    Inherits SitePage
    Private m_DepartmentID As Integer = 0
    Private m_BrandID As Integer = 0
    Protected Brand As StoreBrandRow
    Protected ItemsCollectionCount As Integer = 0
    Private ItemsCollection As StoreItemCollection
    Private filter As DepartmentFilterFields
    Protected ItemType As ItemDisplayType
    Private strMetaDescription As String = String.Empty
    Private strPageTitle As String = String.Empty
    Protected DepTitle As String = String.Empty
    Public description As String
    Public Enum ItemDisplayType
        Normal
        Polish
        Pedicure
        Featured
        Promotion
        QuickOrder
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            LoadData()
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

    Private Sub getRequestBrandID()
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

    Private Sub LoadData()
        getRequestDepartmentID()
        getRequestBrandID()
        Dim Department As StoreDepartmentRow = StoreDepartmentRow.GetRow(DB, m_DepartmentID)
        If Department.IsQuickOrder Then
            Utility.Common.Redirect301(URLParameters.DepartmentCollectionUrl(Department.URLCode, m_DepartmentID))
        End If
        'Check valid URLCode
        If m_DepartmentID > 0 Then
            Utility.Common.CheckValidURLCode(Department.URLCode)
        End If
        'Check link cannonical
        Dim strLink As String = URLParameters.DepartmentUrl(Department.URLCode, Department.DepartmentId)
        If Not Request.RawUrl.Contains(strLink) Then
            Utility.Common.Redirect301(strLink)
            Exit Sub
        End If
        Dim ParentDepartment As StoreDepartmentRow = StoreDepartmentRow.GetMainLevelDepartment(DB, m_DepartmentID)

        'ParentId la chinh no >> redirect sang maibn category
        If ParentDepartment Is Nothing OrElse ParentDepartment.DepartmentId = 0 Then
            Utility.Common.Redirect301(URLParameters.MainDepartmentUrl(Department.URLCode, m_DepartmentID))
            Exit Sub
        ElseIf m_DepartmentID = 23 Then 'Store Home
            Utility.Common.Redirect301("/deals-center")
            Exit Sub
        End If
        ucFilter.Department = Department
        ucFilter.ParentDepartment = ParentDepartment
        filter = New DepartmentFilterFields()
        filter.DepartmentId = Department.DepartmentId
        filter.pg = IIf(GetQueryString("pg") = String.Empty Or IsNumeric(GetQueryString("pg")) = False, 1, GetQueryString("pg"))
        filter.SortBy = GetQueryString("Sort")
        filter.BrandId = m_BrandID
        Dim isUSCustomer As Boolean = Utility.Common.IsUSCustomer()
        If Department.PageTitle = Nothing Then
            strPageTitle = Department.Name
        Else
            strPageTitle = GetPageTitleByCustomerType(isUSCustomer, Department.PageTitle, Department.OutsideUSPageTitle, Department.Name)
        End If
        strMetaDescription = Utility.Common.GetMetaDescriptionByCustomerType(isUSCustomer, Department.MetaDescription, Department.OutsideUSMetaDescription, String.Empty)
        ucFilter.Filter = filter
        ucFilter.Visible = filter.PromotionId = Nothing AndAlso (Not filter.IsFeatured OrElse filter.DepartmentId = 23)

        If GetQueryString("DepartmentId") Is Nothing = False Or GetQueryString("brandid") <> Nothing Or GetQueryString("kw") <> Nothing Then
            Session("LastDepartmentUrl") = Request.RawUrl
        End If
        If Not String.IsNullOrEmpty(Department.NameRewriteUrl) Then
            If Not (m_BrandID > 0 And Department.DepartmentId = 23) Then
                DepTitle = Department.NameRewriteUrl
            Else
                DepTitle = Department.Name
            End If
        Else
            DepTitle = Department.Name
        End If

        If m_DepartmentID = ParentDepartment.DepartmentId AndAlso IIf(GetQueryString("F_BrandId") <> Nothing, "Y", GetQueryString("F_All")) <> "Y" AndAlso Not filter.HasPromotion AndAlso filter.PromotionId = Nothing Then
            ItemType = ItemDisplayType.Featured
            filter.MaxPerPage = 12
        Else
            If filter.PromotionId <> Nothing Then
                ItemType = ItemDisplayType.Promotion
                filter.MaxPerPage = -1
                Dim Promotion As PromotionRow = PromotionRow.GetRow(DB, filter.PromotionId, False)
                filter.GetItems = True
                filter.GetItems = False
            End If

            filter.pg = 1
            filter.MaxPerPage = 12
            ucFilter.FilterCollections = ItemType = ItemDisplayType.Polish
            ucFilter.FilterTones = ItemType = ItemDisplayType.Polish
            ucFilter.FilterShades = ItemType = ItemDisplayType.Polish
            ucFilter.IsFilter = Department.IsFilter
        End If
        Dim queryString As String = "0"
        If Request.RawUrl.Contains("?") Then
            queryString = Request.RawUrl.Substring(Request.RawUrl.LastIndexOf("?") + 1)
        End If
        'litParam.Text = String.Format("pageIndex:{0}, pageSize:{1}, departmentId:{2}, pagesizetruth:0, listheight:0, queryString:""{3}""", filter.pg, filter.MaxPerPage, filter.DepartmentId, queryString.Replace(",", "|"))
        'Dim shortdesc As String = TruncateExtensions.Truncate(Department.Description, 170, "", Truncator.FixedNumberOfWords).Trim()
        'description = IIf(Not String.IsNullOrEmpty(Department.Description) AndAlso Not String.IsNullOrEmpty(Department.Description.Replace(shortdesc, "")), shortdesc & "<span class='morecontent'>" & Department.Description.Substring(shortdesc.Length).Replace("</p>", "") & "</span> <span class='moreellipses'>...<img src='/includes/theme/images/plus.png'><span></span>&nbsp;</span>", shortdesc)
        description = BBCodeHelper.ConvertBBCodeToHTML(Department.Description)
        'Check QueryString Valid

        If Not Request.RawUrl.Contains("/" & m_DepartmentID) Then
            Email.SendError("ToError500", "Sub Category wrong querystring", Request.RawUrl & "<br>" & Request.Url.ToString() & "<br>m_DepartmentID: " & m_DepartmentID.ToString() & "<br>QueryString: " & GetQueryString("DepartmentId") & GetSessionList())
        End If

        'Set MetaTag danh cho mang xa hoi khi share
        Dim shareURL As String = GlobalSecureName & URLParameters.DepartmentUrl(Department.URLCode, m_DepartmentID)
        Dim objMetaTag As New MetaTag
        objMetaTag.MetaKeywords = Department.MetaKeywords
        objMetaTag.PageTitle = strPageTitle
        objMetaTag.MetaDescription = strMetaDescription
        objMetaTag.TypeShare = "category"
        objMetaTag.ImageName = Department.LargeImage
        objMetaTag.ImagePath = Utility.ConfigData.DepartmentMainImageFolder
        objMetaTag.ImgHeight = 202
        objMetaTag.ImgWidth = 360
        objMetaTag.ShareDesc = IIf(String.IsNullOrWhiteSpace(Department.Description), strMetaDescription, Department.Description)
        objMetaTag.ShareTitle = strPageTitle
        objMetaTag.ShareURL = shareURL
        objMetaTag.Canonical = shareURL
        SetPageMetaSocialNetwork(Page, objMetaTag)

        ucListProduct.DepartmentId = m_DepartmentID
    End Sub



End Class
