Imports Components
Imports DataLayer
Imports System.IO

Partial Class store_survey
    Inherits SitePage

    Private OrderId As Integer = 0
    Private FlowLayout As Boolean = True

    Public Property SurveyId() As Integer
        Get
            Dim o As Object = ViewState("SurveyId")
            If o IsNot Nothing Then
                Return DirectCast(o, Integer)
            End If
            Return 0
        End Get

        Set(ByVal value As Integer)
            ViewState("SurveyId") = value
        End Set
    End Property
    Public Property MemberId() As Integer
        Get
            Dim o As Object = ViewState("MemberId")
            If o IsNot Nothing Then
                Return DirectCast(o, Integer)
            End If
            Return 0
        End Get

        Set(ByVal value As Integer)
            ViewState("MemberId") = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim Code As String = GetQueryString("surverycode") ''"SURVEY-WEBSITE" ''   "86613"
        Dim ss As String = GetQueryString("OrderId")
        OrderId = IIf(String.IsNullOrEmpty(ss), 0, Convert.ToInt32(ss))

        lbrqEmail.Visible = False
        lbValidateEmail.Visible = False
        lbExistEmail.Visible = False
        If Not Page.IsPostBack Then
            If Not String.IsNullOrEmpty(Code) Then
                Dim survey As SurveyRow = SurveyRow.GetRowByCode(DB, Code)
                If Not survey Is Nothing AndAlso survey.Id > 0 Then
                    SurveyId = survey.Id
                    If OrderId > 0 Then
                        Dim iCheck As Integer = SurveyResultRow.CheckSurveyByOrderId(DB, SurveyId, LoggedInMemberId, OrderId)
                        If iCheck = 2 Then
                            Response.Redirect("/home.aspx")
                        ElseIf iCheck = 1 Then
                            If LoggedInMemberId > 0 Then
                                ltrInformation.Text = Session("Username").ToString()
                            Else
                                ltrInformation.Text = "Your Order"
                            End If
                            dvSurveyForm.Visible = False
                            divCompleted.Visible = True
                            Exit Sub
                        End If
                        FlowLayout = True
                    Else
                        If LoggedInMemberId > 0 Then
                            If SurveyResultRow.CheckDuplicateEmail(DB, SurveyId, txtEmail.Text, LoggedInMemberId) Then
                                ltrInformation.Text = Session("Username").ToString()
                                dvSurveyForm.Visible = False
                                divCompleted.Visible = True
                                Exit Sub
                            End If
                        End If
                        FlowLayout = False
                    End If
                    ltrDescription.Text = survey.Description
                    trComment.Visible = survey.IsComment
                    BindList()
                Else
                    Response.Redirect("/home.aspx")
                End If
            Else
                Response.Redirect("/home.aspx")
            End If
        End If
    End Sub

    Private Sub BindList()
        If LoggedInMemberId > 0 Then
            MemberId = LoggedInMemberId
            txtName.Text = LoggedInFirstName & " " & LoggedInLastName
            txtEmail.Text = LoggedInEmail
        Else
            MemberId = StoreOrderRow.GetRow(DB, OrderId).MemberId
            Dim member As MemberRow = MemberRow.GetRow(MemberId)
            Dim cust As CustomerRow = member.Customer
            txtName.Text = cust.Name & " " & cust.Name2
            txtEmail.Text = cust.Email
        End If
        Dim lstSurveyQuestion As SurveyQuestionCollection = SurveyQuestionRow.GetQuestionBySurveyId(SurveyId)
        If lstSurveyQuestion.Count > 0 Then
            dlQuestion.DataSource = lstSurveyQuestion
            dlQuestion.DataBind()
        End If
    End Sub

    Private num As Integer = 0
    Protected Sub dlQuestion_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlQuestion.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            num = num + 1
            Dim Question As SurveyQuestionRow = DirectCast(DirectCast(e.Item.DataItem, Object), SurveyQuestionRow)
            Dim lstSurveyAnswer As SurveyAnswerCollection = SurveyAnswerRow.GetAnswersByQuestionId(Question.Id)
            If lstSurveyAnswer.Count > 0 Then
                Dim ltrQuestion As Literal = CType(e.Item.FindControl("ltrQuestion"), Literal)
                Dim hdQuestionId As HiddenField = CType(e.Item.FindControl("hdQuestionId"), HiddenField)
                Dim rdlAnswer As RadioButtonList = CType(e.Item.FindControl("rdlAnswer"), RadioButtonList)
                Dim QuestionNote As TextBox = CType(e.Item.FindControl("txtQNote"), TextBox)
                QuestionNote.Visible = Question.IsShowNote
                ltrQuestion.Text = num & ". " & Question.Question
                hdQuestionId.Value = Question.Id
                If FlowLayout Then
                    rdlAnswer.RepeatLayout = RepeatLayout.Flow
                Else
                    rdlAnswer.RepeatLayout = RepeatLayout.Table

                End If
                For i As Integer = 0 To lstSurveyAnswer.Count - 1
                    Dim item As SurveyAnswerRow = lstSurveyAnswer.Item(i)
                    rdlAnswer.Items.Insert(i, New ListItem(item.Answer, item.Id))
                    rdlAnswer.Items(i).Selected = item.IsDefaultSelect
                Next
            End If
        End If
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        If Page.IsValid Then
            Try
                Dim sQuestion As String = String.Empty
                Dim bError As Boolean = False

                '''''''check validate Name, Email''''''''''''
                If String.IsNullOrEmpty(txtName.Text.Trim()) Then
                    bError = True
                End If
                If String.IsNullOrEmpty(txtEmail.Text.Trim()) Then
                    bError = True
                    lbrqEmail.Visible = True
                Else
                    If Not Utility.Common.CheckValidEmail(txtEmail.Text.Trim()) Then
                        bError = True
                        lbValidateEmail.Visible = True
                    End If
                End If
                If OrderId = 0 Then
                    If SurveyResultRow.CheckDuplicateEmail(DB, SurveyId, txtEmail.Text.Trim(), MemberId) Then
                        bError = True
                        lbExistEmail.Visible = True
                    End If
                Else
                    Dim iCheck As Integer = SurveyResultRow.CheckSurveyByOrderId(DB, SurveyId, LoggedInMemberId, OrderId)
                    If iCheck = 2 Then
                        Response.Redirect("/home.aspx")
                    ElseIf iCheck = 1 Then
                        If LoggedInMemberId > 0 Then
                            ltrInformation.Text = Session("Username").ToString()
                        Else
                            ltrInformation.Text = "Your Order"
                        End If
                        dvSurveyForm.Visible = False
                        divCompleted.Visible = True
                        bError = True
                    End If
                End If
                If bError Then
                    Exit Sub
                End If
                '''''''''''''''''''''''''''''''
                DB.BeginTransaction()
                Dim surveyResult As New SurveyResultRow

                surveyResult.SurveyId = SurveyId
                surveyResult.MemberId = MemberId
                surveyResult.OrderId = OrderId
                surveyResult.CustomerEmail = txtEmail.Text.Trim()
                surveyResult.CustomerName = txtName.Text.Trim()
                surveyResult.Comment = txtComment.InnerText.Trim()

                surveyResult.Id = SurveyResultRow.Insert(DB, surveyResult)

                If surveyResult.Id > 0 Then
                    For Each item As DataListItem In dlQuestion.Items
                        Dim hdQuestionId As HiddenField = CType(item.FindControl("hdQuestionId"), HiddenField)
                        Dim rdlAnswer As RadioButtonList = CType(item.FindControl("rdlAnswer"), RadioButtonList)
                        Dim QuestionNote As TextBox = CType(item.FindControl("txtQNote"), TextBox)
                        Dim surveyDetail As New SurveyResultDetailRow
                        surveyDetail.SurveyResultId = surveyResult.Id
                        surveyDetail.QuestionId = hdQuestionId.Value
                        surveyDetail.AnswerId = IIf(String.IsNullOrEmpty(rdlAnswer.SelectedValue), 0, rdlAnswer.SelectedValue)
                        If (QuestionNote.Text.Trim() = "Leave a suggestion") Then
                            QuestionNote.Text = String.Empty
                        End If
                        surveyDetail.Note = QuestionNote.Text.Trim()
                        If (surveyDetail.AnswerId > 0) Then
                            SurveyResultDetailRow.Insert(DB, surveyDetail)
                            Dim ltrQuestion As Literal = CType(item.FindControl("ltrQuestion"), Literal)
                            Dim w As String = String.Empty
                            Dim align As String = String.Empty
                            If rdlAnswer.Items.Count >= 5 Then
                                w = "width:100%;"
                                align = "text-align:center;"
                            End If
                            sQuestion &= "<div style=""margin-bottom:4px;"">" & ltrQuestion.Text & "<br/><table style=""margin-bottom:3px;" & w & """ cellpadding=""0"" cellspacing=""0""><tr>"
                            Dim t As Integer = 0
                            For Each rd As ListItem In rdlAnswer.Items
                                If t >= 5 Then
                                    sQuestion &= "</tr><tr>"
                                    t = 0
                                End If
                                If rd.Selected Then
                                    sQuestion &= "<td style=""padding-right:10px;color: #000000; font: 12px Arial,Helvetica,Verdana,sans-serif;padding-left:13px;" & align & """>(X) " & rd.Text & "</td>"
                                Else
                                    sQuestion &= "<td style=""padding-right:10px;color: #000000; font: 12px Arial,Helvetica,Verdana,sans-serif;padding-left:13px;" & align & """>( ) " & rd.Text & "</td>"
                                End If
                                t = t + 1
                            Next
                            sQuestion &= "</tr></table>"
                            If Not String.IsNullOrEmpty(QuestionNote.Text) Then
                                sQuestion &= "<div style=""width:350px;padding-left:13px;""><span style=""font-style:italic;text-decoration:underline;"">Note:</span> " & QuestionNote.Text & "</div>"
                            End If
                            sQuestion &= "</div>"
                        Else
                            bError = True
                            Exit For
                        End If
                    Next
                End If
                If bError Then
                    DB.RollbackTransaction()
                    AddError("Please answer all our questions.")
                    Exit Sub
                Else
                    DB.CommitTransaction()
                End If
                Dim obj As StoreOrderRow = StoreOrderRow.GetRow(DB, OrderId)
                Dim member As New MemberRow
                Dim cust As New CustomerRow
                If MemberId > 0 Then
                    member = MemberRow.GetRow(MemberId)
                    cust = member.Customer
                Else
                    member = MemberRow.GetRowByEmail(DB, txtEmail.Text)
                    cust = member.Customer
                End If

                Dim body As String = GetEmailSurvey(txtName.Text, txtEmail.Text, member.MemberId, cust.CustomerNo, cust.Name & " " & cust.Name2, cust.Phone, sQuestion, txtComment.InnerText)
                Dim subject As String = String.Empty
                If OrderId > 0 Then
                    subject = "[Order Survey] Customer #" & cust.CustomerNo & " takes the survey"
                Else
                    If Not String.IsNullOrEmpty(cust.CustomerNo) Then
                        subject = "[Survey] Customer #" & cust.CustomerNo & " takes the survey"
                    Else
                        subject = "[Survey] " & txtEmail.Text & " takes the survey"
                    End If
                End If
                Email.SendReport("ToReportSurvey", subject, body)

                dvSurveyForm.Visible = False
                ltlEmail.Text = txtEmail.Text
                divConfirm.Visible = True
            Catch ex As Exception
                DB.RollbackTransaction()
            End Try
        End If
    End Sub

    Private Function GetEmailSurvey(ByVal CustomerName As String, ByVal CustomerEmail As String, ByVal MemberId As Integer, ByVal CustomerNo As String, ByVal FullName As String, ByVal CustomerPhone As String, ByVal Question As String, ByVal Comment As String) As String
        Dim FullPath As String = Server.MapPath("~/includes/MailTemplate/EmailSurveyTemplate.htm")
        Dim result As String = String.Empty
        Dim strContents As String
        Dim objReader As StreamReader
        Dim ShowMember As String = "<tr>"
        ShowMember &= "<td style=""margin: 0px; width: 100%; padding: 0px 0px 2px 0px;"" align=""left"" valign=""top"">"
        ShowMember &= "<table cellpadding=""0"" cellspacing=""0"">"
        ShowMember &= "<tr>"
        ShowMember &= "<td style=""margin: 0px; padding: 0px 4px 0px 0px; color: #000000; font: bold 12px Arial,Helvetica,Verdana,sans-serif;"" align=""left"""
        ShowMember &= "valign=""top"">Member:"
        ShowMember &= "</td>"
        ShowMember &= "<td align=""left"" valign=""top"" style=""color: #000000; font: 12px Arial;"">"
        ShowMember &= "<a style=""color:Black; font:12px Arial"" href=""##webRoot##/admin/members/edit.aspx?MemberId=##MemberId##&act=email"">##FullName##</a>&nbsp;&nbsp;(##CustomerNo##)"
        ShowMember &= "</td>"
        ShowMember &= "<td style=""width:45px; text-align:center;color: #000000; font:  12px Arial,Helvetica,Verdana,sans-serif;"">  | </td>"
        ShowMember &= "<td style=""color: #000000; font:  12px Arial,Helvetica,Verdana,sans-serif;"">"
        ShowMember &= "##CustomerPhone##"
        ShowMember &= "</td>"
        ShowMember &= "</tr>"
        ShowMember &= "</table>"
        ShowMember &= "</td>"
        ShowMember &= "</tr>"

        Dim ShowOrder As String = String.Empty

        If OrderId > 0 Then
            Dim OrderNo As String = StoreOrderRow.GetRow(DB, OrderId).OrderNo
            ShowOrder &= "<tr>"
            ShowOrder &= "<td style=""margin: 0px; width: 100%; padding: 0px 0px 2px 0px;"" align=""left"" valign=""top"">"
            ShowOrder &= "<table style=""width:610px;"" cellpadding=""0"" cellspacing=""0"">"
            ShowOrder &= "<tr>"
            ShowOrder &= "<td style=""padding: 0px 4px 0px 0px;color: #000000; font:bold 12px Arial,Helvetica,Verdana,sans-serif; width: 50px;"" valign=""top"" align=""left"">Order:</td>"
            ShowOrder &= " <td style=""padding-left: 0px;color: #000000; font: 12px Arial,Helvetica,Verdana,sans-serif;"" valign=""top"" align=""left"">"
            If (Not String.IsNullOrEmpty(OrderNo)) Then
                ShowOrder &= "<a href=""##webRoot##/admin/store/orders/edit.aspx?OrderId=" & OrderId & """>" & OrderNo & "</a>"
            Else
                ShowOrder &= "<a href=""##webRoot##/admin/store/CartDetail/edit.aspx?OrderId=" & OrderId & """>" & OrderId & "</a>"
            End If
            ShowOrder &= "</td>"
            ShowOrder &= " </tr>"
            ShowOrder &= "</table>"
            ShowOrder &= " </td>"
            ShowOrder &= "</tr>"
        End If
        Try
            Dim webRoot As String = Utility.ConfigData.GlobalRefererName
            objReader = New StreamReader(FullPath)
            strContents = objReader.ReadToEnd()
            objReader.Close()
            strContents = strContents.Replace("##DisplayOrder##", ShowOrder)
            strContents = strContents.Replace("##date##", DateTime.Now.ToString("MM/dd/yyyy"))
            strContents = strContents.Replace("##CustomerName##", CustomerName)
            strContents = strContents.Replace("##CustomerEmail##", CustomerEmail)
            If MemberId > 0 Then
                ShowMember = ShowMember.Replace("##FullName##", FullName)
                ShowMember = ShowMember.Replace("##MemberId##", MemberId)
                ShowMember = ShowMember.Replace("##CustomerNo##", CustomerNo)
                ShowMember = ShowMember.Replace("##CustomerPhone##", CustomerPhone)
                strContents = strContents.Replace("##DisplayMember##", ShowMember)
            Else
                strContents = strContents.Replace("##DisplayMember##", String.Empty)
            End If
            strContents = strContents.Replace("##Comment##", Comment.Replace(vbCrLf, "<br/>"))

            If Not String.IsNullOrEmpty(Comment) Then
                strContents = strContents.Replace(" ##TitleComment##", "Comment: ")
                strContents = strContents.Replace("##Comment##", Comment.Replace(vbCrLf, "<br/>"))
            Else
                strContents = strContents.Replace("##TitleComment##", String.Empty)
                strContents = strContents.Replace("##Comment##", String.Empty)
            End If
            strContents = strContents.Replace("##Question##", Question)
            strContents = strContents.Replace("##webRoot##", webRoot)
            Return strContents
        Catch Ex As Exception

        End Try
        Return String.Empty
    End Function
End Class
