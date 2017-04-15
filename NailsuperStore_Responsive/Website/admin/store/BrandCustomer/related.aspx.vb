Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System

Partial Class admin_store_itemenable_related
    Inherits AdminPage

    Protected params As String
    Protected MemberId As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        params = GetPageParams(FilterFieldType.All)
        If Request("MemberId") <> Nothing Then
            If IsNumeric(Request("MemberId")) Then
                MemberId = Convert.ToInt32(Request("MemberId"))
            End If
        End If
        If Not IsPostBack Then
            BindDataGrid()
        End If
    End Sub

    Private Sub BindDataGrid()

        If Not IsPostBack Then
            ViewState("F_PG") = Request("F_PG")
        End If
        If CType(ViewState("F_PG"), String) = String.Empty Then
            ViewState("F_PG") = 1
        End If
        drBrand.DataSource = DB.GetDataTable("Select brandname, brandid from storebrand").DefaultView
        drBrand.DataValueField = "BrandId"
        drBrand.DataTextField = "BrandName"
        drBrand.SelectedValue = 37
        drBrand.DataBind()
        ' BUILD QUERY
        SQL = ""
        SQL &= "select c.customerno, c.name, c.email,m.memberid, m.Username, sb.BrandName, sc.id as EnableId from customer c, Member m, StoreItemEnable sc,StoreBrand sb where c.CustomerId = m.CustomerId and sc.MemberId=m.MemberId and sb.BrandId=sc.BrandId "

        'lblItemName.Text = DB.ExecuteScalar("SELECT ItemName FROM StoreItem WHERE Itemid=" & ItemId)
        Trace.Write(SQL)
        Dim res As DataSet = DB.GetDataSet(SQL)

        myNavigator.NofRecords = res.Tables(0).Rows.Count
        myNavigator.MaxPerPage = dgList.PageSize
        myNavigator.PageNumber = Math.Max(Math.Min(CType(ViewState("F_PG"), Integer), myNavigator.NofPages), 1)
        myNavigator.DataBind()

        ViewState("F_PG") = myNavigator.PageNumber
        tblList.Visible = (myNavigator.NofRecords <> 0)
        plcNoRecords.Visible = (myNavigator.NofRecords = 0)

        dgList.DataSource = res.Tables(0).DefaultView
        dgList.CurrentPageIndex = ViewState("F_PG") - 1
        dgList.DataBind()
    End Sub

    Private Sub myNavigator_PagingEvent(ByVal sender As Object, ByVal e As Controls.NavigatorEventArgs) Handles myNavigator.NavigatorEvent
        ViewState("F_PG") = e.PageNumber
        BindDataGrid()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            If Not IsNumeric(Request.Form("MemberId")) Then
                AddError("Please enter Customer Code to add permission")
                Return
            End If


            Dim dbItem As New StoreBrandRow
            Dim dt As DataTable = StoreItemEnable.ListBrands(Convert.ToInt32(MemberId))
            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("memberbrands").ToString.Contains("," & Convert.ToInt32(drBrand.SelectedValue) & ",") Then
                    AddError("Duplicate data!")
                Else
                    dbItem.InsertItemEnable(Convert.ToInt32(drBrand.SelectedValue), Convert.ToInt32(Request.Form("MemberId")))
                End If
            Else
                dbItem.InsertItemEnable(Convert.ToInt32(drBrand.SelectedValue), Convert.ToInt32(Request.Form("MemberId")))
            End If


            Response.Redirect("related.aspx?MemberId=" & MemberId & "&" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        Catch ex As ApplicationException
            AddError(ex.Message)
        Finally
            BindDataGrid()
        End Try
    End Sub
End Class