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
    Public Class AdminLogDetailRow
        Inherits AdminLogDetailRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal AdminlogDetailId As Integer)
            MyBase.New(database, AdminlogDetailId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal AdminlogDetailId As Integer) As AdminLogDetailRow
            Dim row As AdminLogDetailRow
            row = New AdminLogDetailRow(_Database, AdminlogDetailId)
            row.Load()
            Return row
        End Function
        
   
        Public Shared Function Delete(ByVal _Database As Database, ByVal AdminLogDetailId As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_AdminLogDetail_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("AdminLogDetailId", SqlDbType.Int, 0, AdminLogDetailId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function
        Public Shared Function Insert(ByVal _Database As Database, ByVal data As AdminLogDetailRow) As Boolean

            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_AdminLogDetail_Insert"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("LogId", SqlDbType.Int, 0, data.LogId))
                cmd.Parameters.Add(_Database.InParam("Subject", SqlDbType.VarChar, 0, data.Subject))
                cmd.Parameters.Add(_Database.InParam("Message", SqlDbType.NVarChar, 0, data.Message))
                cmd.Parameters.Add(_Database.InParam("Action", SqlDbType.VarChar, 0, data.Action))
                cmd.Parameters.Add(_Database.InParam("ObjectType", SqlDbType.VarChar, 0, data.ObjectType))
                cmd.Parameters.Add(_Database.InParam("ObjectId", SqlDbType.VarChar, 0, data.ObjectId))
                cmd.Parameters.Add(_Database.InParam("CreatedDate", SqlDbType.DateTime, 0, data.CreatedDate))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function

    End Class


    Public MustInherit Class AdminLogDetailRowBase
        Private m_DB As Database
        Private m_AdminLogDetailId As Integer = Nothing
        Private m_LogId As Integer = Nothing
        Private m_Subject As String = Nothing
        Private m_Message As String = Nothing
        Private m_CreatedDate As DateTime = Nothing
        Private m_TotalRow As Integer = Nothing
        Private m_PageIndex As Integer = Nothing
        Private m_PageSize As Integer = Nothing
        Private m_Condition As String = Nothing
        Private m_OrderBy As String = Nothing
        Private m_OrderDirection As String = Nothing
        Private m_Action As String = Nothing
        Private m_ObjectType As String = Nothing
        Private m_ObjectId As String = Nothing
        Private m_LoggedEmail As String = Nothing

        Public Property AdminLogDetailId() As Integer
            Get
                Return m_AdminLogDetailId
            End Get
            Set(ByVal Value As Integer)
                m_AdminLogDetailId = Value
            End Set
        End Property
        Public Property LogId() As Integer
            Get
                Return m_LogId
            End Get
            Set(ByVal Value As Integer)
                m_LogId = Value
            End Set
        End Property
        Public Property Subject() As String
            Get
                Return m_Subject
            End Get
            Set(ByVal Value As String)
                m_Subject = Value
            End Set
        End Property

        Public Property Message() As String
            Get
                Return m_Message
            End Get
            Set(ByVal Value As String)
                m_Message = Value
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
        Public Property TotalRow() As Integer
            Get
                Return m_TotalRow
            End Get
            Set(ByVal Value As Integer)
                m_TotalRow = Value
            End Set
        End Property
        Public Property PageIndex() As Integer
            Get
                Return m_PageIndex
            End Get
            Set(ByVal Value As Integer)
                m_PageIndex = Value
            End Set
        End Property
        Public Property PageSize() As Integer
            Get
                Return m_PageSize
            End Get
            Set(ByVal Value As Integer)
                m_PageSize = Value
            End Set
        End Property

        Public Property OrderBy() As String
            Get
                Return m_OrderBy
            End Get
            Set(ByVal Value As String)
                m_OrderBy = Value
            End Set
        End Property
        Public Property OrderDirection() As String
            Get
                Return m_OrderDirection
            End Get
            Set(ByVal Value As String)
                m_OrderDirection = Value
            End Set
        End Property
        Public Property Condition() As String
            Get
                Return m_Condition
            End Get
            Set(ByVal Value As String)
                m_Condition = Value
            End Set
        End Property
        Public Property LoggedEmail() As String
            Get
                Return m_LoggedEmail
            End Get
            Set(ByVal Value As String)
                m_LoggedEmail = Value
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
        Public Property Action() As String
            Get
                Return m_Action
            End Get
            Set(ByVal Value As String)
                m_Action = Value
            End Set
        End Property
        Public Property ObjectId() As String
            Get
                Return m_ObjectId
            End Get
            Set(ByVal Value As String)
                m_ObjectId = Value
            End Set
        End Property
        Public Property ObjectType() As String
            Get
                Return m_ObjectType
            End Get
            Set(ByVal Value As String)
                m_ObjectType = Value
            End Set
        End Property
        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal AdminLogDetailId As Integer)
            m_DB = database
            m_AdminLogDetailId = AdminLogDetailId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM AdminLogDetail WHERE AdminLogDetailId = " & DB.Number(m_AdminLogDetailId)
                r = m_DB.GetReader(SQL)
                If Not r Is Nothing Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                m_AdminLogDetailId = 0
                Email.SendError("ToError500", "AdminLogDetail.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("AdminLogDetailId"))) Then
                        m_AdminLogDetailId = Convert.ToInt32(reader("AdminLogDetailId"))
                    Else
                        m_AdminLogDetailId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("LogId"))) Then
                        m_LogId = Convert.ToInt32(reader("LogId"))
                    Else
                        m_LogId = 0
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("Message"))) Then
                        m_Message = reader("Message").ToString()
                    Else
                        m_Message = ""
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("Subject"))) Then
                        m_Subject = reader("Subject").ToString()
                    Else
                        m_Subject = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("CreatedDate"))) Then
                        m_CreatedDate = Convert.ToDateTime(reader("CreatedDate").ToString())
                    Else
                        m_CreatedDate = ""
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try

        End Sub

    End Class

    Public Class AdminLogDetailCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal data As AdminLogDetailRow)
            Me.List.Add(data)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As AdminLogDetailRow
            Get
                Return CType(Me.List.Item(Index), AdminLogDetailRow)
            End Get

            Set(ByVal Value As AdminLogDetailRow)
                Me.List(Index) = Value
            End Set
        End Property
        Public ReadOnly Property Clone() As AdminLogDetailCollection
            Get
                If Me.List Is Nothing Then
                    Return Nothing
                End If
                Dim result As New AdminLogDetailCollection
                For Each obj As AdminLogDetailRow In Me.List
                    result.Add(CloneObject.Clone(obj))
                Next
                Return result
            End Get
        End Property
    End Class
End Namespace


