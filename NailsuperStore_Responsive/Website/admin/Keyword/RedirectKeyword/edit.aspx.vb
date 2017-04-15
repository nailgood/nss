

Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.IO
Partial Class admin_Keyword_RedirectKeyword_edit
    Inherits AdminPage
    Private KeywordId As Integer = 0
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Request.QueryString("id") Then
            KeywordId = Request.QueryString("id")
        End If
       
        If Not Page.IsPostBack Then
            LoadDB()
        End If
    End Sub
    Private Sub LoadDB()
        If KeywordId > 0 Then
            Dim objKeyword As KeywordRedirectRow = KeywordRedirectRow.GetRow(KeywordId)
            If Not (objKeyword Is Nothing) Then
                txtName.Text = objKeyword.KeywordName.Trim()
                txtLink.Text = objKeyword.LinkRedirect.Trim()
                txtDescription.Text = objKeyword.Description
            End If
        End If
    End Sub

    Private Sub Back()
        Response.Redirect("default.aspx")
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Page.IsValid Then
           
            Dim objkeyword As KeywordRedirectRow
            If (KeywordId > 0) Then
                objkeyword = KeywordRedirectRow.GetRow(KeywordId)
            Else
                objkeyword = New KeywordRedirectRow
            End If
            objkeyword.KeywordName = txtName.Text.Trim
            objkeyword.LinkRedirect = txtLink.Text.Trim
            objkeyword.Description = txtDescription.Text.Trim()
            If KeywordId > 0 Then
                objkeyword.Id = KeywordId
                KeywordRedirectRow.Update(objkeyword)
            Else
                KeywordId = KeywordRedirectRow.Insert(objkeyword)
            End If
           
            Back()
        End If
      
    End Sub
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Back()

    End Sub
End Class
