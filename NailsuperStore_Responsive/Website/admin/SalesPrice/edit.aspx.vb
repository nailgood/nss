Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class Edit
	Inherits AdminPage

    Protected SalesPriceId As Integer

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        SalesPriceId = Convert.ToInt32(Request("SalesPriceId"))
		If Not IsPostBack Then
			LoadFromDB()
		End If
	End Sub

	Private Sub LoadFromDB()
		divImg.Visible = False

        If SalesPriceId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbSalesPrice As SalesPriceRow = SalesPriceRow.GetRow(DB, SalesPriceId)
        Dim dbStoreItem As StoreItemRow = StoreItemRow.GetRow(db, dbSalesPrice.ItemId)
        txtSKU.Text = dbStoreItem.SKU
        fuImage.CurrentFileName = dbSalesPrice.Image
        litMap.Text = dbSalesPrice.ImageMap
                hpimg.ImageUrl = ConfigurationManager.AppSettings("PathPromotion") & dbSalesPrice.Image
		If fuImage.CurrentFileName <> Nothing Then
			divImg.Visible = True
            If Right(fuImage.CurrentFileName.ToLower, 4) <> ".swf" Then  Else divImg.InnerHtml = "<script type=""text/javascript"" src=""/includes/flash/homepage.swf.js.aspx?SWF=" & Server.UrlEncode(dbSalesPrice.Image) & """></script>"
		End If
	End Sub

	Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
		If Not IsValid Then Exit Sub
		Try
			DB.BeginTransaction()

            Dim dbSalesPrice As SalesPriceRow

            If SalesPriceId <> 0 Then
                dbSalesPrice = SalesPriceRow.GetRow(DB, SalesPriceId)
            Else
                dbSalesPrice = New SalesPriceRow(DB)
            End If
            fuImage.Width = 475
            fuImage.Height = 205
            fuImage.AutoResize = True
            If fuImage.NewFileName <> String.Empty Then
                fuImage.SaveNewFile()
                dbSalesPrice.Image = fuImage.NewFileName
            ElseIf fuImage.MarkedToDelete Then
                dbSalesPrice.Image = Nothing
            End If

            If SalesPriceId <> 0 Then
                dbSalesPrice.UpdateImage()
            Else
                SalesPriceId = dbSalesPrice.Insert
            End If

			DB.CommitTransaction()

			If fuImage.NewFileName <> String.Empty Or fuImage.MarkedToDelete Then fuImage.RemoveOldFile()

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
        Response.Redirect("delete.aspx?SalesPriceId=" & SalesPriceId & "&" & GetPageParams(FilterFieldType.All))
	End Sub
End Class

