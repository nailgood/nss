Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.IO

Public Class admin_Video_Category_Default
    Inherits AdminPage
    Dim ToTal As Integer
    Private PathFolderImage As String = Utility.ConfigData.VideoBannerFolder
    Private ImageName As String = Utility.ConfigData.DefaultVideoBannerName

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        fuImage.ImageDisplayFolder = PathFolderImage
        fuImage.Folder = fuImage.ImageDisplayFolder

        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            F_Name.Text = Request("F_Name")
            F_IsActive.Text = Request("F_IsActive")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "Arrange"
            If gvList.SortOrder = String.Empty Then gvList.SortBy = "ASC"
            BindList()
        End If
    End Sub

    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder

        SQLFields = "SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " CategoryId,CategoryName,IsActive"
        SQL = " FROM Category c "

        If Not F_Name.Text = String.Empty Then
            SQL = SQL & Conn & "c.CategoryName LIKE " & DB.FilterQuote(F_Name.Text)
            Conn = " AND "
        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            SQL = SQL & Conn & "c.IsActive  = " & DB.Number(F_IsActive.SelectedValue)
            Conn = " AND "
        End If
        SQL = SQL & Conn & "c.Type=" & Utility.Common.CategoryType.Video
        Conn = " AND "
        ToTal = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        gvList.Pager.NofRecords = ToTal

        Dim res As DataTable = DB.GetDataTable(SQLFields & SQL & " ORDER BY " & gvList.SortByAndOrder)
        gvList.DataSource = res.DefaultView
        gvList.DataBind()

        ''View Image
        LoadImage()
    End Sub

    Private Sub AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNew.Click
        Response.Redirect("edit.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub

        gvList.PageIndex = 0
        BindList()
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim active As Boolean = DirectCast(DirectCast(DirectCast(e.Row.DataItem, System.Object), System.Data.DataRowView).Row, System.Data.DataRow).Item("IsActive")

            Dim imbActive As ImageButton = CType(e.Row.FindControl("imbActive"), ImageButton)

            If active Then
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If
            ''Dim data As CategoryRow = DirectCast(e.Row.DataItem, CategoryRow)
            Dim imbUp As ImageButton = CType(e.Row.FindControl("imbUp"), ImageButton)
            Dim imbDown As ImageButton = CType(e.Row.FindControl("imbDown"), ImageButton)
            If ToTal < 2 Then
                imbUp.Visible = False
                imbDown.Visible = False
            Else
                ''imbDown.CommandArgument=
                If e.Row.RowIndex = 0 Then
                    imbUp.Visible = False
                ElseIf e.Row.RowIndex = ToTal - 1 Then
                    imbDown.Visible = False
                End If
            End If

        End If

    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "Up" Then
            CategoryRow.ChangeArrange(DB, e.CommandArgument, 0, True)
        ElseIf e.CommandName = "Down" Then
            CategoryRow.ChangeArrange(DB, e.CommandArgument, 0, False)
        ElseIf e.CommandName = "Delete" Then
            Dim obj As CategoryRow = CategoryRow.GetRow(DB, e.CommandArgument)
            If Not obj Is Nothing AndAlso Not String.IsNullOrEmpty(obj.Banner) Then
                Dim VideoBannerFolder As String = Server.MapPath("~" & Utility.ConfigData.VideoCategoryBannerFolder)
                Utility.File.DeleteFile(VideoBannerFolder & obj.Banner)
            End If
            CategoryRow.Delete(DB, e.CommandArgument)
            Dim logDetail As New AdminLogDetailRow
            logDetail.ObjectId = obj.CategoryId
            logDetail.Action = Utility.Common.AdminLogAction.Delete.ToString()
            logDetail.Message = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(obj, Utility.Common.ObjectType.Category)
            logDetail.ObjectType = Utility.Common.ObjectType.Category.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
        ElseIf e.CommandName = "Active" Then
            Dim obj As CategoryRow = CategoryRow.GetRow(DB, e.CommandArgument)
            CategoryRow.ChangeIsActive(DB, obj.CategoryId)

            Dim logDetail As New AdminLogDetailRow
            logDetail.ObjectId = obj.CategoryId
            logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            logDetail.Message = "IsActive|" & obj.IsActive.ToString() & "|" & (Not obj.IsActive).ToString() & "[br]"
            logDetail.ObjectType = Utility.Common.ObjectType.Category.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
        End If
        Response.Redirect("Default.aspx")
    End Sub

    Public Sub LoadImage()
        Try
            Dim di As New DirectoryInfo(Server.MapPath(fuImage.ImageDisplayFolder))
            Dim rgFiles As FileInfo() = di.GetFiles()
            'Dim extArray() As String = {"swf", "jpg", "jpeg", "gif", "bmp", "png"}
            For Each fi As FileInfo In rgFiles
                'For Each ext As String In extArray
                '    If fi.Extension.Contains(ext) Then
                '        fuImage.CurrentFileName = fi.Name
                '        hpimg.ImageUrl = fuImage.ImageDisplayFolder & fi.Name
                '        Exit For
                '    End If
                'Next
                If fi.Name.ToLower().Contains(ImageName) Then
                    fuImage.CurrentFileName = fi.Name
                    hpimg.ImageUrl = fuImage.ImageDisplayFolder & fi.Name & "?d=" & DateTime.Now.Second.ToString()
                    Exit For
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnSaveImage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveImage.Click
        Dim strdir As String = Server.MapPath("~" & PathFolderImage)
        If fuImage.NewFileName <> String.Empty Then
            fuImage.RemoveOldFile()
            fuImage.Folder = PathFolderImage
            fuImage.NewFileName = ImageName & fuImage.OriginalExtension
            fuImage.SaveNewFile()
            ''Core.ResizeImage(strdir & fuImage.NewFileName, strdir & ImageName & fuImage.OriginalExtension, 754, 100)
            ''Utility.File.DeleteFile(strdir & fuImage.NewFileName)
        ElseIf fuImage.MarkedToDelete Then
            fuImage.RemoveOldFile()
        End If
        Response.Redirect("default.aspx")
    End Sub
End Class


