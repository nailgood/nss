
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

    Public Class FreeGiftLevelRow
        Inherits FreeGiftLevelRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New
        Public Shared Function GetListActive() As FreeGiftLevelCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim c As New FreeGiftLevelCollection
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetSqlStringCommand("Select Id,Name,MinValue,COALESCE(MaxValue,0) as MaxValue,Banner from FreeGiftLevel where IsActive=1 and [dbo].[fc_FreeGiftLevel_CountItem](Id)>0 order by MinValue ASC")
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim row As New FreeGiftLevelRow()
                    row.Id = dr("Id")
                    row.Name = dr("Name")
                    row.Banner = dr("Banner")
                    row.MinValue = dr("MinValue")
                    row.MaxValue = dr("MaxValue")
                    c.Add(row)
                End While
                Core.CloseReader(dr)
                Return c
            Catch ex As Exception
                SendMailLog("GetListActive", ex)
                Core.CloseReader(dr)
            End Try
            Return Nothing
        End Function
        Public Shared Function CheckAllowAddCart(ByVal orderId As Integer, ByVal levelId As Integer) As Boolean
            Dim result As Integer = 0
            Try

                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetSqlStringCommand("select [dbo].[fc_FreeGiftLevel_AllowAddCart](" & orderId & "," & levelId & ")")
                result = CInt(db.ExecuteScalar(cmd))
            Catch ex As Exception
                SendMailLog("CheckAllowAddCart(orderId:=" & orderId & ",LevelId:=" & levelId & ")", ex)
            End Try
            Return (result = 1)
        End Function
        Private Shared Sub SendMailLog(ByVal func As String, ByVal ex As Exception)
            Core.LogError("FreeGiftLevel.vb", func, ex)
        End Sub
        Public Shared Function ListAll() As FreeGiftLevelCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim c As New FreeGiftLevelCollection
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetSqlStringCommand("Select Id,Name,MinValue,COALESCE(MaxValue,0) as MaxValue,IsActive,Banner from FreeGiftLevel order by MinValue ASC")
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim row As New FreeGiftLevelRow()
                    row.Id = dr("Id")
                    row.Name = dr("Name")
                    row.Banner = dr("Banner")
                    row.MinValue = dr("MinValue")
                    row.MaxValue = dr("MaxValue")
                    row.IsActive = dr("IsActive")
                    c.Add(row)
                End While
                Core.CloseReader(dr)
                Return c
            Catch ex As Exception
                SendMailLog("ListAll", ex)
                Core.CloseReader(dr)
            End Try
            Return Nothing
        End Function
        Public Shared Function ChangeActive(ByVal id As Integer) As Boolean
            Try

                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetSqlStringCommand("Update FreeGiftLevel set IsActive=~IsActive where Id=" & id)
                Dim result As Integer = db.ExecuteNonQuery(cmd)
                If result > 0 Then
                    Return True
                End If
            Catch ex As Exception
                SendMailLog("ChangeActive(id=" & id, ex)
            End Try
            Return False
        End Function
        Public Shared Function Insert(ByVal objLevel As FreeGiftLevelRow) As Integer
            Try

                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_FreeGiftLevel_Insert")
                db.AddInParameter(cmd, "Name", DbType.String, objLevel.Name)
                db.AddInParameter(cmd, "Banner", DbType.String, objLevel.Banner)
                db.AddInParameter(cmd, "MinValue", DbType.Double, objLevel.MinValue)
                db.AddInParameter(cmd, "MaxValue", DbType.Double, objLevel.MaxValue)
                db.AddInParameter(cmd, "IsActive", DbType.Boolean, objLevel.IsActive)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
                Return result
            Catch ex As Exception
                SendMailLog("Insert", ex)
            End Try
            Return False
        End Function
        Public Shared Function Update(ByVal objLevel As FreeGiftLevelRow) As Boolean
            Try

                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_FreeGiftLevel_Update")
                db.AddInParameter(cmd, "Id", DbType.Int32, objLevel.Id)
                db.AddInParameter(cmd, "Name", DbType.String, objLevel.Name)
                db.AddInParameter(cmd, "Banner", DbType.String, objLevel.Banner)
                db.AddInParameter(cmd, "MinValue", DbType.Double, objLevel.MinValue)
                db.AddInParameter(cmd, "MaxValue", DbType.Double, objLevel.MaxValue)
                db.AddInParameter(cmd, "IsActive", DbType.Boolean, objLevel.IsActive)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
                Return result = 1
            Catch ex As Exception
                SendMailLog("Update", ex)
            End Try
            Return False
        End Function
        Public Shared Function UpdateBanner(ByVal objLevel As FreeGiftLevelRow) As Boolean
            Try

                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetSqlStringCommand("Update FreeGiftLevel set Banner='" & objLevel.Banner & "' where id=" & objLevel.Id)
                Dim result As Integer = db.ExecuteNonQuery(cmd)
                If result > 0 Then
                    Return True
                End If
            Catch ex As Exception
                SendMailLog("UpdateBanner", ex)
            End Try
            Return False
        End Function
        Public Shared Function Delete(ByVal id As Integer) As Boolean
            Try

                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_FreeGiftLevel_Delete")
                db.AddInParameter(cmd, "Id", DbType.Int32, id)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
                If result = 1 Then
                    Return True
                End If
            Catch ex As Exception
                SendMailLog("Delete(id=" & id & ")", ex)
            End Try
            Return False
        End Function
        Public Shared Function GetRow(ByVal id As Integer) As FreeGiftLevelRow
            Dim dr As SqlDataReader = Nothing
            Try
                Dim result As New FreeGiftLevelRow
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetSqlStringCommand("Select Id,Name,MinValue,COALESCE(MaxValue,0) as MaxValue,IsActive,COALESCE(Banner,'') as Banner  from FreeGiftLevel where Id=" & id)
                dr = db.ExecuteReader(cmd)
                If dr.Read Then
                    result.Id = dr("Id")
                    result.Name = dr("Name")
                    result.Banner = dr("Banner")
                    result.MinValue = dr("MinValue")
                    result.MaxValue = dr("MaxValue")
                    result.IsActive = dr("IsActive")
                End If
                Core.CloseReader(dr)
                Return result
            Catch ex As Exception
                SendMailLog("GetRow(id=" & id & ")", ex)
                Core.CloseReader(dr)
            End Try
            Return Nothing
        End Function
    End Class

    Public MustInherit Class FreeGiftLevelRowBase
        Private m_Name As String = Nothing
        Private m_Id As Integer = Nothing
        Private m_MinValue As Double = Nothing
        Private m_MaxValue As Double = Nothing
        Private m_IsActive As Boolean = Nothing
        Private m_Banner As String = Nothing
        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(ByVal Value As String)
                m_Name = Value
            End Set
        End Property

        Public Property Banner() As String
            Get
                Return m_Banner
            End Get
            Set(ByVal Value As String)
                m_Banner = Value
            End Set
        End Property
        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal Value As Integer)
                m_Id = Value
            End Set
        End Property

        Public Property MinValue() As Double
            Get
                Return m_MinValue
            End Get
            Set(ByVal Value As Double)
                m_MinValue = Value
            End Set
        End Property
        Public Property MaxValue() As Double
            Get
                Return m_MaxValue
            End Get
            Set(ByVal Value As Double)
                m_MaxValue = Value
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



        Public Sub New()
        End Sub 'New






    End Class

    Public Class FreeGiftLevelCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal FreeGift As FreeGiftLevelRow)
            Me.List.Add(FreeGift)
        End Sub

        Public Function Contains(ByVal FreeGift As FreeGiftLevelRow) As Boolean
            Return Me.List.Contains(FreeGift)
        End Function

        Public Function IndexOf(ByVal FreeGift As FreeGiftLevelRow) As Integer
            Return Me.List.IndexOf(FreeGift)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal FreeGift As FreeGiftLevelRow)
            Me.List.Insert(Index, FreeGift)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As FreeGiftLevelRow
            Get
                If Me.List.Item(Index) Is Nothing Then
                    Return Nothing
                End If
                Return CType(Me.List.Item(Index), FreeGiftLevelRow)
            End Get

            Set(ByVal Value As FreeGiftLevelRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal FreeGift As FreeGiftLevelRow)
            Me.List.Remove(FreeGift)
        End Sub
    End Class

End Namespace


