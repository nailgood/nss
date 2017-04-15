Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports System.Diagnostics

Public Class admin_store_items_salesprice_Edit
    Inherits AdminPage

    Protected SalesPriceId As Integer
    Protected dbSalesPrice As New SalesPriceRow
    Protected dbItem As New StoreItemRow
    Protected MemberId As Integer = 0
    Protected lbCase As String = String.Empty
    Private salestype As String 'for case item
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsNumeric(MemberId1.Value) Then
            MemberId = MemberId1.Value
        End If
        SalesPriceId = Convert.ToInt32(Request("SalesPriceId"))
        salestype = Request("salestype")

        If SalesPriceId <> Nothing Then
            dbSalesPrice = SalesPriceRow.GetRowById(DB, SalesPriceId)
        End If
        If Request("MemberId") <> Nothing Then
            dbSalesPrice.MemberId = CInt(Request("MemberId"))
        End If
        If Request("ItemId") = Nothing Then dbItem = StoreItemRow.GetRow(DB, dbSalesPrice.ItemId) Else dbItem = StoreItemRow.GetRow(DB, CInt(Request("ItemId")))
        If Not String.IsNullOrEmpty(salestype) Then
            If salestype = 3 Then
                If dbItem.CaseQty = 0 Or dbItem.CaseQty = Nothing Then
                    dbItem = StoreItemRow.GetRow(DB, dbSalesPrice.ItemId)
                End If
                lbCase = "Case"
                'txtUnitPrice.Text = cprice
                lbCasePrice.Text = " Price = $" & dbItem.CasePrice
                lbCaseQty.Text = "  1 case = " & dbItem.CaseQty & " items"
            End If
        End If
        If dbItem.ItemId = Nothing Then Response.Redirect("/admin/store/items/")
        If Not IsPostBack Then
            LoadFromDB()
        End If


    End Sub

    Private Sub LoadFromDB()
        'drpMemberId.DataSource = MemberRow.GetAllMembers(DB)
        'drpMemberId.DataValueField = "MemberId"
        'drpMemberId.DataTextField = "Username"
        'drpMemberId.DataBind()
        'drpMemberId.Items.Insert(0, New ListItem("", ""))
        Dim member As MemberRow
        drpCustomerPriceGroupId.DataSource = CustomerPriceGroupRow.GetAllCustomerPriceGroups(DB)
        drpCustomerPriceGroupId.DataTextField = "codewithcount"
        drpCustomerPriceGroupId.DataValueField = "CustomerPriceGroupId"
        drpCustomerPriceGroupId.DataBind()
        drpCustomerPriceGroupId.Items.Insert(0, New ListItem("", ""))

        If SalesPriceId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        drpCustomerPriceGroupId.SelectedValue = dbSalesPrice.CustomerPriceGroupId
        txtSalesCode.Text = dbSalesPrice.SalesCode
        txtCurrencyCode.Text = dbSalesPrice.CurrencyCode
        txtUnitPrice.Text = dbSalesPrice.UnitPrice
        txtSalesType.Text = dbSalesPrice.SalesType
        txtMinimumQuantity.Text = dbSalesPrice.MinimumQuantity
        txtUnitOfMeasureCode.Text = dbSalesPrice.UnitOfMeasureCode
        txtVariantCode.Text = dbSalesPrice.VariantCode
        txtUnitPriceIncludingVAT.Text = dbSalesPrice.UnitPriceIncludingVAT
        txtPriceGroupDescription.Text = dbSalesPrice.PriceGroupDescription
        dtStartingDate.Value = dbSalesPrice.StartingDate.ToShortDateString()
        dtEndingDate.Value = dbSalesPrice.EndingDate.ToShortDateString()
        'drpMemberId.SelectedValue = dbSalesPrice.MemberId
        If dbSalesPrice.MemberId > 0 Then
            member = MemberRow.GetRow(dbSalesPrice.MemberId)
            Dim str As String = "<script language=""javascript"">" _
        & "document.getElementId('').value = " & member.Username & "</scritp"
            Response.Write(str)
        End If

        rblPriceIncludesVAT.SelectedValue = dbSalesPrice.PriceIncludesVAT
        rblAllowInvoiceDisc.SelectedValue = dbSalesPrice.AllowInvoiceDisc
        rblAllowLineDisc.SelectedValue = dbSalesPrice.AllowLineDisc
        'fuImage.CurrentFileName = dbSalesPrice.Image
        'chkIsActive.Checked = dbSalesPrice.IsActive
        'litMap.Text = dbSalesPrice.ImageMap
        'hpimg.ImageUrl = ConfigurationManager.AppSettings("PathPromotion") & dbSalesPrice.Image
        'If fuImage.CurrentFileName <> Nothing Then
        '    divImg.Visible = True
        '    If Right(fuImage.CurrentFileName.ToLower, 4) <> ".swf" Then  Else divImg.InnerHtml = "<script type=""text/javascript"" src=""/includes/flash/homepage.swf.js.aspx?SWF=" & Server.UrlEncode(dbSalesPrice.Image) & """></script>"
        'End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If CInt(txtMinimumQuantity.Text) <= 0 Then
            AddError("Minimum Quantity must be greater than zero")
            Exit Sub
        ElseIf CDbl(txtUnitPrice.Text) <= 0 Then
            AddError("Sale Price must be greater than zero")
            Exit Sub
        End If

        If Not IsValid Then Exit Sub

        Try
            Dim logDetail As New AdminLogDetailRow
            Dim dbSalesPriceOld As SalesPriceRow = Nothing
            DB.BeginTransaction()
            If SalesPriceId > 0 Then
                dbSalesPrice = SalesPriceRow.GetRow(DB, SalesPriceId)
                dbSalesPriceOld = CloneObject.Clone(dbSalesPrice)
            Else
                dbSalesPrice = New SalesPriceRow(DB, SalesPriceId)
            End If


            dbSalesPrice.ItemId = dbItem.ItemId
            dbSalesPrice.SalesPriceId = SalesPriceId
            dbSalesPrice.CustomerPriceGroupId = IIf(IsNumeric(drpCustomerPriceGroupId.SelectedValue), drpCustomerPriceGroupId.SelectedValue, Nothing)
            dbSalesPrice.SalesCode = txtSalesCode.Text
            dbSalesPrice.CurrencyCode = txtCurrencyCode.Text
            dbSalesPrice.UnitPrice = txtUnitPrice.Text
            dbSalesPrice.MinimumQuantity = txtMinimumQuantity.Text
            dbSalesPrice.UnitOfMeasureCode = txtUnitOfMeasureCode.Text
            dbSalesPrice.VariantCode = txtVariantCode.Text
            dbSalesPrice.UnitPriceIncludingVAT = Nothing 'txtUnitPriceIncludingVAT.Text
            dbSalesPrice.PriceGroupDescription = txtPriceGroupDescription.Text
            dbSalesPrice.StartingDate = IIf(dtStartingDate.Value <> DateTime.MinValue, dtStartingDate.Value, Nothing)
            dbSalesPrice.EndingDate = IIf(dtEndingDate.Value <> DateTime.MinValue, dtEndingDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59), Nothing)
            'dbSalesPrice.MemberId = IIf(IsNumeric(drpMemberId.SelectedValue), drpMemberId.SelectedValue, Nothing)
            dbSalesPrice.MemberId = MemberId
            dbSalesPrice.PriceIncludesVAT = False 'rblPriceIncludesVAT.SelectedValue
            dbSalesPrice.AllowInvoiceDisc = False 'rblAllowInvoiceDisc.SelectedValue
            dbSalesPrice.AllowLineDisc = False 'rblAllowLineDisc.SelectedValue
            'dbSalesPrice.Image = fuImage.CurrentFileName
            'If dbSalesPrice.MemberId = Nothing AndAlso dbSalesPrice.CustomerPriceGroupId = Nothing Then
            '    dbSalesPrice.SalesType = 2
            'ElseIf dbSalesPrice.CustomerPriceGroupId = Nothing Then
            '    dbSalesPrice.SalesType = 0
            'Else
            '    dbSalesPrice.SalesType = 1
            'End If

            If dbSalesPrice.MemberId = Nothing AndAlso dbSalesPrice.CustomerPriceGroupId = Nothing Then
                dbSalesPrice.SalesType = 2
            ElseIf dbSalesPrice.MemberId <> Nothing AndAlso dbSalesPrice.CustomerPriceGroupId = Nothing Then
                dbSalesPrice.SalesType = 1
            ElseIf dbSalesPrice.CustomerPriceGroupId = Nothing Then
                dbSalesPrice.SalesType = 0
            End If
            If salestype = 3 Then
                dbSalesPrice.SalesType = salestype
            End If
            'dbSalesPrice.IsActive = chkIsActive.Checked


            If SalesPriceId <> 0 Then
                dbSalesPriceOld = SalesPriceRow.GetRow(DB, SalesPriceId)
                dbSalesPrice.Update()
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                logDetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.SalesPrice, dbSalesPriceOld, dbSalesPrice)

            Else
                dbSalesPrice.Type = 1 '1-Add from website, 0-Add from NAVISION
                SalesPriceId = dbSalesPrice.Insert
                logDetail.Action = Utility.Common.AdminLogAction.Insert.ToString()
                logDetail.Message = AdminLogHelper.ConvertObjectInsertToLogMesssageString(dbSalesPrice, Utility.Common.ObjectType.SalesPrice)
            End If
            'If SalesPriceId <> 0 Then
            '    Dim arr As String()
            '    fuImage.Width = 475
            '    fuImage.Height = 205
            '    fuImage.AutoResize = True
            '    If fuImage.NewFileName <> String.Empty Then
            '        arr = fuImage.NewFileName.Split(".")
            '        fuImage.NewFileName = SalesPriceId.ToString & "." & arr(1)
            '        fuImage.SaveNewFile()
            '        dbSalesPrice.Image = fuImage.NewFileName
            '        dbSalesPrice.Update()
            '    ElseIf fuImage.MarkedToDelete Then
            '        dbSalesPrice.Image = Nothing
            '        dbSalesPrice.Update()
            '    End If
            'End If

            DB.CommitTransaction()
            '''''run tool ExportLuceneItem
            ProcessExportLucene()

            logDetail.ObjectId = SalesPriceId
            logDetail.ObjectType = Utility.Common.ObjectType.SalesPrice.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)

            If Request.QueryString("act") = "y" Then
                Response.Redirect("/admin/promotions/salesprice/")
            Else
                Response.Redirect("default.aspx?salestype=" & salestype & "&ItemId=" & dbItem.ItemId)
            End If


        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If Request("act") = "y" Then
            Response.Redirect("../../../promotions/salesprice/default.aspx")
        Else
            Response.Redirect("default.aspx?salestype=" & salestype & "&ItemId=" & dbItem.ItemId & "&" & GetPageParams(FilterFieldType.All))
        End If

    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Response.Redirect("delete.aspx?SalesPriceId=" & SalesPriceId & "&" & GetPageParams(FilterFieldType.All))
    End Sub

    Private Sub ProcessExportLucene()
        Dim filePath As String = Utility.ConfigData.LuceneAppPath()
        If (System.IO.File.Exists(filePath)) Then
            ''set duong dan LuceneApp va time cho de tool chay export 
            Dim info As New System.Diagnostics.ProcessStartInfo(filePath, "update|" & Utility.ConfigData.LuceneAppWaitMinute())
            Dim p As New System.Diagnostics.Process()
            p.StartInfo = info
            Dim sAppName As String = "ExportLuceneItem"
            If Not filePath.Contains(sAppName & ".exe") Then
                Dim i As Integer = filePath.LastIndexOf("\")
                If i < 1 Then
                    i = filePath.LastIndexOf("/")
                End If
                If i > 0 Then
                    sAppName = filePath.Substring(i + 1)
                End If
                Dim j As Integer = sAppName.LastIndexOf(".")
                If j > 0 Then
                    sAppName = sAppName.Substring(0, j)
                End If
            End If
            If Not IsProcessOpen(sAppName) Then
                p.Start()
            End If
        End If
    End Sub
    Private Function IsProcessOpen(ByVal name As String) As Boolean
        For Each clsProcess As Process In Process.GetProcesses()
            If clsProcess.ProcessName.Contains(name) Then
                Return True
            End If
        Next
        Return False
    End Function
End Class
