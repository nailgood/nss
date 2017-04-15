Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Partial Class admin_store_Album_MusicSearch
    Inherits AdminPage
    Dim AlbumSelect As String = String.Empty
    Dim SongSelect As String = String.Empty
    Public Property Type() As String

        Get
            Dim o As Object = ViewState("Type")
            If o IsNot Nothing Then
                Return DirectCast(o, String)
            End If
            Return String.Empty
        End Get

        Set(ByVal value As String)
            ViewState("Type") = value
        End Set
    End Property
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        AlbumSelect = ";" + Request("Album")
        SongSelect = ";" + Request("Song")
        Type = Request("Type")
        If Type = "1" Then
            gvListAlbum.BindList = AddressOf BindListAlbum
            hidMusicSelect.Value = AlbumSelect
        Else
            gvListSong.BindList = AddressOf BindListSong
            hidMusicSelect.Value = SongSelect
        End If
        If Not IsPostBack Then
            F_Name.Text = Request("F_Name")
            If Type = "1" Then
                gvListAlbum.SortBy = Core.ProtectParam(Request("F_SortBy"))
                gvListAlbum.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
                If gvListAlbum.SortBy = String.Empty Then gvListAlbum.SortBy = "AlbumId"
                BindListAlbum()
            Else
                gvListSong.SortBy = Core.ProtectParam(Request("F_SortBy"))
                gvListSong.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
                If gvListSong.SortBy = String.Empty Then gvListSong.SortBy = "SongId"
                BindListSong()
            End If
        End If
    End Sub
    Private Sub BindListAlbum()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvListAlbum.SortBy
        ViewState("F_SortOrder") = gvListAlbum.SortOrder

        SQLFields = "SELECT TOP " & (gvListAlbum.PageIndex + 1) * gvListAlbum.PageSize & " * "
        SQL = " FROM Album a "

        If Not F_Name.Text = String.Empty Then
            SQL = SQL & Conn & "a.Name like '%" & Trim(F_Name.Text.ToString) & "%'"
            Conn = " AND "
        End If
        gvListAlbum.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)

        Dim res As DataSet = DB.GetDataSet(SQLFields & SQL & " ORDER BY " & gvListAlbum.SortByAndOrder)
        gvListAlbum.DataSource = res.Tables(0).DefaultView
        gvListAlbum.DataBind()
    End Sub
    Private Sub BindListSong()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvListSong.SortBy
        ViewState("F_SortOrder") = gvListSong.SortOrder

        SQLFields = "SELECT TOP " & (gvListSong.PageIndex + 1) * gvListSong.PageSize & " * "
        SQL = " FROM Song s "

        If Not F_Name.Text = String.Empty Then
            SQL = SQL & Conn & "s.Name like '%" & Trim(F_Name.Text.ToString) & "%'"
            Conn = " AND "
        End If
        gvListSong.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)

        Dim res As DataSet = DB.GetDataSet(SQLFields & SQL & " ORDER BY " & gvListSong.SortByAndOrder)
        gvListSong.DataSource = res.Tables(0).DefaultView
        gvListSong.DataBind()
    End Sub


    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Dim url As String = DirectCast(DirectCast(Request, System.Web.HttpRequest).Url, System.Uri).AbsoluteUri
        Response.Redirect(url)
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub
        If Type = "1" Then
            gvListAlbum.PageIndex = 0
            BindListAlbum()
        Else
            gvListSong.PageIndex = 0
            BindListSong()
        End If
       
    End Sub


    Private Sub gvListAlbum_ItemDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvListAlbum.RowDataBound
        If Not (e.Row.RowType = DataControlRowType.DataRow) Then
            Exit Sub
        End If

        Dim ALbumId As String = e.Row.DataItem("AlbumId")


        If Type = "1" Then ''wrire checkbox for multi Selected
            Dim chk As CheckBox = CType(e.Row.FindControl("chk_Album"), CheckBox)
            chk.Attributes.Add("onclick", "CheckItem('" + ALbumId + "',this.checked);")
            If AlbumSelect.Contains(";" + ALbumId + ";") Then
                If Not chk Is Nothing Then
                    chk.Checked = True
                    'chk.Enabled = False
                End If
            End If
        End If

    End Sub
    Private Sub gvListSong_ItemDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvListSong.RowDataBound
        If Not (e.Row.RowType = DataControlRowType.DataRow) Then
            Exit Sub
        End If

        Dim SongId As String = e.Row.DataItem("SongId")


        If Type = "2" Then ''wrire checkbox for multi Selected
            Dim chk As CheckBox = CType(e.Row.FindControl("chk_Song"), CheckBox)
            chk.Attributes.Add("onclick", "CheckItem('" + SongId + "',this.checked);")
            If SongSelect.Contains(";" + SongId + ";") Then
                If Not chk Is Nothing Then
                    chk.Checked = True
                    'chk.Enabled = False
                End If
            End If
        End If

    End Sub
 
    'Private Sub getCheckSelect(ByVal id As String)
    '    If Type = "1" Then
    '        If AlbumSelect <> "" Then
    '            hidMusicSelect.Value = AlbumSelect
    '        Else

    '        End If
    '    ElseIf Type = "2" Then
    '        If SongSelect <> "" Then
    '            hidMusicSelect.Value = SongSelect
    '        Else

    '        End If

    '    End If
    'End Sub

End Class

