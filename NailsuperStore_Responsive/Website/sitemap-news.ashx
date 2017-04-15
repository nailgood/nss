<%@ WebHandler Language="VB" Class="sitemap_news" %>

Imports System
Imports System.Web
Imports System.Xml
Imports Components
Imports DataLayer
Public Class sitemap_news : Implements IHttpHandler
    Protected BP As New BasePage()
    Protected webRoot As String = ConfigurationManager.AppSettings("GlobalSecureName")
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim result As String = String.Empty
        result = "<?xml version=""1.0"" encoding=""UTF-8""?>" & vbCrLf & _
        "<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"" xmlns:image=""http://www.google.com/schemas/sitemap-image/1.1"">"
        '"<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"" xmlns:news=""http://www.google.com/schemas/sitemap-news/0.9"">"
        result &= "<url>" & vbCrLf & _
                       "<loc>" & webRoot & "/news-topic</loc>" & vbCrLf & _
                       "<changefreq>daily</changefreq>" & vbCrLf & _
                  "</url>" & vbCrLf & _
                  "<url>" & vbCrLf & _
                       "<loc>" & webRoot & "/media-topic</loc>" & vbCrLf & _
                       "<changefreq>daily</changefreq>" & vbCrLf & _
                  "</url>"
        result &= GetNewsPath()
        result &= vbCrLf & "</urlset>"
        context.Response.ContentType = "text/xml"
        context.Response.Write(result)
    End Sub
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
    Private Function GetNewsPath() As String
        Dim builder As New StringBuilder
        Dim ds As DataSet = NewsRow.ListNewsForSitemap()
        Dim strMenu As String = String.Empty
        Dim strLink As String = String.Empty
        Dim strImage As String = String.Empty
        Dim strTitle As String = String.Empty
        Dim Id As Integer = 0
        Dim IsBlog As Boolean = False
        Dim IsMediaPress As Boolean = False
        Dim CategoryType As Integer = 0
        If Not ds Is Nothing AndAlso ds.Tables.Count > 0 Then
            If Not ds.Tables(0) Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                For i As Int16 = 0 To ds.Tables(0).Rows.Count - 1 Step i + 1
                    Dim dr As DataRow = ds.Tables(0).Rows(i)
                    Id = IIf(dr("ID") Is Nothing, 0, Convert.ToInt32(dr("ID")))
                    CategoryType = IIf(dr("CategoryType") Is Nothing, 0, Convert.ToInt32(dr("CategoryType")))
                    IsBlog = IIf(dr("IsBlog") Is Nothing, False, Convert.ToBoolean(dr("IsBlog")))
                    IsMediaPress = IIf(dr("IsMediaPress") Is Nothing, False, Convert.ToBoolean(dr("IsMediaPress")))
                    strTitle = IIf(dr("Title") Is Nothing, "", dr("Title").ToString())
                    Dim img As String = IIf(dr("ThumbImage") Is Nothing, "", dr("ThumbImage").ToString())
                    Dim bAddFreq As Boolean = False
                    If CategoryType = 1 Then
                        If strTitle.ToLower().Equals("blog") Then
                            strLink = String.Format("{0}", webRoot & "/blog")
                            strImage = String.Empty
                            bAddFreq = True
                        Else
                            strLink = String.Format("{0}", webRoot & "/news-topic/" & HttpUtility.UrlEncode(RewriteUrl.ReplaceUrl(strTitle.ToLower())) & "/" & Id)
                            strImage = String.Empty
                            bAddFreq = True
                        End If
                    ElseIf CategoryType = 3 Then
                        strLink = String.Format("{0}", webRoot & "/media-topic/" & HttpUtility.UrlEncode(RewriteUrl.ReplaceUrl(strTitle.ToLower())) & "/" & Id)
                        strImage = String.Empty
                        bAddFreq = True
                    ElseIf IsMediaPress Then
                        strLink = String.Format("{0}", webRoot & URLParameters.MediaDetailUrl(strTitle, Id))
                        strImage = String.Format("{0}", IIf(String.IsNullOrEmpty(img), "", webRoot & "/" & Utility.ConfigData.MediaThumbPath & img))
                    ElseIf Not IsBlog Then
                        strLink = String.Format("{0}", webRoot & URLParameters.NewsDetailUrl(strTitle, Id))
                        strImage = String.Format("{0}", IIf(String.IsNullOrEmpty(img), "", webRoot & "/" & Utility.ConfigData.PathSmallNewsImage & img))
                    Else
                        strLink = String.Format("{0}", webRoot & URLParameters.BlogDetailUrl(strTitle, Id))
                        strImage = String.Format("{0}", IIf(String.IsNullOrEmpty(img), "", webRoot & Utility.ConfigData.PathThumbBlogImage & img))
                    End If
                    strMenu = vbCrLf & "<url>" & vbCrLf & _
                    "<loc>" & strLink & "</loc>" & vbCrLf
                    If bAddFreq Then
                        strMenu &= "<changefreq>daily</changefreq>" & vbCrLf
                    End If
                    
                    If Not String.IsNullOrEmpty(strImage) Then
                        strMenu &= "<image:image>" & vbCrLf & _
                    "<image:loc>" & strImage & "</image:loc>" & vbCrLf & _
                    "</image:image>" & vbCrLf
                    End If
                    
                    strMenu &= "</url>"
                    ''"<news:news>" & vbCrLf & _
                    ''"<news:title>" & strTitle & "</news:title>" & vbCrLf & _
                    ''"<news:image>" & strImage & "</news:image>" & vbCrLf & _
                    ''"</news:news>" & vbCrLf & _
                    ''"</url>"

                    builder.Append(strMenu)
                Next
            End If
            ds.Dispose()
        End If
        Return builder.ToString()
    End Function
   
End Class