Imports System.Collections.Generic
Imports System.Text.RegularExpressions

Namespace Components
    ''' <summary>
    ''' BBCode Helper allows formatting of text
    ''' without the need to use html
    ''' </summary>
    Public Class BBCodeHelper

#Region "Helper Classes"
        Private Interface IHtmlFormatter
            Function Format(ByVal data As String) As String
        End Interface

        Protected Class RegexFormatter
            Implements IHtmlFormatter
            Private _replace As String
            Private _regex As Regex

            Public Sub New(ByVal pattern As String, ByVal replace As String)

                Me.New(pattern, replace, True)
            End Sub

            Public Sub New(ByVal pattern As String, ByVal replace As String, ByVal ignoreCase As Boolean)
                Dim options As RegexOptions = RegexOptions.Compiled

                If ignoreCase Then
                    options = options Or RegexOptions.IgnoreCase
                End If

                _replace = replace
                _regex = New Regex(pattern, options)
            End Sub

            Public Function Format(ByVal data As String) As String Implements BBCodeHelper.IHtmlFormatter.Format
                Return _regex.Replace(data, _replace)
            End Function
        End Class

        Protected Class SearchReplaceFormatter
            Implements IHtmlFormatter
            Private _pattern As String
            Private _replace As String

            Public Sub New(ByVal pattern As String, ByVal replace As String)
                _pattern = pattern
                _replace = replace
            End Sub

            Public Function Format(ByVal data As String) As String Implements BBCodeHelper.IHtmlFormatter.Format
                Return data.Replace(_pattern, _replace)
            End Function
        End Class
#End Region

