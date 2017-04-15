Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_promotion_linkbanner_default
    Inherits AdminPage
    Protected iType As Integer = 0
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            iType = CInt(Request.QueryString("Type"))
        Catch ex As Exception

        End Try

        If Not IsPostBack Then
            LoadDefault()
        End If

        gvList.BindList = AddressOf BindList
    End Sub

    Private Sub LoadDefault()
        If iType = 2 Then
            ltrHeader.Text = "Strip banner"
        ElseIf iType = 1 Then
            ltrHeader.Text = "Left banner"
        ElseIf iType = 0 Then
            ltrHeader.Text = "Strip banner"
        ElseIf iType = 3 Then
            ltrHeader.Text = "Exclusive Offers"
        ElseIf iType = 4 Then
            ltrHeader.Text = "Bonus Offers"
        End If

        SubTitle.Text = Request("SubTitle")
        MainTitle.Text = Request("MainTitle")
        F_IsActive.Text = Request("F_IsActive")


        F_StartingDateLbound.Text = Request("F_StartingDateLBound")
        F_StartingDateUbound.Text = Request("F_StartingDateUBound")
        F_EndingDateLbound.Text = Request("F_EndingDateLBound")
        F_EndingDateUbound.Text = Request("F_EndingDateUBound")


        gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
        gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
        If gvList.SortBy = String.Empty Then
            gvList.SortBy = "StartingDate"
            gvList.SortOrder = "DESC"
        End If


        BindList()
    End Sub
    Private Sub BindList()
        Dim SQLFields, SQL As String

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        SQL = " FROM PromotionSalesprice WHERE 1=1 "


        If Not SubTitle.Text = String.Empty Then
            SQL &= " AND SubTitle LIKE " & DB.FilterQuote(SubTitle.Text)
        End If

        If Not MainTitle.Text = String.Empty Then
            SQL &= " AND MainTitle LIKE " & DB.FilterQuote(MainTitle.Text)
        End If

        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL &= " AND IsActive  = " & DB.Number(F_IsActive.SelectedValue)
        End If

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
        SQL &= " And Type = " & iType
        If iType = 2 Then
            SQL &= " AND DepartmentID > 0"
        End If
        'Select Case iType
        '    Case (0)
        '        SQL &= " AND IsHomepage = 1"
        '    Case (1)
        '        SQL &= " AND IsResource = 1"
        '    Case (2)
        '        SQL &= " AND DepartmentID > 0"
        '    Case (4)
        '        SQL &= " AND IsBonus = 1 "
        '    Case Else
        '        SQL &= " AND IsHomepage = 0 AND IsResource = 0 AND DepartmentID = 0 AND IsBonus = 0 "
        'End Select

        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)

        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub

    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?Type=" & iType)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindList()
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim DepartmentId As Integer = e.Row.DataItem("DepartmentID")
            If DepartmentId > 0 Then
                Dim row As StoreDepartmentRow = StoreDepartmentRow.GetRow(DB, DepartmentId)
                Dim litDepartment As Literal = CType(e.Row.FindControl("litDepartment"), Literal)
                litDepartment.Text = row.Name
            End If
        End If

    End Sub
End Class

