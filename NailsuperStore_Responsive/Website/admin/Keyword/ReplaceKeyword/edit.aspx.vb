


Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_Keyword_ReplaceKeyword_edit
    Inherits AdminPage

    Protected params As String


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        params = GetPageParams(FilterFieldType.All)
        hidKeywordId.Value = Request("KeywordId")
        If Not IsPostBack Then
           
            BindDataGrid()
        End If
    End Sub
    Dim total As Integer = 0
    Private Sub BindDataGrid()

        If Not IsPostBack Then

            ViewState("F_PG") = Request("F_PG")
            If CType(ViewState("F_PG"), String) = String.Empty Then
                ViewState("F_PG") = 1
            End If

            Dim objKeyword As KeywordRow = KeywordRow.GetRow(DB, hidKeywordId.Value)
            If Not (objKeyword Is Nothing) Then
                lblItemName.Text = objKeyword.KeywordName ''DB.ExecuteScalar("select KeywordName from Keyword where KeywordId=" & hidKeywordId.Value)
                Dim replaceKeyword As String = KeywordRow.GetReplaceKeyword(objKeyword.KeywordName)
                If Not String.IsNullOrEmpty(replaceKeyword) Then
                    Me.Page.ClientScript.RegisterStartupScript(Me.GetType(), "resetKeyword", "ResetKeyword('" & KeywordRow.GetReplaceKeyword(objKeyword.KeywordName) & "');", True)
                End If

                ''KeywordRow.GetReplaceKeyword(objKeyword.KeywordName)

            End If




        End If

    End Sub


    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If String.IsNullOrEmpty(Request.Form("LookupField")) Then
            AddError("Please do keyword search first")
        Else
            Try

                Dim result As Integer = KeywordRow.InsertReplaceKeyword(hidKeywordId.Value, Request.Form("LookupField"))

                If (result = -1) Then
                    AddError("Keyword is not exits")
                    Me.Page.ClientScript.RegisterStartupScript(Me.GetType(), "resetKeyword", "ResetKeyword('" & Request.Form("LookupField") & "');", True)
                Else
                    Response.Redirect("default.aspx?" & params)
                End If


            Catch ex As SqlException
                AddError(ErrHandler.ErrorText(ex))
            Catch ex As ApplicationException
                AddError(ex.Message)
            End Try
        End If


    End Sub

 

    
End Class