#Region "BBCode"
        Shared _formatters As List(Of IHtmlFormatter)

        Shared Sub New()
            _formatters = New List(Of IHtmlFormatter)()

            _formatters.Add(New RegexFormatter("<(.|\n)*?>", String.Empty))

            _formatters.Add(New SearchReplaceFormatter(vbCr, ""))
            _formatters.Add(New SearchReplaceFormatter(vbLf & vbLf, "</p><p>"))
            _formatters.Add(New SearchReplaceFormatter(vbLf, "<br />"))

            _formatters.Add(New RegexFormatter("\[b(?:\s*)\]((.|\n)*?)\[/b(?:\s*)\]", "<b>$1</b>"))
            _formatters.Add(New RegexFormatter("\[i(?:\s*)\]((.|\n)*?)\[/i(?:\s*)\]", "<i>$1</i>"))
            _formatters.Add(New RegexFormatter("\[s(?:\s*)\]((.|\n)*?)\[/s(?:\s*)\]", "<strike>$1</strike>"))
            _formatters.Add(New RegexFormatter("\[u(?:\s*)\]((.|\n)*?)\[/u(?:\s*)\]", "<u>$1</u>"))

            _formatters.Add(New RegexFormatter("\[left(?:\s*)\]((.|\n)*?)\[/left(?:\s*)]", "<div style=""text-align:left"">$1</div>"))
            _formatters.Add(New RegexFormatter("\[center(?:\s*)\]((.|\n)*?)\[/center(?:\s*)]", "<div style=""text-align:center"">$1</div>"))
            _formatters.Add(New RegexFormatter("\[right(?:\s*)\]((.|\n)*?)\[/right(?:\s*)]", "<div style=""text-align:right"">$1</div>"))

            Dim quoteStart As String = "<blockquote><b>$1 said:</b></p><p>"
            Dim quoteEmptyStart As String = "<blockquote>"
            Dim quoteEnd As String = "</blockquote>"

            _formatters.Add(New RegexFormatter("\[quote=((.|\n)*?)(?:\s*)\]", quoteStart))
            _formatters.Add(New RegexFormatter("\[quote(?:\s*)\]", quoteEmptyStart))
            _formatters.Add(New RegexFormatter("\[/quote(?:\s*)\]", quoteEnd))

            _formatters.Add(New RegexFormatter("\[url(?:\s*)\]www\.(.*?)\[/url(?:\s*)\]", "<a class=""bbcode-link"" href=""http://www.$1"" target=""_blank"" title=""$1"">$1</a>"))
            _formatters.Add(New RegexFormatter("\[url(?:\s*)\]((.|\n)*?)\[/url(?:\s*)\]", "<a class=""bbcode-link"" href=""$1"" target=""_blank"" title=""$1"">$1</a>"))
            _formatters.Add(New RegexFormatter("\[url=""((.|\n)*?)(?:\s*)""\]((.|\n)*?)\[/url(?:\s*)\]", "<a class=""bbcode-link"" href=""$1"" target=""_blank"" title=""$1"">$3</a>"))
            _formatters.Add(New RegexFormatter("\[url=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/url(?:\s*)\]", "<a class=""bbcode-link"" href=""$1"" target=""_blank"" title=""$1"">$3</a>"))
            _formatters.Add(New RegexFormatter("\[link(?:\s*)\]((.|\n)*?)\[/link(?:\s*)\]", "<a class=""bbcode-link"" href=""$1"" target=""_blank"" title=""$1"">$1</a>"))
            _formatters.Add(New RegexFormatter("\[link=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/link(?:\s*)\]", "<a class=""bbcode-link"" href=""$1"" target=""_blank"" title=""$1"">$3</a>"))

            _formatters.Add(New RegexFormatter("\[img(?:\s*)\]((.|\n)*?)\[/img(?:\s*)\]", "<img src=""$1"" border=""0"" alt="""" />"))
            _formatters.Add(New RegexFormatter("\[img align=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/img(?:\s*)\]", "<img src=""$3"" border=""0"" align=""$1"" alt="""" />"))
            _formatters.Add(New RegexFormatter("\[img=((.|\n)*?)x((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/img(?:\s*)\]", "<img width=""$1"" height=""$3"" src=""$5"" border=""0"" alt="""" />"))

            _formatters.Add(New RegexFormatter("\[color=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/color(?:\s*)\]", "<span style=""color=$1;"">$3</span>"))

            _formatters.Add(New RegexFormatter("\[hr(?:\s*)\]", "<hr />"))

            _formatters.Add(New RegexFormatter("\[email(?:\s*)\]((.|\n)*?)\[/email(?:\s*)\]", "<a href=""mailto:$1"">$1</a>"))

            _formatters.Add(New RegexFormatter("\[size=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/size(?:\s*)\]", "<span style=""font-size:$1"">$3</span>"))
            _formatters.Add(New RegexFormatter("\[font=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/font(?:\s*)\]", "<span style=""font-family:$1;"">$3</span>"))
            _formatters.Add(New RegexFormatter("\[align=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/align(?:\s*)\]", "<span style=""text-align:$1;"">$3</span>"))
            _formatters.Add(New RegexFormatter("\[float=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/float(?:\s*)\]", "<span style=""float:$1;"">$3</div>"))

            Dim sListFormat As String = "<ol class=""bbcode-list"" style=""list-style:{0};"">$1</ol>"

            _formatters.Add(New RegexFormatter("\[\*(?:\s*)]\s*([^\[]*)", "<li>$1</li>"))
            _formatters.Add(New RegexFormatter("\[list(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", "<ul>$1</ul>"))
            _formatters.Add(New RegexFormatter("\[list=1(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", String.Format(sListFormat, "decimal"), False))
            _formatters.Add(New RegexFormatter("\[list=i(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", String.Format(sListFormat, "lower-roman"), False))
            _formatters.Add(New RegexFormatter("\[list=I(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", String.Format(sListFormat, "upper-roman"), False))
            _formatters.Add(New RegexFormatter("\[list=a(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", String.Format(sListFormat, "lower-alpha"), False))
            _formatters.Add(New RegexFormatter("\[list=A(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", String.Format(sListFormat, "upper-alpha"), False))
        End Sub
#End Region

#Region "Format"
        Public Shared Function Format(ByVal data As String) As String
            If String.IsNullOrEmpty(data) Then
                Return String.Empty
            Else

                For Each formatter As IHtmlFormatter In _formatters
                    data = formatter.Format(data).Replace("\n", "<br>")
                Next

                Return data
            End If


        End Function
        Public Shared Function ConvertBBCodeToDB(ByVal str As String) As String
            Dim result As String = String.Empty
            If str Is Nothing Or str = String.Empty Then
                Return String.Empty
            End If
            ''result = str.Replace("<span style='padding-left: 10px;'>•</span>", "/*")
            result = System.Text.RegularExpressions.Regex.Replace(str, "<\/?span[^>]*>", String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = result.Replace("•", "/*")
            result = result.Replace(vbCrLf, "[br]")
            Return result
        End Function
        Public Shared Function ConvertBBCodeToBBCodeEditor(ByVal str As String) As String
            If str Is Nothing Or str = String.Empty Then
                Return String.Empty
            End If
            str = str.Replace("[br]", "<br>")
            Return str.Replace("/*", "<span style='padding-right:5px'>&bull;</span>")
        End Function
        Public Shared Function ConvertBBCodeToHTML(ByVal str As String) As String


            If str Is Nothing Or str = String.Empty Then
                Return String.Empty
            End If
            str = str.Replace("•", "*")
            str = str.Replace(vbCr, "[vbCr]")
            str = str.Replace(vbLf, "[vbLf]")
            str = str.Replace("[vbCr][vbLf]*", Environment.NewLine & "*")
            str = str.Replace("[vbLf][vbCr]*", Environment.NewLine & "*")
            str = str.Replace("[vbCr][vbLf]/*", Environment.NewLine & "*")
            str = str.Replace("[vbLf][vbCr]/*", Environment.NewLine & "*")
            '' str = str.Replace("[vbCr][vbLf]", String.Empty)
            str = str.Replace("[vbCr]*", Environment.NewLine & "*")
            str = str.Replace("[vbLf]*", Environment.NewLine & "*")
            str = str.Replace("[vbCr]", vbCr)
            str = str.Replace("[vbLf]", vbLf)
            Dim exp As Regex
            ''''''''''


            ''''''''''''
            ' format the bold tags: [b][/b]
            ' becomes: <strong></strong>
            exp = New Regex("\[b\](.+?)\[/b\]")
            str = exp.Replace(str, "<strong>$1</strong>")

            ' format the italic tags: [i][/i]
            ' becomes: <em></em>
            exp = New Regex("\[i\](.+?)\[/i\]")
            str = exp.Replace(str, "<em>$1</em>")

            ' format the underline tags: [u][/u]
            ' becomes: <u></u>
            exp = New Regex("\[u\](.+?)\[/u\]")
            str = exp.Replace(str, "<u>$1</u>")

            ' format the strike tags: [s][/s]
            ' becomes: <strike></strike>
            exp = New Regex("\[s\](.+?)\[/s\]")
            str = exp.Replace(str, "<strike>$1</strike>")

            ' format the url tags: [url=www.website.com]my site[/url]
            ' becomes: <a href="www.website.com">my site</a>
            exp = New Regex("\[url\=([^\]]+)\]([^\]]+)\[/url\]")
            str = exp.Replace(str, "<a href=""$1"">$2</a>")

            ' format the img tags: [img]www.website.com/img/image.jpeg[/img]
            ' becomes: <img src="www.website.com/img/image.jpeg" />
            exp = New Regex("\[img\]([^\]]+)\[/img\]")
            str = exp.Replace(str, "<img src=""$1"" />")

            ' format img tags with alt: [img=www.website.com/img/image.jpeg]this is the alt text[/img]
            ' becomes: <img src="www.website.com/img/image.jpeg" alt="this is the alt text" />
            exp = New Regex("\[img\=([^\]]+)\]([^\]]+)\[/img\]")
            str = exp.Replace(str, "<img src=""$1"" alt=""$2"" />")

            'format the colour tags: [color=red][/color]
            ' becomes: <font color="red"></font>
            ' supports UK English and US English spelling of colour/color
            exp = New Regex("\[color\=([^\]]+)\]([^\]]+)\[/color\]")
            str = exp.Replace(str, "<font color=""$1"">$2</font>")
            exp = New Regex("\[colour\=([^\]]+)\]([^\]]+)\[/colour\]")
            str = exp.Replace(str, "<font color=""$1"">$2</font>")

            ' format the size tags: [size=3][/size]
            ' becomes: <font size="+3"></font>
            exp = New Regex("\[size\=([^\]]+)\]([^\]]+)\[/size\]")
            str = exp.Replace(str, "<font size=""+$1"">$2</font>")
            ''format list style 



            Dim firstCharacter As String = str.Substring(0, 1)
            If (firstCharacter = "/") Then
                str = str.Substring(1, str.Length - 1)
                firstCharacter = str.Substring(0, 1)
            End If
            str = str.Replace("[br]/*", Environment.NewLine & "/*")

            str = str.Replace(Environment.NewLine & "*", Environment.NewLine & "/*")
            If (firstCharacter = "*") Then
                str = "/" & str
            End If
            If (str.Contains("/*")) Then
                Dim lastCharacter As String = str.Substring(str.Length - 1, 1)
                If (lastCharacter <> Environment.NewLine) Then
                    str = str & Environment.NewLine
                End If
            End If
            exp = New Regex("\/\*(.+?)\" & Environment.NewLine)
            str = exp.Replace(str, "<div class='listBBCode'>$1</div>")

            str = str.Replace("[br]", "<br>")
            str = str.Replace(Environment.NewLine, "<br>")
            str = str.Replace("\r\n", "<br>")
            str = str.Replace("\n", "<br>")
            str = str.Replace("\r", "<br>")
            Return str ''.Replace(vbCr, "<br/>")
        End Function

        Public Shared Function ReplaceBBCode(ByVal str As String) As String


            If str Is Nothing Or str = String.Empty Then
                Return String.Empty
            End If
            str = str.Replace(".", ",")
            str = str.Replace("[br]", ",")
            str = str.Replace("•", "")
            str = str.Replace(":", "")
            str = str.Replace(",,,", ",")
            str = str.Replace(",,", ",")

            Return str
        End Function
        Public Shared Function StripHtml(ByVal str As String)
            Return Regex.Replace(str, "<(.|\\n)*?>", String.Empty)
        End Function
#End Region
    End Class
End Namespace