Imports DataLayer
Imports Components
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports Humanizer

Partial Class controls_layout_about_us
    Inherits BaseControl

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadData()
        End If
    End Sub
    Private Sub LoadData()
        Dim item As InforBannerCollection = InforBannerRow.GetAllByType(Utility.Common.InforBannerType.AboutUsHome)
        If Not item Is Nothing AndAlso item.Count > 0 Then
            If item(0).Image <> String.Empty Then
                ltrImg.Text = "<img src='" & Utility.ConfigData.CDNMediaPath & Utility.ConfigData.PathBanner & item(0).Image & "' alt='Anniversary' />"
            Else
                thumb_about.Visible = True
            End If

            ltrtitle.Text &= item(0).Name
            Dim hidedesc As String = String.Empty
            Dim shortdesc As String = item(0).Description '' TruncateExtensions.Truncate(item(0).Description, 120, "", Truncator.FixedNumberOfWords)
            If (shortdesc.Contains("[break]")) Then
                Dim arr() As String = shortdesc.Split(New String() {"[break]"}, StringSplitOptions.None) ''   Dim arrMess() As String = msg.Split(New String() {splitChar}, StringSplitOptions.None)
                If (arr.Length > 0) Then
                    shortdesc = arr(0).Replace("</p>", "")
                   
                    hidedesc = arr(1).Replace("</p>", "")
                End If
            End If
            '' item(0).Description.Substring(shortdesc.Length).Replace("</p>", "")
            ltrDesc.Text = IIf(hidedesc <> String.Empty, shortdesc & "<span class='morecontent'>" & hidedesc & "</span> <span class='moreellipses'>...<img alt='Plus' src='" & Utility.ConfigData.CDNMediaPath & "/includes/theme/images/plus.png'><span></span>&nbsp;</span>", shortdesc)

            'Hardcode Chat
            ltrDesc.Text = ltrDesc.Text.Replace("achat", String.Format("achat"" onclick=""psHwEbow(); return false;"))
        End If
    End Sub
End Class
