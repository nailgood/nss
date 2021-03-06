Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.Data.SqlClient
Partial Class admin_password
    Inherits AdminPage
    '----------------------VARIABLES
    Protected MemberId As Integer = 0

    '----------------------EVENTS
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        gvList.BindList = AddressOf BindList
        If Not Page.IsPostBack Then
            Try
                MemberId = Convert.ToInt32(Request("MemberId"))
            Catch ex As Exception
            End Try

            If MemberId > 0 Then
                Dim dbMember As MemberRow = MemberRow.GetRow(MemberId)
                F_UserName.Text = dbMember.Username
                F_Email.Text = dbMember.Customer.Email
                pass.Visible = True
                LitPass.Text = dbMember.Password
            Else
                pass.Visible = False
                btnSearch.Text = "Search"
            End If

            If Not IsPostBack Then
                gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
                gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
                If gvList.SortBy = String.Empty Then gvList.SortBy = "Email"

                If F_UserName.Text <> "" Or F_Email.Text <> "" Then
                    BindList()
                End If

            End If
        End If
    End Sub
    'Long edit 05/11/2009
    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        SQL = " FROM member,customer  "
        SQL = SQL & Conn & " member.customerid=customer.customerid "
        Conn = " AND "
        If Not F_UserName.Text = String.Empty Then
            SQL = SQL & Conn & "UserName LIKE " & DB.FilterQuote(F_UserName.Text)
            Conn = " AND "
        End If
        If Not F_Email.Text = String.Empty Then
            SQL = SQL & Conn & "Email LIKE " & DB.FilterQuote(F_Email.Text)
            Conn = " AND "
        End If
        'If Not F_Sort.Text = String.Empty Then
        '    SQL = SQL & Conn & "sort = " & F_Sort.Text
        '    Conn = " AND "
        'End If

        'gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        gvList.Pager.NofRecords = DB.GetDataTable("SELECT * " & SQL).Rows.Count ''MemberRow.GetListMember("SELECT * " & SQL).Rows.Count
        'Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder) ''MemberRow.GetListMember(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub
    'end 05/11/2009
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub
        Password.Visible = False
        pass.Visible = False
        If btnSearch.Text = "Search" Then
            If F_UserName.Text <> "" Or F_Email.Text <> "" Then
                gvList.PageIndex = 0
                BindList()
            End If
            'Else
            '    Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        End If

    End Sub

    Protected Sub Password_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Password.Click
        GetPassword(MemberId)
    End Sub
    Private Sub GetPassword(ByVal memberid As Integer)
        Dim dbMember As MemberRow = MemberRow.GetRow(memberid)
        LitPass.Text = dbMember.Password
    End Sub
End Class

