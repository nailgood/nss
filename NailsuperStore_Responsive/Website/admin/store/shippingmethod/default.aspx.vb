Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Partial Class admin_store_UPSFee_default
    Inherits AdminPage
    Protected Subject As String = String.Empty
    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindList()
        End If
    End Sub
    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "

        SQLFields = "SELECT * "
        SQL = " FROM ShipmentMethod " & Conn & " (LOWER(LEFT(code,3)) = 'ups' OR LOWER(LEFT(code,3)) = 'fed')"

        gvList.Pager.NofRecords = DB.ExecuteScalar("SELECT COUNT(*) " & SQL)

        Dim res As DataSet = DB.GetDataSet(SQLFields & SQL)
        gvList.DataSource = res.Tables(0).DefaultView
        gvList.DataBind()
    End Sub

    Protected Sub gvList_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvList.RowCancelingEdit
        gvList.EditIndex = -1
        BindList()
    End Sub


    Protected Sub gvList_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvList.RowEditing
        ChangeHeaderText(e.NewEditIndex)
        gvList.EditIndex = e.NewEditIndex
        BindList()
    End Sub

    Protected Sub gvList_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvList.RowUpdating
        Try
            Dim row As GridViewRow = DirectCast(gvList.Rows(e.RowIndex), GridViewRow)
            Dim txtResidential As TextBox = DirectCast(row.FindControl("txtResidential"), TextBox)
            Dim txtSignature As TextBox = DirectCast(row.FindControl("txtSignature"), TextBox)
            Dim txtInsurance As TextBox = DirectCast(row.FindControl("txtInsurance"), TextBox)
            Dim txtDASResidential As TextBox = DirectCast(row.FindControl("txtDASResidential"), TextBox)
            Dim txtDASCommercial As TextBox = DirectCast(row.FindControl("txtDASCommercial"), TextBox)
            Dim txtFuelRate As TextBox = DirectCast(row.FindControl("txtFuelRate"), TextBox)
            Dim txtHazMatFee As TextBox = DirectCast(row.FindControl("txtHazMatFee"), TextBox)
            Dim lblMethodId As Label = DirectCast(row.FindControl("lblMethodId"), Label)
            gvList.EditIndex = -1
            'DB.ExecuteSQL("update ShipmentMethod set Signature = " & txtSignature.Text & " where MethodId = " & lblMethodId.Text) '  & CDbl(txtResidential.Text) & " ,Signature = " & CDbl(txtSinature.Text) & " ,Insurance = " & CDbl(txtInsurance.Text) & " where MethodId = " & CInt(lblMethodId.Text))
            DB.ExecuteSQL("update ShipmentMethod set Residential = " & txtResidential.Text & " ,Signature = " & txtSignature.Text & " ,Insurance = " & txtInsurance.Text & " ,DASResidential = " & txtDASCommercial.Text & " ,DASCommercial = " & txtDASCommercial.Text & " ,FuelRate = " & txtFuelRate.Text.Replace("%", "") & " ,HazMatFee = " & txtHazMatFee.Text & " where MethodId = " & lblMethodId.Text)
            Utility.CacheUtils.RemoveCache("ShipmentMethod_ListAll")
            BindList()
        Catch ex As Exception

        End Try


    End Sub
    Protected Function ShowIcon(ByVal Code As String, MethodId As Integer, Name As String) As String
        Dim html As String = String.Empty
        If LCase(Left(Code, 3)) = "ups" Then
            html &= "<img src='/includes/theme-admin/images/global/ico-ups.png' title='" & Name & "' alt=''/> "
        ElseIf LCase(Left(Code, 3)) = "fed" AndAlso MethodId = 16 Then
            html &= "<img src='/includes/theme-admin/images/global/ico-groundshipping.png' title='" & Name & "' alt=''/> "
        ElseIf LCase(Left(Code, 3)) = "fed" Then
            html &= "<img src='/includes/theme-admin/images/global/ico-fedex.png' title='" & Name & "' alt=''/> "
        ElseIf LCase(Left(Code, 3)) = "int" Then
            html &= "<img src='/includes/theme-admin/images/global/ico_international.png' title='" & Name & "' alt=''/> "
        ElseIf LCase(Left(Code, 3)) = "tru" Then
            html &= "<img src='/includes/theme-admin/images/global/icon-freight.png' title='" & Name & "' alt=''/> "
        End If
        Return html
    End Function
    Private Sub ChangeHeaderText(Index As Integer)
        Select Case Index
            Case 0
                Subject = "UPS 3-Day Service"
            Case 1
                Subject = "UPS 2-Day Service"
            Case 2
                Subject = "UPS Next Day Service"
            Case 3
                Subject = "Standard Shipping"
            Case 4
                Subject = "Standard Shipping"
            Case 5
                Subject = "FedEx Two Day"
            Case 6
                Subject = "FedEx Next Day"
        End Select
    End Sub
End Class
