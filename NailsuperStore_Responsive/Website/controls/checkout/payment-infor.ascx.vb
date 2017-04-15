

Imports DataLayer
Imports Components
Imports System.Data.SqlClient
Imports System.Collections.Generic
Partial Class controls_checkout_payment_infor
    Inherits ModuleControl
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property
    Private m_Cart As ShoppingCart = Nothing

   
    Public Property Cart() As ShoppingCart
        Set(ByVal value As ShoppingCart)
            m_Cart = value
        End Set
        Get
            Return m_Cart
        End Get
    End Property

    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        


        If Not Cart Is Nothing AndAlso Not Cart.Order Is Nothing AndAlso Cart.GetCartItemCount() > 0 Then
            Select Case Cart.Order.PaymentType
                Case "CC"
                    lblMethod.InnerText = "Credit Card"
                    lblNameOnCard.InnerText = Cart.Order.CardHolderName
                    lblCardType.InnerText = CardTypeRow.GetRow(DB, Cart.Order.CardTypeId).Name
                    lblCardNumber.InnerText = Cart.Order.StarredCardNumber
                    lblExpireDate.InnerText = IIf(Cart.Order.ExpirationDate.Month.ToString.Length = 1, "0", "") & Cart.Order.ExpirationDate.Month & "/" & Cart.Order.ExpirationDate.Year

                    liCardNumber.Visible = True
                    liCardType.Visible = True
                    liNamOnCard.Visible = True
                    liExpireDate.Visible = True
                Case "PAYPAL"
                    lblMethod.InnerText = "PayPal"
                Case Else
                    lblMethod.InnerText = "Invalid Payment Method"
            End Select
        Else
            Me.Visible = False
        End If

    End Sub
   
End Class
