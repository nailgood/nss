Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_admins_Index
    Inherits AdminPage

    Protected params As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            F_Login.Text = Request("F_Login")
            F_FirstName.Text = Request("F_FirstName")
            F_LastName.Text = Request("F_LastName")
            F_IsActive.Text = Request("F_IsActive")

            'BindRepeater()
            btnSearch_Click(sender, e)
        End If
    End Sub
    'Long edit 30/10/2009
    Private Sub BindRepeater()
        params = GetPageParams(FilterFieldType.All)
        If Not (String.IsNullOrEmpty(params)) Then
            params = "&" & params
        End If
        If Not IsPostBack Then
            ViewState("F_SortBy") = Replace(Request("F_SortBy"), ";", "")
            ViewState("F_SortOrder") = Replace(Request("F_SortOrder"), ";", "")
            ViewState("F_PG") = Request("F_PG")
        End If
        If ViewState("F_SortBy") Is Nothing Then
            ViewState("F_SortBy") = "a.Username"
        End If
        If ViewState("F_SortOrder") Is Nothing Then
            ViewState("F_SortOrder") = "ASC"
        End If

        'Dim res As DataSet = DB.GetDataSet(SQL)
        hidCon.Value = hidCon.Value & " ORDER BY " & CStr(ViewState("F_SortBy")) & " " & CStr(ViewState("F_SortOrder"))
        Dim res As DataTable = DB.GetDataTable(hidCon.Value)

        myNavigator.NofRecords = res.Rows.Count
        myNavigator.MaxPerPage = dgList.PageSize
        myNavigator.PageNumber = Math.Max(Math.Min(CType(ViewState("F_PG"), Integer), myNavigator.NofPages), 1)
        myNavigator.DataBind()

        ViewState("F_PG") = myNavigator.PageNumber
        tblList.Visible = (myNavigator.NofRecords <> 0)
        plcNoRecords.Visible = (myNavigator.NofRecords = 0)

        dgList.DataSource = res.DefaultView
        dgList.CurrentPageIndex = CInt(ViewState("F_PG")) - 1
        dgList.DataBind()
    End Sub
    'end 30/10/2009
    Private Sub BinQuery()

        Dim sConn As String
        sConn = " where "

        SQL = " SELECT * FROM Admin a"
        If Not DB.IsEmpty(F_Login.Text) Then
            SQL = SQL & sConn & "a.Username LIKE " & DB.FilterQuote(F_Login.Text)
            sConn = " AND "
        End If
        If Not DB.IsEmpty(F_LastName.Text) Then
            SQL = SQL & sConn & "a.LastName LIKE " & DB.FilterQuote(F_LastName.Text)
            sConn = " AND "
        End If
        If Not DB.IsEmpty(F_FirstName.Text) Then
            SQL = SQL & sConn & "a.FirstName LIKE " & DB.FilterQuote(F_FirstName.Text)
            sConn = " AND "
        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL = SQL & sConn & " a.IsActive  = " & DB.Number(F_IsActive.SelectedValue)
            sConn = " AND "
        End If

        hidCon.Value = SQL
    End Sub

    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        ViewState("F_PG") = 1
        BinQuery()
        BindRepeater()
    End Sub

    Protected Sub dgList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgList.ItemCommand
        If e.CommandName = "Active" Then
            AdminRow.ChangeIsActive(DB, e.CommandArgument)
        End If
        BindRepeater()
    End Sub

    Protected Sub dgList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgList.ItemDataBound

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim active As Boolean = DirectCast(DirectCast(DirectCast(e.Item.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("IsActive")
            Dim UserName As String = String.Empty
            Try
                UserName = DirectCast(DirectCast(DirectCast(e.Item.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("Username")
            Catch ex As Exception

            End Try

            Dim imbActive As ImageButton = CType(e.Item.FindControl("imbActive"), ImageButton)
            Dim ConfirmDelete As HyperLink = CType(e.Item.FindControl("ConfirmDelete"), HyperLink)

            If active Then
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If

            If (UserName = CType(Context.User.Identity, AdminIdentity).Username) Then
                imbActive.Style.Add("cursor", "default")
                imbActive.Enabled = False
                ConfirmDelete.Enabled = False
                ConfirmDelete.Visible = False
            End If
        End If
    End Sub

    Private Sub dgList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgList.SortCommand
        If CStr(ViewState("F_SortOrder")) = "ASC" And CStr(ViewState("F_SortBy")) = e.SortExpression Then
            ViewState("F_SortOrder") = "DESC"
        Else
            ViewState("F_SortOrder") = "ASC"
        End If
        ViewState("F_SortBy") = Replace(e.SortExpression, ";", "")
        ViewState("F_PG") = 1
        BindRepeater()
    End Sub

    Private Sub myNavigator_PagingEvent(ByVal sender As Object, ByVal e As NavigatorEventArgs) Handles myNavigator.NavigatorEvent
        ViewState("F_PG") = e.PageNumber
        BindRepeater()
    End Sub
    Public Function getCountIPAccess(ByVal username As String) As Integer
        Return AdminIPAccessRow.CountByUsername(DB, username)
    End Function
End Class

