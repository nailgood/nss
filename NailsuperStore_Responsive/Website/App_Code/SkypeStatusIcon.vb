Imports Microsoft.VisualBasic
Imports DataLayer
Imports System.Data
Imports System.IO
Imports System.Web
Imports System.Net
Imports Utility
Public Class SkypeStatusIcon
    Public Shared Sub GetSkypeStatusImage(ByRef skypeStausImage As String, ByRef skypeName As String)
        skypeName = GetSkype()
        Dim SkypeStatus As String = String.Empty
        Dim Status As String
        Dim key As String = "ContactSkype_Status"
        Status = CType(CacheUtils.GetCache(key), String)
        If Status = Nothing Then
            Status = GetBytesFromUrl("http://mystatus.skype.com/" & skypeName & ".num")
            CacheUtils.SetCache(key, Status, 60)
        End If

        If Status = "1" Then
            skypeStausImage = "SkypeOffline.png"
        ElseIf Status = "2" Then
            skypeStausImage = "SkypeOnline.png"
        Else
            skypeStausImage = "SkypeAway.png"
        End If
        '' Return SkypeStatus
    End Sub
    Public Shared Function GetSkype() As String

        Dim res As DataTable
        Dim sEmail As String = ""
        res = ContactSkypeRow.GetAllContactSkypes()
        If res.Rows.Count > 0 Then
            Dim i As Integer
            Dim Name As String = ""
            Dim Email As String = ""
            For i = 0 To res.Rows.Count - 1
                If sEmail <> "" Then
                    sEmail += ";"
                End If
                sEmail += res.Rows(i)("skype")
            Next
        Else
            sEmail = Utility.ConfigData.SkypeDefault
        End If
        Return sEmail
    End Function
    Public Shared Function GetBytesFromUrl(ByVal url As String) As String
        Try
            Dim myReq As HttpWebRequest = WebRequest.Create(url)
            myReq.Timeout = 5000
            Dim myResp As WebResponse = myReq.GetResponse()
            Dim stream As System.IO.Stream = myResp.GetResponseStream()
            stream.ReadTimeout = 5000
            Dim Br As New BinaryReader(stream)
            Dim r1 As String = Br.ReadChar()
            Return r1
            Br.Close()
            Br = Nothing
            myResp.Close()
            myResp = Nothing
            myReq = Nothing
        Catch ex As Exception
            Return "3"
        End Try
    End Function
End Class
