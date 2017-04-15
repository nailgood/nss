Imports Components
Imports DataLayer
Imports System.Text
Imports System.IO
Imports Utility
Partial Class ReviewOrderWrite
    Inherits SitePage
    Private OrderId As Integer = 0
    Dim OrderReview As StoreOrderReviewRow
    Protected Rating As String = ""
    Private o As StoreOrderRow
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not GetQueryString("id") Is Nothing Then
            Dim queryId As String = GetQueryString("id")
            If IsNumeric(queryId) Then
                OrderId = queryId
            Else
                Try
                    OrderId = CryptData.Crypt.DecryptTripleDes(queryId)
                Catch ex As Exception
                    If queryId.Contains(" ") Then
                        queryId = queryId.Replace(" ", "+")
                        OrderId = CryptData.Crypt.DecryptTripleDes(queryId)
                    Else
                        OrderId = CryptData.Crypt.Decode(queryId)
                    End If
                End Try

            End If
            ''OrderId = queryId
            If Not Page.IsPostBack Then
                hidCountMessage.Value = Resources.Alert.CountWordReview
                hidMinimumWordComment.Value = ConfigData.MinimumWordReview
                dvMsgComment.InnerText = String.Format(Resources.Alert.CountWordReview, Utility.ConfigData.MinimumWordReview, 0)

            End If

            If OrderReview.CheckReview(DB, OrderId) = True Then
                divReviewForm.Visible = False
                dexist.Visible = True
            Else
                o = StoreOrderRow.GetRow(DB, OrderId)
                lblOrderNo.InnerText = o.OrderNo
                If Not GetQueryString("ex") Is Nothing Then
                    Rating = GetQueryString("ex")
                    Select Case Rating
                        Case "1"
                            lbStarType.InnerText = "(Awful)"
                            hidStar.Value = "1"
                        Case "2"
                            lbStarType.InnerText = "(Poor)"
                            hidStar.Value = "2"
                        Case "3"
                            lbStarType.InnerText = "(Neutral)"
                            hidStar.Value = "3"
                        Case "4"
                            lbStarType.InnerText = "(Good)"
                            hidStar.Value = "4"
                        Case "5"
                            lbStarType.InnerText = "(Excellent)"
                            hidStar.Value = "5"
                    End Select

                    'hidStar.Value = "5"
                    lbExcellence.InnerText = "We're glad you had a good experience form "
                    lbStar.InnerText = Rating
                    Rating &= "0"

                Else
                    Rating = "00"
                End If
            End If

        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If OrderReview.CheckReview(DB, OrderId) = False Then
            Dim isError As Boolean = False
            lbMsgRating.Attributes.Add("style", "display:none")
            dvMsgComment.Attributes.Add("style", "display:none")
            If hidStar.Value = "0" Or hidStar.Value = "" Then
                If Not GetQueryString("ex") Is Nothing Then
                    hidStar.Value = 5
                Else
                    lbMsgRating.Attributes.Add("style", "display:")
                    isError = True
                End If
            End If
            Dim countWords As Integer = 0
            Dim comment As String = txtComments.InnerText
            If Not (String.IsNullOrEmpty(comment)) Then
                countWords = Common.CountWords(comment)
            End If
            If (countWords < Utility.ConfigData.MinimumWordReview) Then
                dvMsgComment.InnerText = String.Format(Resources.Alert.CountWordReview, Utility.ConfigData.MinimumWordReview, countWords)
                dvMsgComment.Attributes.Add("style", "display:")
                isError = True
            End If
            If (isError) Then
                Exit Sub
            End If
            OrderReview = New StoreOrderReviewRow
            'Dim o As StoreOrderRow = StoreOrderRow.GetRow(DB, OrderId)
            OrderReview.OrderId = OrderId
            OrderReview.MemberId = o.MemberId
            OrderReview.ItemArrived = radArrived.SelectedValue
            OrderReview.ItemDescribed = radDescribled.SelectedValue
            OrderReview.ServicePrompt = radServicelist.SelectedValue
            OrderReview.NumStars = IIf(hidStar.Value = "", 5, hidStar.Value).ToString.Replace(0, "")
            OrderReview.Comment = BBCodeHelper.ConvertBBCodeToHTML(BBCodeHelper.StripHtml(txtComments.InnerText))
            OrderReview.Insert()
            Dim mailTemplate As String = GetEmailreview(OrderReview.NumStars, OrderReview.MemberId, txtComments.InnerText, OrderReview.OrderId)
            'Email.SendReport("ReportCustomerReview", "Customer reviews for the purchase order # " & lblOrderNo.InnerText, mailTemplate)
            'Email.SendReport("ReportCustomerReview", "Customer reviews for the purchase order #", OrderReview.Comment)

            ''send report
            Dim lstEmail As String = SysParam.GetValue(DB, "ReportCustomerReview")
            Dim subject As String = String.Format(Resources.Msg.review_order_subject_mail, lblOrderNo.InnerText)
            Dim arr() As String = lstEmail.Split(",")
            For Each ToEmail As String In arr
                If (Not String.IsNullOrEmpty(ToEmail)) Then
                    Dim body As String = mailTemplate.Replace("##Email##", ToEmail)
                    Dim ToName As String = AdminRow.GetRowByEmail(ToEmail)
                    Email.SendHTMLMail(FromEmailType.NoReply, ToEmail, ToName, subject, body)
                End If
            Next
        End If
        divReviewForm.Visible = False
        dexist.Visible = True
    End Sub
    Private Function GetEmailreview(ByVal start As Integer, ByVal memberId As Integer, ByVal comment As String, ByVal OrderId As Integer) As String
        Dim orderRow As StoreOrderRow = StoreOrderRow.GetRow(DB, OrderId)
        Dim FullPath As String = Server.MapPath("~/includes/MailTemplate/EmailReviewOrderTemplate.htm")
        Dim result As String = String.Empty
        Dim strContents As String
        Dim objReader As StreamReader
        Try
            Dim webRoot As String = Utility.ConfigData.GlobalRefererName
            objReader = New StreamReader(FullPath)
            strContents = objReader.ReadToEnd()
            objReader.Close()
            Dim Level As String = Utility.Common.ConvertStartNumberToTextLevelOrderReview(start)
            'Dim rating As String = ""
            'rating = rating & "<div style=""background: url('" & webRoot & "/App_Themes/Default/images/star" & start & "0.png') no-repeat center -1px; display: inline; overflow: hidden; padding: 2px 5px;"" > "
            'rating = rating & "     <img width='90px' height='22px' src='/App_Themes/Default/images/spacer.gif'></div> " & start.ToString() & "" & Level
            strContents = strContents.Replace("##webroot##", webRoot)
            strContents = strContents.Replace("##star##", start.ToString())
            strContents = strContents.Replace("##level##", Level)
            Dim member As MemberRow = MemberRow.GetRow(memberId)
            Dim customerNo As String = member.Customer.CustomerNo
            Dim addressType As MemberAddressRow = MemberAddressRow.GetAddressByType(DB, member.MemberId, Utility.Common.MemberAddressType.Billing.ToString())
            Dim name As String = member.Customer.Name + " " + member.Customer.Name2
            strContents = strContents.Replace("##name##", name)
            strContents = strContents.Replace("##OrderId##", OrderId)
            strContents = strContents.Replace("##OrderNo##", orderRow.OrderNo)
            strContents = strContents.Replace("##CustomerNo##", customerNo)
            strContents = strContents.Replace("##CustomerEmail##", addressType.Email)
            strContents = strContents.Replace("##CustomerPhone##", addressType.Phone)
            strContents = strContents.Replace("##comment##", comment.Replace(vbCrLf, "<br/>"))
            Return strContents
        Catch Ex As Exception

        End Try
        Return String.Empty
    End Function

End Class
