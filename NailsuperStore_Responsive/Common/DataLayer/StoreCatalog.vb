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
    Public Class StoreCatalogRow
        Inherits StoreCatalogRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal CatalogId As Integer)
            MyBase.New(database, CatalogId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal CatalogId As Integer) As StoreCatalogRow
            Dim row As StoreCatalogRow

            row = New StoreCatalogRow(_Database, CatalogId)
            row.Load()

            Return row
        End Function
       
        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal CatalogId As Integer)
            Dim row As StoreCatalogRow

            row = New StoreCatalogRow(_Database, CatalogId)
            row.Remove()
        End Sub
       
        'Custom Methods
    End Class

    Public MustInherit Class StoreCatalogRowBase
        Private m_DB As Database
        Private m_CatalogId As Integer = Nothing
        Private m_Title As String = Nothing
        Private m_CatalogImage As String = Nothing
        Private m_ImageAltTag As String = Nothing
        Private m_RichFXLink As String = Nothing
        Private m_IsActive As Boolean = Nothing
        Private m_SortOrder As Integer = Nothing


        Public Property CatalogId() As Integer
            Get
                Return m_CatalogId
            End Get
            Set(ByVal Value As Integer)
                m_CatalogId = value
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

        Public Property CatalogImage() As String
            Get
                Return m_CatalogImage
            End Get
            Set(ByVal Value As String)
                m_CatalogImage = value
            End Set
        End Property

        Public Property ImageAltTag() As String
            Get
                Return m_ImageAltTag
            End Get
            Set(ByVal Value As String)
                m_ImageAltTag = Value
            End Set
        End Property

        Public Property RichFXLink() As String
            Get
                Return m_RichFXLink
            End Get
            Set(ByVal Value As String)
                m_RichFXLink = Value
            End Set
        End Property

        Public Property IsActive() As Boolean
            Get
                Return m_IsActive
            End Get
            Set(ByVal Value As Boolean)
                m_IsActive = Value
            End Set
        End Property

        Public Property SortOrder() As Integer
            Get
                Return m_SortOrder
            End Get
            Set(ByVal Value As Integer)
                m_SortOrder = Value
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

        Public Sub New(ByVal database As Database, ByVal CatalogId As Integer)
            m_DB = database
            m_CatalogId = CatalogId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM StoreCatalog WHERE CatalogId = " & DB.Quote(CatalogId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
        End Sub

        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                m_Title = Convert.ToString(r.Item("Title"))
                m_CatalogImage = Convert.ToString(r.Item("CatalogImage"))
                m_RichFXLink = Convert.ToString(r.Item("RichFXLink"))
                If r.Item("ImageAltTag") Is Convert.DBNull Then
                    m_ImageAltTag = Nothing
                Else
                    m_ImageAltTag = Convert.ToString(r.Item("ImageAltTag"))
                End If
                m_IsActive = Convert.ToBoolean(r.Item("IsActive"))
                m_SortOrder = Convert.ToInt32(r.Item("SortOrder"))
            Catch ex As Exception
                Throw ex

            End Try

        End Sub 'Load

        Private ReadOnly Property InsertStatement() As String
            Get
                Dim SQL As String

                SQL = " INSERT INTO StoreCatalog (" _
                 & " Title" _
                 & ",CatalogImage" _
                 & ",ImageAltTag" _
                 & ",RichFXLink" _
                 & ",IsActive" _
                 & ",SortOrder" _
                 & ") VALUES (" _
                 & m_DB.Quote(Title) _
                 & "," & m_DB.Quote(CatalogImage) _
                 & "," & m_DB.Quote(ImageAltTag) _
                 & "," & m_DB.Quote(RichFXLink) _
                 & "," & CInt(IsActive) _
                 & "," & m_DB.Quote(SortOrder) _
                 & ")"

                Return SQL
            End Get
        End Property

        Public Overridable Sub Insert()
            m_DB.ExecuteSQL(InsertStatement)
        End Sub 'Insert

        Function AutoInsert() As Integer
            CatalogId = m_DB.InsertSQL(InsertStatement)
            Return CatalogId
        End Function


        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE StoreCatalog SET " _
             & " Title = " & m_DB.Quote(Title) _
             & ",CatalogImage = " & m_DB.Quote(CatalogImage) _
             & ",RichFXLink = " & m_DB.Quote(RichFXLink) _
             & ",ImageAltTag = " & m_DB.Quote(ImageAltTag) _
             & ",IsActive = " & CInt(IsActive) _
             & ",SortOrder = " & m_DB.Quote(SortOrder) _
             & " WHERE CatalogId = " & m_DB.Quote(CatalogId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM StoreCatalog WHERE CatalogId = " & m_DB.Quote(CatalogId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class StoreCatalogCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal StoreCatalog As StoreCatalogRow)
            Me.List.Add(StoreCatalog)
        End Sub

        Public Function Contains(ByVal StoreCatalog As StoreCatalogRow) As Boolean
            Return Me.List.Contains(StoreCatalog)
        End Function

        Public Function IndexOf(ByVal StoreCatalog As StoreCatalogRow) As Integer
            Return Me.List.IndexOf(StoreCatalog)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal StoreCatalog As StoreCatalogRow)
            Me.List.Insert(Index, StoreCatalog)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As StoreCatalogRow
            Get
                Return CType(Me.List.Item(Index), StoreCatalogRow)
            End Get

            Set(ByVal Value As StoreCatalogRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal StoreCatalog As StoreCatalogRow)
            Me.List.Remove(StoreCatalog)
        End Sub
    End Class

End Namespace