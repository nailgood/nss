Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_navision_mixmatch_Edit
    Inherits AdminPage

    Protected Id As Integer
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Id = Convert.ToInt32(Request("Id"))
        If Not IsPostBack Then

            LoadFromDB()

        End If
    End Sub
  
   
    Private Sub LoadFromDB()
        drpCustomerPriceGroupId.Datasource = CustomerPriceGroupRow.GetAllCustomerPriceGroups(DB)
        drpCustomerPriceGroupId.DataValueField = "CustomerPriceGroupId"
        drpCustomerPriceGroupId.DataTextField = "CustomerPriceGroupCode"
        drpCustomerPriceGroupId.Databind()
        drpCustomerPriceGroupId.Items.Insert(0, New ListItem("- - -", ""))

        If Id = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbMixMatch As MixMatchRow = MixMatchRow.GetRow(DB, Id)
        txtMixMatchNo.Text = dbMixMatch.MixMatchNo
        txtProduct.Text = dbMixMatch.Product
        txtDescription.Text = dbMixMatch.Description
        drpDiscountType.SelectedValue = dbMixMatch.DiscountType
        txtLinesToTrigger.Text = dbMixMatch.LinesToTrigger
        txtTimesApplicable.Text = dbMixMatch.TimesApplicable
        txtMandatory.Text = dbMixMatch.Mandatory
        txtOptional.Text = dbMixMatch.Optional
        dtStartingDate.Value = dbMixMatch.StartingDate
        dtEndingDate.Value = dbMixMatch.EndingDate
        drpCustomerPriceGroupId.SelectedValue = dbMixMatch.CustomerPriceGroupId
        chkIsActive.Checked = dbMixMatch.IsActive
        drlType.SelectedValue = dbMixMatch.Type

        cbIsCollection.Checked = dbMixMatch.IsCollection
        ddlDefaultType.SelectedValue = dbMixMatch.DefaultType
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            Dim dbMixMatchBeforUpdate As MixMatchRow = Nothing
            Dim dbMixMatch As MixMatchRow
            If Id <> 0 Then
                dbMixMatch = MixMatchRow.GetRow(DB, Id)
                dbMixMatchBeforUpdate = CloneObject.Clone(dbMixMatch)
            Else
                dbMixMatch = New MixMatchRow(DB)
            End If
            dbMixMatch.MixMatchNo = txtMixMatchNo.Text
            dbMixMatch.Product = txtProduct.Text
            dbMixMatch.Description = txtDescription.Text
            dbMixMatch.DiscountType = drpDiscountType.SelectedValue
            dbMixMatch.LinesToTrigger = txtLinesToTrigger.Text
            dbMixMatch.TimesApplicable = txtTimesApplicable.Text
            dbMixMatch.Mandatory = txtMandatory.Text
            dbMixMatch.Optional = txtOptional.Text
            dbMixMatch.StartingDate = dtStartingDate.Value
            dbMixMatch.EndingDate = dtEndingDate.Value
            dbMixMatch.CustomerPriceGroupId = IIf(IsNumeric(drpCustomerPriceGroupId.SelectedValue), drpCustomerPriceGroupId.SelectedValue, Nothing)
            dbMixMatch.Type = drlType.SelectedValue
            dbMixMatch.IsCollection = cbIsCollection.Checked
            dbMixMatch.DefaultType = ddlDefaultType.SelectedValue
            Dim redirect As Boolean = True
            Dim logDetail As New AdminLogDetailRow
            dbMixMatch.IsActive = chkIsActive.Checked
            logDetail.ObjectType = Utility.Common.ObjectType.MixMatch.ToString()
            If Id <> 0 Then
                If dbMixMatch.Type = Utility.Common.MixmatchType.ProductCoupon Then
                    ''check multi item vallue=0
                    Dim itemValue0 As Integer = MixMatchRow.CountItemBuy(DB, Id)
                    If itemValue0 > 1 Then
                        redirect = False
                        AddError("This mixmatch is not valid because it's more than 1 item  with Value=0")
                        Exit Sub
                    End If
                End If
                Dim result As String = dbMixMatch.Update()
                ''cachePrefixKey & "ItemCount_" & OrderId
                Utility.CacheUtils.RemoveCacheItemWithPrefix(StoreCartItemRow.cachePrefixKey & "ItemCount_")
                Utility.CacheUtils.RemoveCacheItemWithPrefix("popupCart_")

                ''Dim key As String = "popupCart_" & objCart.Order.OrderId
                logDetail.ObjectId = Id
                If (result = "1") Then
                    redirect = True
                    logDetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.MixMatch, dbMixMatchBeforUpdate, dbMixMatch)
                    logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                ElseIf result = "0" Then
                    redirect = False
                    AddError("A error has occured. Please try again.")
                Else
                    redirect = False
                    AddError("Item #" & result & " is active at the same time in other mixmatch . Please check again")
                End If
                '' WriteLogDetail("Update Mixmatch", dbMixMatch)
            Else
                Id = dbMixMatch.Insert
                logDetail.ObjectId = Id
                logDetail.Message = AdminLogHelper.ConvertObjectInsertToLogMesssageString(dbMixMatch, Utility.Common.ObjectType.MixMatch)
                logDetail.Action = Utility.Common.AdminLogAction.Insert.ToString()
            End If
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
            If redirect Then
                Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
            End If

        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Response.Redirect("delete.aspx?Id=" & Id & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

