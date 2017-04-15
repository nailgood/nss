Imports Components

Partial Class msg
    Inherits SitePage


    Public Property Msg() As String
        Get
            Return ViewState("Msg")
        End Get
        Set(ByVal value As String)
            ViewState("Msg") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            ShowMsg()
            Session("Captcha") = Nothing
        End If
    End Sub

    Private Sub ShowMsg()
        If Not Session("Msg") Is Nothing Then
            Msg = Session("Msg")

            If Msg.Contains("##") Then
                Dim arr As String() = Msg.Split("##")
                ltrTitle.Text = arr(0)

                If arr.Length > 1 Then
                    ltrContent.Text = arr(2)
                End If
            Else
                ltrContent.Text = Msg
            End If
        End If



    End Sub

End Class
