Option Explicit On

Imports System
Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Components

Namespace DataLayer

    Public Class PriceMatchRow
        Inherits PriceMatchRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal PriceMatchId As Integer)
            MyBase.New(DB, PriceMatchId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal PriceMatchId As Integer) As PriceMatchRow
            Dim row As PriceMatchRow

            row = New PriceMatchRow(DB, PriceMatchId)
            row.Load()

            Return row
        End Function

        'end 23/10/2009
        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal PriceMatchId As Integer)
            Dim row As PriceMatchRow

            row = New PriceMatchRow(DB, PriceMatchId)
            row.Remove()
        End Sub

        'Custom Methods

    End Class

    Public MustInherit Class PriceMatchRowBase
        Private m_DB As Database
        Private m_PriceMatchId As Integer = Nothing
        Private m_YourName As String = Nothing
        Private m_PhoneNumber As String = Nothing
        Private m_EmailAddress As String = Nothing
        Private m_CompetitorsCompanyName As String = Nothing
        Private m_CompetitorsPhoneNumber As String = Nothing
        Private m_CompetitorsWebsite As String = Nothing
        Private m_CreateDate As DateTime = Nothing


        Public Property PriceMatchId() As Integer
            Get
                Return m_PriceMatchId
            End Get
            Set(ByVal Value As Integer)
                m_PriceMatchId = Value
            End Set
        End Property

        Public Property YourName() As String
            Get
                Return m_YourName
            End Get
            Set(ByVal Value As String)
                m_YourName = Value
            End Set
        End Property

        Public Property PhoneNumber() As String
            Get
                Return m_PhoneNumber
            End Get
            Set(ByVal Value As String)
                m_PhoneNumber = Value
            End Set
        End Property

        Public Property EmailAddress() As String
            Get
                Return m_EmailAddress
            End Get
            Set(ByVal Value As String)
                m_EmailAddress = Value
            End Set
        End Property

        Public Property CompetitorsCompanyName() As String
            Get
                Return m_CompetitorsCompanyName
            End Get
            Set(ByVal Value As String)
                m_CompetitorsCompanyName = Value
            End Set
        End Property

        Public Property CompetitorsPhoneNumber() As String
            Get
                Return m_CompetitorsPhoneNumber
            End Get
            Set(ByVal Value As String)
                m_CompetitorsPhoneNumber = Value
            End Set
        End Property

        Public Property CompetitorsWebsite() As String
            Get
                Return m_CompetitorsWebsite
            End Get
            Set(ByVal Value As String)
                m_CompetitorsWebsite = Value
            End Set
        End Property

        Public ReadOnly Property CreateDate() As DateTime
            Get
                Return m_CreateDate
            End Get
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

        Public Sub New(ByVal DB As Database, ByVal PriceMatchId As Integer)
            m_DB = DB
            m_PriceMatchId = PriceMatchId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_GETOBJECT As String = "sp_PriceMatch_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)
                db.AddInParameter(cmd, "PriceMatchId", DbType.Int32, PriceMatchId)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try

        End Sub

        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            m_PriceMatchId = Convert.ToInt32(r.Item("PriceMatchId"))
            m_YourName = Convert.ToString(r.Item("YourName"))
            m_PhoneNumber = Convert.ToString(r.Item("PhoneNumber"))
            m_EmailAddress = Convert.ToString(r.Item("EmailAddress"))
            m_CompetitorsCompanyName = Convert.ToString(r.Item("CompetitorsCompanyName"))
            m_CompetitorsPhoneNumber = Convert.ToString(r.Item("CompetitorsPhoneNumber"))
            If IsDBNull(r.Item("CompetitorsWebsite")) Then
                m_CompetitorsWebsite = Nothing
            Else
                m_CompetitorsWebsite = Convert.ToString(r.Item("CompetitorsWebsite"))
            End If
            m_CreateDate = Convert.ToDateTime(r.Item("CreateDate"))
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            '----------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_INSERT As String = "sp_PriceMatch_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_INSERT)

            db.AddOutParameter(cmd, "PriceMatchId", DbType.Int32, 16)
            db.AddInParameter(cmd, "YourName", DbType.String, YourName)
            db.AddInParameter(cmd, "PhoneNumber", DbType.String, PhoneNumber)
            db.AddInParameter(cmd, "EmailAddress", DbType.String, EmailAddress)
            db.AddInParameter(cmd, "CompetitorsCompanyName", DbType.String, CompetitorsCompanyName)
            db.AddInParameter(cmd, "CompetitorsPhoneNumber", DbType.String, CompetitorsPhoneNumber)
            db.AddInParameter(cmd, "CompetitorsWebsite", DbType.String, CompetitorsWebsite)

            db.ExecuteNonQuery(cmd)

            PriceMatchId = Convert.ToInt32(db.GetParameterValue(cmd, "PriceMatchId"))
            '----------------------------------------------------------------------

            Return PriceMatchId
        End Function

        Public Overridable Sub Update()
            '----------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_UPDATE As String = "sp_PriceMatch_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_UPDATE)

            db.AddInParameter(cmd, "PriceMatchId", DbType.Int32, PriceMatchId)
            db.AddInParameter(cmd, "YourName", DbType.String, YourName)
            db.AddInParameter(cmd, "PhoneNumber", DbType.String, PhoneNumber)
            db.AddInParameter(cmd, "EmailAddress", DbType.String, EmailAddress)
            db.AddInParameter(cmd, "CompetitorsCompanyName", DbType.String, CompetitorsCompanyName)
            db.AddInParameter(cmd, "CompetitorsPhoneNumber", DbType.String, CompetitorsPhoneNumber)
            db.AddInParameter(cmd, "CompetitorsWebsite", DbType.String, CompetitorsWebsite)

            db.ExecuteNonQuery(cmd)

            '----------------------------------------------------------------------

        End Sub 'Update

        Public Sub Remove()
            '----------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_DELETE As String = "sp_PriceMatch_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_DELETE)

            db.AddInParameter(cmd, "PriceMatchId", DbType.Int32, PriceMatchId)

            db.ExecuteNonQuery(cmd)

            '----------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class PriceMatchCollection
        Inherits GenericCollection(Of PriceMatchRow)
    End Class

End Namespace


