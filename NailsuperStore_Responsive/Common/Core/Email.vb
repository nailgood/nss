Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Mail
Imports System.Text.RegularExpressions
Imports Utility.Common
Imports System.Configuration
Imports Components
Imports DataLayer

Namespace Components
    Public Enum FromEmailType
        [Error]
        NoReply
        Sales
    End Enum
    Public Class Email
        Public Shared Function IsLocal() As Boolean
            If Not System.Web.HttpContext.Current Is Nothing Then
                If Not System.Web.HttpContext.Current.Request Is Nothing Then
                    Dim url As String = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.ToString
                    If url.Contains("local") Or url.Contains("192.168.") Then
                        Return True
                    End If
                End If
            End If
            Return False
        End Function
        Public Shared Function SendHTMLMail2(ByVal FromEmail As String, ByVal FromName As String, ByVal ToEmail As String, ByVal ToName As String, ByVal Subject As String, ByVal Body As String) As Boolean
            SendHTMLMail(FromEmail, FromName, ToEmail, ToName, Subject, Body, "")
        End Function

        Public Shared Sub SendReport(ByVal ToEmailSysParam As String, ByVal Subject As String, ByVal Body As String)
            Dim ToEmail As String = DataLayer.SysParam.GetValue(ToEmailSysParam)
            Dim ToName As String = AdminRow.GetRowByEmail(ToEmail)
            SendHTMLMail(FromEmailType.NoReply, ToEmail, ToName, Subject, Body)
        End Sub

        Public Shared Sub SendError(ByVal ToEmailSysParam As String, ByVal Subject As String, ByVal Body As String)
            If IsLocal() Then
                Web.HttpContext.Current.Response.Write(Subject + "<br>------------<br>" + Body)
                Exit Sub
            End If
            If Not Utility.ConfigData.AllowSendMailLog Then
                Exit Sub
            End If

            Dim ToEmail As String = DataLayer.SysParam.GetValue(ToEmailSysParam)
            Dim ToName As String = AdminRow.GetRowByEmail(ToEmail)
            SendHTMLMail(FromEmailType.Error, ToEmail, ToName, Subject, Body)

            'If IsLocal() Then
            '    Exit Sub
            'End If
            'Dim isAllowSendMailLog As Boolean = Utility.ConfigData.AllowSendMailLog
            'Dim memberID As Integer = Utility.Common.GetCurrentMemberId()
            'If (memberID = Utility.ConfigData.MemberAllowSendMailLog AndAlso memberID > 0) Then
            '    isAllowSendMailLog = True
            'End If
            'If Not isAllowSendMailLog Then
            '    Exit Sub
            'End If

            'Dim ToEmail As String = DataLayer.SysParam.GetValue(ToEmailSysParam)
            'Dim ToName As String = AdminRow.GetRowByEmail(ToEmail)
            'SendHTMLMail(FromEmailType.Error, ToEmail, ToName, Subject, Body)
        End Sub

        Public Shared Function SendHTMLMail(ByVal fromType As FromEmailType, ByVal ToEmail As String, ByVal ToName As String, ByVal Subject As String, ByVal Body As String) As Boolean
            If IsLocal() Then
                Return True
            Else
                Return SendHTMLMail(fromType, ToEmail, ToName, Subject, Body, "")
            End If
        End Function
        Public Shared Function SendSimpleMail(ByVal fromType As FromEmailType, ByVal ToEmail As String, ByVal ToName As String, ByVal Subject As String, ByVal Body As String, ByVal BCC As String) As Boolean
            Dim bResult As Boolean = False
            If (Utility.ConfigData.SendByGmail) Then
                Email.SendHTMLMailByGmail(Subject, Body, ToName, ToEmail, String.Empty)
                Exit Function
            End If
            Dim FromEmail As String
            Dim FromName As String
            If fromType = FromEmailType.NoReply Then
                FromEmail = DataLayer.SysParam.GetValue("FromNoReply")
                FromName = DataLayer.SysParam.GetValue("FromNoReplyName")
            ElseIf fromType = FromEmailType.Sales Then
                FromEmail = DataLayer.SysParam.GetValue("FromSales")
                FromName = DataLayer.SysParam.GetValue("FromSalesName")
            ElseIf fromType = FromEmailType.Error Then
                FromEmail = DataLayer.SysParam.GetValue("FromError")
                FromName = "Error"
            Else
                Return False
            End If
            Return SendSimpleMail(FromEmail, FromName, ToEmail, ToName, Subject, Body, BCC)
        End Function
        Public Shared Function SendSimpleMail(ByVal FromEmail As String, ByVal FromName As String, ByVal ToEmail As String, ByVal ToName As String, ByVal Subject As String, ByVal Body As String, ByVal Bcc As String) As Boolean
            Try

                If ToEmail.Contains(",") Then
                    Dim indexFirst As Integer = ToEmail.IndexOf(",")
                    If (indexFirst = ToEmail.Length - 1) Then
                        ToEmail = ToEmail.Substring(0, ToEmail.Length - 1)
                    Else
                        Dim splitToEmail As String = ToEmail.Substring(0, indexFirst)
                        Dim splitBcc As String = ToEmail.Substring(indexFirst + 1)
                        ToEmail = splitToEmail
                        If (String.IsNullOrEmpty(Bcc)) Then
                            Bcc = splitBcc
                        Else
                            Bcc = Bcc & "," & splitBcc
                        End If
                    End If
                End If
                Dim m As New MailMessage()
                m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserver") = Utility.ConfigData.MailServer
                m.Fields("http://schemas.microsoft.com/cdo/configuration/sendusing") = Utility.ConfigData.MailSendUsing
                m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = Utility.ConfigData.MailServerPort
                m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpusessl") = Utility.ConfigData.MailSSL.ToString().ToLower()
                m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate") = Utility.ConfigData.MailAuthenticate
                m.Fields("http://schemas.microsoft.com/cdo/configuration/sendusername") = Utility.ConfigData.MailUsername
                m.Fields("http://schemas.microsoft.com/cdo/configuration/sendpassword") = Utility.ConfigData.MailPassword
                If String.IsNullOrEmpty(ToName) Then
                    m.To = ToEmail
                Else
                    m.To = String.Format("""{0}"" <{1}>", ToName, ToEmail)
                End If

                If String.IsNullOrEmpty(FromName) Then
                    m.From = FromEmail
                Else
                    m.From = String.Format("""{0}"" <{1}>", FromName, FromEmail)
                End If

                m.Bcc = Bcc
                m.Subject = Subject
                m.Body = Body
                m.BodyFormat = MailFormat.Text
                m.BodyEncoding = System.Text.Encoding.UTF8
                System.Web.Mail.SmtpMail.SmtpServer = Utility.ConfigData.MailServer
                System.Web.Mail.SmtpMail.Send(m)

                Return True
            Catch ex As Exception
                SendMailLog(ToEmail, Subject, Body, ex.ToString, False)
            End Try
            Return False
        End Function

        Public Shared Function SendHTMLMail(ByVal fromType As FromEmailType, ByVal ToEmail As String, ByVal ToName As String, ByVal Subject As String, ByVal Body As String, ByVal BCC As String) As Boolean
            Dim bResult As Boolean = False
            If (Utility.ConfigData.SendByGmail) Then
                Email.SendHTMLMailByGmail(Subject, Body, ToName, ToEmail, String.Empty)
                Exit Function
            End If
            Dim FromEmail As String
            Dim FromName As String
            If fromType = FromEmailType.NoReply Then
                FromEmail = DataLayer.SysParam.GetValue("FromNoReply")
                FromName = DataLayer.SysParam.GetValue("FromNoReplyName")
            ElseIf fromType = FromEmailType.Sales Then
                FromEmail = DataLayer.SysParam.GetValue("FromSales")
                FromName = DataLayer.SysParam.GetValue("FromSalesName")
            ElseIf fromType = FromEmailType.Error Then
                FromEmail = DataLayer.SysParam.GetValue("FromError")
                FromName = "Error"
            Else
                Return False
            End If
            Return SendHTMLMail(FromEmail, FromName, ToEmail, ToName, Subject, Body, BCC)
        End Function

        Public Shared Function SendHTMLMail(ByVal FromEmail As String, ByVal FromName As String, ByVal ToEmail As String, ByVal ToName As String, ByVal Subject As String, ByVal Body As String, ByVal Bcc As String) As Boolean
            Try

                If ToEmail.Contains(",") Then
                    Dim indexFirst As Integer = ToEmail.IndexOf(",")
                    If (indexFirst = ToEmail.Length - 1) Then
                        ToEmail = ToEmail.Substring(0, ToEmail.Length - 1)
                    Else
                        Dim splitToEmail As String = ToEmail.Substring(0, indexFirst)
                        Dim splitBcc As String = ToEmail.Substring(indexFirst + 1)
                        ToEmail = splitToEmail
                        If (String.IsNullOrEmpty(Bcc)) Then
                            Bcc = splitBcc
                        Else
                            Bcc = Bcc & "," & splitBcc
                        End If
                    End If
                End If
                Dim m As New MailMessage()
                m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserver") = Utility.ConfigData.MailServer
                m.Fields("http://schemas.microsoft.com/cdo/configuration/sendusing") = Utility.ConfigData.MailSendUsing
                m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = Utility.ConfigData.MailServerPort
                m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpusessl") = Utility.ConfigData.MailSSL.ToString().ToLower()
                m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate") = Utility.ConfigData.MailAuthenticate
                m.Fields("http://schemas.microsoft.com/cdo/configuration/sendusername") = Utility.ConfigData.MailUsername
                m.Fields("http://schemas.microsoft.com/cdo/configuration/sendpassword") = Utility.ConfigData.MailPassword
                If String.IsNullOrEmpty(ToName) Then
                    m.To = ToEmail
                Else
                    m.To = String.Format("""{0}"" <{1}>", ToName, ToEmail)
                End If

                If String.IsNullOrEmpty(FromName) Then
                    m.From = FromEmail
                Else
                    m.From = String.Format("""{0}"" <{1}>", FromName, FromEmail)
                End If

                m.Bcc = Bcc
                m.Subject = Subject
                m.Body = Body
                m.BodyFormat = MailFormat.Html
                m.BodyEncoding = System.Text.Encoding.UTF8
                System.Web.Mail.SmtpMail.SmtpServer = Utility.ConfigData.MailServer
                System.Web.Mail.SmtpMail.Send(m)

                Return True
            Catch ex As Exception
                SendMailLog(ToEmail, Subject, Body, ex.ToString, False)
            End Try
            Return False
        End Function

        Public Shared Function SendMailLog(ByVal sEmail As String, ByVal sSubject As String, ByVal sBody As String, ByVal Exception As String, ByVal bStatus As Boolean) As Boolean
            If Not bStatus Then
                SendMailError(sSubject, sBody & System.Environment.NewLine & Exception)
            End If

            Dim strPath As String = System.Configuration.ConfigurationManager.AppSettings("sLogMailPath")
            Dim filename As String = "EmailLog_" & Date.Now().Year & "_" & Date.Now().Month & "_" & Date.Now().Day & ".txt"
            Dim sContents As String = ""

            Try
                If Core.FileExists(strPath & filename) = True Then
                    sContents = Core.OpenFile(strPath & filename)
                End If
                sContents = sContents & "Date: " & Date.Now() & " Send Email: " & sEmail & " Subject: " & sSubject & vbCrLf & sBody & vbCrLf & " Status: "
                If bStatus = True Then
                    sContents = sContents & "Success"
                Else
                    sContents = sContents & "Fail"

                End If

                Core.WriteFile(strPath & filename, sContents)
                Return True
            Catch ex As Exception
                Return False
            End Try


        End Function

        Public Shared Function SendMailError(ByVal Subject As String, ByVal Body As String) As Boolean
            If Not Utility.ConfigData.AllowSendMailLog Then
                Return False
            End If

            ''get config here
            Dim serverMail As String = Utility.ConfigData.MailErrorServer
            Dim ServerPort As Integer = Utility.ConfigData.MailErrorPort
            Dim SMTPSSL As Boolean = Utility.ConfigData.MailErrorSSL
            Dim MailUsername As String = Utility.ConfigData.MailErrorUsername
            Dim MailPassword As String = Utility.ConfigData.MailErrorPassword
            Dim ToName As String = String.Empty
            Dim ToEmail As String = Utility.ConfigData.MailErrorTo
            Dim FromName As String = "The Nail Superstore"
            Dim CC As String = Utility.ConfigData.MailErrorCC

            Dim flag As Boolean = False
            Try
                Dim m As New System.Web.Mail.MailMessage()
                m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserver") = serverMail
                m.Fields("http://schemas.microsoft.com/cdo/configuration/sendusing") = 2
                m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = ServerPort
                If SMTPSSL Then
                    m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpusessl") = "true"
                Else
                    m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpusessl") = "false"
                End If
                m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate") = 1

                If Not String.IsNullOrEmpty(MailUsername) And Not String.IsNullOrEmpty(MailPassword) Then
                    m.Fields("http://schemas.microsoft.com/cdo/configuration/sendusername") = MailUsername
                    m.Fields("http://schemas.microsoft.com/cdo/configuration/sendpassword") = MailPassword
                End If

                If String.IsNullOrEmpty(ToName) Then
                    m.To = ToEmail
                Else
                    m.To = String.Format("""{0}"" <{1}>", ToName, ToEmail)
                End If

                If String.IsNullOrEmpty(FromName) Then
                    m.From = MailUsername
                Else
                    m.From = String.Format("""{0}"" <{1}>", FromName, MailUsername)
                End If
                If CC.Length > 0 Then
                    m.Bcc = CC
                End If
                m.Subject = Subject
                m.Body = Body
                m.BodyFormat = System.Web.Mail.MailFormat.Html
                m.BodyEncoding = System.Text.Encoding.UTF8
                System.Web.Mail.SmtpMail.SmtpServer = serverMail
                System.Web.Mail.SmtpMail.Send(m)
                flag = True

            Catch ex As Exception

            End Try

            Return flag
        End Function

        Public Shared Function SendHTMLMailByGmail(ByVal Subject As String, ByVal Body As String, ByVal toName As String, ByVal toEmail As String, ByVal cc As String) As Boolean

            ''get config here
            Dim serverMail As String = Utility.ConfigData.MailErrorServer
            Dim ServerPort As Integer = Utility.ConfigData.MailErrorPort
            Dim SMTPSSL As Boolean = Utility.ConfigData.MailErrorSSL
            Dim MailUsername As String = Utility.ConfigData.MailErrorUsername
            Dim MailPassword As String = Utility.ConfigData.MailErrorPassword
            Dim FromName As String = "The Nail Superstore"
            '' Dim CC As String = Utility.ConfigData.BCCEmailConfirmPayment

            Dim flag As Boolean = False
            Try
                Dim m As New System.Web.Mail.MailMessage()
                m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserver") = serverMail
                m.Fields("http://schemas.microsoft.com/cdo/configuration/sendusing") = 2
                m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = ServerPort
                If SMTPSSL Then
                    m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpusessl") = "true"
                Else
                    m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpusessl") = "false"
                End If
                m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate") = 1

                If Not String.IsNullOrEmpty(MailUsername) And Not String.IsNullOrEmpty(MailPassword) Then
                    m.Fields("http://schemas.microsoft.com/cdo/configuration/sendusername") = MailUsername
                    m.Fields("http://schemas.microsoft.com/cdo/configuration/sendpassword") = MailPassword
                End If

                If String.IsNullOrEmpty(toName) Then
                    m.To = toEmail
                Else
                    m.To = String.Format("""{0}"" <{1}>", toName, toEmail)
                End If

                If String.IsNullOrEmpty(FromName) Then
                    m.From = MailUsername
                Else
                    m.From = String.Format("""{0}"" <{1}>", FromName, MailUsername)
                End If
                If cc.Length > 0 Then
                    m.Bcc = cc
                End If
                m.Subject = Subject
                m.Body = Body
                m.BodyFormat = System.Web.Mail.MailFormat.Html
                m.BodyEncoding = System.Text.Encoding.UTF8
                System.Web.Mail.SmtpMail.SmtpServer = serverMail
                System.Web.Mail.SmtpMail.Send(m)
                flag = True

            Catch ex As Exception

            End Try

            Return flag
        End Function

    End Class

End Namespace