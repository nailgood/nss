Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_groups_options_choices_Index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            F_OptionId.Datasource = StoreItemGroupOptionRow.GetAllItemGroupOptions(DB)
            F_OptionId.DataValueField = "OptionId"
            F_OptionId.DataTextField = "OptionName"
            F_OptionId.Databind()
            F_OptionId.Items.Insert(0, New ListItem("-- ALL --", ""))

            F_ChoiceName.Text = Request("F_ChoiceName")
            F_OptionId.SelectedValue = Request("F_OptionId")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "ChoiceId"

            BindList()
        End If
    End Sub

    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " c.*, o.optionname "
        SQL = " FROM StoreItemGroupChoice c inner join storeitemgroupoption o on c.optionid = o.optionid  "

        If Not F_OptionId.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "c.OptionId = " & DB.Quote(F_OptionId.SelectedValue)
            Conn = " AND "
        End If
        If Not F_ChoiceName.Text = String.Empty Then
            SQL = SQL & Conn & "ChoiceName LIKE " & DB.FilterQuote(F_ChoiceName.Text)
            Conn = " AND "
        End If
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)

        Dim res As DataSet = DB.GetDataSet(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.Tables(0).DefaultView
        gvList.DataBind()
    End Sub

    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindList()
    End Sub
End Class

