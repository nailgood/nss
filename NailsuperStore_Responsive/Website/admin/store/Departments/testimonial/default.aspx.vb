Imports System.Web.UI
Imports Components
Imports Controls
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports DataLayer
Partial Class admin_store_departments_testimonial_default
    Inherits AdminPage
    Dim Total As Integer = 0
    Dim index As Integer = 0
    Public DepartmentId As Integer = 0
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadDepartment()
            LoadData()
        End If
        DepartmentId = Convert.ToInt32(F_DepartmentId.SelectedValue)
    End Sub

    Private Sub LoadDepartment()
        Dim lstDepartment As StoreDepartmentCollection = StoreDepartmentRow.GetAllLevelDepartment()
        F_DepartmentId.DataSource = lstDepartment
        F_DepartmentId.DataTextField = "Name"
        F_DepartmentId.DataValueField = "DepartmentId"
        F_DepartmentId.DataBind()

        F_DepartmentId.SelectedValue = Request("DepartmentId")
    End Sub

    Private Sub LoadData()
        hidPopUpReviewId.Value = ""
        gvList.PageIndex = 1
        Dim result As List(Of StoreDepartmentReviewRow) = StoreDepartmentReviewRow.ListByDepartmentId(F_DepartmentId.SelectedValue)
        gvList.Pager.NofRecords = result.Count()
        Total = result.Count()
        gvList.DataSource = result
        gvList.DataBind()
    End Sub

    Protected Sub gvList_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim row As StoreDepartmentReviewRow = e.Row.DataItem
            Dim ltrReviewName As Literal = e.Row.FindControl("ltrReviewName")
            Dim ltrItemName As Literal = e.Row.FindControl("ltrItemName")
            Dim ltrStar As Literal = e.Row.FindControl("ltrStar")
            Dim imbUp As ImageButton = e.Row.FindControl("imbUp")
            Dim imbDown As ImageButton = e.Row.FindControl("imbDown")
            Dim ltrDate As Literal = e.Row.FindControl("ltrDate")
            Dim ltrComment As Literal = e.Row.FindControl("ltrComment")

            ltrReviewName.Text = "<a href='/admin/members/edit.aspx?MemberId=" & row.MemberId & "'>" & row.ReviewerName & "</a>"
            ltrItemName.Text = "<a href='/admin/store/items/edit.aspx?ItemId=" & row.ItemId & "'>" & row.ItemName & "</a>"
            ltrStar.Text = "<img src='/includes/theme/images/star" & row.NumStars & "0.png' />"
            ltrDate.Text = row.DateAdded.ToString()
            ltrComment.Text = Utility.Common.GetProductReviewComment(row.Comment)
            hidPopUpReviewId.Value &= row.ItemReviewId & ";"
            index = index + 1
            If Total = index AndAlso Total > 1 Then
                imbDown.Visible = False
            ElseIf index = 1 AndAlso Total > 1 Then
                imbUp.Visible = False
            ElseIf Total < 2 Then
                imbDown.Visible = False
                imbUp.Visible = False
            End If
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        DepartmentId = Convert.ToInt32(F_DepartmentId.SelectedValue)
        Dim item As New StoreDepartmentReviewRow
        item.DepartmentId = DepartmentId
        item.CreatedDate = DateTime.Now
        Dim arr As Array = hidPopUpReviewId.Value.Trim.Split(";")
        If arr.Length > 0 Then
            For i As Integer = 0 To arr.Length - 1
                If Not String.IsNullOrEmpty(arr(i)) Then
                    item.ItemReviewId = CInt(arr(i))
                    StoreDepartmentReviewRow.Insert(item)
                End If
            Next
        End If
        LoadData()
    End Sub

    Protected Sub gvList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "Delete" Then
            StoreDepartmentReviewRow.Delete(DepartmentId, e.CommandArgument)
        ElseIf e.CommandName = "Up" Then
            StoreDepartmentReviewRow.ChangeArrange(DepartmentId, e.CommandArgument, True)
        ElseIf e.CommandName = "Down" Then
            StoreDepartmentReviewRow.ChangeArrange(DepartmentId, e.CommandArgument, False)
        End If
        Response.Redirect("default.aspx?DepartmentId=" & DepartmentId & "&" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub F_DepartmentId_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles F_DepartmentId.SelectedIndexChanged
        DepartmentId = F_DepartmentId.SelectedValue
        LoadData()
    End Sub
End Class
