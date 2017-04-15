Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_classifieds_Edit
    Inherits AdminPage

    Protected ClassifiedId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        ClassifiedId = Convert.ToInt32(Request("ClassifiedId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        drpClassifiedCategoryId.Datasource = ClassifiedCategoryRow.GetAllClassifiedCategories(DB)
        drpClassifiedCategoryId.DataValueField = "ClassifiedCategoryId"
        drpClassifiedCategoryId.DataTextField = "Category"
        drpClassifiedCategoryId.Databind()
        drpClassifiedCategoryId.Items.Insert(0, New ListItem("", ""))

        If ClassifiedId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbClassified As ClassifiedRow = ClassifiedRow.GetRow(DB, ClassifiedId)
        txtTitle.Text = dbClassified.Title
        txtDescription.Text = dbClassified.Description
        txtContactName.Text = dbClassified.ContactName
        txtContactNumber.Text = dbClassified.ContactNumber
        txtEmail.Text = dbClassified.Email
        dtExpirationDate.Value = dbClassified.ExpirationDate
        drpClassifiedCategoryId.SelectedValue = dbClassified.ClassifiedCategoryId
        fuPhoto0.CurrentFileName = dbClassified.Photo0
        fuPhoto1.CurrentFileName = dbClassified.Photo1
        fuPhoto2.CurrentFileName = dbClassified.Photo2
        fuPhoto3.CurrentFileName = dbClassified.Photo3
        fuPhoto4.CurrentFileName = dbClassified.Photo4
        fuPhoto5.CurrentFileName = dbClassified.Photo5
        chkIsActive.Checked = dbClassified.IsActive
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbClassified As ClassifiedRow

            If ClassifiedId <> 0 Then
                dbClassified = ClassifiedRow.GetRow(DB, ClassifiedId)
            Else
                dbClassified = New ClassifiedRow(DB)
            End If
            dbClassified.Title = txtTitle.Text
            dbClassified.Description = txtDescription.Text
            dbClassified.ContactName = txtContactName.Text
            dbClassified.ContactNumber = txtContactNumber.Text
            dbClassified.Email = txtEmail.Text
            dbClassified.ExpirationDate = dtExpirationDate.Value
            dbClassified.ClassifiedCategoryId = drpClassifiedCategoryId.SelectedValue
            If fuPhoto0.NewFileName <> String.Empty Then
                fuPhoto0.SaveNewFile()
                dbClassified.Photo0 = fuPhoto0.NewFileName
                Core.ResizeImage(Server.MapPath(fuPhoto0.Folder) & fuPhoto0.NewFileName, Server.MapPath(fuPhoto0.Folder) & fuPhoto0.NewFileName, 300, 225)
            ElseIf fuPhoto0.MarkedToDelete Then
                dbClassified.Photo0 = Nothing
            End If
            If fuPhoto1.NewFileName <> String.Empty Then
                fuPhoto1.SaveNewFile()
                dbClassified.Photo1 = fuPhoto1.NewFileName
                Core.ResizeImage(Server.MapPath(fuPhoto1.Folder) & fuPhoto1.NewFileName, Server.MapPath(fuPhoto1.Folder) & fuPhoto1.NewFileName, 300, 225)
            ElseIf fuPhoto1.MarkedToDelete Then
                dbClassified.Photo1 = Nothing
            End If
            If fuPhoto2.NewFileName <> String.Empty Then
                fuPhoto2.SaveNewFile()
                dbClassified.Photo2 = fuPhoto2.NewFileName
                Core.ResizeImage(Server.MapPath(fuPhoto2.Folder) & fuPhoto2.NewFileName, Server.MapPath(fuPhoto2.Folder) & fuPhoto2.NewFileName, 300, 225)
            ElseIf fuPhoto2.MarkedToDelete Then
                dbClassified.Photo2 = Nothing
            End If
            If fuPhoto3.NewFileName <> String.Empty Then
                fuPhoto3.SaveNewFile()
                dbClassified.Photo3 = fuPhoto3.NewFileName
                Core.ResizeImage(Server.MapPath(fuPhoto3.Folder) & fuPhoto3.NewFileName, Server.MapPath(fuPhoto3.Folder) & fuPhoto3.NewFileName, 300, 225)
            ElseIf fuPhoto3.MarkedToDelete Then
                dbClassified.Photo3 = Nothing
            End If
            If fuPhoto4.NewFileName <> String.Empty Then
                fuPhoto4.SaveNewFile()
                dbClassified.Photo4 = fuPhoto4.NewFileName
                Core.ResizeImage(Server.MapPath(fuPhoto4.Folder) & fuPhoto4.NewFileName, Server.MapPath(fuPhoto4.Folder) & fuPhoto4.NewFileName, 300, 225)
            ElseIf fuPhoto4.MarkedToDelete Then
                dbClassified.Photo4 = Nothing
            End If
            If fuPhoto5.NewFileName <> String.Empty Then
                fuPhoto5.SaveNewFile()
                dbClassified.Photo5 = fuPhoto5.NewFileName
                Core.ResizeImage(Server.MapPath(fuPhoto5.Folder) & fuPhoto5.NewFileName, Server.MapPath(fuPhoto5.Folder) & fuPhoto5.NewFileName, 300, 225)
            ElseIf fuPhoto5.MarkedToDelete Then
                dbClassified.Photo5 = Nothing
            End If

            If Not dbClassified.IsActive AndAlso chkIsActive.Checked Then
                Core.SendSimpleMail("info@nss.com", "info@nss.com", dbClassified.Email, dbClassified.Email, "Nail Superstore: Classified Approved", "The classified you recently posted, '" & dbClassified.Title & "', has been approved for listing." & vbCrLf & vbCrLf & "Thank you," & vbCrLf & "The Nail Superstore" & vbCrLf & "http://www.nss.com/")
            End If
            dbClassified.IsActive = chkIsActive.Checked

            If ClassifiedId <> 0 Then
                dbClassified.Update()
            Else
                ClassifiedId = dbClassified.Insert
            End If

            DB.CommitTransaction()

            If fuPhoto0.NewFileName <> String.Empty OrElse fuPhoto0.MarkedToDelete Then fuPhoto0.RemoveOldFile()
            If fuPhoto1.NewFileName <> String.Empty OrElse fuPhoto1.MarkedToDelete Then fuPhoto1.RemoveOldFile()
            If fuPhoto2.NewFileName <> String.Empty OrElse fuPhoto2.MarkedToDelete Then fuPhoto2.RemoveOldFile()
            If fuPhoto3.NewFileName <> String.Empty OrElse fuPhoto3.MarkedToDelete Then fuPhoto3.RemoveOldFile()
            If fuPhoto4.NewFileName <> String.Empty OrElse fuPhoto4.MarkedToDelete Then fuPhoto4.RemoveOldFile()
            If fuPhoto5.NewFileName <> String.Empty OrElse fuPhoto5.MarkedToDelete Then fuPhoto5.RemoveOldFile()

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
        Response.Redirect("delete.aspx?ClassifiedId=" & ClassifiedId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

