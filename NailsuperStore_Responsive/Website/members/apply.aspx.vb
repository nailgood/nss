Imports Components
Imports DataLayer
Imports System.Data.SqlClient

Partial Class members_apply
	Inherits SitePage

	Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
		If Not HasAccess() Then
			Response.Redirect("/members/login.aspx")
		End If
        Dim Member As MemberRow = MemberRow.GetRow(Session("memberId"))
		If Member.CanPostJob Then Response.Redirect("/members/")
	End Sub


End Class
