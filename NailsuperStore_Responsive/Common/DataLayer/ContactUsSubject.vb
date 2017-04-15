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
Imports Database

Namespace DataLayer

    Public Class ContactUsSubjectRow
        Inherits ContactUsSubjectRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal SubjectId As Integer)
            MyBase.New(DB, SubjectId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal SubjectId As Integer) As ContactUsSubjectRow
            Dim row As ContactUsSubjectRow

            row = New ContactUsSubjectRow(DB, SubjectId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal SubjectId As Integer)
            Dim row As ContactUsSubjectRow

            row = New ContactUsSubjectRow(DB, SubjectId)
            row.Remove()
        End Sub

        'end 22/10/2009

        'Custom Methods
        Public Shared Function GetAllContactUsSubjects(ByVal DB1 As Database) As DataTable
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_GETLIST As String = "sp_ContactUsSubject_GetListAll"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST)
            Return db.ExecuteDataSet(cmd).Tables(0)
        End Function

        Public Shared Function GetTypeContactUsSubjects(ByVal DB1 As Database, ByVal SubjectTypeID As Integer) As DataTable

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_LIST As String = "sp_ContactUsSubject_GetListByType"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_LIST)
            db.AddInParameter(cmd, "SubjectTypeID", DbType.Int32, SubjectTypeID)
            Return db.ExecuteDataSet(cmd).Tables(0)
        End Function

        Public Shared Function GetByLink(ByVal link As String) As ContactUsSubjectRow
            Dim s As ContactUsSubjectRow
            Dim dr As SqlDataReader = Nothing
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_ContactUsSubject_GetByLink"

            Try
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "Link", DbType.String, link)
                dr = db.ExecuteReader(cmd)

                If dr.HasRows Then
                    s = mapList(Of ContactUsSubjectRow)(dr).Item(0)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "GetByLink", ex.Message & ", Stack trace:" & ex.StackTrace)
            End Try
            
            Return s
        End Function
    End Class

    Public MustInherit Class ContactUsSubjectRowBase
        Private m_DB As Database
        Private m_SubjectId As Integer = Nothing
        Private m_Subject As String = Nothing
        Private m_SortOrder As Integer = Nothing
        Private m_Email As String = Nothing
        Private m_Name As String = Nothing
        Public Property SubjectId() As Integer
            Get
                Return m_SubjectId
            End Get
            Set(ByVal Value As Integer)
                m_SubjectId = value
            End Set
        End Property

        Public Property Subject() As String
            Get
                Return m_Subject
            End Get
            Set(ByVal Value As String)
                m_Subject = value
            End Set
        End Property

        Public Property SortOrder() As Integer
            Get
                Return m_SortOrder
            End Get
            Set(ByVal Value As Integer)
                m_SortOrder = value
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

        Public Property Email() As String
            Get
                Return m_Email
            End Get
            Set(ByVal Value As String)
                m_Email = Value
            End Set
        End Property

        Public Property DB() As Database
            Get
                DB = m_DB
            End Get
            Set(ByVal Value As DataBase)
                m_DB = Value
            End Set
        End Property

        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            m_DB = DB
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal SubjectId As Integer)
            m_DB = DB
            m_SubjectId = SubjectId
        End Sub 'New

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_GETOBJECT As String = "sp_ContactUsSubject_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)
                db.AddInParameter(cmd, "SubjectId", DbType.Int32, SubjectId)
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

        Protected Overridable Sub Load(ByVal r As sqlDataReader)
            m_SubjectId = Convert.ToInt32(r.Item("SubjectId"))
            m_Subject = Convert.ToString(r.Item("Subject"))
            m_Email = Convert.ToString(r.Item("Email"))
            m_Name = Convert.ToString(r.Item("Name"))
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009
            '------------------------------------------------------------------------
            Dim SortOrder = GetMaxSortOrder() + 1
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_DELETE As String = "sp_ContactUsSubject_Insert"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_DELETE)
            db.AddOutParameter(cmd, "SubjectId", DbType.Int32, 32)
            db.AddInParameter(cmd, "Subject", DbType.String, Subject)
            db.AddInParameter(cmd, "SortOrder", DbType.Int32, SortOrder)
            db.ExecuteNonQuery(cmd)
            SubjectId = Convert.ToInt32(db.GetParameterValue(cmd, "SubjectID"))
            '----------------------------------------------------------------------
            Return SubjectId
        End Function

        Private Function GetMaxSortOrder() As Integer
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETOBJECT As String = "sp_ContactUsSubject_GetMaxSortOrder"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)

            Return Convert.ToInt32(db.ExecuteScalar(cmd))
        End Function

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_DELETE As String = "sp_ContactUsSubject_UpdateSubject"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_DELETE)
            db.AddInParameter(cmd, "SubjectId", DbType.Int32, SubjectId)
            db.AddInParameter(cmd, "Subject", DbType.String, Subject)
            db.ExecuteNonQuery(cmd)
            '----------------------------------------------------------------------
        End Sub 'Update

        Public Overridable Sub UpdateEmail()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_DELETE As String = "sp_ContactUsSubject_UpdateEmail"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_DELETE)
            db.AddInParameter(cmd, "SubjectId", DbType.Int32, SubjectId)
            db.AddInParameter(cmd, "Email", DbType.String, Email)
            db.AddInParameter(cmd, "Name", DbType.String, Email)
            db.ExecuteNonQuery(cmd)
            '----------------------------------------------------------------------
        End Sub 'Update

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_DELETE As String = "sp_ContactUsSubject_Delete"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_DELETE)
            db.AddInParameter(cmd, "SubjectId", DbType.Int32, SubjectId)
            db.ExecuteNonQuery(cmd)
            '----------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class ContactUsSubjectCollection
        Inherits GenericCollection(Of ContactUsSubjectRow)
    End Class

End Namespace


