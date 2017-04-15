Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_News
    Inherits AdminPage
    Private TotalRecords As Integer
    Protected NewsId As Integer
    Private Type As Integer = 1
    Dim ToTal As Integer
    Dim strIsChecked As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("Type") <> Nothing AndAlso Request.QueryString("Type").Length > 0 Then
            Type = CInt(Request.QueryString("Type"))
        End If

        If Request.QueryString("NewsId") <> Nothing AndAlso IsNumeric(Request.QueryString("NewsId")) Then
            NewsId = Request("NewsId")
        End If

        If Not IsPostBack Then
            LoadCategory()
            If Type = "2" Then
                LoadDetail()
            Else
                'BindList()
                btnSearch_Click(sender, e)
            End If
        End If
    End Sub

    Private Sub LoadDetail()
        pnList.Visible = False
        pnNew.Visible = True
        ltrHeader.Text = "Edit"
        Dim News As NewsRow = NewsRow.GetRow(DB, NewsId)
        txtTitle.Text = News.Title
        txtMetaKeyword.Text = News.MetaKeyword
        txtMetaDescription.Text = News.MetaDescription
        chkIsActive.Checked = News.IsActive
        txtShortContent.Text = News.ShortDescription
        txtContent.Text = News.Description
        txtPageTitle.Text = News.PageTitle
        fuImage.Folder = Utility.ConfigData.PathThumbNewsImage
        fuImage.CurrentFileName = News.ThumbImage
        hpimg.ImageUrl = Utility.ConfigData.PathThumbNewsImage & News.ThumbImage
        If fuImage.CurrentFileName <> Nothing Then
            divImg.Visible = True
        End If
        LoadNewsAudio(NewsId)
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "InitEditor", "InitEditor()", True)
    End Sub
    Private Function GetTitle() As String
        Return "News"
    End Function
    Private Sub BindQuery()
        If Request("F_Title") <> Nothing Then
            F_Title.Text = Request("F_Title")
        End If
        If Request("F_IsActive") <> Nothing Then
            F_IsActive.SelectedValue = Request("F_IsActive")
        End If
        Dim SQL As String
        Dim Conn As String = " where "


        SQL = " FROM News n right join NewsCategory nc on n.NewsId = nc.NewsId  "

        If Not F_Title.Text = String.Empty Then
            SQL = SQL & Conn & " n.title LIKE " & DB.FilterQuote(F_Title.Text)
            Conn = " AND "
        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL = SQL & Conn & " n.isactive  = " & DB.Number(F_IsActive.SelectedValue)
            Conn = " AND "
        End If
        If F_CatId.SelectedValue <> Nothing Then
            SQL = SQL & Conn & " nc.CategoryId  = " & DB.Number(F_CatId.SelectedValue)
            Conn = " AND "
        End If
        hidCon.Value = SQL
    End Sub
    Private Sub BindList()
        Dim SQLFields As String = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " n.NewsId,Title,IsActive"
        ToTal = DB.ExecuteScalar("SELECT COUNT(*) " & hidCon.Value)
        gvList.Pager.NofRecords = ToTal

        Dim res As DataTable = DB.GetDataTable(SQLFields & hidCon.Value & " ORDER BY nc.arrange DESC")
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindQuery()
        BindList()
    End Sub


    Private Sub LoadCategory()
        Dim dt As DataTable = CategoryRow.GetCategoryById(DB, NewsId, Utility.Common.CategoryType.News)
        Dim count As Integer = dt.Rows.Count

        If count > 0 Then
            cblCategory.DataSource = dt.DefaultView
            cblCategory.DataTextField = "CategoryName"
            cblCategory.DataValueField = "CategoryId"
            cblCategory.DataBind()
            For i As Integer = 0 To count - 1
                If dt.Rows(i)("IsChecked") = 1 Then
                    If strIsChecked = "" Then
                        strIsChecked &= dt.Rows(i)("CategoryId")
                    Else
                        strIsChecked &= "," & dt.Rows(i)("CategoryId")
                    End If
                End If
            Next
        End If
        cblCategory.SelectedValues = strIsChecked



        F_CatId.DataSource = dt.DefaultView
        F_CatId.DataTextField = "CategoryName"
        F_CatId.DataValueField = "CategoryId"
        F_CatId.DataBind()
        If Request("F_CatId") <> Nothing Then
            F_CatId.SelectedValue = Request("F_CatId")
        End If
    End Sub
    Private Function InsertAudio() As Integer
        Dim audio As New AudioRow
        audio.FileUrl = txtAudioUrl.Text.Trim()
        audio.Title = txtAudioName.Text.Trim()
        If (txtAudioUrl.Text <> "") Then
            Return AudioRow.Insert(DB, audio)
        Else
            Return 0
        End If
        Return 0
    End Function
    Private Sub LoadNewsAudio(ByVal newsID As Integer)
        Dim result As NewsAudioCollection = NewsAudioRow.GetByNewId(DB, newsID)
        If result.Count < 1 Then
            Return
        End If
        If (result(0).AudioId > 0) Then
            Dim audio As AudioRow = AudioRow.GetRow(DB, result(0).AudioId)
            txtAudioUrl.Text = audio.FileUrl
            txtAudioName.Text = audio.Title
        End If
    End Sub
    Private Sub InsertNewAudio(ByVal audioId As Integer, ByVal newsId As Integer)
        Dim audioNew As New NewsAudioRow
        audioNew.AudioId = audioId
        audioNew.NewsId = newsId
        NewsAudioRow.Insert(DB, audioNew)
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        strIsChecked = cblCategory.SelectedValues
        If strIsChecked = "" Then
            lblCategory.Text = "Please select category"
            Exit Sub
        End If
        Dim News As New NewsRow(DB)
        Dim NewsBefore As New NewsRow
        Dim oldImage As String = String.Empty
        If NewsId > 0 Then
            News = NewsRow.GetRow(DB, NewsId)
            oldImage = News.ThumbImage
            NewsBefore = CloneObject.Clone(News)
            NewsBefore.ListCategoryId = NewsRow.GetListCategoryIdByNewsId(DB, NewsId)
        End If
        Dim logDetail As New AdminLogDetailRow

        Try
            DB.BeginTransaction()
            News.IsActive = chkIsActive.Checked
            News.ShortDescription = txtShortContent.Text
            News.Description = txtContent.Text.Trim().Replace("../../../assets/uploadimage/", "/assets/uploadimage/")
            News.Title = txtTitle.Text.Trim()
            News.MetaKeyword = txtMetaKeyword.Text.Trim()
            News.MetaDescription = txtMetaDescription.Text.Trim()
            News.PageTitle = txtPageTitle.Text
            '''''''''''''''''''''''''''''''''''''''''''
            'fuImage.AutoResize = True
            fuImage.Folder = "~" & Utility.ConfigData.PathNewImage
            Dim newImageName As String = Guid.NewGuid().ToString() & fuImage.OriginalExtension
            Dim ImagePath As String = Server.MapPath("~" & Utility.ConfigData.PathNewImage)
            Dim ThumbImagePath As String = Server.MapPath("~" & Utility.ConfigData.PathThumbNewsImage)

            If fuImage.NewFileName <> String.Empty Then
                fuImage.NewFileName = IIf(oldImage <> String.Empty, oldImage, newImageName)
                fuImage.SaveNewFile()

                Dim OriginalImg As System.Drawing.Image = System.Drawing.Image.FromFile(ImagePath & fuImage.NewFileName)
                If OriginalImg.Width < 770 Or OriginalImg.Height < 433 Then
                    Utility.File.DeleteFile(ImagePath & fuImage.NewFileName)
                    lblImgError.Visible = True
                    DB.RollbackTransaction()
                    Exit Sub
                Else
                    lblImgError.Visible = False
                End If
            ElseIf fuImage.MarkedToDelete Then
                Utility.File.DeleteFile(ImagePath & oldImage)
                Utility.File.DeleteFile(ThumbImagePath & oldImage)
                News.ThumbImage = Nothing
            End If

            If NewsId > 0 Then
                News.NewsId = NewsId
                If fuImage.NewFileName <> String.Empty Then
                    Core.CropByAnchor(ImagePath & fuImage.NewFileName, ThumbImagePath & NewsId & fuImage.OriginalExtension, 770, 433, Utility.Common.ImageAnchorPosition.Center)
                    News.ThumbImage = NewsId & fuImage.OriginalExtension
                End If
                News.Update()

                News.RemoveAllNewsCategory(DB)
                News.InsertNewsCategory(DB, strIsChecked, NewsId)
                News.ListCategoryId = NewsRow.GetListCategoryIdByNewsId(DB, NewsId)

                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                logDetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.News, NewsBefore, News)
            Else
                NewsId = News.Insert()
                If fuImage.NewFileName <> String.Empty Then
                    Core.CropByAnchor(ImagePath & fuImage.NewFileName, ThumbImagePath & NewsId & fuImage.OriginalExtension, 874, 492, Utility.Common.ImageAnchorPosition.Center)
                    News.ThumbImage = NewsId & fuImage.OriginalExtension
                    News.NewsId = NewsId
                    News.Update()
                End If

                News.InsertNewsCategory(DB, strIsChecked, NewsId)
                News.ListCategoryId = NewsRow.GetListCategoryIdByNewsId(DB, NewsId)
                logDetail.Action = Utility.Common.AdminLogAction.Insert.ToString()
                logDetail.Message = AdminLogHelper.ConvertObjectInsertToLogMesssageString(News, Utility.Common.ObjectType.News)
            End If
            Utility.File.DeleteFile(ImagePath & fuImage.NewFileName)
            If NewsId > 0 Then
                Dim audioId As Integer = InsertAudio()
                If (audioId > 0) Then
                    InsertNewAudio(audioId, NewsId)
                Else
                    NewsAudioRow.DeleteByNewsId(DB, NewsId)
                End If
                DB.CommitTransaction()
                logDetail.ObjectId = NewsId
                logDetail.ObjectType = Utility.Common.ObjectType.News.ToString()
                AdminLogHelper.WriteLuceneLogDetail(logDetail)
            Else
                DB.RollbackTransaction()
            End If
            Response.Redirect("Default.aspx?F_CatId=" & cblCategory.SelectedValue)
        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "InitEditor", "InitEditor()", True)
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        pnList.Visible = True
        pnNew.Visible = False
        txtContent.Text = ""
        txtShortContent.Text = ""
        txtTitle.Text = ""
        txtMetaKeyword.Text = ""
        txtMetaDescription.Text = ""
        txtPageTitle.Text = ""
        ltrHeader.Text = GetTitle()
        Response.Redirect("default.aspx?F_CatId=" & cblCategory.SelectedValue)
    End Sub

    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        pnList.Visible = False
        pnNew.Visible = True

        ltrHeader.Text = "Add new"
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "InitEditor", "InitEditor()", True)
        'ddlCategory.SelectedValue = F_CatId.SelectedValue
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim NewsId As Integer = e.Row.DataItem("NewsId")
            Dim CNewsDoc As NewsDocumentCollection = NewsDocumentRow.ListAllForNewsId(DB, NewsId)
            Dim ltDocument As Literal = CType(e.Row.FindControl("ltDocument"), Literal)
            ltDocument.Text = String.Format("<a href='../NewsDocument/default.aspx?NewsId={0}&{2}'>{1}</a>", NewsId, CNewsDoc.Count, GetPageParams(FilterFieldType.All))
            Dim active As Boolean = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("IsActive")
            Dim ltImage As Literal = CType(e.Row.FindControl("ltImage"), Literal)
            Dim RNewsImg As New NewsImageRow
            RNewsImg.PageIndex = 1
            RNewsImg.Condition = "NewsID=" & NewsId
            RNewsImg.PageSize = Int32.MaxValue
            Dim CNewsImg As NewsImageCollection = NewsImageRow.ListAllByNewId(DB, RNewsImg)
            ltImage.Text = String.Format("<a href='NewsImage.aspx?id={0}&{2}'>{1}</a>", NewsId, CNewsImg.Count, GetPageParams(FilterFieldType.All))
            Dim imbActive As ImageButton = CType(e.Row.FindControl("imbActive"), ImageButton)

            If active Then
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If
            ''Dim data As NewsRow = DirectCast(e.Row.DataItem, NewsRow)
            Dim imbUp As ImageButton = CType(e.Row.FindControl("imbUp"), ImageButton)
            Dim imbDown As ImageButton = CType(e.Row.FindControl("imbDown"), ImageButton)

            ''imbDown.CommandArgument=
            If (ToTal < 2) Then
                imbUp.Visible = False
                imbDown.Visible = False
            Else
                If e.Row.RowIndex = 0 Then
                    imbUp.Visible = False
                ElseIf e.Row.RowIndex = ToTal - 1 Then
                    imbDown.Visible = False
                End If
            End If
            If (F_CatId.SelectedValue = Utility.ConfigData.BlogId) Then
                e.Row.Cells(6).Visible = False
                imbDown.Visible = False
                imbUp.Visible = False
            End If


        End If

    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "Up" Then
            NewsRow.ChangeChangeArrange(DB, e.CommandArgument, F_CatId.SelectedValue, False)
            Response.Redirect("Default.aspx?F_CatId=" & F_CatId.SelectedValue)
        ElseIf e.CommandName = "Down" Then
            NewsRow.ChangeChangeArrange(DB, e.CommandArgument, F_CatId.SelectedValue, True)
            Response.Redirect("Default.aspx?F_CatId=" & F_CatId.SelectedValue)
        ElseIf e.CommandName = "Edit" Then
            NewsId = e.CommandArgument
            Response.Redirect("Default.aspx?NewsId=" & NewsId & "&Type=2")
        ElseIf e.CommandName = "Delete" Then
            Dim logDetail As New AdminLogDetailRow
            Dim news As NewsRow = NewsRow.GetRow(DB, e.CommandArgument)
            news.ListCategoryId = NewsRow.GetListCategoryIdByNewsId(DB, news.NewsId)
            logDetail.ObjectId = news.NewsId
            logDetail.Message = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(news, Utility.Common.ObjectType.News)

            NewsRow.Delete(DB, e.CommandArgument)

            Dim ThumbImagePath As String = Server.MapPath("~" & Utility.ConfigData.PathThumbNewsImage)
            Utility.File.DeleteFile(ThumbImagePath & news.ThumbImage)

            logDetail.Action = Utility.Common.AdminLogAction.Delete.ToString()
            logDetail.ObjectType = Utility.Common.ObjectType.News.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)

            Response.Redirect("Default.aspx?F_CatId=" & F_CatId.SelectedValue)
        ElseIf e.CommandName = "Active" Then
            Dim news As NewsRow = NewsRow.GetRow(DB, e.CommandArgument)
            NewsRow.ChangeIsActive(DB, e.CommandArgument)

            Dim logDetail As New AdminLogDetailRow
            logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            logDetail.ObjectId = news.NewsId
            logDetail.ObjectType = Utility.Common.ObjectType.News.ToString()
            logDetail.Message = "IsActive|" & news.IsActive.ToString() & "|" & (Not news.IsActive).ToString() & "[br]"
            AdminLogHelper.WriteLuceneLogDetail(logDetail)

            Response.Redirect("Default.aspx?F_CatId=" & F_CatId.SelectedValue)
        End If
        'Response.Redirect("Default.aspx")
    End Sub
End Class
