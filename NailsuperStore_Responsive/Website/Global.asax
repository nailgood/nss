<%@ Application Language="VB" %>

<script RunAt="server">

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
       
    End Sub
    
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub
        
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
        Dim ctx As HttpContext = HttpContext.Current
        Dim unhandledException As Exception = ctx.Server.GetLastError()
        Dim httpException As HttpException = TryCast(unhandledException, HttpException)
        If httpException Is Nothing Then
            Dim innerException As Exception = unhandledException.InnerException
            httpException = TryCast(innerException, HttpException)
        End If
        If httpException IsNot Nothing Then
            Dim httpCode As Integer = httpException.GetHttpCode()

            'If httpCode = 400 Then
            '    URLParameters.ChangeUrlError(ctx.Request.Url.ToString())
            'End If
            If httpCode = 500 Then
               
                Dim errorInfo As String = String.Empty
                Try
                    errorInfo = "URL: " + ctx.Request.Url.ToString() + "<br><br>Message: " + unhandledException.InnerException.Message + "<br><br>Stack trace: " + unhandledException.InnerException.StackTrace
                Catch ex As Exception
                    errorInfo = "URL: " + ctx.Request.Url.ToString() + "<br><br>Message: " + unhandledException.Message + "<br><br>Stack trace: " + unhandledException.StackTrace
                End Try
                

                'Write list session
                Dim strSession As String = String.Empty
                Dim strName As String = String.Empty
                Dim iLoop As Integer
                Try
                    For Each strName In Session.Contents
                        If IsArray(Session(strName)) Then
                            For iLoop = LBound(Session(strName)) To UBound(Session(strName))
                                strSession &= strName & "(" & iLoop & ") - " & Session(strName)(iLoop) & "<BR>"
                            Next
                        Else
                            strSession &= strName & " - " & Session.Contents(strName) & "<BR>"
                        End If
                    Next
                Catch ex As Exception
                End Try
              
                
                If Not String.IsNullOrEmpty(strSession) Then
                    Dim ip As String = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR").ToString
                    errorInfo &= "<br><br>Session List<br>-------------------------<br>" & strSession & "<br>Date:" & DateTime.Now.ToString("d/M/yyyy HH:mm:ss") & "<br>IP:" & ip
                End If
                'If (errorInfo.Contains("Login failed for user")) Then
                '    Components.Email.SendError("ToError500", "Error 500", errorInfo)
                'Else
                '    Components.Email.SendError("ToError500", "Error 500", errorInfo)
                'End If
                Dim FromError As String = DataLayer.SysParam.GetValue("FromError")
                If (String.IsNullOrEmpty(FromError)) Then ''can not connect DB                  
                    Components.Email.SendHTMLMail(Utility.ConfigData.DefaultMailErrorFrom, Utility.ConfigData.DefaultMailErrorFromName, Utility.ConfigData.DefaultMailErrorTo, Utility.ConfigData.DefaultMailErrorToName, "Error 500", errorInfo, String.Empty)
                Else
                    Components.Email.SendError("ToError500", "Error 500", errorInfo)
                End If
               
                'If (errorInfo.Contains("Invalid character in a Base-64 string")) OrElse errorInfo.Contains("Invalid viewstate") Then
                '    If ctx.Request.Url.ToString().Contains("register.aspx") Then
                '        Response.Redirect("/members/RegisterAccount.aspx", True)
                '    Else
                '        Session("InvalidViewState") = errorInfo
                '        Response.Redirect("/ConfirmError.aspx", True)
                '    End If
                'ElseIf (errorInfo.Contains("A transport-level error has occurred when sending the request to the server")) Then
                '' Response.Redirect("/PageError.aspx", True)
                'End If
            End If
        End If
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
        Session.Remove("DepartmentURLCode")
        Session.Remove("KeywordSearchId")
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        Session.Remove("DepartmentURLCode")
        Session.Remove("KeywordSearchId")
    End Sub
    
    Sub Application_BeginRequest(ByVal sender As [Object], ByVal e As EventArgs)
       
    End Sub
</script>

