Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_opinion_Index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'If Session("TypeContact") <> Nothing Then
        '    F_SubjectId.SelectedItem.Text = Session("TypeContact")
        'End If
        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then

            F_SubjectId.DataSource = ContactUsRow.GetAllTypeContact(DB)
            F_SubjectId.DataValueField = "TypeContact"
            F_SubjectId.DataTextField = "TypeContact"
            F_SubjectId.DataBind()
            'F_CreateDateLBound.Text = Request("F_CreateDateLBound")
            'F_CreateDateUBound.Text = Request("F_CreateDateUBound")

            F_FirstName.Text = Request("F_FirstName")
            F_LastName.Text = Request("F_LastName")
            F_EmailAddress.Text = Request("F_EmailAddress")
            F_Phone.Text = Request("F_Phone")
            'F_OrderNumber.Text = Request("F_OrderNumber")
            F_SubjectId.SelectedValue = Request("F_SubjectId")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then
                gvList.SortBy = "CreateDate"
                gvList.SortOrder = "desc"
            End If
            BindList()
        End If
    End Sub
    'Long edit 05/11/2009
    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        SQL = " FROM ContactUs  "

        If Not F_SubjectId.SelectedItem.Text = String.Empty Then
            SQL = SQL & Conn & "TypeContact = " & DB.Quote(F_SubjectId.SelectedItem.Text)
            Conn = " AND "
        End If
        If Not F_FirstName.Text = String.Empty Then
            SQL = SQL & Conn & "FirstName LIKE " & DB.FilterQuote(F_FirstName.Text)
            Conn = " AND "
        End If
        If Not F_LastName.Text = String.Empty Then
            SQL = SQL & Conn & "LastName LIKE " & DB.FilterQuote(F_LastName.Text)
            Conn = " AND "
        End If
        If Not F_EmailAddress.Text = String.Empty Then
            SQL = SQL & Conn & "EmailAddress LIKE " & DB.FilterQuote(F_EmailAddress.Text)
            Conn = " AND "
        End If
        If Not F_Phone.Text = String.Empty Then
            SQL = SQL & Conn & "Phone LIKE " & DB.FilterQuote(F_Phone.Text)
            Conn = " AND "
        End If
        'If Not F_OrderNumber.Text = String.Empty Then
        '	SQL = SQL & Conn & "OrderNumber LIKE " & DB.FilterQuote(F_OrderNumber.Text)
        '	Conn = " AND "
        'End If
        'If Not F_CreateDateLBound.Text = String.Empty Then
        '	SQL = SQL & Conn & "CreateDate >= " & DB.Quote(F_CreateDateLBound.Text)
        '	Conn = " AND "
        'End If
        'If Not F_CreateDateUBound.Text = String.Empty Then
        '	SQL = SQL & Conn & "CreateDate < " & DB.Quote(DateAdd("d", 1, F_CreateDateUBound.Text))
        '	Conn = " AND "
        'End If
        'gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        gvList.Pager.NofRecords = DB.GetDataTable("SELECT * " & SQL).Rows.Count '' ContactUsRow.GetListContactUs("SELECT * " & SQL).Rows.Count
        'Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.DefaultView
        gvList_Config()
        gvList.DataBind()
    End Sub
    'end 05/11/2009
    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindList()
    End Sub
    Private Sub gvList_Config()
        Dim i As Integer = 0
        For i = 9 To 18
            gvList.Columns(i).Visible = False
        Next
        If F_SubjectId.SelectedItem.Text = "Item Not Received" Then
            For i = 9 To 10
                gvList.Columns(i).Visible = True
            Next
        ElseIf F_SubjectId.SelectedItem.Text = "Detail Warranty Information" Then
            For i = 11 To 12
                gvList.Columns(i).Visible = True
            Next
        ElseIf F_SubjectId.SelectedItem.Text = "Damaged Item" Then
            gvList.Columns(9).Visible = True
            For i = 13 To 18
                gvList.Columns(i).Visible = True
            Next
        ElseIf F_SubjectId.SelectedItem.Text = "Return Authorization NR" Then
            gvList.Columns(12).Visible = True
        Else
            For i = 9 To 18
                gvList.Columns(i).Visible = False
            Next
        End If
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If Not (e.Row.RowType = DataControlRowType.DataRow) Then
            Exit Sub
        End If
        Dim lb As Label
        lb = CType(e.Row.FindControl("lbPro"), Label)
        If Convert.ToString(e.Row.DataItem("ProductDescription")) <> String.Empty Then
            lb.Text = e.Row.DataItem("ProductDescription")

        End If
    End Sub
End Class

