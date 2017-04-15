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

    Public Class ClassifiedCategoryRow
        Inherits ClassifiedCategoryRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal ClassifiedCategoryId As Integer)
            MyBase.New(DB, ClassifiedCategoryId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal ClassifiedCategoryId As Integer) As ClassifiedCategoryRow
            Dim row As ClassifiedCategoryRow

            row = New ClassifiedCategoryRow(DB, ClassifiedCategoryId)
            row.Load()

            Return row
        End Function
        
        'end 24/10/2009
        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal ClassifiedCategoryId As Integer)
            Dim row As ClassifiedCategoryRow

            row = New ClassifiedCategoryRow(DB, ClassifiedCategoryId)
            row.Remove()
        End Sub
      
        'Custom Methods
        Public Shared Function GetAllClassifiedCategories(ByVal DB As Database) As DataSet
            Dim ds As DataSet = DB.GetDataSet("select * from ClassifiedCategory order by Category")
            Return ds
        End Function

    End Class

    Public MustInherit Class ClassifiedCategoryRowBase
        Private m_DB As Database
        Private m_ClassifiedCategoryId As Integer = Nothing
        Private m_Category As String = Nothing


        Public Property ClassifiedCategoryId() As Integer
            Get
                Return m_ClassifiedCategoryId
            End Get
            Set(ByVal Value As Integer)
                m_ClassifiedCategoryId = value
            End Set
        End Property

        Public Property Category() As String
            Get
                Return m_Category
            End Get
            Set(ByVal Value As String)
                m_Category = value
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

        Public Sub New(ByVal DB As Database, ByVal ClassifiedCategoryId As Integer)
            m_DB = DB
            m_ClassifiedCategoryId = ClassifiedCategoryId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT * FROM ClassifiedCategory WHERE ClassifiedCategoryId = " & DB.Number(ClassifiedCategoryId)
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
            m_ClassifiedCategoryId = Convert.ToInt32(r.Item("ClassifiedCategoryId"))
            If IsDBNull(r.Item("Category")) Then
                m_Category = Nothing
            Else
                m_Category = Convert.ToString(r.Item("Category"))
            End If
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String
            SQL = " INSERT INTO ClassifiedCategory (" _
             & " Category" _
             & ") VALUES (" _
             & m_DB.Quote(Category) _
             & ")"
            ClassifiedCategoryId = m_DB.InsertSQL(SQL)

            Return ClassifiedCategoryId
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE ClassifiedCategory SET " _
             & " Category = " & m_DB.Quote(Category) _
             & " WHERE ClassifiedCategoryId = " & m_DB.quote(ClassifiedCategoryId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM ClassifiedCategory WHERE ClassifiedCategoryId = " & m_DB.Quote(ClassifiedCategoryId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class ClassifiedCategoryCollection
        Inherits GenericCollection(Of ClassifiedCategoryRow)
    End Class

End Namespace


