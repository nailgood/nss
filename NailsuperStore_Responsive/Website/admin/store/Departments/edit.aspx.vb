Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_departments_Edit
    Inherits AdminPage

    Protected DepartmentId As Integer
    Protected ParentDepartmentId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        DepartmentId = Convert.ToInt32(Request("DepartmentId"))
        ParentDepartmentId = Convert.ToInt32(Request("ParentId"))
        If Not IsPostBack Then
            fuImage.ImageDisplayFolder = Utility.ConfigData.DepartmentMainImageFolder
            fuImage.Folder = Utility.ConfigData.DepartmentMainImageFolder
            fuLargeImage.ImageDisplayFolder = Utility.ConfigData.DepartmentPopupImageFolder
            fuLargeImage.Folder = Utility.ConfigData.DepartmentPopupImageFolder
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If DepartmentId > 0 Then
            Dim dbStoreDepartment As StoreDepartmentRow = StoreDepartmentRow.GetRow(DB, DepartmentId)
            If (dbStoreDepartment.ParentId = Utility.ConfigData.RootDepartmentID) Then
                'trAlternateName.Visible = True
                txtAlternateName.Text = dbStoreDepartment.AlternateName
            End If
            txtURLCode.Text = dbStoreDepartment.URLCode
            txtName.Text = dbStoreDepartment.Name
            'txtNameRewriteUrl.Text = dbStoreDepartment.NameRewriteUrl
            txtLargeImageUrl.Text = dbStoreDepartment.LargeImageUrl
            txtLargeImageAltTag.Text = dbStoreDepartment.LargeImageAltTag
            txtImageAltTag.Text = dbStoreDepartment.ImageAltTag
            txtPageTitle.Text = dbStoreDepartment.PageTitle
            txtOutsideUSPageTitle.Text = dbStoreDepartment.OutsideUSPageTitle
            txtMetaDescription.Text = dbStoreDepartment.MetaDescription
            txtOutsideUSMetaDescription.Text = dbStoreDepartment.OutsideUSMetaDescription
            txtMetaKeywords.Text = dbStoreDepartment.MetaKeywords
            txtDescription.Text = dbStoreDepartment.Description
            fuLargeImage.CurrentFileName = dbStoreDepartment.Image
            fuImage.CurrentFileName = dbStoreDepartment.LargeImage
            fuNameImage.CurrentFileName = dbStoreDepartment.NameImage
            chkIsInactive.Checked = dbStoreDepartment.IsInactive
            chkIsQuickOrder.Checked = dbStoreDepartment.IsQuickOrder
            chkIsFilter.Checked = dbStoreDepartment.IsFilter
            '' drpEffect.SelectedValue = dbStoreDepartment.BannerEffect
            ''load effect
            Dim dt As DataTable = DB.GetDataTable("Select Code,Name from SlideEffect order by Arrange ASC")
            chkEffect.DataSource = dt
            chkEffect.DataTextField = "Name"
            chkEffect.DataValueField = "Code"
            chkEffect.DataBind()
            ''get list selected
            Dim lstEffectSelect As DepartmentSlideEffectCollection = DepartmentSlideEffectRow.ListByDepartment(DB, DepartmentId)
            If Not lstEffectSelect Is Nothing Then
                If lstEffectSelect.Count > 0 Then
                    Dim isSelect As Boolean = False
                    For Each item As ListItem In chkEffect.Items
                        isSelect = False
                        For Each itemselected As DepartmentSlideEffectRow In lstEffectSelect
                            If itemselected.EffectCode.ToLower().Trim() = item.Value.Trim().ToLower() Then
                                isSelect = True
                                Exit For
                            End If
                        Next
                        item.Selected = isSelect
                    Next
                End If
            End If
            trBannerEffect.Visible = True
        Else
            If ParentDepartmentId = 0 Then
                Response.Redirect("Default.aspx")
            End If
            trBannerEffect.Visible = False
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (StoreDepartmentRow.CheckDuplicateURLCode(txtURLCode.Text.Trim(), DepartmentId)) Then
            AddError("Duplicated URL Code: " & txtURLCode.Text)
            Exit Sub
        End If
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbStoreDepartment As StoreDepartmentRow
            Dim dbStoreDepartmentBefore As New StoreDepartmentRow
            Dim logDetail As New AdminLogDetailRow
            If DepartmentId <> 0 Then
                dbStoreDepartment = StoreDepartmentRow.GetRow(DB, DepartmentId)
                dbStoreDepartmentBefore = CloneObject.Clone(dbStoreDepartment)
                dbStoreDepartmentBefore.ListEffectCode = StoreDepartmentRow.GetListSlideEffectByDepartmentId(DB, DepartmentId)
            Else
                dbStoreDepartment = New StoreDepartmentRow(DB)
            End If
            dbStoreDepartment.URLCode = txtURLCode.Text.Trim()
            dbStoreDepartment.AlternateName = txtAlternateName.Text.Trim()
            dbStoreDepartment.Name = txtName.Text.Trim()
            'dbStoreDepartment.NameRewriteUrl = txtNameRewriteUrl.Text.Trim()
            dbStoreDepartment.LargeImageUrl = txtLargeImageUrl.Text
            dbStoreDepartment.LargeImageAltTag = txtLargeImageAltTag.Text
            dbStoreDepartment.ImageAltTag = txtImageAltTag.Text
            dbStoreDepartment.PageTitle = txtPageTitle.Text.Trim()
            dbStoreDepartment.OutsideUSPageTitle = txtOutsideUSPageTitle.Text.Trim()
            dbStoreDepartment.MetaDescription = txtMetaDescription.Text.Trim()
            dbStoreDepartment.OutsideUSMetaDescription = txtOutsideUSMetaDescription.Text.Trim()
            dbStoreDepartment.MetaKeywords = txtMetaKeywords.Text.Trim()
            dbStoreDepartment.Description = txtDescription.Text.Trim()
            ''dbStoreDepartment.BannerEffect = CInt(drpEffect.SelectedValue)

            If fuLargeImage.NewFileName <> String.Empty Then
                fuLargeImage.SaveNewFile()
                dbStoreDepartment.Image = fuLargeImage.NewFileName
            ElseIf fuLargeImage.MarkedToDelete Then
                dbStoreDepartment.Image = Nothing
            End If

            If fuImage.NewFileName <> String.Empty Then
                fuImage.SaveNewFile()
                dbStoreDepartment.LargeImage = fuImage.NewFileName
            ElseIf fuImage.MarkedToDelete Then
                dbStoreDepartment.LargeImage = Nothing
            End If

            If fuNameImage.NewFileName <> String.Empty Then
                fuNameImage.SaveNewFile()
                dbStoreDepartment.NameImage = fuNameImage.NewFileName
            ElseIf fuNameImage.MarkedToDelete Then
                dbStoreDepartment.NameImage = Nothing
            End If

            dbStoreDepartment.IsInactive = chkIsInactive.Checked
            dbStoreDepartment.IsQuickOrder = chkIsQuickOrder.Checked
            dbStoreDepartment.IsFilter = chkIsFilter.Checked
            If DepartmentId <> 0 Then
                dbStoreDepartment.Update()
                ''update effect
                DepartmentSlideEffectRow.DeleteByDepartment(DB, DepartmentId)
                For Each item As ListItem In chkEffect.Items
                    If item.Selected = True Then
                        Dim data As New DepartmentSlideEffectRow
                        data.EffectCode = item.Value
                        data.DepartmentId = DepartmentId
                        DepartmentSlideEffectRow.Insert(DB, data)
                    End If
                Next
                dbStoreDepartment.ListEffectCode = StoreDepartmentRow.GetListSlideEffectByDepartmentId(DB, DepartmentId)
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                logDetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.Department, dbStoreDepartmentBefore, dbStoreDepartment)
                ''WriteLogDetail("Update Department", dbStoreDepartment)
            Else
                dbStoreDepartment.ParentId = ParentDepartmentId
                dbStoreDepartment.Insert()
                DepartmentId = dbStoreDepartment.DepartmentId

                logDetail.Action = Utility.Common.AdminLogAction.Insert.ToString()
                logDetail.Message = AdminLogHelper.ConvertObjectInsertToLogMesssageString(dbStoreDepartment, Utility.Common.ObjectType.Department)
                'DB.RollbackTransaction()
                'AddError("Cannot create departments from this screen")
            End If
            DB.CommitTransaction()
            If fuLargeImage.NewFileName <> String.Empty OrElse fuLargeImage.MarkedToDelete Then fuLargeImage.RemoveOldFile()
            If fuImage.NewFileName <> String.Empty OrElse fuImage.MarkedToDelete Then fuImage.RemoveOldFile()
            If fuNameImage.NewFileName <> String.Empty OrElse fuNameImage.MarkedToDelete Then fuNameImage.RemoveOldFile()

            logDetail.ObjectId = DepartmentId
            logDetail.ObjectType = Utility.Common.ObjectType.Department.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)

            Utility.CacheUtils.RemoveCache("mainMenuData")
            Utility.CacheUtils.RemoveCache("mainMenuId")
            Utility.CacheUtils.ClearCacheWithPrefix(StoreDepartmentRow.cachePrefixKey)

            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
End Class
