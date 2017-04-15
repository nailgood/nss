Imports System
Imports System.Collections
Imports System.Security
Imports System.Security.Cryptography
Imports System.Text
Imports DataLayer
Imports System.Web.SessionState
Imports Database
Imports Utility
Imports Components
Imports Components.Core
Imports System.Data.SqlClient
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Data.Common

Namespace Components

    Public Class ShoppingCart
        Inherits BaseShoppingCart

        Public Sub New()
            MyBase.New(Common.AddCartType.WebLive, HttpContext.Current)
            Initialize()
        End Sub 'New

        Public Sub New(ByVal _DB As Database)
            MyBase.New(_DB, Common.AddCartType.WebLive, HttpContext.Current)
            Initialize()
        End Sub 'New

        Public Sub New(ByVal _DB As Database, ByVal _OrderId As String)
            MyBase.New(_DB, _OrderId, Common.AddCartType.WebLive, HttpContext.Current)
            Initialize()
        End Sub 'New

        Public Sub New(ByVal _DB As Database, ByVal _OrderId As String, ByVal _bFromAdmin As Boolean)
            MyBase.New(_DB, _OrderId, _bFromAdmin, Common.AddCartType.WebLive, HttpContext.Current)
            If _bFromAdmin = True Then InitializeAdminOrder() Else Initialize()

        End Sub 'New        

        Private Sub InitializeAdminOrder()
            Session = HttpContext.Current.Session
            Context = HttpContext.Current
            If Not OrderId = Nothing Then
                If Not DB Is Nothing Then Order = StoreOrderRow.GetRow(DB, OrderId) Else Exit Sub
            End If
        End Sub


        Private Sub Initialize()
            Session = HttpContext.Current.Session
            m_Context = HttpContext.Current
            OrderId = Common.GetCurrentOrderId()
            MemberId = Common.GetCurrentMemberId()
            Dim bGenerate As Boolean = False

            If OrderId < 1 Then
                bGenerate = True
            ElseIf Not ValidateOrderId(OrderId) Then
                bGenerate = True
            End If

            If bGenerate AndAlso MemberId > 0 Then
                OrderId = GenerateUniqueOrderId(MemberId)
                Session("OrderId") = OrderId
                Common.SetOrderToCartCookie(OrderId)
            End If

            'Load Order record from Database
            If OrderId > 0 Then
                Order = StoreOrderRow.GetRow(DB, OrderId)
                If Not String.IsNullOrEmpty(Order.OrderNo) Then
                    Dim rawURL As String = String.Empty
                    If Not System.Web.HttpContext.Current Is Nothing Then
                        If Not System.Web.HttpContext.Current.Request Is Nothing Then
                            rawURL = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.ToString()
                        End If
                    End If

                    If Not rawURL.Contains("ipn.aspx") Then
                        Session("OrderId") = Nothing
                        Utility.Common.DeleteCachePopupCart(OrderId)
                        Utility.Common.SetOrderToCartCookie(0)
                        Order = Nothing
                    End If
                End If
            Else
                Exit Sub
            End If
        End Sub

        Public Shared Sub GetShoppingCartFromCookie(ByVal _DB As Database, ByVal IsNewCart As Boolean, ByRef shoppingCart As ShoppingCart)
            Dim isUpdateCookieOrder As Boolean = False
            If HttpContext.Current.Session("MemberId") Is Nothing Then '' 
                ''get OrderId from Cookies
                Dim CookieOrderId As Integer = Utility.Common.GetOrderIdFromCartCookie()
                Dim memberID As Integer = Utility.Common.GetMemberIdFromCartCookie()
                If CookieOrderId < 1 Then
                    CookieOrderId = StoreOrderRow.InsertUniqueOrder(_DB, HttpContext.Current.Request.ServerVariables("REMOTE_ADDR"), memberID, HttpContext.Current.Session.SessionID)
                    Utility.Common.SetOrderToCartCookie(CookieOrderId)
                    isUpdateCookieOrder = True
                Else
                    Dim orderNo As String = _DB.ExecuteScalar("Select COALESCE(OrderNo,'') from StoreOrder where OrderId=" & CookieOrderId)
                    If Not String.IsNullOrEmpty(orderNo) Then
                        CookieOrderId = StoreOrderRow.InsertUniqueOrder(_DB, HttpContext.Current.Request.ServerVariables("REMOTE_ADDR"), memberID, HttpContext.Current.Session.SessionID)
                        Utility.Common.SetOrderToCartCookie(CookieOrderId)
                        isUpdateCookieOrder = True
                    End If
                End If
                ' '' Cart.Order.OrderId = CookieOrderId
                HttpContext.Current.Session("OrderId") = CookieOrderId
                If CookieOrderId > 1 Then
                    shoppingCart = New ShoppingCart(_DB, CookieOrderId, False)
                End If

                'If (IsNewCart) Then
                '    If CookieOrderId > 1 AndAlso shoppingCart Is Nothing Then
                '        shoppingCart = New ShoppingCart(_DB, CookieOrderId, False)
                '    End If
                'End If
            Else
                If HttpContext.Current.Session("OrderId") Is Nothing Then
                    Dim lastOrderId As Integer = _DB.ExecuteScalar("Select COALESCE(LastOrderId,0) from Member where MemberId=" & HttpContext.Current.Session("MemberId"))
                    If lastOrderId < 1 Then
                        lastOrderId = StoreOrderRow.InsertUniqueOrder(_DB, HttpContext.Current.Request.ServerVariables("REMOTE_ADDR"), HttpContext.Current.Session("MemberId"), HttpContext.Current.Session.SessionID)
                        Utility.Common.SetOrderToCartCookie(lastOrderId)
                        isUpdateCookieOrder = True
                    Else
                        Dim orderNo As String = _DB.ExecuteScalar("Select COALESCE(OrderNo,'') from StoreOrder where OrderId=" & lastOrderId)
                        If Not String.IsNullOrEmpty(orderNo) Then
                            lastOrderId = StoreOrderRow.InsertUniqueOrder(_DB, HttpContext.Current.Request.ServerVariables("REMOTE_ADDR"), HttpContext.Current.Session("MemberId"), HttpContext.Current.Session.SessionID)
                            Utility.Common.SetOrderToCartCookie(lastOrderId)
                            isUpdateCookieOrder = True
                        End If
                    End If
                    HttpContext.Current.Session("OrderId") = lastOrderId
                    If lastOrderId > 1 Then
                        shoppingCart = New ShoppingCart(_DB, lastOrderId)
                    End If
                    'If (IsNewCart) Then
                    '    If lastOrderId > 1 AndAlso shoppingCart Is Nothing Then
                    '        shoppingCart = New ShoppingCart(_DB, lastOrderId)
                    '    End If
                    'End If
                End If

            End If
            If Not isUpdateCookieOrder Then
                ''update Date Expire for OrderCookie
                Dim CookieOrderId As Integer = Utility.Common.GetOrderIdFromCartCookie()
                Utility.Common.SetOrderToCartCookie(CookieOrderId)

            End If
        End Sub

    End Class

    Public Structure ShoppingCartSummary
        Public Quantity As Integer
        Public Total As Double
    End Structure

    Public Structure BaseShipping
        Public AddressId As Integer
        Public Shipping As Double
    End Structure
End Namespace



