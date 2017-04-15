Option Explicit On 

Imports System
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Database
Imports Components
Namespace DataLayer

    Public Class RegistryItemRow
        Inherits RegistryItemRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal RegistryItemId As Integer)
            MyBase.New(database, RegistryItemId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal RegistryItemId As Integer) As RegistryItemRow
            Dim row As RegistryItemRow

            row = New RegistryItemRow(_Database, RegistryItemId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal RegistryItemId As Integer)
            Dim row As RegistryItemRow

            row = New RegistryItemRow(_Database, RegistryItemId)
            row.Remove()
        End Sub

        'Custom Methods

    End Class

    Public MustInherit Class RegistryItemRowBase
        Private m_DB As Database
        Private m_RegistryItemId As Integer = Nothing
        Private m_RegistryId As Integer = Nothing
        Private m_ItemId As Integer = Nothing
        Private m_Quantity As Integer = Nothing
        Private m_Purchased As Integer = Nothing
        Private m_RegistryPrice As Double = Nothing

        Public Property RegistryItemId() As Integer
            Get
                Return m_RegistryItemId
            End Get
            Set(ByVal Value As Integer)
                m_RegistryItemId = value
            End Set
        End Property

        Public Property RegistryId() As Integer
            Get
                Return m_RegistryId
            End Get
            Set(ByVal Value As Integer)
                m_RegistryId = value
            End Set
        End Property

        Public Property ItemId() As Integer
            Get
                Return m_ItemId
            End Get
            Set(ByVal Value As Integer)
                m_ItemId = value
            End Set
        End Property

        Public Property Quantity() As Integer
            Get
                Return m_Quantity
            End Get
            Set(ByVal Value As Integer)
                m_Quantity = value
            End Set
        End Property

        Public Property Purchased() As Integer
            Get
                Return m_Purchased
            End Get
            Set(ByVal Value As Integer)
                m_Purchased = value
            End Set
        End Property

        Public Property RegistryPrice() As Double
            Get
                Return m_RegistryPrice
            End Get
            Set(ByVal Value As Double)
                m_RegistryPrice = Value
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

        Public Sub New(ByVal database As Database, ByVal RegistryItemId As Integer)
            m_DB = database
            m_RegistryItemId = RegistryItemId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM RegistryItem WHERE RegistryItemId = " & DB.Quote(RegistryItemId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Components.Core.CloseReader(r)
            Catch ex As Exception
                Components.Core.CloseReader(r)
            End Try
            
        End Sub


        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            m_RegistryId = Convert.ToInt32(r.Item("RegistryId"))
            m_ItemId = Convert.ToInt32(r.Item("ItemId"))
            m_Quantity = Convert.ToInt32(r.Item("Quantity"))
            m_Purchased = Convert.ToInt32(r.Item("Purchased"))
            If r.Item("RegistryPrice") Is Convert.DBNull Then
                m_RegistryPrice = Nothing
            Else
                m_RegistryPrice = Convert.ToDouble(r.Item("RegistryPrice"))
            End If
        End Sub 'Load

        Private ReadOnly Property InsertStatement() As String
            Get
                Dim SQL As String

                SQL = " INSERT INTO RegistryItem (" _
                 & " RegistryId" _
                 & ",ItemId" _
                 & ",Quantity" _
                 & ",Purchased" _
                 & ",RegistryPrice" _
                 & ") VALUES (" _
                 & DB.Quote(RegistryId) _
                 & "," & DB.Quote(ItemId) _
                 & "," & DB.Quote(Quantity) _
                 & "," & DB.Quote(Purchased) _
                 & "," & DB.NullQuote(RegistryPrice) _
                 & ")"

                Return SQL
            End Get
        End Property

        Public Overridable Sub Insert()
            m_DB.ExecuteSQL(InsertStatement)
        End Sub 'Insert

        Function AutoInsert() As Integer
            RegistryItemId = m_DB.InsertSQL(InsertStatement)
            Return RegistryItemId
        End Function


        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE RegistryItem SET " _
             & " RegistryId = " & DB.Quote(RegistryId) _
             & ",ItemId = " & DB.Quote(ItemId) _
             & ",Quantity = " & DB.Quote(Quantity) _
             & ",Purchased = " & DB.Quote(Purchased) _
             & ",RegistryPrice = " & DB.NullQuote(RegistryPrice) _
             & " WHERE RegistryItemId = " & DB.Quote(RegistryItemId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM RegistryItem WHERE RegistryItemId = " & DB.Quote(RegistryItemId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class RegistryItemCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal RegistryItem As RegistryItemRow)
            Me.List.Add(RegistryItem)
        End Sub

        Public Function Contains(ByVal RegistryItem As RegistryItemRow) As Boolean
            Return Me.List.Contains(RegistryItem)
        End Function

        Public Function IndexOf(ByVal RegistryItem As RegistryItemRow) As Integer
            Return Me.List.IndexOf(RegistryItem)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal RegistryItem As RegistryItemRow)
            Me.List.Insert(Index, RegistryItem)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As RegistryItemRow
            Get
                Return CType(Me.List.Item(Index), RegistryItemRow)
            End Get

            Set(ByVal Value As RegistryItemRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal RegistryItem As RegistryItemRow)
            Me.List.Remove(RegistryItem)
        End Sub
    End Class

End Namespace


