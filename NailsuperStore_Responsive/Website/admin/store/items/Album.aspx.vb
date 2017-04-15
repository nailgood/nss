Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Partial Class admin_store_items_Album
    Inherits AdminPage
    Dim ToTal As Integer
    Protected ItemId As Integer = 0
    Dim arr As Array
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        gvList.BindList = AddressOf BindList
        ItemId = IIf(Request.QueryString("ItemId") <> Nothing, CInt(Request.QueryString("ItemId")), 0)
        If Not IsPostBack Then
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "abi.Arrange"
            If gvList.SortOrder = String.Empty Then gvList.SortBy = "ASC"
            BindList()
        End If

    End Sub

    Private Sub BindList()
        hidPopUpAlbum.Value = String.Empty
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        '' ViewState("F_SortBy") = gvList.SortBy
        ''ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT  ab.AlbumId, Name, IsActive"
        SQL = " FROM Album ab inner join AlbumItem abi on ab.AlbumId = abi.AlbumId "
        SQL &= " where abi.ItemId = " & ItemId

        Conn = " AND "
        'ToTal = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        'gvList.Pager.NofRecords = ToTal

        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        If Not res Is Nothing Then
            ToTal = res.Rows.Count
        End If
        gvList.DataSource = res.DefaultView
        gvList.DataBind()


       
    End Sub

    Private Sub btnAddAlbum_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddAlbum.Click
        If hidPopUpAlbum.Value <> "" And hidPopUpAlbum.Value <> "undefined" Then
            arr = Split(hidPopUpAlbum.Value.Trim(), ";")
            ''If arr(0) <> "thisForm" Then
            Dim alb As New AlbumRow
            alb.RemoveAlbumItem_Item(ItemId)
            For i As Integer = 0 To arr.Length - 1
                If (arr(i).ToString() <> "") And ItemId > 0 Then
                    'alb = AlbumRow.GetRow(DB, arr(i).ToString())
                    'Insert to AlbumItem

                    alb.InsertAlbumItem(CInt(arr(i)), ItemId)
                    '
                End If
            Next

            ''End If
            BindList()
        End If
    End Sub
    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
           ' Dim active As Boolean = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("IsActive")
            Dim imbUp As ImageButton = CType(e.Row.FindControl("imbUp"), ImageButton)
            Dim imbDown As ImageButton = CType(e.Row.FindControl("imbDown"), ImageButton)
            ' Dim imbActive As ImageButton = CType(e.Row.FindControl("imbActive"), ImageButton)
            Dim imbDelete As ImageButton = CType(e.Row.FindControl("imbDelete"), ImageButton)
            'imbUp.CommandArgument = AlbumId.ToString
            'imbDown.CommandArgument = AlbumId.ToString()
            'imbActive.CommandArgument = AlbumId.ToString()
            'imbDelete.CommandArgument = AlbumId.ToString()


            'If active Then
            '    imbActive.ImageUrl = "/images/admin/active.png"
            'Else
            '    imbActive.ImageUrl = "/images/admin/inactive.png"
            'End If
            ''Dim data As SongRow = DirectCast(e.Row.DataItem, SongRow)

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
            hidPopUpAlbum.Value &= e.Row.DataItem("AlbumId") & ";"

        End If

    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "Up" Then
            AlbumRow.ChangeChangeArrange(DB, ItemId, e.CommandArgument, True)
        ElseIf e.CommandName = "Down" Then
            AlbumRow.ChangeChangeArrange(DB, ItemId, e.CommandArgument, False)
        ElseIf e.CommandName = "Delete" Then
            AlbumRow.Remove_AlbumItem(DB, e.CommandArgument, ItemId)
            'ElseIf e.CommandName = "Active" Then
            '    AlbumRow.ChangeIsActive(DB, e.CommandArgument)
        End If
        Response.Redirect("Album.aspx?ItemId=" & ItemId & "&" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("edit.aspx?ItemId=" & ItemId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class
