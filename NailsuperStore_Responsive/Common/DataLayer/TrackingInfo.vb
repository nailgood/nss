Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components
Imports Utility
Imports System.Security.Cryptography
Imports System.Web
Imports Components.Email
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Data.Common
Imports Database
Namespace DataLayer
    Public Class TrackingInfoRow
        Inherits TrackingInfoRowBase
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal TrackingNo As String)
            MyBase.New(DB, TrackingNo)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal TrackingNo As String) As TrackingInfoRow
            Dim row As TrackingInfoRow

            row = New TrackingInfoRow(DB, TrackingNo)
            row.Load()

            Return row
        End Function
    End Class
    Public MustInherit Class TrackingInfoRowBase
        Private m_DB As Database
        Private m_TrackingNo As String = Nothing
        Private m_ShipTimestamp As DateTime = Nothing
        Private m_ActualDeliveryTimestam As DateTime = Nothing
        Private m_ActualAddress As String = String.Empty
        Private m_DestinationAddress As String = String.Empty
        Private m_StatusAddress As String = String.Empty
        Private m_TravelHistory As String = String.Empty
        Private m_Reference As String = String.Empty
        Private m_Dimensions As String = String.Empty
        Private m_OrderNumber As Integer = Nothing
        Private m_ShipmentId As Integer = Nothing
        Private m_Service As String = String.Empty
        Private m_Weight As Double = Nothing
        Private m_Pieces As Integer = Nothing
        Private m_InvoiceNumber As String = String.Empty
        Private m_Packaging As String = String.Empty
        Private m_StatusCode As String = String.Empty
        Private m_StatusDescription As String = String.Empty
        Private m_DeliverySignatureName As String = String.Empty
        Private m_StatusExceptionCode As String = String.Empty
        Private m_ShipTimestampSpecified As DateTime = Nothing
        Private m_Note As String = String.Empty
        Private m_SignatureProofOfDeliveryAvailableSpecified As String = String.Empty
        Private m_NotificationEventsAvailable As String = String.Empty
        Private m_StatusExceptionDescription As String = String.Empty
        Private m_CreatedDate As DateTime = Nothing
        Private m_ModifiedDate As DateTime = Nothing
        Private m_DoorTagNumber As String = String.Empty
        Public Property TrackingNo() As String
            Get
                Return m_TrackingNo
            End Get
            Set(ByVal Value As String)
                m_TrackingNo = Value
            End Set
        End Property
        Public Property ShipTimestamp() As DateTime
            Get
                Return m_ShipTimestamp
            End Get
            Set(ByVal Value As DateTime)
                m_ShipTimestamp = Value
            End Set
        End Property
        Public Property ActualDeliveryTimestam() As DateTime
            Get
                Return m_ActualDeliveryTimestam
            End Get
            Set(ByVal Value As DateTime)
                m_ActualDeliveryTimestam = Value
            End Set
        End Property
        Public Property ActualAddress() As String
            Get
                Return m_ActualAddress
            End Get
            Set(ByVal Value As String)
                m_ActualAddress = Value
            End Set
        End Property
        Public Property DestinationAddress() As String
            Get
                Return m_DestinationAddress
            End Get
            Set(ByVal Value As String)
                m_DestinationAddress = Value
            End Set
        End Property
        Public Property StatusAddress() As String
            Get
                Return m_StatusAddress
            End Get
            Set(ByVal Value As String)
                m_StatusAddress = Value
            End Set
        End Property
        Public Property TravelHistory() As String
            Get
                Return m_TravelHistory
            End Get
            Set(ByVal Value As String)
                m_TravelHistory = Value
            End Set
        End Property
        Public Property Reference() As String
            Get
                Return m_Reference
            End Get
            Set(ByVal Value As String)
                m_Reference = Value
            End Set
        End Property
        Public Property Dimensions() As String
            Get
                Return m_Dimensions
            End Get
            Set(ByVal Value As String)
                m_Dimensions = Value
            End Set
        End Property
        Public Property OrderNumber() As Integer
            Get
                Return m_OrderNumber
            End Get
            Set(ByVal Value As Integer)
                m_OrderNumber = Value
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
        Public Property Service() As String
            Get
                Return m_Service
            End Get
            Set(ByVal Value As String)
                m_Service = Value
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
        Public Property Pieces() As Integer
            Get
                Return m_Pieces
            End Get
            Set(ByVal Value As Integer)
                m_Pieces = Value
            End Set
        End Property
        Public Property InvoiceNumber() As String
            Get
                Return m_InvoiceNumber
            End Get
            Set(ByVal Value As String)
                m_InvoiceNumber = Value
            End Set
        End Property
        Public Property Packaging() As String
            Get
                Return m_Packaging
            End Get
            Set(ByVal Value As String)
                m_Packaging = Value
            End Set
        End Property
        Public Property StatusCode() As String
            Get
                Return m_StatusCode
            End Get
            Set(ByVal Value As String)
                m_StatusCode = Value
            End Set
        End Property
        Public Property StatusDescription() As String
            Get
                Return m_StatusDescription
            End Get
            Set(ByVal Value As String)
                m_StatusDescription = Value
            End Set
        End Property
        Public Property DeliverySignatureName() As String
            Get
                Return m_DeliverySignatureName
            End Get
            Set(ByVal Value As String)
                m_DeliverySignatureName = Value
            End Set
        End Property
        Public Property StatusExceptionCode() As String
            Get
                Return m_StatusExceptionCode
            End Get
            Set(ByVal Value As String)
                m_StatusExceptionCode = Value
            End Set
        End Property
        Public Property StatusExceptionDescription() As String
            Get
                Return m_StatusExceptionDescription
            End Get
            Set(ByVal Value As String)
                m_StatusExceptionDescription = Value
            End Set
        End Property
        Public Property ShipTimestampSpecified() As DateTime
            Get
                Return m_ShipTimestampSpecified
            End Get
            Set(ByVal Value As DateTime)
                m_ShipTimestampSpecified = Value
            End Set
        End Property
        Public Property SignatureProofOfDeliveryAvailableSpecified() As String
            Get
                Return m_SignatureProofOfDeliveryAvailableSpecified
            End Get
            Set(ByVal Value As String)
                m_SignatureProofOfDeliveryAvailableSpecified = Value
            End Set
        End Property
        Public Property NotificationEventsAvailable() As String
            Get
                Return m_NotificationEventsAvailable
            End Get
            Set(ByVal Value As String)
                m_NotificationEventsAvailable = Value
            End Set
        End Property
        Public Property DoorTagNumber() As String
            Get
                Return m_DoorTagNumber
            End Get
            Set(ByVal Value As String)
                m_DoorTagNumber = Value
            End Set
        End Property
        Public Property Note() As String
            Get
                Return m_Note
            End Get
            Set(ByVal Value As String)
                m_Note = Value
            End Set
        End Property
        Public Property CreatedDate() As DateTime
            Get
                Return m_CreatedDate
            End Get
            Set(ByVal Value As DateTime)
                m_CreatedDate = Value
            End Set
        End Property
        Public Property ModifiedDate() As DateTime
            Get
                Return m_ModifiedDate
            End Get
            Set(ByVal Value As DateTime)
                m_ModifiedDate = Value
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

        Public Sub New(ByVal DB As Database, ByVal TrackingNo As String)
            m_DB = DB
            m_TrackingNo = TrackingNo
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader
            Dim SQL As String

            SQL = "SELECT * FROM TrackingInfo WHERE TrackingNo = '" & TrackingNo & "'"
            r = m_DB.GetReader(SQL)
            If r.Read Then
                Me.Load(r)
            End If
            r.Close()
        End Sub


        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            m_TrackingNo = Convert.ToString(r.Item("TrackingNo"))
            If r.Item("ShipmentId") Is Convert.DBNull Then
                m_ShipmentId = Nothing
            Else
                m_ShipmentId = Convert.ToInt32(r.Item("ShipmentId"))
            End If
            m_TrackingNo = Convert.ToString(r.Item("TrackingNo"))
            If IsDBNull(r.Item("ShipTimestamp")) Then
                m_ShipTimestamp = Nothing
            Else
                m_ShipTimestamp = Convert.ToDateTime(r.Item("ShipTimestamp"))
            End If
            If IsDBNull(r.Item("ActualDeliveryTimestam")) Then
                m_ActualDeliveryTimestam = Nothing
            Else
                m_ActualDeliveryTimestam = Convert.ToDateTime(r.Item("ActualDeliveryTimestam"))
            End If
            If IsDBNull(r.Item("ActualAddress")) Then
                m_ActualAddress = Nothing
            Else
                m_ActualAddress = Convert.ToString(r.Item("ActualAddress"))
            End If
            If IsDBNull(r.Item("DestinationAddress")) Then
                m_DestinationAddress = Nothing
            Else
                m_DestinationAddress = Convert.ToString(r.Item("DestinationAddress"))
            End If
            If IsDBNull(r.Item("StatusAddress")) Then
                m_StatusAddress = Nothing
            Else
                m_StatusAddress = Convert.ToString(r.Item("StatusAddress"))
            End If
            If IsDBNull(r.Item("TravelHistory")) Then
                m_TravelHistory = Nothing
            Else
                m_TravelHistory = Convert.ToString(r.Item("TravelHistory"))
            End If
            If IsDBNull(r.Item("Reference")) Then
                m_Reference = Nothing
            Else
                m_Reference = Convert.ToString(r.Item("Reference"))
            End If
            If IsDBNull(r.Item("Dimensions")) Then
                m_Dimensions = Nothing
            Else
                m_Dimensions = Convert.ToString(r.Item("Dimensions"))
            End If
            If IsDBNull(r.Item("OrderNumber")) Then
                m_OrderNumber = Nothing
            Else
                m_OrderNumber = Convert.ToInt32(r.Item("OrderNumber"))
            End If
            If IsDBNull(r.Item("Service")) Then
                m_Service = Nothing
            Else
                m_Service = Convert.ToString(r.Item("Service"))
            End If
            If IsDBNull(r.Item("Weight")) Then
                m_Weight = Nothing
            Else
                m_Weight = Convert.ToDouble(r.Item("Weight"))
            End If
            If IsDBNull(r.Item("Pieces")) Then
                m_Pieces = Nothing
            Else
                m_Pieces = Convert.ToInt32(r.Item("Pieces"))
            End If
            If IsDBNull(r.Item("InvoiceNumber")) Then
                m_InvoiceNumber = Nothing
            Else
                m_InvoiceNumber = Convert.ToString(r.Item("InvoiceNumber"))
            End If
            If IsDBNull(r.Item("Packaging")) Then
                m_Packaging = Nothing
            Else
                m_Packaging = Convert.ToString(r.Item("Packaging"))
            End If
            If IsDBNull(r.Item("StatusCode")) Then
                m_StatusCode = Nothing
            Else
                m_StatusCode = Convert.ToString(r.Item("StatusCode"))
            End If
            If IsDBNull(r.Item("StatusDescription")) Then
                m_StatusDescription = Nothing
            Else
                m_StatusDescription = Convert.ToString(r.Item("StatusDescription"))
            End If
            If IsDBNull(r.Item("DeliverySignatureName")) Then
                m_DeliverySignatureName = Nothing
            Else
                m_DeliverySignatureName = Convert.ToString(r.Item("DeliverySignatureName"))
            End If
            If IsDBNull(r.Item("StatusExceptionCode")) Then
                m_StatusExceptionCode = Nothing
            Else
                m_StatusExceptionCode = Convert.ToString(r.Item("StatusExceptionCode"))
            End If
            If IsDBNull(r.Item("ShipTimestampSpecified")) Then
                m_ShipTimestampSpecified = Nothing
            Else
                m_ShipTimestampSpecified = Convert.ToDateTime(r.Item("ShipTimestampSpecified"))
            End If
            If IsDBNull(r.Item("SignatureProofOfDeliveryAvailableSpecified")) Then
                m_SignatureProofOfDeliveryAvailableSpecified = Nothing
            Else
                m_SignatureProofOfDeliveryAvailableSpecified = Convert.ToString(r.Item("SignatureProofOfDeliveryAvailableSpecified"))
            End If
            If IsDBNull(r.Item("NotificationEventsAvailable")) Then
                m_NotificationEventsAvailable = Nothing
            Else
                m_NotificationEventsAvailable = Convert.ToString(r.Item("NotificationEventsAvailable"))
            End If

            If IsDBNull(r.Item("DoorTagNumber")) Then
                m_DoorTagNumber = Nothing
            Else
                m_DoorTagNumber = Convert.ToString(r.Item("DoorTagNumber"))
            End If
            If IsDBNull(r.Item("Note")) Then
                m_Note = Nothing
            Else
                m_Note = Convert.ToString(r.Item("Note"))
            End If
            If IsDBNull(r.Item("StatusExceptionDescription")) Then
                m_StatusExceptionDescription = Nothing
            Else
                m_StatusExceptionDescription = Convert.ToString(r.Item("StatusExceptionDescription"))
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
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_TRACKINGINFO_INSERT As String = "sp_TrackingInfo_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_TRACKINGINFO_INSERT)
            db.AddOutParameter(cmd, "Id", DbType.Int32, 1)
            db.AddInParameter(cmd, "TrackingNo", DbType.String, TrackingNo)
            db.AddInParameter(cmd, "ShipTimestamp", DbType.DateTime, ShipTimestamp)
            db.AddInParameter(cmd, "ActualDeliveryTimestam", DbType.DateTime, ActualDeliveryTimestam)
            db.AddInParameter(cmd, "ActualAddress", DbType.String, ActualAddress)
            db.AddInParameter(cmd, "DestinationAddress", DbType.String, DestinationAddress)
            db.AddInParameter(cmd, "StatusAddress", DbType.String, StatusAddress)
            db.AddInParameter(cmd, "TravelHistory", DbType.String, TravelHistory)
            db.AddInParameter(cmd, "Reference", DbType.String, Reference)
            db.AddInParameter(cmd, "Dimensions", DbType.String, Dimensions)
            db.AddInParameter(cmd, "OrderNumber", DbType.Int32, OrderNumber)
            db.AddInParameter(cmd, "ShipmentId", DbType.Int32, ShipmentId)
            db.AddInParameter(cmd, "Service", DbType.String, Service)
            db.AddInParameter(cmd, "Weight", DbType.Double, Weight)
            db.AddInParameter(cmd, "Pieces", DbType.Int16, Pieces)
            db.AddInParameter(cmd, "InvoiceNumber", DbType.String, InvoiceNumber)
            db.AddInParameter(cmd, "Packaging", DbType.String, Packaging)
            db.AddInParameter(cmd, "StatusCode", DbType.String, StatusCode)
            db.AddInParameter(cmd, "StatusDescription", DbType.String, StatusDescription)
            db.AddInParameter(cmd, "DeliverySignatureName", DbType.String, DeliverySignatureName)
            db.AddInParameter(cmd, "StatusExceptionCode", DbType.String, StatusExceptionCode)
            db.AddInParameter(cmd, "StatusExceptionDescription", DbType.String, StatusExceptionDescription)
            db.AddInParameter(cmd, "ShipTimestampSpecified", DbType.DateTime, Param.ObjectToDB(ShipTimestampSpecified))
            db.AddInParameter(cmd, "SignatureProofOfDeliveryAvailableSpecified", DbType.String, SignatureProofOfDeliveryAvailableSpecified)
            db.AddInParameter(cmd, "NotificationEventsAvailable", DbType.String, NotificationEventsAvailable)
            db.AddInParameter(cmd, "DoorTagNumber", DbType.String, DoorTagNumber)
            db.AddInParameter(cmd, "Note", DbType.String, Note)
            db.AddInParameter(cmd, "CreatedDate", DbType.String, DateTime.Now)
            db.ExecuteNonQuery(cmd)

            '------------------------------------------------------------------------
            Return Convert.ToInt32(db.GetParameterValue(cmd, "Id"))
        End Function

        Public Overridable Sub Update()
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STORETONE_UPDATE As String = "sp_TrackingInfo_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STORETONE_UPDATE)

            db.AddInParameter(cmd, "TrackingNo", DbType.String, TrackingNo)
            db.AddInParameter(cmd, "ShipTimestamp", DbType.DateTime, ShipTimestamp)
            db.AddInParameter(cmd, "ActualDeliveryTimestam", DbType.DateTime, ActualDeliveryTimestam)
            db.AddInParameter(cmd, "ActualAddress", DbType.String, ActualAddress)
            db.AddInParameter(cmd, "DestinationAddress", DbType.String, DestinationAddress)
            db.AddInParameter(cmd, "StatusAddress", DbType.String, StatusAddress)
            db.AddInParameter(cmd, "TravelHistory", DbType.String, TravelHistory)
            db.AddInParameter(cmd, "Reference", DbType.String, Reference)
            db.AddInParameter(cmd, "Dimensions", DbType.String, Dimensions)
            db.AddInParameter(cmd, "OrderNumber", DbType.Int32, OrderNumber)
            db.AddInParameter(cmd, "ShipmentId", DbType.Int32, ShipmentId)
            db.AddInParameter(cmd, "Service", DbType.String, Service)
            db.AddInParameter(cmd, "Weight", DbType.Double, Weight)
            db.AddInParameter(cmd, "Pieces", DbType.Int16, Pieces)
            db.AddInParameter(cmd, "InvoiceNumber", DbType.String, InvoiceNumber)
            db.AddInParameter(cmd, "Packaging", DbType.String, Packaging)
            db.AddInParameter(cmd, "StatusCode", DbType.String, StatusCode)
            db.AddInParameter(cmd, "StatusDescription", DbType.String, StatusDescription)
            db.AddInParameter(cmd, "DeliverySignatureName", DbType.String, DeliverySignatureName)
            db.AddInParameter(cmd, "StatusExceptionCode", DbType.String, StatusExceptionCode)
            db.AddInParameter(cmd, "StatusExceptionDescription", DbType.String, StatusExceptionDescription)
            db.AddInParameter(cmd, "ShipTimestampSpecified", DbType.DateTime, Param.ObjectToDB(ShipTimestampSpecified))
            db.AddInParameter(cmd, "SignatureProofOfDeliveryAvailableSpecified", DbType.String, SignatureProofOfDeliveryAvailableSpecified)
            db.AddInParameter(cmd, "NotificationEventsAvailable", DbType.String, NotificationEventsAvailable)
            db.AddInParameter(cmd, "DoorTagNumber", DbType.String, DoorTagNumber)
            db.AddInParameter(cmd, "Note", DbType.String, Note)
            db.AddInParameter(cmd, "ModifiedDate", DbType.String, DateTime.Now)
            db.ExecuteNonQuery(cmd)

        End Sub 'Update

    End Class

    Public Class TrackingInfoCollection
        Inherits GenericCollection(Of TrackingInfoRow)
    End Class
End Namespace

