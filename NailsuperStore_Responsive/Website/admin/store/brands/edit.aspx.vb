Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_brands_Edit
    Inherits AdminPage

    Protected BrandId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        BrandId = Convert.ToInt32(Request("brandid"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If BrandId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbStoreBrand As StoreBrandRow = StoreBrandRow.GetRow(BrandId)
        txtBrandName.Text = dbStoreBrand.BrandName
        txtBrandNameUrl.Text = dbStoreBrand.BrandNameUrl
        txtDescription.Text = dbStoreBrand.Description
        fuHeaderImage.CurrentFileName = dbStoreBrand.HeaderImage
        chkIsTop.Checked = dbStoreBrand.IsTop
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
          
            Dim dbStoreBrand As StoreBrandRow

            If BrandId <> 0 Then
                dbStoreBrand = StoreBrandRow.GetRow(BrandId)
            Else
                dbStoreBrand = New StoreBrandRow(DB)
            End If
            dbStoreBrand.BrandName = txtBrandName.Text
            dbStoreBrand.BrandNameUrl = txtBrandNameUrl.Text
            dbStoreBrand.Description = txtDescription.Text
            dbStoreBrand.IsTop = chkIsTop.Checked
            If fuHeaderImage.NewFileName <> String.Empty Then
                fuHeaderImage.SaveNewFile()
                dbStoreBrand.HeaderImage = fuHeaderImage.NewFileName
            ElseIf fuHeaderImage.MarkedToDelete Then
                dbStoreBrand.HeaderImage = Nothing
            End If
            Dim result As Integer = 0
            If BrandId <> 0 Then
                result = StoreBrandRow.Update(dbStoreBrand)
            Else
                result = StoreBrandRow.Insert(dbStoreBrand)
            End If
            If result > 0 Then
                If fuHeaderImage.NewFileName <> String.Empty Or fuHeaderImage.MarkedToDelete Then fuHeaderImage.RemoveOldFile()
                Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
            ElseIf result = -1 Then
                AddError("This brand URLCode is exists")
                Exit Sub
            Else
                AddError("An error occurred during access data. Please try again later")
                Exit Sub
            End If
            

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Response.Redirect("delete.aspx?brandid=" & BrandId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

