Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_navision_mixmatch_Edit
    Inherits AdminPage

    Protected Id As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Id = Convert.ToInt32(Request("Id"))
        If Not IsPostBack Then
            LoadDepartment()
            LoadFromDB()
        End If

    End Sub

    Private Sub LoadFromDB()
     

        If Id = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbProSales As PromotionSalespriceRow = PromotionSalespriceRow.GetRow(DB, Id)
        txtSubTitle.Text = dbProSales.SubTitle
        txtMainTitle.Text = dbProSales.MainTitle
        txtLinkPage.Text = dbProSales.LinkPage
        dtStartingDate.Value = dbProSales.StartingDate
        dtEndingDate.Value = dbProSales.EndingDate
        chkIsActive.Checked = dbProSales.IsActive
        'chkIsHomePage.Checked = dbProSales.IsHomePage
        'chkResource.Checked = dbProSales.IsResource
        txtTextHtml.Text = dbProSales.TextHtml
        ddlDepartment.SelectedValue = dbProSales.DepartmentID
        fuImage.CurrentFileName = dbProSales.Image
        hpimg.ImageUrl = ConfigurationManager.AppSettings("PathPromotion") & dbProSales.Image
        If fuImage.CurrentFileName <> Nothing Then
            divImg.Visible = True
            If Right(fuImage.CurrentFileName.ToLower, 4) <> ".swf" Then  Else divImg.InnerHtml = "<script type=""text/javascript"" src=""/includes/flash/homepage.swf.js.aspx?SWF=" & Server.UrlEncode(dbProSales.Image) & """></script>"
        End If
    End Sub
    Private Sub LoadDepartment()
        Dim ds As DataSet = StoreDepartmentRow.GetMainLevelDepartments(DB)
        ddlDepartment.DataSource = ds
        ddlDepartment.DataTextField = "Name"
        ddlDepartment.DataValueField = "DepartmentId"
        ddlDepartment.DataBind()
        ddlDepartment.Items.Insert(0, New ListItem("-- Select --", 0))
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbProSales As PromotionSalespriceRow

            If Id <> 0 Then
                dbProSales = PromotionSalespriceRow.GetRow(DB, Id)
            Else
                dbProSales = New PromotionSalespriceRow(DB)
            End If
            dbProSales.SubTitle = txtSubTitle.Text
            dbProSales.MainTitle = txtMainTitle.Text
            dbProSales.LinkPage = txtLinkPage.Text
            
            dbProSales.StartingDate = dtStartingDate.Value
            dbProSales.EndingDate = dtEndingDate.Value
            dbProSales.IsActive = chkIsActive.Checked
            dbProSales.TextHtml = txtTextHtml.Text
            'dbProSales.IsHomePage = chkIsHomePage.Checked
            'dbProSales.IsResource = chkResource.Checked
            dbProSales.DepartmentID = ddlDepartment.SelectedValue
            fuImage.Width = 475
            fuImage.Height = 205
            fuImage.AutoResize = True
            If fuImage.NewFileName <> String.Empty Then

                fuImage.SaveNewFile()
                dbProSales.Image = fuImage.NewFileName
            ElseIf fuImage.MarkedToDelete Then
                dbProSales.Image = Nothing
            End If
            Dim ImagePath As String = Server.MapPath("/assets/SalesPrice/")
            Core.ResizeImage(ImagePath & fuImage.NewFileName, ImagePath & fuImage.NewFileName, 490, 490)
            'If chkResource.Checked = True Then
            '    Core.ResizeImage(ImagePath & fuImage.NewFileName, ImagePath & fuImage.NewFileName, 196, 200)
            'End If
            If Id <> 0 Then
                dbProSales.Update()
            Else
                Id = dbProSales.AutoInsert
            End If

            DB.CommitTransaction()

            If fuImage.NewFileName <> String.Empty Or fuImage.MarkedToDelete Then fuImage.RemoveOldFile()
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Response.Redirect("delete.aspx?Id=" & Id & "&" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub chkResource_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkResource.CheckedChanged
        If chkResource.Checked Then
            ddlDepartment.SelectedValue = 0
            chkIsHomePage.Checked = False
            DB.ExecuteSQL("Update PromotionSalesprice set DepartmentId = 0, IsHomePage = 0 where id = " & Id)
        End If
    End Sub

    Protected Sub chkIsHomePage_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIsHomePage.CheckedChanged
        If chkIsHomePage.Checked Then
            ddlDepartment.SelectedValue = 0
            chkResource.Checked = False
            DB.ExecuteSQL("Update PromotionSalesprice set DepartmentId = 0, IsResource = 0 where id = " & Id)
        End If
    End Sub

    Protected Sub ddlDepartment_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDepartment.SelectedIndexChanged
        If ddlDepartment.SelectedValue <> 0 Then
            chkResource.Checked = False
            chkIsHomePage.Checked = False
            DB.ExecuteSQL("Update PromotionSalesprice set IsHomePage = 0, IsResource = 0 where id = " & Id)
        End If
    End Sub
End Class

