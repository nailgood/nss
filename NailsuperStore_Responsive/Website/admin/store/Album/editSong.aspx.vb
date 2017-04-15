
Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports System.Net
Partial Class admin_store_Album_editSong
    Inherits AdminPage
    Protected Id As Integer
    Protected si As StoreItemRow
    Dim arr As Array

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request("Id") <> Nothing Then
            Id = Convert.ToInt32(Request("Id"))
        End If
        If Not IsPostBack Then
            If Request.QueryString("s") <> Nothing Then
                lblMessage.Text = "You have inserted <span class=""red"">" & Request.QueryString("s") & "</span>"
            End If
            LoadFromDB()
        End If
    End Sub


    Private Sub LoadFromDB()
        If Id <= 0 Then
            Return
        End If
        Dim song As SongRow = SongRow.GetRow(DB, Id)
        txtName.Text = song.Name
        chkIsActive.Checked = song.IsActive
        txtUrl.Text = song.FileSong
        txtLenght.Text = song.FileLength

        Dim dv As DataView = DB.GetDataView("Select * from AlbumSong where SongId = " & Id)
        ltAlbum.Text = ""
        If dv.Count > 0 Then
            Dim abl As AlbumRow
            For i As Integer = 0 To dv.Count - 1
                abl = AlbumRow.GetRow(DB, dv(i)("AlbumId"))
                ltAlbum.Text &= abl.Name & "<br />"
                hidPopUpAlbum.Value &= dv(i)("AlbumId") & ";"
            Next
        End If
       


    End Sub
    Private Sub AddSong(ByVal Type As String)
        If Page.IsValid Then
            Try
                Dim i, j As Integer
                ''''Check Sku''''

                Dim oldImage As String = ""
                Dim oldIsYouTubeImage As Boolean = False
                Dim song As SongRow
                If Id <> 0 Then
                    song = SongRow.GetRow(DB, Id)
                Else
                    song = New SongRow(DB)
                End If

                song.Name = txtName.Text.Trim.Trim()
                song.FileSong = txtUrl.Text
                song.FileLength = txtLenght.Text
                song.IsActive = chkIsActive.Checked

                If Id <> 0 Then
                    song.Update()
                    If hidPopUpAlbum.Value <> "" Then
                        arr = Split(hidPopUpAlbum.Value.Trim(), ";")
                        Dim alb As New AlbumRow
                        alb.RemoveAlbumSong_Song(song.SongId)
                        For i = 0 To arr.Length - 1
                            If (arr(i).ToString() <> "") Then
                                alb.InsertAlbumSong(CInt(arr(i)), song.SongId)
                            End If
                        Next


                    End If
                Else
                    song.Insert()
                    arr = Split(hidPopUpAlbum.Value.Trim(), ";")
                    Dim alb As New AlbumRow
                    For i = 0 To arr.Length - 1
                        If (arr(i).ToString() <> "") Then
                            alb.InsertAlbumSong(CInt(arr(i)), song.SongId)
                        End If
                    Next
                End If
                Response.Redirect(Type & GetPageParams(FilterFieldType.All))
            Catch ex As SqlException
                AddError(ErrHandler.ErrorText(ex))
            End Try

        End If
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        AddSong("song.aspx?")
    End Sub


    Private Sub btnAddAlbum_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddAlbum.Click
        If hidPopUpAlbum.Value <> "" Then
            arr = Split(hidPopUpAlbum.Value.Trim(), ";")
            ''If arr(0) <> "thisForm" Then
            Dim alb As AlbumRow
            ltAlbum.Text = ""
            For i As Integer = 0 To arr.Length - 1
                If (arr(i).ToString() <> "") Then

                    alb = AlbumRow.GetRow(DB, arr(i).ToString())
                    ltAlbum.Text &= alb.Name & "<br />"
                End If
            Next

            ''End If
        End If
        'LoadFromDB()
    End Sub
    Private Function CheckAlbumSong(ByVal Id As String) As Boolean
        Dim CountID As String = DB.ExecuteScalar("Select isnull(count(Id),0) from AlbumSong Where AlbumId = " & CInt(Id))
        If CInt(CountID) > 0 Then
            Return True
        Else
            Return False
        End If

    End Function
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("song.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnSaveAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAdd.Click
        AddSong("editSong.aspx?s=" & txtName.Text.ToString & "&")
    End Sub
End Class
