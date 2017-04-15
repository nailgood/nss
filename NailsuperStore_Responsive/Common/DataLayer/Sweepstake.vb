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
Imports Database
Namespace DataLayer
    Public Class SweepstakeRow
        Inherits SweepstakeRowBase


        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New
        Public Sub New(ByVal database As Database, ByVal SweepstakeId As Integer)
            MyBase.New(database, SweepstakeId)
        End Sub 'New
        Public Shared Sub Update(ByVal _Database As Database, ByVal data As SweepstakeRow)
            Try
                Dim sp As String = "sp_Sweepstake_Update"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                With data
                    cmd.Parameters.Add(_Database.InParam("SweepstakeId", SqlDbType.VarChar, 0, .SweepstakeId))
                    cmd.Parameters.Add(_Database.InParam("Name", SqlDbType.NVarChar, 0, data.Name))
                    cmd.Parameters.Add(_Database.InParam("Result", SqlDbType.NVarChar, 0, .Result))
                    cmd.Parameters.Add(_Database.InParam("YouTubeId", SqlDbType.NVarChar, 0, .YouTubeId))
                    cmd.Parameters.Add(_Database.InParam("DrawingDate", SqlDbType.DateTime, 0, Param.ObjectToDB(.DrawingDate)))
                    cmd.Parameters.Add(_Database.InParam("IsActive", SqlDbType.Bit, 0, data.IsActive))
                    cmd.Parameters.Add(_Database.InParam("PageTitle", SqlDbType.NVarChar, 0, .PageTitle))
                    cmd.Parameters.Add(_Database.InParam("MetaKeyword", SqlDbType.VarChar, 0, .MetaKeyword))
                    cmd.Parameters.Add(_Database.InParam("MetaDescription", SqlDbType.VarChar, 0, .MetaDescription))
                End With
                cmd.ExecuteNonQuery()
            Catch ex As Exception
            End Try
        End Sub
        Public Shared Function Insert(ByVal _Database As Database, ByVal data As SweepstakeRow) As Integer
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Sweepstake_Insert"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                With data
                    cmd.Parameters.Add(_Database.InParam("Name", SqlDbType.NVarChar, 0, data.Name))
                    cmd.Parameters.Add(_Database.InParam("Result", SqlDbType.NVarChar, 0, .Result))
                    cmd.Parameters.Add(_Database.InParam("YouTubeId", SqlDbType.NVarChar, 0, .YouTubeId))
                    cmd.Parameters.Add(_Database.InParam("DrawingDate", SqlDbType.DateTime, 0, Param.ObjectToDB(.DrawingDate)))
                    cmd.Parameters.Add(_Database.InParam("IsActive", SqlDbType.Bit, 0, data.IsActive))
                    cmd.Parameters.Add(_Database.InParam("PageTitle", SqlDbType.NVarChar, 0, .PageTitle))
                    cmd.Parameters.Add(_Database.InParam("MetaKeyword", SqlDbType.VarChar, 0, .MetaKeyword))
                    cmd.Parameters.Add(_Database.InParam("MetaDescription", SqlDbType.VarChar, 0, .MetaDescription))
                    cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                End With
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
            End Try
            Return result
        End Function
        Public Shared Function Delete(ByVal _Database As Database, ByVal Id As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Sweepstake_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("SweepstakeId", SqlDbType.Int, 0, Id))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
            End Try
            If result <> 0 Then
                Return True
            End If
            Return False
        End Function
    End Class

    Public MustInherit Class SweepstakeRowBase
        Private m_DB As Database
        Private m_SweepstakeId As Integer = Nothing
        Private m_Name As String = Nothing
        Private m_YouTubeId As String = Nothing
        Private m_Result As String = Nothing
        Private m_DrawingDate As DateTime = Nothing
        Private m_IsActive As Boolean = Nothing
        Private m_PageTitle As String = Nothing
        Private m_MetaKeyword As String = Nothing
        Private m_MetaDescription As String = Nothing

        Public Property SweepstakeId() As Integer
            Get
                Return m_SweepstakeId
            End Get
            Set(ByVal Value As Integer)
                m_SweepstakeId = Value
            End Set
        End Property
        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(ByVal Value As String)
                m_Name = Value
            End Set
        End Property
        Public Property YouTubeId() As String
            Get
                Return m_YouTubeId
            End Get
            Set(ByVal Value As String)
                m_YouTubeId = Value
            End Set
        End Property
        Public Property Result() As String
            Get
                Return m_Result
            End Get
            Set(ByVal Value As String)
                m_Result = Value
            End Set
        End Property
        Public Property DrawingDate() As DateTime
            Get
                Return m_DrawingDate
            End Get
            Set(ByVal Value As DateTime)
                m_DrawingDate = Value
            End Set
        End Property
        Public Property IsActive() As Boolean
            Get
                Return m_IsActive
            End Get
            Set(ByVal Value As Boolean)
                m_IsActive = Value
            End Set
        End Property
        Public Property PageTitle() As String
            Get
                Return m_PageTitle
            End Get
            Set(ByVal Value As String)
                m_PageTitle = Value
            End Set
        End Property
        Public Property MetaKeyword() As String
            Get
                Return m_MetaKeyword
            End Get
            Set(ByVal Value As String)
                m_MetaKeyword = Value
            End Set
        End Property
        Public Property MetaDescription() As String
            Get
                Return m_MetaDescription
            End Get
            Set(ByVal Value As String)
                m_MetaDescription = Value
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

        Public Sub New(ByVal database As Database, ByVal Id As Integer)
            m_DB = database
            m_SweepstakeId = Id
        End Sub 'New
        Public Shared Function GetRow(ByVal _Database As Database, ByVal SweepstakeId As Integer) As SweepstakeRow
            Dim row As SweepstakeRow
            row = New SweepstakeRow(_Database, SweepstakeId)
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM Sweepstake WHERE SweepstakeId = " & _Database.Number(SweepstakeId)
                r = _Database.GetReader(SQL)
                If r.HasRows Then
                    row = mapList(Of SweepstakeRow)(r).Item(0)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "Sweepstake", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return row
        End Function
        Public Shared Function ChangeIsActive(ByVal _Database As Database, ByVal SweepstakeId As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Sweepstake_ChangeIsActive"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("SweepstakeId", SqlDbType.Int, 0, SweepstakeId))
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

    Public Class SweepstakeCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal data As SweepstakeRowBase)
            Me.List.Add(data)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As SweepstakeRowBase
            Get
                Return CType(Me.List.Item(Index), SweepstakeRowBase)
            End Get

            Set(ByVal Value As SweepstakeRowBase)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace

