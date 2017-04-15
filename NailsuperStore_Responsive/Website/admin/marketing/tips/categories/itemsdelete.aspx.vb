Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_tips_categories_itemsdelete
    Inherits AdminPage

    Private iId As Integer
    Private TipCategoryId As Integer
    Private sType As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        TipCategoryId = Convert.ToInt32(Request("TipCategoryId"))
        iId = Convert.ToInt32(Request("Id"))
        sType = Convert.ToString(Request("sType"))

        Try
            DB.BeginTransaction()
            If sType = "Item" Then
                TipCategoryRow.DeleteTipCategoryItem(DB, TipCategoryId, iId)
            ElseIf sType = "Department" Then
                TipCategoryRow.DeleteTipCategoryDepartment(DB, TipCategoryId, iId)
            End If
            DB.CommitTransaction()

            If sType = "Item" Then
                Response.Redirect("items.aspx?TipCategoryId=" & TipCategoryId & "&" & GetPageParams(FilterFieldType.All))
            ElseIf sType = "Department" Then
                Response.Redirect("departments.aspx?TipCategoryId=" & TipCategoryId & "&" & GetPageParams(FilterFieldType.All))
            End If

        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        Finally
        End Try
    End Sub
End Class