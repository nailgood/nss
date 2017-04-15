Public Class NavisionCodes

	Public Shared Function GetCarrierType(ByVal DB As Database, ByVal CODE As String) As String
		Select Case Trim(CODE)
			Case "UPS", "56100"
				Return 1
			Case "TRUCK"
				Return 3
			Case "EXPEDITED"
				Return 4
			Case Else
				Return 1
		End Select
	End Function
    Public Shared Function ShippingTypeFedex(ByVal SeverType As Integer) As String

        Select Case SeverType
            Case 17
                Return "FedEx Two Day"
            Case 18
                Return "FedEx Next Day"
            Case 16
                Return "FedEx Ground"
        End Select
    End Function
End Class