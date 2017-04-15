Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_groups_Edit
    Inherits AdminPage

    Protected ItemGroupId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        ItemGroupId = Convert.ToInt32(Request("ItemGroupId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        litMsg.Visible = False

        cblcblOptions.DataSource = StoreItemGroupOptionRow.GetAllStoreItemGroupOptions(DB)
        cblcblOptions.DataTextField = "OptionName"
        cblcblOptions.DataValueField = "OptionId"
        cblcblOptions.DataBind()

        If ItemGroupId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbStoreItemGroup As StoreItemGroupRow = StoreItemGroupRow.GetRow(DB, ItemGroupId)
        txtGroupName.Text = dbStoreItemGroup.GroupName
        cblcblOptions.SelectedValues = dbStoreItemGroup.GetSelectedStoreItemGroupOptions

        If DB.ExecuteScalar("select top 1 itemid from storeitem where itemgroupid = " & dbStoreItemGroup.ItemGroupId) <> Nothing Then
            For Each li As ListItem In cblcblOptions.Items
                li.Enabled = False
            Next
            litMsg.Visible = True
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub

        If cblcblOptions.SelectedValues = String.Empty Then
            AddError("You must select at least one option for this group.")
            Exit Sub
        End If

        Try
            DB.BeginTransaction()

            Dim dbStoreItemGroup As StoreItemGroupRow

            If ItemGroupId <> 0 Then
                dbStoreItemGroup = StoreItemGroupRow.GetRow(DB, ItemGroupId)
            Else
                dbStoreItemGroup = New StoreItemGroupRow(DB)
            End If
            dbStoreItemGroup.GroupName = txtGroupName.Text

            If ItemGroupId <> 0 Then
                dbStoreItemGroup.Update()
                WriteLogDetail("Update Item Group", dbStoreItemGroup)
            Else
                ItemGroupId = dbStoreItemGroup.Insert
                WriteLogDetail("Insert Item Group", dbStoreItemGroup)
            End If
            dbStoreItemGroup.DeleteFromAllStoreItemGroupOptions()
            dbStoreItemGroup.InsertToStoreItemGroupOptions(cblcblOptions.SelectedValues)

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
        Response.Redirect("delete.aspx?ItemGroupId=" & ItemGroupId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

