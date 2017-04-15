Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Partial Class admin_shopdesign_Category_edit
    Inherits AdminPage
    Public Id As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Id = Request("id")
        If Not IsPostBack Then
            LoadParent()
            LoadData()
        End If
    End Sub
    Private Sub LoadParent()
        Dim condition As String = " Type=" & Utility.Common.CategoryType.ShopDesign & " AND ParentId = 0"
        Dim lstParent As CategoryCollection = CategoryRow.ListAllParent(condition)
        For i As Integer = 0 To lstParent.Count - 1
            condition = " Type=" & Utility.Common.CategoryType.ShopDesign & " AND ParentId = " & lstParent(i).CategoryId.ToString()
            Dim lstChild As CategoryCollection = CategoryRow.ListAllParent(condition)

        Next
        drlParent.DataSource = lstParent
        drlParent.DataTextField = "CategoryName"
        drlParent.DataValueField = "CategoryId"
        drlParent.DataBind()
        drlParent.Items.Insert(0, New ListItem("==All==", "0"))
    End Sub

    Private Sub LoadData()
        If Id < 1 Then
            Exit Sub
        End If
        Dim result As CategoryRow = CategoryRow.GetRow(DB, Id)
        If Not result Is Nothing Then
            txtName.Text = result.CategoryName
            txtPageTitle.Text = result.PageTitle
            txtMetaDescription.Text = result.MetaDescription
            txtMetaKeyword.Text = result.MetaKeyword
            chkIsActive.Checked = result.IsActive
            drlParent.SelectedValue = result.ParentId.ToString()
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Page.IsValid Then
            Try
                Dim data As CategoryRow
                Dim dataBefore As New CategoryRow
                Dim logdetail As New AdminLogDetailRow
                If Id > 0 Then
                    data = CategoryRow.GetRow(DB, Id)
                    dataBefore = CloneObject.Clone(data)
                Else
                    data = New CategoryRow(DB)
                End If
                data.CategoryName = txtName.Text
                data.IsActive = chkIsActive.Checked
                data.PageTitle = txtPageTitle.Text
                data.MetaDescription = txtMetaDescription.Text
                data.MetaKeyword = txtMetaKeyword.Text
                data.IsActive = chkIsActive.Checked
                data.ParentId = CInt(drlParent.SelectedValue)
                data.Type = Utility.Common.CategoryType.ShopDesign
                data.Banner = String.Empty
                If Id > 0 Then
                    CategoryRow.Update(DB, data)
                    logdetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                    logdetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.Category, dataBefore, data)
                Else
                    CategoryRow.Insert(DB, data)
                    logdetail.Action = Utility.Common.AdminLogAction.Insert.ToString()
                    logdetail.Message = AdminLogHelper.ConvertObjectInsertToLogMesssageString(data, Utility.Common.ObjectType.Category)
                End If
                logdetail.ObjectId = data.CategoryId
                logdetail.ObjectType = Utility.Common.ObjectType.Category.ToString()
                AdminLogHelper.WriteLuceneLogDetail(logdetail)
                Response.Redirect("default.aspx" & GetPageParams())

            Catch ex As Exception
                AddError(ErrHandler.ErrorText(ex))
            End Try
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx" & GetPageParams())
    End Sub
End Class

