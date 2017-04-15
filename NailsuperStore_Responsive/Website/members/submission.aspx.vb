Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports System.IO
Imports System.Drawing
Imports Controls
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Data.Common

Partial Class members_submission
    Inherits SitePage
    Private arrMaxId As String = ""
    Protected cssHide As String = String.Empty
    Protected cssShow As String = "hidden"
    Private type As Integer = 0
    Protected linkRules As String = String.Empty
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            type = GetQueryString("type")
        Catch
            type = 0
        End Try
        ChangeForm(type)
        If Not IsPostBack Then
            LoadCountry(drCountry)
            LoadMemberInfor()
        End If
        nailarttrend_form.Visible = True
    End Sub
    Private Sub LoadMemberInfor()
        If HasAccess() Then
            trlogin.Visible = False
            trline.Visible = False
            Dim CurrentMemberId As Integer = 0
            Dim Member As MemberRow = Nothing
            Dim MemberBillingAddress As MemberAddressRow = Nothing
            CurrentMemberId = Session("memberId")
            If Member Is Nothing Then Member = MemberRow.GetRow(Session("MemberId"))
            MemberBillingAddress = MemberAddressRow.GetRow(DB, DB.ExecuteScalar("SELECT AddressId FROM MemberAddress WHERE MemberId=" & CurrentMemberId & " AND Label = 'Default Billing Address'"))
            txtName.Text = MemberBillingAddress.FirstName
            txtEmailAddress.Text = MemberBillingAddress.Email
        End If
    End Sub

    Protected Sub ServerCheckValidEmail(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        If Not String.IsNullOrEmpty(txtEmailAddress.Text) Then
            Dim email As String = txtEmailAddress.Text.Trim()
            If Not Utility.Common.CheckValidEmail(email) Then
                e.IsValid = False
            End If
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        lbMsg.Visible = False
        If Not IsValid Then
            txtCaptcha.Text = String.Empty
            Exit Sub
        End If
        If Not CheckCaptcha(txtCaptcha.Text.Trim()) Then
            ltCapcha.Text = "<span class='text-danger'>Please try the code shown instead again</span>"
            txtCaptcha.Text = ""
            Exit Sub
        Else
            ltCapcha.Text = ""
        End If
        Dim sMsg As String = ""
        Dim sSubject As String = "Nail Art Trends"
        Dim sYourArt As String = txtYourArt.Text
        Dim sYourSalon As String = txtYourSalon.Text
        Dim sintructions As String = txtIntructions.Text
        Dim sName As String = txtName.Text
        Dim sEmail As String = txtEmailAddress.Text
        Dim sCountry As String = drCountry.SelectedValue
        Try

            Dim OldFile As String = ""
            Dim ms As New MemberSubmissionRow
            Dim ImgError As String = ""

            If ((CheckExtensionImg(fuImage.NewFileName) = False Or CheckExtensionImg(fuImage1.NewFileName) = False Or CheckExtensionImg(fuImage2.NewFileName) = False)) Or (fuImage.NewFileName = "" And fuImage1.NewFileName = "" And fuImage2.NewFileName = "") Then
                AddError("Invalid File. Please upload a File with extension : bmp, gif, png, jpg, jpeg")
                txtCaptcha.Text = ""
                Exit Sub
            Else
                'issue request imagename = fileid + artName, must get id from membersubmissionfile add to name image
                If fuImage.NewFileName <> String.Empty Then
                    ms.FileName = returnFileName(fuImage.NewFileName)
                End If
                If fuImage1.NewFileName <> String.Empty Then
                    ms.FileName &= ";" & returnFileName(fuImage1.NewFileName)
                End If
                If fuImage2.NewFileName <> String.Empty Then
                    ms.FileName &= ";" & returnFileName(fuImage2.NewFileName)
                End If
                Dim dbEnterPrise As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Using conn As DbConnection = dbEnterPrise.CreateConnection()
                    conn.Open()
                    Dim trans As DbTransaction = conn.BeginTransaction()
                    Try
                        ' Credit the first account.
                        ms.Name = sName
                        ms.Email = sEmail
                        ms.Country = sCountry
                        ms.MemberId = Session("MemberId")
                        ms.ArtName = sYourArt
                        ms.SalonName = sYourSalon
                        ms.Instruction = sintructions

                        ms.Type = type
                        ms.Status = 0
                        ms.Insert(dbEnterPrise, trans)
                        If fuImage.NewFileName <> String.Empty Then
                            OldFile = fuImage.NewFileName
                            'fuImage.NewFileName = returnFileName("", fuImage.NewFileName, False)
                            Try
                                fuImage.NewFileName = ms.arrFileInsert.Split(";")(0) 'returnFileName(fuImage.NewFileName)
                            Catch ex As Exception

                            End Try
                            fuImage.SaveNewFile()
                            If CheckPixelImage(fuImage) = False Then
                                ImgError = OldFile ' fuImage.NewFileName
                            End If
                        End If
                        If fuImage1.NewFileName <> String.Empty Then
                            OldFile = fuImage1.NewFileName
                            'fuImage1.NewFileName = returnFileName("1", fuImage1.NewFileName, False)
                            Try
                                fuImage1.NewFileName = ms.arrFileInsert.Split(";")(1)
                            Catch ex As Exception

                            End Try
                            fuImage1.SaveNewFile()
                            If CheckPixelImage(fuImage1) = False Then
                                ImgError &= ";" & OldFile 'fuImage1.NewFileName
                            End If
                        End If
                        If fuImage2.NewFileName <> String.Empty Then
                            OldFile = fuImage2.NewFileName
                            'fuImage2.NewFileName = returnFileName("2", fuImage2.NewFileName, False)
                            Try
                                fuImage2.NewFileName = ms.arrFileInsert.Split(";")(2)
                            Catch ex As Exception

                            End Try
                            fuImage2.SaveNewFile()
                            If CheckPixelImage(fuImage2) = False Then
                                ImgError &= ";" & OldFile 'fuImage2.NewFileName
                            End If
                        End If
                        ms.arrFileInsert = Nothing
                        If ImgError <> "" Then
                            trans.Rollback()
                            ms.arrFileInsert = Nothing
                            AddError("Please input image " & IIf(Left(ImgError, 1) = ";", ImgError.Replace(Left(ImgError, 1), ""), ImgError) & " minimum 500px!")
                            txtCaptcha.Text = ""
                            Exit Sub
                        End If
                        trans.Commit()
                    Catch ex As SqlException
                        trans.Rollback()
                    End Try
                    conn.Close()
                End Using
            End If
            sMsg = "<b>Name</b>: " & sName & vbCrLf & _
           "<br><b>Email</b>: " & sEmail & vbCrLf & _
           IIf(type = 0, "<br><b>Country</b>: " & sCountry, "") & vbCrLf & _
           IIf(type = 0, "<br><b>Name of Your Nail Art Design</b>: " & txtYourArt.Text, "") & vbCrLf & _
           "<br><b>Name Salon</b>: " & txtYourSalon.Text & vbCrLf & _
           "<br><b>" & lbIntructions.Text & "</b>: " & txtIntructions.Text
            '"<br><b>File download</b>: <a href=""" & Utility.ConfigData.GlobalRefererName & Utility.ConfigData.PathUploadArtTrend & fuImage.NewFileName & """>" & fuImage.NewFileName & "</a>"
            Email.SendHTMLMail(FromEmailType.NoReply, SysParam.GetValue("ToReportSubmission"), "Admin", sSubject, sMsg)
            SetField()
            lbMsg.Visible = True

        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
            Email.SendError("ToError500", "SEND SUBMISSION", "Email = " & sEmail & "<br>Error: " & ex.ToString())
        End Try
    End Sub
    Private Sub LoadCountry(ByVal dr As DropDownList)
        Try
            dr.Items.AddRange(CountryRow.GetCountries().ToArray())
            dr.DataBind()
            dr.SelectedValue = "US"
        Catch ex As Exception
        Finally
        End Try
    End Sub
    Private Sub ChangeForm(ByVal type As Integer)
        txtIntructions.Attributes("placeholder") = "Additional Info, Instructions"
        If type = 1 Or type = 2 Then
            cssHide = "hidden"
            cssShow = "show"
            txtIntructions.Attributes("placeholder") = String.Empty
            rfvYourArt.Visible = False
            rqdrpBillingCountry.Visible = False
            Dim errmsg As String = "{0} is required"
            Select Case type
                Case 1
                    lbIntructions.Text = Resources.Msg.SubmissionIntruction1
                    lbTitle.Text = Resources.Msg.SubmissionTitle1
                    errmsg = String.Format(errmsg, Resources.Msg.SubmissionIntruction1)
                    linkRules = "/services/official-rules-red-dragon-contest.aspx"
                Case 2
                    lbIntructions.Text = Resources.Msg.SubmissionIntruction2
                    lbTitle.Text = Resources.Msg.SubmissionTitle2
                    errmsg = String.Format(errmsg, Resources.Msg.SubmissionIntruction2)
                    linkRules = "/services/official-rules-show-us-your-salon-contest.aspx"
            End Select
            rfIntructions.ErrorMessage = errmsg.Replace(":", "")
        End If
    End Sub
    'Private Function returnFileName(ByVal addName As String, ByVal FileName As String, ByVal IsExist As Boolean) As String
    '    Try

    '        Dim arrFileName As String() = FileName.Split(".")
    '        If IsExist = True Then
    '            FileName = arrFileName(0) & addName & "." & arrFileName(1)
    '        Else
    '            FileName = URLParameters.ReplaceUrl(txtYourArt.Text) & addName & "." & arrFileName(1)
    '        End If

    '        If Core.FileExists(Server.MapPath(Utility.ConfigData.PathArtTrends & FileName)) = True Then
    '            FileName = FileName.Split(".")(0) & "1" & "." & FileName.Split(".")(1)
    '            FileName = returnFileName("1", FileName, True)
    '        End If

    '    Catch ex As Exception

    '    End Try
    '    Return FileName
    'End Function
    Private Function returnFileName(ByVal FileName As String) As String
        Dim arrFileName As String() = FileName.Split(".")
        FileName = URLParameters.ReplaceUrl(LCase(txtYourArt.Text)) & "." & arrFileName(1)
        Return FileName
    End Function
    Private Function CheckExtensionImg(ByVal fileName As String) As Boolean
        Dim validFileTypes As String() = {"bmp", "gif", "png", "jpg", "jpeg"}
        Dim ext As String = Path.GetExtension(Server.MapPath(Utility.ConfigData.PathUploadArtTrend & LCase(fileName)))
        Dim isValidFile As Boolean = False
        For i As Integer = 0 To validFileTypes.Length - 1
            If ext = "." & validFileTypes(i) Then
                isValidFile = True
                Exit For
            End If
        Next
        If Not isValidFile And fileName <> "" Then
            Return False
        Else
            Return True
        End If
    End Function
    Private Function CheckPixelImage(ByVal fu As FileUpload) As Boolean
        If Core.GetWidth(Server.MapPath(fu.ImageDisplayFolder & fu.NewFileName)) < 500 And Core.GetHeight(Server.MapPath(fu.ImageDisplayFolder & fu.NewFileName)) < 500 Then
            fuImage.RemoveFileName(fu.ImageDisplayFolder, fuImage.NewFileName)
            Return False
        Else
            Return True
        End If
    End Function
    Private Sub SetField()
        If Session("memberId") Is Nothing Then
            txtName.Text = ""
            txtEmailAddress.Text = ""
        End If
        txtYourArt.Text = ""
        txtIntructions.Text = ""
        txtYourSalon.Text = ""
        txtCaptcha.Text = ""
    End Sub
End Class

