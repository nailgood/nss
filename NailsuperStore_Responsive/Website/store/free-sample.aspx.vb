Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System.IO

Partial Class Store_FreeSample
    Inherits SitePage
    '-------------------------------------------------------------------
    ' VARIABLES
    '-------------------------------------------------------------------

    '-------------------------------------------------------------------
    ' METHODS
    '-------------------------------------------------------------------
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            If Request.RawUrl.Contains("?act=checkout") Then
                Dim memberId = Utility.Common.GetCurrentMemberId()
                If memberId <= 0 And HasAccess() = False Then
                    Response.Redirect("/members/login.aspx?act=checkout")
                End If

                If Cart Is Nothing Then
                    Response.Redirect("/store/cart.aspx")
                End If
                Dim o As StoreOrderRow = Cart.Order
                If o Is Nothing Then
                    Response.Redirect("/store/cart.aspx")
                End If
                Dim Total As Double = CDbl(SysParam.GetValue("FreeSampleOrderMin"))
                If Cart.SubTotalPuChasePoint < Total Then
                    Response.Redirect("/store/free-gift.aspx?act=checkout")
                End If
                hTitle.Visible = False

                If o IsNot Nothing AndAlso o.OrderId > 0 Then
                    Utility.Common.OrderLog(o.OrderId, "Page Load", Nothing)
                End If
            End If

            getFreeSampleMessage(Cart)
            LoadList()
            ucListItem.Cart = Cart

            Dim pageDB As ContentToolPageRow = ContentToolPageRow.GetRowByURL("/store/free-sample.aspx")
            LoadMetaData(DB, pageDB)
            hTitle.InnerText = pageDB.MetaTitle
            ucListItem.Description = pageDB.Content

        End If

    End Sub

    Private Sub getFreeSampleMessage(ByVal objCart As ShoppingCart)
        Dim msg As String = String.Empty
        Dim Total As Double = CDbl(SysParam.GetValue("FreeSampleOrderMin"))
        If objCart Is Nothing Then
            msg = "To receive free product samples, your order must be $" & Total & " or more. Your order subtotal is $0"
        Else
            If objCart.SubTotalPuChasePoint < Total Then
                msg = "To receive free product samples, your order must be $" & Total & " or more. Your order subtotal is $" & objCart.SubTotalPuChasePoint
            End If


        End If


        If Not String.IsNullOrEmpty(msg) Then
            divMsg.Visible = True
            divMsg.InnerHtml = msg
        Else
            divMsg.Visible = True
        End If
    End Sub

    Private Sub LoadList()
        Dim filter As DepartmentFilterFields = New DepartmentFilterFields()
        filter.pg = 1
        filter.SortBy = String.Empty
        filter.SortOrder = String.Empty
        filter.MaxPerPage = Int32.MaxValue
        filter.MemberId = Utility.Common.GetCurrentMemberId()
        If Session("OrderId") <> Nothing Then
            filter.OrderId = Session("OrderId")
        Else
            filter.OrderId = Utility.Common.GetOrderIdFromCartCookie()
        End If
        ''If Session("OrderId") <> Nothing Then filter.OrderId = Session("OrderId")
        Dim iTotalRecords As Integer = Integer.MinValue
        Dim ItemsCollection As StoreItemCollection = StoreItemRow.GetFreeSampleColection(DB, filter, iTotalRecords)
        Dim CountSample As Integer = CInt(SysParam.GetValue("FreeSampleQty"))
        If iTotalRecords > 0 AndAlso CountSample > 0 Then
            ucListItem.ListItem = ItemsCollection
        End If

    End Sub


End Class
