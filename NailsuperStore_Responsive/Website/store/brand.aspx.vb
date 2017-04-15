Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Collections.Generic
Imports Newtonsoft.Json
Imports Utility.Common
Imports Humanizer

Partial Class Store_Brand
    Inherits SitePage

    Protected BrandTitle As String = String.Empty
    Private Shared BrandID As Integer
    Private filter As DepartmentFilterFields
    Private strMetaDescription As String = String.Empty
    Public description As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            LoadData()
        End If

    End Sub

    Private Sub getRequestBrandID()
        BrandID = 0
        If GetQueryString("BrandID") <> Nothing Then
            BrandID = GetQueryString("BrandID")
        End If
    End Sub

    Private Sub LoadData()
        getRequestBrandID()

        Dim brand As StoreBrandRow = StoreBrandRow.GetRow(BrandID)
        If brand IsNot Nothing And brand.BrandId > 0 Then
            Utility.Common.CheckValidURLCode(brand.URLCode)
            description = brand.Description
            BrandTitle = brand.BrandName
            strMetaDescription = brand.Description
            strMetaDescription = SetMetaDescription(strMetaDescription, brand.BrandName, "")

            filter = New DepartmentFilterFields()
            filter.SortBy = GetQueryString("Sort")
            filter.BrandId = BrandID
            filter.pg = 1
            filter.MaxPerPage = 12

            'Set MetaTag danh cho mang xa hoi khi share
            Dim shareURL As String = GlobalSecureName & URLParameters.BrandUrl(brand.URLCode, BrandID)
            Dim objMetaTag As New MetaTag
            'objMetaTag.MetaKeywords = Department.MetaKeywords
            objMetaTag.PageTitle = BrandTitle
            objMetaTag.MetaDescription = strMetaDescription
            objMetaTag.TypeShare = "category"
            'objMetaTag.ImageName = Department.LargeImage
            objMetaTag.ImagePath = Utility.ConfigData.DepartmentMainImageFolder
            objMetaTag.ImgHeight = 202
            objMetaTag.ImgWidth = 360
            objMetaTag.ShareDesc = strMetaDescription ' IIf(String.IsNullOrWhiteSpace(Department.Description), strMetaDescription, Department.Description)
            objMetaTag.ShareTitle = BrandTitle
            objMetaTag.ShareURL = shareURL
            objMetaTag.Canonical = shareURL
            SetPageMetaSocialNetwork(Page, objMetaTag)
        Else
            Utility.Common.Redirect301("/deals-center")
        End If


        Dim queryString As String = "0"
        If Request.RawUrl.Contains("?") Then
            queryString = Request.RawUrl.Substring(Request.RawUrl.LastIndexOf("?") + 1)
        End If

        ucListProduct.BrandId = BrandID
    End Sub



End Class
