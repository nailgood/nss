Imports Components
Imports DataLayer
Imports System.Collections.Generic
Imports Humanizer
Partial Class controls_resource_center_news_top3_news_list
    Inherits BaseControl
    Protected PathThumbBlogImage As String = Utility.ConfigData.PathThumbBlogImage
    Protected PathThumbNewsImg As String = Utility.ConfigData.PathThumbNewsImage
    Public Shared sclass As String = ""
    Public index As Integer = 0
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        ltrmCategory.Text = "<div class='category'><a href='/news-topic'>News & Events</a></div>"
        ltrViewMore.Text = "<div class='viewmore'><a href='/news-topic'>View More</a></div>"
        Dim lstTop3News As List(Of NewsRow) = NewsRow.ListTop3News()
        rptNews.DataSource = lstTop3News
        rptNews.DataBind()
    End Sub

    Protected Sub rptNews_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptNews.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            index = index + 1
            If (index Mod 3 = 0) Then
                sclass = " vlast"
            Else
                sclass = ""
            End If
            Dim item As NewsRow = e.Item.DataItem
            Dim link As String = IIf(item.CategoryId = 12, URLParameters.BlogDetailUrl(item.Title, item.NewsId), URLParameters.NewsDetailUrl(item.Title, item.NewsId))
            Dim thumbImg As String = IIf(item.CategoryId = 12, PathThumbBlogImage, PathThumbNewsImg)
            Dim ltrdvNews As Literal = e.Item.FindControl("ltrdvNews")
            ltrdvNews.Text &= "<div class='dvVideoItem" & sclass & "'><article>"
            ltrdvNews.Text &= IIf(System.IO.File.Exists(Server.MapPath("~/" + thumbImg & item.ThumbImage)), "<div class='image'><a href='" & link & "'><img src='" & Utility.ConfigData.CDNMediaPath & thumbImg & item.ThumbImage & "' alt='" & item.Title & "' /></a></div>", "")
            ltrdvNews.Text &= "<div class='title'><a href='" & link & "'>" & item.Title & "</a></div>"
            ltrdvNews.Text &= "<div class='dvShortDesc'>" & TruncateExtensions.Truncate(item.ShortDescription, 30, Truncator.FixedNumberOfWords) & "</div></article></div>"
        End If
    End Sub
End Class
