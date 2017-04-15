Option Explicit On

Imports System
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components
Namespace DataLayer

    Public Class ItemFeatureRow
        Inherits ItemFeatureRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal FeatureId As Integer)
            MyBase.New(database, FeatureId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal FeatureId As Integer) As ItemFeatureRow
            Dim row As ItemFeatureRow

            row = New ItemFeatureRow(_Database, FeatureId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal FeatureId As Integer)
            Dim row As ItemFeatureRow

            row = New ItemFeatureRow(_Database, FeatureId)
            row.Remove()
        End Sub

        'Custom Methods
        Public Shared Sub RemoveByItem(ByVal _Database As Database, ByVal ItemId As Integer)
            Dim SQL As String = "delete from StoreItemFeature where ItemId = " & _Database.Quote(ItemId.ToString)
            _Database.ExecuteSQL(SQL)
        End Sub

    End Class

    Public MustInherit Class ItemFeatureRowBase
        Private m_DB As Database
        Private m_FeatureId As Integer = Nothing
        Private m_Code As String = Nothing
        Private m_Name As String = Nothing
        Private m_SortOrder As Integer = Nothing


        Public Property FeatureId() As Integer
            Get
                Return m_FeatureId
            End Get
            Set(ByVal Value As Integer)
                m_FeatureId = Value
            End Set
        End Property

        Public Property Code() As String
            Get
                Return m_Code
            End Get
            Set(ByVal Value As String)
                m_Code = Value
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

        Public Sub New(ByVal database As Database, ByVal FeatureId As Integer)
            m_DB = database
            m_FeatureId = FeatureId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM ItemFeature WHERE FeatureId = " & DB.Quote(FeatureId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            
        End Sub

        Public Overridable Sub Load(ByVal r As SqlDataReader)
            m_FeatureId = Convert.ToString(r.Item("FeatureId"))
            m_Code = Convert.ToString(r.Item("Code"))
            m_Name = Convert.ToString(r.Item("Name"))
            m_SortOrder = Convert.ToInt32(r.Item("SortOrder"))
        End Sub 'Load

        Public Overridable Sub Insert()
            Dim SQL As String

            SQL = " INSERT INTO ItemFeature (" _
             & " Code" _
             & ",Name" _
             & ",SortOrder" _
             & ") VALUES (" _
             & m_DB.Quote(Code) _
             & "," & m_DB.Quote(Name) _
             & "," & m_DB.Quote(SortOrder) _
             & ")"

            m_DB.ExecuteSQL(SQL)
        End Sub 'Insert

        Function AutoInsert() As Integer
            Dim SQL As String = "SELECT @@IDENTITY"
            Insert()
            FeatureId = m_DB.ExecuteScalar(SQL)
            Return FeatureId
        End Function


        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE ItemFeature SET " _
             & " Code = " & m_DB.Quote(Code) _
             & ",Name = " & m_DB.Quote(Name) _
             & ",SortOrder = " & m_DB.Quote(SortOrder) _
             & " WHERE FeatureId = " & m_DB.Quote(FeatureId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM ItemFeature WHERE FeatureId = " & m_DB.Quote(FeatureId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class ItemFeatureCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal ItemFeature As ItemFeatureRow)
            Me.List.Add(ItemFeature)
        End Sub

        Public Function Contains(ByVal ItemFeature As ItemFeatureRow) As Boolean
            Return Me.List.Contains(ItemFeature)
        End Function

        Public Function IndexOf(ByVal ItemFeature As ItemFeatureRow) As Integer
            Return Me.List.IndexOf(ItemFeature)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal ItemFeature As ItemFeatureRow)
            Me.List.Insert(Index, ItemFeature)
        End Sub

        Public ReadOnly Property Exists(ByVal s As String) As Boolean
            Get
                Dim row As ItemFeatureRow
                For i As Integer = 0 To Me.List.Count - 1
                    row = Item(i)
                    If row.Code = s Then
                        Return True
                    End If
                Next
                Return False
            End Get
        End Property

        Public Property Item(ByVal Index As Integer) As ItemFeatureRow
            Get
                Return CType(Me.List.Item(Index), ItemFeatureRow)
            End Get

            Set(ByVal Value As ItemFeatureRow)
                Me.List(Index) = Value
            End Set
        End Property


        Public Sub Remove(ByVal ItemFeature As ItemFeatureRow)
            Me.List.Remove(ItemFeature)
        End Sub
    End Class

End Namespace
