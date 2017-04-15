Imports Components
Imports DataLayer
Imports System.Text
Imports System.IO
Imports Utility
Imports System.Data.SqlClient
Partial Class store_review
    Inherits SitePage
    '-------------------------------------------------------------------
    ' VARIABLES
    '-------------------------------------------------------------------
    Protected ItemRewiew As StoreItemReviewRow
    Protected ItemId As Integer
    Protected scart As StoreCartItemRow
    Protected CartItemId As Integer
    Protected o As StoreOrderRow
    Protected MemberId As Integer
    Private ReviewId As Integer
    Private Comment As String = String.Empty
    Private CommentTextError As String = String.Empty
    Private isAddPoint As Boolean
    Protected get20pts As String = " (" & Resources.Msg.review_First & ")"
    Public Property image() As String
        Get
            Return ViewState("image")
        End Get
        Set(ByVal value As String)
            ViewState("image") = value
        End Set
    End Property
    Public Property title() As String
        Get
            Return ViewState("title")
        End Get
        Set(ByVal value As String)
            ViewState("title") = value
        End Set
    End Property
    Public Property altImg() As String
        Get
            Return ViewState("altImg")
        End Get
        Set(ByVal value As String)
            ViewState("altImg") = value
        End Set
    End Property
    Public Property desc() As String
        Get
            Return ViewState("desc")
        End Get
        Set(ByVal value As String)
            ViewState("desc") = value
        End Set
    End Property
    Private isFirstReview As Boolean = False
    '-------------------------------------------------------------------
    ' METHODS
    '-------------------------------------------------------------------
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim cartQueryId As String = String.Empty

            If Not GetQueryString("id") Is Nothing Then
                cartQueryId = GetQueryString("id")
            End If
            If GetQueryString("ReviewId") <> Nothing Then
                ReviewId = GetQueryString("ReviewId")
            End If
            ltScript.Text = ""
            If Not IsPostBack Then
                hidCountMessage.Value = Resources.Alert.CountWordReview
                hidMinimumWordComment.Value = ConfigData.MinimumWordReview
                LoadReview()
            End If
            isAddPoint = False
            If Not String.IsNullOrEmpty(cartQueryId) Then 'from email review
                If IsNumeric(cartQueryId) Then
                    CartItemId = cartQueryId
                Else
                    CartItemId = CryptData.Crypt.DecryptTripleDes(cartQueryId)
                End If
                StoreItemReviewRow.GetInfoCartItemId(CartItemId, isFirstReview, MemberId, ItemId, image, desc, title)
                isAddPoint = True
            Else
                If GetQueryString("ReviewId") Is Nothing Then 'from admin load
                    If GetQueryString("ItemId") <> Nothing And Session("MemberId") <> Nothing Then
                        ItemId = GetQueryString("ItemId")
                        MemberId = Session("MemberId")
                    Else
                        Response.Redirect("/members/login.aspx", False)
                    End If
                End If
            End If

            If GetQueryString("st") = 1 Then
                LoadItem()
                divReviewForm.Visible = False
                litMsg.Text = Resources.Msg.review_thanks
                Exit Sub
            End If


            If Not Page.IsPostBack Then
                If GetQueryString("ReviewId") Is Nothing Then
                    Dim itemReView As StoreItemReviewRow = StoreItemReviewRow.GetRow(DB, ItemId, MemberId)
                    If Not String.IsNullOrEmpty(itemReView.ReviewTitle) Then
                        LoadItem()
                        divReviewForm.Visible = False
                        If (itemReView.IsActive) Then
                            litMsg.Text = Resources.Msg.review_thanks
                        Else
                            litMsg.Text = Resources.Msg.review_approve
                        End If
                        Exit Sub
                    End If
                End If
                LoadItem()
                'LoadTemplate(String.Empty)
            End If
        Catch ex As Exception
            'Response.Write(ex.ToString())
        End Try

    End Sub

    Private Sub LoadReview()
        Dim ReviewId As Integer = CInt(GetQueryString("ReviewId"))
        Dim sr As StoreItemReviewRow = StoreItemReviewRow.GetRow(DB, ReviewId)
        If sr.ReviewId > 0 Then
            divSubmit.Visible = False
            txtFirstName.Text = sr.FirstName
            txtLastName.Text = sr.LastName
            txtTitle.Text = sr.ReviewTitle
            ItemId = sr.ItemId
            MemberId = sr.MemberId
            hidStar.Value = CInt(sr.NumStars.ToString & "0")
            If sr.Comment.Length > 0 Then
                Comment = sr.Comment.Trim()
                txtComments.InnerText = Comment
            End If
            divSave.Visible = True
        Else
            divSave.Visible = False
            divSubmit.Visible = True
        End If
    End Sub

    Private Sub LoadItem()
        Try
            If String.IsNullOrEmpty(title) Then
                StoreItemReviewRow.GetInfoCartItemId(CartItemId, isFirstReview, MemberId, ItemId, image, desc, title)
            End If
            If Not isFirstReview Or CartItemId = 0 Then
                get20pts = Nothing
            End If
            If Not String.IsNullOrEmpty(image) Then
                If Not image.Contains("/assets") Then
                    image = Utility.ConfigData.CDNMediaPath & "/assets/items/featured/" & image
                End If
                'divImg1.Style.Add("background", "url(/assets/items/featured/" & si.Image & ") no-repeat center center;")
            Else
                image = Utility.ConfigData.CDNMediaPath & "/assets/nobg.gif"
            End If
            desc = BBCodeHelper.ConvertBBCodeToHTML(desc)
        Catch ex As Exception

        End Try
    End Sub

