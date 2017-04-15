Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_store_groups_options_choices_delete
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim ChoiceId As Integer = Convert.ToInt32(Request("ChoiceId"))
        Try
            Dim thumbImage As String = DB.ExecuteScalar("Select COALESCE(ThumbImage,'') from StoreItemGroupChoice where ChoiceId=" & ChoiceId)
            Dim result As Integer = StoreItemGroupChoiceRow.Delete(ChoiceId)
            If result = 1 Then
                If Not String.IsNullOrEmpty(thumbImage) Then
                    Dim ImagePath As String = Server.MapPath("~" & Utility.ConfigData.GroupChoiceThumbPath)
                    Utility.File.DeleteFile(ImagePath & "/" & thumbImage)
                End If
                Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
            ElseIf (result = -1) Then
                AddError("Group Option Choice is currently in use. Please try again later.")
            Else
                AddError("An error occured during this operation. Please try again later.")
            End If

        Catch ex As SqlException

            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class

