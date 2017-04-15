
Option Explicit On


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

    Public Class StoreItemSpecialHandlingFeeRow
        Inherits StoreItemSpecialHandlingFeeRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal Id As Integer)
            MyBase.New(Id)
        End Sub 'New
        Public Shared Function GetAll(ByVal _Database As Database) As StoreItemSpecialHandlingFeeCollection
            Dim dr As SqlDataReader = Nothing
            Dim ss As New StoreItemSpecialHandlingFeeCollection
            Try
                Dim sp As String = "sp_StoreItemSpecialHandlingFee_GetAll"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                dr = cmd.ExecuteReader()
                While dr.Read
                    ss.Add(GetDataFromReader(dr))
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "StoreItemSpecialHandlingFee.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

            Return ss
        End Function
        Public Shared Function GetById(ByVal _Database As Database, ByVal id As Integer) As StoreItemSpecialHandlingFeeRow
            Dim result As StoreItemSpecialHandlingFeeRow = Nothing
            Dim dr As SqlDataReader = Nothing
            Try
                dr = _Database.GetReader("Select * from StoreItemSpecialHandlingFee where Id=" & id)
                If dr.Read Then
                    result = GetDataFromReader(dr)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "StoreItemSpecialHandlingFee.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return result
        End Function
        Private Shared Function GetDataFromReader(ByVal reader As SqlDataReader) As StoreItemSpecialHandlingFeeRow
            Dim result As New StoreItemSpecialHandlingFeeRow
            Try
                If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                    result.Id = Convert.ToInt32(reader("Id"))
                Else
                    result.Id = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("LowWeightValue"))) Then
                    result.LowWeightValue = CDbl(reader("LowWeightValue").ToString())
                Else
                    result.LowWeightValue = Nothing
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("HighWeightValue"))) Then
                    result.HighWeightValue = CDbl(reader("HighWeightValue").ToString())
                Else
                    result.HighWeightValue = Nothing
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("SpecialHandlingFee"))) Then
                    result.SpecialHandlingFee = CDbl(reader("SpecialHandlingFee").ToString())
                Else
                    result.SpecialHandlingFee = Nothing
                End If

            Catch ex As Exception
                Throw ex
            End Try
            Return result
        End Function

        Public Shared Function Update(ByVal _Database As Database, ByVal objData As StoreItemSpecialHandlingFeeRow)
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_StoreItemSpecialHandlingFee_Update"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Id", SqlDbType.Int, 0, objData.Id))
                cmd.Parameters.Add(_Database.InParam("LowWeightValue", SqlDbType.Float, 0, objData.LowWeightValue))
                cmd.Parameters.Add(_Database.InParam("HighWeightValue", SqlDbType.Float, 0, objData.HighWeightValue))
                cmd.Parameters.Add(_Database.InParam("SpecialHandlingFee", SqlDbType.Float, 0, objData.SpecialHandlingFee))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            Return result
        End Function

    End Class

    Public MustInherit Class StoreItemSpecialHandlingFeeRowBase

        Private m_Id As Integer = Nothing
        Private m_LowWeightValue As Double = Nothing
        Private m_HighWeightValue As Double = Nothing
        Private m_SpecialHandlingFee As Double = Nothing
        

        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal Value As Integer)
                m_Id = Value
            End Set
        End Property

        Public Property LowWeightValue() As Double
            Get
                Return m_LowWeightValue
            End Get
            Set(ByVal Value As Double)
                m_LowWeightValue = Value
            End Set
        End Property

        Public Property HighWeightValue() As Double
            Get
                Return m_HighWeightValue
            End Get
            Set(ByVal Value As Double)
                m_HighWeightValue = Value
            End Set
        End Property
        Public Property SpecialHandlingFee() As Double
            Get
                Return m_SpecialHandlingFee
            End Get
            Set(ByVal Value As Double)
                m_SpecialHandlingFee = Value
            End Set
        End Property




        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal Id As Integer)
            m_Id = Id
        End Sub 'New

       
    End Class

    Public Class StoreItemSpecialHandlingFeeCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal objData As StoreItemSpecialHandlingFeeRow)
            Me.List.Add(objData)
        End Sub

        Public Function Contains(ByVal objData As StoreItemSpecialHandlingFeeRow) As Boolean
            Return Me.List.Contains(objData)
        End Function

        Public Function IndexOf(ByVal objData As StoreItemSpecialHandlingFeeRow) As Integer
            Return Me.List.IndexOf(objData)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal objData As StoreItemSpecialHandlingFeeRow)
            Me.List.Insert(Index, objData)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As StoreItemSpecialHandlingFeeRow
            Get
                Return CType(Me.List.Item(Index), StoreItemSpecialHandlingFeeRow)
            End Get

            Set(ByVal Value As StoreItemSpecialHandlingFeeRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal objData As StoreItemSpecialHandlingFeeRow)
            Me.List.Remove(objData)
        End Sub
       
    End Class

End Namespace


