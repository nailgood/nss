Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_itemrelate_edit
    Inherits AdminPage

    Private ItemId As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        ItemId = Convert.ToInt32(Request("ItemId"))

        If ItemId = 0 Then
            Delete.Visible = False
        End If

        If Not IsPostBack Then
            Page.Form.DefaultFocus = "ItemName"
            LoadFromDB()


        End If
    End Sub

    Private Sub LoadFromDB()
        drItemRelate.DataSource = DB.GetDataTable("Select itemname, itemid, sku, sku + ' -- ' + itemname as ItemNSk from storeitem where itemid <> " & ItemId & "order by sku").DefaultView
        drItemRelate.DataValueField = "ItemId"
        drItemRelate.DataTextField = "ItemNSk"
        drItemRelate.DataBind()
        drItem.DataSource = DB.GetDataTable("Select itemname, itemid, sku, sku + ' -- ' + itemname as ItemNSk from storeitem order by sku ").DefaultView
        drItem.DataValueField = "ItemId"
        drItem.DataTextField = "ItemNSk"
        drItem.DataBind()
    End Sub

    Private Sub Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Save.Click
        If Not Page.IsValid Then Exit Sub
        SaveItemDetails("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Sub SaveItemDetails(ByVal sRedir As String)
        DB.ExecuteSQL("insert into storeitemrelate (itemid,itemidrelate,desciption,isactive) values (" & drItem.SelectedItem.Value & "," & drItemRelate.SelectedValue & ",'" & txtDesc.Text & "','" & IsActive.Checked & "')")
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Delete.Click
        Response.Redirect("delete.aspx?ItemId=" & ItemId & "&" & GetPageParams(FilterFieldType.All))
    End Sub

End Class