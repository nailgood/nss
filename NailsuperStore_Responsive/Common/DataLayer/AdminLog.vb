Option Explicit On

'Author: Lam Le
'Date: 9/28/2009 9:48:31 AM

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

    Public Class AdminLogRow
        Inherits AdminLogRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal LogId As Integer)
            MyBase.New(LogId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal LogId As Integer) As AdminLogRow
            Dim row As AdminLogRow
            row = New AdminLogRow(LogId)
            row.Load()
            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal LogId As Integer)
            Dim row As AdminLogRow
            row = New AdminLogRow(LogId)
            row.Remove()
        End Sub

        'Custom Methods
        Public Shared Function GetLast10Logins() As DataSet
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 28, 2009 09:48:31 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_ADMINLOG_GETLIST As String = "sp_AdminLog_GetListTop10"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINLOG_GETLIST)
            Return db.ExecuteDataSet(cmd)
            '------------------------------------------------------------------------
        End Function
        Public Shared Function Insert(ByVal _Database As Database, ByVal objData As AdminLogRow)
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_AdminLog_Insert"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("LogId", SqlDbType.Int, 0, objData.LogId))
                cmd.Parameters.Add(_Database.InParam("AdminId", SqlDbType.Int, 0, objData.AdminId))
                cmd.Parameters.Add(_Database.InParam("Username", SqlDbType.VarChar, 0, objData.Username))
                cmd.Parameters.Add(_Database.InParam("RemoteIP", SqlDbType.VarChar, 0, objData.RemoteIP))
                cmd.Parameters.Add(_Database.InParam("LoginDate", SqlDbType.DateTime, 0, objData.LoginDate))
                cmd.Parameters.Add(_Database.InParam("LanIP", SqlDbType.VarChar, 0, objData.LanIP))
                cmd.Parameters.Add(_Database.InParam("ComputerName", SqlDbType.NVarChar, 0, objData.ComputerName))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            Return result
        End Function

    End Class

    Public MustInherit Class AdminLogRowBase

        Private m_LogId As Integer = Nothing
        Private m_AdminId As Integer = Nothing
        Private m_Username As String = Nothing
        Private m_RemoteIP As String = Nothing
        Private m_LoginDate As DateTime = Nothing
        Private m_LanIP As String = Nothing
        Private m_ComputerName As String = Nothing

        Public Property LogId() As Integer
            Get
                Return m_LogId
            End Get
            Set(ByVal Value As Integer)
                m_LogId = value
            End Set
        End Property

        Public Property AdminId() As Integer
            Get
                Return m_AdminId
            End Get
            Set(ByVal Value As Integer)
                m_AdminId = value
            End Set
        End Property

        Public Property Username() As String
            Get
                Return m_Username
            End Get
            Set(ByVal Value As String)
                m_Username = value
            End Set
        End Property
        
        Public Property RemoteIP() As String
            Get
                Return m_RemoteIP
            End Get
            Set(ByVal Value As String)
                m_RemoteIP = value
            End Set
        End Property

        Public Property LoginDate() As DateTime
            Get
                Return m_LoginDate
            End Get
            Set(ByVal Value As DateTime)
                m_LoginDate = value
            End Set
        End Property

        Public Property LanIP() As String
            Get
                Return m_LanIP
            End Get
            Set(ByVal Value As String)
                m_LanIP = Value
            End Set
        End Property
        Public Property ComputerName() As String
            Get
                Return m_ComputerName
            End Get
            Set(ByVal Value As String)
                m_ComputerName = Value
            End Set
        End Property

        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal LogId As Integer)
            m_LogId = LogId
        End Sub 'New

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 28, 2009 09:48:31 AM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_ADMINLOG_GETOBJECT As String = "sp_AdminLog_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINLOG_GETOBJECT)
                db.AddInParameter(cmd, "LogId", DbType.Int32, LogId)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Email.SendError("ToError500", "AdminLog.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 28, 2009 09:48:31 AM
            '------------------------------------------------------------------------
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("LogId"))) Then
                        m_LogId = Convert.ToInt32(reader("LogId"))
                    Else
                        m_LogId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("AdminId"))) Then
                        m_AdminId = Convert.ToInt32(reader("AdminId"))
                    Else
                        m_AdminId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Username"))) Then
                        m_Username = reader("Username").ToString()
                    Else
                        m_Username = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("RemoteIP"))) Then
                        m_RemoteIP = reader("RemoteIP").ToString()
                    Else
                        m_RemoteIP = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("LoginDate"))) Then
                        m_LoginDate = Convert.ToDateTime(reader("LoginDate"))
                    Else
                        m_LoginDate = DateTime.Now
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try

        End Sub 'Load

        'Public Overridable Sub Insert()
        '    '------------------------------------------------------------------------
        '    'Author: Lam Le
        '    'Date: September 28, 2009 09:48:31 AM
        '    '------------------------------------------------------------------------
        '    Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
        '    Dim SP_ADMINLOG_INSERT As String = "sp_AdminLog_Insert"
        '    Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINLOG_INSERT)
        '    db.AddOutParameter(cmd, "LogId", DbType.Int32, 32)
        '    db.AddInParameter(cmd, "AdminId", DbType.Int32, AdminId)
        '    db.AddInParameter(cmd, "Username", DbType.String, Username)
        '    db.AddInParameter(cmd, "RemoteIP", DbType.String, RemoteIP)
        '    db.AddInParameter(cmd, "LoginDate", DbType.DateTime, LoginDate)
        '    '' db.AddInParameter(cmd, "Message", DbType.String, Message)
        '    db.ExecuteNonQuery(cmd)
        '    '------------------------------------------------------------------------
        'End Sub 'Insert

        Function AutoInsert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 28, 2009 09:48:31 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_ADMINLOG_INSERT As String = "sp_AdminLog_Insert"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINLOG_INSERT)
            db.AddOutParameter(cmd, "LogId", DbType.Int32, 32)
            db.AddInParameter(cmd, "AdminId", DbType.Int32, AdminId)
            db.AddInParameter(cmd, "Username", DbType.String, Username)
            db.AddInParameter(cmd, "RemoteIP", DbType.String, RemoteIP)
            db.AddInParameter(cmd, "LoginDate", DbType.DateTime, LoginDate)
            ''db.AddInParameter(cmd, "Message", DbType.String, Message)
            db.ExecuteNonQuery(cmd)
            LogId = Convert.ToInt32(db.GetParameterValue(cmd, "LogId"))
            '------------------------------------------------------------------------
            Return LogId
        End Function

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 28, 2009 09:48:31 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_ADMINLOG_UPDATE As String = "sp_AdminLog_Update"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINLOG_UPDATE)
            db.AddInParameter(cmd, "LogId", DbType.Int32, LogId)
            db.AddInParameter(cmd, "AdminId", DbType.Int32, AdminId)
            db.AddInParameter(cmd, "Username", DbType.String, Username)
            db.AddInParameter(cmd, "RemoteIP", DbType.String, RemoteIP)
            db.AddInParameter(cmd, "LoginDate", DbType.DateTime, LoginDate)
            '' db.AddInParameter(cmd, "Message", DbType.String, Message)

            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Update

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 28, 2009 09:48:31 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_ADMINLOG_DELETE As String = "sp_AdminLog_Delete"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_ADMINLOG_DELETE)
            db.AddInParameter(cmd, "LogId", DbType.Int32, LogId)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class AdminLogCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal AdminLog As AdminLogRow)
            Me.List.Add(AdminLog)
        End Sub

        Public Function Contains(ByVal AdminLog As AdminLogRow) As Boolean
            Return Me.List.Contains(AdminLog)
        End Function

        Public Function IndexOf(ByVal AdminLog As AdminLogRow) As Integer
            Return Me.List.IndexOf(AdminLog)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal AdminLog As AdminLogRow)
            Me.List.Insert(Index, AdminLog)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As AdminLogRow
            Get
                Return CType(Me.List.Item(Index), AdminLogRow)
            End Get

            Set(ByVal Value As AdminLogRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal AdminLog As AdminLogRow)
            Me.List.Remove(AdminLog)
        End Sub
        Public ReadOnly Property Clone() As AdminLogCollection
            Get
                If Me.List Is Nothing Then
                    Return Nothing
                End If
                Dim result As New AdminLogCollection
                For Each obj As AdminLogRow In Me.List
                    result.Add(CloneObject.Clone(obj))
                Next
                Return result
            End Get
        End Property
    End Class

End Namespace