#Region "TemplateReview"
    Private Function GetEmailreview(ByVal data As StoreItemReviewRow, ByVal comment As String) As String
        Dim FullPath As String = Server.MapPath("~/includes/MailTemplate/EmailReviewTemplate.htm")
        Dim result As String = String.Empty
        Dim strContents As String
        Dim objReader As StreamReader
        Try

            objReader = New StreamReader(FullPath)
            strContents = objReader.ReadToEnd()
            objReader.Close()

            Dim si As StoreItemRow = StoreItemRow.GetRow(DB, data.ItemId)
            If si Is Nothing Or String.IsNullOrEmpty(si.SKU) Then
                Return String.Empty
            End If
            Dim member As MemberRow = MemberRow.GetRow(MemberId)
            Dim customerNo As String = member.Customer.CustomerNo
            Dim addressType As MemberAddressRow = MemberAddressRow.GetAddressByType(DB, member.MemberId, Utility.Common.MemberAddressType.Billing.ToString())

            Dim webRoot As String = Utility.ConfigData.GlobalRefererName
            If si.Image <> Nothing Then
                strContents = strContents.Replace("##img##", "<img src='" & webRoot & "/assets/items/featured/" & si.Image & "'/>")
            End If
            strContents = strContents.Replace("##URLCode##", si.URLCode & "/" & si.ItemId)
            strContents = strContents.Replace("##SKU##", si.SKU)
            strContents = strContents.Replace("##itemName##", si.ItemName)
            strContents = strContents.Replace("##itemDesc##", BBCodeHelper.ConvertBBCodeToHTML(si.ShortDesc))
            strContents = strContents.Replace("##title##", data.ReviewTitle)
            strContents = strContents.Replace("##date##", data.DateAdded.ToShortDateString())
            strContents = strContents.Replace("##FirstName##", data.FirstName)
            strContents = strContents.Replace("##LastName##", data.LastName)
            strContents = strContents.Replace("##MemberId##", data.MemberId)
            strContents = strContents.Replace("##rating##", "<img alt='' style='border-style:none' src='" & webRoot & "/includes/theme/images/star" & data.NumStars & "0.png'/>")
            Dim LevelRating As String = Utility.Common.ConvertStartNumberToTextLevelOrderReview(data.NumStars)
            strContents = strContents.Replace("##levelrating##", LevelRating)
            strContents = strContents.Replace("##ratingNumber##", data.NumStars)

            If Not String.IsNullOrEmpty(comment) Then
                strContents = strContents.Replace("##displayComment##", "")
                strContents = strContents.Replace("##comment##", comment.Replace("\n", "<br/>"))
            Else
                strContents = strContents.Replace("##displayComment##", "none")
                strContents = strContents.Replace("##comment##", "&nbsp;")
            End If
            strContents = strContents.Replace("##webRoot##", Utility.ConfigData.GlobalRefererName)
            strContents = strContents.Replace("##reviewId##", data.ReviewId)
            strContents = strContents.Replace("##itemId##", data.ItemId)
            strContents = strContents.Replace("##CustomerNo##", customerNo)
            strContents = strContents.Replace("##CustomerEmail##", addressType.Email)
            strContents = strContents.Replace("##CustomerPhone##", addressType.Phone)

            Dim isAddPoint As Boolean = StoreItemReviewRow.AllowAddPoint(DB, data.ReviewId)

            If isAddPoint Then
                Dim tdAddpoint As String = "<td style='width: 70px;  border-right:1px solid #DDDDDD;' align='left' valign='top'>"
                tdAddpoint = tdAddpoint & "       <a style='color:Black; font:12px Arial' href='" & webRoot & "/store/ReviewAccess.aspx?ReviewId=" & data.ReviewId & "&act=3&email=##Email##'>Add point</a></td>"
                strContents = strContents.Replace("##tdAddpoint##", tdAddpoint)
                strContents = strContents.Replace("##alignActive##", "center")
            Else
                strContents = strContents.Replace("##tdAddpoint##", "")
                strContents = strContents.Replace("##alignActive##", "left")
            End If

            Return strContents
        Catch Ex As Exception

        End Try
        Return String.Empty
    End Function
    Private Function CreateDivShowLevel(ByVal name As String) As String
        If (String.IsNullOrEmpty(name)) Then
            Return String.Empty
        End If
        Return "<div style='padding-left: 10px;color: #000000; font: 12px Arial,Helvetica,Verdana,sans-serif;'>-&nbsp;&nbsp;" & name & "</div>"

    End Function
    Private Function GetReviewProsName(ByVal code As String) As String
        Dim result As String = ""
        If (code = "#ProsEasyToUse#") Then
            result = "Easy To Use"
        ElseIf (code = "#ProsHighQuality#") Then
            result = "High Quality"
        ElseIf (code = "#ProsGoodValue#") Then
            result = "Good Value"
        ElseIf (code = "#ProsLongLasting#") Then
            result = "Long Lasting"
        End If
        Return CreateDivShowLevel(result)
    End Function
    Private Function GetReviewExperienceName(ByVal code As String) As String
        Dim result As String = ""
        If (code = "#ExperienceLevelStudent#") Then
            result = "Student"
        ElseIf (code = "#ExperienceLevel1#") Then
            result = "1+ Years"
        ElseIf (code = "#ExperienceLevel3#") Then
            result = "3+ Years"
        ElseIf (code = "#ExperienceLevel5#") Then
            result = "5+ Years"
        End If
        Return CreateDivShowLevel(result)
    End Function
    Private Function GetReviewConsName(ByVal code As String) As String
        Dim result As String = ""
        If (code = "#ConsDifficultToUse#") Then
            result = "Difficult To Use"
        ElseIf (code = "#ConsPoorQuality#") Then
            result = "Poor Quality"
        ElseIf (code = "#ConsExpensive#") Then
            result = "Expensive"
        ElseIf (code = "#ConsDoesNotWork#") Then
            result = "Does Not Work"
        End If
        Return CreateDivShowLevel(result)
    End Function
