Imports Components
Imports DataLayer
Partial Class store_TrackingOrderId
    Inherits SitePage
    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Dim cookieOrderId As Integer = Utility.Common.GetOrderIdFromCartCookie()
            Dim lastOrderId As Integer = -1
            Dim memberId As Integer = -1
            If Not Session("MemberId") Is Nothing Then
                memberId = Session("MemberId")
                lastOrderId = DB.ExecuteScalar("Select LastOrderId from Member where MemberId=" & memberId)
                ltrData.Text = "MemberId:=" & memberId & "-cookieOrderId:=" & cookieOrderId & "-Last Order Id:=" & lastOrderId
            Else
                memberId = Utility.Common.GetMemberIdFromCartCookie()
                ltrData.Text = "Not login-cookieOrderId:=" & cookieOrderId & "-member cookie id=" & memberId
            End If
        End If
    End Sub
End Class
