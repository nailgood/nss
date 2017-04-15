Public Class LanguageCode
    Public Const English As String = "Desc"
    Public Const Vietnamese As String = "Viet"
    Public Const French As String = "French"
    Public Const Spanish As String = "Spanish"
    Public Const SouthKorea As String = "Korea"

	Public Shared Function GetLanguageCode(ByVal Code As String) As String
		Select Case Code
			Case Vietnamese
				Return Vietnamese
			Case French
				Return French
			Case Spanish
				Return Spanish
            Case SouthKorea
                Return SouthKorea
            Case Else
                Return English
        End Select
	End Function
End Class
