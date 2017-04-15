Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Partial Class controls_store_browser
    Inherits ModuleControl
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property

    Public brandHTML As String = String.Empty
    Protected m_DepartmentId As Integer = 0
    Protected Department As StoreDepartmentRow = Nothing
    Protected ParentDepartment As StoreDepartmentRow = Nothing
    Protected TotalBrands As Integer
    Protected MaxPerPage As Integer = 24
    Private filter As DepartmentFilterFields
    Protected IsSalesDepartment As Boolean = False
    Protected ReWriteURL As RewriteUrl
    Protected SP As New SitePage
    Private ShowBrandSubCate As Boolean = Utility.ConfigData.ShowBrandSubCate
    Protected ShowNarrowSearchSubCate As Boolean = Utility.ConfigData.ShowNarrowSearchSubCate
    Private BrandId As Integer = 0
    'Dim urlDept As String = "<span><b class=""glyphicon arrow-right""></b></span><span>{0}</span>"
    Dim urlDept As String = "<span>{0}</span>"
    Private isQuickOrder As Boolean = False
    Private URL As String = ""
    Private ckChecked As String = String.Empty
    Public ItemCount As Integer = 0
    Private DeptReview As Integer = 0
    Protected noneBorder As String = String.Empty
    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Me.Request.RawUrl.Contains("/store/search-result.aspx") Then

                If ItemCount < 1 AndAlso Not Session("CountProductGender") Is Nothing Then
                    ItemCount = Session("CountProductGender")
                End If
            End If

            If Not Request.Path.ToLower.Contains("home.aspx") Then
                LoadDepartmentBrand()
            End If
        End If
    End Sub

    Private Sub LoadDepartmentBrand()
        If Request.RawUrl.Contains("/search-result.aspx") Then
            ucResourcesCenter.Visible = False
            divTitleDepartment.InnerHtml = "Refine Results"
            If ItemCount < 1 Then
                divTitleDepartment.Visible = False
                If (SitePage.IsHasFilterSearch()) Then
                    'ucResourcesCenter.Visible = False
                    divTitleDepartment.Visible = True
                Else
                    'ucResourcesCenter.Visible = True
                End If
            End If

            rptDepartments.Visible = False
        Else
            'Long add
            pnDepartmentBrand.Visible = True

            Dim ItemCode As String = ""
            If Not String.IsNullOrEmpty(GetQueryString("DepartmentId")) Then
                m_DepartmentId = GetQueryString("DepartmentId")
                DeptReview = m_DepartmentId
                Session("DepartmentId") = m_DepartmentId
            End If
            If IsNumeric(m_DepartmentId) = False Or CheckPageLoadMainDept() Then
                m_DepartmentId = 23
            End If

            'Set Brand
            If Not String.IsNullOrEmpty(SitePage.GetQueryString("brandid")) Then
                BrandId = IIf(IsNumeric(SitePage.GetQueryString("brandid")), SitePage.GetQueryString("brandid"), 0)
            End If

            If SitePage.GetQueryString("ItemCode") <> Nothing Then
                ItemCode = SitePage.GetQueryString("ItemCode")
            End If


            If m_DepartmentId = 0 And ItemCode <> "" Then
                m_DepartmentId = StoreItemRow.GetDefaultDepartmentCodeByItemCode(ItemCode)
            ElseIf m_DepartmentId = 0 Then
                m_DepartmentId = StoreDepartmentRow.GetDefaultDepartment(DB)
            End If
            If m_DepartmentId = 0 Then
                Department = StoreDepartmentRow.GetRow(DB, Utility.ConfigData.RootDepartmentID)
            Else
                Department = StoreDepartmentRow.GetRow(DB, m_DepartmentId)
            End If

            ParentDepartment = StoreDepartmentRow.GetMainLevelDepartment(DB, Department.DepartmentId)
            If ParentDepartment.Name = "" Then
                ParentDepartment = Department
            End If

            BindData()

            ucResourcesCenter.Visible = False
        End If
    End Sub

    Private Sub BindData()
        filter = New DepartmentFilterFields
        Dim strAll As String
        Dim strSalesCate As Integer = 0
        litBreadCrumb.Text = ""
        Dim DepartmentName As String = ""
        Dim sp As New SitePage()
        Dim DeptAlternateName As String = ""
        If IsDBNull(ParentDepartment.NameRewriteUrl) = False Then
            If ParentDepartment.NameRewriteUrl <> "" Then
                DepartmentName = ReWriteURL.ReplaceUrl(ParentDepartment.NameRewriteUrl)
            Else
                DepartmentName = ReWriteURL.ReplaceUrl(ParentDepartment.Name)
            End If
        Else
            DepartmentName = ReWriteURL.ReplaceUrl(ParentDepartment.Name)
        End If

        If Department.DepartmentId <> Utility.ConfigData.RootDepartmentID And Department.DepartmentId > 0 And Not Request.RawUrl.Contains("/nail-sales") Then
            Try
                DeptAlternateName = IIf(Not IsDBNull(ParentDepartment.AlternateName) And Not String.IsNullOrEmpty(ParentDepartment.AlternateName), ParentDepartment.AlternateName.Replace(" ", "&nbsp;"), ParentDepartment.Name.Replace(" ", "&nbsp;"))
            Catch

            End Try
            If DeptAlternateName <> "" Then
                litBreadCrumb.Text &= String.Format("<a href=""{0}"">{1}</a>", URLParameters.MainDepartmentUrl(ParentDepartment.URLCode, ParentDepartment.DepartmentId), DeptAlternateName)
            End If
        ElseIf (Request.Path.ToLower = "/store/sub-category.aspx" AndAlso SitePage.GetQueryString("F_Promotion") <> Nothing) OrElse (Request.Path.ToLower = "/store/default.aspx" AndAlso SitePage.GetQueryString("F_Promotion") <> Nothing) OrElse Request.Path.ToLower = "/store/category.aspx" Then
            If SitePage.GetQueryString("SaleCategoryId") <> Nothing Then
                strSalesCate = SitePage.GetQueryString("SaleCategoryId")
            End If

            m_DepartmentId = strSalesCate

            litBreadCrumb.Text &= "<a href=""/nail-sales-promotion" & """ style = 'text-decoration:none;'>Sales & Specials</a>"
            IsSalesDepartment = True
        Else
            If BrandId > 0 Then
                litBreadCrumb.Text &= StoreBrandRow.GetRow(BrandId).BrandName
            Else
                litBreadCrumb.Text &= IIf(Request.Url.ToString().Contains("product-list.aspx"), "All Categories", String.Format("<a href=""{0}"">All Categories</a>", URLParameters.MainDepartmentUrl(Utility.ConfigData.RootDepartmentCode, 23)))
            End If
            isRefineesult.Value = 0
        End If

        'Check DepartmentName too long
        'If Not String.IsNullOrEmpty(DeptAlternateName) AndAlso DeptAlternateName.Length > 22 Then
        '    h2.Attributes.Add("class", "small")
        'End If
        Dim dsDep As New DataSet()
        Dim KeyDept As String = ""

        If Not IsSalesDepartment Then
            dsDep = ParentDepartment.GetChildrenDepartments()
        Else
            dsDep = SalesCategoryRow.GetCategoriesForMenu()
        End If

        If Not dsDep Is Nothing AndAlso dsDep.Tables.Count > 0 Then
            rptDepartments.DataSource = dsDep
            rptDepartments.DataBind()
            dsDep.Dispose()
        Else
            rptDepartments.Visible = False
        End If
        dsDep = Nothing


        If Not Request.Path.ToLower = "/home.aspx" Then
            filter.DepartmentId = Department.DepartmentId

            If SitePage.GetQueryString("F_All") <> Nothing Then
                strAll = SitePage.GetQueryString("F_All")
            Else
                strAll = "Y"
            End If

            filter.All = strAll <> String.Empty
            filter.IsOnSale = SitePage.GetQueryString("F_Sale") <> String.Empty
            filter.ToneId = IIf(Not IsNumeric(SitePage.GetQueryString("ToneId")), Nothing, SitePage.GetQueryString("ToneId"))
            filter.CollectionId = IIf(Not IsNumeric(SitePage.GetQueryString("CollectionId")), Nothing, SitePage.GetQueryString("CollectionId"))

            Dim newFilter As New DepartmentFilterFields
            newFilter.MaxPerPage = MaxPerPage
            newFilter.pg = 1
            newFilter.All = False

            'Khoa update
            'Dim brands As StoreBrandCollection = StoreBrandRow.GetAllBrand(DB)
            'If brands.Count > 0 Then
            '    Dim y As Integer = -1
            '    For i As Integer = brands.Count - 1 To 0 Step i - 1
            '        'Add Top Brand vao dropdownlist
            '        Dim strValue As String = URLParameters.BrandUrl(brands(i).URLCode, brands(i).BrandId)
            '        drpBrand.Items.Insert(0, New ListItem(brands(i).BrandName, strValue))

            '        If Not brands(i).IsTop Then
            '            brands.RemoveAt(i)
            '        End If
            '    Next

            '    drpBrand.Items.Insert(0, New ListItem("more...", ""))
            '    rptBrands.DataSource = brands
            '    rptBrands.DataBind()

            '    'Check querystrign BrandId
            '    drpBrand.Attributes.Add("onchange", "GoDrp(this);")
            'End If
        End If

        'If rptBrands.Items.Count = 0 OrElse rptBrands.Items.Count = 1 Then rptBrands.Visible = False
    End Sub
    Private Sub rptDepartments_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptDepartments.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            If Not Session("DepartmentId") Is Nothing Or SitePage.GetQueryString("RewriteURLCode") <> Nothing Then
                If Session("DepartmentId") <> 0 And Not Request.RawUrl.Contains("/nail-sales") Then
                    Dim DepartName As String
                    Dim dep As StoreDepartmentRow = StoreDepartmentRow.GetRow(DB, Session("DepartmentId"))
                    DepartName = dep.Name
                    'litPro.Text = "<a href='" & URLParameters.PromotionUrl(dep.URLCode, dep.DepartmentId) & "'>View all " & IIf(Session("DepartmentId") = Utility.ConfigData.RootDepartmentID, "items", DepartName) & " on sale</a>"
                End If

            End If
        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            If Utility.ConfigData.NoloadDept.Contains(e.Item.DataItem("DepartmentId") & ",") Then
                Exit Sub
            End If
            Try
                isQuickOrder = e.Item.DataItem("IsQuickOrder")
            Catch ex As Exception

            End Try
            Dim lit As Literal = CType(e.Item.FindControl("lit"), Literal)
            Dim rpt As Repeater = CType(e.Item.FindControl("rpt"), Repeater)
            If ((e.Item.DataItem("DepartmentId") = Department.ParentId Or e.Item.DataItem("DepartmentId") = Department.DepartmentId)) OrElse IsSalesDepartment Then
                Dim tree As TreeView = CType(e.Item.FindControl("tree"), TreeView)
                Dim ltrSubMenu As Literal = CType(e.Item.FindControl("ltrSubMenu"), Literal)
                Dim dv As DataView, drv As DataRowView
                Dim SQL As String
                Dim nDepartmentURLCode As String = ""
                nDepartmentURLCode = e.Item.DataItem("URLCode")
                Dim DeptId As Integer = e.Item.DataItem("DepartmentId")
                Dim strName As String = String.Empty

                strName &= "/" & e.Item.DataItem("URLCode")
                'IIf(IsSalesDepartment, "nail-sales/" & nDepartmentName, nDepartmentName & IIf(IsSalesDepartment, "F_Promotion=Y&", "")) & IIf(IsSalesDepartment, "", "") & "/" & e.Item.DataItem("DepartmentId")
                If m_DepartmentId = e.Item.DataItem("DepartmentId") Then
                    lit.Text = String.Format("<li class=""select"">" & urlDept, e.Item.DataItem("Name"))
                Else
                    Dim cateUrl As String = URLParameters.DepartmentUrl(nDepartmentURLCode, DeptId)
                    lit.Text = String.Format("<li>" & urlDept, "<a href=""" & cateUrl & """ class=""lgry"">" & e.Item.DataItem("Name") & "</a>") & vbCrLf
                End If

                Dim ds As DataSet = StoreDepartmentRow.GetChildDepartmentByParent(DB.Number(e.Item.DataItem("DepartmentId")))

                dv = ds.Tables(0).DefaultView
                Dim ParentId As Integer = 0
                Dim DepartmentId As Integer = 0
                Dim DepartmentName As String = ""
                If dv.Count > 0 Then
                    tree.Attributes.Add("style", "padding-bottom:0px;")
                End If
                Dim subLi As String = String.Empty
                'If BrandId = 0 Then
                ltrSubMenu.Text = "<ul id=""subCate"">"


                For i As Integer = 0 To dv.Count - 1
                    drv = dv(i)
                    ParentId = IIf(IsDBNull(drv("ParentId")), Nothing, drv("ParentId"))
                    DepartmentId = drv("DepartmentId")
                    isQuickOrder = drv("IsQuickOrder")
                    If IsDBNull(drv("NameRewriteUrl")) = False Then
                        If drv("NameRewriteUrl").ToString <> "" Then
                            DepartmentName = drv("NameRewriteUrl")
                        Else
                            DepartmentName = drv("NAME")
                        End If
                    Else
                        DepartmentName = drv("NAME")
                    End If
                    URL = IIf(isQuickOrder, URLParameters.DepartmentCollectionUrl(drv("URLCode"), DepartmentId), URLParameters.DepartmentUrl(drv("URLCode"), DepartmentId))
                    DepartmentName = URLParameters.ReplaceUrl(HttpUtility.UrlEncode(DepartmentName.Replace("/", "_")))
                    If DepartmentId = m_DepartmentId AndAlso (LCase(Request.ServerVariables("URL")) = "/store/default.aspx" OrElse LCase(Request.ServerVariables("URL")) = "/store/category.aspx" OrElse LCase(Request.ServerVariables("URL")) = "/store/item.aspx" OrElse LCase(Request.ServerVariables("URL")) = "/store/sub-category.aspx") Then
                        If LCase(Request.ServerVariables("URL")) = "/store/item.aspx" Or LCase(Request.ServerVariables("URL")) = "/store/musicitem.aspx" Then
                            subLi = String.Format("<li class=""select"">" & String.Format(urlDept, "<a href=""{0}{1}"" class=""select"">{2}</a>") & "</li>", IIf(IsSalesDepartment, "category.aspx?SalesCategoryId=" & e.Item.DataItem("DepartmentId"), ""), URL, drv("NAME"))
                        Else
                            URL = URLParameters.DepartmentUrl(nDepartmentURLCode, DeptId)
                            subLi = String.Format("<li class=""checkbox""><label for=""chkdept" & i + 1 & """><input type=""checkbox"" id=""chkdept" & i + 1 & """ onclick=""window.location='" & URL & "'"" checked><i class=""fa fa-check checkbox-font"" ></i>" & urlDept & "</label></li>", drv("NAME"))
                        End If
                    Else
                        'subLi = String.Format("<li class=""sub"">" & String.Format(urlDept, "<a href=""{0}{1}"">{2}</a>") & "</li>", IIf(IsSalesDepartment, "category.aspx?SalesCategoryId=" & e.Item.DataItem("DepartmentId"), ""), URL, drv("NAME"))
                        subLi = String.Format("<li class=""checkbox""><label for=""chkdept" & i + 1 & """><input type=""checkbox"" id=""chkdept" & i + 1 & """ onclick=""window.location='" & URL & "'""" & ckChecked & "><i class=""fa fa-check checkbox-font"" ></i>" & String.Format(urlDept, "<a href=""{0}{1}"">{2}</a>") & "</label></li>", IIf(IsSalesDepartment, "category.aspx?SalesCategoryId=" & e.Item.DataItem("DepartmentId"), ""), URL, drv("NAME"))
                    End If
                    ltrSubMenu.Text = ltrSubMenu.Text + subLi

                Next
                ltrSubMenu.Text = ltrSubMenu.Text + "</ul>"
                'End If
                If String.IsNullOrEmpty(subLi) Then
                    lit.Visible = False
                    ltrSubMenu.Visible = False
                    isRefineesult.Value = 0
                    noneBorder = "border-none"
                End If
                'only load menu select
                'Else
                '    URL = IIf(isQuickOrder, URLParameters.ProductCollectionUrl(e.Item.DataItem("URLCode"), CInt(e.Item.DataItem("DepartmentId"))), URLParameters.DepartmentUrl(e.Item.DataItem("URLCode"), CInt(e.Item.DataItem("ParentId")), CInt(e.Item.DataItem("DepartmentId"))))
                '    lit.Text = String.Format("<li>" & String.Format(urlDept, "<a href=""{0}"">{1}</a>"), URL, e.Item.DataItem("Name"))
            ElseIf CheckPageLoadMainDept() = True Then
                Dim checked, urlreview As String
                URL = URLParameters.ProductReviewUrl(e.Item.DataItem("URLCode"), e.Item.DataItem("DepartmentId"))
                If DeptReview = e.Item.DataItem("DepartmentId") Then
                    checked = "checked"
                    urlreview = urlDept
                    URL = "/product-reviews"
                Else
                    urlreview = String.Format(urlDept, String.Format("<a href=""{0}"">{1}</a>", URL, e.Item.DataItem("Name")))
                End If
                lit.Text = String.Format("<li class=""checkbox""><label for=""chkdept" & e.Item.DataItem("DepartmentId") & """><input type=""checkbox"" id=""chkdept" & e.Item.DataItem("DepartmentId") & """ onclick=""window.location='" & URL & "'"" " & checked & " ><i class=""fa fa-check checkbox-font"" ></i>" & urlreview & "</label></li>", e.Item.DataItem("Name"))
            End If
        End If
    End Sub
    Private Function CheckPageLoadMainDept() As Boolean
        If LCase(Request.RawUrl).Contains("product-reviews") Then
            isRefineesult.Value = 1
            brandHTML = String.Empty
            Return True
        End If
        'isRefineesult.Value = 0
        Return False
    End Function
    'Private Sub rptBrands_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptBrands.ItemDataBound
    '    If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
    '        Dim lit As Literal = CType(e.Item.FindControl("lit"), Literal)
    '        Dim brand As StoreBrandRow = CType(e.Item.DataItem, StoreBrandRow)

    '        Dim strBrandName As String = IIf(brand.BrandNameUrl <> "", brand.BrandNameUrl, brand.BrandName)
    '        If BrandId = brand.BrandId AndAlso Department.DepartmentId = Utility.ConfigData.RootDepartmentID Then
    '            If LCase(Request.ServerVariables("URL")) = "/store/default.aspx" Or LCase(Request.ServerVariables("URL")) = "/store/sub-category.aspx" Then
    '                lit.Text = "<li><span><b class=""glyphicon arrow-right""></b></span>" & brand.BrandName & "</li>"
    '            Else
    '                lit.Text = String.Format("<li><span><b class=""glyphicon arrow-right""></b></span><span><a href=""{0}"">{1}</a></span></li>", URLParameters.BrandUrl(brand.URLCode, BrandId))
    '            End If
    '        Else
    '            lit.Text = String.Format("<li><span><b class=""glyphicon arrow-right""></b></span><span><a href=""{0}"">{1}</a></span></li>", URLParameters.BrandUrl(brand.URLCode, brand.BrandId), brand.BrandName)
    '        End If
    '    End If

    'End Sub

    'Private Sub drpBrand_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
    '    If drpBrand.SelectedValue <> "More..." And drpBrand.SelectedValue <> "" Then
    '        Response.Redirect(URLParameters.BrandUrl(drpBrand.SelectedValue))
    '    End If
    'End Sub
#Region "DepartmentBrand"
    Private Sub LoadListBrandForDepartment()
        If Not ShowBrandSubCate And (Me.Request.FilePath.Contains("/store/default.aspx") Or Me.Request.FilePath.Contains("/store/sub-category.aspx")) Then
            Exit Sub
        End If

        Dim defaultShowBrand As Integer = Utility.ConfigData.DefaultBrandShow
        ' Dim rootDepartment As String = String.Empty
        Dim rootDepartment As Integer = 0
        'Dim departmentCode As String = GetQueryString("DepartmentURLCode")
        Dim DepartmentId As Integer = 0
        If Not String.IsNullOrEmpty(GetQueryString("DepartmentId")) Then
            DepartmentId = GetQueryString("DepartmentId")
            Session("DepartmentId") = DepartmentId
        End If
        Dim path As String = StoreDepartmentRow.GetDepartmentPath(Nothing, DepartmentId)
        rootDepartment = ReWriteURL.GetRootDepartmentId(path)
        If rootDepartment = 0 Then
            '    rootDepartment = departmentCode
            rootDepartment = DepartmentId
        End If
        If String.IsNullOrEmpty(rootDepartment) OrElse rootDepartment = 250 Then
            Exit Sub
        End If
        Dim RecentDepartment As StoreDepartmentRow = Nothing
        If (Department.DepartmentId = 0) Then
            RecentDepartment = StoreDepartmentRow.GetRow(DB, DepartmentId) 'GetByURLCode(DB, rootDepartment)
        Else
            If Not Department.URLCode Is Nothing Then
                If rootDepartment <> Department.DepartmentId Then
                    RecentDepartment = StoreDepartmentRow.GetRow(DB, rootDepartment)
                    If RecentDepartment.DepartmentId = 0 Then
                        Exit Sub
                    End If
                Else
                    RecentDepartment = Department
                End If
            Else
                Exit Sub
            End If

        End If
        If (RecentDepartment.DepartmentId = 0) Then
            Response.Redirect("/")
        End If
        ''ltrDepartmentName.Text = RecentDepartment.Name & "'s brands"
        ' Dim brandCode As String = GetQueryString("brand")
        'Dim BrandId As String = 0
        If String.IsNullOrEmpty(GetQueryString("brandid")) = False Then
            BrandId = CInt(GetQueryString("brandid"))
        End If
        ''get list Brand
        'If brandCode Is Nothing Then
        '    brandCode = String.Empty
        'End If
        'If BrandId = 0 Then
        '    brandCode = String.Empty
        'End If

        Dim lstBrand As StoreBrandCollection = StoreBrandRow.GetListByDepartmentId(DB, RecentDepartment.DepartmentId) ''get list all brand for this category
        If Not lstBrand Is Nothing Then
            Dim index As Integer = 1
            Dim brandIdHidden As String = String.Empty
            Dim hasItemBrandSelect As Boolean = False
            Dim brandSelect As Boolean = False
            If Me.Request.FilePath.Contains("/store/default.aspx") Or Me.Request.FilePath.Contains("/store/sub-category.aspx") Then
                Dim result As String = ""
                Dim childresult As String = String.Empty
                Dim show As Boolean = False
                Dim childShow As Boolean = False
                Dim childSelect As Boolean = False
                For Each item As StoreBrandRow In lstBrand
                    'If item.URLCode.Trim().ToLower() <> "999" Then
                    '    Continue For
                    'End If
                    brandSelect = True
                    Dim dtSource As DataTable = StoreDepartmentRow.GetByBrandId(RecentDepartment.DepartmentId, item.BrandId) ''get list all category for Brand
                    Dim dRowLevel As DataRow() = dtSource.Select("Level=2", "Name ASC")
                    If (dRowLevel.Length > 0) Then '' has child
                        If item.BrandId = BrandId Then
                            brandSelect = True
                            hasItemBrandSelect = True
                            childresult = CreateSubLink(childShow, childSelect, True, dRowLevel, dtSource, item.BrandId, DepartmentId, item.BrandId)
                        Else
                            childresult = CreateSubLink(childShow, childSelect, False, dRowLevel, dtSource, item.BrandId, DepartmentId, item.BrandId)
                        End If
                    End If
                    'If Not String.IsNullOrEmpty(brandCode) Then
                    If BrandId > 0 Then
                        If index <= defaultShowBrand Then
                            ''  result = result & "<li><a href='/nail-supply/" & RecentDepartment.URLCode & "?brand=" & item.URLCode & "'>" & item.BrandName & "</a>" & childresult & "</li>"
                            ' result = result & CreateBrandLink(RecentDepartment.URLCode, item, brandCode, childresult, True, childSelect)
                            result = result & CreateBrandLink(RecentDepartment.URLCode, item, BrandId, childresult, True, childSelect)
                        Else
                            'If (hasItemBrandSelect Or childShow) And (item.URLCode.Trim().ToLower() <> brandCode.Trim().ToLower()) Then
                            If (hasItemBrandSelect Or childShow) And (item.BrandId <> BrandId) Then
                                brandIdHidden = brandIdHidden & item.BrandId & ","
                                ''result = result & "<li style='display:none;' id='liBrand_" & item.BrandId & "'><a href='/nail-supply/" & RecentDepartment.URLCode & "?brand=" & item.URLCode & "'>" & item.BrandName & "</a>" & childresult & "</li>"
                                'result = result & CreateBrandLink(RecentDepartment.URLCode, item, brandCode, childresult, False, childSelect)
                                result = result & CreateBrandLink(RecentDepartment.URLCode, item, BrandId, childresult, False, childSelect)
                            Else
                                '' result = result & "<li><a href='/nail-supply/" & RecentDepartment.URLCode & "?brand=" & item.URLCode & "'>" & item.BrandName & "</a>" & childresult & "</li>"
                                'result = result & CreateBrandLink(RecentDepartment.URLCode, item, brandCode, childresult, True, childSelect)
                                result = result & CreateBrandLink(RecentDepartment.URLCode, item, BrandId, childresult, True, childSelect)
                            End If
                        End If
                    Else
                        If index <= defaultShowBrand Then
                            '' result = result & "<li><a href='/nail-supply/" & RecentDepartment.URLCode & "?brand=" & item.URLCode & "'>" & item.BrandName & "</a>" & childresult & "</li>"
                            'result = result & CreateBrandLink(RecentDepartment.URLCode, item, brandCode, childresult, True, childSelect)
                            result = result & CreateBrandLink(RecentDepartment.URLCode, item, BrandId, childresult, True, childSelect)
                        Else
                            brandIdHidden = brandIdHidden & item.BrandId & ","
                            ''result = result & "<li style='display:none;' id='liBrand_" & item.BrandId & "'><a href='/nail-supply/" & RecentDepartment.URLCode & "?brand=" & item.URLCode & "'>" & item.BrandName & "</a>" & childresult & "</li>"
                            'result = result & CreateBrandLink(RecentDepartment.URLCode, item, brandCode, childresult, False, childSelect)
                            result = result & CreateBrandLink(RecentDepartment.URLCode, item, BrandId, childresult, False, childSelect)
                        End If
                    End If
                    index = index + 1
                Next
                brandHTML = "<ul class='content'>" & result & ""
            Else
                brandHTML = "<ul class='content'>"
                For Each item As StoreBrandRow In lstBrand
                    Dim urlBrand As String = String.Format("<span><b class=""glyphicon arrow-right""></b></span><span><a href='{0}?brandid={1}'>{2}</a></span>", URLParameters.DepartmentUrl(RecentDepartment.URLCode, RecentDepartment.DepartmentId), item.BrandId, item.BrandName)
                    If item.BrandId = BrandId Then
                        hasItemBrandSelect = True
                    End If
                    If index <= defaultShowBrand Then
                        brandHTML = brandHTML & "<li>" & urlBrand & "</li>"
                    Else
                        If Not hasItemBrandSelect And BrandId > 0 Then
                            brandHTML = brandHTML & "<li>" & urlBrand & "</li>"
                        Else
                            If hasItemBrandSelect Then
                                brandHTML = brandHTML & "<li>" & urlBrand & "</li>"
                                ''  hasItemBrandSelect = False
                            Else
                                brandIdHidden = brandIdHidden & item.BrandId & ","
                                brandHTML = brandHTML & "<li style='display:none;' id='liBrand_" & item.BrandId & "'>" & urlBrand & "</li>"
                            End If
                        End If
                    End If
                    index = index + 1
                Next
                '' brand = brand & ""
            End If
            If brandIdHidden.Length > 0 Then
                brandHTML = brandHTML & "<li class='more' id='liMoreBrand' ><a href='javascript:void(0)' onclick=""ShowMoreBrand('" & brandIdHidden & "')"">Show All Brand</a></li></ul>"
            Else
                brandHTML = brandHTML & "</ul>"
            End If
        End If

    End Sub
    Private Function CreateBrandLink(ByVal departmentURLCode As String, ByVal objBrand As StoreBrandRow, ByVal requestBrandCode As String, ByVal childResult As String, ByVal display As Boolean, ByVal childSelect As Boolean) As String
        If requestBrandCode.Trim().ToLower() <> objBrand.URLCode.Trim().ToLower() Then
            If display Then
                Return "<li id='liBrand_" & objBrand.BrandId & "'><a href='/nail-supply/" & departmentURLCode & "?brand=" & objBrand.URLCode & "'>" & objBrand.BrandName & "</a>" & childResult & "</li>"
            Else
                Return "<li style='display:none;' id='liBrand_" & objBrand.BrandId & "'><a href='/nail-supply/" & departmentURLCode & "?brand=" & objBrand.URLCode & "'>" & objBrand.BrandName & "</a>" & childResult & "</li>"
            End If
        End If
        If (childSelect) Then
            Return "<li class='select' id='liBrand_" & objBrand.BrandId & "'><a href='/nail-supply/" & departmentURLCode & "?brand=" & objBrand.URLCode & "'>" & objBrand.BrandName & "</a>" & childResult & "</li>"
        Else
            Return "<li class='select' id='liBrand_" & objBrand.BrandId & "' ><a  href='javascript:void(0);' onClick='ShowBrand(" & objBrand.BrandId & ")' >" & objBrand.BrandName & "</a>" & childResult & "</li>"


        End If

    End Function
    Private Function CreateBrandLink(ByVal departmentURLCode As String, ByVal objBrand As StoreBrandRow, ByVal requestBrandId As Integer, ByVal childResult As String, ByVal display As Boolean, ByVal childSelect As Boolean) As String
        If requestBrandId <> objBrand.BrandId Then
            If display Then
                Return "<li id='liBrand_" & objBrand.BrandId & "'><a href='/nail-supply/" & departmentURLCode & "?brandId=" & objBrand.BrandId & "'>" & objBrand.BrandName & "</a>" & childResult & "</li>"
            Else
                Return "<li style='display:none;' id='liBrand_" & objBrand.BrandId & "'><a href='/nail-supply/" & departmentURLCode & "?brandId=" & objBrand.BrandId & "'>" & objBrand.BrandName & "</a>" & childResult & "</li>"
            End If
        End If
        If (childSelect) Then
            Return "<li class='select' id='liBrand_" & objBrand.BrandId & "'><a href='/nail-supply/" & departmentURLCode & "?brandid=" & objBrand.BrandId & "'>" & objBrand.BrandName & "</a>" & childResult & "</li>"
        Else
            Return "<li class='select' id='liBrand_" & objBrand.BrandId & "' ><a  href='javascript:void(0);' onClick='ShowBrand(" & objBrand.BrandId & ")' >" & objBrand.BrandName & "</a>" & childResult & "</li>"


        End If

    End Function
    Public Function CreateSubLink(ByRef isShow As Boolean, ByRef childSelect As Boolean, ByVal parerntShow As Boolean, ByVal dtRow As DataRow(), ByVal dtSource As DataTable, ByVal curentBrandURLCode As String, ByVal curentDepartmentURLCode As String, ByVal parentId As Integer) As String

        Dim result As String = String.Empty
        Dim childresult As String = String.Empty
        Dim childShow As Boolean = False
        Dim currentSelect As Boolean = False
        Dim liCss As String = ""
        isShow = False
        Dim Level As Integer = 1
        For Each item As DataRow In dtRow
            liCss = String.Empty
            Dim departmentId As Integer = item("DepartmentId")
            Level = item("Level")
            Dim urlCode As String = item("URLCode")
            Dim name As String = item("Name")
            Dim dRowLevel As DataRow() = dtSource.Select("ParentId=" & departmentId, "Name ASC")
            Dim checkShowParent As Boolean = False
            If urlCode.Trim().ToLower() = curentDepartmentURLCode.Trim().ToLower() Then
                checkShowParent = True
                currentSelect = True
            Else
                currentSelect = False
            End If
            If (dRowLevel.Length > 0) Then '' has child
                childresult = CreateSubLink(childShow, childSelect, checkShowParent, dRowLevel, dtSource, curentBrandURLCode, curentDepartmentURLCode, departmentId)
            Else
                childresult = String.Empty
                childShow = False
            End If
            If currentSelect Then
                childSelect = True
            End If
            Dim brandCode As String = GetQueryString("brand")
            If brandCode Is Nothing Then
                brandCode = String.Empty
            End If
            If parerntShow Then
                isShow = True
            Else

                If brandCode.Trim.ToLower() = curentBrandURLCode.Trim().ToLower() Then
                    If (childShow) Then
                        isShow = True
                    Else
                        If urlCode.Trim().ToLower() = curentDepartmentURLCode.Trim().ToLower() Then
                            isShow = True
                        End If
                    End If
                Else
                    isShow = False
                End If
            End If
            If checkShowParent Then
                If Not String.IsNullOrEmpty(childresult) Then
                    result = result & "<li class='select'><a  href='/nail-supply/" & urlCode & "?brand=" & curentBrandURLCode & "'>" & name & "</a>" & childresult & "</li>"
                Else
                    result = result & "<li> " & name & " </li>"
                End If

                If brandCode.Trim.ToLower() = curentBrandURLCode.Trim.ToLower() Then
                    isShow = True
                End If
            Else
                If Level = 2 And childShow Then
                    liCss = "select"
                Else
                    If Level = 3 Then
                        liCss = "item"
                    End If
                End If
                result = result & "<li class='" & liCss & "'><a href='/nail-supply/" & urlCode & "?brand=" & curentBrandURLCode & "'>" & name & "</a>" & childresult & "</li>"
            End If
        Next
        If (isShow) Then
            result = " <ul id='departmentBrandL_" & parentId & "' class='departmentBrandL" & Level & "' style='display:;'>" & result & "</ul>"
        Else
            result = " <ul id='departmentBrandL_" & parentId & "'  class='departmentBrandL" & Level & "'  style='display:none;'>" & result & "</ul>"
        End If
        Return result
    End Function
    Public Function CreateSubLink(ByRef isShow As Boolean, ByRef childSelect As Boolean, ByVal parerntShow As Boolean, ByVal dtRow As DataRow(), ByVal dtSource As DataTable, ByVal curentBrandId As Integer, ByVal curentDepartmentId As Integer, ByVal parentId As Integer) As String

        Dim result As String = String.Empty
        Dim childresult As String = String.Empty
        Dim childShow As Boolean = False
        Dim currentSelect As Boolean = False
        Dim liCss As String = ""
        isShow = False
        Dim Level As Integer = 1
        For Each item As DataRow In dtRow
            liCss = String.Empty
            Dim departmentId As Integer = item("DepartmentId")
            Level = item("Level")
            Dim urlCode As String = item("URLCode")
            Dim name As String = item("Name")
            Dim dRowLevel As DataRow() = dtSource.Select("ParentId=" & departmentId, "Name ASC")
            Dim checkShowParent As Boolean = False
            If departmentId = curentDepartmentId Then
                checkShowParent = True
                currentSelect = True
            Else
                currentSelect = False
            End If
            If (dRowLevel.Length > 0) Then '' has child
                childresult = CreateSubLink(childShow, childSelect, checkShowParent, dRowLevel, dtSource, curentBrandId, curentDepartmentId, departmentId)
            Else
                childresult = String.Empty
                childShow = False
            End If
            If currentSelect Then
                childSelect = True
            End If
            Dim brandId As Integer
            If String.IsNullOrEmpty(GetQueryString("brandid")) = False Then
                brandId = CInt(GetQueryString("brandid"))
            End If
            'If brandCode Is Nothing Then
            '    brandCode = String.Empty
            'End If
            If parerntShow Then
                isShow = True
            Else

                If brandId = curentBrandId Then
                    If (childShow) Then
                        isShow = True
                    Else
                        If departmentId = curentDepartmentId Then
                            isShow = True
                        End If
                    End If
                Else
                    isShow = False
                End If
            End If
            If checkShowParent Then
                If Not String.IsNullOrEmpty(childresult) Then
                    result = result & "<li class='select'><a  href='/nail-supply/" & urlCode & "?brandid=" & curentBrandId & "'>" & name & "</a>" & childresult & "</li>"
                Else
                    result = result & "<li> " & name & " </li>"
                End If

                If brandId = curentBrandId Then
                    isShow = True
                End If
            Else
                If Level = 2 And childShow Then
                    liCss = "select"
                Else
                    If Level = 3 Then
                        liCss = "item"
                    End If
                End If
                result = result & "<li class='" & liCss & "'><a href='/nail-supply/" & urlCode & "?brandid=" & curentBrandId & "'>" & name & "</a>" & childresult & "</li>"
            End If
        Next
        If (isShow) Then
            result = " <ul id='departmentBrandL_" & parentId & "' class='departmentBrandL" & Level & "' style='display:;'>" & result & "</ul>"
        Else
            result = " <ul id='departmentBrandL_" & parentId & "'  class='departmentBrandL" & Level & "'  style='display:none;'>" & result & "</ul>"
        End If
        Return result
    End Function
#End Region
End Class
