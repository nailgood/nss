Option Explicit On

'Author: Lam Le
'Date: 9/30/2009 10:10:11 AM

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
    Public Class ImExLogRow
        Inherits ImExLogRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal LogID As Integer)
            MyBase.New(DB, LogID)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal LogID As Integer) As ImExLogRow
            Dim row As ImExLogRow

            row = New ImExLogRow(DB, LogID)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal LogID As Integer)
            Dim row As ImExLogRow

            row = New ImExLogRow(DB, LogID)
            row.Remove()
        End Sub

        'Custom Methods
        Public Shared Function GetAllImExLogs(ByVal DB As Database) As DataTable
            Dim dt As DataTable = DB.GetDataTable("select * from ImExLog order by LogDate")
            Return dt
        End Function

        Public Shared Function GetTypeImExLogs(ByVal DB As Database, ByVal LogTypeID As Integer) As DataTable
            Dim dt As DataTable = DB.GetDataTable("select * from ImExLog WHERE LogType = " & DB.Number(LogTypeID))
            Return dt
        End Function
    End Class

    Public MustInherit Class ImExLogRowBase
        Private m_DB As Database
        Private m_LogID As Integer = Nothing
        Private m_LogTypeID As Integer = Nothing
        Private m_LogFile As String = Nothing
        Private m_Contents As String = Nothing
        Public Property LogID() As Integer
            Get
                Return m_LogID
            End Get
            Set(ByVal Value As Integer)
                m_LogID = Value
            End Set
        End Property

        Public Property LogTypeID() As Integer
            Get
                Return m_LogTypeID
            End Get
            Set(ByVal Value As Integer)
                m_LogTypeID = Value
            End Set
        End Property

        Public Property LogFile() As String
            Get
                Return m_LogFile
            End Get
            Set(ByVal Value As String)
                m_LogFile = Value
            End Set
        End Property

        Public Property Contents() As String
            Get
                Return m_Contents
            End Get
            Set(ByVal Value As String)
                m_Contents = Value
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

        Public Sub New(ByVal DB As Database, ByVal LogID As Integer)
            m_DB = DB
            m_LogID = LogID
        End Sub 'New

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 30, 2009 10:10:11 AM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_IMEXLOG_GETOBJECT As String = "sp_ImExLog_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_IMEXLOG_GETOBJECT)
                db.AddInParameter(cmd, "LogID", DbType.Int32, LogID)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try

            '------------------------------------------------------------------------
        End Sub


        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 30, 2009 10:10:11 AM
            '------------------------------------------------------------------------
            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("LogID"))) Then
                    m_LogID = Convert.ToInt32(reader("LogID"))
                Else
                    m_LogID = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("LogFile"))) Then
                    m_LogFile = reader("LogFile").ToString()
                Else
                    m_LogFile = ""
                End If
                ' Trung Nguyen add - get LogType
                If (Not reader.IsDBNull(reader.GetOrdinal("LogType"))) Then
                    m_LogTypeID = Convert.ToInt32(reader("LogType"))
                Else
                    m_LogTypeID = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("Contents"))) Then
                    m_Contents = reader("Contents").ToString()
                Else
                    m_Contents = ""
                End If
            End If

        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String
            SQL = " INSERT INTO ImExLog (" _
             & " LogType" _
             & ",LogDate" _
             & ",LogFile" _
             & ") VALUES (" _
             & m_DB.Quote(LogTypeID) _
             & ",getdate()" _
             & "," & m_DB.Quote(LogFile) _
             & ")"
            LogID = m_DB.InsertSQL(SQL)
            Return LogID
        End Function

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 30, 2009 10:10:11 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_IMEXLOG_DELETE As String = "sp_ImExLog_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_IMEXLOG_DELETE)

            db.AddInParameter(cmd, "LogID", DbType.Int32, LogID)

            db.ExecuteNonQuery(cmd)

            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class ImExLogCollection
        Inherits GenericCollection(Of ImExLogRow)
    End Class
End Namespace

