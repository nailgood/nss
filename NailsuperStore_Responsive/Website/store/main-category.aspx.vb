Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Collections.Generic
Imports Humanizer
Public Class store_main_category
    Inherits SitePage
    Private departmentId As Integer
    Private departmentURLCode As String
    Protected strDescription As String
    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadDepartment()
            LoadSubCategory()
        End If
    End Sub
    Private Sub LoadDepartment()
        If Not String.IsNullOrEmpty(GetQueryString("DepartmentId")) Then
            departmentId = CInt(GetQueryString("DepartmentId"))
        End If

        Dim Department As StoreDepartmentRow = StoreDepartmentRow.GetRow(DB, departmentId)
        If Department Is Nothing Then
            Response.Redirect("/")
        End If

        If Department.ParentId <> Utility.ConfigData.RootDepartmentID Then
            Response.Redirect("/")
        End If

        Utility.Common.CheckValidURLCode(Department.URLCode)

        'If Department.IsQuickOrder Then
        '    Utility.Common.Redirect301(URLParameters.ProductCollectionUrl(Department.URLCode, departmentId))
        'End If

        departmentURLCode = Department.URLCode
        ltrDepartmentName.Text = Department.Name
        ucLeftMenu.DepartmentId = Department.DepartmentId
        Dim isUSCustomer As Boolean = Utility.Common.IsUSCustomer()
        Dim defaultPageTitle As String = String.Empty
        Dim defaultMetaDes As String = String.Empty

        Dim row As ContentToolPageRow = ContentToolPageRow.GetRowByURL("/store/main-category.aspx")
        If row.PageId > 0 Then
            defaultPageTitle = row.Title
            defaultMetaDes = row.MetaDescription
        End If

        'Set MetaTag
        Dim strPageTitle = Utility.Common.GetPageTitleByCustomerType(isUSCustomer, Department.PageTitle, Department.OutsideUSPageTitle, defaultPageTitle)
        Dim strMetaDescription As String = Utility.Common.GetMetaDescriptionByCustomerType(isUSCustomer, Department.MetaDescription, Department.OutsideUSMetaDescription, defaultMetaDes)

        strDescription = BBCodeHelper.ConvertBBCodeToHTML(Department.Description)
        Dim shareURL As String = GlobalSecureName & URLParameters.MainDepartmentUrl(Department.URLCode, Department.DepartmentId)
        Dim objMetaTag As New MetaTag
        objMetaTag.MetaKeywords = Department.MetaKeywords
        objMetaTag.PageTitle = strPageTitle
        objMetaTag.MetaDescription = strMetaDescription
        objMetaTag.TypeShare = "category"
        objMetaTag.ShareDesc = IIf(String.IsNullOrWhiteSpace(Department.Description), strMetaDescription, Department.Description)
        objMetaTag.ShareTitle = strPageTitle
        objMetaTag.ShareURL = shareURL
        objMetaTag.Canonical = shareURL
        SetPageMetaSocialNetwork(Page, objMetaTag)
    End Sub

    Private Sub LoadSubCategory()
        If departmentId > 0 Then
            Dim lstCate As StoreDepartmentCollection = StoreDepartmentRow.LoadListMainPage(departmentId)
            If Not lstCate Is Nothing AndAlso lstCate.Count > 0 Then
                rptSubCate.DataSource = lstCate
                rptSubCate.DataBind()
            End If
        End If
    End Sub
    Protected Sub rptSubCate_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptSubCate.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim objDepartment As StoreDepartmentRow = CType(e.Item.DataItem, StoreDepartmentRow)
            Dim link As String = String.Empty
            Dim isTab As Boolean = False
            If objDepartment.ParentId > 0 Then
                isTab = False
            Else
                isTab = True
            End If
            If Not isTab Then
                link = URLParameters.DepartmentUrl(objDepartment.URLCode, objDepartment.DepartmentId)
            Else
                link = URLParameters.MainDepartmentUrl(URLParameters.ReplaceUrl(departmentURLCode), departmentId) & "/" & objDepartment.URLCode
            End If
            Dim ltrImage As Literal = DirectCast(e.Item.FindControl("ltrImage"), Literal)
            Dim ltrName As Literal = DirectCast(e.Item.FindControl("ltrName"), Literal)
            If Not ltrImage Is Nothing Then
                ''get image
                Dim img As String = objDepartment.LargeImage
                Dim imgPath As String = String.Empty
                If String.IsNullOrEmpty(img) Then
                    imgPath = Utility.ConfigData.CDNMediaPath & Utility.ConfigData.DepartmentMainNoImagePath
                Else
                    If isTab Then
                        imgPath = Utility.ConfigData.CDNMediaPath & Utility.ConfigData.DepartmentTabImageFolder & img
                    Else
                        imgPath = Utility.ConfigData.CDNMediaPath & Utility.ConfigData.DepartmentMainImageFolder & img
                    End If
                End If
               
                ltrImage.Text = "<a href='" & link & "'><img src='" & imgPath & "' alt='" & objDepartment.Name & "' /></a>"
            End If
            If Not ltrName Is Nothing Then
                ltrName.Text = "<a href='" & link & "'>" & objDepartment.Name & "</a>"
            End If
            Dim divItem As HtmlGenericControl = CType(e.Item.FindControl("divItem"), HtmlGenericControl)
            If Not divItem Is Nothing Then
                Dim classItem As String = String.Empty
                If e.Item.ItemIndex Mod 2 = 0 Then
                    classItem = "item smallfirst"
                Else
                    classItem = "item smalllast"
                End If
                divItem.Attributes.Add("class", classItem)
            End If
        End If
    End Sub
  
End Class
