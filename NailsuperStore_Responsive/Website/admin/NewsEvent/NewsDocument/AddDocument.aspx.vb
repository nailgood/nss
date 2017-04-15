Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Partial Class admin_NewsEvent_NewsDocument_AddDocument
    Inherits AdminPage
    Dim DocIdSelect As String = String.Empty
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

        DocIdSelect = ";" + Request("DocId")
        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            Type = Request("Type")
            F_DocumentName.Text = Request("F_DocumentName")

            F_IsActive.Text = Request("F_IsActive")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "DocumentId"
            BindList()
        End If
    End Sub

    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        SQL = " FROM Document "
        If Not F_DocumentName.Text = String.Empty Then
            SQL = SQL & Conn & "DocumentName LIKE " & DB.FilterQuote(F_DocumentName.Text)
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
        Dim DocId As Integer = e.Row.DataItem("DocumentId")

        ''wrire checkbox for multi Selected
        Dim chk As CheckBox = CType(e.Row.FindControl("chk_DocId"), CheckBox)
        chk.Attributes.Add("onclick", "CheckItem('" & DocId & "',this.checked);")

        If DocIdSelect.Contains(";" & DocId & ";") Then
            If Not chk Is Nothing Then
                chk.Checked = True
                chk.Enabled = False
            End If
        End If
    End Sub

End Class
