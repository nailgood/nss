<%@ WebHandler Language="VB" Class="SolrURL" %>

Imports System
Imports System.Web
Imports System.Net
Imports System.IO

Public Class SolrURL : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim query As String = context.Request.QueryString("q")
        
        If String.IsNullOrEmpty(query) Then
            context.Response.ContentType = "application/json"
            context.Response.Write("{}")
            Return
        End If
        
        Dim url As String = Utility.ConfigData.SolrServerURL + query
        Dim req As HttpWebRequest = WebRequest.Create(url)
        Dim res As HttpWebResponse = req.GetResponse()
        
        Dim dataStream As Stream = res.GetResponseStream()
        Dim reader As StreamReader = New StreamReader(dataStream)
        Dim responseFromServer As String = reader.ReadToEnd()

        context.Response.Write(responseFromServer.Replace("?({""responseHeader", "{""responseHeader").Remove(responseFromServer.Length, 1))
        
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class