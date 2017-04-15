Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_tips_itemsdelete
    Inherits AdminPage

    Private iId As Integer
    Private TipId As Integer
    Private sType As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        TipId = Convert.ToInt32(Request("TipId"))
        iId = Convert.ToInt32(Request("Id"))
        sType = Convert.ToString(Request("sType"))

        Try
            DB.BeginTransaction()
            If sType = "Item" Then
                TipRow.DeleteTipItem(DB, TipId, iId)
            ElseIf sType = "Department" Then
                TipRow.DeleteTipDepartment(DB, TipId, iId)
            End If
            DB.CommitTransaction()

            If sType = "Item" Then
                Response.Redirect("items.aspx?TipId=" & TipId & "&" & GetPageParams(FilterFieldType.All))
            ElseIf sType = "Department" Then
                Response.Redirect("departments.aspx?TipId=" & TipId & "&" & GetPageParams(FilterFieldType.All))
            End If

        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        Finally
        End Try
    End Sub
End Class