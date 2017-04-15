Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Web.Mail
Imports System.Text.RegularExpressions
Imports Utility.Common
Imports System.Configuration
Imports Controls
Namespace Components

    Public Class Core
#Region "Input param store"
        ''input param to Store
        Public Shared Function ObjectToDB(ByVal input As DateTime) As Object
            Try
                If input = "#12:00:00 AM#" Then
                    Return DBNull.Value
                End If
                If input = DateTime.MinValue Then
                    Return DBNull.Value
                End If
                '' Return DateTime.Parse(input)
                Return input
            Catch ex As Exception
                Return DBNull.Value
            End Try
        End Function
        Public Shared Function ObjectToDB(ByVal input As String) As Object
            Try
                If String.IsNullOrEmpty(input) Then
                    Return DBNull.Value
                End If
                Return input
            Catch ex As Exception
                Return DBNull.Value
            End Try
        End Function
#End Region
        Public Shared Function ProtectParam(ByVal sInput As String)
            If sInput = String.Empty Then
                Return String.Empty
            End If
            Return Replace(sInput, ";", "")
        End Function
        Public Shared Sub CloseReader(ByVal reader As SqlDataReader)
            If Not reader Is Nothing Then
                If Not reader.IsClosed Then
                    reader.Close()
                End If
                reader = Nothing
            End If
        End Sub
        Public Shared Function ReadStringDataFormReader(ByVal reader As SqlDataReader, ByVal fieldName As String) As String
            Try
                Return Convert.ToString(reader.Item(fieldName))
            Catch ex As Exception

            End Try
            Return Nothing
        End Function
        Public Shared Sub LogError(ByVal subject As String, ByVal body As String, ByVal ex As Exception)
            Dim page As String = System.Web.HttpContext.Current.Request.Url.AbsoluteUri
            subject = subject & "-" & page
            If body = String.Empty Then
                Email.SendError("ToError500", subject, "Error: " & ex.Message & "<br><br>Trace: " & ex.StackTrace)
            Else
                Email.SendError("ToError500", subject, "Function: " & body & "<br><br>Error: " & ex.Message & "<br><br>Trace: " & ex.StackTrace)
            End If
        End Sub

        Public Shared Function OpenFile(ByVal FileFullPath As String) As String
            Dim Log As StreamReader
            Dim Contents As String
            Try
                Log = File.OpenText(FileFullPath)
                Contents = Log.ReadToEnd()
                Log.Close()
            Catch ex As Exception
                Log.Close()
                Return ""
            End Try
            Return Contents
        End Function

        Public Shared Function WriteFile(ByVal FileFullPath As String, ByVal Contents As String) As Boolean
            Dim Log As StreamWriter
            Try
                Log = New StreamWriter(FileFullPath)
                Log.WriteLine(Contents)
                Log.Close()
            Catch ex As Exception
                Log.Close()
                Return False
            End Try
            Return True
        End Function

        Public Shared Function FileExists(ByVal FileFullPath As String) As Boolean
            Dim f As New IO.FileInfo(FileFullPath)
            Return f.Exists
        End Function

        Public Shared Function GetFileExtension(ByVal FileFullPath As String) As String
            Dim f As New IO.FileInfo(FileFullPath)
            Return f.Extension
        End Function

        Public Shared Function FolderExists(ByVal FolderPath As String) As Boolean
            Dim f As New IO.DirectoryInfo(FolderPath)
            Return f.Exists
        End Function

        Public Shared Function GetCachedDataSet(ByVal DB As Database, ByVal sSQL As String, ByVal sCacheKey As String, ByVal iCacheSeconds As Integer) As DataSet
            Dim ds As DataSet
            If System.Web.HttpContext.Current.Cache(sCacheKey) Is Nothing Then
                ds = DB.GetDataSet(sSQL)
                System.Web.HttpContext.Current.Cache.Insert(sCacheKey, ds, Nothing, DateTime.Now.AddSeconds(iCacheSeconds), TimeSpan.Zero)
            Else
                ds = CType(System.Web.HttpContext.Current.Cache(sCacheKey), DataSet)
                If ds Is Nothing Then ds = DB.GetDataSet(sSQL)
            End If
            Return ds
        End Function

        Public Shared Function GetCachedDataView(ByVal DB As Database, ByVal sSQL As String, ByVal sCacheKey As String, ByVal iCacheSeconds As Integer) As DataView
            Return GetCachedDataSet(DB, sSQL, sCacheKey, iCacheSeconds).Tables(0).DefaultView
        End Function

        Public Shared Function GetCachedDBValue(ByVal DB As Database, ByVal sSQL As String, ByVal sCacheKey As String, ByVal iCacheSeconds As Integer) As Object
            Dim o As Object
            If Not System.Web.HttpContext.Current.Cache(sCacheKey) Is Nothing Then
                o = System.Web.HttpContext.Current.Cache(sCacheKey)
                If o Is Nothing Then o = DB.ExecuteScalar(sSQL)
            Else
                o = DB.ExecuteScalar(sSQL)
                System.Web.HttpContext.Current.Cache.Insert(sCacheKey, o, Nothing, DateTime.Now.AddSeconds(iCacheSeconds), TimeSpan.Zero)
            End If
            Return o
        End Function

        Public Shared Function GetCachedObject(ByVal DB As Database, ByVal Obj As Object, ByVal sCacheKey As String, ByVal iCacheSeconds As Integer) As Object
            Dim o As Object
            If Not System.Web.HttpContext.Current.Cache(sCacheKey) Is Nothing Then
                o = System.Web.HttpContext.Current.Cache(sCacheKey)
            Else
                o = Obj
                System.Web.HttpContext.Current.Cache.Insert(sCacheKey, Obj, Nothing, DateTime.Now.AddSeconds(iCacheSeconds), TimeSpan.Zero)
            End If
            Return o
        End Function

        Public Shared Function ChangeSortOrder(ByVal DB As Database, ByVal KeyField As String, ByVal TableName As String, ByVal SortField As String, ByVal WhereClause As String, ByVal KeyValue As Integer, ByVal Action As String) As Boolean
            Dim SQL As String
            Utility.CacheUtils.ClearCacheWithPrefix(TableName & "_")
            Dim iRowsAffected As Integer
            Dim res As SqlDataReader
            Dim NEXT_SORT_ORDER As String = ""
            Dim NEXT_ID As String = ""

            SQL = "SELECT top 1 " + ProtectParam(KeyField) + "," + ProtectParam(SortField) + " FROM " + ProtectParam(TableName)
            If UCase(Action) = "UP" Then
                SQL &= " WHERE " & ProtectParam(SortField) & " < "
            Else
                SQL &= " WHERE " & ProtectParam(SortField) & " > "
            End If
            SQL &= "(SELECT " & ProtectParam(SortField) & " FROM " & ProtectParam(TableName) & " WHERE " & ProtectParam(KeyField) & "=" & DB.Quote(KeyValue) & ")"
            If Not DB.IsEmpty(WhereClause) Then
                SQL &= " AND " & WhereClause
            End If
            SQL &= " order by " & ProtectParam(SortField)

            If UCase(Action) = "UP" Then
                SQL &= " DESC "
            Else
                SQL &= " ASC "
            End If

            res = DB.GetReader(SQL)
            If res.Read() Then
                NEXT_ID = res(KeyField).ToString()
                NEXT_SORT_ORDER = res(SortField).ToString()
            End If
            res.Close()
            res = Nothing

            If DB.IsEmpty(NEXT_ID) Then
                Return False
            End If

            SQL = "UPDATE " & ProtectParam(TableName) & " SET " & ProtectParam(SortField) & "=(SELECT " & ProtectParam(SortField) & " FROM " & ProtectParam(TableName) & " WHERE " & ProtectParam(KeyField) & "=" & DB.Quote(KeyValue) & ") WHERE " & ProtectParam(KeyField) & "=" & DB.Quote(NEXT_ID)
            iRowsAffected = DB.ExecuteSQL(SQL)
            If iRowsAffected = 0 Then
                Return False
            End If

            SQL = "UPDATE " & ProtectParam(TableName) & " SET " & SortField & "=" & DB.Quote(NEXT_SORT_ORDER) & " WHERE " & ProtectParam(KeyField) & "=" & DB.Quote(KeyValue)
            iRowsAffected = DB.ExecuteSQL(SQL)
            If iRowsAffected = 0 Then
                Return False
            End If

            Return True
        End Function

        Public Shared Function ChangeCase(ByVal DB As Database, ByVal s As String, ByVal LowerCaseWords As String, ByVal UpperCaseWords As String) As String
            Dim words As String() = Split(s, " ")
            Dim ignored As String() = Split(LowerCaseWords, ",")
            Dim uppercase As String() = Split(UpperCaseWords, ",")
            Dim str As String = String.Empty
            Dim c As Char() = "!@@$%^^*()_-+=[{]};:>|./?".ToCharArray()
            Dim j As Integer, IsSet As Boolean

            For i As Integer = 0 To UBound(words)
                'set word to lowercase
                Dim word As String = words(i).ToString

                If word <> "" Then
                    If Array.IndexOf(ignored, word) <> -1 Then 'if word is an ignored word
                        str &= word.ToLower

                    ElseIf Array.IndexOf(uppercase, word.ToUpper) <> -1 Then 'if word is an upper case word
                        str &= word

                    Else 'otherwise capitalize only first letter
                        j = 0
                        IsSet = False
                        While j < word.Length AndAlso Not IsSet
                            If Array.IndexOf(c, word(j)) = -1 Then
                                str &= word(j).ToString.ToUpper & Right(word.ToLower, word.Length - 1 - j)
                                IsSet = True
                            Else
                                str &= word(j).ToString
                            End If
                            j += 1
                        End While
                    End If

                    If i <> UBound(words) Then str &= " "
                End If
            Next

            Return Trim(str)
        End Function

        Public Shared Sub GetImageSize(ByVal sOriginalPath As String, ByRef Width As Integer, ByRef Height As Integer)
            Dim OriginalImg As Image = Image.FromFile(sOriginalPath)

            Width = OriginalImg.Width
            Height = OriginalImg.Height

            OriginalImg.Dispose()
            OriginalImg = Nothing
        End Sub
        Public Shared Sub CropByAnchor(ByVal sOriginalPath As String, ByVal sNewPath As String, ByVal Width As Integer, ByVal Height As Integer, ByVal Anchor As ImageAnchorPosition)
            Try
                If Not System.IO.File.Exists(sOriginalPath) Then
                    Exit Sub
                End If
                Dim imgPhoto As System.Drawing.Image = System.Drawing.Image.FromFile(sOriginalPath)
                Dim sourceWidth As Integer = imgPhoto.Width
                Dim sourceHeight As Integer = imgPhoto.Height
                Dim sourceX As Integer = 0
                Dim sourceY As Integer = 0
                Dim destX As Integer = 0
                Dim destY As Integer = 0

                Dim nPercent As Double = 0
                Dim nPercentW As Double = 0
                Dim nPercentH As Double = 0

                nPercentW = (CDbl(Width) / CDbl(sourceWidth))
                nPercentH = (CDbl(Height) / CDbl(sourceHeight))

                If nPercentH < nPercentW Then
                    nPercent = nPercentW
                    Select Case Anchor
                        Case ImageAnchorPosition.Top
                            destY = 0
                            Exit Select
                        Case ImageAnchorPosition.Bottom
                            destY = CInt(Math.Truncate(Height - (sourceHeight * nPercent)))
                            Exit Select
                        Case Else
                            destY = CInt(Math.Truncate((Height - (sourceHeight * nPercent)) / 2))
                            Exit Select
                    End Select
                Else
                    nPercent = nPercentH
                    Select Case Anchor
                        Case ImageAnchorPosition.Left
                            destX = 0
                            Exit Select
                        Case ImageAnchorPosition.Right
                            destX = CInt(Math.Truncate(Width - (sourceWidth * nPercent)))
                            Exit Select
                        Case Else
                            destX = CInt(Math.Truncate((Width - (sourceWidth * nPercent)) / 2))
                            Exit Select
                    End Select
                End If

                'newHeight = Math.Round(newHeight, 0)
                'newWidth = Math.Round(newWidth, 0)

                'Dim newPic As Bitmap = New Bitmap(Convert.ToInt32(newWidth), Convert.ToInt32(newHeight))
                'Dim gr As Graphics = Graphics.FromImage(newPic)
                'Dim MemStream As New MemoryStream

                'gr.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                'gr.DrawImage(OriginalImg, 0, 0, Convert.ToInt32(newWidth), Convert.ToInt32(newHeight))

                'OriginalImg.Dispose()
                'gr.Dispose()

                ''save tot he memory stream first
                'newPic.Save(MemStream, Imaging.ImageFormat.Jpeg)
                'newPic.Dispose()


                Dim destWidth As Integer = CInt(Math.Truncate(sourceWidth * nPercent))
                Dim destHeight As Integer = CInt(Math.Truncate(sourceHeight * nPercent))

                Dim bmPhoto As New Bitmap(Width, Height)
                '' bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution)

                Dim grPhoto As Graphics = Graphics.FromImage(bmPhoto)
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic
                Dim rect1 As New Rectangle(destX, destY, destWidth, destHeight)
                Dim rect2 As New Rectangle(sourceX, sourceY, sourceWidth, sourceHeight)
                grPhoto.DrawImage(imgPhoto, rect1, rect2, GraphicsUnit.Pixel)


                grPhoto.Dispose()
                imgPhoto.Dispose()
                'save to file
                If sOriginalPath <> sNewPath Then
                    bmPhoto.Save(sNewPath, Imaging.ImageFormat.Jpeg)
                Else
                    System.IO.File.Delete(sNewPath)
                    bmPhoto.Save(sNewPath, Imaging.ImageFormat.Jpeg)
                End If
            Catch ex As Exception

            End Try

        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="imgPhoto"></param>
        ''' <param name="Width"></param>
        ''' <param name="Height"></param>
        ''' <param name="Anchor"></param>
        ''' <param name="IsCrop">Scale and Crop</param>
        ''' <param name="IsThumbWidthOrHeight">IsCrop = false, W>0, H>0 => Thumb width or height </param>
        ''' <returns></returns>
        Public Shared Function ScaleCrop(imgPhoto As Image, Width As Integer, Height As Integer, Anchor As AnchorPosition, IsCrop As Boolean, IsThumbWidthOrHeight As Boolean) As Image
            If Width <= 0 AndAlso Height <= 0 Then
                Return imgPhoto
            End If

            Dim sourceWidth As Integer = imgPhoto.Width
            Dim sourceHeight As Integer = imgPhoto.Height
            Dim sourceX As Integer = 0
            Dim sourceY As Integer = 0
            Dim destX As Integer = -1
            Dim destY As Integer = -1
            Dim destWidth As Integer = 0
            Dim destHeight As Integer = 0


            Dim nPercent As Single = 0
            Dim nPercentW As Single = 0
            Dim nPercentH As Single = 0
            Dim nPercentAdd As Integer = 2
            ' tang % kich thuoc hinh sau khi scale
            If Width < 150 And Height < 150 Then
                nPercentAdd = 5
            End If
            If Width < 30 And Height < 30 Then
                nPercentAdd = 10
            End If

            nPercentW = (CSng(Width) / CSng(sourceWidth))
            nPercentH = (CSng(Height) / CSng(sourceHeight))

            If IsCrop Then
                ' ===== scale and crop
                If nPercentH < nPercentW Then
                    nPercent = nPercentW + ((nPercentW / 100) * nPercentAdd)
                    Select Case Anchor
                        Case AnchorPosition.Top
                            destY = -1
                            Exit Select
                        Case AnchorPosition.Bottom
                            destY = CInt(Math.Truncate(Height - (sourceHeight * nPercent))) + 1
                            Exit Select
                        Case Else
                            destY = CInt(Math.Truncate((Height - (sourceHeight * nPercent)) / 2))
                            Exit Select
                    End Select
                Else
                    nPercent = nPercentH + ((nPercentH / 100) * nPercentAdd)
                    Select Case Anchor
                        Case AnchorPosition.Left
                            destX = -1
                            Exit Select
                        Case AnchorPosition.Right
                            destX = CInt(Math.Truncate(Width - (sourceWidth * nPercent))) + 1
                            Exit Select
                        Case Else
                            destX = CInt(Math.Truncate((Width - (sourceWidth * nPercent)) / 2))
                            Exit Select
                    End Select
                End If
                destWidth = CInt(Math.Truncate(sourceWidth * nPercent))
                destHeight = CInt(Math.Truncate(sourceHeight * nPercent))
            Else
                ' ===== only scale
                If Width = 0 Then
                    Width = CInt(Math.Truncate(sourceWidth * nPercentH))

                    nPercent = nPercentH + ((nPercentH / 100) * nPercentAdd)
                    destWidth = CInt(Math.Truncate(sourceWidth * nPercent))
                    destHeight = CInt(Math.Truncate(sourceHeight * nPercent))
                ElseIf Height = 0 Then
                    Height = CInt(Math.Truncate(sourceHeight * nPercentW))

                    nPercent = nPercentW + ((nPercentW / 100) * nPercentAdd)
                    destWidth = CInt(Math.Truncate(sourceWidth * nPercent))

                    destHeight = CInt(Math.Truncate(sourceHeight * nPercent))
                Else

                    If nPercentH < nPercentW Then
                        ' width
                        If IsThumbWidthOrHeight Then
                            Height = CInt(Math.Truncate(sourceHeight * nPercentW))
                            nPercent = nPercentW + ((nPercentW / 100) * nPercentAdd)
                            destWidth = CInt(Math.Truncate(sourceWidth * nPercent))
                            destHeight = CInt(Math.Truncate(sourceHeight * nPercent))
                        Else
                            Width = CInt(Math.Truncate(sourceWidth * nPercentH))
                            nPercent = nPercentH + ((nPercentH / 100) * nPercentAdd)
                            destWidth = CInt(Math.Truncate(sourceWidth * nPercent))
                            destHeight = CInt(Math.Truncate(sourceHeight * nPercent))
                        End If
                    Else
                        ' height

                        If IsThumbWidthOrHeight Then
                            Width = CInt(Math.Truncate(sourceWidth * nPercentH))
                            nPercent = nPercentH + ((nPercentH / 100) * nPercentAdd)
                            destWidth = CInt(Math.Truncate(sourceWidth * nPercent))
                            destHeight = CInt(Math.Truncate(sourceHeight * nPercent))
                        Else
                            Height = CInt(Math.Truncate(sourceHeight * nPercentW))
                            nPercent = nPercentW + ((nPercentW / 100) * nPercentAdd)
                            destWidth = CInt(Math.Truncate(sourceWidth * nPercent))
                            destHeight = CInt(Math.Truncate(sourceHeight * nPercent))
                        End If
                    End If
                End If
            End If

            Dim bmPhoto As New Bitmap(Width, Height, PixelFormat.Format24bppRgb)

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution)
            Dim grPhoto As Graphics = Graphics.FromImage(bmPhoto)
            grPhoto.Clear(Color.Red)
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic
            grPhoto.DrawImage(imgPhoto, New Rectangle(destX, destY, destWidth, destHeight), New Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel)
            grPhoto.Dispose()

            Return bmPhoto
        End Function

        Public Shared Sub ResizeImage(ByVal sOriginalPath As String, ByVal sNewPath As String, ByVal dWidth As Double, ByVal dHeight As Double)
            If Not System.IO.File.Exists(sOriginalPath) Then
                Exit Sub
            End If

            Dim OriginalImg As Image = Image.FromFile(sOriginalPath)
            Dim inp As IntPtr = New IntPtr
            Dim newWidth As Double, newHeight As Double
            Dim oHeight As Double, oWidth As Double

            oHeight = OriginalImg.Height
            oWidth = OriginalImg.Width

            If (dWidth < oWidth And dHeight < oHeight) Then
                newHeight = oHeight * (dWidth / oWidth)
                newWidth = dWidth
                If (dHeight < newHeight) Then
                    newWidth = dWidth * (dHeight / newHeight)
                    newHeight = dHeight
                End If
            ElseIf (dWidth < oWidth) Then
                newWidth = dWidth
                newHeight = oHeight * (dWidth / oWidth)
            ElseIf (dHeight < oHeight) Then
                newHeight = dHeight
                newWidth = oWidth * (dHeight / oHeight)
            Else
                newHeight = oHeight
                newWidth = oWidth
            End If

            newHeight = Math.Round(newHeight, 0)
            newWidth = Math.Round(newWidth, 0)

            Dim newPic As Bitmap = New Bitmap(Convert.ToInt32(newWidth), Convert.ToInt32(newHeight))
            Dim gr As Graphics = Graphics.FromImage(newPic)
            Dim MemStream As New MemoryStream

            gr.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            gr.DrawImage(OriginalImg, 0, 0, Convert.ToInt32(newWidth), Convert.ToInt32(newHeight))

            OriginalImg.Dispose()
            gr.Dispose()

            'save tot he memory stream first
            newPic.Save(MemStream, Imaging.ImageFormat.Jpeg)
            newPic.Dispose()

            'load from memory stream (seekable stream)
            newPic = Image.FromStream(MemStream)

            'save to file
            If sOriginalPath <> sNewPath Then
                newPic.Save(sNewPath, Imaging.ImageFormat.Jpeg)
            Else
                System.IO.File.Delete(sNewPath)
                newPic.Save(sNewPath, Imaging.ImageFormat.Jpeg)
            End If
            newPic.Dispose()
        End Sub

        Public Shared Sub ResizeImageWithQuality(ByVal sOriginalPath As String, ByVal sNewPath As String, ByVal dWidth As Double, ByVal dHeight As Double, ByVal Quality As Integer)
            If Not System.IO.File.Exists(sOriginalPath) Then
                Exit Sub
            End If

            Dim OriginalImg As Image = Image.FromFile(sOriginalPath)
            Dim inp As IntPtr = New IntPtr
            Dim newWidth As Double, newHeight As Double
            Dim oHeight As Double, oWidth As Double

            oHeight = OriginalImg.Height
            oWidth = OriginalImg.Width

            If (dWidth < oWidth And dHeight < oHeight) Then
                newHeight = oHeight * (dWidth / oWidth)
                newWidth = dWidth
                If (dHeight < newHeight) Then
                    newWidth = dWidth * (dHeight / newHeight)
                    newHeight = dHeight
                End If
            ElseIf (dWidth < oWidth) Then
                newWidth = dWidth
                newHeight = oHeight * (dWidth / oWidth)
            ElseIf (dHeight < oHeight) Then
                newHeight = dHeight
                newWidth = oWidth * (dHeight / oHeight)
            Else
                newHeight = oHeight
                newWidth = oWidth
            End If

            newHeight = Math.Round(newHeight, 0)
            newWidth = Math.Round(newWidth, 0)

            Dim newPic As Bitmap = New Bitmap(Convert.ToInt32(newWidth), Convert.ToInt32(newHeight))
            Dim gr As Graphics = Graphics.FromImage(newPic)
            Dim MemStream As New MemoryStream

            gr.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            gr.DrawImage(OriginalImg, 0, 0, Convert.ToInt32(newWidth), Convert.ToInt32(newHeight))

            OriginalImg.Dispose()
            gr.Dispose()

            'save tot he memory stream first
            newPic.Save(MemStream, Imaging.ImageFormat.Jpeg)
            newPic.Dispose()

            'load from memory stream (seekable stream)
            newPic = Image.FromStream(MemStream)

            'save to file
            If sOriginalPath <> sNewPath Then
                SaveJpeg(sNewPath, newPic, Quality)
            Else
                System.IO.File.Delete(sNewPath)
                SaveJpeg(sNewPath, newPic, Quality)
            End If
        End Sub

        Public Shared Sub SaveJpeg(ByVal path As String, ByVal img As Image, ByVal Quality As Integer)
            If Quality < 0 OrElse Quality > 100 Then
                Throw New ArgumentOutOfRangeException("quality must be between 0 and 100.")
            End If

            Dim qualityParam As EncoderParameter = New EncoderParameter(Encoder.Quality, Quality)
            Dim jpegCodec As ImageCodecInfo = GetEncoderInfo("image/jpeg")
            Dim encoderParams As EncoderParameters = New EncoderParameters(1)
            encoderParams.Param(0) = qualityParam
            img.Save(path, jpegCodec, encoderParams)

        End Sub

        Private Shared Function GetEncoderInfo(ByVal mimeType As String) As ImageCodecInfo
            Dim codecs As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders
            Dim i As Integer = 0
            While i < codecs.Length
                If codecs(i).MimeType = mimeType Then
                    Return codecs(i)
                End If
                System.Math.Min(System.Threading.Interlocked.Increment(i), i - 1)
            End While
            Return Nothing
        End Function

        Public Shared Function Text2HTML(ByVal s As String) As String
            Dim Result As String = String.Empty

            Result = Replace(s, vbLf, "<br>")
            Result = Replace(Result, "<br><br><br>", "<br><br>")

            Return Result
        End Function

        Public Shared Function BuildFullName(ByVal FirstName As String, ByVal MiddleInitial As String, ByVal LastName As String) As String
            Dim Result As String = String.Empty

            Result = Trim(FirstName & " " & MiddleInitial)
            Result = Trim(Result & " " & LastName)

            Return Result
        End Function

        Public Shared Sub SendMail(ByVal msg As System.Net.Mail.MailMessage)
            Dim Client As System.Net.Mail.SmtpClient

            Client = New System.Net.Mail.SmtpClient()
            Client.Host = System.Configuration.ConfigurationManager.AppSettings("MailServer")
            Client.Timeout = System.Configuration.ConfigurationManager.AppSettings("MailServerTimeout")

            'Send from MailServer first and if any error occurs then then try to send from MailServerBackup
            Try
                Client.Send(msg)
            Catch ex As Exception
                Client.Host = System.Configuration.ConfigurationManager.AppSettings("MailServerBackup")
                Client.Send(msg)
            End Try
        End Sub

        Public Shared Sub SendSimpleMail(ByVal FromEmail As String, ByVal FromName As String, ByVal ToEmail As String, ByVal ToName As String, ByVal Subject As String, ByVal Body As String)
            Try
                Dim m As New MailMessage()
                m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserver") = ConfigurationManager.AppSettings("MailServer")
                m.Fields("http://schemas.microsoft.com/cdo/configuration/sendusing") = 2
                m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = 25
                m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpusessl") = "false"
                m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate") = 1
                m.Fields("http://schemas.microsoft.com/cdo/configuration/sendusername") = "no-reply@nssemail.vn"
                m.Fields("http://schemas.microsoft.com/cdo/configuration/sendpassword") = "654123@nails"
                m.To = ToEmail
                m.From = FromEmail
                m.Bcc = System.Configuration.ConfigurationManager.AppSettings("CheckMailBcc")
                m.Subject = Subject
                m.Body = Body
                m.BodyFormat = MailFormat.Text
                m.BodyEncoding = System.Text.Encoding.UTF8
                System.Web.Mail.SmtpMail.SmtpServer = ConfigurationManager.AppSettings("MailServer")
                System.Web.Mail.SmtpMail.Send(m)

                'Dim msgFrom As MailAddress = New MailAddress(FromEmail, FromName)
                'Dim msgTo As MailAddress = New MailAddress(ToEmail, ToName)
                'Dim msg As MailMessage = New MailMessage(msgFrom, msgTo)
                'msg.IsBodyHtml = False
                'msg.Subject = Subject
                'msg.Body = Body
                'msg.Bcc.Add(New MailAddress(System.Configuration.ConfigurationManager.AppSettings("CheckMailBcc"), "Long"))
                'Dim SmtpMail As SmtpClient = New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("MailServer"))
                'SmtpMail.Send(msg)
                Email.SendMailLog(ToEmail, Subject, String.Empty, String.Empty, True)
            Catch ex As Exception
                Email.SendMailLog(ToEmail, Subject, Body, ex.ToString(), False)
            End Try

        End Sub

        'Public Shared Function SendHTMLMail(ByVal FromEmail As String, ByVal FromName As String, ByVal ToEmail As String, ByVal ToName As String, ByVal Subject As String, ByVal Body As String, ByVal Bcc As String) As Boolean
        '    Try
        '        If (Utility.ConfigData.SendByGmail) Then
        '            Return Email.SendHTMLMailByGmail(Subject, Body, ToName, ToEmail, Bcc)
        '        End If
        '        Dim m As New MailMessage()
        '        m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserver") = ConfigurationManager.AppSettings("MailServer")
        '        m.Fields("http://schemas.microsoft.com/cdo/configuration/sendusing") = 2
        '        m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = 25
        '        m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpusessl") = "false"
        '        m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate") = 1
        '        m.Fields("http://schemas.microsoft.com/cdo/configuration/sendusername") = "no-reply@nssemail.vn"
        '        m.Fields("http://schemas.microsoft.com/cdo/configuration/sendpassword") = "654123@nails"
        '        If String.IsNullOrEmpty(ToName) Then
        '            m.To = ToEmail
        '        Else
        '            m.To = String.Format("""{0}"" <{1}>", ToName, ToEmail)
        '        End If

        '        If String.IsNullOrEmpty(FromName) Then
        '            m.From = FromEmail
        '        Else
        '            m.From = String.Format("""{0}"" <{1}>", FromName, FromEmail)
        '        End If

        '        m.Bcc = Bcc
        '        m.Subject = Subject
        '        m.Body = Body
        '        m.BodyFormat = MailFormat.Html
        '        m.BodyEncoding = System.Text.Encoding.UTF8
        '        System.Web.Mail.SmtpMail.SmtpServer = ConfigurationManager.AppSettings("MailServer")
        '        System.Web.Mail.SmtpMail.Send(m)

        '        'Dim msgFrom As MailAddress = New MailAddress(FromEmail, FromName)
        '        'Dim msgTo As MailAddress = New MailAddress(ToEmail, ToName)
        '        'Dim msg As MailMessage = New MailMessage(msgFrom, msgTo)
        '        'msg.IsBodyHtml = True
        '        'msg.Subject = Subject
        '        'msg.Body = Body
        '        'msg.Bcc.Add(New MailAddress(Bcc, "BCC"))
        '        'SendMail(msg)
        '        Email.SendMailLog(ToEmail, Subject, String.Empty, String.Empty, True)
        '        Return True
        '    Catch ex As Exception
        '        Email.SendMailLog(ToEmail, Subject, Body, ex.ToString, False)
        '    End Try
        '    Return False
        'End Function

        'Public Shared Sub SendHTMLMail(ByVal FromEmail As String, ByVal FromName As String, ByVal ToEmail As String, ByVal ToName As String, ByVal Subject As String, ByVal Body As String)
        '    Try
        '        If (Utility.ConfigData.SendByGmail) Then
        '            Email.SendHTMLMailByGmail(Subject, Body, ToName, ToEmail, String.Empty)
        '            Exit Sub
        '        End If
        '        Dim m As New MailMessage()
        '        m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserver") = ConfigurationManager.AppSettings("MailServer")
        '        m.Fields("http://schemas.microsoft.com/cdo/configuration/sendusing") = 2
        '        m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = 25
        '        m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpusessl") = "false"
        '        m.Fields("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate") = 1
        '        m.Fields("http://schemas.microsoft.com/cdo/configuration/sendusername") = "no-reply@nssemail.vn"
        '        m.Fields("http://schemas.microsoft.com/cdo/configuration/sendpassword") = "654123@nails"
        '        If String.IsNullOrEmpty(ToName) Then
        '            m.To = ToEmail
        '        Else
        '            m.To = String.Format("""{0}"" <{1}>", ToName, ToEmail)
        '        End If

        '        If String.IsNullOrEmpty(FromName) Then
        '            m.From = FromEmail
        '        Else
        '            m.From = String.Format("""{0}"" <{1}>", FromName, FromEmail)
        '        End If
        '        m.Bcc = System.Configuration.ConfigurationManager.AppSettings("CheckMailBcc")
        '        m.Subject = Subject
        '        m.Body = Body
        '        m.BodyFormat = MailFormat.Html
        '        m.BodyEncoding = System.Text.Encoding.UTF8
        '        System.Web.Mail.SmtpMail.SmtpServer = ConfigurationManager.AppSettings("MailServer")
        '        System.Web.Mail.SmtpMail.Send(m)

        '        'Dim msgFrom As MailAddress = New MailAddress(FromEmail, FromName)
        '        'Dim msgTo As MailAddress = New MailAddress(ToEmail, ToName)
        '        'Dim msg As MailMessage = New MailMessage(msgFrom, msgTo)
        '        'msg.IsBodyHtml = True
        '        'msg.Subject = Subject
        '        'msg.Body = Body
        '        'msg.Bcc.Add(New MailAddress(System.Configuration.ConfigurationManager.AppSettings("CheckMailBcc"), "Long"))
        '        'SendMail(msg)
        '        Email.SendMailLog(ToEmail, Subject, String.Empty, String.Empty, True)
        '    Catch ex As Exception
        '        Email.SendMailLog(ToEmail, Subject, Body, ex.ToString, False)
        '    End Try
        'End Sub

        Public Shared Sub SendSystemMail(ByVal FromEmail As String, ByVal FromName As String, ByVal ToEmail As String, ByVal ToName As String, ByVal Subject As String, ByVal Body As String)
            Try
                Dim msgFrom As System.Net.Mail.MailAddress = New System.Net.Mail.MailAddress(FromEmail, FromName)
                Dim msgTo As System.Net.Mail.MailAddress = New System.Net.Mail.MailAddress(ToEmail, ToName)
                Dim msg As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage(msgFrom, msgTo)
                msg.IsBodyHtml = True
                msg.Subject = Subject
                msg.Body = Body
                msg.Bcc.Add(New System.Net.Mail.MailAddress(System.Configuration.ConfigurationManager.AppSettings("CheckMailBcc"), "Long"))
                SendMail(msg)
            Catch ex As Exception
                Email.SendMailLog(ToEmail, Subject, Body, ex.ToString, False)
            End Try
        End Sub

        Public Shared Sub SendAttachmentMail(ByVal FromEmail As String, ByVal FromName As String, ByVal ToEmail As String, ByVal ToName As String, ByVal Subject As String, ByVal Body As String, ByVal FullAttachmentPath As String)
            Dim msgFrom As Net.Mail.MailAddress = New Net.Mail.MailAddress(FromEmail, FromName)
            Dim msgTo As Net.Mail.MailAddress = New Net.Mail.MailAddress(ToEmail, ToName)
            Dim msg As Net.Mail.MailMessage = New Net.Mail.MailMessage(msgFrom, msgTo)
            Dim att As Net.Mail.Attachment = New Net.Mail.Attachment(FullAttachmentPath)
            msg.IsBodyHtml = False
            msg.Subject = Subject
            msg.Body = Body
            msg.Attachments.Add(att)

            Dim SmtpMail As New Net.Mail.SmtpClient(System.Configuration.ConfigurationManager.AppSettings("MailServer"))
            SmtpMail.Send(msg)
        End Sub


        Public Shared Function StripDblQuote(ByVal sInput As String) As String
            If sInput = String.Empty Then Return String.Empty

            If Left(sInput, 1) = """" And Right(sInput, 1) = """" Then
                Return Mid(sInput, 2, Len(sInput) - 2)
            Else
                Return sInput
            End If
        End Function

        Public Shared Function DblQuote(ByVal s As String) As String
            Dim t

            If s = String.Empty Then
                Return ""
            Else
                t = Replace(s, """", """""")
                t = Replace(t, vbCrLf, " ")
                t = Trim(t)
                Return """" & t & """"
            End If
        End Function

        Public Shared Function QuoteCSV(ByVal sInput As String) As String
            Dim bDblQuote As Boolean = False

            If InStr(sInput, ",") > 0 Then bDblQuote = True
            If InStr(sInput, """") > 0 Then bDblQuote = True
            If InStr(sInput, vbCrLf) > 0 Then bDblQuote = True

            If bDblQuote Then
                Return DblQuote(sInput)
            Else
                Return sInput
            End If
        End Function

        Public Shared Function GenerateFileID() As String
            Dim sResult

            sResult = System.Guid.NewGuid().ToString()
            sResult = Replace(sResult, "{", "")
            sResult = Replace(sResult, "}", "")
            sResult = Replace(sResult, "-", "")

            Return Left(sResult, 32)
        End Function

        ' Strips the HTML tags from strHTML
        Public Shared Function StripHTML(ByVal sInput As String) As String
            Dim r As Regex = New Regex("<(.|\n)+?>", RegexOptions.IgnoreCase)
            Dim Result As String = String.Empty

            'Replace all HTML tag matches with the empty string
            Result = r.Replace(sInput, " ")

            'Replace all < and > with &lt; and &gt;
            Result = Replace(Result, "<", "&lt;")
            Result = Replace(Result, ">", "&gt;")
            Result = Replace(Result, "&#160;", " ")

            Return Result
        End Function

        Public Shared Function SplitSearchOR(ByVal Search As String) As String
            Dim Result As String, Result1 As String = String.Empty, Result2 As String = String.Empty, iLoop As Integer
            Dim aWords() As String, ConnStr1 As String = String.Empty, ConnStr2 As String = String.Empty

            aWords = Split(Trim(Search), " "c)

            For iLoop = LBound(aWords) To UBound(aWords)
                If Trim(aWords(iLoop)) <> String.Empty Then
                    Result1 &= ConnStr1 & DblQuote(aWords(iLoop))
                    ConnStr1 = " or "

                    Result2 &= ConnStr2 & DblQuote(aWords(iLoop))
                    ConnStr2 = " and "
                End If
            Next

            If aWords.Length >= 1 Then
                Result = "(" & Result1 & ") or (" & Result2 & ") or " & DblQuote(Search)
            Else
                Result = Result1
            End If
            Return Result
        End Function

        Public Shared Function SplitSearchAND(ByVal Search As String) As String
            Dim Result As String = String.Empty, iLoop As Integer
            Dim aWords() As String, ConnStr As String = String.Empty

            aWords = Split(Trim(Search), " "c)

            For iLoop = LBound(aWords) To UBound(aWords)
                If Trim(aWords(iLoop)) <> String.Empty Then
                    If aWords(iLoop).Length > 0 Then
                        Result &= ConnStr & DblQuote(aWords(iLoop))
                        ConnStr = " and "
                    End If

                End If
            Next

            If aWords.Length >= 1 Then
                Result = "(" & Result & ") or " & DblQuote(Search)
            End If
            Return Result
        End Function
        Public Shared Function GetWidth(ByVal Path As String) As Integer
            Try
                Dim img As Image = Image.FromFile(Path)
                Dim width As Integer = img.Size.Width
                img.Dispose()
                Return width
            Catch ex As Exception
                Return 0
            End Try
        End Function
        Public Shared Function GetHeight(ByVal Path As String) As Integer
            Try
                Dim img As Image = Image.FromFile(Path)
                Dim height As Integer = img.Size.Height
                img.Dispose()
                Return height
            Catch ex As Exception
                Return 0
            End Try
        End Function
        Public Shared Function CheckPixelImage(ByVal path As String, ByVal fu As FileUpload, ByVal width As Integer, ByVal height As Integer) As Boolean
            If Core.GetWidth(path & fu.NewFileName) < width Or Core.GetHeight(path & fu.NewFileName) < height Then
                fu.RemoveFileName(fu.ImageDisplayFolder, fu.NewFileName)
                Return False
            Else
                Return True
            End If
        End Function
    End Class

    Public Class GenericCollection(Of T)
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Overridable Sub Add(ByVal item As T)
            Me.List.Add(item)
        End Sub

        Public Function Contains(ByVal item As T) As Boolean
            Return Me.List.Contains(item)
        End Function

        Public Function IndexOf(ByVal item As T) As Integer
            Return Me.List.IndexOf(item)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal item As T)
            Me.List.Insert(Index, item)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As T
            Get
                Return CType(Me.List.Item(Index), T)
            End Get

            Set(ByVal Value As T)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal item As T)
            Me.List.Remove(item)
        End Sub
    End Class

    <Serializable()> _
    Public Class GenericSerializableCollection(Of ItemType)
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal item As ItemType)
            Me.List.Add(item)
        End Sub

        Public Function Contains(ByVal item As ItemType) As Boolean
            Return Me.List.Contains(item)
        End Function

        Public Function IndexOf(ByVal item As ItemType) As Integer
            Return Me.List.IndexOf(item)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal item As ItemType)
            Me.List.Insert(Index, item)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As ItemType
            Get
                Return CType(Me.List.Item(Index), ItemType)
            End Get

            Set(ByVal Value As ItemType)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal item As ItemType)
            Me.List.Remove(item)
        End Sub

      
    End Class

End Namespace
