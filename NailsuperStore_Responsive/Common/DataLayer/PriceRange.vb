Option Explicit On

Imports System
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Components
Namespace DataLayer

    Public Class PriceRangeRow
        Inherits PriceRangeRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal RangeId As Integer)
            MyBase.New(database, RangeId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal RangeId As Integer) As PriceRangeRow
            Dim row As PriceRangeRow

            row = New PriceRangeRow(_Database, RangeId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal RangeId As Integer)
            Dim row As PriceRangeRow

            row = New PriceRangeRow(_Database, RangeId)
            row.Remove()
        End Sub

        'Custom Methods
        Public Shared Function GetPriceRanges(ByVal _DB As Database) As DataSet
            Dim SQL As String

            SQL = ""
            SQL &= " select distinct pr.* FROM StoreItem si, StoreItemFeature sif, ItemFeature ift, PriceRange pr where si.IsActive = 1 and si.Status not in ('C1', 'KC', 'J1')"
            SQL &= " and sif.ItemId = si.ItemId and sif.FeatureId = ift.FeatureId and ift.Code = 'GiftRegistry'"
            SQL &= " and si.Price between pr.[From] and pr.[To] and si.Price > 0"
            SQL &= " order by pr.sortorder"

            Dim ds As DataSet = _DB.GetDataSet(SQL)

            Return ds
        End Function

        Public Shared Function GetOccasions(ByVal _DB As Database) As DataSet
            Dim SQL As String

            SQL = ""
            SQL &= " select distinct o.occasionid, o.occasionname, o.sortorder FROM StoreItem si, StoreItemFeature sif, ItemFeature ift, occasion o, storeitemoccasion sio where si.IsActive = 1 and si.Status not in ('C1', 'KC', 'J1')"
            SQL &= " and sif.ItemId = si.ItemId and sif.FeatureId = ift.FeatureId and ift.Code = 'GiftRegistry'"
            SQL &= " and sio.occasionid = o.occasionid and sio.itemId = si.itemid and si.Price > 0"
            SQL &= " order by o.sortorder"

            Dim ds As DataSet = _DB.GetDataSet(SQL)
            Return ds
        End Function
    End Class

    Public MustInherit Class PriceRangeRowBase
        Private m_DB As Database
        Private m_RangeId As Integer = Nothing
        Private m_Name As String = Nothing
        Private m_From As Double = Nothing
        Private m_To As Double = Nothing
        Private m_SortOrder As Integer = Nothing


        Public Property RangeId() As Integer
            Get
                Return m_RangeId
            End Get
            Set(ByVal Value As Integer)
                m_RangeId = Value
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

        Public Property From() As Double
            Get
                Return m_From
            End Get
            Set(ByVal Value As Double)
                m_From = Value
            End Set
        End Property

        Public Property [To]() As Double
            Get
                Return m_To
            End Get
            Set(ByVal Value As Double)
                m_To = Value
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

        Public Sub New(ByVal database As Database, ByVal RangeId As Integer)
            m_DB = database
            m_RangeId = RangeId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM PriceRange WHERE RangeId = " & DB.Quote(RangeId)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            
        End Sub


        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            m_Name = Convert.ToString(r.Item("Name"))
            m_From = Convert.ToDouble(r.Item("From"))
            m_To = Convert.ToDouble(r.Item("To"))
            m_SortOrder = Convert.ToInt32(r.Item("SortOrder"))
        End Sub 'Load

        Public Overridable Sub Insert()
            Dim SQL As String

            SQL = " INSERT INTO PriceRange (" _
             & " Name" _
             & ",[From]" _
             & ",[To]" _
             & ",SortOrder" _
             & ") select " _
             & m_DB.Quote(Name) _
             & "," & m_DB.Quote(From) _
             & "," & m_DB.Quote([To]) _
             & ",coalesce((select Max(SortOrder) from PriceRange), 0) + 1"

            m_DB.ExecuteSQL(SQL)
        End Sub 'Insert

        Function AutoInsert() As Integer
            Dim SQL As String = "SELECT @@IDENTITY"
            Insert()
            RangeId = m_DB.ExecuteScalar(SQL)
            Return RangeId
        End Function


        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE PriceRange SET " _
             & " Name = " & m_DB.Quote(Name) _
             & ",[From] = " & m_DB.Quote(From) _
             & ",[To] = " & m_DB.Quote([To]) _
             & ",SortOrder = " & m_DB.Quote(SortOrder) _
             & " WHERE RangeId = " & m_DB.Quote(RangeId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM PriceRange WHERE RangeId = " & m_DB.Quote(RangeId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class PriceRangeCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal PriceRange As PriceRangeRow)
            Me.List.Add(PriceRange)
        End Sub

        Public Function Contains(ByVal PriceRange As PriceRangeRow) As Boolean
            Return Me.List.Contains(PriceRange)
        End Function

        Public Function IndexOf(ByVal PriceRange As PriceRangeRow) As Integer
            Return Me.List.IndexOf(PriceRange)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal PriceRange As PriceRangeRow)
            Me.List.Insert(Index, PriceRange)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As PriceRangeRow
            Get
                Return CType(Me.List.Item(Index), PriceRangeRow)
            End Get

            Set(ByVal Value As PriceRangeRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal PriceRange As PriceRangeRow)
            Me.List.Remove(PriceRange)
        End Sub
    End Class

End Namespace