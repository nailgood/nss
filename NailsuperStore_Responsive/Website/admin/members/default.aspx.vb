
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_members_default
    Inherits AdminPage
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        gvList.BindList = AddressOf BindList
        If Not IsPostBack Then
            F_ShipToCounty.Items.AddRange(StateRow.GetStateList().ToArray())
            F_ShipToCounty.DataBind()
            F_ShipToCounty.Items.Insert(0, New ListItem("-- State --", ""))

            F_ShipToCountry.Items.AddRange(CountryRow.GetCountries().ToArray())
            F_ShipToCountry.DataBind()
            F_ShipToCountry.Items.Insert(0, New ListItem("-- Country --", ""))

            F_MemberTypeId.DataSource = MemberTypeRow.GetAllMemberTypes(DB)
            F_MemberTypeId.DataValueField = "MemberTypeId"
            F_MemberTypeId.DataTextField = "MemberType"
            F_MemberTypeId.DataBind()
            F_MemberTypeId.Items.Insert(0, New ListItem("-- ALL --", ""))

            F_CustomerPostingGroup.DataSource = CustomerPostingGroupRow.GetList(DB)
            F_CustomerPostingGroup.DataTextField = "Code"
            F_CustomerPostingGroup.DataValueField = "Code"
            F_CustomerPostingGroup.DataBind()
            F_CustomerPostingGroup.Items.Insert(0, New ListItem("-- ALL --", ""))
            F_CustomerNo.Text = Request("F_CustomerNo")
            F_Username.Text = Request("F_Username")
            F_EmailAddress.Text = Request("F_EmailAddress")
            F_IsActive.Text = Request("F_IsActive")
            F_DeActive.Text = Request("F_DeActive")
            F_MemberTypeId.SelectedValue = Request("F_MemberTypeId")
            F_CreateDateLbound.Text = Request("F_CreateDateLBound")
            F_CreateDateUbound.Text = Request("F_CreateDateUBound")
            F_CustomerPostingGroup.SelectedValue = Request("F_CustomerPostingGroup")

            gvList.SortBy = Core.ProtectParam(Request("F_SortBy"))
            gvList.SortOrder = Core.ProtectParam(Request("F_SortOrder"))
            If gvList.SortBy = String.Empty Then
                gvList.SortBy = "m.CreateDate"
                gvList.SortOrder = "desc"
            End If
            'BindList()
            btnSearch_Click(sender, e)

        End If
    End Sub

    Private Function CheckActiveCode() As String
        Dim res As DataTable = DB.GetDataTable("select * from Member where isActive ='false' and activecode is not null and dateadd(""d"",7,CreateDate) <= getdate()")
        If res.Rows.Count > 0 Then
            Return CStr(res.Rows.Count)
        End If
        Return ""
    End Function


    Private Sub BindSearch()

        Dim Conn As String = " 1=1 "
        ViewState("F_SortBy") = gvList.SortBy
        ViewState("F_SortOrder") = gvList.SortOrder
        ''''Search MemberAdress First - vuphuong edit 27/03/2009
        If Not F_EmailAddress.Text = String.Empty Then
            Conn = Conn & " and c.Email LIKE " & DB.FilterQuote(F_EmailAddress.Text)
        End If
        If Not F_Phone.Text = String.Empty Then
            Conn = Conn & " and c.Phone LIKE " & DB.FilterQuote(F_Phone.Text)
        End If
        If Not F_Address.Text = String.Empty Then
            Conn = Conn & " and Address LIKE " & DB.FilterQuote(F_Address.Text)
        End If
        If Not F_Username.Text = String.Empty Then
            Conn = Conn & " and Username LIKE " & DB.FilterQuote(F_Username.Text)
        End If
        Dim arrCusNo As String()
        If Not F_CustomerNo.Text = String.Empty Then
            Try
                arrCusNo = F_CustomerNo.Text.Split(",")
                If arrCusNo.Length > 1 Then
                    Dim listCusNo As String = ""
                    For i As Integer = 0 To arrCusNo.Length - 1
                        listCusNo &= "'" & Trim(arrCusNo(i)) & "',"
                    Next
                    Conn = Conn & " and c.CustomerNo in( " & Left(listCusNo, listCusNo.Length - 1) & ")"
                Else
                    Conn = Conn & " and c.CustomerNo LIKE " & DB.FilterQuote(F_CustomerNo.Text)
                End If
            Catch ex As Exception

            End Try

        End If
        If Not F_IsActive.SelectedValue = String.Empty Then
            Conn = Conn & " and IsActive  = " & DB.Number(F_IsActive.SelectedValue)
        End If
        If Not F_DeActive.SelectedValue = String.Empty Then
            Conn = Conn & " and DeActive  = " & DB.Number(F_DeActive.SelectedValue)
        End If
        If Not F_CreateDateLbound.Text = String.Empty Then
            Conn = Conn & " and m.CreateDate >= " & DB.Quote(F_CreateDateLbound.Text)
        End If
        If Not F_CreateDateUbound.Text = String.Empty Then
            Conn = Conn & " and m.CreateDate < " & DB.Quote(DateAdd("d", 1, F_CreateDateUbound.Text))
        End If
        If Not F_MemberTypeId.SelectedValue = String.Empty Then
            Conn = Conn & " and m.MemberTypeId = " & DB.Quote(F_MemberTypeId.SelectedValue)
        End If
        If Not F_ShipToCounty.SelectedValue = String.Empty And F_ShipToCountry.SelectedValue = "US" Then
            Conn = Conn & "And ma.state = " & DB.Quote(F_ShipToCounty.SelectedValue)
        End If
        If Not F_ShipToCountry.SelectedValue = String.Empty Then
            Conn = Conn & "And Country = " & DB.Quote(F_ShipToCountry.SelectedValue)
        End If
        If Not F_CustomerPostingGroup.SelectedValue = String.Empty Then
            Conn = Conn & " and c.CustomerPostingGroup = " & DB.Quote(F_CustomerPostingGroup.SelectedValue)
        End If
        If Not F_OrderDate.SelectedValue = String.Empty Then
            Dim interval As String = "day"
            Dim valDate As String = F_OrderDate.SelectedValue
            If F_OrderDate.SelectedValue = "month" Then
                ' interval = "month"
                valDate = 30
            End If
            Conn = Conn & "And DATEDIFF(" & interval & ",m.CreateDate,getdate()) <= " & valDate
        End If
        hidCon.Value = Conn

    End Sub

    Private Sub BindList()
        Dim total As Integer = 0
        Dim ds As MemberCollection = MemberRow.GetListAdmin(gvList.PageIndex + 1, gvList.PageSize, hidCon.Value, gvList.SortBy, gvList.SortOrder)
        'Dim ds As MemberCollection = MemberRow.GetListAdmin(DB, gvList.PageIndex + 1, gvList.PageSize, hidCon.Value, gvList.SortBy, gvList.SortOrder, total)
        gvList.Pager.NofRecords = ds.TotalRecords
        gvList.PageSelectIndex = gvList.PageIndex
        gvList.DataSource = ds
        gvList.DataBind()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gvList.PageIndex = 0
        'ViewState("IsSearch") = True
        BindSearch()
        BindList()
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ltrTotalPoint As Literal = DirectCast(e.Row.FindControl("ltrTotalPoint"), Literal)
            Dim ltrUserName As Literal = DirectCast(e.Row.FindControl("ltrUserName"), Literal)
            Dim lblastDate As Label = DirectCast(e.Row.FindControl("lblastDate"), Label)
            Dim CreateDate, LastDate As DateTime
            Dim m As MemberRow = e.Row.DataItem
            Dim i As Integer = m.Username.IndexOf("marketplace.amazon.com")
            ltrUserName.Text = m.Username
            If i > 0 Then
                ltrUserName.Text = m.Username.Substring(0, i) & "..."
            End If
            Try
                If Not String.IsNullOrEmpty(m.CreateDate) Then
                    CreateDate = m.CreateDate
                End If
            Catch ex As Exception
                CreateDate = Date.Now
            End Try
            Try
                If Not String.IsNullOrEmpty(m.LastLoginDate) AndAlso m.LastLoginDate <> Date.MinValue Then
                    LastDate = m.LastLoginDate
                End If
            Catch ex As Exception
                LastDate = CreateDate
            End Try
            If CreateDate.ToString("dd/MM/yyyy") <> LastDate.ToString("dd/MM/yyyy") AndAlso m.LastLoginDate <> Date.MinValue Then
                lblastDate.Text = LastDate.ToString()
            End If
            Dim memberId As Integer = 0
            Try
                memberId = CInt(m.MemberId)
            Catch ex As Exception

            End Try
            Dim point As Integer = 0
            Try
                point = CInt(m.TotalPoint)
            Catch ex As Exception

            End Try

            ltrTotalPoint.Text = "<a href='addpoint.aspx?MemberId=" & memberId & "&" & GetPageParams(Components.FilterFieldType.All) & "'>" & point & "</a>"

        End If

    End Sub
End Class

