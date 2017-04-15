Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Partial Class admin_store_Album_default
    Inherits AdminPage
    Dim ToTal As Integer
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            F_Name.Text = Request("F_Name")
            F_IsActive.Text = Request("F_IsActive")
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "Arrange"
            If gvList.SortOrder = String.Empty Then gvList.SortBy = "ASC"
            BindList()
        End If
    End Sub
   
    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " AlbumId, Name, IsActive"
        SQL = " FROM Album ab "

        If Not F_Name.Text = String.Empty Then
            SQL = SQL & Conn & "ab.Name LIKE " & DB.FilterQuote(F_Name.Text)
            Conn = " AND "
        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "ab.IsActive  = " & DB.Number(F_IsActive.SelectedValue)
            Conn = " AND "
        End If

        Conn = " AND "
        ToTal = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        gvList.Pager.NofRecords = ToTal

        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub

    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindList()
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim active As Boolean = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("IsActive")

            Dim imbActive As ImageButton = CType(e.Row.FindControl("imbActive"), ImageButton)

            If active Then
                imbActive.ImageUrl = "/images/admin/active.png"
            Else
                imbActive.ImageUrl = "/images/admin/inactive.png"
            End If
            ''Dim data As AlbumRow = DirectCast(e.Row.DataItem, AlbumRow)
            Dim imbUp As ImageButton = CType(e.Row.FindControl("imbUp"), ImageButton)
            Dim imbDown As ImageButton = CType(e.Row.FindControl("imbDown"), ImageButton)

            'If ToTal < 2 Then
            '    imbUp.Visible = False
            '    imbDown.Visible = False
            'Else
            '    If e.Row.RowIndex = 0 Then
            '        imbUp.Visible = False
            '    ElseIf e.Row.RowIndex = ToTal - 1 Then
            '        imbDown.Visible = False
            '    End If
            'End If
            Dim AlbumId As Integer = e.Row.DataItem("AlbumId")
            Dim ltrRelated As Literal = CType(e.Row.FindControl("ltrRelated"), Literal)
            '' ltrRelated.Text = AlbumRow.CountRelated(DB, AlbumId)
            'ltrRelated.Text = String.Format("<a href='addSong.aspx?Albumid={0}&{1}'>{2}</a>", AlbumId, GetPageParams(FilterFieldType.All), AlbumRow.CountRelated(DB, AlbumId))
        End If

    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        'If e.CommandName = "Up" Then
        '    AlbumRow.ChangeChangeArrange(DB, e.CommandArgument, True)
        'ElseIf e.CommandName = "Down" Then
        '    AlbumRow.ChangeChangeArrange(DB, e.CommandArgument, False)
        'Elseif
        If e.CommandName = "Delete" Then
            Dim objAlbum As AlbumRow = AlbumRow.GetRow(DB, e.CommandArgument)
            If AlbumRow.Delete(DB, e.CommandArgument) Then
                Dim ImagePath As String = Server.MapPath("~/" & Utility.ConfigData.AlbumThumbPath)

                ''Delete Old File
                Utility.File.DeleteFile(ImagePath & objAlbum.ThumImg)

            End If

        ElseIf e.CommandName = "Active" Then
            AlbumRow.ChangeIsActive(DB, e.CommandArgument)
        End If
        Response.Redirect("Default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
End Class
