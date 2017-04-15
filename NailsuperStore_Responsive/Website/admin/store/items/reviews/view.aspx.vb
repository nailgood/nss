Imports Components
Imports DataLayer
Imports System.IO
Partial Class admin_store_items_reviews_view
    Inherits AdminPage
    Private strTemplate As String
    Protected GroupPros As String = ConfigurationManager.AppSettings("GroupPros")
    Protected GroupCons As String = ConfigurationManager.AppSettings("GroupCons")
    Protected GroupExperienceLevel As String = ConfigurationManager.AppSettings("GroupExperienceLevel")
    Private ItemId As Integer = 0
    Private sir As StoreItemReviewRow
    Private ReviewId As String
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Dim objReader As FileStream
        ReviewId = Request.QueryString("ReviewId")
        If Not Page.IsPostBack Then
            If ReviewId <> Nothing Then
                sir = StoreItemReviewRow.GetRow(DB, CInt(ReviewId))
                ItemId = sir.ItemId
                ''check Allow Addpoint
                Dim isAddPoint As Boolean = StoreItemReviewRow.AllowAddPoint(DB, ReviewId)
                If isAddPoint Then
                    btnAddPoint.Visible = True
                Else
                    btnAddPoint.Visible = False
                End If
                LoadItem()
                'objReader = New FileStream(Server.MapPath(ConfigurationManager.AppSettings("Reviewfile") & "template_loadreview_admin.txt"), FileMode.Open)
                'Dim sr As New StreamReader(objReader)
                strTemplate = Utility.Common.LoadTemplateReview(Server.MapPath(Utility.ConfigData.ReviewTemplateFile()), [Enum].GetName(GetType(Utility.Common.TemplateReview), 1)) 'sr.ReadToEnd
                ltrContent.Text = ByDataTemplate(strTemplate, sir) ''& "<div><b>The Nail Superstore says</b>:&nbsp;Yes, I would recommend this to a friend</div>"
                'objReader.Close()
                If sir.IsActive Then
                    btnActive.Visible = False
                End If
            End If
        End If
    End Sub
    Private Sub LoadItem()
        Try
            Dim si As StoreItemRow = StoreItemRow.GetRow(DB, ItemId)
            If si.Image <> Nothing Then
                divImg1.Style.Add("background", "url(/assets/items/featured/" & si.Image & ") no-repeat center center;")
            End If
            lblTitle.Text = si.ItemName
            lblDesc.Text = BBCodeHelper.ConvertBBCodeToHTML(si.ShortDesc)

        Catch ex As Exception

        End Try
    End Sub
    Private Function ByDataTemplate(ByVal Template As String, ByVal si As StoreItemReviewRow) As String
        Template = Template.Replace("#Title#", si.ReviewTitle)
        Template = Template.Replace("#Name#", si.FirstName & " " & si.LastName)
        Template = Template.Replace("#DateAdded#", si.DateAdded.ToShortDateString)
        Template = Template.Replace("#Star#", si.NumStars)
        Template = Template.Replace("#BottomLine#", IIf(si.IsRecommendFriend = True, "Yes, I would recommend this to a friend", "No, I would not recommend this to a friend"))
        Template = Template.Replace("<div id=""newComment"" class=""hidediv"">", "<div id=""newComment"" class=""showdiv"">")
        If (si.Comment.Contains("#txtComments#")) Then
            Dim arrData, arrData1 As String()
            arrData = si.Comment.Trim.Split(vbCrLf)
            Dim str0, str1, str2, str3 As String
            If arrData.Length > 1 Then
                'Template = Template.Replace("<div id=""newComment"" class=""hidediv"">", "<div id=""newComment"" class=""showdiv"">")
                For i As Integer = 0 To arrData.Length - 1
                    arrData1 = arrData(i).Split("=")
                    If arrData1.Length > 1 Then
                        str0 = arrData1(0).Replace(" ", "").Trim
                        str1 = arrData1(1)
                        str2 = FindDiv(str0, "hidediv")
                        str3 = FindDiv(str0, "showdiv")
                        If str1 <> "" Then
                            If str1 = "on" Then
                                Template = Template.Replace(str2, str3).Replace(str0.Trim, "")
                                Template = LoadGroup(str0.Trim.Replace("#", ""), Template)
                            Else
                                Template = Template.Replace(str2, str3)
                                Template = Template.Replace(str0.Trim, arrData1(1).Trim)
                            End If
                        Else
                            Template = Template.Replace(str0.Trim, "")
                        End If
                    End If
                Next
            Else
                Template = Template.Replace("<div id=""oldComment"" class=""hidediv"">#OldComment#</div>", "<div id=""oldComment"" class=""showdiv"">" & si.Comment & "</div>")
            End If
        Else
            Template = Template.Replace("#txtComments#", si.Comment)
            Template = Template.Replace("<div id=""txtComments"" class=""hidediv"">", "<div id=""txtComments"" class=""showdiv"">")
        End If


        Return Template.Replace("#", "").Replace("\n", "<br />")
    End Function
    Function LoadGroup(ByVal str As String, ByVal Template As String) As String
        If GroupPros.Contains(str) Then
            Template = Template.Replace("<div id=""GPros"" class=""groupChk hidediv"">", "<div id=""GPros"" class=""groupChk showdiv"">")
        End If
        If GroupCons.Contains(str) Then
            Template = Template.Replace("<div id=""GCons"" class=""groupChk hidediv"">", "<div id=""GCons"" class=""groupChk showdiv"">")
        End If
        If GroupExperienceLevel.Contains(str) Then
            Template = Template.Replace("<div id=""GExperienceLevel"" class=""groupChk hidediv"">", "<div id=""GExperienceLevel"" class=""groupChk showdiv"">")
        End If
        Return Template
    End Function
    Function FindDiv(ByVal DivId As String, ByVal Status As String) As String
        Dim str As String = "<div id=""" & DivId.Trim.Replace(" ", "").Replace("+", "").Replace("#", "") & """ class=""" & Status & """>"
        Return str
    End Function

    Protected Sub btnActive_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnActive.Click
        If ReviewId <> Nothing Then
            sir = StoreItemReviewRow.GetRow(DB, CInt(ReviewId))
            Dim addPoint As Integer = IIf(sir.IsFirstReview, Utility.ConfigData.PointFirstReview, Utility.ConfigData.PointOldReview)
            Dim logDetail As New AdminLogDetailRow

            sir.IsActive = True
            sir.Update()
            logDetail.Message = "IsActive|False|True[br]"
            Dim addPointresult As Boolean = CashPointRow.AddPointProductReview(DB, sir.MemberId, sir.ItemId, addPoint)
            If (addPointresult) Then
                logDetail.Message &= "AddPoint|False|True[br]"
            End If

            logDetail.ObjectType = Utility.Common.ObjectType.ProductReview.ToString()
            logDetail.ObjectId = sir.ReviewId
            logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)

            '' Response.Write("<script language='javascript'> {  window.opener.SetValue('1') ; window.close();}</script>")
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "ActiveReview", "ReloadParent();", True)
        End If
    End Sub
    Protected Sub btnAddpoint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddPoint.Click
        If ReviewId <> Nothing Then
            sir = StoreItemReviewRow.GetRow(DB, CInt(ReviewId))
            Dim addPoint As Integer = IIf(sir.IsFirstReview, Utility.ConfigData.PointFirstReview, Utility.ConfigData.PointOldReview)
            Dim logDetail As New AdminLogDetailRow
            Dim logMessage As String = Utility.Common.ObjectToString(sir)
            Dim addPointresult As Boolean = CashPointRow.AddPointProductReview(DB, sir.MemberId, sir.ItemId, addPoint)
            If (addPointresult) Then
                logMessage = logMessage & ", Add point to MemberId=" & sir.MemberId & " and ItemId=" & sir.ItemId

                logDetail.Message &= "AddPoint|False|True[br]"
                logDetail.ObjectType = Utility.Common.ObjectType.ProductReview.ToString()
                logDetail.ObjectId = sir.ReviewId
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                AdminLogHelper.WriteLuceneLogDetail(logDetail)
            Else
                logMessage = logMessage & ", Can not Add point to MemberId=" & sir.MemberId & " and ItemId=" & sir.ItemId
            End If


            WriteLogDetail("AddPoint Item Review", logMessage)
            ''  Response.Write("<script language='javascript'> {  window.opener.SetValue('1') ; window.close();}</script>")
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "AddPoint", "ReloadParent();", True)
        End If
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        ''Response.Write("<script language='javascript'> { window.close();}</script>")
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "Close", "ClosePopup();", True)
    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        ''Response.Write("<script language='javascript'> {  window.opener.SetValue('edit.aspx?ReviewId=" & ReviewId & "') ; window.close();}</script>")
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "EditReview", "EditReview('edit.aspx?ReviewId=" & ReviewId & "');", True)

    End Sub
End Class
