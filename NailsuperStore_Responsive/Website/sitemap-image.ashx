<%@ WebHandler Language="VB" Class="sitemap_image" %>

Imports System
Imports System.Web
Imports System.Xml
Imports Components
Imports DataLayer

Public Class sitemap_image : Implements IHttpHandler
    Protected BP As New BasePage()
    Protected webRoot As String = ConfigurationManager.AppSettings("GlobalSecureName")
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
   
        Dim result As String = String.Empty
        result = "<?xml version=""1.0"" encoding=""UTF-8""?>" & vbCrLf & _
        "<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"" xmlns:image=""http://www.google.com/schemas/sitemap-image/1.1"">" & vbCrLf & _
        "<url><loc>https://www.nailsuperstore.com</loc>"
        result &= GetImagePath()
        result &= vbCrLf & "</url>" & vbCrLf & _
       "</urlset>"
        context.Response.ContentType = "text/xml"
        context.Response.Write(result)
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
    Private Function GetImagePath() As String
        Dim builder As New StringBuilder
        Dim ds As DataSet = StoreItemRow.GetAllItemActive(BP.DB)
        Dim LinkImage As String = String.Empty
        Dim TitleImage As String = String.Empty
        Dim strContent As String = String.Empty
        
        If Not ds Is Nothing AndAlso ds.Tables.Count > 0 Then
            If Not ds.Tables(0) Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                For i As Int16 = 0 To ds.Tables(0).Rows.Count - 1 Step i + 1
                    Dim dr As DataRow = ds.Tables(0).Rows(i)
                    '' strLink = String.Format("<a href=""{0}"">{1}</a>", SitePage.MainDepartmentUrl(dr("Name"), dr("DepartmentId")), RewriteUrl.ReplaceUrl(dr("Name")))
                    LinkImage = webRoot & "/assets/items/" & dr("Image").ToString()
                    TitleImage = dr("ItemName")
                    strContent = vbCrLf & "<image:image>" & vbCrLf & _
                    "<image:loc>" & LinkImage & "</image:loc>" & vbCrLf & _
                    "<image:title><![CDATA[" & TitleImage & "]]></image:title>" & vbCrLf & _
                    "</image:image>"
                    builder.Append(strContent)
                Next
            End If
            ds.Dispose()
        End If
        Return builder.ToString()
    End Function
End Class