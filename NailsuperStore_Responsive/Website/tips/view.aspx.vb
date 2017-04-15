Imports Components
Imports DataLayer

Partial Class tips_view
	Inherits SitePage

    Protected Tip As TipRow

	Private Sub page_load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If String.IsNullOrEmpty(Request("TipId")) Then
                Utility.Common.Redirect301("/tips/default.aspx")
            End If
            Tip = TipRow.GetRow(DB, Request("TipId"))
            Dim objMetaTag As New MetaTag

            objMetaTag.MetaDescription = Tip.MetaDescription
            objMetaTag.MetaKeywords = Tip.MetaKeywords
            MetaTitle = Tip.MetaTitle
            objMetaTag.PageTitle = Tip.PageTitle

            'Dim objCate As TipCategoryRow = TipCategoryRow.GetRow(DB, Tip.TipCategoryId)
            If Not Tip Is Nothing Then
                Utility.Common.CheckValidURLCode(URLParameters.ReplaceUrl(HttpUtility.UrlEncode(Tip.Title.ToLower())))
                Session("TipCateId") = Tip.TipCategoryId
            End If
            If Not IsPostBack Then
                If Session("Language") = Nothing OrElse Tip.VietTitle = Nothing OrElse Tip.VietText = Nothing Then
                    lblTitle.Text = "<h1>" & Tip.Title & "</h1>"
                    If LCase(Tip.FullText).Contains("youtube.com") Then
                        Tip.FullText = Tip.FullText.Replace("http://", "https://")
                    End If
                    If LCase(Tip.FullText).Contains("youtube.com") And Not LCase(Tip.FullText).Contains("<object") And Not LCase(Tip.FullText).Contains("type=""application/x-shockwave-flash""") Then
                        Dim source As String = Utility.Common.RemoveHTMLTag(Tip.FullText)
                        divTip.InnerHtml = Utility.Common.GetVideoResource(source, 754, 434, 1, 0)
                    Else
                        divTip.InnerHtml = Tip.FullText
                    End If
                Else
                    lblTitle.Text = "<h1>" & Tip.VietTitle & "</h1>"
                    If LCase(Tip.VietText).Contains("youtube.com") Then
                        Tip.VietText = Tip.VietText.Replace("http://", "https://")
                    End If
                    If LCase(Tip.VietText).Contains("youtube.com") And Not LCase(Tip.VietText).Contains("<object") And Not LCase(Tip.VietText).Contains("type=""application/x-shockwave-flash""") Then
                        Dim source As String = Utility.Common.RemoveHTMLTag(Tip.VietText)
                        divTip.InnerHtml = Utility.Common.GetVideoResource(source, 754, 434, 1, 0)
                    Else
                        divTip.InnerHtml = Tip.VietText
                    End If
                End If
            End If
            SetPageMetaSocialNetwork(Page, objMetaTag)

        Catch ex As Exception
        End Try
	End Sub

End Class
