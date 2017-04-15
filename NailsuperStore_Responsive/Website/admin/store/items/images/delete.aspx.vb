Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_items_images_delete
    Inherits AdminPage

    Protected ItemId As Integer
    Protected ImageId As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim FileName As String = Request("FileName")

        ItemId = Convert.ToInt32(Request("F_ItemId"))
        ImageId = Convert.ToInt32(Request("ImageId"))
        If ItemId = Nothing Or ImageId = Nothing Then Response.Redirect("/admin/store/items/")

        Try
            If System.IO.File.Exists(Server.MapPath("/assets/items/" & FileName)) Then System.IO.File.Delete(Server.MapPath("/assets/items/" & FileName))
            If System.IO.File.Exists(Server.MapPath("/assets/items/large/" & FileName)) Then System.IO.File.Delete(Server.MapPath("/assets/items/large/" & FileName))
            If System.IO.File.Exists(Server.MapPath("/assets/items/middle/" & FileName)) Then System.IO.File.Delete(Server.MapPath("/assets/items/middle/" & FileName))
            If System.IO.File.Exists(Server.MapPath("/assets/items/swatch/" & FileName)) Then System.IO.File.Delete(Server.MapPath("/assets/items/swatch/" & FileName))
            If System.IO.File.Exists(Server.MapPath("/assets/items/swatch/large/" & FileName)) Then System.IO.File.Delete(Server.MapPath("/assets/items/swatch/large/" & FileName))
            If System.IO.File.Exists(Server.MapPath("/assets/items/thumbnails/" & FileName)) Then System.IO.File.Delete(Server.MapPath("/assets/items/thumbnails/" & FileName))
            If System.IO.File.Exists(Server.MapPath("/assets/items/room/" & FileName)) Then System.IO.File.Delete(Server.MapPath("/assets/items/room/" & FileName))
            If System.IO.File.Exists(Server.MapPath("/assets/items/room/small/" & FileName)) Then System.IO.File.Delete(Server.MapPath("/assets/items/room/small/" & FileName))
        Catch ex As Exception
            AddError("An error occurred while removing the images.")
        End Try

        Try
            DB.ExecuteSQL("DELETE FROM StoreItemImage WHERE Id=" & ImageId)
            Response.Redirect("default.aspx?Itemid=" & ItemId.ToString & "&" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("default.aspx?ItemId=" & ItemId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class
