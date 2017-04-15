
Imports System
Imports Components
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Utility
Namespace DataLayer
    Public Class KeywordActionRow
        Inherits KeywordActionRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal id As Integer)
            MyBase.New(database, id)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal KeywordActionId As Int64) As KeywordActionRow
            Dim row As KeywordActionRow
            row = New KeywordActionRow(_Database, KeywordActionId)
            row.Load()
            Return row
        End Function
        Private Shared Function GetDataFromReader(ByVal reader As SqlDataReader) As KeywordActionRow
            Dim result As New KeywordActionRow
            Try
                If (Not reader.IsDBNull(reader.GetOrdinal("KeywordActionId"))) Then
                    result.KeywordActionId = Convert.ToInt64(reader("KeywordActionId"))
                Else
                    result.KeywordActionId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("KeywordSearchId"))) Then
                    result.KeywordSearchId = Convert.ToInt64(reader("KeywordSearchId"))
                Else
                    result.KeywordSearchId = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ItemId"))) Then
                    result.ItemId = Convert.ToInt32(reader("ItemId"))
                Else
                    result.ItemId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Action"))) Then
                    result.Action = Convert.ToInt32(reader("Action"))
                Else
                    result.Action = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Point"))) Then
                    result.Point = Convert.ToInt32(reader("Point"))
                Else
                    result.Point = 0
                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return result
        End Function
        Public Shared Function Insert(ByVal data As KeywordActionRow) As Integer
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_KeywordAction_Insert")
                db.AddInParameter(cmd, "KeywordSearchId", DbType.Int64, data.KeywordSearchId)
                db.AddInParameter(cmd, "Action", DbType.Int32, data.Action)
                db.AddInParameter(cmd, "Point", DbType.Int32, data.Point)
                db.AddInParameter(cmd, "ItemId", DbType.Int32, data.ItemId)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
                If result = 1 Then
                    Return True
                End If
            Catch ex As Exception
                Core.LogError("KeywordAction.vb", "Insert(KeywordSearchId=" & data.KeywordSearchId & ",ItemId=" & data.ItemId.ToString() & ",Action=" & data.Action.ToString() & ")", ex)
            End Try
            Return False
        End Function
    End Class


    Public MustInherit Class KeywordActionRowBase
        Private m_DB As Database
        Private m_KeywordActionId As Int64 = Nothing
        Private m_KeywordSearchId As Int64 = Nothing
        Private m_Action As Integer = Nothing
        Private m_Point As Integer = Nothing
        Private m_ItemId As Integer = Nothing
        Public Property KeywordActionId() As Int64
            Get
                Return m_KeywordActionId
            End Get
            Set(ByVal Value As Int64)
                m_KeywordActionId = Value
            End Set
        End Property
        Public Property KeywordSearchId() As Int64
            Get
                Return m_KeywordSearchId
            End Get
            Set(ByVal Value As Int64)
                m_KeywordSearchId = Value
            End Set
        End Property
        Public Property Action() As Integer
            Get
                Return m_Action
            End Get
            Set(ByVal Value As Integer)
                m_Action = Value
            End Set
        End Property
        Public Property Point() As Integer
            Get
                Return m_Point
            End Get
            Set(ByVal Value As Integer)
                m_Point = Value
            End Set
        End Property
        Public Property ItemId() As Integer
            Get
                Return m_ItemId
            End Get
            Set(ByVal Value As Integer)
                m_ItemId = Value
            End Set
        End Property

        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal KeywordActionId As Integer)
            m_DB = database
            m_KeywordActionId = KeywordActionId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM KeywordAction WHERE Id = " & m_DB.Number(KeywordActionId)
                r = m_DB.GetReader(SQL)
                If Not r Is Nothing Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Core.LogError("KeywordAction.vb", "Load", ex)
            End Try
        End Sub
        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("KeywordActionId"))) Then
                        m_KeywordActionId = Convert.ToInt64(reader("KeywordActionId"))
                    Else
                        m_KeywordActionId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("KeywordSearchId"))) Then
                        m_KeywordSearchId = reader("KeywordSearchId").ToString()
                    Else
                        m_KeywordSearchId = 0
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("Action"))) Then
                        m_Action = Convert.ToInt64(reader("Action"))
                    Else
                        m_Action = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Point"))) Then
                        m_Point = Convert.ToInt32(reader("Point"))
                    Else
                        m_Point = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ItemId"))) Then
                        m_ItemId = Convert.ToInt64(reader("ItemId"))
                    Else
                        m_ItemId = 0
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
    End Class

    Public Class KeywordActionCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal data As KeywordActionRowBase)
            Me.List.Add(data)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As KeywordActionRowBase
            Get
                Return CType(Me.List.Item(Index), KeywordActionRowBase)
            End Get

            Set(ByVal Value As KeywordActionRowBase)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace


