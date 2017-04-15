

Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_NewsEvent_Document_Delete
    Inherits AdminPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim Id As Integer = Convert.ToInt32(Request("Id"))
        Try
            ''Delete Iamges
            Dim itemDB As DocumentRow = DocumentRow.GetRow(DB, Id)
            If (DocumentRow.Delete(DB, Id)) Then
                Dim DocumentPath As String = Server.MapPath("~" & Utility.ConfigData.PathNewDocument)
                Utility.File.DeleteFile(DocumentPath & itemDB.FileName)
            End If
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class
