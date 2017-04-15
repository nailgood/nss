Imports Components
Imports DataLayer
Partial Class controls_layout_barstatus
    Inherits BaseControl
    Public link As String = ""
    Protected PointReferFriend As String = ""
    Public Sub LoadData(ByVal alink As String)
        PointReferFriend = SysParam.GetValue("PointReferFriend")
        link = alink
    End Sub
End Class
