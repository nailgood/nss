Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data

Namespace DataLayer

    Public Class StoreOrderShipmentTrackingRow
        Inherits StoreOrderShipmentTrackingRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal TrackingId As Integer)
            MyBase.New(DB, TrackingId)
        End Sub 'New
        Public Sub New(ByVal DB As Database, ByVal TrackingNo As String)
            MyBase.New(DB, TrackingNo)
        End Sub 'New
        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal TrackingId As Integer) As StoreOrderShipmentTrackingRow
            Dim row As StoreOrderShipmentTrackingRow

            row = New StoreOrderShipmentTrackingRow(DB, TrackingId)
            row.Load()

            Return row
        End Function
        Public Shared Function GetRowFromTrackingNo(ByVal _Database As Database, ByVal TrackingNo As String) As StoreOrderShipmentTrackingRow
            Dim row As StoreOrderShipmentTrackingRow
            row = New StoreOrderShipmentTrackingRow(_Database)
            row.LoadFromTrackingNo(TrackingNo)
            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal TrackingId As Integer)
            Dim row As StoreOrderShipmentTrackingRow

            row = New StoreOrderShipmentTrackingRow(DB, TrackingId)
            row.Remove()
        End Sub
        Public Shared Function GetTrakcingIdFromOrderNo(ByVal OrderNo As String) As Integer
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim trackingid As Integer = 0
            Try
                trackingid = db.ExecuteScalar("select Trackingid from StoreOrderShipmentTracking sost inner join StoreOrder so on sost.OrderId = so.OrderId Where OrderNo = '" & OrderNo & "'")
            Catch ex As Exception
                trackingid = 0
            End Try


            Return trackingid
        End Function
        Public Shared Function GetList(ByVal DB As Database, Optional ByVal SortBy As String = "", Optional ByVal SortOrder As String = "ASC") As DataTable
            Dim SQL As String = "select * from StoreOrderShipmentTracking"
            If Not SortBy = String.Empty Then
                SortBy = Core.ProtectParam(SortBy)
                SortOrder = Core.ProtectParam(SortOrder)

                SQL &= " order by " & SortBy & " " & SortOrder
            End If
            Return DB.GetDataTable(SQL)
        End Function

        'Custom Methods
        Public Sub CopyFromNavision(ByVal r As NavisionSalesShipmentTrackingNoRow)
            Dim dbShipment As StoreOrderShipmentRow = StoreOrderShipmentRow.GetRow(DB, r.Sales_Shipment_No)
            TrackingNo = r.Tracking_No
            ShipmentId = dbShipment.ShipmentId

            If ShipmentId = Nothing Then Exit Sub

            If TrackingId = Nothing Then
                Insert()
                dbShipment.EmailSent = True
                dbShipment.Update()
            Else
                Update()
            End If
        End Sub

    End Class

    Public MustInherit Class StoreOrderShipmentTrackingRowBase
        Private m_DB As Database
        Private m_TrackingId As Integer = Nothing
        Private m_ShipmentId As Integer = Nothing
        Private m_TrackingNo As String = Nothing
        Private m_OrderNo As String = Nothing
        Private m_OrderId As Integer = Nothing
        Private m_CreatedDate As DateTime = Nothing
        Private m_ModifiedDate As DateTime = Nothing
        Private m_ShipmentType As Integer = Nothing
        Private m_Note As String = String.Empty
        Private m_CustomerNo As String = String.Empty
        Private m_P_O_Number As Integer = Nothing
        Private m_InvoiceNumber As Integer = Nothing
        Private m_ServiceType As String = String.Empty
        Private m_NumberOfPackage As Integer = Nothing
        Private m_Weight As Double = Nothing
        Public Property TrackingId() As Integer
            Get
                Return m_TrackingId
            End Get
            Set(ByVal Value As Integer)
                m_TrackingId = Value
            End Set
        End Property

        Public Property ShipmentId() As Integer
            Get
                Return m_ShipmentId
            End Get
            Set(ByVal Value As Integer)
                m_ShipmentId = Value
            End Set
        End Property

        Public Property TrackingNo() As String
            Get
                Return m_TrackingNo
            End Get
            Set(ByVal Value As String)
                m_TrackingNo = Value
            End Set
        End Property
        Public Property OrderNo() As String
            Get
                Return m_OrderNo
            End Get
            Set(ByVal Value As String)
                m_OrderNo = Value
            End Set
        End Property
        Public Property OrderId() As Integer
            Get
                Return m_OrderId
            End Get
            Set(ByVal value As Integer)
                m_OrderId = value
            End Set
        End Property
        Public Property CreatedDate() As DateTime
            Get
                Return m_CreatedDate
            End Get
            Set(ByVal value As DateTime)
                m_CreatedDate = value
            End Set
        End Property
        Public Property ModifiedDate() As DateTime
            Get
                Return m_ModifiedDate
            End Get
            Set(ByVal value As DateTime)
                m_ModifiedDate = value
            End Set
        End Property
        Public Property ShipmentType() As Integer
            Get
                Return m_ShipmentType
            End Get
            Set(ByVal value As Integer)
                m_ShipmentType = value
            End Set
        End Property
        Public Property CustomerNo() As String
            Get
                Return m_CustomerNo
            End Get
            Set(ByVal Value As String)
                m_CustomerNo = Value
            End Set
        End Property
        Public Property P_O_Number() As Integer
            Get
                Return m_P_O_Number
            End Get
            Set(ByVal Value As Integer)
                m_P_O_Number = Value
            End Set
        End Property
        Public Property InvoiceNumber() As Integer
            Get
                Return m_InvoiceNumber
            End Get
            Set(ByVal Value As Integer)
                m_InvoiceNumber = Value
            End Set
        End Property
        Public Property ServiceType() As String
            Get
                Return m_ServiceType
            End Get
            Set(ByVal Value As String)
                m_ServiceType = Value
            End Set
        End Property
        Public Property NumberOfPackage() As Integer
            Get
                Return m_NumberOfPackage
            End Get
            Set(ByVal Value As Integer)
                m_NumberOfPackage = Value
            End Set
        End Property
        Public Property Weight() As Double
            Get
                Return m_Weight
            End Get
            Set(ByVal Value As Double)
                m_Weight = Value
            End Set
        End Property
        Public Property Note() As String
            Get
                Return m_Note
            End Get
            Set(ByVal value As String)
                m_Note = value
            End Set
        End Property
        Public Property DB() As Database
            Get
                DB = m_DB
            End Get
            Set(ByVal Value As Database)
                m_DB = Value
            End Set
        End Property

        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            m_DB = DB
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal TrackingId As Integer)
            m_DB = DB
            m_TrackingId = TrackingId
        End Sub 'New
        'Public Function GetRowFromTrackingNo(ByVal DB As Database, ByVal TrackingNo As String) As String
        '    Dim SQL As String = "SELECT * FROM StoreOrderShipmentTracking WHERE TrackingNo = '" & TrackingNo & "'"
        '    Dim result As String = String.Empty
        '    Dim r As SqlDataReader = Nothing
        '    Try
        '        r = DB.GetReader(SQL)
        '        If Not r Is Nothing Then
        '            Dim code As String = String.Empty
        '            While r.Read()
        '                Me.Load(r)
        '            End While
        '        End If
        '        Core.CloseReader(r)
        '    Catch ex As Exception
        '        Core.CloseReader(r)
        '    End Try
        '    Return result
        'End Function
        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT sost.*,so.OrderNo FROM StoreOrderShipmentTracking sost inner join StoreOrder so on sost.OrderId = so.OrderId WHERE TrackingId = " & DB.Number(TrackingId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub

        Protected Overridable Sub LoadFromTrackingNo(ByVal TrackingNo As String)
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT * FROM StoreOrderShipmentTracking WHERE TrackingNo = " & DB.Quote(TrackingNo)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                ' Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub
        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                m_TrackingId = Convert.ToInt32(r.Item("TrackingId"))
                If r.Item("ShipmentId") Is Convert.DBNull Then
                    m_OrderId = Nothing
                Else
                    m_OrderId = Convert.ToInt32(r.Item("ShipmentId"))
                End If
                m_TrackingNo = Convert.ToString(r.Item("TrackingNo"))
                If r.Item("OrderId") Is Convert.DBNull Then
                    m_OrderId = Nothing
                Else
                    m_OrderId = Convert.ToInt32(r.Item("OrderId"))
                End If
                If r.Item("OrderNo") Is Convert.DBNull Then
                    m_OrderNo = Nothing
                Else
                    m_OrderNo = Convert.ToInt32(r.Item("OrderNo"))
                End If
                If IsDBNull(r.Item("CreatedDate")) Then
                    m_CreatedDate = Nothing
                Else
                    m_CreatedDate = Convert.ToDateTime(r.Item("CreatedDate"))
                End If
                If IsDBNull(r.Item("ModifiedDate")) Then
                    m_ModifiedDate = Nothing
                Else
                    m_ModifiedDate = Convert.ToDateTime(r.Item("ModifiedDate"))
                End If
                If r.Item("ShipmentType") Is Convert.DBNull Then
                    m_ShipmentType = Nothing
                Else
                    m_ShipmentType = Convert.ToInt32(r.Item("ShipmentType"))
                End If
                If r.Item("Note") Is Convert.DBNull Then
                    m_Note = Nothing
                Else
                    m_Note = Convert.ToString(r.Item("Note"))
                End If

                'If IsDBNull(r.Item("CustomerNo")) Then
                '    m_CustomerNo = Nothing
                'Else
                '    m_CustomerNo = Convert.ToString(r.Item("m_CustomerNo"))
                'End If
                'If IsDBNull(r.Item("P_O_Number")) Then
                '    m_P_O_Number = Nothing
                'Else
                '    m_P_O_Number = Convert.ToString(r.Item("P_O_Number"))
                'End If
                'If IsDBNull(r.Item("InvoiceNumber")) Then
                '    m_InvoiceNumber = Nothing
                'Else
                '    m_InvoiceNumber = Convert.ToString(r.Item("InvoiceNumber"))
                'End If
                'If IsDBNull(r.Item("ServiceType")) Then
                '    m_ServiceType = Nothing
                'Else
                '    m_ServiceType = Convert.ToString(r.Item("ServiceType"))
                'End If
                'If IsDBNull(r.Item("NumberOfPackage")) Then
                '    m_NumberOfPackage = Nothing
                'Else
                '    m_NumberOfPackage = Convert.ToString(r.Item("NumberOfPackage"))
                'End If
                'If IsDBNull(r.Item("Weight")) Then
                '    m_Weight = Nothing
                'Else
                '    m_Weight = Convert.ToString(r.Item("Weight"))
                'End If
            Catch ex As Exception
                Throw ex

            End Try


        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String
            SQL = " INSERT INTO StoreOrderShipmentTracking (" _
            & " ShipmentId" _
            & ",TrackingNo" _
            & ",OrderId" _
            & ",ShipmentType" _
            & ",CreatedDate" _
            & ",ModifiedDate" _
             & ",Note" _
            & ") VALUES (" _
            & m_DB.NullNumber(ShipmentId) _
            & "," & m_DB.Quote(TrackingNo) _
            & "," & m_DB.NullNumber(OrderId) _
            & "," & m_DB.NullNumber(ShipmentType) _
            & "," & m_DB.Quote(CreatedDate) _
            & "," & m_DB.Quote(ModifiedDate) _
            & "," & m_DB.Quote(Note) _
            & ")"

            Dim str As String = ""
            TrackingId = m_DB.InsertSQL(SQL)

            Return TrackingId
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE StoreOrderShipmentTracking SET " _
             & " ShipmentId = " & m_DB.NullNumber(ShipmentId) _
             & ",TrackingNo = " & m_DB.Quote(TrackingNo) _
             & ",OrderId = " & m_DB.Quote(OrderId) _
             & ", ShipmentType = " & m_DB.NullNumber(ShipmentType) _
              & ",Note = " & m_DB.Quote(Note) _
               & ",ModifiedDate = " & m_DB.Quote(ModifiedDate) _
             & " WHERE TrackingId = " & m_DB.Quote(TrackingId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM StoreOrderShipmentTracking WHERE TrackingId = " & m_DB.Number(TrackingId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class StoreOrderShipmentTrackingCollection
        Inherits GenericCollection(Of StoreOrderShipmentTrackingRow)
    End Class

End Namespace


