Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_items_images_Edit
    Inherits AdminPage

    Protected F_ItemId As Integer
    Protected ImageId As Integer
    Protected dbStoreItem As StoreItemRow

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        ImageId = Convert.ToInt32(Request("ImageId"))
        F_ItemId = Convert.ToInt32(Request("F_ItemId"))
        dbStoreItem = StoreItemRow.GetRow(DB, F_ItemId)
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If ImageId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbStoreItemImage As StoreItemImageRow = StoreItemImageRow.GetRow(DB, ImageId)
        txtImageAltTag.Text = dbStoreItemImage.ImageAltTag
        fuImage.CurrentFileName = dbStoreItemImage.Image
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbStoreItemImage As StoreItemImageRow

            If ImageId <> 0 Then
                dbStoreItemImage = StoreItemImageRow.GetRow(DB, ImageId)
            Else
                dbStoreItemImage = New StoreItemImageRow(DB)
            End If
            dbStoreItemImage.ItemId = F_ItemId
            dbStoreItemImage.ImageAltTag = txtImageAltTag.Text
            Dim RootPath As String = SysParam.GetValue("FilePath")
            Dim ImagePath As String = Server.MapPath("/assets/items/")
            If fuImage.NewFileName <> String.Empty Then
                fuImage.SaveNewFile()
                dbStoreItemImage.Image = fuImage.NewFileName
                Core.ResizeImage(ImagePath & "original/" & fuImage.NewFileName, ImagePath & "large/" & fuImage.NewFileName, 500, 500)
                Core.ResizeImage(ImagePath & "original/" & fuImage.NewFileName, ImagePath & fuImage.NewFileName, 230, 230)
                Core.ResizeImage(ImagePath & "original/" & fuImage.NewFileName, ImagePath & "medium/" & fuImage.NewFileName, 167, 167)
                Core.ResizeImage(ImagePath & "original/" & fuImage.NewFileName, ImagePath & "featured/" & fuImage.NewFileName, 115, 115)
                Core.ResizeImage(ImagePath & "original/" & fuImage.NewFileName, ImagePath & "related/" & fuImage.NewFileName, 80, 80)
                Core.ResizeImage(ImagePath & "original/" & fuImage.NewFileName, ImagePath & "small/" & fuImage.NewFileName, 77, 47) 'Polish
                Core.ResizeImage(ImagePath & "original/" & fuImage.NewFileName, ImagePath & "cart/" & fuImage.NewFileName, 58, 58)
                Core.ResizeImage(ImagePath & "original/" & fuImage.NewFileName, ImagePath & "free/" & fuImage.NewFileName, 35, 35)
            ElseIf fuImage.MarkedToDelete Then
                dbStoreItemImage.Image = Nothing
            End If

            If ImageId <> 0 Then
                dbStoreItemImage.Update()
            Else
                ImageId = dbStoreItemImage.Insert
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
        Response.Redirect("delete.aspx?ImageId=" & ImageId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

