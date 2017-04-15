Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Partial Class admin_NewsEvent_Blog_default
    Inherits AdminPage

    Private TotalRecords As Integer
    Protected NewsId As Integer
    Private Type As Integer = 1
    Dim ToTal As Integer
    Dim CategoryId As Integer = Utility.ConfigData.BlogId()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("Type") <> Nothing AndAlso Request.QueryString("Type").Length > 0 Then
            Type = CInt(Request.QueryString("Type"))
        End If

        If Request.QueryString("NewsId") <> Nothing AndAlso IsNumeric(Request.QueryString("NewsId")) Then
            NewsId = Request("NewsId")
        End If
        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
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

        fuImage.Folder = Utility.ConfigData.PathBlogImage
        fuImage.CurrentFileName = News.ThumbImage
        hpimg.ImageUrl = Utility.ConfigData.PathThumbBlogImage & News.ThumbImage
        If fuImage.CurrentFileName <> Nothing Then
            divImg.Visible = True
        End If

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


        SQL = " FROM News n left join NewsCategory nc on n.NewsId = nc.NewsId  "

        If Not F_Title.Text = String.Empty Then
            SQL = SQL & Conn & " n.title LIKE " & DB.FilterQuote(F_Title.Text)
            Conn = " AND "
        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL = SQL & Conn & " n.isactive  = " & DB.Number(F_IsActive.SelectedValue)
            Conn = " AND "
        End If

        SQL = SQL & Conn & " nc.CategoryId  = " & Utility.ConfigData.BlogId()
        Conn = " AND "
        hidCon.Value = SQL
    End Sub
    Private Sub BindList()
        Dim SQLFields As String = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " n.NewsId,Title,IsActive, IsFacebook"
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

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Not IsValid Then Exit Sub
        Dim News As New NewsRow(DB)
        Dim NewsBefore As New NewsRow
        Dim logDetail As New AdminLogDetailRow

        Dim oldImage As String = String.Empty
        If NewsId > 0 Then
            News = NewsRow.GetRow(DB, NewsId)
            oldImage = News.ThumbImage
            NewsBefore = CloneObject.Clone(News)
        End If
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
            fuImage.Folder = "~" & Utility.ConfigData.PathBlogImage
            Dim newImageName As String = Guid.NewGuid().ToString() & fuImage.OriginalExtension
            Dim ImagePath As String = Server.MapPath("~" & Utility.ConfigData.PathBlogImage)
            Dim ThumbImagePath As String = Server.MapPath("~" & Utility.ConfigData.PathThumbBlogImage)

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

                logDetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.Blog, NewsBefore, News)
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            Else
                NewsId = News.Insert()
                If fuImage.NewFileName <> String.Empty Then
                    Core.CropByAnchor(ImagePath & fuImage.NewFileName, ThumbImagePath & NewsId & fuImage.OriginalExtension, 770, 433, Utility.Common.ImageAnchorPosition.Center)
                    News.ThumbImage = NewsId & fuImage.OriginalExtension
                    News.NewsId = NewsId
                    News.Update()
                End If

                News.InsertNewsCategory(DB, Utility.ConfigData.BlogId, NewsId)
                logDetail.Message = AdminLogHelper.ConvertObjectInsertToLogMesssageString(News, Utility.Common.ObjectType.Blog)
                logDetail.Action = Utility.Common.AdminLogAction.Insert.ToString()
            End If
            Utility.File.DeleteFile(ImagePath & fuImage.NewFileName)
            If NewsId > 0 Then
                DB.CommitTransaction()
                logDetail.ObjectId = NewsId
                logDetail.ObjectType = Utility.Common.ObjectType.Blog.ToString()
                AdminLogHelper.WriteLuceneLogDetail(logDetail)
            Else
                DB.RollbackTransaction()
            End If
            Response.Redirect("Default.aspx")
        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "InitEditor", "InitEditor()", True)
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx")
    End Sub

    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        pnList.Visible = False
        pnNew.Visible = True

        ltrHeader.Text = "Add Blog"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "InitEditor", "InitEditor()", True)
        'ddlCategory.SelectedValue = F_CatId.SelectedValue
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim NewsId As Integer = e.Row.DataItem("NewsId")
            Dim active As Boolean = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("IsActive")

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

            Dim facebook As Boolean = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("IsFacebook")
            Dim imbFacebook As ImageButton = CType(e.Row.FindControl("imbFacebook"), ImageButton)
            If facebook Then
                imbFacebook.ImageUrl = "/includes/theme-admin/images/active.png"
                imbFacebook.Enabled = False
            Else
                imbFacebook.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If
        End If

    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "Up" Then
            NewsRow.ChangeChangeArrange(DB, e.CommandArgument, CategoryId, False)
        ElseIf e.CommandName = "Down" Then
            NewsRow.ChangeChangeArrange(DB, e.CommandArgument, CategoryId, True)
        ElseIf e.CommandName = "Edit" Then
            NewsId = e.CommandArgument
            Response.Redirect("Default.aspx?NewsId=" & NewsId & "&Type=2")
        ElseIf e.CommandName = "Delete" Then
            Response.Redirect("delete.aspx?NewsId=" & e.CommandArgument & "&" & GetPageParams(FilterFieldType.All))
        ElseIf e.CommandName = "Active" Then
            Dim news As NewsRow = NewsRow.GetRow(DB, e.CommandArgument)
            NewsRow.ChangeIsActive(DB, e.CommandArgument)

            Dim logDetail As New AdminLogDetailRow
            logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            logDetail.ObjectId = news.NewsId
            logDetail.ObjectType = Utility.Common.ObjectType.Blog.ToString()
            logDetail.Message = "IsActive|" & news.IsActive.ToString() & "|" & (Not news.IsActive).ToString() & "[br]"
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
        ElseIf e.CommandName = "Facebook" Then
            GetBlogToFaceBook(e.CommandArgument)
        End If
        BindList()
    End Sub
    Private Sub GetBlogToFaceBook(ByVal NewsId As Integer)
        If NewsId > 0 Then
            Dim news As NewsRow = NewsRow.GetRow(DB, NewsId)
            If Not news.IsFacebook Then
                Dim result As Integer = SocialHelper.PostBlogToFB(DB, news)
                If (result <> 0) Then
                    Dim logDetail As New AdminLogDetailRow
                    news.IsFacebook = True
                    news.Update()

                    logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                    logDetail.ObjectId = news.NewsId
                    logDetail.ObjectType = Utility.Common.ObjectType.Blog.ToString()
                    logDetail.Message = "IsFacebook|False|True[br]"
                    AdminLogHelper.WriteLuceneLogDetail(logDetail)
                End If
            End If
        End If
    End Sub

End Class
