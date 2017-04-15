Option Explicit On

'Author: Lam Le
'Date: 10/26/2009 2:12:55 PM

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
Imports System.Web.UI.WebControls
Imports Utility

Namespace DataLayer
    Public Class StateRow
        Inherits StateRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal StateId As Integer)
            MyBase.New(database, StateId)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal StateCode As String)
            MyBase.New(database, StateCode)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal StateId As Integer) As StateRow
            Dim row As StateRow

            row = New StateRow(_Database, StateId)
            row.Load()

            Return row
        End Function

        Public Shared Function GetRow(ByVal _Database As Database, ByVal StateCode As String) As StateRow
            Dim row As StateRow

            row = New StateRow(_Database, StateCode)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal StateId As Integer)
            Dim row As StateRow

            row = New StateRow(_Database, StateId)
            row.Remove()
        End Sub

        Private Shared _cacheKey As String = "State_"
        Public Shared Function GetStateList() As List(Of ListItem)
            Dim listItem As List(Of ListItem) = Nothing

            Dim key As String = _cacheKey & "GetStateList"
            listItem = CType(CacheUtils.GetCache(key), List(Of ListItem))

            If listItem Is Nothing Then
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

                Dim SP_STATE_GETLIST As String = "sp_State_GetListAll"

                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STATE_GETLIST)

                Dim reader As SqlDataReader = db.ExecuteReader(cmd)
                If reader.HasRows Then
                    listItem = New List(Of ListItem)()
                    While reader.Read()
                        listItem.Add(New Web.UI.WebControls.ListItem(reader("StateName"), reader("StateCode")))
                    End While
                End If
                CacheUtils.SetCache(key, listItem)
            End If
            Return listItem
        End Function

        Public Shared Function GetAllStatesSetupFee(ByVal itemId As Integer) As DataSet
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_STATE_GETLIST As String = "sp_State_GetListAll"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STATE_GETLIST)
            Return db.ExecuteDataSet(cmd)
        End Function
    End Class

    Public MustInherit Class StateRowBase
        Private m_DB As Database
        Private m_StateId As Integer = Nothing
        Private m_StateCode As String = Nothing
        Private m_StateName As String = Nothing
        Private m_TaxRate As Double = Nothing
        Private m_IncludeDelivery As Boolean = Nothing
        Private m_IncludeGiftWrap As Boolean = Nothing
        Private m_USPSOnly As Boolean = Nothing

        Public Property StateId() As Integer
            Get
                Return m_StateId
            End Get
            Set(ByVal Value As Integer)
                m_StateId = Value
            End Set
        End Property

        Public Property StateCode() As String
            Get
                Return m_StateCode
            End Get
            Set(ByVal Value As String)
                m_StateCode = Value
            End Set
        End Property

        Public Property StateName() As String
            Get
                Return m_StateName
            End Get
            Set(ByVal Value As String)
                m_StateName = Value
            End Set
        End Property

        Public Property TaxRate() As Double
            Get
                Return m_TaxRate
            End Get
            Set(ByVal Value As Double)
                m_TaxRate = Value
            End Set
        End Property

        Public Property IncludeDelivery() As Boolean
            Get
                Return m_IncludeDelivery
            End Get
            Set(ByVal Value As Boolean)
                m_IncludeDelivery = Value
            End Set
        End Property

        Public Property IncludeGiftWrap() As Boolean
            Get
                Return m_IncludeGiftWrap
            End Get
            Set(ByVal Value As Boolean)
                m_IncludeGiftWrap = Value
            End Set
        End Property

        Public Property USPSOnly() As Boolean
            Get
                Return m_USPSOnly
            End Get
            Set(ByVal Value As Boolean)
                m_USPSOnly = Value
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

        Public Sub New(ByVal database As Database, ByVal StateId As Integer)
            m_DB = database
            m_StateId = StateId
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal StateCode As String)
            m_DB = database
            m_StateCode = StateCode
        End Sub 'New

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_STATE_GETOBJECT As String = "sp_State_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STATE_GETOBJECT)
                db.AddInParameter(cmd, "StateCode", DbType.String, StateCode)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)

            End Try

            '------------------------------------------------------------------------
        End Sub

        Public Shared Function IsUSPSOnly(ByVal DB As Database, ByVal StateCode As String)
            If DB.ExecuteScalar("SELECT USPSOnly FROM State WHERE StateCode=" & DB.Quote(StateCode)) = True Then
                Return True
            Else
                Return False
            End If
        End Function

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("StateId"))) Then
                        m_StateId = Convert.ToInt32(reader("StateId"))
                    Else
                        m_StateId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("StateCode"))) Then
                        m_StateCode = reader("StateCode").ToString()
                    Else
                        m_StateCode = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("StateName"))) Then
                        m_StateName = reader("StateName").ToString()
                    Else
                        m_StateName = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("TaxRate"))) Then
                        m_TaxRate = Convert.ToDouble(reader("TaxRate"))
                    Else
                        m_TaxRate = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("IncludeDelivery"))) Then
                        m_IncludeDelivery = Convert.ToBoolean(reader("IncludeDelivery"))
                    Else
                        m_IncludeDelivery = False
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("IncludeGiftWrap"))) Then
                        m_IncludeGiftWrap = Convert.ToBoolean(reader("IncludeGiftWrap"))
                    Else
                        m_IncludeGiftWrap = False
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("USPSOnly"))) Then
                        m_USPSOnly = Convert.ToBoolean(reader("USPSOnly"))
                    Else
                        m_USPSOnly = False
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try

        End Sub 'Load

        Private ReadOnly Property InsertStatement() As String
            Get
                Dim SQL As String

                SQL = " INSERT INTO State (" _
                 & " StateCode" _
                 & ",StateName" _
                 & ",TaxRate" _
                 & ",IncludeDelivery" _
                 & ",USPSOnly" _
                 & ",IncludeGiftWrap" _
                 & ") VALUES (" _
                 & m_DB.Quote(StateCode) _
                 & "," & m_DB.Quote(StateName) _
                 & "," & m_DB.Quote(TaxRate) _
                 & "," & CInt(IncludeDelivery) _
                 & "," & CInt(USPSOnly) _
                 & "," & CInt(IncludeGiftWrap) _
                 & ")"

                Return SQL
            End Get
        End Property

        Public Overridable Sub Insert()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STATE_INSERT As String = "sp_State_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STATE_INSERT)

            db.AddInParameter(cmd, "StateId", DbType.Int32, StateId)
            db.AddInParameter(cmd, "StateCode", DbType.String, StateCode)
            db.AddInParameter(cmd, "StateName", DbType.String, StateName)
            db.AddInParameter(cmd, "TaxRate", DbType.Double, TaxRate)
            db.AddInParameter(cmd, "IncludeDelivery", DbType.Boolean, IncludeDelivery)
            db.AddInParameter(cmd, "IncludeGiftWrap", DbType.Boolean, IncludeGiftWrap)
            db.AddInParameter(cmd, "USPSOnly", DbType.Boolean, USPSOnly)

            db.ExecuteNonQuery(cmd)

            '------------------------------------------------------------------------
        End Sub 'Insert

        Function AutoInsert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STATE_INSERT As String = "sp_State_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STATE_INSERT)

            db.AddInParameter(cmd, "StateId", DbType.Int32, 32)
            db.AddInParameter(cmd, "StateCode", DbType.String, StateCode)
            db.AddInParameter(cmd, "StateName", DbType.String, StateName)
            db.AddInParameter(cmd, "TaxRate", DbType.Double, TaxRate)
            db.AddInParameter(cmd, "IncludeDelivery", DbType.Boolean, IncludeDelivery)
            db.AddInParameter(cmd, "IncludeGiftWrap", DbType.Boolean, IncludeGiftWrap)
            db.AddInParameter(cmd, "USPSOnly", DbType.Boolean, USPSOnly)

            db.ExecuteNonQuery(cmd)

            StateId = Convert.ToInt32(db.GetParameterValue(cmd, "StateId"))

            '------------------------------------------------------------------------
            Return StateId
        End Function


        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STATE_UPDATE As String = "sp_State_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STATE_UPDATE)

            db.AddInParameter(cmd, "StateId", DbType.Int32, StateId)
            db.AddInParameter(cmd, "StateCode", DbType.String, StateCode)
            db.AddInParameter(cmd, "StateName", DbType.String, StateName)
            db.AddInParameter(cmd, "TaxRate", DbType.Double, TaxRate)
            db.AddInParameter(cmd, "IncludeDelivery", DbType.Boolean, IncludeDelivery)
            db.AddInParameter(cmd, "IncludeGiftWrap", DbType.Boolean, IncludeGiftWrap)
            db.AddInParameter(cmd, "USPSOnly", DbType.Boolean, USPSOnly)

            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------

        End Sub 'Update

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:55 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_STATE_DELETE As String = "sp_State_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STATE_DELETE)

            db.AddInParameter(cmd, "StateId", DbType.Int32, StateId)

            db.ExecuteNonQuery(cmd)

            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class StateCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal State As StateRow)
            Me.List.Add(State)
        End Sub

        Public Function Contains(ByVal State As StateRow) As Boolean
            Return Me.List.Contains(State)
        End Function

        Public Function IndexOf(ByVal State As StateRow) As Integer
            Return Me.List.IndexOf(State)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal State As StateRow)
            Me.List.Insert(Index, State)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As StateRow
            Get
                Return CType(Me.List.Item(Index), StateRow)
            End Get

            Set(ByVal Value As StateRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal State As StateRow)
            Me.List.Remove(State)
        End Sub
    End Class
End Namespace