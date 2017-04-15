Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_homepage_Edit
    Inherits AdminPage

    Protected HomepageImageId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        HomepageImageId = Convert.ToInt32(Request("HomepageImageId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        divImg.Visible = False

        If HomepageImageId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbHomepage As HomepageRow = HomepageRow.GetRow(DB, HomepageImageId)
        txtImageName.Text = dbHomepage.ImageName
        txtImageMap.Text = dbHomepage.ImageMap
        fuImage.CurrentFileName = dbHomepage.Image
        chkIsActive.Checked = dbHomepage.IsActive
        litMap.Text = dbHomepage.ImageMap
        hpimg.ImageUrl = "/assets/homepage/" & dbHomepage.Image
        If fuImage.CurrentFileName <> Nothing Then
            divImg.Visible = True
            If Right(fuImage.CurrentFileName.ToLower, 4) <> ".swf" Then  Else divImg.InnerHtml = "<script type=""text/javascript"" src=""/includes/flash/homepage.swf.js.aspx?SWF=" & Server.UrlEncode(dbHomepage.Image) & """></script>"
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbHomepage As HomepageRow

            If HomepageImageId <> 0 Then
                dbHomepage = HomepageRow.GetRow(DB, HomepageImageId)
            Else
                dbHomepage = New HomepageRow(DB)
            End If
            dbHomepage.ImageName = txtImageName.Text
            dbHomepage.IsActive = chkIsActive.Checked
            dbHomepage.ImageMap = txtImageMap.Text
            If fuImage.NewFileName <> String.Empty Then
                fuImage.SaveNewFile()
                dbHomepage.Image = fuImage.NewFileName
            ElseIf fuImage.MarkedToDelete Then
                dbHomepage.Image = Nothing
            End If

            If HomepageImageId <> 0 Then
                dbHomepage.Update()
            Else
                HomepageImageId = dbHomepage.Insert
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
        Response.Redirect("delete.aspx?HomepageImageId=" & HomepageImageId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

