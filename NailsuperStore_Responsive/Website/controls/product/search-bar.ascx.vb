Imports Components
Imports DataLayer
Imports System.Web.Services

Partial Class modules_SearchBar
    Inherits ModuleControl
    Protected strSearch As String = ""
    Protected up As String = Utility.ConfigData.urlProduct

    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property
    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        If Not Request("print") = Nothing Then
            Me.Visible = False
        End If
        If Not IsPostBack Then

            'If NOt  Request("kw") is nothing then
            If Not Request.Form("LookupField") Is Nothing Then
                strSearch = Request.Form("LookupField").ToString
            Else
                strSearch = "Search by keyword or item #"
            End If
        End If

    End Sub

    Protected Sub lnkSearch_Click(sender As Object, e As System.EventArgs) Handles lnkSearch.Click

        'If Not Request.Form("LookupHidden") = Nothing Then
        '    Response.Redirect("/nail-products/" + Request.Form("LookupHidden").ToString.Trim())
        'End If

        Dim strKw As String = ""
        If Not Request.Form("LookupField") Is Nothing Then
            strKw = Utility.Common.CheckSearchKeyword(Request.Form("LookupField").ToString.Trim())
            strSearch = strKw
        End If

        LogSearch(strKw)
        GetKeywordSearch(strKw)
        If strKw.Length < 1 Then
            Response.Redirect(Request.UrlReferrer.ToString)
        Else
            Session("DepartmentURLCode") = Nothing
            DB.Close()
            ''check redirect keyword
            If (Not Request.Form("LookupKeyword") = Nothing) Then
                Response.Redirect("/store/search-result.aspx?kw=" & Server.UrlEncode(strSearch) & "&F_All=Y&F_Search=Y")
            Else
                Dim redirectLink As String = KeywordRedirectRow.GetLinkRedirect(strSearch.Trim())
                If Not String.IsNullOrEmpty(redirectLink) Then
                    Response.Redirect(redirectLink)
                Else
                    Response.Redirect("/store/search-result.aspx?kw=" & Server.UrlEncode(strSearch) & "&F_All=Y&F_Search=Y")
                End If

            End If
        End If
    End Sub

    Private Sub LogSearch(ByVal keyword As String)
        Dim strPath As String = Utility.ConfigData.LogSearchFilePath
        Dim sContents As String = ""
        Try
            If Core.FileExists(strPath) = True Then
                sContents = Core.OpenFile(strPath)
            End If
            sContents = sContents & Date.Now() & "[" & keyword & "]" & vbCrLf
            Core.WriteFile(strPath, sContents)
        Catch ex As Exception

        End Try
    End Sub

    Private Function IsKeywordSKU(ByVal keyword As String) As Boolean
        Dim result As Integer
        Try
            result = CInt(keyword)
            Return True
        Catch ex As Exception

        End Try
        Return False
    End Function

    Private Sub GetKeywordSearch(ByVal keyword As String)
        If (keyword.Trim() = "Search by keyword or item") Then
            Exit Sub
        End If
        If (IsKeywordSKU(keyword)) Then
            Exit Sub
        End If
        ''check ip allow access
        Dim ip As String = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        If KeywordIPExcludeRow.CheckNotAllowTrackingKeyword(ip) Then
            Exit Sub
        End If
        Dim objKeyword As New KeywordRow
        objKeyword.KeywordName = keyword
        Dim memberId As Integer = IIf(HttpContext.Current.Session("MemberId") = Nothing, 0, Convert.ToInt32(HttpContext.Current.Session("MemberId")))
        Dim keywordSearchId As Integer = KeywordRow.Insert(DB, objKeyword, memberId)
        If keywordSearchId > 0 Then
            HttpContext.Current.Session("KeywordSearchId") = keywordSearchId
        Else
            HttpContext.Current.Session.Remove("KeywordSearchId")
        End If
    End Sub
End Class