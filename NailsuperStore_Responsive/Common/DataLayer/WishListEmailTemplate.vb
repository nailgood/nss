Option Explicit On
Imports System
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Components
Namespace DataLayer
    Public Class WishListEmailTemplateRow
        Inherits WishListEmailTemplateBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal EmailTemplateId As Integer)
            MyBase.New(database, EmailTemplateId)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal StateCode As String)
            MyBase.New(database, StateCode)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal EmailTemplateId As Integer) As WishListEmailTemplateRow
            Dim row As WishListEmailTemplateRow
            row = New WishListEmailTemplateRow(_Database, EmailTemplateId)
            row.Load()
            Return row
        End Function
       
        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal EmailTemplateId As Integer)
            Dim row As WishListEmailTemplateRow
            row = New WishListEmailTemplateRow(_Database, EmailTemplateId)
            row.Remove()
        End Sub
    End Class

    Public MustInherit Class WishListEmailTemplateBase
        Private m_DB As Database
        Private m_EmailTemplateId As Integer = Nothing
        Private m_EmailBodyText As String = Nothing
        Private m_EmailSubject As String = Nothing
        Private m_EmailPurpose As String = Nothing

        Public Property EmailTemplateId() As Integer
            Get
                Return m_EmailTemplateId
            End Get
            Set(ByVal Value As Integer)
                m_EmailTemplateId = Value
            End Set
        End Property

        Public Property EmailBodyText() As String
            Get
                Return m_EmailBodyText
            End Get
            Set(ByVal Value As String)
                m_EmailBodyText = Value
            End Set
        End Property

        Public Property EmailSubject() As String
            Get
                Return m_EmailSubject
            End Get
            Set(ByVal Value As String)
                m_EmailSubject = Value
            End Set
        End Property

        Public Property EmailPurpose() As String
            Get
                Return m_EmailPurpose
            End Get
            Set(ByVal Value As String)
                m_EmailPurpose = Value
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
            m_EmailTemplateId = StateId
        End Sub 'New
       
        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

                Dim SP_GETOBJECT As String = "sp_WishListEmailTemplate_GetObject"

                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)

                db.AddInParameter(cmd, "EmailTemplateId", DbType.Int32, EmailTemplateId)

                r = CType(db.ExecuteReader(cmd), SqlDataReader)

                If r.Read() Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub

        Public Overridable Sub Load(ByVal reader As SqlDataReader)
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 25, 2009 01:20:34 PM
            '------------------------------------------------------------------------
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("EmailTemplateId"))) Then
                        m_EmailTemplateId = Convert.ToInt32(reader("EmailTemplateId"))
                    Else
                        m_EmailTemplateId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("EmailBodyText"))) Then
                        m_EmailBodyText = reader("EmailBodyText").ToString()
                    Else
                        m_EmailBodyText = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("EmailSubject"))) Then
                        m_EmailSubject = reader("EmailSubject").ToString()
                    Else
                        m_EmailSubject = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("EmailPurpose"))) Then
                        m_EmailPurpose = reader("EmailPurpose").ToString()
                    Else
                        m_EmailPurpose = ""
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try

        End Sub 'Load

        Public Overridable Sub Insert()
            '----------------------------------------------------------------------
            'Start Edit by Lam Le, Aug 26, 2009

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_INSERT As String = "sp_WishListEmailTemplate_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_INSERT)

            db.AddInParameter(cmd, "EmailBodyText", DbType.String, EmailBodyText)
            db.AddInParameter(cmd, "EmailPurpose", DbType.String, EmailPurpose)
            db.AddInParameter(cmd, "EmailSubject", DbType.String, m_EmailSubject)

            db.ExecuteNonQuery(cmd)

            '----------------------------------------------------------------------

        End Sub 'Insert

        Public Overridable Sub Update()
            '----------------------------------------------------------------------
            'Start Edit by Lam Le, Aug 26, 2009
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_UPDATE As String = "sp_WishListEmailTemplate_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_UPDATE)

            db.AddInParameter(cmd, "EmailTemplateId", DbType.Int32, EmailTemplateId)
            db.AddInParameter(cmd, "EmailBodyText", DbType.String, EmailBodyText)
            db.AddInParameter(cmd, "EmailPurpose", DbType.String, EmailPurpose)
            db.AddInParameter(cmd, "EmailSubject", DbType.String, m_EmailSubject)

            db.ExecuteNonQuery(cmd)
            'End Edit
            '----------------------------------------------------------------------
        End Sub 'Update

        Public Sub Remove()
            'Edit by Lam Le, Aug 26 2009
            Dim db As Microsoft.Practices.EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase

            Dim SP_DELETE = "sp_WishListEmailTemplate_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_DELETE)

            db.AddInParameter(cmd, "EmailTemplateId", DbType.Int32, EmailTemplateId)

            db.ExecuteNonQuery(cmd)
            'End Edit
        End Sub 'Remove
    End Class
End Namespace

