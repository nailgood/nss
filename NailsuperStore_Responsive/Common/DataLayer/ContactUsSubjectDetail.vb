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

    Public Class ContactUsSubjectDetailRow
        Inherits ContactUsSubjectDetailRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal DetailID As Integer)
            MyBase.New(DB, DetailID)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal DetailID As Integer) As ContactUsSubjectDetailRow
            Dim row As ContactUsSubjectDetailRow

            row = New ContactUsSubjectDetailRow(DB, DetailID)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal DetailID As Integer)
            Dim row As ContactUsSubjectDetailRow

            row = New ContactUsSubjectDetailRow(DB, DetailID)
            row.Remove()
        End Sub

        'Custom Methods
        Public Shared Function GetAllContactUsSubjectDetails(ByVal DB1 As Database) As DataTable
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 11:52:53 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_CONTACTUSSUBJECTDETAIL_GETLIST As String = "sp_ContactUsSubjectDetail_GetListAll"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_CONTACTUSSUBJECTDETAIL_GETLIST)
            Return db.ExecuteDataSet(cmd).Tables(0)
            '------------------------------------------------------------------------
        End Function
        Public Shared Function GetByEmailId(ByVal DB1 As Database, ByVal EmailId As Integer) As DataTable
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_CONTACTUSSUBJECTDETAIL_GETBYEMAILID As String = "sp_ContactUsSubjectDetail_GetByEmailId"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_CONTACTUSSUBJECTDETAIL_GETBYEMAILID)
            db.AddInParameter(cmd, "EmailId", DbType.Int32, EmailId)
            Return db.ExecuteDataSet(cmd).Tables(0)
        End Function

        Public Shared Sub DeleteAllByEmail(ByVal DB1 As Database, ByVal EmailId As Integer)
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_CONTACTUSSUBJECTDETAIL_DELETEALLBYEMAILID As String = "sp_ContactUsSubjectDetail_DeleteAllByEmailId"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_CONTACTUSSUBJECTDETAIL_DELETEALLBYEMAILID)
            db.AddInParameter(cmd, "EmailId", DbType.Int32, EmailId)
            db.ExecuteNonQuery(cmd)
        End Sub

    End Class

    Public MustInherit Class ContactUsSubjectDetailRowBase
        Private m_DB As Database
        Private m_DetailID As Integer = Nothing
        Private m_EmailID As Integer = Nothing
        Private m_SubjectID As Integer = Nothing
        Public Property DetailID() As Integer
            Get
                Return m_DetailID
            End Get
            Set(ByVal Value As Integer)
                m_DetailID = Value
            End Set
        End Property

        Public Property SubjectID() As Integer
            Get
                Return m_SubjectID
            End Get
            Set(ByVal Value As Integer)
                m_SubjectID = Value
            End Set
        End Property

        Public Property EmailID() As Integer
            Get
                Return m_EmailID
            End Get
            Set(ByVal Value As Integer)
                m_EmailID = Value
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

        Public Sub New(ByVal DB As Database, ByVal DetailID As Integer)
            m_DB = DB
            m_DetailID = DetailID
        End Sub 'New

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 11:47:46 AM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_CONTACTUSSUBJECTDETAIL_GETOBJECT As String = "sp_ContactUsSubjectDetail_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_CONTACTUSSUBJECTDETAIL_GETOBJECT)
                db.AddInParameter(cmd, "DetailID", DbType.Int32, DetailID)
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
            'Date: September 25, 2009 11:47:46 AM
            '------------------------------------------------------------------------
            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("DetailID"))) Then
                    DetailID = Convert.ToInt32(reader("DetailID"))
                Else
                    DetailID = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("SubjectID"))) Then
                    SubjectID = Convert.ToInt32(reader("SubjectID"))
                Else
                    SubjectID = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("EmailID"))) Then
                    EmailID = Convert.ToInt32(reader("EmailID"))
                Else
                    EmailID = 0
                End If
            End If
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 10:58:14 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_CONTACTUSSUBJECTDETAIL_UPDATE As String = "sp_ContactUsSubjectDetail_Insert"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_CONTACTUSSUBJECTDETAIL_UPDATE)
            db.AddOutParameter(cmd, "DetailID", DbType.Int32, 32)
            db.AddInParameter(cmd, "SubjectID", DbType.Int32, SubjectID)
            db.AddInParameter(cmd, "EmailID", DbType.Int32, EmailID)
            db.ExecuteNonQuery(cmd)
            DetailID = Convert.ToInt32(db.GetParameterValue(cmd, "DetailID"))
            '------------------------------------------------------------------------
            Return DetailID
        End Function

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 10:54:25 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_CONTACTUSSUBJECTDETAIL_UPDATE As String = "sp_ContactUsSubjectDetail_Update"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_CONTACTUSSUBJECTDETAIL_UPDATE)
            db.AddInParameter(cmd, "DetailID", DbType.Int32, DetailID)
            db.AddInParameter(cmd, "SubjectID", DbType.Int32, SubjectID)
            db.AddInParameter(cmd, "EmailID", DbType.Int32, EmailID)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Update      


        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_DELETE As String = "sp_ContactUsSubjectDetail_Delete"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_DELETE)
            db.AddInParameter(cmd, "DetailID", DbType.Int32, DetailID)
            db.ExecuteNonQuery(cmd)
            '----------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class ContactUsSubjectDetailCollection
        Inherits GenericCollection(Of ContactUsSubjectDetailRow)
    End Class

End Namespace


