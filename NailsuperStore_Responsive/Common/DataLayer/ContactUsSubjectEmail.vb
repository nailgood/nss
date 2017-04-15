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

    Public Class ContactUsSubjectEmailRow
        Inherits ContactUsSubjectEmailRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal EmailID As Integer)
            MyBase.New(DB, EmailID)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal EmailID As Integer) As ContactUsSubjectEmailRow
            Dim row As ContactUsSubjectEmailRow

            row = New ContactUsSubjectEmailRow(DB, EmailID)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal EmailID As Integer)
            Dim row As ContactUsSubjectEmailRow

            row = New ContactUsSubjectEmailRow(DB, EmailID)
            row.Remove()
        End Sub

        'Custom Methods
        Public Shared Function GetAllContactUsSubjectEmails(ByVal DB1 As Database) As DataTable
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 12:10:48 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_CONTACTUSSUBJECTEMAIL_GETLIST As String = "sp_ContactUsSubjectEmail_GetListAll"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_CONTACTUSSUBJECTEMAIL_GETLIST)
            Return db.ExecuteDataSet(cmd).Tables(0)
            '------------------------------------------------------------------------
        End Function
        Public Shared Function GetAllContactUsSubjectEmailsGroup(ByVal DB As Database, ByVal id As String) As DataTable
            Dim dt As DataTable = DB.GetDataTable("select ce.emailid,ce.name,ce.email,cd.detailid,cd.subjectid from ContactUsSubjectEmail ce,ContactUsSubjectDetail cd where ce.emailid=cd.emailid and Subjectid='" & id & "'")
            Return dt
        End Function

        Public Shared Function CheckEmailExist(ByVal DB As Database, ByVal EmailID As Integer, ByVal Email As String) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_ContactUsSubjectEmail_CheckEmailExist"
                Dim cmd As SqlCommand = DB.CreateCommand(sp)
                cmd.Parameters.Add(DB.InParam("EmailID", SqlDbType.Int, 0, EmailID))
                cmd.Parameters.Add(DB.InParam("Email", SqlDbType.VarChar, 0, Email))
                cmd.Parameters.Add(DB.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
                If result = 1 Then
                    Return True
                End If
            Catch ex As Exception

            End Try
            Return False

        End Function

    End Class

    Public MustInherit Class ContactUsSubjectEmailRowBase
        Private m_DB As Database
        Private m_EmailID As Integer = Nothing
        Private m_Email As String = Nothing
        Private m_Name As String = Nothing
        Public Property EmailID() As Integer
            Get
                Return m_EmailID
            End Get
            Set(ByVal Value As Integer)
                m_EmailID = Value
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
            Set(ByVal Value As Database)
                m_DB = Value
            End Set
        End Property

        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            m_DB = DB
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal EmailID As Integer)
            m_DB = DB
            m_EmailID = EmailID
        End Sub 'New

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 12:00:07 PM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_CONTACTUSSUBJECTEMAIL_GETOBJECT As String = "sp_ContactUsSubjectEmail_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_CONTACTUSSUBJECTEMAIL_GETOBJECT)
                db.AddInParameter(cmd, "EmailID", DbType.Int32, EmailID)
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
            'Date: September 25, 2009 12:00:07 PM
            '------------------------------------------------------------------------
            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("EmailID"))) Then
                    EmailID = Convert.ToInt32(reader("EmailID"))
                Else
                    EmailID = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Name"))) Then
                    Name = reader("Name").ToString()
                Else
                    Name = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Email"))) Then
                    Email = reader("Email").ToString()
                Else
                    Email = ""
                End If
            End If
            '------------------------------------------------------------------------
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 12:00:07 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_CONTACTUSSUBJECTEMAIL_INSERT As String = "sp_ContactUsSubjectEmail_Insert"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_CONTACTUSSUBJECTEMAIL_INSERT)
            db.AddOutParameter(cmd, "EmailID", DbType.Int32, 32)
            db.AddInParameter(cmd, "Name", DbType.String, Name)
            db.AddInParameter(cmd, "Email", DbType.String, Email)
            db.ExecuteNonQuery(cmd)
            EmailID = Convert.ToInt32(db.GetParameterValue(cmd, "EmailID"))
            '------------------------------------------------------------------------
            Return EmailID
        End Function

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 12:00:07 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_CONTACTUSSUBJECTEMAIL_UPDATE As String = "sp_ContactUsSubjectEmail_Update"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_CONTACTUSSUBJECTEMAIL_UPDATE)
            db.AddInParameter(cmd, "EmailID", DbType.Int32, EmailID)
            db.AddInParameter(cmd, "Name", DbType.String, Name)
            db.AddInParameter(cmd, "Email", DbType.String, Email)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Update      


        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 12:00:07 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_CONTACTUSSUBJECTEMAIL_DELETE As String = "sp_ContactUsSubjectEmail_Delete"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_CONTACTUSSUBJECTEMAIL_DELETE)
            db.AddInParameter(cmd, "EmailID", DbType.Int32, EmailID)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class ContactUsSubjectEmailCollection
        Inherits GenericCollection(Of ContactUsSubjectEmailRow)
    End Class

End Namespace


