Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_items_SetupFlatFee
    Inherits AdminPage

    Protected ItemId As Integer
    Protected stateCode As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ItemId = Convert.ToInt32(Request("ItemId"))
        Try
            '' stateCode = Convert.ToInt32(Request("stateCode"))
        Catch ex As Exception

        End Try
        If Not IsPostBack Then
            LoadFeeDataForItem()
            LoadState()
            LoadFeeData()
        End If


    End Sub
    Private Sub LoadFeeDataForItem()
        Dim item As StoreItemRow = StoreItemRow.GetRow(DB, ItemId)
        If Not item Is Nothing Then
            If item.FeeShipOversize > 0 Then
                txtItemFee.Text = item.FeeShipOversize
            End If
        End If
    End Sub
    Private Sub LoadFeeData()
        Dim objData As FeeShippingStateRow = FeeShippingStateRow.GetDetailByStateCode(DB, ItemId, drlState.SelectedValue)
        If Not objData Is Nothing Then
            If objData.Id > 0 Then
                txtFirstItemFeeShipping.Text = objData.FirstItemFeeShipping
                txtNextItemFeeShipping.Text = objData.NextItemFeeShipping
                hidID.Value = objData.Id
            Else
                txtFirstItemFeeShipping.Text = ""
                txtNextItemFeeShipping.Text = ""
                hidID.Value = ""
            End If
        End If
    End Sub
    Private Sub LoadState()
        Dim ds As DataSet = StateRow.GetAllStatesSetupFee(ItemId)
        drlState.DataSource = ds
        drlState.DataTextField = "StateName"
        drlState.DataValueField = "StateCode"
        drlState.DataBind()
        drlState.Items.Insert(0, New ListItem("- - -", ""))
        ''  drlState.SelectedValue = stateCode
    End Sub
    Protected Sub drlState_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drlState.SelectedIndexChanged
        LoadFeeData()
    End Sub
    Protected Sub btnSaveItemFee_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveItemFee.Click
        If Page.IsValid Then
            Dim item As StoreItemRow = StoreItemRow.GetRow(DB, ItemId)
            If Not item Is Nothing Then
                item.FeeShipOversize = txtItemFee.Text
                item.Update()
            End If
            Response.Redirect(Me.Request.RawUrl)
          
        End If
    End Sub
    Protected Sub btnSaveItemFeeByState_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveItemFeeByState.Click
        If txtItemFee.Text = "" Then
            AddError("Fee Apply In US is blank")
            Exit Sub
        End If
        If Page.IsValid Then
            SaveData()
            LoadFeeData()
        End If
    End Sub
    Private Sub SaveData()
        
        Dim row As New FeeShippingStateRow()
        row.ItemId = ItemId
        row.StateCode = drlState.SelectedValue
        row.FirstItemFeeShipping = CDbl(txtFirstItemFeeShipping.Text)
        row.NextItemFeeShipping = CDbl(txtNextItemFeeShipping.Text)
        If hidID.Value = "" Then
            FeeShippingStateRow.Insert(row)
        Else
            row.Id = CInt(hidID.Value)
            FeeShippingStateRow.Update(row)
            hidID.Value = ""
        End If

          
    End Sub
   
   
   
End Class
