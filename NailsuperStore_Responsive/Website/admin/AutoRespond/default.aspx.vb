Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Partial Class admin_AutoRespond_Default
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Not Core.ProtectParam(Request("F_SortBy")) = String.Empty Then
                gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            End If
            If Not Core.ProtectParam(Request("F_SortOrder")) = String.Empty Then
                gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            End If
            If gvList.SortBy = String.Empty Then
                gvList.SortBy = "CreateDate"
                gvList.SortOrder = "Desc"
            End If


            BindList()
        End If
    End Sub
    Private Sub BindList()
        Dim SQLFields, SQL As String
       
        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        SQL = " FROM AutoRespond WHERE 1=1 "
        If Not F_StartingDateLbound.Text = String.Empty Then
            SQL &= " AND StartingDate >= " & DB.Quote(F_StartingDateLbound.Text)
        End If

        If Not F_StartingDateUbound.Text = String.Empty Then
            SQL &= " AND StartingDate < " & DB.Quote(DateAdd("d", 1, F_StartingDateUbound.Text))
        End If

        If Not F_EndingDateLbound.Text = String.Empty Then
            SQL &= " AND EndingDate >= " & DB.Quote(F_EndingDateLbound.Text)
        End If

        If Not F_EndingDateUbound.Text = String.Empty Then
            SQL &= " AND EndingDate < " & DB.Quote(DateAdd("d", 1, F_EndingDateUbound.Text))
        End If
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)

        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub

    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        Response.Redirect("edit.aspx")
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindList()
    End Sub
End Class
