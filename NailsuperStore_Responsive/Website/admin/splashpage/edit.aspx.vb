Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_splashpage_Edit
    Inherits AdminPage
    Protected SplashPageImageId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SplashPageImageId = Convert.ToInt32(Request("SplashPageImageId"))

        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        divImg.Visible = False

        If SplashPageImageId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbSplashPage As SplashPageRow = SplashPageRow.GetRow(DB, SplashPageImageId)
        txtImageName.Text = dbSplashPage.ImageName
        txtImageMap.Text = dbSplashPage.ImageMap
        fuImage.CurrentFileName = dbSplashPage.Image
        chkIsActive.Checked = dbSplashPage.IsActive
        litMap.Text = dbSplashPage.ImageMap
        hpimg.ImageUrl = "/assets/splashpage/" & dbSplashPage.Image
        If fuImage.CurrentFileName <> Nothing Then
            divImg.Visible = True
            If Right(fuImage.CurrentFileName.ToLower, 4) <> ".swf" Then  Else divImg.InnerHtml = "<script type=""text/javascript"" src=""/includes/flash/homepage.swf.js.aspx?SWF=" & Server.UrlEncode(dbSplashPage.Image) & """></script>"
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbSplashPage As SplashPageRow

            If SplashPageImageId <> 0 Then
                dbSplashPage = SplashPageRow.GetRow(DB, SplashPageImageId)
            Else
                dbSplashPage = New SplashPageRow(DB)
            End If
            dbSplashPage.ImageName = txtImageName.Text
            dbSplashPage.IsActive = chkIsActive.Checked
            dbSplashPage.ImageMap = txtImageMap.Text
            If fuImage.NewFileName <> String.Empty Then
                fuImage.SaveNewFile()
                dbSplashPage.Image = fuImage.NewFileName
            ElseIf fuImage.MarkedToDelete Then
                dbSplashPage.Image = Nothing
            End If

            If SplashPageImageId <> 0 Then
                dbSplashPage.Update()
            Else
                SplashPageImageId = dbSplashPage.Insert
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
        Response.Redirect("delete.aspx?SplashPageImageId=" & SplashPageImageId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

