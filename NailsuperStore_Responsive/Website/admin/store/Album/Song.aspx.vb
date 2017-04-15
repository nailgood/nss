
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Partial Class admin_store_Album_Song
    Inherits AdminPage
    Dim ToTal As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            LoadAlbum()
            F_Name.Text = Request("F_Name")
            F_IsActive.Text = Request("F_IsActive")
            F_Album.Text = Request("F_Album")
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "Name"
            If gvList.SortOrder = String.Empty Then gvList.SortBy = "ASC"
            BindList()
        End If
    End Sub
    Private Sub LoadAlbum()
        Dim collection As AlbumCollection = AlbumRow.LoadAll()
        F_Album.DataSource = collection
        F_Album.DataTextField = "Name"
        F_Album.DataValueField = "AlbumId"
        F_Album.DataBind()
        F_Album.Items.Insert(0, New ListItem("-- ALL --", 0))
    End Sub
    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " s.SongId,Name,IsActive"
        SQL = " FROM Song s " 'inner join albumsong als on s.SongId = als.SongId "
        If F_Album.SelectedValue > 0 Then
            SQL = SQL & "inner join albumsong als on s.SongId = als.SongId "
        End If
        If Not F_Name.Text = String.Empty Then
            SQL = SQL & Conn & "s.Name LIKE " & DB.FilterQuote(F_Name.Text)
            Conn = " AND "
        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "s.IsActive  = " & DB.Number(F_IsActive.SelectedValue)
            Conn = " AND "
        End If
        If F_Album.SelectedValue > 0 Then
            SQL = SQL & Conn & "als.AlbumId  = " & DB.Number(F_Album.SelectedValue)
        End If

        Conn = " AND "
        ToTal = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        gvList.Pager.NofRecords = ToTal

        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub

    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("editSong.aspx?" & GetPageParams(FilterFieldType.All))
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
            ''Dim data As SongRow = DirectCast(e.Row.DataItem, SongRow)
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
            Dim SongId As Integer = e.Row.DataItem("SongId")

        End If

    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        'If e.CommandName = "Up" Then
        '    SongRow.ChangeChangeArrange(DB, e.CommandArgument, True)
        'ElseIf e.CommandName = "Down" Then
        '    SongRow.ChangeChangeArrange(DB, e.CommandArgument, False)
        'Else
        If e.CommandName = "Delete" Then
            Dim objSong As SongRow = SongRow.GetRow(DB, e.CommandArgument)
            If SongRow.Delete(DB, e.CommandArgument) Then
                Dim ImagePath As String = Server.MapPath("~/" & Utility.ConfigData.SongPath)
                ''Delete Old File
                Utility.File.DeleteFile(ImagePath & objSong.FileSong)

            End If

        ElseIf e.CommandName = "Active" Then
            SongRow.ChangeIsActive(DB, e.CommandArgument)
        End If
        Response.Redirect("song.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
End Class
