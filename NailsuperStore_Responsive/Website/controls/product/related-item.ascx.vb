Imports DataLayer
Imports Components
Imports System.Data.SqlClient

Partial Class Controls_RelatedItem
    Inherits ModuleControl
   
    Public ItemId As Integer
    Public ItemGroupId As Integer
    Public VideoId As Integer

    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            If VideoId > 0 Then
                ucListProduct.VideoId = VideoId
            Else
                ucListProduct.RelatedItemId = ItemId
                ucListProduct.ItemGroupId = ItemGroupId
            End If
        End If

    End Sub
End Class
