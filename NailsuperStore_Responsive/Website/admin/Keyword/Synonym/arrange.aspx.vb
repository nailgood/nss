



Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_Keyword_Synonym_arrange
    Inherits AdminPage

    Protected params As String
    Public Property SynonymType() As String
        Get
            Return ViewState("SynonymType")
        End Get
        Set(ByVal value As String)
            ViewState("SynonymType") = value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        params = GetPageParams(FilterFieldType.All)
        SynonymType = GetQueryString("SynonymType")
        If Not IsPostBack Then
           
            If Not Request("KeywordId") Is Nothing Then
                hidKeywordId.Value = Request("KeywordId")
            End If
            If Not String.IsNullOrEmpty(hidKeywordId.Value) Then
                ltrKeywordName.Text = DB.ExecuteScalar("select KeywordName from Keyword where KeywordId=" & hidKeywordId.Value)
            End If
            BindData(ltrKeywordName.Text)
        End If
    End Sub
    Dim total As Integer = 0

    Private Sub BindData(ByVal mainKeyword As String)
        gvList.Visible = False
        If String.IsNullOrEmpty(mainKeyword) Then

            Exit Sub
        End If
      
        Dim lstresult As KeywordSynonymCollection = KeywordSynonymRow.GetListKeywordSynonym(mainKeyword)
        If Not lstresult Is Nothing Then
            total = lstresult.Count
        End If
        gvList.DataSource = lstresult
        gvList.DataBind()
        If (total > 0) Then
            gvList.Visible = True
        End If
    End Sub

    
   
   

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim objKeyword As KeywordSynonymRow = e.Row.DataItem
            Dim keywordId As Integer = objKeyword.KeywordId
            Dim imbUp As ImageButton = CType(e.Row.FindControl("imbUp"), ImageButton)
            
            ''chkIsTwoWay.
            Dim imbDown As ImageButton = CType(e.Row.FindControl("imbDown"), ImageButton)
            If (total < 2) Then
                imbUp.Visible = False
                imbDown.Visible = False
            Else
                imbUp.CommandArgument = keywordId
                imbDown.CommandArgument = keywordId
                If e.Row.DataItemIndex = 0 Then
                    imbUp.Visible = False
                ElseIf e.Row.DataItemIndex = total - 1 Then
                    imbDown.Visible = False
                End If
            End If

            Dim ltrDelete As Literal = CType(e.Row.FindControl("ltrDelete"), Literal)
            If Not ltrDelete Is Nothing Then
                ltrDelete.Text = "<a href='javascript:void(0);' onclick='return Delete(" & hidKeywordId.Value & "," & keywordId & ");'><img src='/includes/theme-admin/images/delete.gif'/></a>"
            End If

        End If
    End Sub
   
    

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand

        Try
            Dim id As Integer = Convert.ToInt32(e.CommandArgument)
            If e.CommandName = "Down" Then
                KeywordSynonymRow.ChangeArrange(hidKeywordId.Value, id, True)
            ElseIf (e.CommandName = "Up") Then
                KeywordSynonymRow.ChangeArrange(hidKeywordId.Value, id, False)
            End If
        Catch ex As Exception

        End Try

        Response.Redirect(Me.Request.RawUrl)
    End Sub
End Class