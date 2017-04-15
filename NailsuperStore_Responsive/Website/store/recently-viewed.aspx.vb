Imports Components
Imports DataLayer

Partial Class Store_RecentlyViewed
    Inherits SitePage

    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindData()
        End If
    End Sub

    Private Sub BindData()
        Dim OrderId As Integer = 0
        Dim CookieOrderId As Integer = 0
        Try
            CookieOrderId = Utility.Common.GetOrderIdFromCartCookie()
            If CookieOrderId <> 0 Then OrderId = CookieOrderId
            If Session("OrderId") <> Nothing Then OrderId = Session("OrderId")
        Catch ex As Exception

        End Try

        ucListProduct.MemberId = Utility.Common.GetCurrentMemberId()
        ucListProduct.OrderId = OrderId
        ucListProduct.SessionId = Session.SessionID
    End Sub
End Class