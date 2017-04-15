Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.IO

Partial Class admin_NewsEvent_Image_Default
    Inherits AdminPage
    Dim ToTal As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        gvList.BindList = AddressOf BindListData

        If Not IsPostBack Then
            F_Name.Text = Request("F_Name")
            F_IsActive.Text = Request("F_IsActive")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "ImageName"
            If gvList.SortOrder = String.Empty Then gvList.SortBy = "ASC"

            BindListData()
        End If
    End Sub

    Private Sub BindListData()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " ImageId,ImageName,FileName,IsActive"
        SQL = " FROM Image i "

        If Not F_Name.Text = String.Empty Then
            SQL = SQL & Conn & "i.ImageName LIKE " & DB.FilterQuote(F_Name.Text)
            Conn = " AND "
        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "i.IsActive  = " & DB.Number(F_IsActive.SelectedValue)
            Conn = " AND "
        End If
        ToTal = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        gvList.Pager.NofRecords = ToTal

        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim active As Boolean = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("IsActive")
            Dim fileName As String = String.Empty
            Try
                fileName = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("FileName")
            Catch ex As Exception

            End Try

            Dim imbActive As ImageButton = CType(e.Row.FindControl("imbActive"), ImageButton)

            If active Then
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If
            Dim imbIcon As Literal = CType(e.Row.FindControl("litImage"), Literal)
            If Not imbIcon Is Nothing Then
                imbIcon.Text = "<img src='/" & Utility.ConfigData.PathSmallNewsImage & fileName & "'/>"
            End If
        End If

    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "Delete" Then
            Response.Redirect("Delete.aspx?Id=" & e.CommandArgument & "&" & GetPageParams(FilterFieldType.All))
        ElseIf e.CommandName = "Active" Then
            ImageRow.ChangeIsActive(DB, e.CommandArgument)
        End If
        BindListData()
    End Sub

   
    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindListData()
    End Sub
End Class


