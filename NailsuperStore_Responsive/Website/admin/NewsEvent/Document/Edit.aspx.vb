

Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Partial Class admin_NewsEvent_Document_Edit
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
        Dim dbDocument As DocumentRow = DocumentRow.GetRow(DB, Id)
        fuDocument.Folder = "~" & Utility.ConfigData.PathNewDocument
        txtName.Text = dbDocument.DocumentName
        chkIsActive.Checked = dbDocument.IsActive
        fuDocument.CurrentFileName = dbDocument.FileName
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Page.IsValid Then
            Try

                Dim dbDocumentRow As DocumentRow
                Dim oldDocument As String = ""
                If Id <> 0 Then
                    dbDocumentRow = DocumentRow.GetRow(DB, Id)
                    oldDocument = dbDocumentRow.FileName
                Else
                    dbDocumentRow = New DocumentRow(DB)
                End If
                dbDocumentRow.DocumentName = txtName.Text.Trim()
                dbDocumentRow.IsActive = chkIsActive.Checked
                fuDocument.Folder = "~" & Utility.ConfigData.PathNewDocument
                Dim newDocumentName As String
                Dim DocumentPath As String = Server.MapPath("~" & Utility.ConfigData.PathNewDocument)
                If fuDocument.NewFileName <> String.Empty Then
                    ''Delete Old File
                    Utility.File.DeleteFile(DocumentPath & oldDocument)
                    newDocumentName = Guid.NewGuid().ToString() & fuDocument.OriginalExtension
                    fuDocument.NewFileName = newDocumentName
                    fuDocument.SaveNewFile()
                    dbDocumentRow.FileName = newDocumentName
                ElseIf fuDocument.MarkedToDelete Then
                    dbDocumentRow.FileName = Nothing
                End If
                If Id <> 0 Then
                    If Not DocumentRow.Update(DB, dbDocumentRow) And fuDocument.NewFileName <> String.Empty Then
                        Utility.File.DeleteFile(DocumentPath & newDocumentName)
                    End If
                Else
                    If Not (DocumentRow.Insert(DB, dbDocumentRow) And fuDocument.NewFileName <> String.Empty) Then
                        Utility.File.DeleteFile(DocumentPath & newDocumentName)
                    End If
                End If
                If fuDocument.MarkedToDelete Then
                    Utility.File.DeleteFile(DocumentPath & oldDocument)
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

