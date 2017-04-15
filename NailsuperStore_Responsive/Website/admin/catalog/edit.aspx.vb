Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports Components
Imports DataLayer

Public Class admin_catalog_edit
    Inherits AdminPage

    Private CatalogId As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        CatalogId = Convert.ToInt32(Request("CatalogId"))
        If CatalogId <> 0 Then Delete.Visible = True Else Delete.Visible = False
        lblSizes.Text = "<span class=smaller>Image should be " & SysParam.GetValue("CatalogImageHeight") & " pixels high by " & SysParam.GetValue("CatalogImageWidth") & " pixels wide as a minimum.</span>"
        If Not Page.IsPostBack Then
            If CatalogId <> 0 Then
                Dim dbCatalog As StoreCatalogRow = StoreCatalogRow.GetRow(DB, CatalogId)
                txtTitle.Text = dbCatalog.Title
                IsActive.Checked = dbCatalog.IsActive
                txtImageAltTag.Text = dbCatalog.ImageAltTag
                txtLink.Text = dbCatalog.RichFXLink

                If Not dbCatalog.CatalogImage = String.Empty Then img.ImageUrl = "/assets/catalog/" & dbCatalog.CatalogImage
                'Image is not required in this case, but if entered then overwrite the old one
                ImageNameReq.Enabled = False
            End If
        End If
    End Sub

    Private Function SaveCatalogImage() As String
        'Check file size
        Dim myFile As HttpPostedFile = Image.PostedFile
        If myFile.ContentLength = 0 Then Throw New ApplicationException("No file was uploaded for the catalog image")

        ' Check file extension (must be JPG or GIF)
        Dim Extension As String = System.IO.Path.GetExtension(myFile.FileName).ToLower()
        If Not (Extension = ".jpg" Or Extension = ".gif") Then
            Throw New ApplicationException("The file must have an extension of JPG or GIF")
        End If

        Dim Img As System.Drawing.Image = Drawing.Image.FromStream(myFile.InputStream)
        If Img.Width < SysParam.GetValue("CatalogImageWidth") Then
            Throw New ApplicationException("You must upload an image that is wider than " & SysParam.GetValue("CatalogImageWidth") & " pixels.")
        End If

        'Remove special charactes such as space and comma
        Dim FileName As String = System.IO.Path.GetFileName(myFile.FileName)
        FileName = Replace(FileName, " ", "_")
        FileName = Replace(FileName, ",", "_")

        'If file exists, then add counter to the filename
        Dim OriginalName As String = System.IO.Path.GetFileNameWithoutExtension(FileName)
        Dim OriginalExtension As String = System.IO.Path.GetExtension(FileName)
        Dim tmpName As String = OriginalName & OriginalExtension
        Dim iCounter As Integer = 1
        While System.IO.File.Exists(Server.MapPath("/assets/catalog/" & tmpName))
            tmpName = OriginalName & iCounter & OriginalExtension
            iCounter += 1
        End While
        tmpName = Replace(Trim(tmpName), " ", "_")
        tmpName = Replace(Trim(tmpName), "&", "")

        'Save File    
        myFile.SaveAs(Server.MapPath("/assets/catalog/temp" & Extension))
        Core.ResizeImage(Server.MapPath("/assets/catalog/temp" & Extension), Server.MapPath("/assets/catalog/" & tmpName), SysParam.GetValue("CatalogImageHeight"), SysParam.GetValue("CatalogImageWidth"))
        Core.ResizeImage(Server.MapPath("/assets/catalog/temp" & Extension), Server.MapPath("/assets/catalog/small/" & tmpName), SysParam.GetValue("CatalogSmallImageHeight"), SysParam.GetValue("CatalogSmallImageWidth"))
        System.IO.File.Delete(Server.MapPath("/assets/catalog/temp" & Extension))

        Return tmpName
    End Function

    Private Sub Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Save.Click
        If Not Page.IsValid Then Exit Sub

        Try
            DB.BeginTransaction()

            If CatalogId <> 0 Then
                Dim dbCatalog As StoreCatalogRow = StoreCatalogRow.GetRow(DB, CatalogId)
                dbCatalog.Title = txtTitle.Text
                dbCatalog.IsActive = IsActive.Checked
                dbCatalog.RichFXLink = txtLink.Text
                dbCatalog.ImageAltTag = txtImageAltTag.Text
                If Image.PostedFile.ContentLength <> 0 Then dbCatalog.CatalogImage = SaveCatalogImage()

                dbCatalog.Update()
            Else
                Dim dbCatalog As StoreCatalogRow = New StoreCatalogRow(DB)
                dbCatalog.Title = txtTitle.Text
                dbCatalog.IsActive = IsActive.Checked
                dbCatalog.RichFXLink = txtLink.Text
                dbCatalog.ImageAltTag = txtImageAltTag.Text
                If Image.PostedFile.ContentLength <> 0 Then dbCatalog.CatalogImage = SaveCatalogImage()
                dbCatalog.Insert()
            End If
            DB.CommitTransaction()
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As ApplicationException
            DB.RollbackTransaction()
            AddError(ex.Message)
        Catch ex As SqlException
            DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        Finally
        End Try
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Delete.Click
        If CatalogId <> 0 Then
            Response.Redirect("delete.aspx?CatalogId=" & CatalogId & "&" & GetPageParams(FilterFieldType.All))
        End If
    End Sub
End Class