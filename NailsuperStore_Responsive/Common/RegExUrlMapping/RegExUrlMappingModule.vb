Imports Microsoft.VisualBasic
Imports System.Web
Imports System.Configuration

'http://pietschsoft.com/blog/post.aspx?postid=762
Namespace RegExUrlMapping

    Public Class RegExUrlMappingModule
        Inherits RegExUrlMappingBaseModule

        Overrides Sub Rewrite(ByVal requestedPath As String, ByVal app As HttpApplication)
            'Implement functionality here that mimics the 'URL Mapping' features of ASP.NET 2.0
            Dim config As RegExUrlMappingConfigHandler = CType(ConfigurationManager.GetSection("system.web/RegExUrlMapping"), RegExUrlMappingConfigHandler)
            If Not config.Enabled Then Exit Sub

            Dim pathOld As String = app.Request.RawUrl

            'Get the request page without the querystring parameters
            Dim requestedPage As String = app.Request.RawUrl
            If requestedPage.IndexOf("?") > -1 Then
                requestedPage = requestedPage.Substring(0, requestedPage.IndexOf("?"))
            End If

            'Format the requested page (url) to have a ~ instead of the virtual path of the app
            Dim appVirtualPath As String = app.Request.ApplicationPath
            If requestedPage.Length >= appVirtualPath.Length Then
                If requestedPage.Substring(0, appVirtualPath.Length).ToLower = appVirtualPath.ToLower Then
                    requestedPage = requestedPage.Substring(appVirtualPath.Length)
                    If requestedPage.Substring(0, 1) = "/" Then
                        requestedPage = "~" & requestedPage
                    Else
                        requestedPage = "~/" & requestedPage
                    End If
                End If
            End If

            'Get the new path to rewrite the url to if it meets one of the defined virtual urls.
            Dim pathNew As String = config.MappedUrl(requestedPage)

            'If the requested url matches one of the virtual one the lets go and rewrite it.
            If pathNew.Length > 0 Then
                If pathNew.IndexOf("?") > -1 Then
                    'The matched page has a querystring defined
                    If pathOld.IndexOf("?") > -1 Then
                        pathNew += "&" & Right(pathOld, pathOld.Length - pathOld.IndexOf("?") - 1)
                    End If
                Else
                    'The matched page doesn't have a querystring defined
                    If pathOld.IndexOf("?") > -1 Then
                        pathNew += Right(pathOld, pathOld.Length - pathOld.IndexOf("?"))
                    End If
                End If
                'Rewrite to the new url
                HttpContext.Current.RewritePath(pathNew)
            End If
        End Sub
    End Class

End Namespace

