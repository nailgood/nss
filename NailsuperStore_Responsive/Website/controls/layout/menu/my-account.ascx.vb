Imports System.Web.UI.WebControls
Imports System.Data
Imports Components
Imports DataLayer
Partial Class controls_layout_menu_my_account
    Inherits ModuleControl

    Public memberID As Integer = 0
    Public Overrides Property Args As String
        Get

        End Get
        Set(ByVal value As String)

        End Set
    End Property
    Public Function GenerateGroupMenu(ByVal groupName As String, ByVal linkDefault As String) As String
        Dim result As String = String.Empty
        If (Request.RawUrl.ToLower.Contains(linkDefault)) Then
            result = result & "<li class='active'><a href='" & linkDefault & "'>" & groupName & "<span class=""arrow""></span></a>"
        ElseIf (Request.RawUrl.Contains("product-write.aspx") Or Request.RawUrl.Contains("order-write.aspx")) AndAlso linkDefault.Contains("leavereview.aspx") Then
            result = result & "<li class='active'><a href='" & linkDefault & "'>" & groupName & "<span class=""arrow""></span></a>"
        ElseIf Request.RawUrl.Contains("refer.aspx") AndAlso linkDefault.Contains("manager.aspx") Then
            result = result & "<li class='active'><a href='" & linkDefault & "'>" & groupName & "<span class=""arrow""></span></a>"
        Else
            result = result & "<li><a href='" & linkDefault & "'>" & groupName & "<span class=""arrow""></span></a>"
        End If
        Return result
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        memberID = Session("MemberId")
    End Sub
End Class
