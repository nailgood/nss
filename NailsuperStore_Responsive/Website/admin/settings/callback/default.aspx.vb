Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.Data.SqlClient

Partial Class admin_store_requestcallbacklanguage_Index
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        F_DetailID.Text = Core.ProtectParam(Request("DetailID"))
        gvList.BindList = AddressOf BindList

        If Not IsPostBack Then
            F_LanguageId.DataSource = RequestCallBackLanguageRow.GetAllRequestCallBackLanguages(DB)
            F_LanguageId.DataValueField = "LanguageId"
            F_LanguageId.DataTextField = "Language"
            F_LanguageId.DataBind()
            F_LanguageId.Items.Insert(0, New ListItem("", ""))

            F_EmailId.DataSource = ContactUsSubjectEmailRow.GetAllContactUsSubjectEmailsGroup(DB, "14")
            F_EmailId.DataValueField = "EmailId"
            F_EmailId.DataTextField = "Email"
            F_EmailId.DataBind()
            F_EmailId.Items.Insert(0, New ListItem("", ""))

            'F_LanguageName.Text = Request("F_LanguageName")
            'F_LanguageCode.Text = Request("F_LanguageCode")
            'F_LanguageId.SelectedValue = Request("F_LanguageId")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "LanguageId"
            If F_DetailID.Text <> "" Then
                Dim dbCustomerContact As EmailLanguageRow = EmailLanguageRow.GetRow(DB, F_DetailID.Text)
                F_LanguageId.SelectedValue = dbCustomerContact.LanguageID
                F_EmailId.SelectedValue = dbCustomerContact.EmailID
            End If

            BindList()
        End If
        If F_EmailId.SelectedItem.Value <> "" Then
            btnDeleteEmail.Visible = True
        Else
            btnDeleteEmail.Visible = False
        End If
    End Sub

    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        'ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " * "
        SQL = " FROM Vie_EmailLanguage  "

        If Not F_LanguageId.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "LanguageId = " & DB.Quote(F_LanguageId.SelectedValue)
            Conn = " AND "
        End If
        If Not F_EmailId.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "EmailId = " & DB.Quote(F_EmailId.SelectedValue)
            Conn = " AND "
        End If
        If Not F_LanguageCode.Text = String.Empty Then
            SQL = SQL & Conn & "LanguageCode LIKE " & DB.FilterQuote(F_LanguageCode.Text)
            Conn = " AND "
        End If
        If Not F_LanguageName.Text = String.Empty Then
            SQL = SQL & Conn & "Language LIKE " & DB.FilterQuote(F_LanguageName.Text)
            Conn = " AND "
        End If

        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)

        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        If res.Rows.Count = 1 Then
            F_DetailID.Text = res.Rows(0)("DetailID")
        End If
        gvList.DataSource = res.DefaultView
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

    Protected Sub F_LanguageId_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles F_LanguageId.SelectedIndexChanged
        If F_LanguageId.SelectedItem.Value <> "" Then
            Dim dbRequestCallBack As RequestCallBackLanguageRow = RequestCallBackLanguageRow.GetRow(DB, F_LanguageId.SelectedItem.Value)
            F_LanguageCode.Text = dbRequestCallBack.LanguageCode
            F_LanguageName.Text = dbRequestCallBack.Language
            AddNewEmail.Text = "Edit Language"
            btnDeleteEmail.Visible = True
        Else
            F_LanguageCode.Text = ""
            F_LanguageName.Text = ""
            AddNewEmail.Text = "Add New Language"
        End If
        gvList.PageIndex = 0
        BindList()
    End Sub

    Protected Sub AddNewEmail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddNewEmail.Click
        If F_LanguageId.SelectedValue <> "" Then
            Response.Redirect("editEmail.aspx?LanguageId=" & F_LanguageId.SelectedValue & "&" & GetPageParams(FilterFieldType.All))
            'Else
            '    Response.Redirect("editEmail.aspx?" & GetPageParams(FilterFieldType.All))
        End If
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbRequestCallBack As RequestCallBackLanguageRow
            dbRequestCallBack = New RequestCallBackLanguageRow(DB)
            dbRequestCallBack.Language = F_LanguageName.Text
            dbRequestCallBack.LanguageCode = F_LanguageCode.Text
            If F_EmailId.SelectedItem.Value <> "" Then
                dbRequestCallBack.LanguageId = F_LanguageId.SelectedItem.Value
                dbRequestCallBack.Update()
            Else
                dbRequestCallBack.Insert()
            End If
            DB.CommitTransaction()
            F_LanguageId.DataSource = RequestCallBackLanguageRow.GetAllRequestCallBackLanguages(DB)
            F_LanguageId.DataValueField = "LanguageId"
            F_LanguageId.DataTextField = "Language"
            F_LanguageId.DataBind()
            F_LanguageId.Items.Insert(0, New ListItem("", ""))
            'Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    'Protected Sub F_LanguageId_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles F_LanguageId.SelectedIndexChanged
    '    gvList.PageIndex = 0
    '    BindList()
    'End Sub

    Protected Sub btnDeleteEmail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteEmail.Click
        Dim EmailID As String = F_LanguageId.SelectedItem.Value
        Try
            DB.BeginTransaction()
            RequestCallBackLanguageRow.RemoveRow(DB, EmailID)
            DB.CommitTransaction()
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class

