Imports System
Imports Components
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Utility

Namespace DataLayer
    Public Class KeywordSearchRow
        Inherits KeywordSearchRowBase
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal id As Integer)
            MyBase.New(database, id)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal id As Int64) As KeywordSearchRow
            Dim row As KeywordSearchRow
            row = New KeywordSearchRow(_Database, id)
            row.Load()
            Return row
        End Function
        Private Shared Function GetDataFromReader(ByVal reader As SqlDataReader) As KeywordSearchRow
            Dim result As New KeywordSearchRow
            Try
                If (Not reader.IsDBNull(reader.GetOrdinal("KeywordSearchId"))) Then
                    result.KeywordSearchId = Convert.ToInt64(reader("KeywordSearchId"))
                Else
                    result.KeywordSearchId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("KeywordId"))) Then
                    result.KeywordId = reader("KeywordId").ToString()
                Else
                    result.KeywordId = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("SearchedDate"))) Then
                    result.SearchedDate = Convert.ToDateTime(reader("SearchedDate"))
                Else
                    result.SearchedDate = DateTime.MinValue
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("TotalAddCart"))) Then
                    result.TotalAddCart = Convert.ToInt64(reader("TotalAddCart"))
                Else
                    result.TotalAddCart = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("TotalDetail"))) Then
                    result.TotalDetail = Convert.ToInt64(reader("TotalDetail"))
                Else
                    result.TotalDetail = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("TotalSearch"))) Then
                    result.TotalSearch = Convert.ToInt64(reader("TotalSearch"))
                Else
                    result.TotalSearch = 0
                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return result
        End Function

        Public Shared Function GetDetailListForKeyword(ByVal KeywordId As Int64, ByVal OrderBy As String, ByVal OrderDirection As String, ByVal CurrentPage As Integer, ByVal PageSize As Integer, ByRef total As Integer) As DataTable
            If KeywordId < 1 Then
                Return Nothing
            End If
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim ds As New DataSet
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_KeywordSearch_GetDetailForKeyword")

                db.AddInParameter(cmd, "KeywordId", DbType.Int64, KeywordId)
                db.AddInParameter(cmd, "OrderBy", DbType.String, OrderBy)
                db.AddInParameter(cmd, "OrderDirection", DbType.String, OrderDirection)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, CurrentPage)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize)

                ds = db.ExecuteDataSet(cmd)
                total = ds.Tables(1).Rows(0)(0)
                Return ds.Tables(0)
            Catch ex As Exception
                'SendMailLog("sp_GetDetailForKeyword(ByVal ItemId As Int64)", ex)
                Core.LogError("KeywordAction.vb", "GetDetailListForKeyword(KeywordId=" & KeywordId & ")", ex)
            End Try
            Return Nothing
        End Function
        
    End Class

    Public MustInherit Class KeywordSearchRowBase
        Private m_DB As Database
        Private m_KeywordSearchId As Int64 = Nothing
        Private m_KeywordId As Int64 = Nothing
        Private m_SearchedDate As DateTime = Nothing
        Private m_TotalSearch As Int64 = Nothing
        Private m_TotalDetail As Int64 = Nothing
        Private m_TotalAddCart As Int64 = Nothing

        Public Property KeywordSearchId() As Long
            Get
                Return m_KeywordSearchId
            End Get
            Set(ByVal Value As Long)
                m_KeywordSearchId = Value
            End Set
        End Property
        Public Property KeywordId() As String
            Get
                Return m_KeywordId
            End Get
            Set(ByVal Value As String)
                m_KeywordId = Value
            End Set
        End Property
        Public Property SearchedDate() As DateTime
            Get
                Return m_SearchedDate
            End Get
            Set(ByVal Value As DateTime)
                m_SearchedDate = Value
            End Set
        End Property
        Public Property TotalSearch() As Int64
            Get
                Return m_TotalSearch
            End Get
            Set(ByVal Value As Int64)
                m_TotalSearch = Value
            End Set
        End Property
        Public Property TotalDetail() As Int64
            Get
                Return m_TotalDetail
            End Get
            Set(ByVal Value As Int64)
                m_TotalDetail = Value
            End Set
        End Property
        Public Property TotalAddCart() As Int64
            Get
                Return m_TotalAddCart
            End Get
            Set(ByVal Value As Int64)
                m_TotalAddCart = Value
            End Set
        End Property

        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal Id As Integer)
            m_DB = database
            m_KeywordSearchId = Id
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM KeywordSearch WHERE KeywordSearchId = " & m_DB.Number(KeywordSearchId)
                r = m_DB.GetReader(SQL)
                If Not r Is Nothing Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Core.LogError("KeywordSearch.vb", "Load", ex)
            End Try
        End Sub
        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("KeywordSearchId"))) Then
                        m_KeywordSearchId = Convert.ToInt64(reader("KeywordSearchId"))
                    Else
                        m_KeywordSearchId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("KeywordId"))) Then
                        m_KeywordId = Convert.ToInt64(reader("KeywordId"))
                    Else
                        m_KeywordId = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("SearchedDate"))) Then
                        m_SearchedDate = Convert.ToDateTime(reader("SearchedDate"))
                    Else
                        m_SearchedDate = DateTime.MinValue
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("TotalAddCart"))) Then
                        m_TotalAddCart = Convert.ToInt64(reader("TotalAddCart"))
                    Else
                        m_TotalAddCart = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("TotalDetail"))) Then
                        m_TotalDetail = Convert.ToInt64(reader("TotalDetail"))
                    Else
                        m_TotalDetail = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("TotalSearch"))) Then
                        m_TotalSearch = Convert.ToInt64(reader("TotalSearch"))
                    Else
                        m_TotalSearch = 0
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
    End Class

    Public Class KeywordSearchCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal data As KeywordSearchRowBase)
            Me.List.Add(data)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As KeywordSearchRowBase
            Get
                Return CType(Me.List.Item(Index), KeywordSearchRowBase)
            End Get

            Set(ByVal Value As KeywordSearchRowBase)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace

