Imports Components
Imports DataLayer
Imports System.Text
Imports System.IO
Imports Utility
Imports System.Web.Services
Imports System.Collections.Generic

Partial Class ReviewOrderList
    Inherits SitePage
    Private Shared filter As DepartmentFilterFields
    Private Shared OrderReviewsCollection As StoreOrderReviewCollection
    Private m_Item As StoreItemRow
    Protected TotalPages As Integer
    'Protected Const PageSize As Integer = 12
    Protected NoRecords As String = "<div style='padding:15px 0px 0px 15px'>There are no reviews for this item.</div>"
    Protected m_ItemId As Integer
    Protected strTemplate As String
    Protected Shared PageSize As Integer = Utility.ConfigData.PageSizeScroll
    Private Shared pgIndex As Integer = 1
    Protected Shared TotalRecords As Integer = 0

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindData()
        End If
    End Sub

    Public Sub BindData()
        filter = New DepartmentFilterFields()
        filter.pg = pgIndex
        filter.MaxPerPage = PageSize
        filter.ItemId = m_ItemId
        filter.RatingRange = GetQueryString("rating")
        'OrderReviewsCollection = StoreOrderReviewRow.GetOrderReview(DB, filter)
        OrderReviewsCollection = StoreOrderReviewRow.GetOrderReviewNarrowSearch(filter)
        If OrderReviewsCollection.Count > 0 Then
            TotalRecords = OrderReviewsCollection.TotalRecords
            rptOrderReview.DataSource = OrderReviewsCollection
            rptOrderReview.DataBind()
            '' End If
        Else
            rptOrderReview.Visible = False
            lblNoRecords.Visible = True
        End If
        Dim ltCanonical As Literal = Me.Master.FindControl("ltrCanonical")
        ltCanonical.Text = String.Format(Resources.Msg.Canonical, Utility.ConfigData.GlobalRefererName & Request.RawUrl.ToString().Split("?")(0))
    End Sub

    Sub rptOrderReview_DataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptOrderReview.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim sor As StoreOrderReviewRow = e.Item.DataItem
            Dim ucReview As controls_product_revieworder
            ucReview = CType(e.Item.FindControl("ucReview"), controls_product_revieworder)
            ucReview.Fill(sor, True)
        End If
    End Sub

    <WebMethod()> _
    Public Shared Function GetData(ByVal pageIndex As Integer, ByVal pageSize As Integer) As String
        ' pageIndex = pageIndex + Utility.ConfigData.ScrollPurchaseProduct - pageSize  '((pageIndex * pageSize) - pageSize) + 1 + (Utility.ConfigData.ScrollPurchaseProduct - pageSize)
        filter.MaxPerPage = pageSize
        filter.pg = pageIndex
        Dim xmlData As String = ""
        Dim ucReview As controls_product_revieworder
        OrderReviewsCollection = New StoreOrderReviewCollection
        OrderReviewsCollection = StoreOrderReviewRow.GetOrderReviewNarrowSearch(filter)
        If OrderReviewsCollection.Count > 0 Then
            TotalRecords = OrderReviewsCollection.TotalRecords
            xmlData = GetXmlData(OrderReviewsCollection, TotalRecords) 'StoreItemRow.BindList(ItemsCollection, ItemsCollectionCount, ucproduct, isInternational) 'BindList(ItemsCollection, Countdata, ucproduct)
        End If
        Return xmlData
    End Function

    Public Shared Function GetXmlData(ByVal OrderReviewsCollection As StoreOrderReviewCollection, ByVal TotalRecords As Integer) As String
        ' ItemIndex = StoreItemRow.counter
        Dim xmlData As String = String.Empty
        Dim controlPath As String = "~/controls/product/review-order.ascx"
        Dim pageHolder As New Page()
        Dim ucOrder As controls_product_revieworder = DirectCast(pageHolder.LoadControl(controlPath), controls_product_revieworder)
        If OrderReviewsCollection.Count > 0 Then
            xmlData = BindList(OrderReviewsCollection, TotalRecords, ucOrder) 'BindList(ItemsCollection, Countdata, ucproduct)
            OrderReviewsCollection = Nothing
        End If
        Return xmlData
    End Function

    Public Shared Function BindList(ByVal OrderReviewsCollection As StoreOrderReviewCollection, ByVal countData As Integer, ByVal uc As Object) As String
        Dim pageHolder As New Page()
        Dim strXmlData As String = ""
        strXmlData = "<Data>"
        For i As Integer = 0 To OrderReviewsCollection.Count - 1
            Dim o As New StoreOrderReviewRow
            o = OrderReviewsCollection(i)
            uc.Fill(o, False)
            pageHolder.Controls.Add(uc)
            Dim output As New System.IO.StringWriter()
            HttpContext.Current.Server.Execute(pageHolder, output, False)
            strXmlData += vbCrLf & "<Items>"
            strXmlData += StoreItemRow.SetXMLtag("content", output.ToString, True)
            strXmlData += StoreItemRow.SetXMLtag("RowNum", o.itemIndex, True)
            strXmlData += "</Items>"
        Next
        strXmlData += vbCrLf & "<PageCount>" & vbCrLf & "<PageCount>" & countData & "</PageCount>" & vbCrLf & "</PageCount>" & vbCrLf & "</Data>"
        Return strXmlData
    End Function
End Class

