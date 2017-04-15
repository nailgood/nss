
Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer
Imports Controls
Partial Class admin_Keyword_Synonym_delete
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim KeywordId As Integer = Convert.ToInt64(Request("KeywordId"))
        Try
            DB.ExecuteSQL("delete KeywordSynonym where KeywordId=" & KeywordId)
            Response.Redirect("default.aspx?SynonymType=" + GetQueryString("SynonymType")) ''& GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class

