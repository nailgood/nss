Public Class ItemInfo
    Public ItemNo As String = ""
    Public Price As Double = 0
    Public ShipMethod As String = ""
    Public RushDelivery As Boolean = False
    Public DropShip As Boolean = False
    Public Status As String = ""
    Public DeliveryUpCharge As Double = 0
    Public DoNotExpedite As Boolean = False
    Public PNHExclude As Boolean = False
    Public RestrictShip As Boolean = False
    Public ExcludeFromDiscount As Boolean = False

    Public ReadOnly Property SKU() As String
        Get
            Return Left(ItemNo, 3) & "-" & Right(ItemNo, 4)
        End Get
    End Property

End Class

Public Class InventoryInfo
    Private _Quantity As Integer = 0
    Public ItemType As String = ""
    Public DropShip As Boolean = False
    Public EdpNo As String = ""
    Public PODate As String = ""
    Public SmartNumber As String = ""

    Public Property Quantity() As Integer
        Get
            If ItemType = "T1" Or ItemType = "T2" Then
                Return 1
            End If
            Return _Quantity
        End Get
        Set(ByVal Value As Integer)
            _Quantity = Value
        End Set
    End Property


    Public ReadOnly Property ShipmentDate() As Date
        Get
            If Not PODate = String.Empty AndAlso PODate <> "00000000" Then
                ShipmentDate = CDate(Mid(PODate, 5, 2) & "/" & Right(PODate, 2) & "/" & Left(PODate, 4))
            End If
        End Get
    End Property

End Class

Public Class InventoryInfoCollection
    Inherits CollectionBase

    Public Sub New()
    End Sub

    Public Sub Add(ByVal item As InventoryInfo)
        Me.List.Add(item)
    End Sub

    Public Function Contains(ByVal item As InventoryInfo) As Boolean
        Return Me.List.Contains(item)
    End Function

    Public Function IndexOf(ByVal item As InventoryInfo) As Integer
        Return Me.List.IndexOf(item)
    End Function

    Public Sub Insert(ByVal Index As Integer, ByVal item As InventoryInfo)
        Me.List.Insert(Index, item)
    End Sub

    Default Public Property Item(ByVal Index As Integer) As InventoryInfo
        Get
            Return CType(Me.List.Item(Index), InventoryInfo)
        End Get

        Set(ByVal Value As InventoryInfo)
            Me.List(Index) = Value
        End Set
    End Property

    Public Sub Remove(ByVal item As InventoryInfo)
        Me.List.Remove(item)
    End Sub

    Public Function FindByEdpNo(ByVal EdpNo As String)
        For Each item As InventoryInfo In Me.List
            If item.EDPNo = EdpNo Then
                Return item
            End If
        Next
        Return Nothing
    End Function
End Class
