Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System.Web.UI.WebControls
Imports Utility

Public Class logout
    Inherits SitePage

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Overrides Sub OnInit(ByVal e As EventArgs)
        MyBase.OnInit(e)
        AddHandler Me.Load, AddressOf Page_Load
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Session.Remove("MemberId")
        Session.Remove("Username")
        Session.Remove("CheckoutType")
        Session.Remove("MemberInfo")
        Session.Remove("AddressInfo")
        Session.Remove("CustomerInfo")
        Session.Remove("OrderId")
        Session.Remove("cartRender")

        CookieUtil.SetTripleDESEncryptedCookie("MemberId", Nothing)
        CookieUtil.SetTripleDESEncryptedCookie("OrderId", Nothing)

        If Not Session("MemberId") Is Nothing Then
            If Not Cart Is Nothing Then
                If Not Cart.Order Is Nothing Then
                    Utility.Common.DeleteCachePopupCart(Cart.Order.OrderId)
                End If
            End If
            ''Utility.CookieUtil.SetTripleDESEncryptedCookie("OrderId", Nothing)
        End If
        Utility.Common.ClearCartCookieAddCart(DB)
        Response.Write("Logout")
        Response.Redirect(ConfigData.GlobalRefererName())
    End Sub

End Class
