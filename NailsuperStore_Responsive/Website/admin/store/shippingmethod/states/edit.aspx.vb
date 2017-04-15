Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports Components
Imports DataLayer

Public Class admin_store_shippingint_edit
    Inherits AdminPage

    Private StateId As Integer

    Protected Overrides Sub OnInit(ByVal e As EventArgs)
        MyBase.OnInit(e)
        AddHandler Me.Load, AddressOf Page_Load
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        StateId = Convert.ToInt32(Request("StateId"))
        Delete.Visible = False
        If Not Page.IsPostBack Then
            If StateId <> 0 Then
                Dim dbState As StateRow = StateRow.GetRow(DB, StateId)
                lblStateName.Text = dbState.StateName
                chkIncludeDelivery.Checked = dbState.IncludeDelivery
                chkIncludeGiftWrap.Checked = dbState.IncludeGiftWrap
                lblStateCode.Text = dbState.StateCode
                txtRATE.Text = dbState.TaxRate
            End If
        End If
    End Sub

    Private Sub Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Save.Click
        'If the user entered anything in the password fields
        'then turn on password validators
        If Not Page.IsValid Then Exit Sub
        Try
            DB.BeginTransaction()
            If StateId <> 0 Then
                Dim dbState As StateRow = StateRow.GetRow(DB, StateId)
                dbState.StateName = lblStateName.Text
                dbState.StateCode = lblStateCode.Text
                dbState.IncludeDelivery = chkIncludeDelivery.Checked
                dbState.IncludeGiftWrap = chkIncludeGiftWrap.Checked
                dbState.TaxRate = txtRATE.Text
                dbState.Update()
            Else
                Dim dbState As StateRow = New StateRow(DB)
                dbState.StateName = lblStateName.Text
                dbState.StateCode = lblStateCode.Text
                dbState.IncludeDelivery = chkIncludeDelivery.Checked
                dbState.IncludeGiftWrap = chkIncludeGiftWrap.Checked
                dbState.TaxRate = txtRATE.Text
                dbState.Insert()
            End If
            DB.CommitTransaction()
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        Finally
        End Try

    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Delete.Click
        If StateId <> 0 Then
            Response.Redirect("delete.aspx?StateId=" & StateId & "&" & GetPageParams(FilterFieldType.All))
        End If
    End Sub
End Class