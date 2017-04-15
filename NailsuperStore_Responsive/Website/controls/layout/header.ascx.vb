Option Strict Off

Imports Components
Imports DataLayer
Imports System.Data
Imports System.IO
Imports System.Web
Imports System.Net
Imports Utility
Imports System.Web.Services
Imports System.Collections.Generic
Partial Class Header
    Inherits ModuleControl
    Protected Shared m_Cart As ShoppingCart
    Protected m_HasAccess As Boolean
    Public m_MemberId As Integer = 0
    Protected cartItemCount As Integer = 0
    Protected m_Point As Integer
    Protected m_LoggedInName As String
    Protected isShowLiveChat As Boolean = True
    Protected arrPro As String()
    Public ContainerCss As String = "container"
    Protected strPage As String = HttpContext.Current.Request.Url.AbsolutePath
    Protected css As String = ""
    Private imgShopsave As String = "<img alt=""{1}"" src=""" & Utility.ConfigData.CDNMediaPath & "/assets/shopsave/home/{0}"">"
    Protected TotalRecords As Integer = 0
    Protected getTimeEnd As Integer
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property
    Protected ReadOnly Property HasAccess() As Boolean
        Get
            Return Session("MemberId") <> Nothing
        End Get
    End Property
    Public Property IsFullScreen() As Boolean
        Get
            Return ViewState("IsFullScreen")
        End Get
        Set(ByVal value As Boolean)
            ViewState("IsFullScreen") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Try
        '    Dim getSecond As TimeSpan = Convert.ToDateTime("2015/09/01").Subtract(DateTime.Now)
        '    getTimeEnd = IIf(getSecond.TotalSeconds >= 0, getSecond.TotalSeconds - 2, 0)
        'Catch

        'End Try
        If Utility.Common.IsViewFromAdmin() Then
            Me.Visible = False
            Exit Sub
        End If
        If TypeOf Me.Page Is SitePage Then
            Dim p As SitePage = CType(Me.Page, SitePage)
            If p IsNot Nothing Then
                m_Cart = p.Cart(True)
                Try
                    If Not m_Cart Is Nothing Then
                        cartItemCount = m_Cart.GetCartItemCount()
                    End If

                Catch ex As Exception
                End Try
            Else
                Email.SendError("ToError500", "Header SitePage Nothing", "OrderId: " & Session("OrderId") & "<br>MemberId: " & Session("MemberId"))
            End If

            'Check order is commit
            If Not m_Cart Is Nothing AndAlso Not m_Cart.Order Is Nothing AndAlso Not m_Cart.Order.OrderNo Is Nothing AndAlso m_Cart.Order.OrderNo.Length > 0 Then
                Session("OrderId") = Nothing
                m_Cart = Nothing
            End If
            m_HasAccess = p.HasAccess

            If HasAccess() AndAlso p.GetLoggedInName IsNot Nothing Then
                m_MemberId = Session("MemberId")
                m_LoggedInName = p.GetLoggedInName
                'If m_LoggedInName Is Nothing Then
                '    m_LoggedInName = p.LoggedInEmail
                'End If

                'New header: turn off summary point
                'Try
                '    If Not MemberRow.MemberInGroupWHS(m_MemberId) = True Then
                '        m_Point = p.PointAvailable
                '    End If
                'Catch
                'End Try
            Else
                m_MemberId = Utility.Common.GetMemberIdFromCartCookie()
                If m_MemberId > 0 Then
                    Dim dbLastOrderId As Integer = 0
                    'New header: turn off summary point
                    'If Not MemberRow.MemberInGroupWHS(m_MemberId) = True Then
                    '    m_Point = p.PointAvailable
                    'End If

                    Dim dtLogin As DataTable = DB.GetDataTable("Select COALESCE(c.Name,'') as Name,COALESCE(c.Name2,'') as Name2,Email,LastOrderId from Member m left join Customer c on(m.CustomerId=c.CustomerId) where m.MemberId=" & m_MemberId)
                    If Not dtLogin Is Nothing AndAlso dtLogin.Rows.Count > 0 Then
                        If Not String.IsNullOrEmpty(dtLogin.Rows(0)("Name").ToString()) Then
                            m_LoggedInName = dtLogin.Rows(0)("Name").ToString()
                        Else
                            m_LoggedInName = dtLogin.Rows(0)("Name2").ToString()
                            If String.IsNullOrEmpty(m_LoggedInName) Then
                                m_LoggedInName = dtLogin.Rows(0)("Email").ToString()
                            End If
                        End If

                        dbLastOrderId = CInt(dtLogin.Rows(0)("LastOrderId").ToString())
                        If dbLastOrderId > 0 Then
                            Utility.Common.SetOrderToCartCookie(dbLastOrderId)
                        End If
                    End If
                End If
            End If

            If m_MemberId > 0 AndAlso Not String.IsNullOrEmpty(m_LoggedInName) AndAlso m_LoggedInName.Length > 15 Then
                Try
                    If m_LoggedInName.Split(" ")(0).Length > 15 Then
                        m_LoggedInName = Left(m_LoggedInName, 15) & "..."
                        m_LoggedInName = m_LoggedInName.Replace("....", "...")
                    End If
                Catch ex As Exception

                End Try
            End If
        Else
            m_Cart = Nothing
            m_HasAccess = False
            m_LoggedInName = Nothing
        End If
        If Not IsPostBack Then
            ContainerCss &= IIf(IsFullScreen, "-fluid", "")

            If SysParam.GetValue("LiveChat") = 0 Then
                isShowLiveChat = False
            Else
                isShowLiveChat = True
            End If

            'BindData()
            Dim DepartmentURLCode As String = SelectedDepartmentCode()
            Dim path As String = StoreDepartmentRow.GetDepartmentPathByURLCode(Nothing, DepartmentURLCode)
            Dim menuRootCode As String = RewriteUrl.GetRootDepartmentCode(path)
            If menuRootCode <> "" Then
                DepartmentURLCode = menuRootCode
            End If
            Dim departmentId As Integer = StoreDepartmentRow.GetDepartmentIdByDepertmentCode(DepartmentURLCode)
            Try
                If Not m_Cart Is Nothing Then
                    cartItemCount = m_Cart.GetCartItemCount
                End If

            Catch
            End Try

            If (Me.Request.FilePath.Contains("/store/cart.aspx") Or Me.Request.RawUrl.Contains("/store/reward-point.aspx") Or Me.Request.RawUrl.Contains("/store/payment.aspx") Or Me.Request.RawUrl.Contains("/store/confirmation.aspx")) Then
                hidAllowShowPopupCart.Value = "0"
            Else
                hidAllowShowPopupCart.Value = "1"
            End If
        End If

        BuildMainDepartment()
        LoadDealsCenterMenu()
        LoadShopbyDesignMenu()
    End Sub


    Private Sub lnkremoveBuyPoint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkremoveBuyPoint.Click
        If hidRemove.Value.Length > 0 Then
            m_Cart.RemoveItemBuyPoint(hidRemove.Value, m_Point)
            m_Cart.RecalculateOrderDetail("header.lnkremoveBuyPoint_Click")
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "ResetCart", "ResetCart(" & m_Cart.GetCartItemCount & ");", True)
        End If
    End Sub

    Private Sub BindData()
        Try
            Dim key As String = PromotionSalespriceRow.cachePromotion
            Dim listPro As String = ""
            listPro = CType(CacheUtils.GetCache(key), String)
            If String.IsNullOrEmpty(listPro) Then
                listPro = DB.ExecuteScalar("select stuff((Select '|' + SubTitle + ';' + MainTitle FROM promotionsalesprice WHERE (Type = 3 or Type = 4) and IsActive = 1 AND CONVERT(DATE, GETDATE()) BETWEEN CONVERT(DATE,coalesce(StartingDate,GETDATE())) AND CONVERT(DATE, coalesce(EndingDate,GETDATE())) FOR XML PATH('')), 1, 1, '')")
            End If
            arrPro = listPro.Split("|")
            CacheUtils.SetCache(key, listPro, Utility.ConfigData.TimeCacheData)
        Catch ex As Exception
            Email.SendError("ToError500", "header.ascx _Get Promotion BindDate()", ex.ToString())
        End Try

    End Sub

    'Build HTMl cho menu Main Department
    Private Sub BuildMainDepartment()
        Dim ds As DataSet = StoreDepartmentRow.GetMainLevelDepartments(DB)
        Dim lnkDepartment As String = "<li id=""{0}"" class=""main""><a href=""{1}"" class=""main"" data-context='{1}'>{2}<span class='icon'></span></a></li>"
        Dim html As String = String.Empty
        Try
            If Not ds Is Nothing AndAlso ds.Tables.Count > 0 Then
                If Not ds.Tables(0) Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                    For i As Int16 = 0 To ds.Tables(0).Rows.Count - 1
                        Dim dr As DataRow = ds.Tables(0).Rows(i)
                        Dim URLCode As String = dr("URLCode")
                        Dim DepartmentId As Integer = CInt(dr("DepartmentId"))
                        Dim mainUrl As String = URLParameters.MainDepartmentUrl(URLCode, DepartmentId)
                        Dim ElementId As String = IIf(dr("isQuickOrder"), Utility.Common.DepartmentType.nailcollection, Utility.Common.DepartmentType.nailsupplies) & "-" & URLCode & "-" & DepartmentId
                        Dim mainName As String = IIf(Not IsDBNull(dr("AlternateName")), dr("AlternateName"), dr("Name"))

                        'Build HTML cho menu sub cua department cho main department
                        html &= String.Format(lnkDepartment, ElementId, mainUrl, mainName)
                    Next
                End If
            End If
            html = "<ul class=""main-dept"">" & html & "</ul>"
            ltrListDepartment.Text = html
        Catch ex As Exception
            Email.SendError("ToError500", "header.ascx _BuildMainDepartment", ex.ToString())
        Finally
            ds.Dispose()
        End Try
    End Sub

    ''tao HTML menu sub department
    Private Function BuildChildDepartment(ByVal ParentId As Integer, ByVal URLCode As String, ByVal mainName As String, ByVal ElementId As String) As String

        Dim dsLevel1 As DataSet = StoreDepartmentRow.GetChildDepartmentByParentId(ParentId)
        Dim mainURL As String = URLParameters.MainDepartmentUrl(URLCode, ParentId)
        Dim url As String = String.Empty
        Dim name As String = String.Empty
        Dim result As String = String.Empty
        result = "<div id=""sub-" & ElementId & """ class=""sub-menu-dept"">"
        If (Not dsLevel1 Is Nothing) Then
            If (dsLevel1.Tables(0).Rows.Count > 0) Then
                result = result & "<ul>"
                For i As Integer = 0 To dsLevel1.Tables(0).Rows.Count - 1
                    URLCode = dsLevel1.Tables(0).Rows(i)("URLCode")
                    name = dsLevel1.Tables(0).Rows(i)("Name")
                    url = URLParameters.DepartmentUrl(URLCode, dsLevel1.Tables(0).Rows(i)("DepartmentId"))
                    result = result & "<li><a href='" & Server.HtmlEncode(url) & "'>" & Server.HtmlEncode(name) & "</a>"
                    result = result & "</li>"
                Next
                result = result & "</ul>"
            End If

            result = result & "<ul class=""promotion""><li class='title'>Featured Deals</li>"
            Dim tabNew As String = String.Empty
            Dim listTab As DepartmentTabCollection = DepartmentTabRow.FrontEndListByDepartmentId(DB, ParentId)
            If Not (listTab Is Nothing) Then
                Dim tabUrl As String
                For Each tab As DepartmentTabRow In listTab
                    tabUrl = mainURL & "/" & tab.URLCode
                    If Not (tab.URLCode.Equals("whats-new") Or tab.URLCode.Equals("what's-new")) Then
                        result = result & "<li><a href='" & Server.HtmlEncode(tabUrl) & "'>" & Server.HtmlEncode(tab.Name) & "</a></li>"
                    Else
                        tabNew = "<li><a href=""" & Server.HtmlEncode(tabUrl) & """>" & Server.HtmlEncode(tab.Name) & "</a></li>"
                    End If

                Next
            End If
            result = result & "<li class='title'>More Promotions</li>"
            Dim promotionURL As String = URLParameters.PromotionUrl(URLCode, ParentId)

            result = result & "<li><a href='" & Server.HtmlEncode(promotionURL) & "'>" & Server.HtmlEncode(mainName) & " on sale</a></li>"
            result = result & tabNew

            ''get image banner
            Dim lstBannerData As BannerCollection = BannerRow.ListByDepartmentId(ParentId)
            If Not lstBannerData Is Nothing Then
                If lstBannerData.Count > 0 Then
                    Dim path As String = Server.MapPath(Utility.ConfigData.PathBanner & "/small/")
                    For Each banner As BannerRow In lstBannerData
                        If System.IO.File.Exists(path & banner.BannerName) Then
                            result = result & "<li class=""img-banner""><a href='" & Server.HtmlEncode(banner.Url) & "' title='' target='_self'><img alt=""" & banner.BannerName & """ src=""" & Utility.ConfigData.PathBanner & "/small/" & banner.BannerName & """ /></a></li>"
                            Exit For
                        End If
                    Next

                End If
            End If

            result &= "</ul>"
        End If
        result &= "</div>"
        Return result
    End Function
    ''load menu Deals Center voi ShopNow, SaveNow, SaleCategory da check isactive = true va ko co item
    Private Sub LoadDealsCenterMenu()
        'rptShopNow.DataSource = ShopSaveRow.ListByType(DB, 1,, Convert.ToInt32(True)) '1: Shop Now
        Dim ss As ShopSaveCollection = ShopSaveRow.ListShopSave(DB, "top 7") '1,2: Shop Now & save now
        TotalRecords = ss.TotalRecords
        rptShopNow.DataSource = ss
        rptShopNow.DataBind()
        If rptShopNow.Items.Count = 0 Then
            rptShopNow.DataSource = Nothing
            rptShopNow.DataBind()
        End If

        'rptSaveNow.DataSource = ShopSaveRow.ListByType(DB, 2, Convert.ToInt32(True)) '2: Save Now
        'rptSaveNow.DataBind()
        'If rptSaveNow.Items.Count = 0 Then
        '    rptSaveNow.DataSource = Nothing
        '    rptSaveNow.DataBind()
        'End If

        'Dim dsSaleCategory As DataSet = SalesCategoryRow.GetCategoriesForMenu()
        'If Not dsSaleCategory Is Nothing AndAlso dsSaleCategory.Tables(0).Rows.Count > 1 Then
        '    rptSaleCategory.DataSource = dsSaleCategory
        '    rptSaleCategory.DataBind()
        'End If
    End Sub
    'Protected Sub rptSaveNow_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptSaveNow.ItemDataBound
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        Dim ltrLink As Literal = CType(e.Item.FindControl("ltrLink"), Literal)
    '        Dim tab As ShopSaveRow = e.Item.DataItem
    '        If tab.Url = String.Empty Then
    '            ltrLink.Text = "<a href=""/save-now/" & HttpUtility.UrlEncode(RewriteUrl.ReplaceUrl(tab.Name.ToLower())) & "/" & tab.ShopSaveId & """>" & tab.Name & "</a>"
    '        Else
    '            ltrLink.Text = "<a href=""" & tab.Url & """>" & tab.Name & "</a>"
    '        End If
    '    End If
    'End Sub

    Protected Sub rptShopNow_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptShopNow.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim ltrLink As Literal = CType(e.Item.FindControl("ltrLink"), Literal)
            Dim tab As ShopSaveRow = e.Item.DataItem
            If tab.Url = String.Empty Then
                ltrLink.Text = "<a href=""/shop-now/" & HttpUtility.UrlEncode(RewriteUrl.ReplaceUrl(tab.Name.ToLower())) & "/" & tab.ShopSaveId & """>" & imgShopsave & "<p>" & tab.Name & "</p></a>"
            Else
                ltrLink.Text = "<a href=""" & tab.Url & """>" & imgShopsave & "<p>" & tab.Name & "</p></a>"
            End If
            ltrLink.Text = String.Format(ltrLink.Text, tab.HomeBanner, tab.Name)
        End If
    End Sub
    'Protected Sub rptSaleCategory_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptSaleCategory.ItemDataBound
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        Dim ltrLink As Literal = CType(e.Item.FindControl("ltrLink"), Literal)
    '        Dim SalesCategoryId As Integer = IIf(e.Item.DataItem("DepartmentId") Is Nothing, 0, CInt(e.Item.DataItem("DepartmentId")))
    '        Dim urlCode As String = IIf(e.Item.DataItem("URLCode") Is Nothing, String.Empty, e.Item.DataItem("URLCode"))
    '        Dim cateName As String = IIf(e.Item.DataItem("Name") Is Nothing, String.Empty, e.Item.DataItem("Name"))
    '        ltrLink.Text = "<a href=""" & URLParameters.SalesUrl(urlCode, SalesCategoryId) & """>" & cateName & "</a>"
    '    End If
    'End Sub
    Private Function SelectedDepartmentCode() As String

        Dim result As String = String.Empty
        If (Not Request.QueryString("DepartmentURLCode") Is Nothing) Then
            Session("DepartmentURLCode") = Request.QueryString("DepartmentURLCode")
        Else ''get by ItemID
            Dim itemCode As String = Request("ItemCode")
            If Session("DepartmentURLCode") Is Nothing Then ''get default department
                If itemCode <> "" And Not itemCode Is Nothing Then
                    Dim path As String = StoreDepartmentRow.GetDepartmentPathByURLCode(itemCode, Nothing)
                    Session("DepartmentURLCode") = GetLastDepartmentCode(path)
                End If
            Else
                ''kiem tra item hien tai phai thuoc Category chua trong session, neu ko thi load lai category moi theo item nay
                If Not StoreDepartmentRow.IsItemIncategory(DB, Session("DepartmentURLCode"), itemCode) Then
                    Dim path As String = StoreDepartmentRow.GetDepartmentPathByURLCode(itemCode, Nothing)
                    Session("DepartmentURLCode") = GetLastDepartmentCode(path)
                End If
            End If
        End If
        result = Session("DepartmentURLCode")
        Return result
    End Function
    Private Function GetLastDepartmentCode(ByVal path As String) As String
        If path Is Nothing Or path = "" Then
            Return Nothing
        End If
        Dim departmentCode As String = ""
        If path.Contains("/") Then
            Dim indexChar As Integer = path.LastIndexOf("/")
            If indexChar > 0 Then
                departmentCode = path.Substring(indexChar + 1)
            Else
                departmentCode = path
            End If
        Else
            departmentCode = path
        End If
        ''Dim department As StoreDepartmentRow = StoreDepartmentRow.GetRow(DB, departmentID)
        Return departmentCode
    End Function

    Private Sub LoadShopbyDesignMenu()
        'Dim arr As New List(Of Integer)
        Dim lstparent As List(Of CategoryRow) = CategoryRow.ListByItem("ParentId=0 AND Type=" & Utility.Common.CategoryType.ShopDesign)
        If lstparent IsNot Nothing AndAlso lstparent.Count > 0 Then
            ltrCategory.Text &= "<div class='popover bottom submenu shop-design'><span class='no-bg'></span> <div class='arrow'></div><div class='popover-content'><ul>"
            For i As Integer = 0 To lstparent.Count() - 1
                ltrCategory.Text &= "<li><a href='" & URLParameters.ShopDesignListUrl(lstparent(i).CategoryName, lstparent(i).CategoryId) & "'>" & lstparent(i).CategoryName & "</a></li>"
                'If (Not IsExistCategoryId(arr, lstChildcategory(i).ParentId)) Then
                '    arr.Add(lstChildcategory(i).ParentId)
                '    Dim CategoryName As String = CategoryRow.GetCategoryNameByCategoryId(lstChildcategory(i).ParentId)
                '    ltrCategory.Text &= IIf(i = 0, "<ul>", "</ul><ul>")
                '    ltrCategory.Text &= "<li class='title'><a href='" & URLParameters.ShopDesignListUrl(CategoryName, lstChildcategory(i).ParentId) & "'>" & CategoryName & "</a></li>"
                '    ltrCategory.Text &= "<li><a href='" & URLParameters.ShopDesignListUrl(lstChildcategory(i).CategoryName, lstChildcategory(i).CategoryId) & "'>" & lstChildcategory(i).CategoryName & "</a></li>"
                'Else
                '    ltrCategory.Text &= "<li><a href='" & URLParameters.ShopDesignListUrl(lstChildcategory(i).CategoryName, lstChildcategory(i).CategoryId) & "'>" & lstChildcategory(i).CategoryName & "</a></li>"
                'End If
            Next
            ltrCategory.Text &= "</ul></div></div>"
        End If
    End Sub

    Private Function IsExistCategoryId(ByVal list As List(Of Integer), ByVal ParentId As Integer) As Boolean
        For i As Integer = 0 To list.Count() - 1
            If (list(i) = ParentId) Then
                Return True
            End If
        Next
        Return False
    End Function
End Class
