<%@ WebHandler Language="VB" Class="register_check" %>

Imports System
Imports System.Web
Imports Components
Imports DataLayer

Public Class register_check : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim result As Integer = 0
        If context.Request.QueryString("act") <> Nothing Then
            If context.Request.QueryString("act") = "email" Then
                result = CheckEmail(context.Request.QueryString("key").Trim())
            ElseIf (context.Request.QueryString("act") = "pass") Then
                result = CheckPassword(context.Request.QueryString("key").Trim())
            Else
                result = CheckUsername(context.Request.QueryString("key").Trim())
            End If
        End If
        
        context.Response.ContentType = "text/plain"
        
       
        context.Response.Write(result.ToString())
       
        
    End Sub
    Private Function CheckPassword(ByVal password As String) As Integer
       
        If Utility.Common.IsPasswordValid(password) Then
            Return 1
        Else
            Return 0 ''pass not valid
        End If
    End Function
    Private Function CheckEmail(ByVal email As String) As Integer
        If Regex.IsMatch(email, "^(([^<>()[\]\\.,;:\s@\""]+(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$") Then
            Dim DB As New Database()
            DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
            
            Try
                Dim sCheckEmail As Integer = DB.ExecuteScalar("SELECT COUNT(MemberId) FROM Member WHERE CustomerId in (select customerid from customer where email = '" & email & "')")
                If sCheckEmail > 0 Then
                    Return 2 '' Email is exists                           
                    
                End If
                Dim sCheckUsername As Integer = DB.ExecuteScalar("SELECT COUNT(MemberId) FROM Member WHERE Username = '" & email & "'")
                If sCheckUsername > 0 Then
                    Return 3 ''User name is exist                    
                End If
                Return 4 ''check valid
            Catch ex As Exception
                Return 0 '' Unknow error
            Finally
                DB.Close()
            End Try
        Else
            Return 1 ''Email not valid
        End If
        
    End Function
    
    Private Function CheckUsername(ByVal userName As String) As Boolean
        If Regex.IsMatch(userName, "\w{4,20}") Then
            Dim DB As New BasePage()
            Try
                Dim sCheckUsername As Integer = DB.DB.ExecuteScalar("SELECT COUNT(MemberId) FROM Member WHERE Username = " & DB.DB.Quote(userName))
                If sCheckUsername > 0 Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Return True
            End Try
        Else
            Return True
        End If
        
    End Function
    
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class