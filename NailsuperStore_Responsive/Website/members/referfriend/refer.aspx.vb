Imports Components
Imports DataLayer
Imports Utility.Common
Imports System.IO

Partial Class members_ReferFriend_Refer
    Inherits SitePage

    Private LoggedInUser As New MemberRow
    Private LinkRefer As String = Utility.ConfigData.LinkRefer
    Private SendLinkRefer As String
    Protected awardPoint As Integer = 50
    Protected referedPoint As Integer = 0
    Protected totalPoint As Integer = 0
    Private AwardReferPoint As Integer = 0

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not HasAccess() Then
            DB.Close()
            Response.Redirect("/members/login.aspx")
        End If
        LoggedInUser = MemberRow.GetRow(LoggedInMemberId)
        SendLinkRefer = String.Format(LinkRefer, LoggedInUser.ReferCode)

        awardPoint = Convert.ToInt32(SysParam.GetValue("AwardedPoint"))
        referedPoint = Convert.ToInt32(SysParam.GetValue("PointUseReferFriend"))
        totalPoint = awardPoint + referedPoint
        AwardReferPoint = SysParam.GetValue("PointReferFriend")

        'ltrAwardReferPoint.Text = AwardReferPoint
        txtLink.Text = SendLinkRefer
        hdLinkFacebook.Value = SendLinkRefer & "?src=" & CType(ReferSource.FaceBook, Integer)
        hdLinkTwitter.Value = SendLinkRefer & "?src=" & CType(ReferSource.Twitter, Integer)
        barstatus1.LoadData("Invite a friend")
    End Sub
    Protected Sub btnSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSend.Click
        Dim bError As Boolean = False
        Dim lstExist As String = String.Empty
        Dim countExist As Integer = 0
        Dim lstInValid As String = String.Empty
        Dim countInValid As Integer = 0
        Dim lstSent As String = String.Empty
        Dim countSent As Integer = 0
        Dim total As Integer = 0
        Dim lstEmail As String() = SplitLisEmail(txtEmail.Text)
        Dim tmp As String = String.Empty
        Dim lstSuccess As String = String.Empty
        Dim mailTitle As String = String.Empty
        Dim mailBody As String = String.Empty
        Dim FullName As String = LoggedInFirstName & " " & LoggedInLastName
        For Each sEmail As String In lstEmail
            sEmail = sEmail.Trim()
            If Not String.IsNullOrEmpty(sEmail) Then
                If CheckValidEmail(sEmail) Then
                    Dim status As Integer = MemberReferRow.CheckStatusEmailRefer(DB, LoggedInUser.MemberId, sEmail)
                    If status = 2 Then
                        ''email registered 
                        lstExist = lstExist & sEmail & ", "
                        countExist = countExist + 1
                        bError = True
                    ElseIf status = 1 Then
                        If (Not tmp.Contains(sEmail.ToLower())) Then
                            ''email invited by this member
                            lstSent = lstSent & sEmail & ", "
                            countSent = countSent + 1
                            bError = True
                        End If
                    Else
                        ''email valid, insert and send invitation
                        Dim MemberRefer As New MemberReferRow(DB)
                        MemberRefer.TypeRefer = ReferType.ReferFriends
                        MemberRefer.MemberRefer = LoggedInUser.MemberId
                        MemberRefer.Email = sEmail
                        MemberRefer.Source = ReferSource.Email
                        MemberRefer.Status = ReferFriendStatus.SentInvite
                        MemberRefer.CreatedDate = DateTime.Now
                        MemberRefer.Insert()
                        mailBody = Core.OpenFile(Utility.ConfigData.TemplateEmailRefer)
                        If Not String.IsNullOrEmpty(mailBody) Then
                            mailTitle = Resources.Alert.SubjectReferInvite
                            Dim body As String = String.Format(mailBody, FullName, SendLinkRefer, sEmail, CType(ReferSource.Email, Integer), awardPoint, GetMoneyFromCashpoint(awardPoint), referedPoint, totalPoint, GetMoneyFromCashpoint(totalPoint))
                            Email.SendHTMLMail(FromEmailType.NoReply, sEmail, sEmail, String.Format(mailTitle, FullName), body)
                            lstSuccess = lstSuccess & "<li>" & sEmail & "</li>"
                            tmp = tmp & sEmail.ToLower() & ","
                            total = total + 1
                        End If
                    End If
                Else
                    ''Email is incorrect
                    lstInValid = lstInValid & sEmail & ", "
                    countInValid = countInValid + 1
                    bError = True
                End If
            End If
        Next
        If countExist < 1 And countInValid < 1 And countSent < 1 And total < 1 Then
            txtEmail.Text = String.Empty
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "ShowError2", "ShowError2('Please enter at least one email.<br/>');BlurText();", True)
            Exit Sub
        End If

        Dim message As String = String.Empty
        Dim msgError As String = String.Empty
        Dim showtext As String = "FocusText();"
        If bError Then
            If Not String.IsNullOrEmpty(lstExist) Then
                If String.IsNullOrEmpty(lstInValid) And String.IsNullOrEmpty(lstSent) Then
                    msgError = String.Empty '"<span class=""mcontent"">You invited existing users</span><br/><br/>"
                End If
                If countExist > 1 Then
                    msgError = msgError & "<span class=""note"">These emails are already registered at nss.com: " & lstExist.Substring(0, lstExist.Length - 2) & ".</span>"
                Else
                    msgError = msgError & "<span class=""note"">" & lstExist.Substring(0, lstExist.Length - 2) & " is already registered at nss.com.</span>"
                End If
            End If
            If Not String.IsNullOrEmpty(lstInValid) Then
                If countInValid > 1 Then
                    msgError = msgError & "<span class=""note"">These emails are incorrect: " & lstInValid.Substring(0, lstInValid.Length - 2) & ".</span>"
                Else
                    msgError = msgError & "<span class=""note"">" & lstInValid.Substring(0, lstInValid.Length - 2) & " is incorrect.</span>"
                End If
            End If
            If Not String.IsNullOrEmpty(lstSent) Then
                If countSent > 1 Then
                    msgError = msgError & "<span class=""note"">These emails have already been invited: " & lstSent.Substring(0, lstSent.Length - 2) & ".</span>"
                Else
                    msgError = msgError & "<span class=""note"">" & lstSent.Substring(0, lstSent.Length - 2) & " has already been invited.</span>"
                End If
            End If
        Else
            txtEmail.Text = ""
            showtext = "BlurText();"
        End If
        If total > 0 Then
            If total = 1 Then
                message = "<span class=""mcontent"">You invited " & total & " person to shop at The Nail Superstore.</span>"
            Else
                message = "<span class=""mcontent"">You invited " & total & " people to shop at The Nail Superstore.</span>"
            End If
            message &= "<ul>" & lstSuccess & "</ul>" _
                       & "<span class=""note"">What happens next?</span>" & AwardReferPoint & " Cash Reward Points will be added to your account after each person you invite makes an intial puchase. We will email you to let you know when this happens."

            If bError Then
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "showSuccess", "ShowSuccessError('" & message & "','" & msgError & "');" & showtext, True)
            Else
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "showSuccess", "ShowSuccess('" & message & "');" & showtext, True)
            End If
        Else
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "ShowError2", "ShowError2('" & msgError & "');" & showtext, True)
        End If
    End Sub

    Private Function SplitLisEmail(ByVal lstEmail As String) As String()
        Dim src As String = lstEmail.Replace(vbCrLf, "|")
        src = src.Replace(vbCr, "|")
        src = src.Replace(vbLf, "|")
        src = src.Replace(",", "|")
        src = src.Replace(";", "|")
        Dim result As String() = src.Trim().Split("|")
        Return result
    End Function

    Private Function GetMoneyFromCashpoint(ByVal point As Integer) As String
        Dim MoneyEachPoint As Double = SysParam.GetValue("MoneyEachPoint")
        Return ViewCurrency(point * MoneyEachPoint)
    End Function
End Class
