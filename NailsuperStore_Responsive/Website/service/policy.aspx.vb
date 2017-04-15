Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System

Partial Class Policy_Detail
    Inherits SitePage
    '-------------------------------------------------------------------
    ' VARIABLES
    '-------------------------------------------------------------------
    Public objPolicy As PolicyRow
    Public PolicyId As Integer
    Private ReviewType As Integer = Utility.Common.ReviewType.News
    '-------------------------------------------------------------------
    ' METHODS
    '-------------------------------------------------------------------
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Request.QueryString("id") <> Nothing AndAlso IsNumeric(Request.QueryString("id")) Then
            PolicyId = CInt(Request.QueryString("id"))
        Else
            Response.Redirect("/")
        End If

        If Not IsPostBack Then
            LoadDetail()
        End If

    End Sub
    
    Private Sub LoadDetail()
        objPolicy = PolicyRow.GetRow(PolicyId)
        If Not objPolicy Is Nothing AndAlso Not String.IsNullOrEmpty(objPolicy.Title) Then
            litTitle.Text = Server.HtmlEncode(objPolicy.Title)
            Dim strNow As String = DateTime.Now.ToString("yyyy-MM-dd") & "T" & DateTime.Now.ToString("HH:mm:ss")
            divContent.InnerHtml = objPolicy.Content.Replace("#CountDown#", "").Replace("#Now#", strNow)
            SetSocialNetwork(objPolicy)
        Else
            divContent.InnerHtml = Resources.Msg.PolicyNotFound
        End If
    End Sub

    Private Sub SetSocialNetwork(ByVal objItem As PolicyRow)
        Dim objMetaTag As New MetaTag
        'objMetaTag.MetaKeywords = objItem.MetaKeyword
        objMetaTag.PageTitle = objItem.Title
        objMetaTag.MetaDescription = objItem.Message

        Dim shareURL As String = GlobalSecureName & URLParameters.PolicyUrl(objItem.PolicyId)
        Dim shareTitle As String = objItem.Title
        Dim shareDesc As String = Utility.Common.RemoveHTMLTag(objItem.Message)

        objMetaTag.Canonical = shareURL

        SetPageMetaSocialNetwork(Page, objMetaTag)
    End Sub

End Class
