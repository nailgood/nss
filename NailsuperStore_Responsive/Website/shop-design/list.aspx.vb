Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Collections.Generic
Imports Utility.Common
Partial Class shop_design_list
    Inherits SitePage

    Dim arrCateID As New ArrayList

    Protected index As Integer = 0
    Protected Shared countPageSize As Integer = 0
    Protected Shared TotalRecords As Integer
    Dim PageIndex As Integer = 1
    Protected Shared PageSize As Integer = 9 ' Utility.ConfigData.PageSizeScroll
    Private Shared result As New List(Of ShopDesignRow)
    Public Shared hidShopDesignIndex As String
    Public Property CategoryId() As Integer
        Get
            Return ViewState("CategoryId")
        End Get
        Set(ByVal value As Integer)
            ViewState("CategoryId") = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Request.QueryString("cateId") Is Nothing AndAlso IsNumeric(Request.QueryString("cateId")) Then
            CategoryId = CInt(Request.QueryString("cateId"))
        End If

        If Not IsPostBack Then
            LoadCategory()
            LoadShopDesign()
        End If
    End Sub

    Private Sub LoadCategory()
        Dim row As CategoryRow = CategoryRow.GetRow(DB, CategoryId)
        If row IsNot Nothing AndAlso row.CategoryId > 0 Then
            ltrTitle.Text = row.CategoryName
            Utility.Common.CheckValidURLCode(URLParameters.ReplaceUrl(HttpUtility.UrlEncode(row.CategoryName.ToLower())))
            Dim objMetaTag As New MetaTag

            If (row.PageTitle <> "") Then
                objMetaTag.PageTitle = row.PageTitle
            End If
            If row.MetaDescription <> "" Then
                objMetaTag.MetaDescription = row.MetaDescription
            End If
            If row.MetaKeyword <> "" Then
                objMetaTag.MetaKeywords = row.MetaKeyword
            End If
            SetPageMetaSocialNetwork(Page, objMetaTag)
        Else
            ltrTitle.Text = "Shop by Design"
        End If
    End Sub

    Private Sub LoadShopDesign()
        Dim log As String = String.Empty
        Try
            result = ShopDesignRow.ListTop3ByCategoryID(CategoryId)
            log &= "1-"
            Dim data As New ShopDesignRow()
            log &= "2-"
            data.PageIndex = 1
            data.PageSize = PageSize
            data.CategoryId = CategoryId

            log &= "3-"
            If result Is Nothing Then
                log &= "4-"
                result = ShopDesignRow.ListByCategoryID(data)
                log &= "5-"
                TotalRecords = data.TotalRow
                log &= "6-"
            Else
                log &= "7-"
                TotalRecords = 0
            End If
            log &= "8-"
            rptlist.DataSource = result
            rptlist.DataBind()
            log &= "9-"
            hidShopDesignIndex = PageIndex & "," & result.Count
            log &= "10-"
        Catch ex As Exception
            Email.SendError("ToError500", "Page_Load >> LoadShopDesign", log & "<br>" & ex.ToString())
        End Try

    End Sub

    Private Shared Function IsExistCategory(ByVal lstdesign As ArrayList, ByVal parentID As Integer) As Boolean
        For Each item As String In lstdesign
            Dim str As String() = item.Split(",")
            If str(0) = parentID Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Shared Function IsExistShopDesign(ByVal lstdesign As ArrayList, ByVal ID As Integer, ByVal parentID As Integer) As Boolean
        For Each item As String In lstdesign
            Dim str As String() = item.Split(",")
            If str(0) = parentID AndAlso str(1) = ID Then
                Return True
            End If
        Next
        Return False
    End Function

    Protected Sub rplist_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptlist.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            countPageSize = (PageIndex - 1) * PageSize
            Dim item As ShopDesignRow = e.Item.DataItem
            Dim ltrCategory As Literal = e.Item.FindControl("ltrCategory")
            Dim divShopDesign As Literal = e.Item.FindControl("divShopDesign")

            If CategoryId = 0 AndAlso Not (IsExistShopDesign(arrCateID, item.ShopDesignId, item.CategoryId)) Then
                If Not IsExistCategory(arrCateID, item.CategoryId) Then
                    Dim cate As CategoryRow = CategoryRow.GetRow(DB, item.CategoryId)
                    ltrCategory.Text = "<a class='subcate_sDesign' href='" & URLParameters.ShopDesignListUrl(cate.CategoryName, cate.CategoryId) & "'>" & cate.CategoryName & "</a>"
                    index = 1
                Else
                    index = index + 1
                End If
                arrCateID.Add(item.CategoryId & "," & item.ShopDesignId)
            ElseIf CategoryId > 0 AndAlso item.ParentId = CategoryId AndAlso Not (IsExistShopDesign(arrCateID, item.ShopDesignId, item.CategoryId)) Then
                If Not (IsExistCategory(arrCateID, item.CategoryId)) Then
                    ltrCategory.Text = "<a class='subcate_sDesign' href='" & URLParameters.ShopDesignListUrl(item.CategoryName, item.CategoryId) & "'>" & item.CategoryName & "</a>"
                    index = 1
                Else
                    index = index + 1
                End If
                arrCateID.Add(item.CategoryId & "," & item.ShopDesignId)
            ElseIf CategoryId = 0 AndAlso item.CategoryId <> CategoryId Then
                Exit Sub
            Else
                index = index + 1
            End If
            Dim ucShopDesign As controls_shop_design = e.Item.FindControl("dvShopDesign")
            'ucShopDesign.Fill(item, index)
            ucShopDesign.ShopDesign = item
            ucShopDesign.index = index
        End If
    End Sub

    <WebMethod()> _
    Public Shared Function GetDataVideo(ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal categoryId As Integer) As String
        Dim xmlData As String = ""
        result = New List(Of ShopDesignRow)
        Dim data As New ShopDesignRow
        data.PageIndex = pageIndex
        data.PageSize = pageSize
        data.CategoryId = categoryId
        countPageSize = (pageIndex - 1) * pageSize
        result = ShopDesignRow.ListByCategoryID(data)
        Dim htmlShopDesign As String = String.Empty
        If result.Count > 0 Then
            TotalRecords = data.TotalRow
            'xmlData = GetXmlData(result, TotalRecords)
            For i As Integer = 0 To result.Count - 1
                HttpContext.Current.Session("indexRender") = countPageSize + i + 1
                HttpContext.Current.Session("shopDesignRender") = result(i)
                htmlShopDesign &= Utility.Common.RenderUserControl("~/controls/shop-design.ascx")
                HttpContext.Current.Session("indexRender") = Nothing
                HttpContext.Current.Session("shopDesignRender") = Nothing
            Next
        End If
        htmlShopDesign &= "<input type='hidden' id='nexthidVideoIndex' clientidmode='Static' value='" & countPageSize + 1 & "' />"
        Return htmlShopDesign
    End Function

End Class
