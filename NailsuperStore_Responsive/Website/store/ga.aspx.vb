
Imports com
Imports DataLayer
Imports Components
Imports System.Web.Services

Partial Class store_ga
    Inherits System.Web.UI.Page

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadOrder()
        End If
    End Sub

    Private Sub LoadOrder()
        If Request("OrderId") IsNot Nothing Then
            Dim strOrder As String = Request("OrderId")
            ga.ListOrderId = strOrder
            'Dim sp As New SitePage()

            'For Each s As String In strOrder.Split(",")
            '    If s.Trim().Length <= 0 Then
            '        Continue For
            '    End If

            '    Try
            '        Dim uc As controls_GoogleTracking = CType(LoadControl("~/controls/layout/google-analytics.ascx"), controls_GoogleTracking)
            '        uc.OrderId = CInt(s.Trim())
            '        phdGA.Controls.Add(uc)
            '    Catch ex As Exception

            '    End Try
            'Next
        End If

    End Sub
    <WebMethod> _
    Public Shared Function SubmitGA(ByVal orderId As String) As Object
        Try
            Dim cm As Command = Command.Create("SubmitGA")
            Return cm.SubmitGA(orderId)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    <WebMethod> _
    Public Shared Function SendError(ByVal err As String) As Object
        Try
            Email.SendError("ToError500", "GoogleAnalytics", "Err JavaScript: " & err)

        Catch ex As Exception
            Return Nothing
        End Try
        Return Nothing
    End Function
End Class
