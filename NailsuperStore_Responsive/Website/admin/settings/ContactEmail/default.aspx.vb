Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.Data.SqlClient
Partial Class admin_contactEmail_Index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        gvList.BindList = AddressOf BindList

        If Not IsPostBack Then
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "EmailID"
            BindList()
        End If
    End Sub
    'Long edit 05/11/2009
    Private Sub BindList()
        Dim SQLFields, SQL As String

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT DISTINCT cse.* "
        SQL = " FROM ContactUsSubjectEmail cse join ContactUsSubjectDetail csd on cse.EmailID = csd.EmailID  "

        'gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        gvList.Pager.NofRecords = DB.GetDataTable("SELECT DISTINCT cse.* " & SQL).Rows.Count '' ContactUsSubjectRow.GetListContactUsSubject("SELECT * " & SQL).Rows.Count
        'Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder) ''ContactUsSubjectRow.GetListContactUsSubject(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)

        gvList.DataSource = res.DefaultView
        gvList.DataBind()
    End Sub
    'end 05/11/2009
    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim EmailId As Integer = e.Row.DataItem("EmailId")
            Dim ltrSubjects As Literal = CType(e.Row.FindControl("ltrSubjects"), Literal)
            Dim dt As DataTable = ContactUsSubjectDetailRow.GetByEmailId(DB, EmailId)
            Dim count As Integer = dt.Rows.Count
            If count > 0 Then
                For i As Integer = 0 To count - 1
                    If count = 1 Then
                        ltrSubjects.Text = "<li class=""ns"">" & dt.Rows(i)("Subject") & "</li>"
                    Else
                        ltrSubjects.Text = ltrSubjects.Text & "<li>" & dt.Rows(i)("Subject") & "</li>"
                    End If
                Next
            End If

        End If

    End Sub
End Class

