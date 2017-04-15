Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Collections.Generic
Imports Utility.Common
Partial Class store_department_tab
    Inherits SitePage
    


    Protected DepartmentId As Integer = Integer.MinValue
    Protected DepartmentTabUrlCode As String = String.Empty
    Protected Description As String = String.Empty
    Private Department As StoreDepartmentRow
    Private DepartmentTab As DepartmentTabRow

    Private strMetaDescription As String = String.Empty
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            If Not String.IsNullOrEmpty(GetQueryString("DepartmentId")) Then
                DepartmentId = CInt(GetQueryString("DepartmentId"))
            End If

            If Not String.IsNullOrEmpty(GetQueryString("TabURLCode")) Then
                DepartmentTabUrlCode = GetQueryString("TabURLCode")
            End If

            If String.IsNullOrEmpty(DepartmentTabUrlCode) Or DepartmentId < 0 Then
                Response.Redirect("/")
            End If

            LoadTab()
        End If
    End Sub

    Private Sub LoadTab()
        Department = StoreDepartmentRow.GetRow(DB, DepartmentId)
        Utility.Common.CheckValidURLCode(Department.URLCode)

        Dim TabId As Integer = 0
        DepartmentTab = DepartmentTabRow.GetByURLCode(DB, DepartmentTabUrlCode, Department.DepartmentId)
        TabId = DepartmentTab.DepartmentTabId
        If TabId < 1 Then ''URL Code Tab is not valid
            Dim url As String = URLParameters.MainDepartmentUrl(Department.URLCode, Department.DepartmentId)
            Utility.Common.Redirect301(url)
        End If

        ltrDepartmentName.Text = DepartmentTab.Name
        Description = DepartmentTab.Description
        'Meta tags
        Dim objMetaTag As New MetaTag
        objMetaTag.PageTitle = DepartmentTab.Name & " - " & Utility.Common.GetPageTitleByCustomerType(Utility.Common.IsUSCustomer(), DepartmentTab.PageTitle, DepartmentTab.OutsideUSPageTitle, Department.PageTitle)
        strMetaDescription = Utility.Common.GetMetaDescriptionByCustomerType(IsUSCustomer, DepartmentTab.MetaDescription, DepartmentTab.OutsideUSMetaDescription, Department.MetaDescription)
        objMetaTag.MetaKeywords = DepartmentTab.MetaKeyword
        strMetaDescription = SetMetaDescription(strMetaDescription, Department.Name)
        strMetaDescription = SetMetaDescription(strMetaDescription, DepartmentTab.Name)
        objMetaTag.MetaDescription = strMetaDescription
        objMetaTag.TypeShare = "category"
        SetPageMetaSocialNetwork(Page, objMetaTag)
        'BreadCrumb
        Dim ucBreadCrumb As Controls_Breadcrumb = Me.Master.FindControl("ucBreadCrumb")
        If Not ucBreadCrumb Is Nothing Then
            ucBreadCrumb.Args = Department.Name & "[" & Department.URLCode & "]" & Department.DepartmentId & "^" & DepartmentTab.Name & "^"
        End If

        ucListProduct.DepartmentId = DepartmentId
        ucListProduct.DepartmentTabId = TabId



        



    End Sub



End Class
