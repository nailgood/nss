<%@ WebHandler Language="VB" Class="sitemap_video" %>


Imports System
Imports System.Web
Imports System.Xml
Imports Components
Imports DataLayer
Imports System.Data.SqlClient

Public Class sitemap_video : Implements IHttpHandler
    Protected BP As New BasePage()
    Protected webRoot As String = ConfigurationManager.AppSettings("GlobalSecureName")
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim result As String = String.Empty
        result = "<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"" xmlns:video=""http://www.google.com/schemas/sitemap-video/1.1"">"
        
      
        result &= GetItemPath()
       
     
        result &= vbCrLf & "</urlset>"
        context.Response.ContentType = "text/xml"
        context.Response.Write(result)
    End Sub
    Private Function GetItemPath() As String
        Dim builder As New StringBuilder
        ' Dim ds As DataSet = StoreItemRow.GetAllItemActive(BP.DB)
        Dim r As SqlDataReader = BP.DB.GetReader("select * from Video where IsActive = 1") 'BP.DB.GetReader("select siv.*, si.ItemName, si.PriceDesc, si.ChoiceName from StoreItemVideo siv inner join StoreItem si on siv.ItemId = si.ItemId ")
        Dim Description As String = String.Empty
        Dim Title As String = String.Empty
        Dim ThumNail As String = String.Empty
        Dim PlayUrl As String = String.Empty
        Dim strLink As String = String.Empty
        Dim str As String = String.Empty
        While r.Read()
            Title = r("Title")
            ThumNail = webRoot & "/assets/video/Thumb/" & r("ThumbImage")
            Description = IIf(r("ShortDescription") = "", r("Title"), r("ShortDescription"))
            PlayUrl = r("VideoFile")
            strLink = webRoot & URLParameters.VideoDetailUrl(Title, r("VideoId"))
            str = vbCrLf & "<url>" & vbCrLf & _
             "<loc>" & strLink & "</loc>" & vbCrLf & _
             "<video:video>" & vbCrLf & _
             "<video:thumbnail_loc>" & ThumNail & "</video:thumbnail_loc>" & vbCrLf & _
             "<video:title><![CDATA[" & Title & "]]></video:title>" & vbCrLf & _
             "<video:description><![CDATA[" & Description & "]]></video:description>" & vbCrLf & _
             IIf(Right(PlayUrl, 4).Contains("."), "<video:content_loc>" & webRoot & PlayUrl & "</video:content_loc>", "<video:player_loc allow_embed=""yes"" autoplay=""ap=1""><![CDATA[" & PlayUrl & "]]></video:player_loc>") & vbCrLf & _
             "<video:rating>5.0</video:rating>" & vbCrLf & _
             "<video:uploader info=""http://www.youtube.com/nailsuperstore"">The Nail SuperStore</video:uploader>" & vbCrLf & _
             "</video:video>" & vbCrLf & _
             "</url>"
            builder.Append(str)
        End While
               
    
        Return builder.ToString()
    End Function
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class