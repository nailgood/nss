Imports Components
Imports Utility

Partial Class admin_testemail
    Inherits AdminPage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        DB.BeginTransaction()
        Try
            Dim i As Integer = DB.ExecuteSQL("UPDATE MemberSubmission SET SubmittedDate = " & DB.Quote(DateTime.Now))
            Threading.Thread.Sleep(10000)
            DB.CommitTransaction()
        Catch ex As Exception
            DB.RollbackTransaction()
        End Try

        Return
        If Not IsPostBack Then
            txtUsername.Text = ConfigData.MailUsername
            txtPassword.Text = ConfigData.MailPassword
            txtSmtp.Text = ConfigData.MailServer
            txtPort.Text = ConfigData.MailServerPort
            drpSSL.SelectedValue = Convert.ToInt32(ConfigData.MailSSL).ToString()
        End If

    End Sub

    Protected Sub btnSend_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim Mail As New System.Web.Mail.MailMessage()
            Mail.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserver") = txtSmtp.Text.Trim()
            Mail.Fields("http://schemas.microsoft.com/cdo/configuration/sendusing") = 2
            Mail.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = Convert.ToInt16(txtPort.Text.Trim())
            Mail.Fields("http://schemas.microsoft.com/cdo/configuration/smtpusessl") = IIf(drpSSL.SelectedValue = "1", "true", "false")
            Mail.Fields("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate") = 1
            Mail.Fields("http://schemas.microsoft.com/cdo/configuration/sendusername") = txtUsername.Text.Trim()
            Mail.Fields("http://schemas.microsoft.com/cdo/configuration/sendpassword") = txtPassword.Text.Trim()
            Mail.To = txtToEmail.Text.Trim()
            Mail.From = txtFromEmail.Text.Trim()
            Mail.Bcc = txtBcc.Text.Trim()
            Mail.Subject = txtSubject.Text.Trim()
            Mail.Body = txtBody.Text.Trim()
            Mail.BodyFormat = System.Web.Mail.MailFormat.Html
            Mail.BodyEncoding = System.Text.Encoding.UTF8
            System.Web.Mail.SmtpMail.SmtpServer = txtSmtp.Text.Trim()
            System.Web.Mail.SmtpMail.Send(Mail)
            litMsg.Text = "<span style=""color:Green"">Send OK</span>"

        Catch ex As Exception
            litMsg.Text = "<span style=""color:Red"">Error :" & ex.ToString & "</span>"
        End Try
    End Sub


    Protected Sub btnSend2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim msgFrom As New System.Net.Mail.MailAddress(txtFromEmail.Text.Trim(), txtFromName.Text.Trim())
            Dim msgTo As New System.Net.Mail.MailAddress(txtToEmail.Text.Trim(), txtToName.Text.Trim())
            Dim msg As New System.Net.Mail.MailMessage(msgFrom, msgTo)
            msg.IsBodyHtml = True
            msg.Subject = txtSubject.Text.Trim()
            msg.Body = txtBody.Text.Trim()
            'msg.Bcc.Add(New System.Net.Mail.MailAddress(txtBcc.Text.Trim(), "Bcc"))

            Dim Client As New System.Net.Mail.SmtpClient()
            Client.EnableSsl = IIf(drpSSL.SelectedValue = "1", True, False)
            Client.Host = txtSmtp.Text.Trim()
            Client.Port = Convert.ToInt16(txtPort.Text.Trim())
            Try
                Client.Send(msg)
                litMsg.Text = "<span style=""color:Green"">Send OK</span>"
            Catch ex As Exception
                litMsg.Text = "<span style=""color:Red"">Error :" & ex.ToString & "</span>"
            End Try

        Catch ex As Exception
            litMsg.Text = "<span style=""color:Red"">Error :" & ex.ToString & "</span>"
        End Try
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ItemIdList As String = ",50865,50865,"

        Try
            Dim itemClear As String = ""
            Dim arr() As String = Split(ItemIdList, ",")
            If arr.Length > 0 Then
                For Each item As String In arr
                    If item <> "" Then
                        If Utility.CacheUtils.RemoveCacheItemWithPrefix("StoreItem_GetRow_" & item & "_") Then
                            itemClear = itemClear & ","
                        Else
                            Email.SendHTMLMail(FromEmailType.NoReply, "k@nss.com", "khoa", "Import call - 0", "StoreItem_GetRow_" & item & "_")
                        End If
                    End If
                Next
            End If

        Catch ex As Exception
            Email.SendHTMLMail(FromEmailType.NoReply, "k@nss.com", "khoa", "Import call - 3", ex.ToString())
        End Try
    End Sub

    'Private Sub SendMail(ByVal msg As MailMessage)
    '    Dim Client As New SmtpClient("smtp.gmail.com", 465)
    '    Client.EnableSsl = False
    '    Client.Credentials = New NetworkCredential("admin@vss.com", "vss10")
    '    Client.UseDefaultCredentials = False
    '    Try
    '        Client.Send(msg)
    '        litMsg.Text = "<span style=""color:Green"">Send OK</span>"
    '    Catch ex As Exception
    '        litMsg.Text = "<span style=""color:Red"">Error :" & ex.ToString & "</span>"
    '    End Try
    'End Sub

End Class
