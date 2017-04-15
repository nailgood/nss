

Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_Keyword_Synonym_Edit
    Inherits AdminPage

    Private m_params As String
    Public Property params() As String
        Get
            Return ViewState("Params")
        End Get
        Set(ByVal value As String)
            ViewState("Params") = value
        End Set
    End Property

    Public Property SynonymType() As String
        Get
            Return ViewState("synonymType")
        End Get
        Set(ByVal value As String)
            ViewState("synonymType") = value
        End Set
    End Property


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        SynonymType = GetQueryString("SynonymType")
        params = IIf(String.IsNullOrEmpty(SynonymType), String.Empty, "SynonymType=" + SynonymType)
        If Not IsPostBack Then
           If Not Request("F_Keyword") Is Nothing Then
            hidF_KeywordName.Value = Request("F_Keyword")
        End If
        If Not Request("KeywordId") Is Nothing Then
            hidKeywordId.Value = Request("KeywordId")

        End If

            If String.IsNullOrEmpty(SynonymType) Then
                ddlTone.Visible = False
                rfvMainKeywordDdl.Visible = False
                txtMainKeyword.Visible = True
                rfvMainKeyword.Visible = True
                If Not String.IsNullOrEmpty(hidKeywordId.Value) Then
                    txtMainKeyword.Text = DB.ExecuteScalar("select KeywordName from Keyword where KeywordId=" & hidKeywordId.Value)
                Else
                    txtMainKeyword.Text = hidF_KeywordName.Value
                End If
                If Not String.IsNullOrEmpty(txtMainKeyword.Text) Then
                    hidReset.Value = "1"
                End If
            ElseIf SynonymType = "tone"
                txtRoundTrip.Enabled = False

                ddlTone.Visible = True
                rfvMainKeywordDdl.Visible = True
                txtMainKeyword.Visible = False
                rfvMainKeyword.Visible = False

                If String.IsNullOrEmpty(hidKeywordId.Value) Then
                    ddlTone.DataSource = DB.GetDataTable("select Tone from StoreTone a where not exists (select 1 from KeywordSynonym b inner join Keyword c on b.KeywordId = c.KeywordId where b.SynonymType = 'tone' and a.Tone = c.KeywordName)")
                Else
                    ddlTone.DataSource = DB.GetDataTable("select Tone from StoreTone a where exists (select 1 from KeywordSynonym b inner join Keyword c on b.KeywordId = c.KeywordId and c.KeywordId = " & hidKeywordId.Value & " where b.SynonymType = 'tone' and a.Tone = c.KeywordName)")
                End If

                ddlTone.DataValueField = "Tone"
                ddlTone.DataTextField = "Tone"
                ddlTone.DataBind()
            ElseIf SynonymType = "buyinbulk"
                txtRoundTrip.Enabled = False
                ddlTone.Visible = False
                rfvMainKeywordDdl.Visible = True
                txtMainKeyword.Visible = True
                txtMainKeyword.Enabled = False
                txtMainKeyword.Text = "Buy In Bulk Main Keyword"
                rfvMainKeyword.Visible = True

            End If

            BindData(IIf(SynonymType <> "tone", txtMainKeyword.Text, ddlTone.SelectedValue))
            End If
    End Sub
    Dim total As Integer = 0
    
    Private Sub BindData(ByVal mainKeyword As String)

        If String.IsNullOrEmpty(mainKeyword) Then
            Exit Sub
        End If

        txtOneWay.Text = String.Empty
        txtRoundTrip.Text = String.Empty
        Dim lstresult As KeywordSynonymCollection = KeywordSynonymRow.GetListKeywordSynonym(mainKeyword, synonymType)
        If Not lstresult Is Nothing Then
            total = lstresult.Count
            For Each objKeyword As KeywordSynonymRow In lstresult
                KeywordRowDataBound(objKeyword)
            Next
        End If
      
    End Sub

    Private Function GetListKeywordSynonym(ByVal source As String, ByVal isRoundTrip As Boolean) As KeywordSynonymCollection
        If String.IsNullOrEmpty(source) Then
            Return Nothing
        End If
        Dim replaceChar As String = "|"
        source = source.Replace(Environment.NewLine, replaceChar)
        Dim lstresult As New KeywordSynonymCollection
        If Not source.Contains(replaceChar) Then
            Dim objKeyword As New KeywordSynonymRow
            '' objKeyword.KeywordId = hidKeywordId.Value
            objKeyword.KeywordSynonymName = source
            objKeyword.IsRoundTrip = isRoundTrip
            lstresult.Add(objKeyword)
        Else
            Dim arr() As String = source.Split(replaceChar)
            If (arr.Length > 0) Then
                For Each subKeyword As String In arr
                    If Not String.IsNullOrEmpty(subKeyword) Then
                        If Not subKeyword.Equals(replaceChar) Then
                            Dim objKeyword As New KeywordSynonymRow
                            '' objKeyword.KeywordId = hidKeywordId.Value
                            objKeyword.KeywordSynonymName = subKeyword.Trim()
                            objKeyword.IsRoundTrip = isRoundTrip
                            lstresult.Add(objKeyword)
                        End If

                    End If
                Next
            End If
        End If
        Return lstresult
    End Function
    Private Function ValidateKeyword(ByVal lstOneWayKeyword As KeywordSynonymCollection, ByVal lstRoundTripKeyword As KeywordSynonymCollection) As String
        Dim countOneWay As Integer = 0
        Dim countRoundTrip As Integer = 0
        If Not lstOneWayKeyword Is Nothing Then
            countOneWay = lstOneWayKeyword.Count
        End If
        If Not lstRoundTripKeyword Is Nothing Then
            countRoundTrip = lstRoundTripKeyword.Count
        End If
        If countOneWay < 1 AndAlso countRoundTrip < 1 Then
            Return "Field 'One way keyword' and ' Round Trip keyword' is blank"
        End If
        Dim result As String = String.Empty
        If Not lstOneWayKeyword Is Nothing Then
            For Each objOneway As KeywordSynonymRow In lstOneWayKeyword
                If (objOneway.KeywordSynonymName.Trim().ToLower() = txtMainKeyword.Text.Trim().ToLower()) Then
                    result = "Keyword '" & objOneway.KeywordSynonymName & "' is the same as main keyword"
                    Return result
                End If
            Next
        End If
        If Not lstRoundTripKeyword Is Nothing Then
            For Each objRound As KeywordSynonymRow In lstRoundTripKeyword
                If (objRound.KeywordSynonymName.Trim().ToLower() = txtMainKeyword.Text.Trim().ToLower()) Then
                    result = "Keyword '" & objRound.KeywordSynonymName & "' is the same as main keyword"
                    Return result
                End If
                If Not lstOneWayKeyword Is Nothing Then
                    For Each objOneway As KeywordSynonymRow In lstOneWayKeyword
                        If (objOneway.KeywordSynonymName.Trim().ToLower() = objRound.KeywordSynonymName.Trim().ToLower()) Then
                            result = "Keyword '" & objRound.KeywordSynonymName & "' is exists in Field 'One way keyword'"
                            Return result
                        End If
                    Next
                End If
            Next
        End If
        Return result
    End Function
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If String.IsNullOrEmpty(txtRoundTrip.Text) AndAlso String.IsNullOrEmpty(txtOneWay.Text) Then
            AddError("Field 'One way keyword' and ' Round Trip keyword' is blank")
        Else

            Try
                Dim lstOneWayKeyword As KeywordSynonymCollection = GetListKeywordSynonym(txtOneWay.Text, False)
                Dim lstRoundTripKeyword As KeywordSynonymCollection = GetListKeywordSynonym(txtRoundTrip.Text, True)
                Dim msg As String = ValidateKeyword(lstOneWayKeyword, lstRoundTripKeyword)
                If Not String.IsNullOrEmpty(msg) Then
                    AddError(msg)
                    Exit Sub
                End If
                Dim objMainKeyword As New KeywordRow
                If Not String.IsNullOrEmpty(SynonymType) AndAlso SynonymType = "tone" Then
                    objMainKeyword.KeywordName = ddlTone.SelectedValue
                Else
                    objMainKeyword.KeywordName = txtMainKeyword.Text.Trim()
                End If
                ' objMainKeyword.KeywordName = IIf(Not String.IsNullOrEmpty(SynonymType), txtMainKeyword.Text.Trim(), ddlTone.SelectedValue)
                objMainKeyword.KeywordId = DB.ExecuteScalar("Select KeywordId from Keyword where KeywordName='" & objMainKeyword.KeywordName & "'")
                Dim result As Integer = KeywordSynonymRow.InsertListKeywordSynonym(objMainKeyword, lstOneWayKeyword, lstRoundTripKeyword, synonymType)
                If (result > 0) Then ''successfull
                    '' BindData(objMainKeyword.KeywordName)
                    Utility.CacheUtils.RemoveCache("KeywordSynonym_Synonym")
                    Response.Redirect("default.aspx?" & params)

                End If

                ''  Response.Redirect(Me.Request.RawUrl)

            Catch ex As SqlException
                AddError(ErrHandler.ErrorText(ex))
            Catch ex As ApplicationException
                AddError(ex.Message)
            End Try
        End If

    End Sub

    Protected Sub KeywordRowDataBound(ByVal objKeyword As KeywordSynonymRow)


        Dim keywordId As Integer = objKeyword.KeywordId

        If (objKeyword.IsRoundTrip) Then

            BindKeyword(txtRoundTrip, objKeyword.KeywordSynonymName)
        Else
            BindKeyword(txtOneWay, objKeyword.KeywordSynonymName)
        End If



    End Sub
    Private Sub BindKeyword(ByRef txt As TextBox, ByVal keyword As String)
        If String.IsNullOrEmpty(keyword) Then
            Return
        End If
        keyword.Replace(Environment.NewLine, "")
        If (String.IsNullOrEmpty(txt.Text)) Then
            txt.Text = keyword
        Else
            txt.Text = txt.Text & Environment.NewLine & keyword
        End If
    End Sub
   
End Class