

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
    Public Class DepartmentSlideEffectRow
        Inherits DepartmentSlideEffectRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New       

        Public Shared Function ListByDepartment(ByVal _Database As Database, ByVal departmentId As Integer) As DepartmentSlideEffectCollection
            Dim ss As DepartmentSlideEffectCollection = Nothing
            Dim dr As SqlDataReader = Nothing
            Try
                Dim keyData As String = cachePrefixKey & "ListByDepartment_" & departmentId
                ss = CType(CacheUtils.GetCache(keyData), DepartmentSlideEffectCollection)
                If Not ss Is Nothing Then
                    Return ss
                Else
                    ss = New DepartmentSlideEffectCollection
                End If
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_DepartmentSlideEffect_ListByDepartment"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                cmd.Parameters.Add(_Database.InParam("DepartmentId", SqlDbType.Int, 0, departmentId))
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    ss.Add(GetDataFromReader(dr))
                End While
                Core.CloseReader(dr)
                CacheUtils.SetCache(keyData, ss)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
            Return ss
        End Function
        Private Shared Function GetDataFromReader(ByVal reader As SqlDataReader) As DepartmentSlideEffectRow
            Dim result As New DepartmentSlideEffectRow
            If (Not reader.IsDBNull(reader.GetOrdinal("DepartmentId"))) Then
                result.DepartmentId = Convert.ToInt32(reader("DepartmentId"))
            Else
                result.DepartmentId = 0
            End If

            If (Not reader.IsDBNull(reader.GetOrdinal("EffectCode"))) Then
                result.EffectCode = reader("EffectCode").ToString()
            Else
                result.EffectCode = ""
            End If
            Return result
        End Function

        Public Shared Function DeleteByDepartment(ByVal _Database As Database, ByVal DepartmentId As Integer) As Boolean

            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_DepartmentSlideEffect_DeleteByDepartment"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("DepartmentId", SqlDbType.Int, 0, DepartmentId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
                Return True
            End If
            Return False
        End Function

        Public Shared Function Insert(ByVal _Database As Database, ByVal data As DepartmentSlideEffectRow) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_DepartmentSlideEffect_Insert"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("DepartmentId", SqlDbType.Int, 0, data.DepartmentId))
                cmd.Parameters.Add(_Database.InParam("EffectCode", SqlDbType.NVarChar, 0, data.EffectCode))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
            End Try
            If result = 1 Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
                Return True
            End If
            Return False
        End Function
   
    End Class


    Public MustInherit Class DepartmentSlideEffectRowBase
        Private m_DB As Database
        Private m_DepartmentId As Integer = Nothing
        Private m_EffectCode As String = Nothing
        Public Shared cachePrefixKey As String = "DepartmentSlideEffect_"

        Public Property DepartmentId() As Integer
            Get
                Return m_DepartmentId
            End Get
            Set(ByVal Value As Integer)
                m_DepartmentId = Value
            End Set
        End Property
        Public Property EffectCode() As String
            Get
                Return m_EffectCode
            End Get
            Set(ByVal Value As String)
                m_EffectCode = Value
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

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New
    End Class

    Public Class DepartmentSlideEffectCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal effect As DepartmentSlideEffectRow)
            Me.List.Add(effect)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As DepartmentSlideEffectRow
            Get
                Return CType(Me.List.Item(Index), DepartmentSlideEffectRow)
            End Get

            Set(ByVal Value As DepartmentSlideEffectRow)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace


