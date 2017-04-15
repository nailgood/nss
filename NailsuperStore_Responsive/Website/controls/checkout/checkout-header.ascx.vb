Imports DataLayer
Imports Components
Imports System.Data.SqlClient
Imports System.Collections.Generic
Partial Class controls_checkout_checkout_header
    Inherits ModuleControl
    Public step1Link As String = String.Empty
    Public step2Link As String = String.Empty
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Utility.Common.IsViewFromAdmin() Then
            Me.Visible = False
            Exit Sub
        End If
        If Request.Path = "/store/cart.aspx" Or Request.Path = "/store/reward-point.aspx" Or Request.Path = "/store/free-sample.aspx" Or Request.Path = "/store/confirmation.aspx" Or Request.Path = "/store/free-gift.aspx" Then
            divLogo.Visible = False
        End If
        If Request.Path = "/store/confirmation.aspx" Then
            step1Link = ""
            step2Link = ""
        Else
            step1Link = GetLink(1)
            step2Link = GetLink(2)
        End If

    End Sub
    Public Function GetNodeNumberClass(ByVal index As Integer) As String
        Dim currentProcessIndex As Integer = GetCurrentProcessIndex()
        If (index <= currentProcessIndex) Then
            Return "node-active"
        End If
        Return "node"
    End Function
    Public Function GetNodeBarClass(ByVal index As Integer) As String
        Dim currentProcessIndex As Integer = GetCurrentProcessIndex()
        If (index <= currentProcessIndex) Then
            Return "bar-active"
        End If
        Return "bar"
    End Function
    Private Function GetCurrentProcessIndex() As Integer
        Dim result As Integer = 0
        If Request.Path = "/store/cart.aspx" Or Request.Path = "/store/reward-point.aspx" Or Request.Path = "/store/revise-cart.aspx" Or Request.Path = "/store/free-sample.aspx" Or Request.Path = "/store/free-gift.aspx" Then
            result = 1
        ElseIf Request.Path = "/store/payment.aspx" Or Request.Path = "/store/billingint.aspx" Or Request.Path = "/store/billing.aspx" Or Request.Path = "/members/addressbook/edit.aspx" Then
            result = 2
        ElseIf Request.Path = "/store/confirmation.aspx" Then
            result = 3
        End If
        Return result
    End Function
    Public Function GetLink(ByVal index As Integer) As String
        Dim currentProcessIndex As Integer = GetCurrentProcessIndex()
        If (index < currentProcessIndex) Then
            If (index = 1) Then
                Return "/store/revise-cart.aspx"
            End If
            If (index = 2) Then
                Return "/store/payment.aspx"
            End If
        End If
        Return String.Empty
    End Function
End Class
