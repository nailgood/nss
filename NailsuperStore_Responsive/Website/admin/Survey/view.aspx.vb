Imports Components
Imports DataLayer

Partial Class admin_Survey_view
    Inherits AdminPage
    Private Id As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ss As String = GetQueryString("Id")
        Id = IIf(String.IsNullOrEmpty(ss), 0, CInt(ss))
        If Not Page.IsPostBack Then
            If Id > 0 Then
                LoadSurvey()
            End If
        End If
    End Sub
    Private Sub LoadSurvey()
        Dim surveyResult As SurveyResultRow = SurveyResultRow.GetRow(DB, Id)
        If Not surveyResult Is Nothing Then
            lbCustName.Text = surveyResult.CustomerName
            lbCustEmail.Text = surveyResult.CustomerEmail
            lbDate.Text = surveyResult.CreatedDate.ToShortDateString()
            If Not String.IsNullOrEmpty(surveyResult.Comment) Then
                trCommnent.Visible = True
                lbComment.Text = surveyResult.Comment
            Else
                trCommnent.Visible = False
            End If

            If surveyResult.MemberId > 0 Then
                Dim member As MemberRow = MemberRow.GetRow(surveyResult.MemberId)
                ltrMember.Text = member.Customer.Name & " " & member.Customer.Name2 & " (" & member.Customer.CustomerNo & ")&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;" & member.Customer.Phone
            Else
                trMember.Visible = False
            End If
        End If
        Dim lstSurveyQuestion As SurveyQuestionCollection = SurveyQuestionRow.GetQuestionBySurveyId(surveyResult.SurveyId)
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
            Dim dt As DataTable = SurveyAnswerRow.GetlistAnswerSelectedBySurveyResult(Id, Question.Id)
            If dt.Rows.Count > 0 Then
                Dim ltrQuestion As Literal = CType(e.Item.FindControl("ltrQuestion"), Literal)
                Dim rdlAnswer As RadioButtonList = CType(e.Item.FindControl("rdlAnswer"), RadioButtonList)
                Dim ltrNote As Literal = CType(e.Item.FindControl("ltrNote"), Literal)
                ltrQuestion.Text = num & ". " & Question.Question
                For i As Integer = 0 To dt.Rows.Count - 1
                    rdlAnswer.Items.Insert(i, New ListItem(dt.Rows(i)("Answer").ToString(), dt.Rows(i)("Id").ToString()))
                    rdlAnswer.Items(i).Selected = Convert.ToBoolean(dt.Rows(i)("IsSelect"))
                Next

                If Question.IsShowNote Then
                    rdlAnswer.RepeatLayout = RepeatLayout.Table
                Else
                    rdlAnswer.RepeatLayout = RepeatLayout.Flow
                End If
                Dim sNote As String = SurveyResultDetailRow.GetQuestionNoteBySurveyResultId(Id, Question.Id)
                If (Not String.IsNullOrEmpty(sNote)) Then
                    ltrNote.Text = "<span class=""note"">Note</span>: " & sNote
                End If
            End If
        End If
    End Sub
   
End Class
