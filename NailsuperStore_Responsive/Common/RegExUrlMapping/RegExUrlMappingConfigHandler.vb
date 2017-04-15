Imports Microsoft.VisualBasic
Imports System.Configuration
Imports System.Xml
Imports System.Text.RegularExpressions

'http://pietschsoft.com/blog/post.aspx?postid=762
Namespace RegExUrlMapping

    Public Class RegExUrlMappingConfigHandler
        Implements IConfigurationSectionHandler

        Dim m_Section As XmlNode

        Public Function Create(ByVal parent As Object, ByVal configContext As Object, ByVal section As System.Xml.XmlNode) As Object Implements System.Configuration.IConfigurationSectionHandler.Create
            m_Section = section
            Return Me
        End Function

        '''Get whether url mapping is enabled in the app.config
        Friend Function Enabled() As Boolean
            If m_Section.Attributes("enabled").Value.ToLower = "true" Then
                Return True
            Else
                Return False
            End If

        End Function

        ''' Get the matching "mapped Url" from the web.config file if there is one.
        Friend Function MappedUrl(ByVal url As String) As String
            Dim x As XmlNode
            Dim oReg As Regex

            For Each x In m_Section.ChildNodes
                oReg = New Regex(x.Attributes("url").Value.ToLower, RegexOptions.IgnoreCase)
                If oReg.Match(url).Success Then
                    Return oReg.Replace(url, x.Attributes("mappedUrl").Value)
                End If
            Next
            Return String.Empty
        End Function

    End Class

End Namespace

