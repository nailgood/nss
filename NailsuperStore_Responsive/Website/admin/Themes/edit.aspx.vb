Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports System.IO

Public Class admin_themes_Edit
    Inherits AdminPage

    Protected ThemeId As Integer
    Protected dbTheme As ThemeRow

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ThemeId = Convert.ToInt32(Request("ThemeId"))
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        If ThemeId <> Nothing Then
            dbTheme = ThemeRow.GetRow(DB, ThemeId)
        End If

        Dim SQL As String = "select classname, cssclassid, image from themecssclass"
        rptClasses.DataSource = DB.GetDataSet(SQL)
        rptClasses.DataBind()
        chkIsActive.Enabled = False

        If ThemeId = 0 Then
            trActive.Visible = False
            btnDelete.Visible = False
            Exit Sub
        End If
        txtTheme.Text = dbTheme.Theme
        If dbTheme.Theme = "Default" Then
            txtTheme.Enabled = False
            btnDelete.Visible = False
        End If
        fuLogo.CurrentFileName = dbTheme.Logo
        fuLogoFooter.CurrentFileName = dbTheme.LogoFooter
        fuTagline.CurrentFileName = dbTheme.Tagline
        chkIsActive.Checked = dbTheme.IsActive
    End Sub

    Private Sub rptClasses_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptClasses.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim rptProperties As Repeater = e.Item.FindControl("rptProperties")
            AddHandler rptProperties.ItemDataBound, AddressOf rptProperties_ItemDataBound
            Dim lit As Literal = e.Item.FindControl("litImg")

            Dim SQL As String
            If ThemeId > 0 Then
                SQL = "select iseditable, p.propertyid, [property], DefaultValue, [Value] from theme t cross join themecssclass c left outer join themecssclassproperty p on c.cssclassid = p.cssclassid left outer join themeproperty tp on p.propertyid = tp.propertyid and t.themeid = tp.themeid where t.themeid = " & dbTheme.ThemeId & " and c.cssclassid = " & e.Item.DataItem("cssclassid") & " order by p.propertyid"
            Else
                SQL = "select iseditable, p.propertyid, [property], DefaultValue, null as [Value] from themecssclassproperty p left outer join themecssclass c on p.cssclassid = c.cssclassid where p.cssclassid = " & e.Item.DataItem("cssclassid") & " order by p.propertyid"
            End If

            If Not IsDBNull(e.Item.DataItem("image")) Then
                lit.Text = "<img src=""/assets/theme/admin/" & e.Item.DataItem("image") & """ />"
            End If

            rptProperties.DataSource = DB.GetDataSet(SQL)
            rptProperties.DataBind()
        End If
    End Sub

    Private Sub rptProperties_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            If Not dbTheme Is Nothing AndAlso dbTheme.Theme = "Default" Then
                CType(e.Item.FindControl("txtProperty"), TextBox).Enabled = False
            End If
            If CBool(e.Item.DataItem("IsEditable")) Then
                e.Item.FindControl("tdReadOnly").Visible = False
            Else
                e.Item.FindControl("tdEdit").Visible = False
            End If
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            If ThemeId <> 0 Then
                dbTheme = ThemeRow.GetRow(DB, ThemeId)
            Else
                dbTheme = New ThemeRow(DB)
            End If
            dbTheme.Theme = txtTheme.Text
            If fuLogo.NewFileName <> String.Empty Then
                fuLogo.SaveNewFile()
                dbTheme.Logo = fuLogo.NewFileName
                Core.ResizeImage(Server.MapPath(fuLogo.Folder & fuLogo.NewFileName), Server.MapPath(fuLogo.Folder & fuLogo.NewFileName), 119, 97)
            ElseIf fuLogo.MarkedToDelete Then
                dbTheme.Logo = Nothing
            End If
            If fuLogoFooter.NewFileName <> String.Empty Then
                fuLogoFooter.SaveNewFile()
                dbTheme.LogoFooter = fuLogoFooter.NewFileName
                Core.ResizeImage(Server.MapPath(fuLogoFooter.Folder & fuLogoFooter.NewFileName), Server.MapPath(fuLogoFooter.Folder & fuLogoFooter.NewFileName), 119, 97)
            ElseIf fuLogoFooter.MarkedToDelete Then
                dbTheme.LogoFooter = Nothing
            End If
            If fuTagline.NewFileName <> String.Empty Then
                fuTagline.SaveNewFile()
                dbTheme.Tagline = fuTagline.NewFileName
                Core.ResizeImage(Server.MapPath(fuTagline.Folder & fuTagline.NewFileName), Server.MapPath(fuTagline.Folder & fuTagline.NewFileName), 170, 33)
            ElseIf fuTagline.MarkedToDelete Then
                dbTheme.Tagline = Nothing
            End If

            If ThemeId <> 0 Then
                dbTheme.Update()
            Else
                ThemeId = dbTheme.Insert
            End If

            Dim SQL As String
            For Each r As RepeaterItem In rptClasses.Items
                Dim CssClassId As Integer = CType(r.FindControl("lblCssClassId"), Label).Text
                For Each ri As RepeaterItem In CType(r.FindControl("rptProperties"), Repeater).Items
                    Dim Id As Integer = CType(ri.FindControl("lblPropertyId"), Label).Text
                    Dim txt As TextBox = CType(ri.FindControl("txtProperty"), TextBox)
                    Dim tdEdit As HtmlTableCell = CType(ri.FindControl("tdEdit"), HtmlTableCell)
                    Dim span As HtmlGenericControl = CType(ri.FindControl("spanValue"), HtmlGenericControl)
                    SQL = "select top 1 propertyid from themeproperty where themeid = " & ThemeId & " and propertyid = " & Id
                    If DB.ExecuteScalar(SQL) = Nothing Then
                        SQL = "insert into themeproperty (themeid, propertyid, [value]) values (" & ThemeId & "," & Id & "," & DB.Quote(IIf(tdEdit.Visible = True, txt.Text, span.InnerHtml)) & ")"
                        DB.ExecuteSQL(SQL)
                    Else
                        If tdEdit.Visible = True Then
                            SQL = "update themeproperty set [value] = " & DB.Quote(CType(ri.FindControl("txtProperty"), TextBox).Text) & " where themeid = " & ThemeId & " and propertyid = " & Id
                            DB.ExecuteSQL(SQL)
                        End If
                    End If
                Next
            Next

            DB.CommitTransaction()

            If fuLogo.NewFileName <> String.Empty OrElse fuLogo.MarkedToDelete Then fuLogo.RemoveOldFile()
            If fuLogoFooter.NewFileName <> String.Empty OrElse fuLogoFooter.MarkedToDelete Then fuLogoFooter.RemoveOldFile()
            If fuTagline.NewFileName <> String.Empty OrElse fuTagline.MarkedToDelete Then fuTagline.RemoveOldFile()

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
        Response.Redirect("delete.aspx?ThemeId=" & ThemeId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

