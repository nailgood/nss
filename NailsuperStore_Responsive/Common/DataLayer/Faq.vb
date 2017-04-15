Option Explicit On

'Author: Lam Le
'Date: 9/29/2009 10:07:52 AM

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

    Public Class FaqRow
        Inherits FaqRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal FaqId As Integer)
            MyBase.New(DB, FaqId)
        End Sub 'New
      
        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal FaqId As Integer) As FaqRow
            Dim row As FaqRow

            row = New FaqRow(DB, FaqId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal FaqId As Integer)
            Dim row As FaqRow

            row = New FaqRow(DB, FaqId)
            row.Remove()
        End Sub

        'Custom Methods

    End Class

    Public MustInherit Class FaqRowBase
        Private m_DB As Database
        Private m_FaqId As Integer = Nothing
        Private m_FaqCategoryId As Integer = Nothing
        Private m_IsActive As Boolean = Nothing
        Private m_SortOrder As Integer = Nothing
        Private m_Question As String = Nothing
        Private m_Answer As String = Nothing
        Private m_Email As String = Nothing
        Private m_IsMyOrderPage As Boolean = Nothing

        Public Property IsMyOrderPage() As Boolean
            Get
                Return m_IsMyOrderPage
            End Get
            Set(ByVal value As Boolean)
                m_IsMyOrderPage = value
            End Set
        End Property

        Public Property FaqId() As Integer
            Get
                Return m_FaqId
            End Get
            Set(ByVal Value As Integer)
                m_FaqId = value
            End Set
        End Property

        Public Property FaqCategoryId() As Integer
            Get
                Return m_FaqCategoryId
            End Get
            Set(ByVal Value As Integer)
                m_FaqCategoryId = value
            End Set
        End Property

        Public Property IsActive() As Boolean
            Get
                Return m_IsActive
            End Get
            Set(ByVal Value As Boolean)
                m_IsActive = value
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

        Public Property Question() As String
            Get
                Return m_Question
            End Get
            Set(ByVal Value As String)
                m_Question = value
            End Set
        End Property

        Public Property Answer() As String
            Get
                Return m_Answer
            End Get
            Set(ByVal Value As String)
                m_Answer = value
            End Set
        End Property

        Public Property Email() As String
            Get
                Return m_Email
            End Get
            Set(ByVal value As String)
                m_Email = value
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

        Public Sub New(ByVal DB As Database, ByVal FaqId As Integer)
            m_DB = DB
            m_FaqId = FaqId
        End Sub 'New
     
        'end 23/10/2009
        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 29, 2009 10:07:52 AM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_FAQ_GETOBJECT As String = "sp_Faq_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_FAQ_GETOBJECT)
                db.AddInParameter(cmd, "FaqId", DbType.Int32, FaqId)
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
            'Date: September 29, 2009 10:07:52 AM
            '------------------------------------------------------------------------
            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("FaqId"))) Then
                    m_FaqId = Convert.ToInt32(reader("FaqId"))
                Else
                    m_FaqId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("FaqCategoryId"))) Then
                    m_FaqCategoryId = Convert.ToInt32(reader("FaqCategoryId"))
                Else
                    m_FaqCategoryId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                    m_IsActive = Convert.ToBoolean(reader("IsActive"))
                Else
                    m_IsActive = False
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("SortOrder"))) Then
                    m_SortOrder = Convert.ToInt32(reader("SortOrder"))
                Else
                    m_SortOrder = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Question"))) Then
                    m_Question = reader("Question").ToString()
                Else
                    m_Question = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Answer"))) Then
                    m_Answer = reader("Answer").ToString()
                Else
                    m_Answer = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Email"))) Then
                    m_Email = reader("Email").ToString()
                Else
                    m_Email = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("IsMyOrderPage"))) Then
                    m_IsMyOrderPage = Convert.ToBoolean(reader("IsMyOrderPage"))
                Else
                    m_IsMyOrderPage = False
                End If
            End If
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 29, 2009 10:07:52 AM
            '------------------------------------------------------------------------
            Dim maxSortOrder As Integer = GetMaxSortOrder()
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_FAQ_INSERT As String = "sp_Faq_Insert"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_FAQ_INSERT)
            db.AddOutParameter(cmd, "FaqId", DbType.Int32, 32)
            db.AddInParameter(cmd, "FaqCategoryId", DbType.Int32, FaqCategoryId)
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
            db.AddInParameter(cmd, "SortOrder", DbType.Int32, maxSortOrder)
            db.AddInParameter(cmd, "Question", DbType.String, Question)
            db.AddInParameter(cmd, "Answer", DbType.String, Answer)
            db.AddInParameter(cmd, "Email", DbType.String, Email)
            db.AddInParameter(cmd, "IsMyOrderPage", DbType.Boolean, IsMyOrderPage)
            db.ExecuteNonQuery(cmd)
            FaqId = Convert.ToInt32(db.GetParameterValue(cmd, "FaqId"))
            '------------------------------------------------------------------------
            Return FaqId
        End Function

        Private Function GetMaxSortOrder() As Integer
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETOBJECT As String = "sp_Faq_GetMaxSortOrder"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)

            Return Convert.ToInt32(db.ExecuteScalar(cmd))
        End Function

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 29, 2009 10:07:52 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_FAQ_UPDATE As String = "sp_Faq_Update"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_FAQ_UPDATE)
            db.AddInParameter(cmd, "FaqId", DbType.Int32, FaqId)
            db.AddInParameter(cmd, "FaqCategoryId", DbType.Int32, FaqCategoryId)
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
            db.AddInParameter(cmd, "SortOrder", DbType.Int32, SortOrder)
            db.AddInParameter(cmd, "Question", DbType.String, Question)
            db.AddInParameter(cmd, "Answer", DbType.String, Answer)
            db.AddInParameter(cmd, "Email", DbType.String, Email)
            db.AddInParameter(cmd, "IsMyOrderPage", DbType.Boolean, IsMyOrderPage)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Update

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 29, 2009 10:07:52 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_FAQ_DELETE As String = "sp_Faq_Delete"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_FAQ_DELETE)
            db.AddInParameter(cmd, "FaqId", DbType.Int32, FaqId)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class FaqCollection
        Inherits GenericCollection(Of FaqRow)
    End Class

End Namespace

