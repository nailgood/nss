Imports Components
Imports DataLayer
Imports Utility
Partial Class controls_checkout_free_gift_level
    Inherits BaseControl
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BindData()
    End Sub
    Private Sub BindData()

        Dim lstFreeGiftLevel As FreeGiftLevelCollection = FreeGiftLevelRow.GetListActive()
        If Not lstFreeGiftLevel Is Nothing AndAlso lstFreeGiftLevel.Count > 0 Then
            Dim CurrentLevel As Integer = 0
            If (CurrentLevel < 1 AndAlso Not Session("FreeGiftLevelGender") Is Nothing) Then
                CurrentLevel = Session("FreeGiftLevelGender")
            End If
            Dim index As Integer = 0
            For Each level As FreeGiftLevelRow In lstFreeGiftLevel

                If CurrentLevel = 0 Then
                    CurrentLevel = level.Id
                End If
                If (CurrentLevel = level.Id) Then
                    ltrData.Text &= "<li class='active'>" & RowIcon(index) & level.Name & " </li>"
                Else
                    ltrData.Text &= "<li>" & RowIcon(index) & "<a href='javascript:void(0);' onclick='LoadFreeGift(" & level.Id & ");'>" & level.Name & "</a></li>"
                End If
                index = index + 1

            Next
            ulOrderLevel.Visible = True
        End If


    End Sub
    Private Function RowIcon(ByVal index As Integer) As String
        If index = 0 Then
            Return String.Empty
        End If
        Return "<span>|</span>"
    End Function
End Class
