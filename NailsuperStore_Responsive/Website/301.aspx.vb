Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Partial Class _301
    Inherits SitePage
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadData()
    End Sub
    Private Sub LoadData()
        Dim ItemId As String = String.Empty
        Dim ItemURLCode As String = String.Empty
        Dim DepartmentId As String = String.Empty
        Dim DepartmentURLCode As String = String.Empty
        Dim DepartmentTabURLCode As String = String.Empty
        Dim BrandId As String = String.Empty
        Dim F_Promotion As String = String.Empty
        Dim SaleID As String = String.Empty
        Dim Type As String = String.Empty
        Dim page As String = String.Empty
        Dim sort As String = String.Empty
        Dim brand As String = String.Empty
        Dim price As String = String.Empty
        Dim rating As String = String.Empty
        Dim ID As String = String.Empty
        Dim ratingReview As String = String.Empty
        Dim OrderId As Integer = 0
        Dim email As String = String.Empty
        Dim reviewId As String = String.Empty
        Dim TipId As String = String.Empty
        Dim act As String = String.Empty
        Try
            ItemId = Request.QueryString("ItemId")
            If Not String.IsNullOrEmpty(ItemId) Then
                ItemId = ItemId.Replace("301,", "")
                If ItemId.Contains("x") Then
                    ItemId = DepartmentId.Substring(0, ItemId.IndexOf("x"))
                End If
            End If
            ItemURLCode = Request.QueryString("ItemURLCode")
            If String.IsNullOrEmpty(ItemURLCode) Then
                ItemURLCode = Request.QueryString("itemcode")
            End If
            DepartmentId = Request.QueryString("DepartmentId")
            If Not String.IsNullOrEmpty(DepartmentId) Then
                DepartmentId = DepartmentId.Replace("301,", "")
                DepartmentId = DepartmentId.Replace(".aspx", "")
                If DepartmentId.Contains("-") Then
                    DepartmentId = DepartmentId.Substring(0, DepartmentId.IndexOf("-"))
                End If
            End If

            DepartmentURLCode = Request.QueryString("DepartmentURLCode")
            If Not String.IsNullOrEmpty(DepartmentURLCode) Then
                DepartmentURLCode = DepartmentURLCode.Replace("301,", "")
                DepartmentURLCode = DepartmentURLCode.Replace(" ", "-")
                DepartmentURLCode = DepartmentURLCode.Replace("%20", "-")
            End If

            DepartmentTabURLCode = Request.QueryString("DepartmentTabURLCode")
            If String.IsNullOrEmpty(DepartmentTabURLCode) Then
                DepartmentTabURLCode = Request.QueryString("taburlcode")
            End If
            sort = Request.QueryString("sort")
            brand = Request.QueryString("brand")
            price = Request.QueryString("price")
            rating = Request.QueryString("rating")

            Type = Request.QueryString("Type")
            BrandId = Request.QueryString("brandid")
            If Not String.IsNullOrEmpty(BrandId) Then
                BrandId = BrandId.Replace("301,", "")
                BrandId = BrandId.Replace(".aspx", "")
            End If
            F_Promotion = Request.QueryString("F_Promotion")
            SaleID = Request.QueryString("saleID")
            If Not String.IsNullOrEmpty(SaleID) Then
                SaleID = SaleID.Replace("301,", "")
            End If
            ID = Request.QueryString("Id")
            ratingReview = Request.QueryString("ex")
            OrderId = Request.QueryString("OrderId")
            email = Request.QueryString("email")
            reviewId = Request.QueryString("ReviewId")
            TipId = Request.QueryString("TipId")
            page = Request.QueryString("page")
            act = Request.QueryString("act")
        Catch ex As Exception
        End Try

        Dim strURL As String = "/"
        If Not SaleID Is Nothing Then
            strURL = GoSale(SaleID)
        ElseIf Not String.IsNullOrEmpty(ID) AndAlso LCase(Request.RawUrl).ToString.Contains("orderreview.aspx") Then
            strURL = GoOrderReviewId(ID, ratingReview)
        ElseIf ItemId = "dropdown" Then
            If Session("ReferralURL") IsNot Nothing Then
                strURL = Session("ReferralURL")
            Else
                strURL = "/"
            End If
        ElseIf (Not String.IsNullOrEmpty(ID) Or ItemId > 0) AndAlso Request.RawUrl.Contains("review.aspx") Then
            strURL = IIf(Not String.IsNullOrEmpty(ID), GoReviewItemByID(ID), GoReviewItem(ItemId))
        ElseIf Not String.IsNullOrEmpty(ItemId) AndAlso IsNumeric(ItemId) Then
            strURL = GoProduct(ItemId)
        ElseIf Not String.IsNullOrEmpty(ItemURLCode) Then
            strURL = GoProductByURLCode(ItemURLCode)
        ElseIf BrandId > 0 Then
            strURL = GoBrand(BrandId)
        ElseIf Not String.IsNullOrEmpty(brand) Then
            strURL = GoBrandUrlCode(brand)
        ElseIf (DepartmentId > 0 Or Not String.IsNullOrEmpty(DepartmentURLCode)) And Type = "vb" And Not Request.RawUrl.Contains("?") Then
            strURL = GoDepartmentVB(DepartmentId, DepartmentURLCode)
        ElseIf Not String.IsNullOrEmpty(DepartmentURLCode) Then
            strURL = GoDepartmentURLCode(DepartmentURLCode, DepartmentTabURLCode, sort, brand, price, rating)
        ElseIf DepartmentId > 0 And F_Promotion <> "Y" Then
            strURL = GoDepartmentPD(DepartmentId)
        ElseIf DepartmentId > 0 And F_Promotion = "Y" Then
            strURL = GoPromotion(DepartmentId)
        ElseIf OrderId > 0 AndAlso Not String.IsNullOrEmpty(email) Then
            strURL = GoOrderReviewAccess(OrderId, email)
        ElseIf reviewId > 0 AndAlso Not String.IsNullOrEmpty(email) Then
            strURL = GoReviewAccess(reviewId, email, act)
        ElseIf TipId > 0 Then
            strURL = GoTipsDetail(TipId)
            'ElseIf (page = "spa-videos") Then
            '    strURL = "/spa-videos"
        ElseIf (page = "promotion-sales-price") Then
            strURL = "/deals-center"
        ElseIf (page <> "" AndAlso (page.Contains("catalog") Or page.Contains("sitemap") Or page.Contains("deals-center") Or page.Contains("contact/default"))) Then
            strURL = page
        ElseIf (page <> "") Then
            strURL = "/service/" & page
        Else
            strURL = "/"
        End If

        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.Status = "301 Moved Permanently"
        HttpContext.Current.Response.AddHeader("Location", strURL)
        HttpContext.Current.Response.End()
    End Sub

    Private Function GoSale(ByVal SaleID As String) As String

        Dim sURL As String = "/"
        Dim id As Integer = 0
        Try
            id = Convert.ToInt32(SaleID)
            Dim row As SalesCategoryRow = SalesCategoryRow.GetRow(DB, id)
            If row Is Nothing Or row.URLCode Is Nothing Or row.URLCode = "" Then
                sURL = "/nail-sales-promotion"
            Else
                sURL = URLParameters.SalesUrl(row.URLCode)
            End If


        Catch ex As Exception
            sURL = sURL + "nail-sales-promotion"
        End Try

        Return sURL
    End Function
    Private Function GoProductByURLCode(ByVal urlCode As String) As String
        Dim sURL As String = "/"
        Try
            Dim ItemId As Integer = StoreItemRow.GetByURLCode301(DB, urlCode)
            If ItemId > 0 Then
                sURL = URLParameters.ProductUrl(urlCode, ItemId)
            End If
        Catch ex As Exception

        End Try
        Return sURL
    End Function
    Private Function GoProduct(ByVal ItemId As String) As String
        Dim sURL As String = "/"
        Try
            Dim row As StoreItemRow = StoreItemRow.GetRow(DB, Convert.ToInt32(ItemId))

            If Not row Is Nothing Then
                If Not String.IsNullOrEmpty(row.URLCode) Then
                    sURL = URLParameters.ProductUrl(row.URLCode, row.ItemId)
                End If

            End If
        Catch ex As Exception

        End Try
        Return sURL
    End Function

    Private Function GoDepartmentPD(ByVal DepartmentId As String) As String
        Dim sURL As String = "/"
        Dim old As DepartmentOldRow = DepartmentOldRow.GetRow(DB, DepartmentId)

        If old.DepartmentNewId = Nothing Then
            Dim dep As StoreDepartmentRow = StoreDepartmentRow.GetRow(DB, DepartmentId)
            If dep.DepartmentId > 0 Then
                If dep.ParentId = 23 Then
                    sURL = URLParameters.MainDepartmentUrl(dep.URLCode, dep.DepartmentId)

                    If Request.QueryString("DepartmentTabId") IsNot Nothing AndAlso IsNumeric(Request.QueryString("DepartmentTabId")) Then
                        Dim deptab As DepartmentTabRow = DepartmentTabRow.GetRow(DB, CInt(Request.QueryString("DepartmentTabId")))
                        sURL &= "/" & deptab.URLCode
                    End If
                Else
                    sURL = URLParameters.DepartmentUrl(dep.URLCode, dep.DepartmentId)
                End If
            Else
                GoMsg("Sorry! Not found department that you want.")
            End If
        Else
            'Check main category or sub category
            Dim dep As StoreDepartmentRow = StoreDepartmentRow.GetRow(DB, old.DepartmentNewId)
            If dep.ParentId = 23 Then
                sURL = URLParameters.MainDepartmentUrl(dep.URLCode, dep.DepartmentId)
            Else
                sURL = URLParameters.DepartmentUrl(dep.URLCode, dep.DepartmentId)
            End If

        End If

        Return sURL
    End Function

    Private Function GoDepartmentVB(ByVal DepartmentId As String, ByVal DepartmentURLCode As String) As String
        Dim sURL As String = "/"
        If DepartmentId > 0 Then
            Dim dep As StoreDepartmentRow = StoreDepartmentRow.GetRow(DB, DepartmentId)
            If dep.ParentId = 23 Then
                sURL = URLParameters.MainDepartmentUrl(dep.URLCode, dep.DepartmentId)

                If Request.QueryString("DepartmentTabId") IsNot Nothing AndAlso IsNumeric(Request.QueryString("DepartmentTabId")) Then
                    Dim deptab As DepartmentTabRow = DepartmentTabRow.GetRow(DB, CInt(Request.QueryString("DepartmentTabId")))
                    sURL &= "/" & deptab.URLCode
                End If
            Else
                sURL = URLParameters.DepartmentUrl(dep.URLCode, dep.DepartmentId)
            End If
        Else
            Dim old As DepartmentOldRow = DepartmentOldRow.GetByURLCode(DB, DepartmentURLCode)
            If old.DepartmentNewId = Nothing Then
                GoMsg("Sorry! Not found department that you want.")
            Else
                'Check main category or sub category
                Dim dep As StoreDepartmentRow = StoreDepartmentRow.GetRow(DB, old.DepartmentNewId)
                If dep.ParentId = 23 Then
                    sURL = URLParameters.MainDepartmentUrl(dep.URLCode, dep.DepartmentId)
                Else
                    sURL = URLParameters.DepartmentUrl(dep.URLCode, dep.DepartmentId)
                End If
            End If
        End If
        Return sURL
    End Function

    Private Function GoDepartmentURLCode(ByVal DepartmentURLCode As String, ByVal DepartmentTabURLCode As String, ByVal paramValueSort As String, ByVal paramValueBrand As String, ByVal paramValuePrice As String, ByVal paramValueRating As String) As String
        Dim sURL As String = "/"
        Dim dep As StoreDepartmentRow = StoreDepartmentRow.GetByURLCode(DB, DepartmentURLCode)

        'If dep.ParentId = 23 Then
        '    sURL = URLParameters.MainDepartmentUrl(dep.URLCode)

        '    If Request.QueryString("DepartmentTabId") IsNot Nothing AndAlso IsNumeric(Request.QueryString("DepartmentTabId")) Then
        '        Dim deptab As DepartmentTabRow = DepartmentTabRow.GetRow(DB, CInt(Request.QueryString("DepartmentTabId")))
        '        sURL &= "/" & deptab.URLCode
        '    End If
        'Else
        If dep.DepartmentId < 1 Then
            Dim old As DepartmentOldRow = DepartmentOldRow.GetByURLCode(DB, DepartmentURLCode)

            If old.DepartmentNewId = 0 Then
                Return "/deals-center"
            End If

            dep = StoreDepartmentRow.GetRow(DB, old.DepartmentNewId)
        End If

        sURL = URLParameters.DepartmentUrl(dep.URLCode, dep.ParentId, dep.DepartmentId)
        If dep.DepartmentId = 0 Then
            sURL = "/deals-center"
        End If

        If Not String.IsNullOrEmpty(DepartmentTabURLCode) Then
            sURL = sURL & "/" & DepartmentTabURLCode
        End If
        If Not String.IsNullOrEmpty(paramValueBrand) Then
            Dim brandId = StoreBrandRow.GetIdByURLCode(paramValueBrand)
            sURL = GetLink(sURL, "brandid", brandId, False)
        End If
        If Not String.IsNullOrEmpty(paramValueSort) AndAlso (Not paramValueSort.Contains("product") And Not paramValueSort.Contains("on-sale")) Then
            sURL = GetLink(sURL, "sort", paramValueSort, False)
        End If
        If Not String.IsNullOrEmpty(paramValuePrice) Then
            sURL = GetLink(sURL, "price", paramValuePrice, False)
        End If
        If Not String.IsNullOrEmpty(paramValueRating) Then
            sURL = GetLink(sURL, "rating", paramValueRating, False)
        End If


        Return sURL
    End Function

    Private Function GetLink(ByVal url As String, ByVal paraName As String, ByVal paraValue As String, ByVal addEmptyValue As Boolean) As String
        url = URLParameters.AddParamaterToURL(url, paraName, paraValue, addEmptyValue)
        Return url
    End Function

    Private Function GoBrand(ByVal BrandId As String) As String
        Dim brand As StoreBrandRow = StoreBrandRow.GetRow(CInt(BrandId))
        Return URLParameters.BrandUrl(brand.URLCode, brand.BrandId)
    End Function

    Private Function GoBrandUrlCode(ByVal brand As String) As String
        Dim row As StoreBrandRow = StoreBrandRow.GetByURLCode(brand)
        Return URLParameters.BrandUrl(row.URLCode, row.BrandId)
    End Function

    Private Function GoPromotion(ByVal DepartmentId As String)
        Dim sURL As String = "/"
        Dim old As DepartmentOldRow = DepartmentOldRow.GetRow(DB, DepartmentId)
        If old.DepartmentNewId = Nothing Then
            GoMsg("Sorry! Not found department that you want.")
        Else
            'Check main category or sub category
            Dim dep As StoreDepartmentRow = StoreDepartmentRow.GetRow(DB, old.DepartmentNewId)
            If dep.ParentId = 23 Then
                sURL = URLParameters.PromotionUrl(old.Name, old.DepartmentNewId)
            End If

        End If

        Return sURL
    End Function

    Private Function GoOrderReviewId(ByVal OrderReviewId As String, ByVal ratingReview As String) As String
        Dim strURL As String = "/"
        ratingReview = Utility.Common.ConvertTextLevelOrderReviewToStartNumber(ratingReview)
        strURL = String.Format("/store/review/order-write.aspx?Id={0}&ex={1}", OrderReviewId, ratingReview)
        Return strURL
    End Function

    Private Function GoOrderReviewAccess(ByVal OrderId As Integer, ByVal email As String) As String
        Dim strURL As String = "/"
        strURL = String.Format("/store/review/order-access.aspx?OrderId={0}&email={1}", OrderId, email)
        Return strURL
    End Function

    Private Function GoReviewItem(ByVal reviewId As String) As String
        Dim strURL As String = "/"
        strURL = String.Format("/store/review/product-write.aspx?ItemId={0}", reviewId)
        Return strURL
    End Function

    Private Function GoReviewItemByID(ByVal reviewId As String) As String
        Dim strURL As String = "/"
        strURL = String.Format("/store/review/product-write.aspx?Id={0}", reviewId)
        Return strURL
    End Function

    Private Function GoReviewAccess(ByVal reviewId As String, ByVal email As String, ByVal act As String) As String
        Dim strURL As String = "/"
        strURL = String.Format("/store/review/product-access.aspx?reviewid={0}&act={1}&email={2}", reviewId, act, email)
        Return strURL
    End Function

    Private Function GoTipsDetail(ByVal TipId As String) As String
        Dim strURL As String = "/"
        strURL = String.Format("/tips/view.aspx?TipId={0}", TipId)
        Return strURL
    End Function
End Class
