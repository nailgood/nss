
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.IO
Partial Class admin_settings_shipping_Default
    Inherits AdminPage
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            BindRepeater()
        End If
    End Sub

    Private Sub BindRepeater()

        sysparamRepeater.DataSource = StoreItemSpecialHandlingFeeRow.GetAll(DB)
        sysparamRepeater.DataBind()
        ''load Sysparam
        Dim amountFree As Double = SysParam.GetValue("FreeShippingOrderAmount")
        If amountFree > 0 Then
            txtAmountFee.Text = amountFree

        End If
    End Sub

    Private Sub SysparamRepeater_OnItemDataBound(ByVal sender As System.Object, ByVal e As RepeaterItemEventArgs) Handles sysparamRepeater.ItemDataBound
        Dim txtLowWeightValue As TextBox = Nothing
        Dim txtHighWeightValue As TextBox = Nothing
        Dim txtHandlingFee As TextBox = Nothing
        Dim lblWeightRange As Label = Nothing
        Dim bHasRow As Boolean = False
        Dim hidId As HiddenField = Nothing
        Dim index As Integer = 0
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            lblWeightRange = e.Item.FindControl("lblWeightRange")
            txtLowWeightValue = e.Item.FindControl("txtLowWeightValue")
            txtHighWeightValue = e.Item.FindControl("txtHighWeightValue")
            txtHandlingFee = e.Item.FindControl("txtHandlingFee")
            hidId = e.Item.FindControl("hidId")
            bHasRow = True
        End If


        If bHasRow Then
            index = e.Item.ItemIndex + 1
            Dim objData As StoreItemSpecialHandlingFeeRow = e.Item.DataItem
            txtLowWeightValue.Text = objData.LowWeightValue
            txtHighWeightValue.Text = objData.HighWeightValue
            txtHandlingFee.Text = objData.SpecialHandlingFee
            lblWeightRange.Text = lblWeightRange.Text & " " & index
            hidId.Value = objData.Id
        End If
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Page.IsValid Then
            Dim dbSysParam As SysparamRow = SysparamRow.GetRow(DB, "FreeShippingOrderAmount")
            Dim AmountFee As Double = CDbl(txtAmountFee.Text)
            Dim amountFreeDB As Double = dbSysParam.Value
            Dim changeLog As String = String.Empty
            Dim logDetail As New AdminLogDetailRow
            If AmountFee <> amountFreeDB Then
                'Dim dbSysParam As SysparamRow = SysparamRow.GetRow(DB, "FreeShippingOrderAmount")
                changeLog = dbSysParam.Comments & "|" & dbSysParam.Value.ToString().Trim() & "|" & AmountFee & "[br]"
                DB.ExecuteScalar("Update Sysparam set Value=" & AmountFee & " where Name='FreeShippingOrderAmount'")
                'logDetail.ObjectId = dbSysParam.ParamId
                'logDetail.ObjectType = Utility.Common.ObjectType.Sysparam.ToString()
                'logDetail.Message = changeLog
                'logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                'AdminLogHelper.WriteLuceneLogDetail(logDetail)

                Utility.CacheUtils.RemoveCache("Sysparam_ListAll")
            End If
            Dim hidId As HiddenField = Nothing
            Dim txtLowWeightValue As TextBox = Nothing
            Dim txtHighWeightValue As TextBox = Nothing
            Dim txtHandlingFee As TextBox = Nothing
            Dim num As Integer = 1
            For Each i As RepeaterItem In sysparamRepeater.Items
                hidId = CType(i.FindControl("hidId"), HiddenField)
                txtLowWeightValue = CType(i.FindControl("txtLowWeightValue"), TextBox)
                txtHighWeightValue = CType(i.FindControl("txtHighWeightValue"), TextBox)
                txtHandlingFee = CType(i.FindControl("txtHandlingFee"), TextBox)
                Dim objData As StoreItemSpecialHandlingFeeRow = StoreItemSpecialHandlingFeeRow.GetById(DB, CInt(hidId.Value))
                Dim objBefore As StoreItemSpecialHandlingFeeRow = CloneObject.Clone(objData)
                If Not objData Is Nothing Then
                    ''  objData.Id = hidId.Value
                    objData.LowWeightValue = txtLowWeightValue.Text
                    objData.HighWeightValue = txtHighWeightValue.Text
                    objData.SpecialHandlingFee = txtHandlingFee.Text
                    StoreItemSpecialHandlingFeeRow.Update(DB, objData)
                    Dim tmp As String = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.Sysparam, objBefore, objData)
                    If Not String.IsNullOrEmpty(tmp) Then
                        changeLog &= "(" & num & ")" & tmp
                    End If
                    num = num + 1
                End If
            Next
            logDetail.ObjectId = dbSysParam.ParamId
            logDetail.ObjectType = Utility.Common.ObjectType.Sysparam.ToString()
            logDetail.Message = changeLog
            logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
            ''
            Response.Redirect(Request.RawUrl)
        End If
    End Sub
End Class
