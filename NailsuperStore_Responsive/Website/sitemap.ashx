<%@ WebHandler Language="VB" Class="sitemap" %>

Imports System
Imports System.Web
Imports System.Xml
Imports Components
Imports DataLayer
Public Class sitemap : Implements IHttpHandler
    Protected BP As New BasePage()
    Protected webRoot As String = ConfigurationManager.AppSettings("GlobalSecureName")
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim result As String = String.Empty
        result = "<?xml version=""1.0"" encoding=""UTF-8""?>" & vbCrLf & _
        "<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"" xmlns:image=""http://www.google.com/schemas/sitemap-image/1.1"">"

        If (context.Request.QueryString("t") <> Nothing) Then
            If (context.Request.QueryString("t") = "item") Then
                result &= GetItemPath()
            Else
                result &= GetCategoryPath()
            End If
        Else
            result &= GetCategoryPath()
            result &= GetItemPath()
        End If

        result &= vbCrLf & "</urlset>"
        context.Response.ContentType = "text/xml"
        context.Response.Write(result)
    End Sub
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
    Private Function GetItemPath() As String
        Dim builder As New StringBuilder
        Dim ds As DataSet = StoreItemRow.GetAllItemActive(BP.DB)
        Dim strMenu As String = String.Empty
        Dim strLink As String = String.Empty
        Dim strImage As String = String.Empty
        Dim strUrlCode As String = String.Empty

        If Not ds Is Nothing AndAlso ds.Tables.Count > 0 Then
            If Not ds.Tables(0) Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                For i As Int16 = 0 To ds.Tables(0).Rows.Count - 1 Step i + 1
                    Dim dr As DataRow = ds.Tables(0).Rows(i)
                    '' strLink = String.Format("<a href=""{0}"">{1}</a>", SitePage.MainDepartmentUrl(dr("Name"), dr("DepartmentId")), RewriteUrl.ReplaceUrl(dr("Name")))
                    strUrlCode = IIf(dr("URLCode") Is Nothing, "", dr("URLCode").ToString())
                    strLink = String.Format("{0}", webRoot & URLParameters.ProductUrl(strUrlCode, dr("ItemId").ToString()))
                    strImage = String.Format("{0}", webRoot & "/assets/items/large/" & dr("Image"))
                    strMenu = vbCrLf & "<url>" & vbCrLf &
                    "<loc>" & strLink & "</loc>" & vbCrLf &
                    "<image:image>" & vbCrLf &
                    "<image:loc>" & strImage & "</image:loc>" & vbCrLf &
                    "</image:image>" & vbCrLf &
                    "<lastmod>2016-03-01</lastmod>" & vbCrLf &
                    "<changefreq>weekly</changefreq>" & vbCrLf &
                    "<priority>1.0</priority>" & vbCrLf &
                    "</url>"
                    builder.Append(strMenu)
                Next
            End If
            ds.Dispose()
        End If
        Return builder.ToString()
    End Function
    Private Function GetCategoryPath() As String
        Dim builder As New StringBuilder
        Dim ds As DataSet = StoreDepartmentRow.GetAllChildDepartments(BP.DB)
        Dim strMenu As String = String.Empty
        Dim strLink As String = String.Empty
        If Not ds Is Nothing AndAlso ds.Tables.Count > 0 Then
            If Not ds.Tables(0) Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                For i As Int16 = 0 To ds.Tables(0).Rows.Count - 1 Step i + 1
                    Dim dr As DataRow = ds.Tables(0).Rows(i)
                    If dr("ParentId") = "23" Then
                        strLink = webRoot & URLParameters.MainDepartmentUrl(dr("URLCode"), dr("DepartmentId"))
                    Else
                        strLink = webRoot & URLParameters.DepartmentUrl(dr("URLCode"), dr("DepartmentId"))
                    End If

                    strMenu = vbCrLf & "<url>" & vbCrLf & _
                    "<loc>" & strLink & "</loc>" & vbCrLf & _
                    "<priority>0.9</priority>" & vbCrLf & _
                    "<changefreq>daily</changefreq>" & vbCrLf & _
                    "</url>"
                    builder.Append(strMenu)
                Next
            End If
            ds.Dispose()
        End If
        Return builder.ToString()
    End Function
End Class