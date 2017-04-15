Imports System.Web
Imports System.Collections
Imports System.Collections.Specialized
Imports System.Net.Mail
Imports System.Xml
Imports System.Web.Caching
Imports System.Threading
Imports System.Runtime.InteropServices
Imports System.Security.Permissions
Imports DataLayer

Namespace Components

    Public Class ErrorHandler
        Private ErrorMapCollection As StringDictionary
        Private DB As Database

        Private Sub LoadErrMapCollection()
            Dim Filename As String

            'RETRIEVE FROM CACHE
            ErrorMapCollection = CType(HttpContext.Current.Cache("ErrMapCollection"), StringDictionary)
            If ErrorMapCollection Is Nothing Then

                ErrorMapCollection = New StringDictionary

                Filename = System.Configuration.ConfigurationManager.AppSettings("ErrorMapFile").ToString
                Dim xmlReader As XmlTextReader = Nothing
                Try
                    xmlReader = New XmlTextReader(HttpContext.Current.Server.MapPath(Filename))
                    While (xmlReader.Read())
                        If xmlReader.NodeType = XmlNodeType.Element Then
                            If xmlReader.HasAttributes Then
                                While xmlReader.MoveToNextAttribute()
                                    Dim original As String = xmlReader.Value
                                    If xmlReader.MoveToNextAttribute() Then
                                        ErrorMapCollection.Add(original, xmlReader.Value)
                                    End If
                                End While
                            End If
                        End If
                    End While
                Finally
                    If Not xmlReader Is Nothing Then xmlReader.Close()
                End Try

                'SAVE IN THE CACHE    
                HttpContext.Current.Cache.Insert("ErrMapCollection", ErrorMapCollection, New CacheDependency(HttpContext.Current.Server.MapPath(Filename)))
            End If
        End Sub

        Public Sub New(ByVal DB As Database)
            Me.DB = DB
            LoadErrMapCollection()
        End Sub

        Public Sub LogError(ByVal ex As Exception)
            Dim dct As StringDictionary = New StringDictionary
            Dim sError As String = Nothing
            Dim ErrorInfo As Exception = ex.GetBaseException()

            sError &= "*** Error Message ***" & vbCrLf & "---------------------" & vbCrLf & ErrorInfo.Message & vbCrLf & vbCrLf
            sError &= "*** Error Source ***" & vbCrLf & "---------------------" & vbCrLf & ErrorInfo.Source & vbCrLf & vbCrLf
            sError &= "*** Error Target Site ***" & vbCrLf & "---------------------" & vbCrLf & ErrorInfo.TargetSite.ToString & vbCrLf & vbCrLf
            sError &= "*** Date/Time ***" & vbCrLf & "---------------------" & vbCrLf & DateTime.Now.ToString() & vbCrLf & vbCrLf
            sError &= "*** Stack Trace ***" & vbCrLf & "---------------------" & vbCrLf & ex.StackTrace.ToString & vbCrLf & vbCrLf
            If Not HttpContext.Current.Request.ServerVariables("HTTP_REFERER") Is Nothing Then
                sError &= "*** Referrer ***" & vbCrLf & "---------------------" & vbCrLf & HttpContext.Current.Request.ServerVariables("HTTP_REFERER").ToString & vbCrLf & vbCrLf
            End If
            If Not HttpContext.Current.Request.QueryString Is Nothing Then
                sError &= "*** Query String ***" & vbCrLf & "---------------------" & vbCrLf & HttpContext.Current.Request.QueryString.ToString & vbCrLf & vbCrLf
            End If
            If Not HttpContext.Current.Request.Form Is Nothing Then
                sError &= "*** Form Fields ***" & vbCrLf & "---------------------" & vbCrLf
                For i As Integer = 0 To HttpContext.Current.Request.Form.Count - 1
                    If HttpContext.Current.Request.Form.Keys(i).ToString <> "__VIEWSTATE" Then
                        sError &= HttpContext.Current.Request.Form.Keys(i) & "=" & HttpContext.Current.Request.Form(i) & vbCrLf
                    End If
                Next
            End If
            sError &= vbCrLf & "*** Session ***" & vbCrLf & "---------------------" & vbCrLf
            Dim keys As ICollection = HttpContext.Current.Session.Keys
            For Each key As String In keys
                'sError &= key & "=" & CStr(HttpContext.Current.Session(key)) & vbCrLf
            Next

            ' LOG ERROR IN THE DATABASE AND SEND EMAILS TO ADMINISTRATORS
            Try
                'LogErrorInDatabase(sError)
                'SendErrorNotification(sError)
            Catch
            End Try

        End Sub

        Public ReadOnly Property ErrorText(ByVal ex As Exception) As String
            Get
                Dim ErrorDesc As String = String.Empty

                If Left(ex.Message, 7) = "CUSTOM:" Then
                    ErrorDesc = Right(ex.Message, Len(ex.Message) - 7)
                Else
                    For Each entry As DictionaryEntry In ErrorMapCollection
                        If InStr(UCase(ex.Message), UCase(CStr(entry.Key))) > 0 Then
                            ErrorDesc = CStr(entry.Value)
                            Exit For
                        End If
                    Next
                End If

                If ErrorDesc = String.Empty Then
                    LogError(ex)
                    Return "A System Error has occured. Our systems administrators have been notified and are working to fix the problem. If the problem persists, please exit your Web browser and try again. Please contact Customer Service at <a href=""mailto:" & SysParam.GetValue("ToError500") & """>" & SysParam.GetValue("ToError500") & "</a> if you need further assistance."
                End If
                Return ErrorDesc
            End Get
        End Property

        Private Sub LogErrorInDatabase(ByVal sErrMsg As String)
            Dim SQL As String = "INSERT INTO ErrorLog (RemoteIP, ErrorDate, ErrorDesc) VALUES (" _
              & DB.Quote(HttpContext.Current.Request.ServerVariables("REMOTE_ADDR").ToString) & "," _
              & DB.Quote(Now().ToString) & "," _
              & DB.Quote(sErrMsg) _
              & ")"
            DB.ExecuteSQL(SQL)
            '----------------------------------------------------------------------
        End Sub

        Private Sub SendErrorNotification(ByVal sBody As String)
            Dim Sender As String = System.Configuration.ConfigurationManager.AppSettings("ErrorLogEmailFrom")
            Dim Recipients As String = System.Configuration.ConfigurationManager.AppSettings("ErrorLogEmailRecipients")

            'Don't try to send email if the error notification email is set to empty string    
            If Recipients = String.Empty Then
                Exit Sub
            End If

            Dim Subject As String = System.Configuration.ConfigurationManager.AppSettings("ErrorLogEmailSubject")
            Dim aRecipients As String() = Recipients.Split(";"c)
            Dim sConn As String = ""

            Dim msgFrom As MailAddress = New MailAddress(Sender)
            Dim msgTo As MailAddress = New MailAddress(aRecipients(0))
            Dim msg As MailMessage = New MailMessage(msgFrom, msgTo)
            msg.IsBodyHtml = False
            For i As Integer = 1 To aRecipients.Length - 1
                msg.CC.Add(aRecipients(i))
            Next
            msg.Subject = Subject
            msg.Body = sBody

            Dim SmtpMail As SmtpClient = New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("MailServer"))
            SmtpMail.Send(msg)
        End Sub
    End Class

End Namespace
