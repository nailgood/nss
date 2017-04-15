
Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Partial Class admin_NewsEvent_Image_Edit
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
        fuImage.Folder = "/" & Utility.ConfigData.PathNewImage
        '' fuImage.ImageDisplayFolder = "/" & Utility.ConfigData.PathSmallNewImage.Substring(0, Utility.ConfigData.PathSmallNewImage.Length - 1)
        '' fuImage.Folder = "/assets/NewsImage/small"
        '' fuImage.ImageDisplayFolder = "/assets/NewsImage/small"
        Dim dbImage As ImageRow = ImageRow.GetRow(DB, Id)
        txtName.Text = dbImage.ImageName
        chkIsActive.Checked = dbImage.IsActive
        fuImage.CurrentFileName = dbImage.FileName
        hpimg.ImageUrl = "/" & Utility.ConfigData.PathNewImage & dbImage.FileName
        If fuImage.CurrentFileName <> Nothing Then
            divImg.Visible = True
            ''If Right(fuImage.CurrentFileName.ToLower, 4) <> ".swf" Then  Else divImg.InnerHtml = "<script type=""text/javascript"" src=""/flash/homepage.swf.js.aspx?SWF=" & Server.UrlEncode(dbImage.BannerImage) & """></script>"
        End If
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Page.IsValid Then


            Try

                Dim dbImageRow As ImageRow
                Dim oldImage As String = ""
                If Id <> 0 Then
                    dbImageRow = ImageRow.GetRow(DB, Id)
                    oldImage = dbImageRow.FileName
                Else
                    dbImageRow = New ImageRow(DB)
                End If
                dbImageRow.ImageName = txtName.Text.Trim()
                dbImageRow.IsActive = chkIsActive.Checked
                '' fuImage.Width = 754
                '' fuImage.Height = 320
                fuImage.AutoResize = True
                fuImage.Folder = "~/" & Utility.ConfigData.PathNewImage
                Dim newImageName As String = Guid.NewGuid().ToString() & fuImage.OriginalExtension
                Dim ImagePath As String = Server.MapPath("~/" & Utility.ConfigData.PathNewImage)
                Dim SmallImagePath As String = Server.MapPath("~/" & Utility.ConfigData.PathSmallNewsImage)
                '' Dim LargeImagePath As String = Server.MapPath("~/" & Utility.ConfigData.PathLargeNewImage)

                If fuImage.NewFileName <> String.Empty Then
                    ''Delete Old File
                    Utility.File.DeleteFile(ImagePath & oldImage)
                    '' Utility.File.DeleteFile(LargeImagePath & oldImage)
                    Utility.File.DeleteFile(SmallImagePath & oldImage)

                    fuImage.NewFileName = newImageName
                    fuImage.SaveNewFile()
                    dbImageRow.FileName = newImageName
                    '' Core.ResizeImage(ImagePath & fuImage.NewFileName, LargeImagePath & newImageName, 800, 600)
                    Core.ResizeImage(ImagePath & fuImage.NewFileName, SmallImagePath & newImageName, 120, 96)
                    ''delete original image
                    '' Utility.File.DeleteFile(ImagePath & newImageName)
                ElseIf fuImage.MarkedToDelete Then
                    dbImageRow.FileName = Nothing
                End If
                If Id <> 0 Then
                    If Not ImageRow.Update(DB, dbImageRow) And fuImage.NewFileName <> String.Empty Then

                        Utility.File.DeleteFile(ImagePath & newImageName)
                        ''Utility.File.DeleteFile(LargeImagePath & newImageName)
                        Utility.File.DeleteFile(SmallImagePath & newImageName)
                    End If
                Else
                    If Not (ImageRow.Insert(DB, dbImageRow) And fuImage.NewFileName <> String.Empty) Then
                        Utility.File.DeleteFile(ImagePath & newImageName)
                        '' Utility.File.DeleteFile(LargeImagePath & newImageName)
                        Utility.File.DeleteFile(SmallImagePath & newImageName)
                    End If
                End If
                If fuImage.MarkedToDelete Then
                    Utility.File.DeleteFile(ImagePath & oldImage)
                    '' Utility.File.DeleteFile(LargeImagePath & oldImage)
                    Utility.File.DeleteFile(SmallImagePath & oldImage)
                End If
                Response.Redirect("default.aspx")
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

