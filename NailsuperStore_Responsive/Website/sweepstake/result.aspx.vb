Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports Utility.Common
Partial Class Sweepstake_Result
    Inherits SitePage
    Protected DrawingDate, Title, CountDownEndDate As String
    Protected Id As Integer = 0
    Protected isShowVideo As Boolean = False
    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Id = GetQueryString("Id")

        If Not Page.IsPostBack Then

            If Request.RawUrl.Contains("result.aspx") Then
                Utility.Common.Redirect301("/sweepstake/" & Id)
                Exit Sub
            End If
            If IsNumeric(Id) Then
                Dim row As SweepstakeRow = SweepstakeRow.GetRow(DB, Id)
                With row
                    If .SweepstakeId > 0 Then
                        Dim objMetaTag As New MetaTag
                        If String.IsNullOrEmpty(.PageTitle) Then
                            objMetaTag.PageTitle = .Name
                        Else
                            objMetaTag.PageTitle = .PageTitle
                        End If
                        objMetaTag.MetaDescription = .MetaDescription
                        objMetaTag.MetaKeywords = .MetaKeyword
                        SetPageMetaSocialNetwork(Page, objMetaTag)
                        DrawingDate = .DrawingDate.ToString("yyyy-MM-dd") & "T" & .DrawingDate.ToString("HH:mm:ss")
                        CountDownEndDate = DateTime.Now.ToString("yyyy-MM-dd") & "T" & DateTime.Now.ToString("HH:mm:ss")
                        Title = .Name
                        If CountDownEndDate >= DrawingDate Then
                            Countdown.Visible = False
                            isShowVideo = True
                            lbName.InnerHtml = .Result
                            If Not String.IsNullOrEmpty(.YouTubeId) Then
                                ltVideo.Text = Utility.Common.GetVideoResource(.YouTubeId, 1140, 641, 1, 0, 0)
                            End If
                        Else
                            isShowVideo = False
                        End If
                    End If
                End With
            End If


        End If
    End Sub
End Class
