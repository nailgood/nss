
Imports DataLayer
Imports Components

Partial Class controls_product_review
    Inherits BaseControl

    Public itemIndex As Integer
    Public item As StoreItemReviewRow
    Public itemfirstClass As String = "first"
    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        BindData(item, itemIndex)
    End Sub
    Public Sub BindData(ByVal si As StoreItemReviewRow, ByVal index As Integer)
        If index > 0 Then
            itemfirstClass = String.Empty
        End If
        'liStar.InnerHtml = "<img alt='" & si.ReviewTitle & "' style='border-style:none' src='" & Utility.ConfigData.CDNMediaPath & "/includes/theme/images/star" & si.NumStars & "0.png'>"
        liStar.InnerHtml = SitePage.BindIconStar(CStr(si.NumStars))
        liName.InnerText = si.ReviewTitle
        spAuthor.InnerText = si.FirstName

        'liStarSmall.InnerHtml = "<img alt='" & si.ReviewTitle & "' style='border-style:none' src='" & Utility.ConfigData.CDNMediaPath & "/includes/theme/images/star" & si.NumStars & "0.png'>"
        liStarSmall.InnerHtml = SitePage.BindIconStar(CStr(si.NumStars))
        liNameSmall.InnerText = si.ReviewTitle
        spAuthorSmall.InnerText = si.FirstName
        Dim rqUrl As String = LCase(Request.RawUrl)
        If rqUrl.Contains("nail-collection") Or rqUrl.Contains("collection.aspx") Then
            grSku.Visible = True
            lsku.InnerText = "Item #" & si.SKU
            lnamecollection.InnerText = si.NameCollection
        End If

        Dim strComment As String = String.Empty
        Dim strSuggestion As String = String.Empty
        Dim hasProsEasyToUse As Boolean = False
        Dim hasProsHighQuality As Boolean = False
        Dim hasProsGoodValue As Boolean = False
        Dim hasProsLongLasting As Boolean = False


        Dim hasConsDifficultToUse As Boolean = False
        Dim hasConsPoorQuality As Boolean = False
        Dim hasConsExpensive As Boolean = False
        Dim hasConsDoesNotWork As Boolean = False

        Dim hasExperienceLevelStudent As Boolean = False
        Dim hasExperienceLevel1 As Boolean = False
        Dim hasExperienceLevel3 As Boolean = False
        Dim hasExperienceLevel5 As Boolean = False

        Dim arrData, arrData1 As String()
        'old data load comment
        arrData = si.Comment.Trim.Split(vbCrLf)
        If (arrData.Length > 1) Then
            For i As Integer = 0 To arrData.Length - 1
                If (arrData(i).ToString().Contains("#txtComments#")) Then
                    arrData1 = arrData(i).Split("=")
                    If arrData1.Length > 1 Then
                        strComment = arrData1(1).ToString().Replace("\n", "<br />")
                    End If
                ElseIf (arrData(i).ToString().Contains("#txtSuggestion#")) Then
                    arrData1 = arrData(i).Split("=")
                    If arrData1.Length > 1 Then
                        strSuggestion = arrData1(1).ToString().Replace("\n", "<br />")
                    End If
                ElseIf (arrData(i).ToString().Contains("#ProsEasyToUse#")) Then
                    hasProsEasyToUse = True
                ElseIf (arrData(i).ToString().Contains("#ProsHighQuality#")) Then
                    hasProsEasyToUse = True
                ElseIf (arrData(i).ToString().Contains("#ProsGoodValue#")) Then
                    hasProsGoodValue = True
                ElseIf (arrData(i).ToString().Contains("#ProsLongLasting#")) Then
                    hasProsLongLasting = True
                ElseIf (arrData(i).ToString().Contains("#ConsDifficultToUse#")) Then
                    hasConsDifficultToUse = True
                ElseIf (arrData(i).ToString().Contains("#ConsPoorQuality#")) Then
                    hasConsPoorQuality = True
                ElseIf (arrData(i).ToString().Contains("#ConsExpensive#")) Then
                    hasConsExpensive = True
                ElseIf (arrData(i).ToString().Contains("#ConsDoesNotWork#")) Then
                    hasConsDoesNotWork = True
                ElseIf (arrData(i).ToString().Contains("#ExperienceLevelStudent#")) Then
                    hasExperienceLevelStudent = True
                ElseIf (arrData(i).ToString().Contains("#ExperienceLevel1#")) Then
                    hasExperienceLevel1 = True
                ElseIf (arrData(i).ToString().Contains("#ExperienceLevel3#")) Then
                    hasExperienceLevel3 = True
                ElseIf (arrData(i).ToString().Contains("#ExperienceLevel5#")) Then
                    hasExperienceLevel5 = True
                End If
            Next
        End If
        ''''''
        'For load new data comment
        arrData = si.Comment.Trim.Split("#txtComments#")
        If arrData.Length = 1 Then
            strComment = si.Comment
        End If
        ''''''
        Dim ulPros As String = GetGroupPros(hasProsEasyToUse, hasProsHighQuality, hasProsGoodValue, hasProsLongLasting)
        Dim ulCons As String = GetGroupCons(hasConsDifficultToUse, hasConsPoorQuality, hasConsExpensive, hasConsDoesNotWork)
        Dim ulExperience As String = GetGroupExperience(hasExperienceLevelStudent, hasExperienceLevel1, hasExperienceLevel3, hasExperienceLevel5)
        If Not String.IsNullOrEmpty(ulPros) Or Not String.IsNullOrEmpty(ulCons) Or Not String.IsNullOrEmpty(ulExperience) Then
            ltrGroup.Text = "<ul class='groupChk hidden-xs'>"
            If Not String.IsNullOrEmpty(ulPros) Then
                ltrGroup.Text &= "<li class='pros'><span>Pros:</span>" & ulPros & "</li>"
            End If
            If Not String.IsNullOrEmpty(ulExperience) Then
                ltrGroup.Text &= "<li class='experience'><span>Experience Level:</span>" & ulExperience & "</li>"
            End If
            If Not String.IsNullOrEmpty(ulCons) Then
                ltrGroup.Text &= "<li class='cons'><span>Cons:</span>" & ulCons & "</li>"
            End If
            ltrGroup.Text &= "  </ul>"
        End If
        divComment.Visible = False
        If Not (String.IsNullOrEmpty(strComment)) Then
            divComment.Visible = True
            ltrComment.Text = strComment
        End If
        divSuggestion.Visible = False
        If Not (String.IsNullOrEmpty(strSuggestion)) Then
            divSuggestion.Visible = True
            ltrSuggestion.Text = strSuggestion
        End If
        divBottomLine.Visible = False
        If (si.IsRecommendFriend) Then
            divBottomLine.Visible = True
        End If
        ''get Admin Reply
        divAdminReply.Visible = False

        Dim objReply As ReplyReviewRow = ReplyReviewRow.GetRowByReviewId(si.ReviewId, Utility.Common.TypeReview.ProductReview)
        If (objReply.ReplyReviewId > 0) Then
            divAdminReply.Visible = True
            ltrAdminReply.Text = Replace(objReply.Content.Trim(), vbCrLf, "<br/>")
        End If
        If Request.Path.Contains("/store/review/product-list.aspx") Then
            Dim link As String = URLParameters.ProductUrl(si.URLCode, si.ItemId)
            divImage.Visible = True
            If String.IsNullOrEmpty(si.Image) Then
                si.Image = "na.jpg"
            End If
            ltrItemImage.Text = "<a href='" & link & "'><img src='" & Utility.ConfigData.CDNMediaPath & "/assets/items/featured/" & si.Image & "' alt='" & si.ItemName & "'/></a>"
            ltrLinkItem.Text = "<a href='" & link & "'>" & si.ItemName & "</a>"
        Else
            divReviewData.Attributes.Add("style", "margin-left:0px")
        End If
    End Sub

    Private Function GetGroupPros(ByVal hasProsEasyToUse As Boolean, ByVal hasProsHighQuality As Boolean, ByVal hasProsGoodValue As Boolean, ByVal hasProsLongLasting As Boolean) As String
        Dim result As String = String.Empty
        If Not hasProsEasyToUse AndAlso Not hasProsGoodValue AndAlso Not hasProsHighQuality AndAlso Not hasProsLongLasting Then
            Return String.Empty
        End If
        result = "<ul class='sub'>"
        If hasProsEasyToUse Then
            result &= "<li>Easy To Use</li>"
        End If
        If hasProsHighQuality Then
            result &= "<li>High Quality</li>"
        End If
        If hasProsGoodValue Then
            result &= "<li>Good Value</li>"
        End If
        If hasProsLongLasting Then
            result &= "<li>Long Lasting</li>"
        End If
        result &= "</ul>"
        Return result
    End Function
    Private Function GetGroupCons(ByVal hasConsDifficultToUse As Boolean, ByVal hasConsPoorQuality As Boolean, ByVal hasConsExpensive As Boolean, ByVal hasConsDoesNotWork As Boolean) As String
        Dim result As String = String.Empty
        If Not hasConsDifficultToUse AndAlso Not hasConsPoorQuality AndAlso Not hasConsExpensive AndAlso Not hasConsDoesNotWork Then
            Return String.Empty
        End If
        result = "<ul class='sub'>"
        If hasConsDifficultToUse Then
            result &= "<li>Difficult To Use</li>"
        End If
        If hasConsPoorQuality Then
            result &= "<li>Poor Quality</li>"
        End If
        If hasConsExpensive Then
            result &= "<li>Expensive</li>"
        End If
        If hasConsDoesNotWork Then
            result &= "<li>Does Not Work</li>"
        End If
        result &= "</ul>"
        Return result
    End Function
    Private Function GetGroupExperience(ByVal hasExperienceLevelStudent As Boolean, ByVal hasExperienceLevel1 As Boolean, ByVal hasExperienceLevel3 As Boolean, ByVal hasExperienceLevel5 As Boolean) As String
        Dim result As String = String.Empty
        If Not hasExperienceLevelStudent AndAlso Not hasExperienceLevel1 AndAlso Not hasExperienceLevel3 AndAlso Not hasExperienceLevel5 Then
            Return String.Empty
        End If
        result = "<ul class='sub'>"
        If hasExperienceLevelStudent Then
            result &= "<li>Student</li>"
        End If
        If hasExperienceLevel1 Then
            result &= "<li>1+ Years</li>"
        End If
        If hasExperienceLevel3 Then
            result &= "<li>3+ Years</li>"
        End If
        If hasExperienceLevel5 Then
            result &= "<li>5+ Years</li>"
        End If
        result &= "</ul>"
        Return result
    End Function
End Class
