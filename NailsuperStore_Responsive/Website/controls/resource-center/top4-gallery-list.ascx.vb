Imports Components
Imports DataLayer
Imports System.Collections.Generic
Partial Class controls_resource_center_top3_gallery_list
    Inherits BaseControl
    Protected PathThumbBlogImage As String = Utility.ConfigData.PathThumbBlogImage
    Protected PathThumbNewsImg As String = Utility.ConfigData.PathThumbNewsImage
    Public Shared sclass As String = ""
    Public index As Integer = 0
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        ltrmCategory.Text = "<div class='category'><a href='/gallery/nail-art-trend'>Photo Gallery</a></div>"
        ltrViewMore.Text = "<div class='viewmore'><a href='/gallery/nail-art-trend'>View More</a></div>"
        Dim lstTop4Video As List(Of MemberSubmissionRow) = MemberSubmissionRow.ListTop4Gallery()
        rptGallery.DataSource = lstTop4Video
        rptGallery.DataBind()
    End Sub

    Protected Sub rptNews_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptGallery.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            index = index + 1
            If (index Mod 4 = 0) Then
                sclass = " glast"
            Else
                sclass = ""
            End If
            Dim item As MemberSubmissionRow = e.Item.DataItem
            Dim fileId As String = item.AdminUploadFile.Split("|")(1)
            Dim thumbImg As String = Utility.ConfigData.PathUploadArtTrend & "list/" & item.AdminUploadFile.Split("|")(0)
            Dim ltrdvGallery As Literal = e.Item.FindControl("ltrdvGallery")
            ltrdvGallery.Text &= "<div class='Gallery" & sclass & "'><article>"
            ltrdvGallery.Text &= "<div class='imgGallery'><img class='thumbnail-img'  onclick=""GalleryDetail('" & item.SubmissionId & "','" & fileId & "');"" src='" & Utility.ConfigData.CDNMediaPath & thumbImg & "' alt='" & item.ArtName & "' /></div>"
            ltrdvGallery.Text &= "<div class='thumbnail-a'><a onclick=""GalleryDetail('" & item.SubmissionId & "','" & fileId & "');"" class=""link-bold"" alt='" & item.ArtName & "'>" & item.ArtName & "</a></div>"
            ltrdvGallery.Text &= "<div class='title'>" & item.Name & "</div>"
            ltrdvGallery.Text &= IIf(item.SalonName <> String.Empty, "<div class='salon'>" & item.SalonName & "</div></article></div>", "</article></div>")
        End If
    End Sub
End Class
