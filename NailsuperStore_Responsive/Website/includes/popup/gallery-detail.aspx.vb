Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System
Partial Class includes_popup_gallery_detail
    Inherits SitePage

    Private sid, fid As String
    Protected imgDetail, artName, salonName, instruction, title, imgRelated As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            LoadData()
        End If
    End Sub

    Private Sub LoadData()
        Try

            If Not String.IsNullOrEmpty(Request("sid")) Then
                sid = GetQueryString("sid")
            End If
            If Not String.IsNullOrEmpty(Request("fid")) Then
                fid = GetQueryString("fid")
            End If
            Dim imgSelect As String = ""
            Dim imgPath As String = ""
            If Not sid = String.Empty Then
                Dim mrc As MemberSubmissionCollection = MemberSubmissionRow.GetGalleryById(sid)
                Dim mr As New MemberSubmissionRow
                Dim imgMain As String
                For i As Integer = 0 To mrc.Count - 1
                    imgMain = ""
                    imgPath = Utility.ConfigData.PathUploadArtTrend
                    mr = mrc(i)
                    With mr
                        imgMain = imgPath & "admin/" & .AdminUploadFile
                        If Core.FileExists(MapPath(imgMain)) Then
                            imgPath &= "list/" & .AdminUploadFile
                            If Core.FileExists(Server.MapPath(imgPath)) = False Then
                                imgPath = imgMain
                            End If
                            If .FileId = fid Then
                                imgDetail = imgMain
                                artName = .ArtName
                                title = IIf(artName = "", .Name, artName & " - " & .Name)
                                salonName = IIf(.SalonName = "", .Country, .SalonName & " - " & .Country)
                                instruction = .Instruction
                                imgSelect = "<li id=""img-" & .FileId & """ onclick=""ChangeCss('img-" & .FileId & "');"" style=""padding-top:0px"" class=""active""><img src=""" & imgPath & """ class=""imgrelated"" alt=""" & .ArtName & """ onclick = ""updateAlternateimg('" & imgPath & "','" & artName & "');"" /></li>"
                            Else
                                imgRelated &= "<li class=""normal"" id=""img-" & .FileId & """ onclick=""ChangeCss('img-" & .FileId & "');""><img src=""" & imgPath & """ class=""imgrelated"" alt=""" & .ArtName & """ onclick = ""updateAlternateimg('" & imgPath & "','" & artName & "');"" /></li>"
                            End If
                        End If
                    End With
                Next
            End If
            relatedImage.InnerHtml = "<ul id=""lisImg"">" & imgSelect & imgRelated & "</ul>"
        Catch ex As Exception
            Email.SendError("ToError500", "[GALLERY DETAIL]", "SubmissionId = " & sid & "<br>Error: " & ex.ToString())
        End Try
    End Sub
End Class
