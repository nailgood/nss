
Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports System.Net
Partial Class admin_store_Album_edit
    Inherits AdminPage
    Protected Id As Integer
    ' Protected ItemId As Integer
    Protected si As StoreItemRow
    Dim arr As Array
    Dim ToTal As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Id = Convert.ToInt32(Request("Id"))
        If Id <> Nothing Then
            'Button1.Disabled = False
            sp1.Visible = True
            If hidPopUpSong.Value = "" Or hidPopUpSong.Value = "undefined" Then
                Dim dv As DataView = DB.GetDataView("Select SongId from AlbumSong where AlbumId = " & Id)
                ltSong.Text = ""
                If dv.Count > 0 Then
                    Dim s As SongRow
                    For i As Integer = 0 To dv.Count - 1
                        s = SongRow.GetRow(DB, dv(i)("SongId"))
                        ltSong.Text &= s.Name & "<br />"
                        hidPopUpSong.Value &= dv(i)("SongId") & ";"
                    Next
                End If
            End If

        End If
        'If Request("ItemId") <> Nothing Then
        '    ItemId = Convert.ToInt32(Request("ItemId"))
        'End If
        If Not IsPostBack Then
            LoadFromDB()
            LoadSong()
        End If
    End Sub
 
   
    Private Sub LoadFromDB()
        If Id <= 0 Then
            Return
        End If
        Dim ab As AlbumRow = AlbumRow.GetRow(DB, Id)
        'If ItemId <> Nothing Then
        '    si = StoreItemRow.GetRow(DB, ItemId)
        '    txtSku.Text = si.SKU
        'Else
        '    txtSku.Text = StoreItemRow.GetRow(DB, ab.ItemId).SKU
        'End If
        txtName.Text = ab.Name
        chkIsActive.Checked = ab.IsActive
        txtDescription.Text = ab.Description
        fuThumb.CurrentFileName = ab.ThumImg
        fuThumb.Folder = "/" & Utility.ConfigData.AlbumThumbPath.Substring(0, Utility.ConfigData.AlbumThumbPath.Length - 1)
        fuThumb.ImageDisplayFolder = "/" & Utility.ConfigData.AlbumThumbPath.Substring(0, Utility.ConfigData.AlbumThumbPath.Length - 1)
        hpimg.ImageUrl = "/" & Utility.ConfigData.AlbumThumbPath & ab.ThumImg
        fuThumb.EnableDelete = False
        txtMetaDescription.Text = ab.MetaDescription
        txtMetaKeyword.Text = ab.MetaKeywords
        txtPageTitle.Text = ab.PageTitle

        divImg.Visible = True
  
     

    End Sub
    Private Sub LoadSong()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " Name,s.SongId"
        SQL = " FROM Song s inner join AlbumSong abs on s.SongId = abs.SongId "
        SQL &= " where abs.AlbumId = " & Id

        Conn = " AND "
        ToTal = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        gvList.Pager.NofRecords = ToTal

        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY abs.Arrange ")
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub
    Dim newImageName As String = ""
    Dim ImagePath As String = ""
    Dim CopyPath As String = ""

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Page.IsValid Then
            Try
                Dim i As Integer
                ''''Check Sku''''
                'If txtSku.Text.Length > 0 Then
                '    si = StoreItemRow.GetRow(DB, CStr(txtSku.Text))
                '    If si.ItemId = Nothing Or si.ItemId = 0 Then
                '        lbMsgSku.Text = "Item Sku not exist"
                '        lbMsgSku.Visible = True
                '        Exit Sub
                '    End If
                'End If
                DB.BeginTransaction()
                Dim oldImage As String = ""
                Dim oldIsYouTubeImage As Boolean = False
                Dim qr As String = "Id="
                Dim ab As AlbumRow
                If Id <> 0 Then
                    ab = AlbumRow.GetRow(DB, Id)
                    oldImage = ab.ThumImg

                Else
                    ab = New AlbumRow(DB)
                End If
                If (fuThumb.NewFileName = String.Empty And oldImage = String.Empty) Then
                    AddError("Field 'Thumb Image' is blank")
                    Exit Sub
                End If
                ab.Name = txtName.Text.Trim.Trim()
                ab.IsActive = chkIsActive.Checked
                ab.MetaKeywords = txtMetaKeyword.Text.Trim()
                ab.Description = txtDescription.Text.Trim()
                ab.MetaDescription = txtMetaDescription.Text.Trim()
                ab.PageTitle = txtPageTitle.Text.Trim()
                ab.CreateDate = DateTime.Now
                fuThumb.Folder = "~/" & Utility.ConfigData.AlbumCopyPath
                newImageName = Guid.NewGuid().ToString()
                ImagePath = Server.MapPath("~/" & Utility.ConfigData.AlbumThumbPath)
                CopyPath = Server.MapPath("~/" & Utility.ConfigData.AlbumCopyPath)
                If fuThumb.NewFileName <> String.Empty Then
                    newImageName = newImageName & fuThumb.OriginalExtension
                    If (oldImage <> String.Empty) Then
                        ''Delete Old File
                        Utility.File.DeleteFile(CopyPath & oldImage)

                    End If
                    fuThumb.NewFileName = newImageName
                    fuThumb.SaveNewFile()
                    ab.ThumImg = newImageName
                    Core.CropByAnchor(CopyPath & newImageName, ImagePath & newImageName, 225, 225, Utility.Common.ImageAnchorPosition.Center)
                    Utility.File.DeleteFile(CopyPath & newImageName)
                ElseIf fuThumb.MarkedToDelete Then
                    '' ab.ThumImage = Nothing
                    ''Delete Old File
                    Utility.File.DeleteFile(CopyPath & oldImage)
                    Utility.File.DeleteFile(ImagePath & oldImage)
                    ''get thum nail from youtube
                    ab.ThumImg = newImageName
                End If

                If Id <> 0 Then
                    ab.Update()
                    If hidPopUpSong.Value <> "" Then
                        arr = Split(hidPopUpSong.Value.Trim(), ";")
                        ab.RemoveAlbumSong_Album(ab.AlbumId)
                        For i = 0 To arr.Length - 1
                            If (arr(i).ToString() <> "") Then
                                ab.InsertAlbumSong(ab.AlbumId, CInt(arr(i)))
                            End If
                        Next
                    End If
                    qr = qr & ab.AlbumId
                Else
                    ab.Insert()
                    Id = ab.AlbumId
                    'If hidPopUpSong.Value <> "" Then
                    '    arr = Split(hidPopUpSong.Value.Trim(), ";")
                    '    For i = 0 To arr.Length - 1
                    '        If (arr(i).ToString() <> "") Then
                    '            ab.InsertAlbumSong(ab.AlbumId, CInt(arr(i)))
                    '        End If
                    '    Next
                    'End If
                    qr = qr & ab.AlbumId
                End If
                DB.CommitTransaction()
                Response.Redirect("edit.aspx?s=1&" & qr & "&" & GetPageParams(FilterFieldType.All))
            Catch ex As SqlException
                DB.RollbackTransaction()
                AddError(ErrHandler.ErrorText(ex))
            End Try

        End If
    End Sub
    Private Sub btnAddSong_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddSong.Click
        If hidPopUpSong.Value <> "" And hidPopUpSong.Value <> "undefined" Then
            arr = Split(hidPopUpSong.Value.Trim(), ";")
            ''If arr(0) <> "thisForm" Then
            Dim s As SongRow
            Dim ab As New AlbumRow
            ltSong.Text = ""
            ab.RemoveAlbumSong_Album(Id)
            For i As Integer = 0 To arr.Length - 1
                If (arr(i).ToString() <> "") Then
                    s = SongRow.GetRow(DB, arr(i).ToString())
                    ltSong.Text &= s.Name & "<br />"
                    ab.InsertAlbumSong(Id, CInt(arr(i)))
                End If
            Next

            ''End If
        End If
        'LoadFromDB()
        LoadSong()
    End Sub

    Private Sub client_DownloadFileCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
        ''Create Thumb Image
        Utility.File.DeleteFile(ImagePath & newImageName)
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim imbUp As ImageButton = CType(e.Row.FindControl("imbUp"), ImageButton)
            Dim imbDown As ImageButton = CType(e.Row.FindControl("imbDown"), ImageButton)
            Dim imbDelete As ImageButton = CType(e.Row.FindControl("imbDelete"), ImageButton)
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
        End If
    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "Up" Then
            SongRow.ChangeChangeArrange(DB, Id, e.CommandArgument, True)
        ElseIf e.CommandName = "Down" Then
            SongRow.ChangeChangeArrange(DB, Id, e.CommandArgument, False)
        ElseIf e.CommandName = "Delete" Then
            SongRow.Remove_AlbumSong(DB, e.CommandArgument, Id)
            'ElseIf e.CommandName = "Active" Then
            '    AlbumRow.ChangeIsActive(DB, e.CommandArgument)
        End If
        Response.Redirect("edit.aspx?Id=" & Id & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class
