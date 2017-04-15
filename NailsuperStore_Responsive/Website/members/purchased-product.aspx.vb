Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Collections.Generic
Partial Class members_purchased_product
    Inherits SitePage
    Private Shared OrderCollection As StoreOrderCollection
    Protected Shared TotalRecords As Integer = 0
    Protected Shared PageSize As Integer = Utility.ConfigData.ScrollPurchaseProduct
    Private Shared pgIndex As Integer = 0
    Private Shared MemberId As Integer = 0
    Private Shared isInternational As Boolean = False
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not HasAccess() Then
            DB.Close()
            Response.Redirect("/members/login.aspx")
        End If
        ' Dim dbMember As MemberRow = MemberRow.GetRow(Session("memberId"))
        MemberId = Utility.Common.GetCurrentMemberId()
        Dim cOrderId As Integer = 0
        Dim CookieOrderId As Integer = Utility.Common.GetOrderIdFromCartCookie()
        If CookieOrderId <> 0 Then cOrderId = CookieOrderId
        If Session("OrderId") <> Nothing Then cOrderId = Session("OrderId")
        If MemberId > 0 Then
            isInternational = MemberRow.CheckMemberIsInternational(MemberId, cOrderId)
        Else
            isInternational = False
        End If
        'ltlPageTitle.Text = "My Order History"
        'ltlMemberNavigation.Text = MemberRow.MemberNavigationString
        OrderCollection = MemberRow.GetMemberPurchasedProduct(PageSize, 1, MemberId, TotalRecords)
        rpListOrder.DataSource = OrderCollection
        rpListOrder.DataBind()
    End Sub

    Protected Sub rpListOrder_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rpListOrder.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim o As StoreOrderRow = e.Item.DataItem
            Dim ucOrder As controls_product_purchased
            ucOrder = CType(e.Item.FindControl("ucOrder"), controls_product_purchased)
            ucOrder.Fill(o.OrderId, o.itemindex, True, isInternational)
        End If
    End Sub

    Public Shared Function GetXmlData(OrderCollection As StoreOrderCollection, TotalRecords As Integer, ByVal internationnal As Boolean) As String
        ' ItemIndex = StoreItemRow.counter
        Dim xmlData As String = String.Empty
        Dim controlPath As String = "~/controls/product/purchased.ascx"
        Dim pageHolder As New Page()
        Dim ucOrder As controls_product_purchased = DirectCast(pageHolder.LoadControl(controlPath), controls_product_purchased)
        If OrderCollection.Count > 0 Then
            xmlData = BindList(OrderCollection, TotalRecords, ucOrder, internationnal) 'BindList(ItemsCollection, Countdata, ucproduct)
            OrderCollection = Nothing
        End If
        Return xmlData
    End Function
    Public Shared Function BindList(OrderCollection As StoreOrderCollection, ByVal countData As Integer, ByVal uc As Object, ByVal internationnal As Boolean) As String
        Dim pageHolder As New Page()
        Dim strXmlData As String = ""
        strXmlData = "<Data>"
        For i As Integer = 0 To OrderCollection.Count - 1
            Dim o As New StoreOrderRow
            o = OrderCollection(i)
            uc.Fill(o.OrderId, o.itemindex, False, internationnal)
            pageHolder.Controls.Add(uc)
            Dim output As New System.IO.StringWriter()
            HttpContext.Current.Server.Execute(pageHolder, output, False)
            strXmlData += vbCrLf & "<Items>"
            strXmlData += StoreItemRow.SetXMLtag("content", output.ToString, True)
            strXmlData += StoreItemRow.SetXMLtag("RowNum", o.itemindex, True)
            strXmlData += "</Items>"
        Next
        strXmlData += vbCrLf & "<PageCount>" & vbCrLf & "<PageCount>" & countData & "</PageCount>" & vbCrLf & "</PageCount>" & vbCrLf & "</Data>"
        Return strXmlData
    End Function
    <WebMethod()> _
    Public Shared Function GetData(ByVal pageIndex As Integer, ByVal pageSize As Integer) As String
        ' pageIndex = pageIndex + Utility.ConfigData.ScrollPurchaseProduct - pageSize  '((pageIndex * pageSize) - pageSize) + 1 + (Utility.ConfigData.ScrollPurchaseProduct - pageSize)
        pageSize = pageSize
        pgIndex = pageIndex
        MemberId = HttpContext.Current.Session("MemberId")
        Dim dbMember As MemberRow = MemberRow.GetRow(MemberId)
        isInternational = dbMember.IsInternational
        Dim xmlData As String = ""
        Dim ucOrder As New controls_product_purchased
        OrderCollection = New StoreOrderCollection
        OrderCollection = MemberRow.GetMemberPurchasedProduct(pageSize, pgIndex, MemberId, TotalRecords)
        If OrderCollection.Count > 0 Then
            xmlData = GetXmlData(OrderCollection, TotalRecords, isInternational) 'StoreItemRow.BindList(ItemsCollection, ItemsCollectionCount, ucproduct, isInternational) 'BindList(ItemsCollection, Countdata, ucproduct)
        End If
        Return xmlData
    End Function
End Class
