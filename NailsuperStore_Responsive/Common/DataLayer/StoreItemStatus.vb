
Option Explicit On

'Author: Lam Le
'Date: 10/26/2009 2:13:06 PM

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Components

Namespace DataLayer

    Public Class StoreItemStatusRow
        Inherits StoreItemStatusRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New           
        Public Shared Function GetStatusNameByCode(ByVal code As String) As String
            Dim result As String = Nothing
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sql As String = "Select StatusName from StoreItemStatus where Status='" & code & "'"
                reader = db.ExecuteReader(CommandType.Text, sql)
                If reader.Read() Then
                    result = reader.GetValue(0).ToString()
                End If
                Core.CloseReader(reader)
                db = Nothing
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try

            Return result
        End Function

    End Class

    Public MustInherit Class StoreItemStatusRowBase
        Private m_DB As Database
        Private m_StoreItemStatusId As Integer = Nothing
        Private m_Status As String = Nothing
        Private m_StatusName As String = Nothing

        Public Property StoreItemStatusId() As Integer
            Get
                Return m_StoreItemStatusId
            End Get
            Set(ByVal Value As Integer)
                m_StoreItemStatusId = Value
            End Set
        End Property

        Public Property Status() As String
            Get
                Return m_Status
            End Get
            Set(ByVal Value As String)
                m_Status = Value
            End Set
        End Property
        Public Property StatusName() As String
            Get
                Return m_StatusName
            End Get
            Set(ByVal Value As String)
                m_StatusName = Value
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
    End Class

    Public Class StoreItemStatusCollection
        Inherits GenericCollection(Of StoreItemStatusRow)
    End Class

End Namespace


