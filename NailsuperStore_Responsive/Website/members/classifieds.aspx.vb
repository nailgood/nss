Imports Components
Imports DataLayer
Imports System.Data.SqlClient

Public Class Members_classifieds
    Inherits SitePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not HasAccess() Then
            DB.Close()
            Response.Redirect("/members/login.aspx")
        End If

        gvList.BindList = AddressOf BindData
        If Not IsPostBack Then
            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then gvList.SortBy = "ClassifiedCategoryId"

            BindData()
        End If
    End Sub

    Private Sub gvList_Command(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvList.RowCommand
        Select Case e.CommandName
            Case "Remove"
                DB.ExecuteSQL("delete from classified where classifiedid = " & DB.Number(e.CommandArgument))
                BindData()
        End Select
    End Sub

    Private Sub BindData()
        Dim SQL As String = " FROM classified c inner join classifiedcategory cc on c.classifiedcategoryid = cc.classifiedcategoryid where c.isactive = 1 and c.expirationdate > " & DB.Quote(Date.Now.ToShortDateString) & " and memberid = " & Session("MemberId")
        gvList.DataSource = DB.GetDataSet("SELECT TOP " & (gvList.PageIndex + 1) * gvList.PageSize & " c.*, cc.category " & SQL)
        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)
        gvList.DataBind()

    End Sub
End Class