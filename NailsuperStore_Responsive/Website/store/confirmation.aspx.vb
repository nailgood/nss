Imports Components
Imports DataLayer

Partial Class confirmation
    Inherits SitePage

    Private lstCartItem As StoreCartItemCollection

    Private _db As Database
    Private o As StoreOrderRow
    Private m_Member As MemberRow
    Protected LiftGateService As Double
    Protected InsideDeliveryService As Double
    Protected CODThreshold As Double
    Protected CODFee As Double
    Protected EbayOrder As Boolean = False
    Public linkTracking As String = ""
    Public linkEdit As String = ""
    Public formWidth As Integer = 754
    Public m_LoggedInName As String
    Protected OrderId As Integer = Nothing
    Protected dbOrder As StoreOrderRow
    Private strMemberId As String = String.Empty
    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Not GetQueryString("OrderId") Is Nothing Then
            Try
                OrderId = Convert.ToInt32(GetQueryString("OrderId"))
            Catch ex As Exception
                OrderId = Utility.Crypt.DecryptTripleDes(GetQueryString("OrderId"))
            End Try

            If OrderId > 0 Then
                dbOrder = StoreOrderRow.GetRow(DB, OrderId)
            End If
        End If

        If OrderId = Nothing Then
            Try
                Dim strOrderId As String = String.Empty
                If Not Session("OrderId") Is Nothing Then
                    strOrderId = Session("OrderId")
                End If

                If Not String.IsNullOrEmpty(strOrderId) Then
                    Components.Email.SendError("ToError500", "Checkout Error", Request.Url.ToString() & "<br>OrderId: " & strOrderId & "<br>MemberId: " & strMemberId & ShoppingCart.GetSessionList())
                End If
            Catch ex As Exception
                Components.Email.SendError("ToError500", "Checkout Error Exception", Request.Url.ToString() & "<br>MemberId: " & strMemberId & "<br>Exception: " & ex.ToString() & ShoppingCart.GetSessionList())
            End Try

            Response.Redirect("/")
            Exit Sub
        End If

        If dbOrder Is Nothing Then
            Response.Redirect("/")
            Exit Sub
        Else
            If dbOrder.PaymentType <> "PAYPAL" Then
                strMemberId = Utility.Common.GetCurrentMemberId()
            End If
        End If


        If Not IsPostBack Then
            Dim redirect As Boolean = False
            Try
                BindData()
                LoadMetaData(DB, "/store/confirmation.aspx")
            Catch ex As Exception
            End Try
        End If
    End Sub
    Private Sub BindData()
        Try

            Dim objCart As New ShoppingCart(DB, OrderId, True)
            Dim iMemberId As Integer = objCart.Order.MemberId
            If iMemberId > 0 Then
                If Not String.IsNullOrEmpty(strMemberId) AndAlso strMemberId <> "0" AndAlso iMemberId.ToString() <> strMemberId Then
                    Response.Redirect("/")
                    Exit Sub
                End If
            End If

            Dim BillToName As String = objCart.Order.BillToName & " " & objCart.Order.BillToName2
            ltrCustomername.Text = BillToName.Trim()
            If objCart.Order.PaypalStatus = 1 Then
                divPending.Visible = True
            End If

            ucCart.Cart = objCart
            Dim ucPayment As controls_checkout_payment_infor = Me.Master.FindControl("ucPayment")
            If Not ucPayment Is Nothing Then
                ucPayment.Cart = objCart
            End If

            Dim ucSummary As controls_checkout_cart_summary = Me.Master.FindControl("ucCartSummary")
            If Not ucSummary Is Nothing Then
                ucSummary.Cart = objCart
            End If
        Catch ex As Exception
            Email.SendError("ToError500", "Confirmation.aspx > BindData", "OrderId: " & OrderId & "<Br>Exception: " & ex.ToString())
        End Try
    End Sub



End Class
