Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_broadcast_templates_slots_move
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim SlotId As Integer = Convert.ToInt32(Request("SlotId"))
        Dim TemplateId As Integer = Convert.ToInt32(Request("TemplateId"))
        Dim Action As String = Request("ACTION")
        Try
            Core.ChangeSortOrder(DB, "SlotId", "MailingTemplateSlot", "SortOrder", " templateid = " & TemplateId, SlotId, Action)
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class

