Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_groups_options_choices_Edit
    Inherits AdminPage

    Protected ChoiceId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        ChoiceId = Convert.ToInt32(Request("ChoiceId"))
        If Not IsPostBack Then
            fuThumbImage.Folder = Utility.ConfigData.GroupChoiceThumbPath
            fuThumbImage.ImageDisplayFolder = Utility.ConfigData.GroupChoiceThumbPath
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        drpOptionId.Datasource = StoreItemGroupOptionRow.GetAllItemGroupOptions(DB)
        drpOptionId.DataValueField = "OptionId"
        drpOptionId.DataTextField = "OptionName"
        drpOptionId.Databind()
        drpOptionId.Items.Insert(0, New ListItem("", ""))

        If ChoiceId = 0 Then
            If IsNumeric(Request("F_OptionId")) Then drpOptionId.SelectedValue = CInt(Request("F_OptionId"))
            btnDelete.Visible = False
            Exit Sub
        End If

        drpOptionId.Enabled = False

        Dim dbStoreItemGroupChoice As StoreItemGroupChoiceRow = StoreItemGroupChoiceRow.GetRow(DB, ChoiceId)
        txtChoiceName.Text = dbStoreItemGroupChoice.ChoiceName
        drpOptionId.SelectedValue = dbStoreItemGroupChoice.OptionId
        fuThumbImage.CurrentFileName = dbStoreItemGroupChoice.ThumbImage
        hpimg.ImageUrl = fuThumbImage.Folder & dbStoreItemGroupChoice.ThumbImage
        If fuThumbImage.CurrentFileName <> Nothing Then
            divImg.Visible = True

        End If
        fuThumbImage.EnableDelete = True
    End Sub
   
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            Dim dbStoreItemGroupChoice As StoreItemGroupChoiceRow
            If ChoiceId <> 0 Then
                dbStoreItemGroupChoice = StoreItemGroupChoiceRow.GetRow(DB, ChoiceId)
            Else
                dbStoreItemGroupChoice = New StoreItemGroupChoiceRow(DB)
            End If
            dbStoreItemGroupChoice.ChoiceName = txtChoiceName.Text
            dbStoreItemGroupChoice.OptionId = drpOptionId.SelectedValue
            If fuThumbImage.NewFileName <> String.Empty Then
                Dim ImagePath As String = Server.MapPath("~" & Utility.ConfigData.GroupChoiceThumbPath)
                Using bitmap As New Bitmap(fuThumbImage.MyFile.InputStream, False)
                    If bitmap.Width <> 50 Or bitmap.Height <> 50 Then
                        AddError("Image size must be 50x50")
                        Exit Sub
                    End If
                End Using
                fuThumbImage.SaveNewFile()
                If Not String.IsNullOrEmpty(dbStoreItemGroupChoice.ThumbImage) Then
                    Utility.File.DeleteFile(ImagePath & "/" & dbStoreItemGroupChoice.ThumbImage)
                End If
                dbStoreItemGroupChoice.ThumbImage = fuThumbImage.NewFileName
            ElseIf fuThumbImage.MarkedToDelete Then
                dbStoreItemGroupChoice.ThumbImage = Nothing
                fuThumbImage.RemoveOldFile()
            End If
            If ChoiceId <> 0 Then
                dbStoreItemGroupChoice.Update()
            Else
                ChoiceId = dbStoreItemGroupChoice.Insert
            End If
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Response.Redirect("delete.aspx?ChoiceId=" & ChoiceId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

