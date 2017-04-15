Imports DataLayer

Partial Class controls_checkout_contact_us
    Inherits System.Web.UI.UserControl

    Protected isShowLiveChat As Boolean = True
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            If SysParam.GetValue("LiveChat") = 0 Then
                isShowLiveChat = False
            Else
                isShowLiveChat = True
            End If
        End If
    End Sub
End Class
