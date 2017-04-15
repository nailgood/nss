Imports System.IO
Imports System.Text
Imports System.Net
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports Twitter
Imports Utility
Public Class admin_FacebookPage_Default
    Inherits AdminPage
    Dim ToTal As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then

            F_Title.Text = Request("F_Name")
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "Arrange"
            If gvList.SortOrder = String.Empty Then gvList.SortBy = "ASC"
            ' BindList()
            btnSearch_Click(sender, e)
        End If
    End Sub

    Private Sub BindQuery()
        hidId.Value = ""
        Dim SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQL = " FROM FacebookPage p "
        If Not F_Title.Text = String.Empty Then
            SQL = SQL & Conn & "p.PageTitle LIKE " & DB.FilterQuote(F_Title.Text)
            Conn = " AND "
        End If
        hidCon.Value = SQL
    End Sub
    Private Sub BindList()
        Dim SQLFields As String = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " PageId,Link,Thumb,PageTitle,MetaDescription"
        ToTal = DB.ExecuteScalar("SELECT COUNT(*) " & hidCon.Value)
        gvList.Pager.NofRecords = ToTal

        Dim res As DataTable = DB.GetDataTable(SQLFields & hidCon.Value & " ORDER BY PageId DESC")
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub

    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindQuery()
        BindList()
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim pageTitle As String = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("PageTitle")
            Dim pageId As String = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("PageId")
            Dim txtLink As TextBox = CType(e.Row.FindControl("txtLink"), TextBox)
            Dim url As String = ConfigData.GlobalRefererName & URLParameters.FaceBookPageUrl(pageTitle, pageId)
            txtLink.Text = url
            hidId.Value = hidId.Value & pageId & ","
        End If
    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "Delete" Then
            Dim objPage As FacebookPageRow = FacebookPageRow.GetRow(DB, e.CommandArgument)
            If FacebookPageRow.Delete(DB, e.CommandArgument) Then
                Dim ImagePath As String = Server.MapPath("~/" & Utility.ConfigData.SharefaceBookPath)
                Utility.File.DeleteFile(ImagePath & objPage.Thumb)
            End If
        End If

        Response.Redirect("Default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
    Private Sub PostFaceBook_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PostFaceBook.Click
        GetListPagePostToFaceBook()
    End Sub
    Private Sub GetListPagePostToFaceBook()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "
        SQLFields = "SELECT Link,Thumb,PageTitle,MetaDescription"
        SQL = " FROM FacebookPage p "
        SQL = SQL & Conn & "p.PageId in(" & hidSelect.Value.Substring(0, hidSelect.Value.Length - 1) & ")"
        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL)
        If Not res Is Nothing Then
            GetListPagePostToFaceBook(res)
        End If
    End Sub
    Private Sub PostLinkToFaceBook(ByVal objData As FacebookPageRow, ByVal pageToken As String)
        Dim data As New System.Collections.Generic.Dictionary(Of String, String)()
        data.Add("message", objData.PageTitle)
        data.Add("link", objData.Link)
        data.Add("picture", objData.Thumb)
        data.Add("description", objData.MetaDescription)
        
        ''post to page
        Try
            Dim pageFBAPI As New Facebook.FacebookAPI(pageToken)
            pageFBAPI.Post("/" & Utility.ConfigData.FaceBookPageId & "/feed", data)
        Catch ex As Exception

        End Try
    End Sub


    Private Sub GetListPagePostToFaceBook(ByVal dtSour As DataTable)
        Dim objTwitterToken As New OAuthData(Utility.ConfigData.TwitterConsumerKey, Utility.ConfigData.TwitterConsumerSecret, Utility.ConfigData.TwitterUserID, Utility.ConfigData.TwitterScreenName, Utility.ConfigData.TwitterToken, Utility.ConfigData.TwitterTokenSecret)

        Dim pagetoken As String = Utility.ConfigData.FaceBookPageAccessToken
        If (String.IsNullOrEmpty(pagetoken)) Then
            Exit Sub
        End If
        For Each row As DataRow In dtSour.Rows
            Dim objData As New FacebookPageRow
            objData.Link = row("Link").ToString()
            objData.PageTitle = row("PageTitle").ToString()
            objData.Thumb = ConfigData.GlobalRefererName & "/" & ConfigData.SharefaceBookPath & row("Thumb").ToString()
            objData.MetaDescription = row("MetaDescription").ToString()
            PostLinkToFaceBook(objData, pagetoken)
            PostToTwitter(objTwitterToken, objData)
        Next
        Response.Redirect("Default.aspx")
    End Sub
    Private Sub PostToTwitter(ByVal objTwitterToken As OAuthData, ByVal objData As FacebookPageRow)
        Try
            Dim twitter As New Twitter.TwitterPush(objTwitterToken)
            Dim response As Twitter.TwitterResponse = twitter.UpdateStatus(objData.PageTitle & " " & objData.Link)
        Catch ex As Exception

        End Try
    End Sub

End Class



