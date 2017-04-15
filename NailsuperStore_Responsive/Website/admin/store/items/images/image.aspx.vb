Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_items_images_image
    Inherits AdminPage

    Protected ItemId As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        ItemId = Convert.ToInt32(Request("ItemId"))
        If ItemId = Nothing Then Response.Redirect("/admin/store/items/")

        If Not IsPostBack Then
            Dim FileName As String = Request("FileName")
            Dim NewImage As System.Drawing.Image

            NewImage = Drawing.Image.FromFile(Server.MapPath("/assets/items/" & FileName))
            lblThumb.Text = "Uploaded Image (" & NewImage.Width & " x " & NewImage.Height & ")"
            Thumb.ImageUrl = "/assets/items/" & FileName
        End If
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("default.aspx?ItemId=" & ItemId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class