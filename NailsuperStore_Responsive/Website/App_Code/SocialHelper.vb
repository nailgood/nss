Imports Microsoft.VisualBasic
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Public Class SocialHelper
    Public Shared Function PostItemReviewToFB(ByVal DB As Database, ByVal sir As StoreItemReviewRow) As Integer
        Try
            If sir Is Nothing Then
                Return 0
            End If
            Dim webRoot As String = Utility.ConfigData.GlobalRefererName
            Dim pagetoken As String = Utility.ConfigData.FaceBookPageAccessToken
            If (String.IsNullOrEmpty(pagetoken)) Then
                Return 0
            End If
            Dim objData As New FacebookPageRow
            Dim si As StoreItemRow = StoreItemRow.GetRow(DB, sir.ItemId)
            objData.Link = webRoot & URLParameters.ProductUrl(si.URLCode, si.ItemId)
            objData.PageTitle = ByDataTemplate(sir).Replace("\n", " ")  'create function convert template to 
            objData.Thumb = webRoot & "/assets/items/featured/" & si.Image.ToString()
            objData.MetaDescription = si.ItemName
            Return PostLinkToFaceBook(objData, pagetoken)
        Catch ex As Exception

        End Try
        Return 0
    End Function
    Public Shared Function PostOrderReviewToFB(ByVal DB As Database, ByVal sor As StoreOrderReviewRow) As Integer
        Try
            If sor Is Nothing Then
                Return 0
            End If
            Dim webRoot As String = Utility.ConfigData.GlobalRefererName
            Dim pagetoken As String = Utility.ConfigData.FaceBookPageAccessToken
            If (String.IsNullOrEmpty(pagetoken)) Then
                Return 0
            End If
            Dim objData As New FacebookPageRow
            'objData.Link = webRoot & "/store/OrderReview.aspx?id=" & sor.OrderId 'SitePage.ProductUrl(si.URLCode)
            objData.PageTitle = OrderTemplate(sor)
            'objData.Thumb = webRoot & "/assets/items/related/110041.gif" ' & si.Image.ToString()
            'objData.MetaDescription = sor.Comment
            Return PostLinkToFaceBook(objData, pagetoken)
        Catch ex As Exception

        End Try
        Return False
    End Function
    Private Shared Function PostLinkToFaceBook(ByVal objData As FacebookPageRow, ByVal pageToken As String) As Integer
        Dim data As New System.Collections.Generic.Dictionary(Of String, String)()
        data.Add("message", objData.PageTitle)
        data.Add("link", objData.Link)
        data.Add("picture", objData.Thumb)
        data.Add("description", objData.MetaDescription)
        '' Dim postWall As Boolean = False
        Dim postPage As Boolean = False
        ' ''post to wall
        'Try
        '    Dim wallFBAPI As New Facebook.FacebookAPI(wallToken)
        '    wallFBAPI.Post("/me/feed", data)
        '    postWall = True
        'Catch ex As Exception

        'End Try
        ''post to page
        Try
            Dim pageFBAPI As New Facebook.FacebookAPI(pageToken)
            pageFBAPI.Post("/" & Utility.ConfigData.FaceBookPageId & "/feed", data)
            postPage = True
        Catch ex As Exception

        End Try
        If (postPage) Then
            Return 3 ''just successfull post to page
        End If
        Return 0 ''default error
    End Function

    Public Shared Function PostBlogToFB(ByVal DB As Database, ByVal news As NewsRow) As Integer
        Try
            If news Is Nothing Then
                Return 0
            End If
            Dim webRoot As String = Utility.ConfigData.GlobalRefererName
            Dim pagetoken As String = Utility.ConfigData.FaceBookPageAccessToken
            If (String.IsNullOrEmpty(pagetoken)) Then
                Return 0
            End If
            Dim objData As New FacebookPageRow
            objData.Link = webRoot & URLParameters.NewsDetailUrl(news.Title, news.NewsId)
            objData.PageTitle = news.ShortDescription
            objData.Thumb = webRoot & Utility.ConfigData.PathThumbBlogImage & news.ThumbImage.ToString()
            objData.MetaDescription = news.MetaDescription
            Return PostLinkToFaceBook(objData, pagetoken)
        Catch ex As Exception

        End Try
        Return 0
    End Function
    Public Shared Function PostVideoReviewToFB(ByVal DB As Database, ByVal rv As ReviewRow) As Integer
        Try
            If rv Is Nothing Then
                Return 0
            End If
            Dim webRoot As String = Utility.ConfigData.GlobalRefererName
            Dim pagetoken As String = Utility.ConfigData.FaceBookPageAccessToken
            If (String.IsNullOrEmpty(pagetoken)) Then
                Return 0
            End If
            Dim objData As New FacebookPageRow
            Dim v As VideoRow = VideoRow.GetRow(DB, rv.ItemReviewId)
            objData.Link = webRoot & URLParameters.VideoDetailUrl(v.Title, v.VideoId)
            objData.PageTitle = v.ShortDescription  'create function convert template to 
            If Not String.IsNullOrEmpty(v.ThumbImage) Then
                objData.Thumb = webRoot & "/assets/video/Thumb/" & v.ThumbImage.ToString()
            Else
                objData.Thumb = webRoot & "/assets/video/Thumb/noimage.gif"
            End If
            objData.MetaDescription = v.MetaDescription
            Return PostLinkToFaceBook(objData, pagetoken)
        Catch ex As Exception

        End Try
        Return 0
    End Function

    Public Shared Function PostNewsReviewToFB(ByVal DB As Database, ByVal rv As ReviewRow) As Integer
        Try
            If rv Is Nothing Then
                Return 0
            End If
            Dim webRoot As String = Utility.ConfigData.GlobalRefererName
            Dim pagetoken As String = Utility.ConfigData.FaceBookPageAccessToken
            If (String.IsNullOrEmpty(pagetoken)) Then
                Return 0
            End If
            Dim objData As New FacebookPageRow
            Dim v As NewsRow = NewsRow.GetRow(DB, rv.ItemReviewId)
            objData.Link = webRoot & URLParameters.NewsDetailUrl(v.Title, v.NewsId)
            objData.PageTitle = v.ShortDescription  'create function convert template to 
            If Not String.IsNullOrEmpty(v.ThumbImage) Then
                objData.Thumb = webRoot & "/assets/NewsImage/MainThumb/" & v.ThumbImage.ToString()
            Else
                objData.Thumb = webRoot & "/assets/NewsImage/MainThumb/noimage.gif"
            End If
            objData.MetaDescription = v.MetaDescription
            Return PostLinkToFaceBook(objData, pagetoken)
        Catch ex As Exception

        End Try
        Return 0
    End Function

    Public Shared Function PostShopDesignReviewToFB(ByVal DB As Database, ByVal rv As ReviewRow) As Integer
        Try
            If rv Is Nothing Then
                Return 0
            End If
            Dim webRoot As String = Utility.ConfigData.GlobalRefererName
            Dim pagetoken As String = Utility.ConfigData.FaceBookPageAccessToken
            If (String.IsNullOrEmpty(pagetoken)) Then
                Return 0
            End If
            Dim objData As New FacebookPageRow
            Dim v As ShopDesignRow = ShopDesignRow.GetRow(DB, rv.ItemReviewId)
            objData.Link = webRoot & URLParameters.ShopDesignUrl(v.Title, v.ShopDesignId)
            objData.PageTitle = v.ShortDescription  'create function convert template to 
            If Not String.IsNullOrEmpty(v.Image) Then
                objData.Thumb = webRoot & "/assets/shopdesign/thumb/" & v.Image.ToString()
            Else
                objData.Thumb = webRoot & "/assets/shopdesign/thumb/noimage.gif"
            End If
            objData.MetaDescription = v.MetaDescription
            Return PostLinkToFaceBook(objData, pagetoken)
        Catch ex As Exception

        End Try
        Return 0
    End Function

    Private Shared Function OrderTemplate(ByVal sor As StoreOrderReviewRow) As String
        Dim sTemplate As String = ""
        sTemplate &= "Recent purchase experience from The Nail SuperStore reviewed by " & sor.Name & vbCrLf & _
                     "Rate: " & sor.NumStars & " Stars" & vbCrLf & _
                     "How was the order experience: " & vbCrLf & _
                     " - " & IIf(sor.ItemArrived = True, "Yes", "No") & ", item arrived on time." & vbCrLf & _
                     " - " & IIf(sor.ItemDescribed = True, "Yes", "No") & ", item as describled." & Environment.NewLine
        Dim strService As String = ""
        Select Case sor.ServicePrompt
            Case 0
                strService = "No"
            Case 1
                strService = "Yes"
            Case 2
                strService = "Did not contact"
        End Select
        sTemplate &= " - " & strService & ", prompt and counteous service." & vbCrLf & _
                    sor.Comment
        Return sTemplate
    End Function
    Private Shared Function ByDataTemplate(ByVal si As StoreItemReviewRow) As String
        Dim sTemplate As String = ""
        sTemplate &= "Review: " & si.ReviewTitle & " by " & si.FirstName & " " & si.LastName & vbCrLf &
                     "Rate: " & si.NumStars & IIf(si.NumStars > 1, " Stars", " Stars")
        Dim arrData, arrData1 As String()
        arrData = si.Comment.Trim.Split(vbCrLf)
        Dim str0, str1 As String
        If arrData.Length > 1 Then
            For i As Integer = 0 To arrData.Length - 1
                arrData1 = arrData(i).Split("=")
                If arrData1.Length > 1 Then
                    str0 = arrData1(0).Replace(" ", "").Trim
                    If Not str0.Contains("Exp") Then
                        str1 = arrData1(1)
                        If str1 = "on" Then
                            sTemplate = ReturnContent(str0, sTemplate)
                        ElseIf str1 <> "on" And str1 <> "" Then
                            sTemplate &= Environment.NewLine & str1
                        End If
                    End If

                End If
            Next
        End If
        Return sTemplate
    End Function
    Private Shared Function ReturnContent(ByVal eStr As String, ByVal strContent As String) As String
        Dim result As String = ""
        Dim confReview As String = ConfigurationManager.AppSettings("ReviewString")
        Dim strLeft As String = Left(eStr.Replace("#", ""), 4)
        confReview = confReview.Replace(eStr, "")
        Dim arr As String() = confReview.Split(";")
        If LCase(strContent).ToString.Contains(LCase(strLeft)) = False Then
            strContent &= Environment.NewLine & Left(eStr.Replace("#", ""), 4) & ": "
            For i As Integer = 0 To arr.Length - 1
                If arr(i).ToString.Contains("#") = False Then
                    If LCase(strContent).ToString.Contains(LCase(strLeft)) Then
                        strContent &= arr(i).ToString
                    End If
                End If
            Next
        Else
            If strLeft <> "Expe" Then
                For i As Integer = 0 To arr.Length - 1
                    If arr(i).ToString.Contains("#") = False Then
                        If LCase(strContent).ToString.Contains(LCase(strLeft)) Then
                            strContent &= ", " & arr(i).ToString
                        End If
                    End If
                Next
            End If
        End If
        If strLeft = "Expe" And LCase(strContent).ToString.Contains("experience") = False Then
            strContent &= Environment.NewLine & "Experience Level: "
            For i As Integer = 0 To arr.Length - 1
                If arr(i).ToString.Contains("#") = False Then
                    If LCase(strContent).ToString.Contains(LCase(strLeft)) Then
                        strContent &= arr(i).ToString
                    End If
                End If
            Next
        Else
            If LCase(strContent).ToString.Contains("experience") Then
                For i As Integer = 0 To arr.Length - 1
                    If arr(i).ToString.Contains("#") = False Then
                        If LCase(strContent).ToString.Contains(LCase(strLeft)) Then
                            strContent &= ", " & arr(i).ToString
                        End If
                    End If
                Next
            End If
        End If
        Return strContent
    End Function
End Class
