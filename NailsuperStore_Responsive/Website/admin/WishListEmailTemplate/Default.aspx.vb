Imports Components
Imports DataLayer

Partial Class admin_WishListEmailTemplate_Default
    Inherits AdminPage
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            BindRepeater()
        End If
    End Sub
    'Long edit 05/11/2009
    Private Sub BindRepeater()
        ' BUILD QUERY
        SQL = "SELECT * FROM WishListEmailTemplate"
        'Dim res As DataSet = DB.GetDataSet(SQL)
        Dim res As DataTable = DB.GetDataTable(SQL) '' WishListEmailTemplateRow.GetListWishListEmailTemplate(SQL)
        dgList.DataSource = res.DefaultView
        dgList.DataBind()
    End Sub
    'end 05/11/2009
End Class
