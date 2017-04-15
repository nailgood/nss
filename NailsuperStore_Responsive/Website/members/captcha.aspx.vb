Imports CaptchaImage
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Random

Partial Class store_captcha
    Inherits System.Web.UI.Page

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim random As New Random
        Session("Captcha") = GenerateRandomCode(random)

        Dim ca As New CaptchaImage.CaptchaImage(Session("Captcha").ToString(), 200, 50, "Verdana")
        Me.Response.Clear()
        Me.Response.ContentType = "image/jpeg"
        ca.Image.Save(Me.Response.OutputStream, ImageFormat.Jpeg)
        ca.Dispose()
    End Sub

    Private Function GenerateRandomCode(ByVal random As Random) As String
        Dim s As String = ""
        Dim i As Integer = 0
        While i < 4
            s = [String].Concat(s, random.[Next](10).ToString())
            System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
        End While
        Return s
    End Function
End Class
