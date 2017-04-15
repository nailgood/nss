

Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_Keyword_RedirectKeyword_default
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            BindList()
        End If
    End Sub

    Private Sub BindList()

        Dim res As DataSet = DB.GetDataSet("Select Id,coalesce(KeywordName,'') as KeywordName,coalesce(LinkRedirect,'') as LinkRedirect, Description from KeywordRedirect order by KeywordName ASC")
        If Not res Is Nothing AndAlso Not res.Tables(0) Is Nothing Then
            gvList.Pager.NofRecords = res.Tables(0).Rows.Count
        End If
        gvList.DataSource = res.Tables(0).DefaultView
        gvList.DataBind()
    End Sub

    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub gvList_ItemDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvList.RowDataBound
        If Not (e.Row.RowType = DataControlRowType.DataRow) Then
            Exit Sub
        End If

        

    End Sub
End Class

