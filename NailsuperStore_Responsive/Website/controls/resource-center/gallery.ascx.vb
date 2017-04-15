Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Partial Class controls_resource_center_gallery
    Inherits ModuleControl
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property
    Private m_MemberSubmission As MemberSubmissionRow = Nothing
    Public Property MemberSubmission() As MemberSubmissionRow
        Set(ByVal value As MemberSubmissionRow)
            m_MemberSubmission = value
        End Set
        Get
            Return m_MemberSubmission
        End Get
    End Property
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        BindData()
    End Sub

    Public Sub BindData()
        Try
            If MemberSubmission Is Nothing AndAlso Not Session("galleryRender") Is Nothing Then
                MemberSubmission = Session("galleryRender")
            End If
            Dim img, fileId, ArtName, lbName, lbSalonName As String
            Dim arrAdminFile As String()
            Dim sId As Integer = 0
            img = Utility.ConfigData.PathUploadArtTrend

            If MemberSubmission IsNot Nothing Then
                ltrDvGallery.Text = IIf((MemberSubmission.itemIndex Mod 4) = 0, "<div class='Gallery glast' id='galleryItem_" & MemberSubmission.itemIndex & "'>", "<div class='Gallery' id='galleryItem_" & MemberSubmission.itemIndex & "'>")
                ltrDvGallery.Text &= "<article>"
                With MemberSubmission
                    arrAdminFile = .AdminUploadFile.ToString().Split("|")
                    ArtName = .ArtName
                    lbName = .Name
                    lbSalonName = .SalonName
                    Try
                        sId = .SubmissionId
                    Catch
                        sId = 0
                    End Try
                End With
                Try
                    img &= "list/" & arrAdminFile(0)
                    If Core.FileExists(Server.MapPath(img)) = False Then
                        img = Utility.ConfigData.PathUploadArtTrend & "admin/" & arrAdminFile(0)
                    End If
                    fileId = arrAdminFile(1)
                Catch

                End Try
                ltrDvGallery.Text &= "<div id='imgGallery_" & MemberSubmission.itemIndex & "' class='imgGallery'><img  class=""thumbnail-img"" src='" & Utility.ConfigData.CDNMediaPath & img & "' alt='" & ArtName & "' onclick=""GalleryDetail('" & sId & "','" & fileId & "');"" /></div>"
                ltrDvGallery.Text &= "<div class='thumbnail-a'><a onclick=""GalleryDetail('" & sId & "','" & fileId & "');"" class=""link-bold"" alt='" & ArtName & "'>" & ArtName & "</a></div>"
                ltrDvGallery.Text &= "<div class='title'>" & lbName & "</div>"
                ltrDvGallery.Text &= IIf(lbSalonName <> String.Empty, "<div class='salon'>" & lbSalonName & "</div></article></div>", "</article></div>")
            End If
            
        Catch ex As Exception
            Email.SendError("ToError500", "[GALLERY]", "SubmissionId = " & MemberSubmission.SubmissionId & "<br>Error: " & ex.ToString())
        End Try
    End Sub
End Class
