Imports Components
Imports DataLayer

Partial Class controls_layout_menu_resource_center
    Inherits ModuleControl

    Public Overrides Property Args() As String
        Get
            Return m_Args
        End Get
        Set(ByVal Value As String)
            m_Args = Value
        End Set
    End Property
    Private m_Args As String
    Protected dvNews As DataView
    Protected dvVideo As DataView
    Protected cateID As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim result As New CategoryCollection
        Dim urlID As String = ""
        If LCase(Request.Url.ToString).Contains("news.aspx") = True Or LCase(Request.Url.ToString).Contains("video.aspx") = True Then
            Session("NewsCateId") = Nothing
            Session("VideoCateId") = Nothing
        End If
        If LCase(Request.Path.ToString).Contains("news") Then
            result = CategoryRow.ListByType(DB, Utility.Common.CategoryType.News)
            urlID = "news-topic"
            cateID = Session("NewsCateId")
        ElseIf LCase(Request.RawUrl.ToString).Contains("video") Then
            result = CategoryRow.GetAllVideoCategoryByType(DB, Utility.Common.CategoryType.Video)
            urlID = "video-topic"
            cateID = Session("VideoCateId")
            'ElseIf LCase(Request.RawUrl.ToString).Contains("media") Then
            '    result = CategoryRow.ListByType(DB, Utility.Common.CategoryType.MediaPress)
            '    urlID = "media-topic"
        End If
        If dvNews Is Nothing Then
            dvNews = New DataView
            If Not result Is Nothing AndAlso LCase(Request.Path.ToString).Contains("news") Then
                For Each item As CategoryRow In result
                    If LCase(item.CategoryName) <> "blog" Then
                        AddRow(dvNews, item.CategoryId, "/" & urlID & "/" & HttpUtility.UrlEncode(RewriteUrl.ReplaceUrl(item.CategoryName.ToLower())) & "/" & item.CategoryId, item.CategoryName)
                    End If
                Next
            End If
        End If
        If dvVideo Is Nothing Then
            dvVideo = New DataView
            If Not result Is Nothing AndAlso LCase(Request.RawUrl.ToString).Contains("video") Then
                For Each item As CategoryRow In result
                    AddRow(dvVideo, item.CategoryId, "/" & urlID & "/" & HttpUtility.UrlEncode(RewriteUrl.ReplaceUrl(item.CategoryName.ToLower())) & "/" & item.CategoryId, item.CategoryName)
                Next
            End If
        End If
    End Sub

    Protected Sub AddRow(ByRef dv As DataView, ByVal Id As Integer, ByVal Url As String, ByVal Text As String)
        If dv.Table Is Nothing Then
            Dim dt As New DataTable
            dt.TableName = "tbl"
            dt.Columns.Add("Id", GetType(Integer))
            dt.Columns.Add("Url", GetType(String))
            dt.Columns.Add("Text", GetType(String))
            dv.Table = dt
        End If

        Dim row As DataRow = dv.Table.NewRow
        row("Id") = ID
        row("Url") = Url
        row("Text") = Text
        dv.Table.Rows.Add(row)
    End Sub

    Public Function GenerateGroupMenu(ByVal groupCode As String, ByVal groupName As String, ByVal linkDefault As String) As String
        Dim result As String = String.Empty
        Dim dv As DataView = GetMenuSource(groupCode)
        Dim childClass As String = "sub"
        'Or ((linkDefault.Contains("video") Or linkDefault.Contains("news")) AndAlso Request.RawUrl.ToLower.Contains("-detail"))
        If (Request.RawUrl.ToLower.Contains(linkDefault) Or (linkDefault.Contains("nail-art-trend") And Request.RawUrl.ToLower.Contains("nail-art-trend"))) Then
            result = result & "<li class='active'><a href='" & linkDefault & "'>" & groupName & "<span class=""arrow""></span></a>" & IIf(LCase(groupName) = "customer reviews", "<span class=""content-rating""></span>", "")
        ElseIf (ChildActive(groupCode)) Then
            result = result & "<li class='active'><a href='" & linkDefault & "'>" & groupName & "<span class=""arrow""></span></a>"
        Else
            result = result & "<li><a href='" & linkDefault & "'>" & groupName & "<span class=""arrow""></span></a>"
            childClass = childClass & " hiddenshow"
        End If
        result = result & ListSub(groupCode, childClass) & "</li>"
        Return result
    End Function

    Private Function GetMenuSource(ByVal groupCode As String) As DataView
        Dim dv As New DataView
        Select Case groupCode
            Case "news"
                dv = dvNews
            Case "video"
                dv = dvVideo
        End Select
        Return dv
    End Function

    Private Function ChildActive(ByVal groupCode As String) As Boolean
        If (String.IsNullOrEmpty(groupCode)) Then
            Return False
        End If
        Dim dv As DataView = GetMenuSource(groupCode)
        Dim str As String = String.Empty
        If UrlIn(dv, Request.RawUrl.ToLower) Then
            For Each row As DataRowView In dv
                If Not row("Url") = groupCode & "-topic" Then
                    If Not Request.RawUrl.ToLower = row("Url") Then
                        Return True
                    End If
                End If
            Next
        End If
        Return False
    End Function

    Protected Function UrlIn(ByVal dv As DataView, ByVal currentFilePath As String) As Boolean
        For Each row As DataRowView In dv
            If row("Url").ToLower = currentFilePath Or row("Id") = cateID Then
                Return True
            End If
        Next
        Return False
    End Function

    Protected Function ListSub(ByVal dvName As String, ByVal className As String) As String
        Dim dv As New DataView
        Select Case dvName
            Case "news"
                dv = dvNews
            Case "video"
                dv = dvVideo
        End Select
        Dim currentPathFile As String = Request.RawUrl.ToLower
        Dim str As String = String.Empty
        If dv Is Nothing Then
            Return String.Empty
        End If
        If dv.Count < 1 Then
            Return String.Empty
        End If
        str &= "<ul class='" & className & "'>"
        For Each row As DataRowView In dv
            If Not row("Url") = dvName & "-topic" Then
                If Not currentPathFile = row("Url") AndAlso row("Id") <> cateID Then
                    str &= "<li ><span><b class='glyphicon arrow-right'></b></span><span><a href=""" & row("Url") & """>" & row("Text") & "</a></span></li>"
                Else
                    If LCase(Request.RawUrl).Contains("video-detail") Or LCase(Request.RawUrl).Contains("news-detail") Then
                        str &= "<li class=""select""><span><b class='glyphicon arrow-right'></b></span><span><a href=""" & row("Url") & """ class=""hand"">" & row("Text") & "</a></span></li>"
                    Else
                        str &= "<li class=""select""><span><b class='glyphicon arrow-right'></b></span><span><a>" & row("Text") & "</a></span></li>"
                    End If

                End If
            End If
        Next
        str &= "</ul>"

        Return str
    End Function
End Class
