Imports System.Data.SqlClient
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data.Common

Public Class ExportCustomerLogRow
    Inherits ExportCustomerLogBase

    Public Sub New()
        MyBase.New()
    End Sub 'New

    Public Sub New(ByVal DB As Database)
        MyBase.New(DB)
    End Sub 'New

    Public Sub New(ByVal DB As Database, ByVal CustomerId As Integer)
        MyBase.New(DB, CustomerId)
    End Sub 'New

    'Shared function to get one row
    Public Shared Function GetRow(ByVal DB As Database, ByVal CustomerId As Integer) As ExportCustomerLogRow
        Dim row As ExportCustomerLogRow
        row = New ExportCustomerLogRow(DB, CustomerId)
        row.Load()
        Return row
    End Function

End Class

Public MustInherit Class ExportCustomerLogBase
    Private m_CustomerId As Integer
    Private m_CustomerNo As String
    Private m_CustomerFile As String
    Private m_AddressFile As String
    Private m_CustomerStatus As String
    Private m_AddressStatus As String
    Private m_CreatedDate As DateTime
    Private m_ModifiedDate As DateTime
    Private m_NoteCustomerFile As String
    Private m_NoteAddressFile As String
    Private m_DB As Database

    Public Property CustomerId() As Integer
        Get
            Return m_CustomerId
        End Get
        Set(ByVal value As Integer)
            m_CustomerId = value
        End Set
    End Property

    Public Property CustomerNo() As String
        Get
            Return m_CustomerNo
        End Get
        Set(ByVal value As String)
            m_CustomerNo = value
        End Set
    End Property

    Public Property CustomerFile() As String
        Get
            Return m_CustomerFile
        End Get
        Set(ByVal value As String)
            m_CustomerFile = value
        End Set
    End Property

    Public Property AddressFile() As String
        Get
            Return m_AddressFile
        End Get
        Set(ByVal value As String)
            m_AddressFile = value
        End Set
    End Property

    Public Property CustomerStatus() As String
        Get
            Return m_CustomerStatus
        End Get
        Set(ByVal value As String)
            m_CustomerStatus = value
        End Set
    End Property

    Public Property AddressStatus() As String
        Get
            Return m_AddressStatus
        End Get
        Set(ByVal value As String)
            m_AddressStatus = value
        End Set
    End Property

    Public Property CreatedDate() As DateTime
        Get
            Return m_CreatedDate
        End Get
        Set(ByVal value As DateTime)
            m_CreatedDate = value
        End Set
    End Property

    Public Property ModifiedDate() As DateTime
        Get
            Return m_ModifiedDate
        End Get
        Set(ByVal value As DateTime)
            m_ModifiedDate = value
        End Set
    End Property

    Public Property NoteCustomerFile() As String
        Get
            Return m_NoteCustomerFile
        End Get
        Set(ByVal value As String)
            m_NoteCustomerFile = value
        End Set
    End Property

    Public Property NoteAddressFile() As String
        Get
            Return m_NoteAddressFile
        End Get
        Set(ByVal value As String)
            m_NoteAddressFile = value
        End Set
    End Property

    Public Property DB() As Database
        Get
            Return m_DB
        End Get
        Set(ByVal value As Database)
            m_DB = value
        End Set
    End Property

    Public Sub New()
    End Sub
    Public Sub New(ByVal DB As Database)
        m_DB = DB
    End Sub
    Public Sub New(ByVal DB As Database, ByVal CustomerId As Integer)
        m_DB = DB
        m_CustomerId = CustomerId
    End Sub

    Protected Overridable Sub Load()
        Dim r As SqlDataReader
        Dim SQL As String

        SQL = "SELECT * FROM ExportCustomerLog WHERE CustomerId = " & DB.Quote(CustomerId)
        r = m_DB.GetReader(SQL)
        If r.Read Then
            Me.Load(r)
        End If
        r.Close()
    End Sub


    Protected Overridable Sub Load(ByVal reader As SqlDataReader)
        If (Not reader Is Nothing And Not reader.IsClosed) Then
            If (Not reader.IsDBNull(reader.GetOrdinal("CustomerId"))) Then
                m_CustomerId = Convert.ToInt32(reader("CustomerId"))
            Else
                m_CustomerId = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("CustomerNo"))) Then
                m_CustomerNo = reader("CustomerNo").ToString()
            Else
                m_CustomerNo = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("CustomerFile"))) Then
                m_CustomerFile = reader("CustomerFile").ToString()
            Else
                m_CustomerFile = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("AddressFile"))) Then
                m_AddressFile = reader("AddressFile").ToString()
            Else
                m_AddressFile = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("CustomerStatus"))) Then
                m_CustomerStatus = reader("CustomerStatus").ToString()
            Else
                m_CustomerStatus = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("AddressStatus"))) Then
                m_AddressStatus = reader("AddressStatus").ToString()
            Else
                m_AddressStatus = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("CreatedDate"))) Then
                m_CreatedDate = Convert.ToDateTime(reader("CreatedDate"))
            Else
                m_CreatedDate = Nothing
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("ModifiedDate"))) Then
                m_ModifiedDate = Convert.ToDateTime(reader("ModifiedDate"))
            Else
                m_ModifiedDate = Nothing
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("NoteCustomerFile"))) Then
                m_NoteCustomerFile = reader("NoteCustomerFile").ToString()
            Else
                m_NoteCustomerFile = ""
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("NoteAddressFile"))) Then
                m_NoteAddressFile = reader("NoteAddressFile").ToString()
            Else
                m_NoteAddressFile = ""
            End If
        End If
    End Sub 'Load

    Public Overridable Sub Insert()
        Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

        Dim SP_EXPORTCUSTOMERLOG_INSERT As String = "sp_ExportCustomerLog_Insert"

        Dim cmd As DbCommand = db.GetStoredProcCommand(SP_EXPORTCUSTOMERLOG_INSERT)

        db.AddInParameter(cmd, "CustomerId", DbType.Int32, CustomerId)
        db.AddInParameter(cmd, "CustomerNo", DbType.String, CustomerNo)
        db.AddInParameter(cmd, "CustomerFile", DbType.String, CustomerFile)
        db.AddInParameter(cmd, "AddressFile", DbType.String, AddressFile)
        db.AddInParameter(cmd, "CustomerStatus", DbType.String, CustomerStatus)
        db.AddInParameter(cmd, "AddressStatus", DbType.String, AddressStatus)
        db.AddInParameter(cmd, "CreatedDate", DbType.DateTime, CreatedDate)
        db.AddInParameter(cmd, "NoteCustomerFile", DbType.String, NoteCustomerFile)
        db.AddInParameter(cmd, "NoteAddressFile", DbType.String, NoteAddressFile)
        db.ExecuteNonQuery(cmd)
    End Sub

    Public Overridable Sub Update()
        Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

        Dim SP_EXPORTCUSTOMERLOG_UPDATE As String = "sp_ExportCustomerLog_Update"

        Dim cmd As DbCommand = db.GetStoredProcCommand(SP_EXPORTCUSTOMERLOG_UPDATE)

        db.AddInParameter(cmd, "CustomerId", DbType.Int32, CustomerId)
        db.AddInParameter(cmd, "CustomerNo", DbType.String, CustomerNo)
        db.AddInParameter(cmd, "CustomerFile", DbType.String, CustomerFile)
        db.AddInParameter(cmd, "AddressFile", DbType.String, AddressFile)
        db.AddInParameter(cmd, "CustomerStatus", DbType.String, CustomerStatus)
        db.AddInParameter(cmd, "AddressStatus", DbType.String, AddressStatus)
        If Not ModifiedDate = DateTime.MinValue Then
            db.AddInParameter(cmd, "ModifiedDate", DbType.DateTime, ModifiedDate)
        End If
        db.AddInParameter(cmd, "NoteCustomerFile", DbType.String, NoteCustomerFile)
        db.AddInParameter(cmd, "NoteAddressFile", DbType.String, NoteAddressFile)
        db.ExecuteNonQuery(cmd)

    End Sub 'Update



End Class
