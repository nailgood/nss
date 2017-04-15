Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Controls
Imports Components

Public Class admin__faq_move
    Inherits AdminPage

    Protected params As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim FaqId As Integer = CType(Request("FaqId"), Integer)
        Dim ACTION As String = Request("ACTION")

        params = GetPageParams(FilterFieldType.All)
        Try
            Core.ChangeSortOrder(DB, "FaqId", "Faq", "SortOrder", "", FaqId, ACTION)
            Response.Redirect("default.aspx?FaqId=" & FaqId & "&" & params)
        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class
