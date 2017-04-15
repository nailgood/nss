Imports DataLayer
Imports Components

'Page use for attached file to send mail
Partial Class store_confirmation_send
    Inherits System.Web.UI.Page

    Protected OrderId As Integer = Nothing
    Public webRoot As String

    Protected dbOrder As StoreOrderRow
    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim sp As New SitePage()

        webRoot = ConfigurationManager.AppSettings("GlobalRefererName")
        If webRoot = "" Then
            webRoot = "https://www.nss.com/"
        End If
        If Not Request("OrderId") = Nothing Then
            Try
                '' OrderId = Utility.Crypt.DecryptTripleDes(Request("OrderId"))
                OrderId = Request("OrderId")
                If Not OrderId = Nothing Then
                    dbOrder = StoreOrderRow.GetRow(sp.DB, OrderId)
                End If
            Catch ex As Exception
                OrderId = Nothing
            End Try
        End If
        'OrderId = 87142
        'dbOrder = StoreOrderRow.GetRow(sp.DB, OrderId)
        If OrderId = Nothing Then Response.Redirect(ConfigurationManager.AppSettings("GlobalRefererName") & "/home.aspx")

        If Not IsPostBack Then
            Dim redirect As Boolean = False
            Try
                BindData()
            Catch ex As Exception
                redirect = True
            End Try
            'If redirect Then Response.Redirect(GlobalRefererName & "/")
        End If
    End Sub

    Private Sub BindData()
        Dim sp As New SitePage()
        od.Cart = New ShoppingCart(sp.DB, OrderId, True)
    End Sub
End Class
