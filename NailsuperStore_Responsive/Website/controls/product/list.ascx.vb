Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Collections.Generic
Imports Newtonsoft.Json
Imports Utility.Common
Imports System.Web.Script.Services

Partial Class Controls_ProductList
    Inherits BaseControl
    'Sub Category/Collection/Promotion/Brand
    Public DepartmentId As Integer
    Public BrandId As Integer
    Public ShopSaveId As Integer
    Public DepartmentTabId As Integer

    'Related Item
    Public RelatedItemId As Integer
    Public ItemGroupId As Integer
    Public VideoId As Integer

    'RecentlyView
    Public OrderId As Integer
    Public MemberId As Integer
    Public SessionId As String

    Protected lstName As String = ""
    Protected strPath As String = ""
    Protected queryString As String = ""

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            queryString = "0"
            If Request.RawUrl.Contains("?") Then
                queryString = Request.RawUrl.Substring(Request.RawUrl.LastIndexOf("?") + 1)
            End If

            If RelatedItemId > 0 Or VideoId > 0 Then
                litParam.Text = String.Format("pageIndex:1, pageSize:99, ItemId:{0}, ItemGroupId:{1}, VideoId:{2}, listheight:0", RelatedItemId, ItemGroupId, VideoId)
            ElseIf OrderId > 0 Or Not String.IsNullOrEmpty(SessionId) Then
                litParam.Text = String.Format("pageIndex:1, pageSize:99, OrderId:{0}, MemberId:{1}, SessionId:""{2}"", listheight:0", OrderId, MemberId, SessionId)
            Else
                If Not String.IsNullOrEmpty(GetQueryString("DepartmentId")) Then
                    Dim strDepartmentId = GetQueryString("DepartmentId")
                    If strDepartmentId.Contains("-") Then
                        strDepartmentId = strDepartmentId.Substring(0, strDepartmentId.IndexOf("-"))
                    End If
                    strDepartmentId = strDepartmentId.Replace("'", "").Replace("""", "")
                    DepartmentId = CInt(strDepartmentId)
                ElseIf Not String.IsNullOrEmpty(GetQueryString("ShopSaveId")) Then
                    ShopSaveId = CInt(GetQueryString("ShopSaveId"))
                End If

                litParam.Text = String.Format("pageIndex:1, pageSize:12, queryString:""{0}"", DepartmentId:{1}, DepartmentTabId:{2}, ShopSaveId:{3}, BrandId:{4}, listheight:0", queryString.Replace(",", "|"), DepartmentId, DepartmentTabId, ShopSaveId, BrandId)
                If Request.RawUrl.Contains("/promotion") Then
                    If Not Request.RawUrl.Contains("/" & ShopSaveId.ToString()) Then
                        Email.SendError("ToError500", "Promotion shows wrong list product", "RawUrl: " & Request.RawUrl.ToString() & "<br>ShopSaveId: " & ShopSaveId.ToString() & "<br>QueryString: " & GetQueryString("ShopSaveId"))
                    End If
                End If
            End If

            strPath = Request.Path & "/"
            If RelatedItemId > 0 Or VideoId > 0 Then
                strPath &= "GetRelatedProduct"
            ElseIf MemberId > 0 Or OrderId > 0 Or Not String.IsNullOrEmpty(SessionId) Then
                strPath &= "GetRecentlyView"
            Else
                strPath &= "GetProductList"
            End If

            Dim isQuickOrder As Boolean = HttpContext.Current.Request.Path.Contains("collection.aspx")
            If isQuickOrder Then
                lstName = "c"
            End If

            'update log
            If Not (Request.RawUrl.Contains("/nail-products/")) Then
                ViewedItemRow.updateSearchResult(String.Empty, Request.RawUrl, "List")
            End If

        End If
    End Sub

End Class
