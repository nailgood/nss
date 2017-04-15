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

    Public Class StoreItemCategoryRow
        Inherits StoreItemCategoryRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal id As Integer)
            MyBase.New(DB, id)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal Category As String)
            MyBase.New(DB, Category)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal id As Integer) As StoreItemCategoryRow
            Dim row As StoreItemCategoryRow

            row = New StoreItemCategoryRow(DB, id)
            row.Load()

            Return row
        End Function

        Public Shared Function GetRow(ByVal DB As Database, ByVal Category As String) As StoreItemCategoryRow
            Dim row As StoreItemCategoryRow

            row = New StoreItemCategoryRow(DB, Category)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal id As Integer)
            Dim row As StoreItemCategoryRow

            row = New StoreItemCategoryRow(DB, id)
            row.Remove()
        End Sub
       
        'Custom Methods
        Public Sub RemoveDepartmentItems()
            Dim SQL As String = "delete from StoreItemCategoryDepartment where CategoryId = " & DB.Quote(id)
            DB.ExecuteSQL(SQL)
        End Sub

        Public Sub InsertDepartmentItems(ByVal DepartmentListSeparatedByComma As String)
            If DepartmentListSeparatedByComma = String.Empty Then
                Exit Sub
            End If
            Dim SQL As String = "INSERT INTO StoreItemCategoryDepartment (CategoryId, DepartmentId) Select " & DB.Quote(id) & ", DepartmentId FROM StoreDepartment WHERE DepartmentId IN " & DB.NumberMultiple(DepartmentListSeparatedByComma)
            DB.ExecuteSQL(SQL)
        End Sub

        Public Sub CopyFromNavision(ByVal r As NavisionItemCategoryRow)
            Category = Trim(r.Category)
            Name = Trim(r.Name)

            If id = Nothing Then
                Insert()
            Else
                Update()
            End If
        End Sub
    End Class

    Public MustInherit Class StoreItemCategoryRowBase
        Private m_DB As Database
        Private m_id As Integer = Nothing
        Private m_Category As String = Nothing
        Private m_Name As String = Nothing

        Public Property id() As Integer
            Get
                Return m_id
            End Get
            Set(ByVal Value As Integer)
                m_id = Value
            End Set
        End Property

        Public Property Category() As String
            Get
                Return m_Category
            End Get
            Set(ByVal Value As String)
                m_Category = Value
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

        Public Sub New(ByVal DB As Database, ByVal id As Integer)
            m_DB = DB
            m_id = id
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal Code As String)
            m_DB = DB
            m_Category = Code
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM StoreItemCategory WHERE " & IIf(id <> Nothing, "id = " & DB.Number(id), "category = " & m_DB.Quote(Category))
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            

        End Sub


        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                m_id = Convert.ToInt32(r.Item("id"))
                m_Category = Convert.ToString(r.Item("Category"))
                If IsDBNull(r.Item("Name")) Then
                    m_Name = Nothing
                Else
                    m_Name = Convert.ToString(r.Item("Name"))
                End If
            Catch ex As Exception
                Throw ex
                '' Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String


            SQL = " INSERT INTO StoreItemCategory (" _
             & " Category" _
             & ",Name" _
             & ") VALUES (" _
             & m_DB.Quote(Category) _
             & "," & m_DB.Quote(Name) _
             & ")"

            id = m_DB.InsertSQL(SQL)

            Return id
        End Function

        Public Overridable Sub Update()
            Dim SQL As String
            SQL = " UPDATE StoreItemCategory SET " _
             & " Category = " & m_DB.Quote(Category) _
             & ",Name = " & m_DB.Quote(Name) _
             & " WHERE id = " & m_DB.Quote(id)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM StoreItemCategory WHERE id = " & m_DB.Quote(id)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class StoreItemCategoryCollection
        Inherits GenericCollection(Of StoreItemCategoryRow)
    End Class

End Namespace


