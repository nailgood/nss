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
Imports Components.Core

Namespace DataLayer
    Public Class ExportLogRow
        Inherits ExportLogBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal OrderId As Integer)
            MyBase.New(DB, OrderId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal OrderId As Integer) As ExportLogRow
            Dim row As ExportLogRow

            row = New ExportLogRow(DB, OrderId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal OrderId As Integer)
            Dim row As ExportLogRow

            row = New ExportLogRow(DB, OrderId)
            row.Remove()
        End Sub

        Public Shared Function GetAllToCheckDownload() As DataTable

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GetAllToCheckDownload As String = "sp_ExportLog_GetAllToCheckDownload"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GetAllToCheckDownload)

            Return db.ExecuteDataSet(cmd).Tables(0)

        End Function
    End Class

    Public MustInherit Class ExportLogBase
        Private m_DB As Database
        Private m_OrderId As Integer = Nothing
        Private m_HeaderFile As String = Nothing
        Private m_CartItemFile As String = Nothing
        Private m_OrderStatus As Integer = Nothing
        Private m_CartItemStatus As Integer = Nothing
        Private m_NoteHeaderFile As String = Nothing
        Private m_NoteCartItemFile As String = Nothing

        Public Property OrderId() As Integer
            Get
                Return m_OrderId
            End Get
            Set(ByVal value As Integer)
                m_OrderId = value
            End Set
        End Property

        Public Property HeaderFile() As String
            Get
                Return m_HeaderFile
            End Get
            Set(ByVal value As String)
                m_HeaderFile = value
            End Set
        End Property

        Public Property CartItemFile() As String
            Get
                Return m_CartItemFile
            End Get
            Set(ByVal value As String)
                m_CartItemFile = value
            End Set
        End Property

        Public Property OrderStatus() As Integer
            Get
                Return m_OrderStatus
            End Get
            Set(ByVal value As Integer)
                m_OrderStatus = value
            End Set
        End Property

        Public Property CartItemStatus() As Integer
            Get
                Return m_CartItemStatus
            End Get
            Set(ByVal value As Integer)
                m_CartItemStatus = value
            End Set
        End Property

        Public Property NoteHeaderFile() As String
            Get
                Return m_NoteHeaderFile
            End Get
            Set(ByVal value As String)
                m_NoteHeaderFile = value
            End Set
        End Property

        Public Property NoteCartItemFile() As String
            Get
                Return m_NoteCartItemFile
            End Get
            Set(ByVal value As String)
                m_NoteCartItemFile = value
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

        Public Sub New(ByVal database As Database, ByVal OrderId As Integer)
            m_DB = database
            m_OrderId = OrderId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM ExportLog WHERE OrderId = " & DB.Number(OrderId)
                r = m_DB.GetReader(SQL)
                If Not r Is Nothing Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Core.LogError("ExportLog.vb", "Load", ex)
            End Try
        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("OrderId"))) Then
                    m_OrderId = Convert.ToInt32(reader("OrderId"))
                Else
                    m_OrderId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("HeaderFile"))) Then
                    m_HeaderFile = reader("HeaderFile").ToString()
                Else
                    m_HeaderFile = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("CartItemFile"))) Then
                    m_CartItemFile = reader("CartItemFile").ToString()
                Else
                    m_CartItemFile = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("OrderStatus"))) Then
                    m_OrderStatus = Convert.ToInt32(reader("OrderStatus"))
                Else
                    m_OrderStatus = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("CartItemStatus"))) Then
                    m_CartItemStatus = Convert.ToInt32(reader("CartItemStatus"))
                Else
                    m_CartItemStatus = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("NoteHeaderFile"))) Then
                    m_NoteHeaderFile = reader("NoteHeaderFile").ToString()
                Else
                    m_NoteHeaderFile = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("NoteCartItemFile"))) Then
                    m_NoteCartItemFile = reader("NoteCartItemFile").ToString()
                Else
                    m_NoteCartItemFile = ""
                End If
            End If
        End Sub 'Load

        Public Overridable Sub Insert()
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_EXPORTLOG_INSERT As String = "sp_ExportLog_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_EXPORTLOG_INSERT)

            db.AddInParameter(cmd, "OrderId", DbType.Int32, OrderId)
            db.AddInParameter(cmd, "HeaderFile", DbType.String, HeaderFile)
            db.AddInParameter(cmd, "CartItemFile", DbType.String, CartItemFile)
            db.AddInParameter(cmd, "OrderStatus", DbType.Int32, OrderStatus)
            db.AddInParameter(cmd, "CartItemStatus", DbType.Int32, CartItemStatus)
            db.AddInParameter(cmd, "NoteHeaderFile", DbType.String, NoteHeaderFile)
            db.AddInParameter(cmd, "NoteCartItemStatus", DbType.String, NoteCartItemFile)
            db.ExecuteNonQuery(cmd)
        End Sub

        Public Overridable Sub Update()
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_EXPORTLOG_UPDATE As String = "sp_ExportLog_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_EXPORTLOG_UPDATE)

            db.AddInParameter(cmd, "OrderId", DbType.Int32, OrderId)
            db.AddInParameter(cmd, "HeaderFile", DbType.String, HeaderFile)
            db.AddInParameter(cmd, "CartItemFile", DbType.String, CartItemFile)
            db.AddInParameter(cmd, "OrderStatus", DbType.Int32, OrderStatus)
            db.AddInParameter(cmd, "CartItemStatus", DbType.Int32, CartItemStatus)
            db.AddInParameter(cmd, "NoteHeaderFile", DbType.String, NoteHeaderFile)
            db.AddInParameter(cmd, "NoteCartItemFile", DbType.String, NoteCartItemFile)
            db.ExecuteNonQuery(cmd)

        End Sub 'Update

        Public Sub Remove()
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_EXPORTLOG_DELETE As String = "sp_ExportLog_Delete"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_EXPORTLOG_DELETE)
            db.AddInParameter(cmd, "OrderId", DbType.Int32, OrderId)
            db.ExecuteNonQuery(cmd)

        End Sub 'Remove

    End Class

    Public Class ExportLogCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal ExportLog As ExportLogRow)
            Me.List.Add(ExportLog)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As ExportLogRow
            Get
                Return CType(Me.List.Item(Index), ExportLogRow)
            End Get

            Set(ByVal Value As ExportLogRow)
                Me.List(Index) = Value
            End Set
        End Property
        Public ReadOnly Property Clone() As ExportLogCollection
            Get
                If Me.List Is Nothing Then
                    Return Nothing
                End If
                Dim result As New ExportLogCollection
                For Each obj As ExportLogRow In Me.List
                    result.Add(CloneObject.Clone(obj))
                Next
                Return result
            End Get
        End Property
    End Class

End Namespace
