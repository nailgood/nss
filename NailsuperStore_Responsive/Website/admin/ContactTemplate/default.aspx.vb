Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.Data.SqlClient
Partial Class admin_emailTemplate_Index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        F_DetailID.Text = Core.ProtectParam(Request("DetailID"))
        gvList.BindList = AddressOf BindList

        If Not IsPostBack Then
            F_SubjectTypeId.Items.Insert(0, New ListItem("Outbound Mail", "1"))
            F_SubjectTypeId.Items.Insert(0, New ListItem("Inbound Mail", "2"))

            F_SubjectId.DataSource = ContactUsSubjectRow.GetTypeContactUsSubjects(DB, F_SubjectTypeId.SelectedValue)
            F_SubjectId.DataValueField = "SubjectId"
            F_SubjectId.DataTextField = "Subject"
            F_SubjectId.DataBind()
            F_SubjectId.Items.Insert(0, New ListItem("", ""))

            gvList.Columns(5).Visible = False
            gvList.Columns(6).Visible = False
            gvList.Columns(7).Visible = False

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "EmailId"

            'BindList()
            btnSearch_Click(sender, e)
        End If

    End Sub

    Private Sub BindQuery()
        Dim SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQL = " FROM Vie_EmailTemplet  "

        If Not F_SubjectTypeId.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "SubjectTypeId = " & DB.Quote(F_SubjectTypeId.SelectedValue)
            Conn = " AND "
        End If

        If Not F_SubjectId.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "SubjectId = " & DB.Quote(F_SubjectId.SelectedValue)
            Conn = " AND "
        End If
        If Not F_Subject.Text = String.Empty Then
            SQL = SQL & Conn & "Subject LIKE " & DB.FilterQuote(F_Subject.Text)
            Conn = " AND "
        End If
        If Not F_Name.Text = String.Empty Then
            SQL = SQL & Conn & "Name LIKE " & DB.FilterQuote(F_Name.Text)
            Conn = " AND "
        End If
        If Not F_Contents.Text = String.Empty Then
            SQL = SQL & Conn & "Contents LIKE " & DB.FilterQuote(F_Contents.Text)
            Conn = " AND "
        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "IsActive  = " & DB.Number(F_IsActive.SelectedValue)
            Conn = " AND "
        End If
        If Not F_StartDateLbound.Text = String.Empty Then
            SQL = SQL & Conn & "StartDate >= " & DB.Quote(F_StartDateLbound.Text)
            Conn = " AND "
        End If
        If Not F_StartDateUbound.Text = String.Empty Then
            SQL = SQL & Conn & "StartDate < " & DB.Quote(DateAdd("d", 1, F_StartDateUbound.Text))
            Conn = " AND "
        End If
        hidCon.Value = SQL
    End Sub

    Private Sub BindList()
        Dim SQLFields As String = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        'gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        gvList.Pager.NofRecords = DB.GetDataTable("SELECT * " & hidCon.Value).Rows.Count
        'Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        Dim res As DataTable = DB.GetDataTable(SQLFields & hidCon.Value & " ORDER BY " & gvList.SortByAndOrder)
        If res.Rows.Count = 1 Then
            F_DetailID.Text = res.Rows(0)("EmailID")
        End If
        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub
    'end 05/11/2009
    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        If F_SubjectTypeId.SelectedValue = "1" Then
            Response.Redirect("editEmail.aspx?" & GetPageParams(FilterFieldType.All))
        Else
            Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindQuery()
        BindList()
    End Sub

    Protected Sub F_SubjectId_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles F_SubjectId.SelectedIndexChanged
        gvList.PageIndex = 0
        BindList()
    End Sub

    Protected Sub F_SubjectTypeId_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles F_SubjectTypeId.SelectedIndexChanged
        F_SubjectId.DataSource = ContactUsSubjectRow.GetTypeContactUsSubjects(DB, F_SubjectTypeId.SelectedValue)
        F_SubjectId.DataValueField = "SubjectId"
        F_SubjectId.DataTextField = "Subject"
        F_SubjectId.DataBind()
        F_SubjectId.Items.Insert(0, New ListItem("", ""))
        If F_SubjectTypeId.SelectedValue = 2 Then
            gvList.Columns(5).Visible = False
            gvList.Columns(6).Visible = False
            gvList.Columns(7).Visible = False
        Else
            gvList.Columns(5).Visible = True
            gvList.Columns(6).Visible = True
            gvList.Columns(7).Visible = True
        End If
        BindList()

    End Sub
End Class

