


Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.Configuration.ConfigurationManager

Public Class admin_Video_Video_Default
    Inherits AdminPage
    Dim ToTal As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            LoadCategory()
            F_Name.Text = Request("F_Name")
            F_IsActive.Text = Request("F_IsActive")
            F_Category.Text = Request("F_Category")
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "vc.Arrange"
            If gvList.SortOrder = String.Empty Then gvList.SortBy = "ASC"
            btnSearch_Click(sender, e)
            'BindList()
        End If
    End Sub
    Private Sub LoadCategory()
        Dim collection As CategoryCollection = CategoryRow.ListByType(DB, Utility.Common.CategoryType.Video)
        F_Category.DataSource = collection
        F_Category.DataTextField = "CategoryName"
        F_Category.DataValueField = "CategoryId"
        F_Category.DataBind()
        F_Category.Items.Insert(0, New ListItem("-- ALL --", String.Empty))
    End Sub
    Private Sub BindQuery()
        Dim SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQL = " FROM Video v right join VideoCategory vc on v.VideoId = vc.VideoId "

        If Not F_Name.Text = String.Empty Then
            SQL = SQL & Conn & "v.Title LIKE " & DB.FilterQuote(F_Name.Text)
            Conn = " AND "
        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "v.IsActive  = " & F_IsActive.SelectedValue
            Conn = " AND "
        End If
        If Not String.IsNullOrEmpty(F_Category.SelectedValue) Then
            SQL = SQL & Conn & "vc.CategoryId  = " & F_Category.SelectedValue
            Conn = " AND "
        End If
        hidCon.Value = SQL
    End Sub
    Private Sub BindList()
        Dim SQLFields As String = "SELECT DISTINCT* FROM (SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " v.VideoId,Title,IsActive"
        ToTal = DB.ExecuteScalar("SELECT COUNT(DISTINCT v.VideoId) " & hidCon.Value)
        gvList.Pager.NofRecords = ToTal

        Dim res As DataTable = DB.GetDataTable(SQLFields & hidCon.Value & " ORDER BY " & gvList.SortBy & " DESC )tbltmp")
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub

    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindQuery()
        BindList()
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim active As Boolean = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("IsActive")

            Dim imbActive As ImageButton = CType(e.Row.FindControl("imbActive"), ImageButton)

            If active Then
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If
            ''Dim data As VideoRow = DirectCast(e.Row.DataItem, VideoRow)
            Dim imbUp As ImageButton = CType(e.Row.FindControl("imbUp"), ImageButton)
            Dim imbDown As ImageButton = CType(e.Row.FindControl("imbDown"), ImageButton)

            If ToTal < 2 Then
                imbUp.Visible = False
                imbDown.Visible = False
            Else
                If e.Row.RowIndex = 0 Then
                    imbUp.Visible = False
                ElseIf e.Row.RowIndex = ToTal - 1 Then
                    imbDown.Visible = False
                End If
            End If
            Dim VideoId As Integer = e.Row.DataItem("VideoId")
            Dim ltrRelated As Literal = CType(e.Row.FindControl("ltrRelated"), Literal)
            '' ltrRelated.Text = VideoRow.CountRelated(DB, VideoId)
            ltrRelated.Text = String.Format("<a href='VideoRelated.aspx?Videoid={0}&{1}'>{2}</a>", VideoId, GetPageParams(FilterFieldType.All), VideoRow.CountRelated(DB, VideoId))
            Dim ltrRelatedItem As Literal = CType(e.Row.FindControl("ltrRelatedItem"), Literal)
            ltrRelatedItem.Text = String.Format("<a href='ItemRelatedVideo.aspx?Videoid={0}&{1}'>{2}</a>", VideoId, GetPageParams(FilterFieldType.All), VideoRow.CountItemRelated(DB, VideoId))
            Dim ltrLikeFB As Literal = e.Row.FindControl("ltrLikeFB")
            Dim likeFB As New NailCache()
            ltrLikeFB.Text = likeFB.BindCountLikeFB(ConfigurationManager.AppSettings("GlobalSecureName") & URLParameters.VideoDetailUrl(e.Row.DataItem("Title"), e.Row.DataItem("VideoId")))
        End If

    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "Up" Then
            VideoRow.ChangeChangeArrange(DB, e.CommandArgument, True)
        ElseIf e.CommandName = "Down" Then
            VideoRow.ChangeChangeArrange(DB, e.CommandArgument, False)
        ElseIf e.CommandName = "Delete" Then
            Dim objVideo As VideoRow = VideoRow.GetRow(DB, e.CommandArgument)
            If VideoRow.Delete(DB, e.CommandArgument) Then
                Dim ThumbImagePath As String = Server.MapPath("~" & Utility.ConfigData.VideoThumbPath)
                Dim RelatedThumbPath As String = Server.MapPath("~" & Utility.ConfigData.VideoRelatedThumbPath)
                Dim LargeImagePath As String = Server.MapPath("~" & Utility.ConfigData.VideoLargePath)
                ''Delete Old File
                Utility.File.DeleteFile(RelatedThumbPath & objVideo.ThumbImage)
                Utility.File.DeleteFile(ThumbImagePath & objVideo.ThumbImage)
                Utility.File.DeleteFile(LargeImagePath & objVideo.ThumbImage)
            End If
            Dim logDetail As New AdminLogDetailRow
            logDetail.ObjectType = Utility.Common.ObjectType.Video.ToString()
            logDetail.Action = Utility.Common.AdminLogAction.Delete.ToString()
            logDetail.ObjectId = objVideo.VideoId
            logDetail.Message = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(objVideo, Utility.Common.ObjectType.Video)
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
        ElseIf e.CommandName = "Active" Then
            Dim objVideo As VideoRow = VideoRow.GetRow(DB, e.CommandArgument)
            VideoRow.ChangeIsActive(DB, objVideo.VideoId)

            Dim logDetail As New AdminLogDetailRow
            logDetail.ObjectType = Utility.Common.ObjectType.Video.ToString()
            logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            logDetail.ObjectId = objVideo.VideoId
            logDetail.Message = "IsActive|" & objVideo.IsActive.ToString() & "|" & (Not objVideo.IsActive).ToString() & "[br]"
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
        End If
        Response.Redirect("Default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
End Class



