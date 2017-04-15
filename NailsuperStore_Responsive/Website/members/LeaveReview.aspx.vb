Imports Components
Imports DataLayer
Imports System.Text
Imports System.IO
Imports Utility
Imports System.Data.SqlClient
Imports System
Partial Class members_LeaveReview
    Inherits SitePage
    Protected LiftGateService As Double
    Protected InsideDeliveryService As Double

    Public weebRoot As String = System.Configuration.ConfigurationManager.AppSettings("GlobalRefererName")
    Public MemberId As Integer
    Protected Shared TotalRecords As Integer = 0
    Protected CountRecord As Integer = 0
    Protected Shared PageSize As Integer = Utility.ConfigData.ScrollPurchaseProduct
    Private lDay As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not HasAccess() Then
            Response.Redirect("/members/login.aspx")
        End If
        LiftGateService = SysParam.GetValue("LiftGateService")
        InsideDeliveryService = SysParam.GetValue("InsideDeliveryService")

        If Not Page.IsPostBack Then
            lDay = ddTopRecord.SelectedValue
            LoadFromDb(True)
        End If
    End Sub
    Private Sub LoadFromDb(firstload As Boolean)
        Dim ItemsCollection As StoreOrderCollection = StoreOrderRow.GetOrderReview(Session("MemberId"), ConfigData.ReviewDay, lDay, TotalRecords)
        CountRecord = ItemsCollection.Count
        'LoadTopRecord(TotalRecords)
        If TotalRecords > 0 And ItemsCollection.Count = 0 And firstload = True Then
            ItemsCollection = StoreOrderRow.GetOrderReview(Session("MemberId"), ConfigData.ReviewDay, Int32.MaxValue, TotalRecords)
            ddTopRecord.SelectedValue = Int32.MaxValue
            CountRecord = TotalRecords
        End If
        rptOrder.DataSource = ItemsCollection
        rptOrder.DataBind()
    End Sub
    Private Sub LoadTopRecord(count As Integer)
        If count > PageSize Then
            Dim LimitRecord As Integer = Math.Floor(count / PageSize)
            For i As Integer = 0 To LimitRecord - 1
                ddTopRecord.Items.Insert(i, New ListItem(SitePage.NumberToString((i + 1) * PageSize)))
            Next
            If LimitRecord * PageSize < count Then
                ddTopRecord.Items.Insert(LimitRecord, New ListItem(SitePage.NumberToString(count)))
            End If
        Else
            Filter.Visible = False
        End If
    End Sub
    Private currentOrderLoop As StoreOrderRow
    Private Sub rptOrder_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptOrder.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            currentOrderLoop = e.Item.DataItem
            Dim ucOrder As controls_product_purchased
            ucOrder = CType(e.Item.FindControl("ucOrder"), controls_product_purchased)
            ucOrder.isReview = currentOrderLoop.IsReview
            ucOrder.isShowReviewCart = currentOrderLoop.IsShowReviewCart
            ucOrder.isProductReview = True
            ucOrder.Fill(currentOrderLoop.OrderId, currentOrderLoop.itemindex, True, False)
        End If
    End Sub
    Protected Sub ddTopRecord_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddTopRecord.SelectedIndexChanged
        lDay = ddTopRecord.SelectedValue
        LoadFromDb(False)
    End Sub
End Class
