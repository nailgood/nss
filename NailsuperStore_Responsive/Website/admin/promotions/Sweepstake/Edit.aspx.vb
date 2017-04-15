Imports Components
Imports DataLayer

Partial Class admin_promotions_Sweepstake_edit
    Inherits AdminPage

    Protected Id As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Id = Convert.ToInt32(Request("Id"))
        If Not IsPostBack Then
            LoadFromDB()
        End If

    End Sub

    Private Sub LoadFromDB()
        If Id <= 0 Then
            Return
        End If
        Dim row As SweepstakeRow = SweepstakeRow.GetRow(DB, Id)
        If Not row Is Nothing Then
            With row
                txtName.Text = .Name
                txtPageTitle.Text = .PageTitle
                txtMetaKeyword.Text = .MetaKeyword
                txtMetaDescription.Text = .MetaDescription
                Dim lensDate As String = Core.ObjectToDB(.DrawingDate)
                Dim getTime As String = .DrawingDate.ToString("HH:mm:ss")
                txtTime.Text = IIf(lensDate.Length < 15 And getTime = "12:00:00", "", getTime)
                dprDrawingDate.Text = .DrawingDate.ToString("MM/dd/yyyy")
                chkIsActive.Checked = .IsActive
                txtResult.Text = .Result
                txtYoutube.Text = .YouTubeId
            End With

        End If
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim DrawingDate As DateTime = Convert.ToDateTime(dprDrawingDate.Text)
        If Page.IsValid Then
            Try
                Dim row As SweepstakeRow
                Dim rowBefore As New SweepstakeRow
                Dim logDetail As New AdminLogDetailRow
                If Id <> 0 Then
                    row = SweepstakeRow.GetRow(DB, Id)
                    rowBefore = CloneObject.Clone(row)
                Else
                    row = New SweepstakeRow(DB)
                End If
                With row
                    .Name = txtName.Text
                    .Result = txtResult.Text
                    .DrawingDate = Convert.ToDateTime(dprDrawingDate.Text & IIf(Not String.IsNullOrEmpty(txtTime.Text), " " & txtTime.Text, Nothing))
                    .IsActive = chkIsActive.Checked
                    .YouTubeId = txtYoutube.Text
                    .PageTitle = txtPageTitle.Text
                    .MetaDescription = txtMetaDescription.Text
                    .MetaKeyword = txtMetaKeyword.Text
                End With
                If Id <> 0 Then
                    SweepstakeRow.Update(DB, row)
                    logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                    logDetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.LandingPage, rowBefore, row)
                Else
                    Id = SweepstakeRow.Insert(DB, row)
                    logDetail.Action = Utility.Common.AdminLogAction.Insert.ToString()

                    logDetail.Message = AdminLogHelper.ConvertObjectInsertToLogMesssageString(row, Utility.Common.ObjectType.LandingPage)
                End If
                logDetail.ObjectId = Id
                logDetail.ObjectType = Utility.Common.ObjectType.LandingPage.ToString()
                AdminLogHelper.WriteLuceneLogDetail(logDetail)

                Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

            Catch ex As Exception
                AddError(ErrHandler.ErrorText(ex))

            End Try
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
End Class
