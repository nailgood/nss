Imports Components
Imports DataLayer
Imports System.IO
Partial Class includes_popup_review
    Inherits SitePage

    Protected lbTitle As String = String.Empty
    Public ItemReviewId As Integer = 0
    Public ParentReviewId As Integer = 0
    Public Type As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Request.QueryString("ItemReviewId") <> Nothing Or Request.QueryString("ParentId") <> Nothing) AndAlso Request.QueryString("Type") <> Nothing Then
            ItemReviewId = CInt(Request.QueryString("ItemReviewId"))
            ParentReviewId = CInt(Request.QueryString("ParentId"))
            Type = CInt(Request.QueryString("Type"))
        End If
        lbTitle = IIf(ParentReviewId > 0, "Reply", "Write")
        If Session("MemberId") Is Nothing Then
            ltrResult.Text = "Please login to review."
            pnResult.Visible = True
            pnField.Visible = False
        Else
            pnResult.Visible = False
        End If
    End Sub

    Protected Sub btnComment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnComment.Click
        If Session("MemberId") Is Nothing Then
            'Response.Redirect("/Members/login.aspx")
            ltrResult.Text = "Please login to review."
            pnResult.Visible = True
            pnField.Visible = False
        Else
            If ItemReviewId > 0 AndAlso Not String.IsNullOrEmpty(txtComment.Text.Trim()) Then
                Dim review As New ReviewRow
                review.Type = Type
                review.ItemReviewId = ItemReviewId
                review.ParentReviewId = IIf(ParentReviewId > 0, ParentReviewId, 0)
                review.MemberId = Session("MemberId")
                review.IsActive = False
                review.Comment = txtComment.Text
                review.IsFacebook = False
                review.ReviewId = ReviewRow.Insert(DB, review)
                If review.ReviewId > 0 Then
                    SendEmailReview(review)
                    ltrResult.Text = "Your review sent sucessfully."
                    pnResult.Visible = True
                    pnField.Visible = False
                End If
            End If
        End If
    End Sub

    Private Sub SendEmailReview(ByVal obj As ReviewRow)
        Dim FullPath As String = Server.MapPath("~/includes/MailTemplate/EmailVideoReviewTemplate.htm")
        Dim result As String = String.Empty
        Dim strContents As String
        Dim objReader As StreamReader
        Try
            Dim webRoot As String = Utility.ConfigData.GlobalRefererName
            objReader = New StreamReader(FullPath)
            strContents = objReader.ReadToEnd()
            objReader.Close()
            strContents = strContents.Replace("##ReviewType##", [Enum].GetName(GetType(Utility.Common.ReviewType), obj.Type) & ":")
            If obj.Type = Utility.Common.ReviewType.Video Then
                Dim video As VideoRow = VideoRow.GetRow(DB, obj.ItemReviewId)
                If Not video Is Nothing Then
                    strContents = strContents.Replace("##ReviewTypeTitle##", video.Title)
                    If Not String.IsNullOrEmpty(video.ShortDescription.Trim()) Then
                        strContents = strContents.Replace("##Description##", "Description:")
                        strContents = strContents.Replace("##DescriptionContent##", video.ShortDescription.Replace(vbCrLf, "<br/>"))
                        strContents = strContents.Replace("##Link##", webRoot & URLParameters.VideoDetailUrl(video.Title, video.VideoId))
                    Else
                        strContents = strContents.Replace("##Description##", String.Empty)
                        strContents = strContents.Replace("##DescriptionContent##", String.Empty)
                        strContents = strContents.Replace("##Link##", String.Empty)
                    End If
                Else
                    strContents = strContents.Replace("##ReviewTypeTitle##", String.Empty)
                    strContents = strContents.Replace("##Description##", String.Empty)
                    strContents = strContents.Replace("##DescriptionContent##", String.Empty)
                    strContents = strContents.Replace("##Link##", String.Empty)
                End If
            ElseIf obj.Type = Utility.Common.ReviewType.News Then
                Dim news As NewsRow = NewsRow.GetRow(DB, obj.ItemReviewId)
                If Not news Is Nothing Then
                    strContents = strContents.Replace("##ReviewTypeTitle##", news.Title)
                    If Not String.IsNullOrEmpty(news.ShortDescription.Trim()) Then
                        strContents = strContents.Replace("##Description##", "Description:")
                        strContents = strContents.Replace("##DescriptionContent##", news.ShortDescription.Replace(vbCrLf, "<br/>"))
                        strContents = strContents.Replace("##Link##", webRoot & URLParameters.NewsDetailUrl(news.Title, news.NewsId))
                    Else
                        strContents = strContents.Replace("##Description##", String.Empty)
                        strContents = strContents.Replace("##DescriptionContent##", String.Empty)
                        strContents = strContents.Replace("##Link##", String.Empty)
                    End If
                Else
                    strContents = strContents.Replace("##ReviewTypeTitle##", String.Empty)
                    strContents = strContents.Replace("##Description##", String.Empty)
                    strContents = strContents.Replace("##DescriptionContent##", String.Empty)
                    strContents = strContents.Replace("##Link##", String.Empty)
                End If
            ElseIf obj.Type = Utility.Common.ReviewType.ShopDesign Then
                Dim shopdesign As ShopDesignRow = ShopDesignRow.GetRow(DB, obj.ItemReviewId)
                If Not shopdesign Is Nothing Then
                    strContents = strContents.Replace("##ReviewTypeTitle##", shopdesign.Title)
                    If Not String.IsNullOrEmpty(shopdesign.ShortDescription.Trim()) Then
                        strContents = strContents.Replace("##Description##", "Description:")
                        strContents = strContents.Replace("##DescriptionContent##", shopdesign.ShortDescription.Replace(vbCrLf, "<br/>"))
                        strContents = strContents.Replace("##Link##", webRoot & URLParameters.ShopDesignUrl(shopdesign.Title, shopdesign.ShopDesignId))
                    Else
                        strContents = strContents.Replace("##Description##", String.Empty)
                        strContents = strContents.Replace("##DescriptionContent##", String.Empty)
                        strContents = strContents.Replace("##Link##", String.Empty)
                    End If
                Else
                    strContents = strContents.Replace("##ReviewTypeTitle##", String.Empty)
                    strContents = strContents.Replace("##Description##", String.Empty)
                    strContents = strContents.Replace("##DescriptionContent##", String.Empty)
                    strContents = strContents.Replace("##Link##", String.Empty)
                End If
            End If
            strContents = strContents.Replace("##date##", DateTime.Now.ToString("MM/dd/yyyy"))
            Dim member As MemberRow = MemberRow.GetRow(obj.MemberId)
            Dim cust As CustomerRow = member.Customer
            strContents = strContents.Replace("##FullName##", cust.Name & " " & cust.Name2)
            strContents = strContents.Replace("##MemberId##", obj.MemberId)
            strContents = strContents.Replace("##CustomerNo##", cust.CustomerNo)
            strContents = strContents.Replace("##CustomerPhone##", cust.Phone)
            strContents = strContents.Replace("##Comment##", obj.Comment.Replace(vbCrLf, "<br/>"))
            strContents = strContents.Replace("##webRoot##", webRoot)
            strContents = strContents.Replace("##pathReview##", IIf(obj.Type = Utility.Common.ReviewType.ShopDesign, "shop-design", [Enum].GetName(GetType(Utility.Common.ReviewType), obj.Type)))
            strContents = strContents.Replace("##pathReviewInAdmin##", IIf(obj.Type = Utility.Common.ReviewType.News, "newsevent", [Enum].GetName(GetType(Utility.Common.ReviewType), obj.Type)))
            strContents = strContents.Replace("##ReviewId##", obj.ReviewId)
            ''Dim body As String = strContents
            Dim subject As String = "[" & [Enum].GetName(GetType(Utility.Common.ReviewType), obj.Type) & " Review] " & "Customer #" & cust.CustomerNo & " sent comment"

            ''send report
            Dim lstEmail As String = SysParam.GetValue(DB, "ToReportReview")
            Dim arr() As String = lstEmail.Split(",")
            For Each ToEmail As String In arr
                If (Not String.IsNullOrEmpty(ToEmail)) Then
                    Dim body As String = strContents.Replace("##Email##", ToEmail)
                    Dim ToName As String = AdminRow.GetRowByEmail(ToEmail)
                    Email.SendHTMLMail(FromEmailType.NoReply, ToEmail, ToName, subject, body)
                End If
            Next


            ''Email.SendReport("ToReportReview", subject, body)
        Catch Ex As Exception
        End Try
    End Sub



End Class
