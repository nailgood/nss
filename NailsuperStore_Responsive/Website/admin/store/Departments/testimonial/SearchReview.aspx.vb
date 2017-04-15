Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.Drawing.Image
Imports System.Web.UI.WebControls
Partial Class admin_store_departments_testimonial_SearchReview
    Inherits AdminPage
    Private ItemId As Integer = 0
    Dim reviewIdSelect As String = String.Empty
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsNumeric(ItemId1.Value) Then
            ItemId = ItemId1.Value
        End If
        reviewIdSelect = Request("ReviewId")
        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            ' F_ItemId.Text = Request("F_ItemId")
            F_NumStarsLBound.Text = Request("F_NumStarsLBound")
            F_NumStarsUBound.Text = Request("F_NumStarsUBound")
            F_DateAddedLbound.Text = Request("F_DateAddedLbound")
            F_DateAddedUbound.Text = Request("F_DateAddedUbound")

            LoadDepartment()
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "ReviewId"

            BindQuery()
            BindList()
        End If
    End Sub

    Private Sub LoadDepartment()
        Dim lstDepartment As StoreDepartmentCollection = StoreDepartmentRow.GetAllLevelDepartment()
        F_DepartmentId.DataSource = lstDepartment
        F_DepartmentId.DataTextField = "Name"
        F_DepartmentId.DataValueField = "DepartmentId"
        F_DepartmentId.DataBind()

        F_DepartmentId.SelectedValue = Request("F_DepartmentId")
        F_DepartmentId.Items.Insert(0, New ListItem("---All---", ""))
    End Sub

    Private Sub BindQuery()
        Dim SQL As String = " FROM StoreItemReview sir inner join storeitem si on(sir.itemid = si.itemid) left join Member mb on(mb.MemberId=sir.MemberId)"
        Dim Conn As String = " AND "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        If F_DepartmentId.SelectedValue <> Nothing Then
            SQL = SQL & " left join StoreDepartmentItem sdi on (si.ItemId = sdi.ItemId) WHERE sir.IsActive = 1" & Conn & "sdi.DepartmentId = " & F_DepartmentId.SelectedValue
            Conn = " AND "
        Else
            SQL = SQL & " WHERE sir.IsActive = 1 "
        End If
        If ItemId > 0 Then
            SQL = SQL & Conn & "sir.ItemId = " & ItemId
            Conn = " AND "
        ElseIf LookupField.Value <> Nothing Then
            ItemId = StoreItemRow.GetRow(DB, LookupField.Value.ToString).ItemId
            SQL = SQL & Conn & "sir.ItemId = " & ItemId
            Conn = " AND "
        End If
        If Not F_DateAddedLbound.Text = String.Empty Then
            SQL = SQL & Conn & "DateAdded >= " & DB.Quote(F_DateAddedLbound.Text)
            Conn = " AND "
        End If
        If Not F_DateAddedUbound.Text = String.Empty Then
            SQL = SQL & Conn & "DateAdded < " & DB.Quote(DateAdd("d", 1, F_DateAddedUbound.Text))
            Conn = " AND "
        End If

        If Not F_NumStarsLBound.Text = String.Empty Then
            SQL = SQL & Conn & "NumStars >= " & DB.Number(F_NumStarsLBound.Text)
            Conn = " AND "
        End If
        If Not F_NumStarsUBound.Text = String.Empty Then
            SQL = SQL & Conn & "NumStars <= " & DB.Number(F_NumStarsUBound.Text)
            Conn = " AND "
        End If
        hidCon.Value = SQL
    End Sub

    Private Sub BindList()
        Dim SQLFields As String = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " sir.*, si.Itemname, si.SKU,si.itemid,mb.Username"
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(sir.ReviewId) as dem " & hidCon.Value)

        Dim res As DataTable = DB.GetDataTable(SQLFields & hidCon.Value & " ORDER BY " & IIf(gvList.SortByAndOrder.Contains("DateAdded") = False, " Dateadded desc", gvList.SortByAndOrder))
        gvList.DataSource = res.DefaultView
        gvList.DataBind()

    End Sub

    Protected Sub gvList_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim reviewId As String = e.Row.DataItem("ReviewId")
            Dim chk As CheckBox = CType(e.Row.FindControl("chk_ReviewId"), CheckBox)
            Dim ltrComment As Literal = e.Row.FindControl("ltrComment")

            chk.Attributes.Add("onclick", "CheckItem('" + reviewId + "',this.checked);")
            If reviewIdSelect.Contains(reviewId + ";") Then
                If Not chk Is Nothing Then
                    chk.Checked = True
                    chk.Enabled = False
                End If
            End If
            Dim ltrStar As Literal = e.Row.FindControl("ltrStar")
            Dim imbFacebook As Image = e.Row.FindControl("imbFacebook")

            ltrStar.Text = "<img src='/includes/theme/images/star" & e.Row.DataItem("NumStars") & "0.png' />"
            If e.Row.DataItem("IsFacebook") Then
                imbFacebook.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbFacebook.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If
            ltrComment.Text = Utility.Common.GetProductReviewComment(e.Row.DataItem("Comment"))
        End If
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As System.EventArgs) Handles btnSearch.Click
        gvList.PageIndex = 0
        BindQuery()
        BindList()
    End Sub

    Protected Sub btnClear_Click(sender As Object, e As System.EventArgs) Handles btnClear.Click
        Response.Redirect("SearchReview.aspx?ReviewId=" & reviewIdSelect & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class
