Imports Components
Imports DataLayer
Imports System.Collections.Generic
Imports System.Web.Services
Partial Class tips_default
	Inherits SitePage

    Public Shared sSearch As String = ""
    Public index As Integer = 0
    Protected Shared PageSize As Integer = 4
    Private Shared PageIndex As Integer = 1
    Protected Shared TotalRecords As Integer = 0
    Protected Shared countPageSize As Integer = 0
    Public Shared hidTipsIndex As String = ""
    Private Shared result As List(Of TipCategoryRow)
    Private Shared data As New TipCategoryRow
	Private Sub page_load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' rptCategory.BindList = AddressOf BindData
        ShowLanguage.Visible = SysParam.GetValue(DB, "Multilingual")
		If Session("Language") = Nothing Then
			lnkEnglish.Visible = False
			lblVietnamese.Visible = False
		Else
			lblEnglish.Visible = False
			lnkVietnamese.Visible = False
		End If

		If Not IsPostBack Then
			F_TipCategoryId.DataSource = TipCategoryRow.GetAllTipCategoriesWithTips(DB)
			F_TipCategoryId.DataValueField = "TipCategoryId"
			F_TipCategoryId.DataTextField = "TipCategory"
			F_TipCategoryId.DataBind()
            F_TipCategoryId.Items.Insert(0, New ListItem("-- ALL Category--", ""))

			F_TipCategoryId.SelectedValue = Request("F_TipCategoryId")

			BindData()
		End If
    End Sub

	Private Sub lnkVietnamese_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkVietnamese.Click
        Session("Language") = LanguageCode.Vietnamese
		UpdateMemberLanguage(Session("Language"))
		Response.Redirect(Request.Url.PathAndQuery)
	End Sub

	Private Sub UpdateMemberLanguage(ByVal Language As String)
		If Session("MemberId") <> Nothing Then
            Dim Member As MemberRow = MemberRow.GetRow(Session("Memberid"))
			Member.Customer.LanguageCode = Language
			Member.Customer.Update()
		End If
	End Sub

	Private Sub lnkEnglish_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkEnglish.Click
		Session("Language") = Nothing
		UpdateMemberLanguage(Session("Language"))
		Response.Redirect(Request.Url.PathAndQuery)
	End Sub

    Private Sub BindData()
        data.PageIndex = PageIndex
        data.PageSize = PageSize
        data.Condition = "1=1"
        data.OrderBy = "SortOrder"
        data.OrderDirection = "DESC"
        If Not F_TipCategoryId.SelectedValue = String.Empty Then
            data.Condition &= " AND TipCategoryId = " & F_TipCategoryId.SelectedValue
        End If
        If Not Trim(F_Text.Text) = String.Empty Then
            sSearch = Core.SplitSearchAND(Trim(F_Text.Text))
            sSearch = sSearch.Replace("'", "''")
            data.Condition &= " AND (tipcategoryid in (select [key] from CONTAINSTABLE(tipcategory, * , " & DB.Quote(sSearch) & ")) or tipcategoryid in (select tipcategoryid from tip where IsActive = 1 and tipid in (select [key] from CONTAINSTABLE(tip, * , " & DB.Quote(sSearch) & ")))) "
        End If

        result = TipCategoryRow.ListTipCategory(data)
        rptCategory.DataSource = result
        rptCategory.DataBind()
        TotalRecords = data.TotalRow
        hidTipsIndex = PageIndex & "," & result.Count
    End Sub

	Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub
		BindData()
    End Sub

    Protected Sub rptCategory_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptCategory.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            index = index + 1
            Dim tipcate As TipCategoryRow = e.Item.DataItem
            Dim dvTip As controls_resource_center_tip = CType(e.Item.FindControl("dvTip"), controls_resource_center_tip)
            dvTip.TipsCategoryRow = tipcate
            dvTip.index = index
            dvTip.sSearch = sSearch
            dvTip.Fill()
        End If
    End Sub


    <WebMethod()> _
    Public Shared Function GetDataVideo(ByVal pageIndex As Integer, ByVal pageSize As Integer) As String
        Dim xmlData As String = ""
        result = New List(Of TipCategoryRow)
        data.PageIndex = pageIndex
        data.PageSize = pageSize
        countPageSize = (pageIndex - 1) * pageSize
        result = TipCategoryRow.ListTipCategory(data)
        Dim htmlTips As String = String.Empty
        If result.Count > 0 Then
            For i As Integer = 0 To result.Count - 1
                HttpContext.Current.Session("indexRender") = countPageSize + i + 1
                HttpContext.Current.Session("tipsRender") = result(i)
                HttpContext.Current.Session("searchRender") = sSearch
                htmlTips &= Utility.Common.RenderUserControl("~/controls/resource-center/tip.ascx")
                HttpContext.Current.Session("indexRender") = Nothing
                HttpContext.Current.Session("tipsRender") = Nothing
                HttpContext.Current.Session("searchRender") = Nothing
            Next
        End If
        htmlTips &= "<input type='hidden' id='nexthidVideoIndex' clientidmode='Static' value='" & countPageSize + 1 & "' />"
        Return htmlTips
    End Function

End Class
