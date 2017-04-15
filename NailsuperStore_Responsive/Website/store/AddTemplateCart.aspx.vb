Imports DataLayer
Imports Components
'Imports localhost
Imports System.IO

Partial Class store_AddTemplateCart
    Inherits SitePage
    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            AddCart()
        End If
    End Sub
    Private Sub AddCart()
        Dim templateId As Integer = 0
        Dim packageId As Integer = 0
        Dim itemId As Integer = 0

        If Request.QueryString("Id") IsNot Nothing Then
            itemId = CInt(Request.QueryString("Id"))
        Else
            If Not Request.QueryString("tid") Is Nothing Then
                templateId = CInt(Request.QueryString("tid"))
            End If

            If Not Request.QueryString("pid") Is Nothing Then
                packageId = CInt(Request.QueryString("pid"))
            End If

            itemId = StoreItemRow.GetTemplateItemId(templateId, packageId)
        End If

        If itemId > 0 Then
            ''check item is Valid
            Dim MemberId As Integer = Utility.Common.GetCurrentMemberId()
            If MemberId > 0 Then
                ''add cart here
                DB.BeginTransaction()
                Try
                    Cart.Add2Cart(itemId, Nothing, 1, Nothing, "Myself", Nothing, Nothing, Nothing, Nothing, False, True, Nothing)
                    Cart.RecalculateOrderDetail("AddTemplateCart.AddCart")
                    DB.CommitTransaction()
                Catch ex As Exception
                    DB.RollbackTransaction()
                End Try
                Response.Redirect("/store/cart.aspx")
            Else
                SetLastWebsiteURL(Request.RawUrl)
                Response.Redirect("/members/login.aspx?template=" & itemId)
            End If
        Else
            Response.Redirect("/")
        End If
    End Sub
End Class