#End Region

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click, btnSave.Click
        Try
            Dim strComment As String = String.Empty
            ''  Comment
            Dim reviewComment As String = txtComments.InnerText
            Comment = strComment.Trim()
            'validate form
            Dim formError As Boolean = False
            Dim countWord As String = Common.CountWords(txtComments.InnerText)
            If (countWord < Utility.ConfigData.MinimumWordReview) Then
                formError = True
                CommentTextError = String.Format(Resources.Alert.CountWordReview, ConfigData.MinimumWordReview, countWord)
                dvCommentError.InnerText = CommentTextError
            End If
            If (formError) Then
                Exit Sub
            End If
            ItemRewiew = New StoreItemReviewRow
            ItemRewiew.ItemId = ItemId
            ItemRewiew.MemberId = MemberId
            ItemRewiew.ReviewTitle = RewriteUrl.StripTags(txtTitle.Text.Trim())
            ItemRewiew.FirstName = RewriteUrl.StripTags(txtFirstName.Text.Trim())
            ItemRewiew.LastName = RewriteUrl.StripTags(txtLastName.Text.Trim())
            ItemRewiew.CartItemId = CartItemId
            ItemRewiew.Comment = RewriteUrl.StripTags(txtComments.InnerText)
            ItemRewiew.DateAdded = DateTime.Now
            ItemRewiew.NumStars = IIf(hidStar.Value = "", 5, hidStar.Value).ToString.Replace(0, "")

            If ReviewId > 0 Then
                Dim dbStoreItemReview As StoreItemReviewRow = StoreItemReviewRow.GetRow(DB, ReviewId)
                ItemRewiew.ItemId = dbStoreItemReview.ItemId
                ItemRewiew.MemberId = dbStoreItemReview.MemberId
                ItemRewiew.ReviewId = ReviewId
                ItemRewiew.IsActive = dbStoreItemReview.IsActive
                ItemRewiew.Update()
            Else
                Dim CountData As Integer = DB.ExecuteScalar("select * from StoreItemReview where ItemId = " & ItemId & " and MemberId = " & MemberId)
                If CountData > 0 Then
                    LoadItem()
                    divReviewForm.Visible = False
                    litMsg.Text = Resources.Msg.review_thanks
                    Exit Sub
                End If

                ItemRewiew.Insert()
                Dim mailTitle As String = String.Empty
                If (ItemRewiew.CartItemId < 1) Then
                    mailTitle = "[Item Review]"
                Else
                    mailTitle = "[Cart Item Review]"
                End If
                Dim mailContent As String = GetEmailreview(ItemRewiew, reviewComment)
                ''Email.SendReport("ReportCustomerReview", mailTitle & txtTitle.Text, mailContent)

                ''send report
                Dim lstEmail As String = SysParam.GetValue(DB, "ReportCustomerReview")
                Dim subject As String = mailTitle & txtTitle.Text
                Dim arr() As String = lstEmail.Split(",")
                For Each ToEmail As String In arr
                    If (Not String.IsNullOrEmpty(ToEmail)) Then
                        Dim body As String = mailContent.Replace("##Email##", ToEmail)
                        Dim ToName As String = AdminRow.GetRowByEmail(ToEmail)
                        Email.SendHTMLMail(FromEmailType.NoReply, ToEmail, ToName, subject, body)
                    End If
                Next

            End If

            If ReviewId > 0 Then
                Response.Redirect("/admin/store/items/reviews/edit.aspx?ReviewId=" & ReviewId)
            End If

            LoadItem()
            divReviewForm.Visible = False
            litMsg.Text = Resources.Msg.review_thanks

        Catch ex As Exception
            'Response.Write(ex.ToString())
        End Try

    End Sub
    Private Function checkValidForm(ByVal Comment As String) As Boolean
        Dim flagName As Boolean = False
        Dim flagTitle As Boolean = False
        Dim result As Boolean = False
        If txtFirstName.Text = "" And txtLastName.Text = "" Then
            flagName = False
            '    lbName.Visible = True
        Else
            flagName = True
            '    lbName.Visible = False
        End If
        If txtTitle.Text = "" Then
            flagTitle = False
            'lbTitle.Visible = True
        Else
            flagTitle = True
            'lbTitle.Visible = False
        End If
        If flagName = False Then
            result = flagName
        Else
            result = flagTitle
        End If
        If Comment = "" Then
            ltScript.Text = "<script>document.getElementById('dvComment').style.display = '';</script>"
            result = False
        Else
            Dim countWord As String = Common.CountWords(Comment)
            If (countWord < 5) Then
                ltScript.Text = "<script>document.getElementById('dvCountWordError').innerHTML='Required minimum 5 words.You have typed " & countWord & " words.'; document.getElementById('dvCountWordError').style.display = '';</script>"
                result = False
            End If
        End If
        Return result
    End Function
End Class
