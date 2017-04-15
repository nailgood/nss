Imports Components
Imports System.Data
Imports DataLayer

Partial Class admin_content_pages_index
    Inherits AdminPage

    Protected params As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim SQL As String = ""

        If IsPostBack Then
            Exit Sub
        End If
        F_Name.Text = Request("F_Name")
        F_PageURL.Text = Request("F_PageURL")
        btnSearch_Click(sender, e)
    End Sub
    'Long edit 04/11/2009
    Private Sub BindQuery()
        params = GetPageParams(FilterFieldType.All)

        If Not IsPostBack Then
            ViewState("F_SortBy") = Replace(Request("F_SortBy"), ";", "")
            ViewState("F_SortOrder") = Replace(Request("F_SortOrder"), ";", "")
            ViewState("F_PG") = Request("F_PG")
        End If
        If ViewState("F_SortBy") Is Nothing Then
            ViewState("F_SortBy") = "SectionName"
        End If
        If ViewState("F_SortOrder") Is Nothing Then
            ViewState("F_SortOrder") = "ASC"
        End If

        ' BUILD QUERY
        Dim sConn As String
        sConn = " and "

        SQL = "select PageId,Name,PageURL, MetaTitle,MetaKeywords,MetaDescription from ContentToolPage where PageURL is not null "
       
        If Not DB.IsEmpty(F_PageURL.Text) Then
            SQL = SQL & sConn & "PageURL LIKE " & DB.FilterQuote(F_PageURL.Text)
        End If
        If Not DB.IsEmpty(F_Name.Text) Then
            SQL = SQL & sConn & "Name LIKE " & DB.FilterQuote(F_Name.Text)
        End If
        SQL = SQL & " ORDER BY Name ASC"
        hidCon.Value = SQL
    End Sub
    'end 04/11/2009
    Private Sub BindDataGrid()
        Dim res As DataTable = DB.GetDataTable(hidCon.Value) '' ContentToolPageRow.GetListContentToolPage(SQL)

        myNavigator.NofRecords = res.Rows.Count
        myNavigator.MaxPerPage = dgList.PageSize
        myNavigator.PageNumber = Math.Max(Math.Min(CType(ViewState("F_PG"), Integer), myNavigator.NofPages), 1)
        myNavigator.DataBind()

        ViewState("F_PG") = myNavigator.PageNumber
        Me.dgList.Visible = (myNavigator.NofRecords <> 0)
        Me.myNavigator.Visible = (myNavigator.NofRecords <> 0)
        plcNoRecords.Visible = (myNavigator.NofRecords = 0)

        dgList.DataSource = res.DefaultView
        dgList.CurrentPageIndex = CInt(ViewState("F_PG")) - 1
        dgList.DataBind()
    End Sub

    Private Sub btnRegister_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRegister.Click
        Response.Redirect("register.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub



    Private Sub myNavigator_PagingEvent(ByVal sender As Object, ByVal e As Controls.NavigatorEventArgs) Handles myNavigator.NavigatorEvent
        ViewState("F_PG") = e.PageNumber
        BindDataGrid()
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        BindQuery()
        BindDataGrid()
    End Sub

    Protected Sub dgList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item) Or (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim row As DataRowView = e.Item.DataItem
            Dim ltrName As Literal = e.Item.FindControl("ltrName")
            Dim ltrPageURL As Literal = e.Item.FindControl("ltrPageURL")
            Dim ltrPageTitle As Literal = e.Item.FindControl("ltrPageTitle")
            Dim ltrMetaKeywords As Literal = e.Item.FindControl("ltrMetaKeywords")
            Dim ltrMetaDescription As Literal = e.Item.FindControl("ltrMetaDescription")
            Dim ltrLeft As Literal = e.Item.FindControl("ltrLeft")
            Dim ltrRight As Literal = e.Item.FindControl("ltrRight")

            ltrName.Text = row.Row(1).ToString()
            ltrPageURL.Text = "<a href='register.aspx?PageId=" & row.Row(0) & "&" & GetPageParams(Components.FilterFieldType.All) & "'>" & row.Row(2).ToString() & "</a>"
            If row.Row(3) IsNot DBNull.Value AndAlso Not String.IsNullOrEmpty(row.Row(3)) Then
                ltrPageTitle.Text = "<input type='checkbox' checked='checked' disabled='disabled' />"
            Else
                ltrPageTitle.Text = ""
            End If
            If row.Row(4) IsNot DBNull.Value AndAlso Not String.IsNullOrEmpty(row.Row(4)) Then
                ltrMetaKeywords.Text = "<input type='checkbox' checked='checked' disabled='disabled' />"
            Else
                ltrMetaKeywords.Text = ""
            End If
            If row.Row(5) IsNot DBNull.Value AndAlso Not String.IsNullOrEmpty(row.Row(5)) Then
                ltrMetaDescription.Text = "<input type='checkbox' checked='checked' disabled='disabled' />"
            Else
                ltrMetaDescription.Text = ""
            End If
            ltrLeft.Text = "<a href='control.aspx?PageId=" & row.Row(0) & "&Region=CT_Left&" & GetPageParams() & "' ><img src='/includes/theme-admin/images/Create.gif' style='border:0px' /></a>"
            ltrRight.Text = "<a href='control.aspx?PageId=" & row.Row(0) & "&Region=CT_Right&" & GetPageParams() & "' ><img src='/includes/theme-admin/images/Create.gif' style='border:0px' /></a>"
        End If
    End Sub
End Class

