﻿
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Partial Class admin_NewsEvent_News_PopupImage
    Inherits AdminPage
    Dim idSelect As String = String.Empty
    Public Property Type() As String

        Get
            Dim o As Object = ViewState("Type")
            If o IsNot Nothing Then
                Return DirectCast(o, String)
            End If
            Return String.Empty
        End Get

        Set(ByVal value As String)
            ViewState("Type") = value
        End Set
    End Property
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        idSelect = ";" + Request("item")
        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "Imageid"
            BindList()
        End If
    End Sub

    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "
        hidImageSelect.Value = String.Empty
        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        SQL = " FROM Image img "
        If Not F_Name.Text = String.Empty Then
            SQL = SQL & Conn & "ImageName LIKE " & DB.FilterQuote(F_Name.Text)
            Conn = " AND "
        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "IsActive  = " & DB.Number(F_IsActive.SelectedValue)
            Conn = " AND "
        End If
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)

        Dim res As DataSet = DB.GetDataSet(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.Tables(0).DefaultView
        gvList.DataBind()
    End Sub


    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Dim url As String = DirectCast(DirectCast(Request, System.Web.HttpRequest).Url, System.Uri).AbsoluteUri
        Response.Redirect(url)
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindList()
    End Sub

    

    Private Sub gvList_ItemDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvList.RowDataBound
        If Not (e.Row.RowType = DataControlRowType.DataRow) Then
            Exit Sub
        End If
        Dim id As String = e.Row.DataItem("ImageId")
        Dim chk As CheckBox = CType(e.Row.FindControl("chkImageId"), CheckBox)
        chk.Attributes.Add("onclick", "CheckItem('" + id + "',this.checked);")
        If idSelect.Contains(";" + id + ";") Then
            If Not chk Is Nothing Then
                chk.Checked = True
                chk.Enabled = False
            End If
        End If
        Dim imbIcon As Literal = CType(e.Row.FindControl("litImage"), Literal)
        If Not imbIcon Is Nothing Then
            imbIcon.Text = "<img src='/" & Utility.ConfigData.PathSmallNewsImage & e.Row.DataItem("FileName") & "'/>"
        End If

    End Sub
   
End Class