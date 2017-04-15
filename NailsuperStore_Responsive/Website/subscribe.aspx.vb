Imports Components
Imports System.IO
Imports DataLayer
Imports MasterPages
Imports System.Data.SqlClient

Public Class subscribe
	Inherits SitePage

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            If GetQueryString("r") = "success" Then
                divConfirm.Visible = True
                divMain.Visible = False
                ltlEmail.Text = txtEmail.Text
            ElseIf Not String.IsNullOrEmpty(Request("e")) Then
                txtEmail.Text = Request("e")
                If txtEmail.Text = "your@email.com" Then txtEmail.Text = String.Empty
            End If
            LoadFromDB()
        End If
    End Sub
    Private strNewletter As String = "<div class='checkbox'><label for='chkList{0}'>" & vbCrLf &
                    "<input type='checkbox' id='chkList{0}' checked={1} /><i class=""fa fa-check checkbox-font"" ></i>{2}</label></div>"
    Private Sub LoadFromDB()
        Dim dt As DataTable = MailingListRow.GetPermanentLists(DB)
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            For Each row As DataRow In dt.Rows
                Dim checked As String = ""
                If (row(2) = "Active") Then
                    checked = "checked"
                    hidList.Value = row(0) & ","
                End If
                'ltrEmailList.Text &= "<div class='checkbox'><label for='chkList" & row(0) & "'>" & vbCrLf & _
                '    "<input type='checkbox' id='chkList" & row(0) & "' checked=" & checked & " /><span class='checkbox-icon'></span>" & row(1) & "</label></div>"
                ltrEmailList.Text &= String.Format(strNewletter, row(0), checked, row(1))
            Next
        Else
            ltrEmailList.Text &= String.Format(strNewletter, 0, "checked", "Newsletter")
        End If
    End Sub

    Protected Sub ServerCheckValidEmail(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        If Not String.IsNullOrEmpty(txtEmail.Text) Then
            Dim email As String = txtEmail.Text.Trim()
            If Not Utility.Common.CheckValidEmail(email) Then
                e.IsValid = False
            End If
        End If
    End Sub

    Protected Sub btnSignup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSignup.Click
        If String.IsNullOrEmpty(txtEmail.Text) Or String.IsNullOrEmpty(txtName.Text) Then
            Return
        End If

        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbMailingMember As MailingMemberRow
            Dim MemberId As Integer = 0

            dbMailingMember = MailingMemberRow.GetRowByEmail(DB, txtEmail.Text)
            dbMailingMember.Email = txtEmail.Text
            dbMailingMember.Name = txtName.Text
            dbMailingMember.MimeType = radEmailFormat.SelectedValue
            dbMailingMember.Status = "ACTIVE"
            If dbMailingMember.MemberId <> 0 Then
                dbMailingMember.Update()
            Else
                MemberId = dbMailingMember.Insert
            End If
            If (dbMailingMember.MemberId > 0) Then
                dbMailingMember.DeleteFromAllPermanentLists()
                dbMailingMember.InsertToLists(hidList.Value)
            End If
            DB.CommitTransaction()

            divConfirm.Visible = True
            divMain.Visible = False
            ltlEmail.Text = txtEmail.Text
            Email.SendReport("ToReportUnsubscribe", "Report Newsletter", "Name: " & dbMailingMember.Name & "<br /> Email: " & dbMailingMember.Email)

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        Finally
            Response.Redirect("subscribe.aspx?r=success")
        End Try
    End Sub
End Class
