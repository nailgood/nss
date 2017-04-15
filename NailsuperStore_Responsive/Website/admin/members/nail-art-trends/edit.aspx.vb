Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports System.Net
Partial Class admin_members_nail_art_trends_edit
    Inherits AdminPage
    Protected SubmissionId As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        SubmissionId = Convert.ToInt32(Request("SubmissionId"))
        If Not IsPostBack Then
            Dim fukeName As String = ""
            Try
                fukeName = Request("f")
            Catch ex As Exception
                fukeName = ""
            End Try
            If fukeName <> "" Then
                DownloadExport(fukeName)
            End If
            LoadCountry()
            LoadFromDB()
        End If
    End Sub
    Private Sub LoadFromDB()
        If SubmissionId <= 0 Then
            Return
        End If
        Dim ms As MemberSubmissionRow = MemberSubmissionRow.GetRow(DB, SubmissionId)
        Dim arrFile As String()
        Try
            arrFile = ms.FileName.Split(";")
            For i As Integer = 0 To arrFile.Length - 1
                ltImg.Text &= "<a href=""edit.aspx?f=" & arrFile(i) & """>" & arrFile(i) & "<br>"
            Next
        Catch ex As Exception
            arrFile = Nothing
        End Try

        txtName.Text = ms.Name
        txtEmail.Text = ms.Email
        txtArtName.Text = ms.ArtName
        chkStatus.Checked = ms.Status
        txtInstruction.Text = ms.Instruction
        txtSalonName.Text = ms.SalonName
        dtSubmitdate.Value = ms.SubmittedDate.Date
        drCountry.SelectedValue = ms.Country
        'bType.Text = IIf(ms.Type = 0, "Nail Art Trends", "")
        Dim arrAdminUpload As String()
        Try
            arrAdminUpload = DB.ExecuteScalar("select stuff ((select ',' + AdminUploadFile from MemberSubmissionFile where SubmissionId = " & SubmissionId & "  FOR XML PATH('')),1,1,'')").ToString.Split(",")
        Catch ex As Exception
            arrAdminUpload = Nothing
        End Try
        If arrAdminUpload Is Nothing = False Then
            Try
                fuImage.CurrentFileName = arrAdminUpload(0)

            Catch ex As Exception

            End Try
            Try
                fuImage1.CurrentFileName = arrAdminUpload(1)

            Catch ex As Exception

            End Try
            Try
                fuImage2.CurrentFileName = arrAdminUpload(2)

            Catch ex As Exception

            End Try
        End If
        fuImage.EnableDelete = False
        fuImage1.EnableDelete = False
        fuImage2.EnableDelete = False
    End Sub
    Private Sub LoadCountry()
        Try
            drCountry.Items.AddRange(CountryRow.GetCountries().ToArray())
            drCountry.DataBind()
        Catch ex As Exception
        Finally
        End Try
    End Sub
    Private Sub DownloadExport(ByVal sFileName As String)
        Dim context As HttpContext = HttpContext.Current
        context.Response.Buffer = True
        context.Response.Clear()
        context.Response.AddHeader("content-disposition", "attachment; filename=" + sFileName)
        context.Response.ContentType = "application/octet-stream"
        context.Response.WriteFile(Server.MapPath("~/" & Utility.ConfigData.PathArtTrends + sFileName))
        context.Response.Flush()
        context.Response.Close()
    End Sub


    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim isUpload As Boolean = False
        Try
            DB.BeginTransaction()

            Dim msf As MemberSubmissionFileRow
            Dim ms As MemberSubmissionRow = MemberSubmissionRow.GetRow(DB, SubmissionId)
            Dim arrAdminFile As String()
            Try
                arrAdminFile = ms.AdminUploadFile.Split(";")
            Catch ex As Exception
                arrAdminFile = Nothing
            End Try

            Dim arrFile As String()
            Try
                arrFile = ms.FileName.Split(";")
            Catch ex As Exception
                arrFile = Nothing
            End Try
            Dim strImg As String = ""
            If fuImage.NewFileName <> String.Empty Then
                Try
                    If arrFile(0) <> Nothing Then
                        strImg = arrFile(0)
                    End If
                Catch ex As Exception

                End Try
                Try
                    If arrAdminFile(0) <> Nothing Then
                        strImg = arrAdminFile(0)
                    End If
                Catch ex As Exception

                End Try
                If strImg <> "" Then
                    msf = MemberSubmissionFileRow.GetRow(DB, strImg, SubmissionId)
                    fuImage.NewFileName = strImg
                    msf.AdminUploadFile = fuImage.NewFileName
                Else
                    msf = New MemberSubmissionFileRow()
                    fuImage.NewFileName = returnFileName(fuImage.NewFileName)
                End If


                If msf.FileId <> Nothing Then
                    msf.AdminUpload(msf.FileId, msf.AdminUploadFile)
                Else
                    msf.SubmissionId = SubmissionId
                    msf.FileName = Nothing
                    msf.Insert()
                    fuImage.NewFileName = msf.NewId & "-" & fuImage.NewFileName
                    msf.AdminUpload(msf.NewId, fuImage.NewFileName)
                End If
                fuImage.SaveNewFile()
                ResizeImage(fuImage.NewFileName)
                isUpload = True
            End If
            If fuImage1.NewFileName <> String.Empty Then
                strImg = ""
                Try
                    If arrFile(1) <> Nothing Then
                        strImg = arrFile(1)
                    End If
                Catch ex As Exception

                End Try
                Try
                    If arrAdminFile(1) <> Nothing Then
                        strImg = arrAdminFile(1)
                    End If
                Catch ex As Exception

                End Try
                If strImg <> "" Then
                    msf = MemberSubmissionFileRow.GetRow(DB, strImg, SubmissionId)
                    fuImage1.NewFileName = strImg
                Else
                    msf = New MemberSubmissionFileRow()
                    fuImage1.NewFileName = returnFileName(fuImage1.NewFileName)
                End If
                msf.AdminUploadFile = fuImage1.NewFileName
                If msf.FileId <> Nothing Then
                    msf.AdminUpload(msf.FileId, msf.AdminUploadFile)
                Else
                    msf.SubmissionId = SubmissionId
                    msf.FileName = Nothing
                    msf.Insert()
                    fuImage1.NewFileName = msf.NewId & "-" & fuImage1.NewFileName
                    msf.AdminUpload(msf.NewId, fuImage1.NewFileName)
                End If
                fuImage1.SaveNewFile()
                ResizeImage(fuImage1.NewFileName)
                isUpload = True
            End If
            If fuImage2.NewFileName <> String.Empty Then
                strImg = ""
                Try
                    If arrFile(2) <> Nothing Then
                        strImg = arrFile(2)
                    End If
                Catch ex As Exception

                End Try
                Try
                    If arrAdminFile(2) <> Nothing Then
                        strImg = arrAdminFile(2)
                    End If
                Catch ex As Exception

                End Try
                If strImg <> "" Then
                    msf = MemberSubmissionFileRow.GetRow(DB, strImg, SubmissionId)
                    fuImage2.NewFileName = strImg
                Else
                    msf = New MemberSubmissionFileRow()
                    fuImage2.NewFileName = returnFileName(fuImage2.NewFileName)
                End If
                msf.AdminUploadFile = fuImage2.NewFileName
                If msf.FileId <> Nothing Then
                    msf.AdminUpload(msf.FileId, msf.AdminUploadFile)
                Else
                    msf.SubmissionId = SubmissionId
                    msf.FileName = Nothing
                    msf.Insert()
                    fuImage2.NewFileName = msf.NewId & "-" & fuImage2.NewFileName
                    msf.AdminUpload(msf.NewId, fuImage2.NewFileName)
                End If
                fuImage2.SaveNewFile()
                ResizeImage(fuImage2.NewFileName)
                isUpload = True
            End If
            If isUpload = False Then
                lbmsg.Text = "Please input a image for upload!"
                Exit Sub
            End If
            Dim tmpTime As DateTime
            Try
                tmpTime = Format(Now, "h:mm:ss tt")
            Catch ex As Exception

            End Try
            ms.SubmittedDate = IIf(dtSubmitdate.Value = "#12:00:00 AM#", Date.Now, dtSubmitdate.Value & " " & tmpTime)
            ms.Status = chkStatus.Checked
            ms.Name = txtName.Text
            ms.SalonName = txtSalonName.Text
            ms.ArtName = txtArtName.Text
            ms.Instruction = txtInstruction.Text
            ms.Email = txtEmail.Text
            ms.Country = drCountry.SelectedValue
            ms.Update()
            DB.CommitTransaction()
        Catch ex As Exception

            AddError(ex.ToString)
        Finally
            If isUpload Then
                Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
            End If

        End Try

    End Sub
    Private Sub ResizeImage(ByVal FileName As String)
        Dim ImagePath As String = Utility.ConfigData.PathArtTrends
        Dim sRate As Double = 0
        Dim sHeight As Integer = 0
        Dim height, width As Integer
        height = Core.GetHeight(Server.MapPath(ImagePath & "fullupload\") & FileName)
        width = Core.GetWidth(Server.MapPath(ImagePath & "fullupload\") & FileName)
        If height > width Then
            sRate = CDbl(height / width)
        Else
            sRate = CDbl(width / height)
        End If
        sHeight = 500 * sRate
        Core.ResizeImage(Server.MapPath(ImagePath & "fullupload/") & FileName, Server.MapPath(ImagePath & "admin/") & FileName, 500, sHeight)
        'Core.ResizeImage(Server.MapPath(ImagePath & "fullupload/") & FileName, Server.MapPath(ImagePath & "list/") & FileName, 176, 176)
        Core.CropByAnchor(Server.MapPath(ImagePath & "fullupload/") & FileName, Server.MapPath(ImagePath & "list/") & FileName, 210, 210, Utility.Common.ImageAnchorPosition.Center)
    End Sub
    'Private Function returnFileName(ByVal addName As String,ByVal FileName As String, ByVal IsExist As Boolean) As String
    Private Function returnFileName(ByVal FileName As String) As String
        Try
            Dim arrFileName As String() = FileName.Split(".")
            'If IsExist = True Then
            '    FileName = arrFileName(0) & addName & "." & arrFileName(1)
            'Else
            '    FileName = URLParameters.ReplaceUrl(LCase(lbArtName.Text)) & addName & "." & arrFileName(1)
            'End If
            'If Core.FileExists(Server.MapPath(Utility.ConfigData.PathArtTrends & "\admin\" & FileName)) = True Then
            '    FileName = FileName.Split(".")(0) & "1" & "." & FileName.Split(".")(1)
            '    FileName = returnFileName("1", FileName, True)
            'End If
            FileName = URLParameters.ReplaceUrl(LCase(txtArtName.Text)) & "." & arrFileName(1)
        Catch ex As Exception

        End Try
        Return FileName
    End Function
End Class
