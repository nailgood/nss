Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.IO
Partial Class admin_NewsEvent_NewsFeed_default
    Inherits AdminPage
    Dim ToTal As Integer
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            F_txtTitle.Text = Request("F_txtTitle")
            F_IsActive.Text = Request("F_IsActive")
            F_CreatedDateLbound.Text = Request("F_CreatedDateLbound")
            F_CreatedDateUbound.Text = Request("F_CreatedDateUbound")
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then
                gvList.SortBy = "CreatedDate"
                gvList.SortOrder = "desc"
            End If
            'BindList()
            btnSearch_Click(sender, e)
        End If
    End Sub
    Private Sub BindQuery()
        Dim SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        ' SQL = " FROM NewsFeed ms inner join NewsFeedFile msf on ms.Submissionid = msf.SubmissionId   "
        SQL = " FROM NewsFeed"
        If Not F_txtTitle.Text = String.Empty Then
            SQL = SQL & Conn & " Title like '%" & F_txtTitle.Text & "%'"
            Conn = " AND "
        End If
        If Not F_IsActive.Text = String.Empty Then
            SQL = SQL & Conn & " IsActive = " & DB.Number(F_IsActive.Text)
            Conn = " AND "
        End If

        If Not F_CreatedDateLbound.Text = String.Empty Then
            SQL = SQL & Conn & " CreatedDate >= " & DB.Quote(F_CreatedDateLbound.Text)
            Conn = " AND "
        End If
        If Not F_CreatedDateUbound.Text = String.Empty Then
            SQL = SQL & Conn & " CreatedDate < " & DB.Quote(DateAdd("d", 1, F_CreatedDateUbound.Text))
            Conn = " AND "
        End If
        hidCon.Value = SQL
    End Sub
    Private Sub BindList()
        Dim SQLFields As String = "SELECT DISTINCT * FROM (SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " NewsFeedId, Title, Url, Image, IsActive, CreatedDate "
        ToTal = DB.ExecuteScalar("SELECT COUNT(DISTINCT NewsFeedId) " & hidCon.Value)
        gvList.Pager.NofRecords = ToTal

        Dim res As DataTable = DB.GetDataTable(SQLFields & hidCon.Value & " ORDER BY " & gvList.SortByAndOrder & ")tbltmp")
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'If Not IsValid Then Exit Sub
        gvList.PageIndex = 0
        BindQuery()
        BindList()
    End Sub
    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim IsActive As Boolean = CBool(e.Row.DataItem("IsActive"))

            Dim imbActive As ImageButton = CType(e.Row.FindControl("imbStatus"), ImageButton)

            If IsActive Then
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If

        End If
    End Sub
    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "Delete" Then
            Dim nf As NewsFeedRow = NewsFeedRow.GetRow(DB, e.CommandArgument)
            If NewsFeedRow.Delete(DB, e.CommandArgument) Then
                Try

                    Dim ImagePath As String = Server.MapPath("~/" & Utility.ConfigData.NewsFeedImg)
                    ''Delete Old File
                    Utility.File.DeleteFile(ImagePath & nf.Image)

                Catch ex As Exception

                End Try
            End If
            Dim logDetail As New AdminLogDetailRow
            logDetail.ObjectType = Utility.Common.ObjectType.Video.ToString()
            logDetail.Action = Utility.Common.AdminLogAction.Delete.ToString()
            logDetail.ObjectId = nf.NewsFeedId
            logDetail.Message = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(nf, Utility.Common.ObjectType.NailArtTrends)
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
        ElseIf e.CommandName = "Active" Then
            Dim nf As NewsFeedRow = NewsFeedRow.GetRow(DB, e.CommandArgument)
            NewsFeedRow.ChangeIsActive(DB, e.CommandArgument)

            Dim logDetail As New AdminLogDetailRow
            logDetail.ObjectType = Utility.Common.ObjectType.Video.ToString()
            logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            logDetail.ObjectId = nf.NewsFeedId
            logDetail.Message = "IsActive|" & nf.IsActive.ToString() & "|" & (Not nf.IsActive).ToString() & "[br]"
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
        End If
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub


    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
   
End Class
