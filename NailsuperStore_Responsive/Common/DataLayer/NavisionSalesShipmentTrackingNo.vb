Option Explicit On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components

Namespace DataLayer

    Public Class NavisionSalesShipmentTrackingNoRow
        Inherits NavisionSalesShipmentTrackingNoRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Shared Function GetCollection(ByVal DB As Database) As NavisionSalesShipmentTrackingNoCollection
            Dim r As SqlDataReader = Nothing
            Dim c As New NavisionSalesShipmentTrackingNoCollection
            Try
                Dim row As NavisionSalesShipmentTrackingNoRow
                Dim SQL As String = "select * from _NAVISION_SALES_SHIPMENT_TRACKING_NO"
                r = DB.GetReader(SQL)
                While r.Read
                    row = New NavisionSalesShipmentTrackingNoRow(DB)
                    row.Load(r)
                    c.Add(row)
                End While
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            Return c
        End Function
    End Class

    Public MustInherit Class NavisionSalesShipmentTrackingNoRowBase
        Private m_DB As Database
        Private m_Sales_Shipment_No As String
        Private m_Tracking_No As String
        Private m_Tracking_No_ID As String

        Public Property Sales_Shipment_No() As String
            Get
                Return m_Sales_Shipment_No
            End Get
            Set(ByVal value As String)
                m_Sales_Shipment_No = value
            End Set
        End Property

        Public Property Tracking_No() As String
            Get
                Return m_Tracking_No
            End Get
            Set(ByVal value As String)
                m_Tracking_No = value
            End Set
        End Property
        Public Property Tracking_No_ID() As String
            Get
                Return m_Tracking_No_ID
            End Get
            Set(ByVal value As String)
                m_Tracking_No_ID = value
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

        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            If IsDBNull(r.Item("Sales_Shipment_No")) Then
                m_Sales_Shipment_No = Nothing
            Else
                m_Sales_Shipment_No = Convert.ToString(r.Item("Sales_Shipment_No"))
            End If
            If IsDBNull(r.Item("Tracking_No")) Then
                m_Tracking_No = Nothing
            Else
                m_Tracking_No = Convert.ToString(r.Item("Tracking_No"))
            End If
            If IsDBNull(r.Item("Tracking_No_ID")) Then
                m_Tracking_No_ID = Nothing
            Else
                m_Tracking_No_ID = Convert.ToString(r.Item("Tracking_No_ID"))
            End If
        End Sub 'Load
    End Class

    Public Class NavisionSalesShipmentTrackingNoCollection
        Inherits GenericCollection(Of NavisionSalesShipmentTrackingNoRow)
    End Class

End Namespace