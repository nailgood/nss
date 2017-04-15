Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.IO
Partial Class admin_members_nail_art_trends_default
    Inherits AdminPage
    Dim ToTal As Integer
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            Dim fukeName As String = ""
            Try
                fukeName = Request("f")
            Catch ex As Exception
                fukeName = ""
            End Try
            If fukeName <> "" Then
                DownloadExport(fukeName)
            End If
            F_Type.Text = Request("F_Type")
            F_Status.Text = Request("F_Status")
            F_SubmitedDateLbound.Text = Request("F_SubmitedDateLbound")
            F_SubmitedDateUbound.Text = Request("F_SubmitedDateUbound")
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then
                gvList.SortBy = "SubmittedDate"
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

        ' SQL = " FROM MemberSubmission ms inner join MemberSubmissionFile msf on ms.Submissionid = msf.SubmissionId   "
        SQL = " FROM MemberSubmission ms"
        If Not F_Type.Text = String.Empty Then
            SQL = SQL & Conn & " Type = " & DB.Number(F_Type.Text)
            Conn = " AND "
        End If
        If Not F_Status.Text = String.Empty Then
            SQL = SQL & Conn & " Status = " & DB.Number(F_Status.Text)
            Conn = " AND "
        End If

        If Not F_SubmitedDateLbound.Text = String.Empty Then
            SQL = SQL & Conn & " SubmittedDate >= " & DB.Quote(F_SubmitedDateLbound.Text)
            Conn = " AND "
        End If
        If Not F_SubmitedDateUbound.Text = String.Empty Then
            SQL = SQL & Conn & " SubmittedDate < " & DB.Quote(DateAdd("d", 1, F_SubmitedDateUbound.Text))
            Conn = " AND "
        End If
        hidCon.Value = SQL
    End Sub
    Private Sub BindList()
        Dim SQLFields As String = "SELECT DISTINCT * FROM (SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " ms.SubmissionId, MemberId,Name, Email,ArtName,Status,SubmittedDate,(stuff((Select ';' + FileName FROM MemberSubmissionFile WHERE SubmissionId = ms.SubmissionId  FOR XML PATH('')), 1, 1, '')) as filename"
        ToTal = DB.ExecuteScalar("SELECT COUNT(DISTINCT ms.SubmissionId) " & hidCon.Value)
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
            Dim Status As Boolean = CBool(e.Row.DataItem("Status"))
            Dim ltrMemberId As Literal = CType(e.Row.FindControl("ltrMemberId"), Literal)
            Dim ltrFile As Literal = CType(e.Row.FindControl("ltrFile"), Literal)
            Dim arrFile As String() = e.Row.DataItem("FileName").ToString.Split(";")
            Dim memberId As Integer = 0
            Try
                memberId = CInt(e.Row.DataItem("MemberId"))
            Catch ex As Exception
                memberId = 0
            End Try
            Dim imbActive As ImageButton = CType(e.Row.FindControl("imbStatus"), ImageButton)

            If Status Then
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If
            If memberId = 0 Then
                ltrMemberId.Text = e.Row.DataItem("Name")
            Else
                ltrMemberId.Text = "<a href='../edit.aspx?MemberId=" & memberId & "'>" & e.Row.DataItem("Name") & "</a>"
            End If
            For i As Integer = 0 To arrFile.Length - 1

                ltrFile.Text &= "<a href=""default.aspx?f=" & arrFile(i) & """>" & arrFile(i) & "</a><br>"
            Next
        End If
    End Sub
    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "Delete" Then
            Dim ms As MemberSubmissionRow = MemberSubmissionRow.GetRow(DB, e.CommandArgument)
            If MemberSubmissionRow.Delete(DB, e.CommandArgument) Then
                Try
                    Dim arrFile As String() = ms.FileName.Split(";")

                    Dim ImgPath As String = Server.MapPath("~/" & Utility.ConfigData.PathUploadArtTrend)
                    ''Delete Old File
                    For i As Integer = 0 To arrFile.Length - 1
                        Utility.File.DeleteFile(ImgPath & arrFile(i))
                    Next
                    Try
                        Dim arrAdminFile As String() = ms.AdminUploadFile.Split(";")
                        For j As Integer = 0 To arrAdminFile.Length - 1
                            Utility.File.DeleteFile(ImgPath & "admin\" & arrAdminFile(j))
                        Next
                    Catch ex As Exception

                    End Try
                Catch ex As Exception

                End Try
            End If
            Dim logDetail As New AdminLogDetailRow
            logDetail.ObjectType = Utility.Common.ObjectType.Video.ToString()
            logDetail.Action = Utility.Common.AdminLogAction.Delete.ToString()
            logDetail.ObjectId = ms.SubmissionId
            logDetail.Message = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(ms, Utility.Common.ObjectType.NailArtTrends)
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
        ElseIf e.CommandName = "Active" Then
            Dim ms As MemberSubmissionRow = MemberSubmissionRow.GetRow(DB, e.CommandArgument)
            MemberSubmissionRow.ChangeIsActive(DB, e.CommandArgument)

            Dim logDetail As New AdminLogDetailRow
            logDetail.ObjectType = Utility.Common.ObjectType.Video.ToString()
            logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            logDetail.ObjectId = ms.SubmissionId
            logDetail.Message = "Status|" & ms.Status.ToString() & "|" & (Not ms.Status).ToString() & "[br]"
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
        End If
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
    Private Sub DownloadExport(ByVal sFileName As String)
        Dim context As HttpContext = HttpContext.Current
        context.Response.Buffer = True
        context.Response.Clear()
        context.Response.AddHeader("content-disposition", "attachment; filename=" + sFileName)
        context.Response.ContentType = "application/octet-stream"
        context.Response.WriteFile(Server.MapPath(Utility.ConfigData.PathArtTrends + sFileName))
        context.Response.Flush()
        context.Response.Close()
    End Sub
End Class
