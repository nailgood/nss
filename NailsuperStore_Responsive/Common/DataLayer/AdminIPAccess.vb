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
    Public Class AdminIPAccessRow
        Inherits AdminIPAccessRowBase

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
        Public Shared Function GetRow(ByVal _Database As Database, ByVal id As Integer) As AdminIPAccessRow
            Dim row As AdminIPAccessRow
            row = New AdminIPAccessRow(_Database, id)
            row.Load()
            Return row
        End Function

        Public Shared Function ListAllByUsername(ByVal _Database As Database, ByVal username As String) As AdminIPAccessCollection
            Dim dr As SqlDataReader = Nothing
            Dim ss As New AdminIPAccessCollection
            Try
                Dim sp As String = "sp_AdminIPAccess_ListAllByUsername"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Username", SqlDbType.VarChar, 0, username))
                dr = cmd.ExecuteReader()
                While dr.Read
                    ss.Add(GetDataFromReader(dr))
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "AdminIPAccess.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

            Return ss
        End Function
        Public Shared Function CountByUsername(ByVal _Database As Database, ByVal username As String) As Integer
            Dim result As Integer = 0
            Dim dr As SqlDataReader = Nothing
            Try
                Dim sp As String = "sp_AdminIPAccess_CountByUsername"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Username", SqlDbType.VarChar, 0, username))
                dr = cmd.ExecuteReader()
                If dr.Read Then
                    result = Convert.ToInt32(dr.GetValue(0).ToString())
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
           
            Return result
        End Function

        Private Shared Function GetDataFromReader(ByVal reader As SqlDataReader) As AdminIPAccessRow
            Dim result As New AdminIPAccessRow
            Try
                If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                    result.Id = Convert.ToInt32(reader("Id"))
                Else
                    result.Id = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Username"))) Then
                    result.Username = reader("Username").ToString()
                Else
                    result.Username = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("IP"))) Then
                    result.IP = reader("IP").ToString()
                Else
                    result.IP = ""
                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return result
        End Function

        Public Shared Function Delete(ByVal _Database As Database, ByVal Id As Integer) As Boolean

            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_AdminIPAccess_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Id", SqlDbType.Int, 0, Id))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            Return False
        End Function

        Public Shared Function Insert(ByVal _Database As Database, ByVal data As AdminIPAccessRow) As Integer
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_AdminIPAccess_Insert"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Username", SqlDbType.VarChar, 0, data.Username))
                cmd.Parameters.Add(_Database.InParam("IP", SqlDbType.VarChar, 0, data.IP))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
            End Try
            Return result
        End Function
        Public Shared Function Update(ByVal _Database As Database, ByVal data As AdminIPAccessRow) As Integer
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_AdminIPAccess_Update"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Id", SqlDbType.Int, 0, data.Id))
                cmd.Parameters.Add(_Database.InParam("Username", SqlDbType.VarChar, 0, data.Username))
                cmd.Parameters.Add(_Database.InParam("IP", SqlDbType.VarChar, 0, data.IP))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
            End Try
            Return result
        End Function
    End Class


    Public MustInherit Class AdminIPAccessRowBase
        Private m_DB As Database
        Private m_Id As Integer = Nothing
        Private m_Username As String = Nothing
        Private m_IP As String = Nothing
        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal Value As Integer)
                m_Id = Value
            End Set
        End Property
        Public Property Username() As String
            Get
                Return m_Username
            End Get
            Set(ByVal Value As String)
                m_Username = Value
            End Set
        End Property


        Public Property IP() As String
            Get
                Return m_IP
            End Get
            Set(ByVal Value As String)
                m_IP = Value
            End Set
        End Property

        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal Id As Integer)
            m_DB = database
            m_Id = Id
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM AdminIPAccess WHERE Id = " & m_DB.Number(Id)
                r = m_DB.GetReader(SQL)
                If Not r Is Nothing Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Email.SendError("ToError500", "AdminIPAccess.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                        m_Id = Convert.ToInt32(reader("Id"))
                    Else
                        m_Id = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Username"))) Then
                        m_Username = reader("Username").ToString()
                    Else
                        m_Username = ""
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("IP"))) Then
                        m_IP = reader("IP").ToString()
                    Else
                        m_IP = ""
                    End If
                End If
            Catch ex As Exception
                Throw ex

            End Try

        End Sub

    End Class

    Public Class AdminIPAccessCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal data As AdminIPAccessRow)
            Me.List.Add(data)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As AdminIPAccessRow
            Get
                Return CType(Me.List.Item(Index), AdminIPAccessRow)
            End Get

            Set(ByVal Value As AdminIPAccessRow)
                Me.List(Index) = Value
            End Set
        End Property
        ReadOnly Property Clone() As AdminIPAccessCollection
            Get
                If Me.List Is Nothing Then
                    Return Nothing
                End If
                Dim result As New AdminIPAccessCollection
                For Each obj As AdminIPAccessRow In Me.List
                    result.Add(CloneObject.Clone(obj))
                Next
                Return result
            End Get
        End Property
    End Class
End Namespace


