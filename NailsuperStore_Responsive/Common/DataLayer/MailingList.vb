Option Explicit On

'Author: Lam Le
'Date: 9/29/2009 10:07:53 AM

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

    Public Class MailingListRow
        Inherits MailingListRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal ListId As Integer)
            MyBase.New(database, ListId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal ListId As Integer) As MailingListRow
            Dim row As MailingListRow

            row = New MailingListRow(_Database, ListId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal ListId As Integer)
            Dim row As MailingListRow

            row = New MailingListRow(_Database, ListId)
            row.Remove()
        End Sub

        'end 24/10/2009
        'Custom Methods
        Public Shared Function GetLists(ByVal db1 As Database) As DataTable
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_MAILINGLIST_GETLIST As String = "sp_MailingList_GetActiveList"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MAILINGLIST_GETLIST)

            Return db.ExecuteDataSet(cmd).Tables(0)

            '------------------------------------------------------------------------
        End Function

        Public Shared Function GetPermanentLists(ByVal db1 As Database) As DataTable
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:52 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_MAILINGLIST_GETLIST As String = "sp_MailingList_GetPermanentList"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MAILINGLIST_GETLIST)

            Return db.ExecuteDataSet(cmd).Tables(0)

            '------------------------------------------------------------------------
        End Function

        Public Shared Function GetDynamicLists(ByVal db1 As Database) As DataTable
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:52 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_MAILINGLIST_GETLIST As String = "sp_MailingList_GetDynamicsList"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MAILINGLIST_GETLIST)

            Return db.ExecuteDataSet(cmd).Tables(0)

            '------------------------------------------------------------------------
        End Function

    End Class

    Public MustInherit Class MailingListRowBase
        Private m_DB As Database
        Private m_ListId As Integer = Nothing
        Private m_Name As String = Nothing
        Private m_Status As String = Nothing
        Private m_Filename As String = Nothing
        Private m_CreateDate As DateTime = Nothing
        Private m_CreateAdminId As Integer = Nothing
        Private m_ModifyDate As DateTime = Nothing
        Private m_ModifyAdminId As Integer = Nothing
        Private m_IsPermanent As Boolean = Nothing


        Public Property ListId() As Integer
            Get
                Return m_ListId
            End Get
            Set(ByVal Value As Integer)
                m_ListId = Value
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

        Public Property Status() As String
            Get
                Return m_Status
            End Get
            Set(ByVal Value As String)
                m_Status = Value
            End Set
        End Property

        Public Property Filename() As String
            Get
                Return m_Filename
            End Get
            Set(ByVal Value As String)
                m_Filename = Value
            End Set
        End Property

        Public ReadOnly Property CreateDate() As DateTime
            Get
                Return m_CreateDate
            End Get
        End Property

        Public Property CreateAdminId() As Integer
            Get
                Return m_CreateAdminId
            End Get
            Set(ByVal Value As Integer)
                m_CreateAdminId = Value
            End Set
        End Property

        Public ReadOnly Property ModifyDate() As DateTime
            Get
                Return m_ModifyDate
            End Get
        End Property

        Public Property ModifyAdminId() As Integer
            Get
                Return m_ModifyAdminId
            End Get
            Set(ByVal Value As Integer)
                m_ModifyAdminId = Value
            End Set
        End Property

        Public Property IsPermanent() As Boolean
            Get
                Return m_IsPermanent
            End Get
            Set(ByVal Value As Boolean)
                m_IsPermanent = Value
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

        Public Sub New(ByVal database As Database, ByVal ListId As Integer)
            m_DB = database
            m_ListId = ListId
        End Sub 'New

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 29, 2009 10:07:53 AM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_MAILINGLIST_GETOBJECT As String = "sp_MailingList_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MAILINGLIST_GETOBJECT)
                db.AddInParameter(cmd, "ListId", DbType.Int32, ListId)
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
            'Date: September 29, 2009 10:07:53 AM
            '------------------------------------------------------------------------
            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("ListId"))) Then
                    m_ListId = Convert.ToInt32(reader("ListId"))
                Else
                    m_ListId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Name"))) Then
                    m_Name = reader("Name").ToString()
                Else
                    m_Name = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Status"))) Then
                    m_Status = reader("Status").ToString()
                Else
                    m_Status = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Filename"))) Then
                    m_Filename = reader("Filename").ToString()
                Else
                    m_Filename = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("CreateDate"))) Then
                    m_CreateDate = Convert.ToDateTime(reader("CreateDate"))
                Else
                    m_CreateDate = DateTime.Now
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("CreateAdminId"))) Then
                    m_CreateAdminId = Convert.ToInt32(reader("CreateAdminId"))
                Else
                    m_CreateAdminId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ModifyDate"))) Then
                    m_ModifyDate = Convert.ToDateTime(reader("ModifyDate"))
                Else
                    m_ModifyDate = DateTime.Now
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ModifyAdminId"))) Then
                    m_ModifyAdminId = Convert.ToInt32(reader("ModifyAdminId"))
                Else
                    m_ModifyAdminId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("IsPermanent"))) Then
                    m_IsPermanent = Convert.ToBoolean(reader("IsPermanent"))
                Else
                    m_IsPermanent = False
                End If
            End If
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 29, 2009 10:07:53 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_MAILINGLIST_INSERT As String = "sp_MailingList_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MAILINGLIST_INSERT)

            db.AddOutParameter(cmd, "ListId", DbType.Int32, ListId)
            db.AddInParameter(cmd, "Name", DbType.String, Name)
            db.AddInParameter(cmd, "Status", DbType.String, Status)
            db.AddInParameter(cmd, "Filename", DbType.String, Filename)
            db.AddInParameter(cmd, "CreateDate", DbType.DateTime, Now)
            db.AddInParameter(cmd, "CreateAdminId", DbType.Int32, CreateAdminId)
            db.AddInParameter(cmd, "ModifyDate", DbType.DateTime, Now)
            db.AddInParameter(cmd, "ModifyAdminId", DbType.Int32, CreateAdminId)
            db.AddInParameter(cmd, "IsPermanent", DbType.Boolean, IsPermanent)

            db.ExecuteNonQuery(cmd)

            ListId = Convert.ToInt32(db.GetParameterValue(cmd, "ListId"))

            '------------------------------------------------------------------------

            Return ListId
        End Function

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 29, 2009 10:07:53 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_MAILINGLIST_UPDATE As String = "sp_MailingList_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MAILINGLIST_UPDATE)

            db.AddInParameter(cmd, "ListId", DbType.Int32, ListId)
            db.AddInParameter(cmd, "Name", DbType.String, Name)
            db.AddInParameter(cmd, "Status", DbType.String, Status)
            db.AddInParameter(cmd, "Filename", DbType.String, Filename)
            db.AddInParameter(cmd, "ModifyDate", DbType.DateTime, ModifyDate)
            db.AddInParameter(cmd, "ModifyAdminId", DbType.Int32, ModifyAdminId)
            db.AddInParameter(cmd, "IsPermanent", DbType.Boolean, IsPermanent)

            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------

        End Sub 'Update

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 29, 2009 10:07:53 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_MAILINGLIST_DELETE As String = "sp_MailingList_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MAILINGLIST_DELETE)

            db.AddInParameter(cmd, "ListId", DbType.Int32, ListId)

            db.ExecuteNonQuery(cmd)

            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class MailingListCollection
        Inherits GenericCollection(Of MailingListRow)
    End Class

End Namespace

