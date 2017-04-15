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

    Public Class ClassifiedRow
        Inherits ClassifiedRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal ClassifiedId As Integer)
            MyBase.New(DB, ClassifiedId)
        End Sub 'New
      
        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal ClassifiedId As Integer) As ClassifiedRow
            Dim row As ClassifiedRow

            row = New ClassifiedRow(DB, ClassifiedId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal ClassifiedId As Integer)
            Dim row As ClassifiedRow

            row = New ClassifiedRow(DB, ClassifiedId)
            row.Remove()
        End Sub

    End Class

    Public MustInherit Class ClassifiedRowBase
        Private m_DB As Database
        Private m_ClassifiedId As Integer = Nothing
        Private m_ClassifiedCategoryId As Integer = Nothing
        Private m_Title As String = Nothing
        Private m_Description As String = Nothing
        Private m_Photo0 As String = Nothing
        Private m_Photo1 As String = Nothing
        Private m_Photo2 As String = Nothing
        Private m_Photo3 As String = Nothing
        Private m_Photo4 As String = Nothing
        Private m_Photo5 As String = Nothing
        Private m_ContactName As String = Nothing
        Private m_ContactNumber As String = Nothing
        Private m_Email As String = Nothing
        Private m_IsActive As Boolean = Nothing
        Private m_ExpirationDate As DateTime = Nothing
        Private m_MemberId As Integer = Nothing

        Public Property MemberId() As Integer
            Get
                Return m_MemberId
            End Get
            Set(ByVal Value As Integer)
                m_MemberId = Value
            End Set
        End Property

        Public Property ClassifiedId() As Integer
            Get
                Return m_ClassifiedId
            End Get
            Set(ByVal Value As Integer)
                m_ClassifiedId = value
            End Set
        End Property

        Public Property ClassifiedCategoryId() As Integer
            Get
                Return m_ClassifiedCategoryId
            End Get
            Set(ByVal Value As Integer)
                m_ClassifiedCategoryId = Value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return m_Title
            End Get
            Set(ByVal Value As String)
                m_Title = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set(ByVal Value As String)
                m_Description = value
            End Set
        End Property

        Public Property Photo0() As String
            Get
                Return m_Photo0
            End Get
            Set(ByVal Value As String)
                m_Photo0 = Value
            End Set
        End Property

        Public Property Photo1() As String
            Get
                Return m_Photo1
            End Get
            Set(ByVal Value As String)
                m_Photo1 = Value
            End Set
        End Property

        Public Property Photo2() As String
            Get
                Return m_Photo2
            End Get
            Set(ByVal Value As String)
                m_Photo2 = Value
            End Set
        End Property

        Public Property Photo3() As String
            Get
                Return m_Photo3
            End Get
            Set(ByVal Value As String)
                m_Photo3 = Value
            End Set
        End Property

        Public Property Photo4() As String
            Get
                Return m_Photo4
            End Get
            Set(ByVal Value As String)
                m_Photo4 = Value
            End Set
        End Property

        Public Property Photo5() As String
            Get
                Return m_Photo5
            End Get
            Set(ByVal Value As String)
                m_Photo5 = Value
            End Set
        End Property

        Public Property ContactName() As String
            Get
                Return m_ContactName
            End Get
            Set(ByVal Value As String)
                m_ContactName = Value
            End Set
        End Property

        Public Property ContactNumber() As String
            Get
                Return m_ContactNumber
            End Get
            Set(ByVal Value As String)
                m_ContactNumber = value
            End Set
        End Property

        Public Property Email() As String
            Get
                Return m_Email
            End Get
            Set(ByVal Value As String)
                m_Email = value
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

        Public Property ExpirationDate() As DateTime
            Get
                Return m_ExpirationDate
            End Get
            Set(ByVal Value As DateTime)
                m_ExpirationDate = value
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

        Public Sub New(ByVal DB As Database, ByVal ClassifiedId As Integer)
            m_DB = DB
            m_ClassifiedId = ClassifiedId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT * FROM Classified WHERE ClassifiedId = " & DB.Number(ClassifiedId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            
        End Sub


        Protected Overridable Sub Load(ByVal r As sqlDataReader)
            m_ClassifiedId = Convert.ToInt32(r.Item("ClassifiedId"))
            If IsDBNull(r.Item("MemberId")) Then
                m_MemberId = Nothing
            Else
                m_MemberId = Convert.ToInt32(r.Item("MemberId"))
            End If
            If IsDBNull(r.Item("ClassifiedCategoryId")) Then
                m_ClassifiedCategoryId = Nothing
            Else
                m_ClassifiedCategoryId = Convert.ToInt32(r.Item("ClassifiedCategoryId"))
            End If
            If IsDBNull(r.Item("Title")) Then
                m_Title = Nothing
            Else
                m_Title = Convert.ToString(r.Item("Title"))
            End If
            If IsDBNull(r.Item("Description")) Then
                m_Description = Nothing
            Else
                m_Description = Convert.ToString(r.Item("Description"))
            End If
            If IsDBNull(r.Item("Photo0")) Then
                m_Photo0 = Nothing
            Else
                m_Photo0 = Convert.ToString(r.Item("Photo0"))
            End If
            If IsDBNull(r.Item("Photo1")) Then
                m_Photo1 = Nothing
            Else
                m_Photo1 = Convert.ToString(r.Item("Photo1"))
            End If
            If IsDBNull(r.Item("Photo2")) Then
                m_Photo2 = Nothing
            Else
                m_Photo2 = Convert.ToString(r.Item("Photo2"))
            End If
            If IsDBNull(r.Item("Photo3")) Then
                m_Photo3 = Nothing
            Else
                m_Photo3 = Convert.ToString(r.Item("Photo3"))
            End If
            If IsDBNull(r.Item("Photo4")) Then
                m_Photo4 = Nothing
            Else
                m_Photo4 = Convert.ToString(r.Item("Photo4"))
            End If
            If IsDBNull(r.Item("Photo5")) Then
                m_Photo5 = Nothing
            Else
                m_Photo5 = Convert.ToString(r.Item("Photo5"))
            End If
            If IsDBNull(r.Item("ContactName")) Then
                m_ContactName = Nothing
            Else
                m_ContactName = Convert.ToString(r.Item("ContactName"))
            End If
            If IsDBNull(r.Item("ContactNumber")) Then
                m_ContactNumber = Nothing
            Else
                m_ContactNumber = Convert.ToString(r.Item("ContactNumber"))
            End If
            If IsDBNull(r.Item("Email")) Then
                m_Email = Nothing
            Else
                m_Email = Convert.ToString(r.Item("Email"))
            End If
            m_IsActive = Convert.ToBoolean(r.Item("IsActive"))
            If IsDBNull(r.Item("ExpirationDate")) Then
                m_ExpirationDate = Nothing
            Else
                m_ExpirationDate = Convert.ToDateTime(r.Item("ExpirationDate"))
            End If
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String


            SQL = " INSERT INTO Classified (" _
             & " ClassifiedCategoryId" _
             & ",Title" _
             & ",Description" _
             & ",Photo0" _
             & ",Photo1" _
             & ",Photo2" _
             & ",Photo3" _
             & ",Photo4" _
             & ",Photo5" _
             & ",ContactName" _
             & ",ContactNumber" _
             & ",Email" _
             & ",IsActive" _
             & ",MemberId" _
             & ",ExpirationDate" _
             & ") VALUES (" _
             & m_DB.NullNumber(ClassifiedCategoryId) _
             & "," & m_DB.NQuote(Title) _
             & "," & m_DB.NQuote(Description) _
             & "," & m_DB.Quote(Photo0) _
             & "," & m_DB.Quote(Photo1) _
             & "," & m_DB.Quote(Photo2) _
             & "," & m_DB.Quote(Photo3) _
             & "," & m_DB.Quote(Photo4) _
             & "," & m_DB.Quote(Photo5) _
             & "," & m_DB.Quote(ContactName) _
             & "," & m_DB.Quote(ContactNumber) _
             & "," & m_DB.Quote(Email) _
             & "," & CInt(IsActive) _
             & "," & m_DB.Number(MemberId) _
             & "," & m_DB.NullQuote(ExpirationDate) _
             & ")"

            ClassifiedId = m_DB.InsertSQL(SQL)

            Return ClassifiedId
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE Classified SET " _
             & " ClassifiedCategoryId = " & m_DB.NullNumber(ClassifiedCategoryId) _
             & ",Title = " & m_DB.NQuote(Title) _
             & ",Description = " & m_DB.NQuote(Description) _
             & ",Photo0 = " & m_DB.Quote(Photo0) _
             & ",Photo1 = " & m_DB.Quote(Photo1) _
             & ",Photo2 = " & m_DB.Quote(Photo2) _
             & ",Photo3 = " & m_DB.Quote(Photo3) _
             & ",Photo4 = " & m_DB.Quote(Photo4) _
             & ",Photo5 = " & m_DB.Quote(Photo5) _
             & ",ContactName = " & m_DB.Quote(ContactName) _
             & ",ContactNumber = " & m_DB.Quote(ContactNumber) _
             & ",Email = " & m_DB.Quote(Email) _
             & ",IsActive = " & CInt(IsActive) _
             & ",MemberId = " & m_DB.Number(MemberId) _
             & ",ExpirationDate = " & m_DB.NullQuote(ExpirationDate) _
             & " WHERE ClassifiedId = " & m_DB.Quote(ClassifiedId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM Classified WHERE ClassifiedId = " & m_DB.Quote(ClassifiedId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class ClassifiedCollection
        Inherits GenericCollection(Of ClassifiedRow)
    End Class

End Namespace


