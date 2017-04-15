Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System
Partial Class members_unsubscribe
    Inherits SitePage
    Private Member As MemberRow = Nothing
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not HasAccess() Then
            Response.Redirect("/members/login.aspx")
        End If

        Try
            Member = MemberRow.GetRow(Session("MemberId"))
            If Not Page.IsPostBack Then
                LoadSubscribe()
            End If
        Catch ex As Exception
            Email.SendReport("ToReportUnsubscribe", "Error Unsubscribe", ex.ToString())
        End Try
    End Sub

    Private Sub LoadSubscribe()
        Try
            Dim MailingMemberId As Integer = DB.ExecuteScalar("SELECT MemberId FROM MailingMember WHERE Email=" & DB.Quote(Member.Customer.Email))
            Dim dbMailingMember As MailingMemberRow
            dbMailingMember = MailingMemberRow.GetRow(DB, MailingMemberId)
            'If dbMailingMember.MimeType = "HTML" Then rbtnFormatHTML.Checked = True Else rbtnFormatTEXT.Checked = True
            If dbMailingMember.Status = "ACTIVE" Then rbtnNewsletter.SelectedValue = "1" Else rbtnNewsletter.SelectedValue = "0"
        Catch ex As Exception
            Email.SendReport("ToReportUnsubscribe", "Error Unsubscribe Function: LoadSubscribe()", ex.ToString())
        End Try
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim MemberBillingAddress As MemberAddressRow = MemberAddressRow.GetAddressByType(DB, Member.MemberId, Utility.Common.MemberAddressType.Billing.ToString())
        Dim MailingMemberId As Integer = DB.ExecuteScalar("SELECT MemberId FROM MailingMember WHERE Email=" & DB.Quote(Member.Customer.Email))
        Dim dbMailingMember As MailingMemberRow
        If rbtnNewsletter.SelectedValue = "1" Then
            ' Add new mailing member subscription
            dbMailingMember = MailingMemberRow.GetRow(DB, MailingMemberId)
            'If rbtnFormatHTML.Checked = True Then dbMailingMember.MimeType = "HTML" Else dbMailingMember.MimeType = "TEXT"
            dbMailingMember.MimeType = "HTML"
            dbMailingMember.Email = Member.Customer.Email
            dbMailingMember.Name = MemberBillingAddress.FirstName & " " & MemberBillingAddress.LastName
            dbMailingMember.Status = "ACTIVE"
            dbMailingMember.Update()
            dbMailingMember.DeleteFromAllLists()
            If rbtnNewsletter.SelectedValue = "1" Then
                dbMailingMember.InsertToList(1)
            End If
        ElseIf Not rbtnNewsletter.SelectedValue = "1" And MailingMemberId > 0 Then
            dbMailingMember = MailingMemberRow.GetRow(DB, MailingMemberId)
            dbMailingMember.Status = "DELETED"
            dbMailingMember.Update()
        End If

        Email.SendReport("ToReportUnsubscribe", "Report Unsubscribe", "Email: " & dbMailingMember.Email & "<br /> Unsubscribe: " & dbMailingMember.Status)
        Response.Redirect("/members/")

    End Sub
End Class
