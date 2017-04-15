

Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_NewsEvent_Image_Delete
    Inherits AdminPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim Id As Integer = Convert.ToInt32(Request("Id"))
        Try
            ''Delete Iamges
            Dim itemDB As ImageRow = ImageRow.GetRow(DB, Id)
            If (ImageRow.Delete(DB, Id)) Then
                Dim ImagePath As String = Server.MapPath("~/" & Utility.ConfigData.PathNewImage)
                Dim SmallImagePath As String = Server.MapPath("~/" & Utility.ConfigData.PathSmallNewsImage)
                '' Dim LargeImagePath As String = Server.MapPath("~/" & Utility.ConfigData.PathLargeNewImage)
                Utility.File.DeleteFile(ImagePath & itemDB.FileName)
                ''Utility.File.DeleteFile(LargeImagePath & itemDB.FileName)
                Utility.File.DeleteFile(SmallImagePath & itemDB.FileName)
            End If
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
    Public Sub RemoveFileName(ByVal Path As String, ByVal FileName As String)
        Try
            System.IO.File.Delete(HttpContext.Current.Server.MapPath(Path & FileName))
        Catch ex As Exception
        End Try
    End Sub
End Class
