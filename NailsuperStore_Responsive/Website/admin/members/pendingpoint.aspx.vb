


Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_members_PendingPoint
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then

            F_MemberTypeId.DataSource = MemberTypeRow.GetAllMemberTypes(DB)
            F_MemberTypeId.DataValueField = "MemberTypeId"
            F_MemberTypeId.DataTextField = "MemberType"
            F_MemberTypeId.DataBind()
            F_MemberTypeId.Items.Insert(0, New ListItem("-- ALL --", ""))

            F_CustomerPriceGroupId.DataSource = CustomerPriceGroupRow.GetAllCustomerPriceGroups(DB)
            F_CustomerPriceGroupId.DataTextField = "codewithcount"
            F_CustomerPriceGroupId.DataValueField = "CustomerPriceGroupId"
            F_CustomerPriceGroupId.DataBind()
            F_CustomerPriceGroupId.Items.Insert(0, New ListItem("-- ALL --", ""))
            F_CustomerNo.Text = Request("F_CustomerNo")
            F_Username.Text = Request("F_Username")

            F_MemberTypeId.SelectedValue = Request("F_MemberTypeId")
            F_CreateDateLbound.Text = Request("F_CreateDateLBound")
            F_CreateDateUbound.Text = Request("F_CreateDateUBound")
            F_CustomerPriceGroupId.SelectedValue = Request("F_CustomerPriceGroupId")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))

            If gvList.SortBy = String.Empty Then
                gvList.SortBy = "CreateDate"
                gvList.SortOrder = "desc"
            End If
            'BindList()
            btnSearch_Click(sender, e)
        End If
    End Sub
   

    
    Private Sub BindQuery()

        Dim Conn As String = " PendingPoint<>0 "
        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder
        If Not F_EmailAddress.Text = String.Empty Then
            Conn = Conn & " and Email LIKE " & DB.FilterQuote(F_EmailAddress.Text.Trim())
        End If
        If Not F_Phone.Text = String.Empty Then
            Conn = Conn & " and Phone LIKE " & DB.FilterQuote(F_Phone.Text.Trim())
        End If
        If Not F_Address.Text = String.Empty Then
            Conn = Conn & "and Address1 LIKE " & DB.FilterQuote(F_Address.Text.Trim())
        End If
        If Not F_Username.Text = String.Empty Then
            Conn = Conn & "and Username LIKE " & DB.FilterQuote(F_Username.Text.Trim())
        End If
        If Not F_CreateDateLbound.Text = String.Empty Then
            Conn = Conn & "and CreateDate >= " & DB.Quote(F_CreateDateLbound.Text.Trim())
        End If
        If Not F_CreateDateUbound.Text = String.Empty Then
            Conn = Conn & "and CreateDate < " & DB.Quote(DateAdd("d", 1, F_CreateDateUbound.Text.Trim()))
        End If
        If Not F_MemberTypeId.SelectedValue = String.Empty Then
            Conn = Conn & "and m.MemberTypeId = " & DB.Quote(F_MemberTypeId.SelectedValue)
        End If
        If Not F_CustomerNo.Text = String.Empty Then
            Conn = Conn & "and c.CustomerNo='" & F_CustomerNo.Text & "'"
        End If
        If Not F_CustomerPriceGroupId.SelectedValue = String.Empty Then
            Conn = Conn & "and customerpricegroupid = " & DB.Number(F_CustomerPriceGroupId.SelectedValue)
        End If
        hidCon.Value = Conn
    End Sub

    Private Sub BindList()
        Dim total As Integer = 0
        Dim ds As DataTable = MemberRow.GetListPendingPoint(DB, gvList.PageIndex + 1, gvList.PageSize, hidCon.Value, gvList.SortBy, gvList.SortOrder, total)
        gvList.Pager.NofRecords = total
        gvList.PageSelectIndex = gvList.PageIndex
        gvList.DataSource = ds
        gvList.DataBind()
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'If Not IsValid Then Exit Sub
        gvList.PageIndex = 0
        BindQuery()
        BindList()
    End Sub
    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ltr As Literal = CType(e.Row.FindControl("ltrTotalPoint"), Literal)
            Dim memberId As Integer = 0
            Try
                memberId = CInt(e.Row.DataItem("MemberId"))
            Catch ex As Exception
            End Try
            Dim point As Integer = CInt(e.Row.DataItem("TotalPoint"))
           
            ltr.Text = "<a href='addpoint.aspx?MemberId=" & memberId & "&" & GetPageParams(Components.FilterFieldType.All) & "'>" & point & "</a>"
            ltr = CType(e.Row.FindControl("ltrPendingPoint"), Literal)
            Try
                point = CInt(e.Row.DataItem("PendingPoint"))
            Catch ex As Exception

            End Try
            ltr.Text = "" & point & ""

        End If
    End Sub
End Class


