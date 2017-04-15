Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_coupon_Edit
    Inherits AdminPage

    Protected CouponId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        CouponId = Convert.ToInt32(Request("CouponId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        drpReferralId.DataSource = ReferralRow.GetAllReferrals(DB)
        drpReferralId.DataValueField = "ReferralId"
        drpReferralId.DataTextField = "Name"
        drpReferralId.DataBind()
        drpReferralId.Items.Insert(0, New ListItem("", ""))

        If CouponId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbStoreCoupon As StoreCouponRow = StoreCouponRow.GetRow(DB, CouponId)
        txtName.Text = dbStoreCoupon.Name
        dtStartDate.Value = dbStoreCoupon.StartDate
        dtEndDate.Value = dbStoreCoupon.EndDate
        drpReferralId.SelectedValue = dbStoreCoupon.ReferralId
        fuImage.CurrentFileName = dbStoreCoupon.Image
        rblIsActive.SelectedValue = dbStoreCoupon.IsActive
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbStoreCoupon As StoreCouponRow

            If CouponId <> 0 Then
                dbStoreCoupon = StoreCouponRow.GetRow(DB, CouponId)
            Else
                dbStoreCoupon = New StoreCouponRow(DB)
            End If
            dbStoreCoupon.StartDate = dtStartDate.Value
            dbStoreCoupon.EndDate = dtEndDate.Value
            dbStoreCoupon.ReferralId = drpReferralId.SelectedValue
            If fuImage.NewFileName <> String.Empty Then
                fuImage.SaveNewFile()
                dbStoreCoupon.Image = fuImage.NewFileName
            ElseIf fuImage.MarkedToDelete Then
                dbStoreCoupon.Image = Nothing
            End If
            dbStoreCoupon.IsActive = rblIsActive.SelectedValue

            If CouponId <> 0 Then
                dbStoreCoupon.Update()
            Else
                CouponId = dbStoreCoupon.Insert
            End If

            DB.CommitTransaction()

            If fuImage.NewFileName <> String.Empty OrElse fuImage.MarkedToDelete Then fuImage.RemoveOldFile()

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
        Response.Redirect("delete.aspx?CouponId=" & CouponId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class
