Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_requestcallbacklanguage_editEmail
    Inherits AdminPage

    Protected LanguageId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        LanguageId = Convert.ToInt32(Request("LanguageId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()

        drpLanguageId.DataSource = RequestCallBackLanguageRow.GetAllRequestCallBackLanguages(DB)
        drpLanguageId.DataValueField = "LanguageId"
        drpLanguageId.DataTextField = "Language"
        drpLanguageId.DataBind()
        drpLanguageId.Items.Insert(0, New ListItem("", ""))
        If LanguageId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If
        Dim dbCustomerContact As RequestCallBackLanguageRow = RequestCallBackLanguageRow.GetRow(DB, LanguageId)
        drpLanguageId.SelectedValue = dbCustomerContact.LanguageId

        If drpLanguageId.SelectedItem.Value <> "" Then
            Dim dbContactUs As RequestCallBackLanguageRow = RequestCallBackLanguageRow.GetRow(DB, drpLanguageId.SelectedItem.Value)

            txtLanguageCode.Text = dbContactUs.LanguageCode
        Else

            txtLanguageCode.Text = ""
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbCustomerContact As RequestCallBackLanguageRow

            If LanguageId <> 0 Then
                dbCustomerContact = RequestCallBackLanguageRow.GetRow(DB, LanguageId)
            Else
                dbCustomerContact = New RequestCallBackLanguageRow(DB)
            End If
            dbCustomerContact.LanguageId = drpLanguageId.SelectedValue
            dbCustomerContact.LanguageCode = txtLanguageCode.Text

            If LanguageId <> 0 Then
                dbCustomerContact.Update()
            Else
                LanguageId = dbCustomerContact.Insert
            End If
            DB.CommitTransaction()

            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Response.Redirect("delete.aspx?DetailId=" & LanguageId & "&" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub drpLanguageId_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpLanguageId.SelectedIndexChanged
        If drpLanguageId.SelectedItem.Value <> "" Then
            Dim dbContactUs As RequestCallBackLanguageRow = RequestCallBackLanguageRow.GetRow(DB, drpLanguageId.SelectedItem.Value)

            txtLanguageCode.Text = dbContactUs.LanguageCode
            LanguageId = drpLanguageId.SelectedItem.Value
        Else

            txtLanguageCode.Text = ""
        End If
    End Sub
End Class

