

Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports System.Net
Partial Class admin_FacebookPage_Edit
    Inherits AdminPage
    Protected Id As Integer
    Protected itemID As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Id = Convert.ToInt32(Request("Id"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub
   
   
    Private Sub LoadFromDB()
        If Id <= 0 Then
            Return
        End If
        Dim dbPage As FacebookPageRow = FacebookPageRow.GetRow(DB, Id)
        fuThumb.CurrentFileName = dbPage.Thumb
        ''fuThumb.Folder = "/" & Utility.ConfigData.SharefaceBookPath.Substring(0, Utility.ConfigData.SharefaceBookThumbPath.Length - 1)
        '' fuThumb.ImageDisplayFolder = "/" & Utility.ConfigData.SharefaceBookPath.Substring(0, Utility.ConfigData.SharefaceBookPath.Length - 1)
        hpimg.ImageUrl = "/" & Utility.ConfigData.SharefaceBookPath & dbPage.Thumb
        txtMetaDescription.Text = dbPage.MetaDescription
        txtPageTitle.Text = dbPage.PageTitle
        divImg.Visible = True
        txtLink.Text = dbPage.Link
        hpimg.ImageUrl = "/" & Utility.ConfigData.SharefaceBookPath & dbPage.Thumb
        If dbPage.Thumb <> "" Then
            fuThumb.EnableDelete = True
        Else
            fuThumb.EnableDelete = False
        End If
    End Sub
    Dim newImageName As String = ""
    Dim ImagePath As String = ""

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Page.IsValid Then
            Try
                Dim oldImage As String = ""
                Dim dbPageRow As FacebookPageRow
                If Id <> 0 Then
                    dbPageRow = FacebookPageRow.GetRow(DB, Id)
                    oldImage = dbPageRow.Thumb
                Else
                    dbPageRow = New FacebookPageRow(DB)
                End If
                dbPageRow.MetaDescription = txtMetaDescription.Text.Trim()
                dbPageRow.PageTitle = txtPageTitle.Text.Trim()
                dbPageRow.Link = txtLink.Text.Trim()
                fuThumb.Folder = "~/" & Utility.ConfigData.SharefaceBookPath
                newImageName = Guid.NewGuid().ToString()
                ImagePath = Server.MapPath("~/" & Utility.ConfigData.SharefaceBookPath)
                If fuThumb.NewFileName <> String.Empty Then
                    newImageName = newImageName & fuThumb.OriginalExtension
                    If (oldImage <> String.Empty) Then
                        ''Delete Old File
                        Utility.File.DeleteFile(ImagePath & oldImage)
                    End If
                    fuThumb.NewFileName = newImageName
                    fuThumb.SaveNewFile()
                    dbPageRow.Thumb = newImageName
                    '' Core.CropByAnchor(ImagePath & newImageName, SmallImagePath & newImageName, 230, 230, Utility.Common.ImageAnchorPosition.Center)                    ''176,170
                    '' delete original image
                    ''Utility.File.DeleteFile(ImagePath & newImageName)
                ElseIf fuThumb.MarkedToDelete Then
                    '' dbPageRow.ThumbImage = Nothing
                    ''Delete Old File
                    Utility.File.DeleteFile(ImagePath & oldImage)
                    '' Utility.File.DeleteFile(SmallImagePath & oldImage)
                    dbPageRow.Thumb = Nothing
                End If

                If Id <> 0 Then
                    FacebookPageRow.Update(DB, dbPageRow)
                Else
                    FacebookPageRow.Insert(DB, dbPageRow)
                End If
                Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
            Catch ex As SqlException
                AddError(ErrHandler.ErrorText(ex))
            End Try

        End If
    End Sub
   

    

    Private Sub ViewError(ByVal message As String)
        lblMessage.Text = "<span class='red'>" + message + "</span>"
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

End Class


