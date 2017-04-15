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
    Public Class KeywordItemRow
        Inherits KeywordItemRowBase
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
        Public Shared Function GetRow(ByVal _Database As Database, ByVal id As Int64) As KeywordItemRow
            Dim row As KeywordItemRow
            row = New KeywordItemRow(_Database, id)
            row.Load()
            Return row
        End Function
        Private Shared Function GetDataFromReader(ByVal reader As SqlDataReader) As KeywordItemRow
            Dim result As New KeywordItemRow
            Try
                If (Not reader.IsDBNull(reader.GetOrdinal("KeywordId"))) Then
                    result.KeywordId = reader("KeywordId").ToString()
                Else
                    result.KeywordId = ""
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
                If (Not reader.IsDBNull(reader.GetOrdinal("TotalPoint"))) Then
                    result.TotalPoint = Convert.ToInt64(reader("TotalPoint"))
                Else
                    result.TotalPoint = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("SKU"))) Then
                    result.SKU = reader("SKU").ToString()
                Else
                    result.SKU = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ItemName"))) Then
                    result.ItemName = reader("ItemName").ToString()
                Else
                    result.ItemName = ""
                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return result
        End Function

        Public Shared Function GetItemForKeyword(ByVal KeywordId As Int64, ByVal OrderBy As String, ByVal OrderDirection As String, ByVal CurrentPage As Integer, ByVal PageSize As Integer, ByRef total As Integer) As DataTable
            If KeywordId < 1 Then
                Return Nothing
            End If
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim ds As New DataSet
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_KeywordItem_GetItemForKeyword")

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
                Core.LogError("KeywordAction.vb", "GetItemForKeyword(KeywordId=" & KeywordId & ")", ex)
            End Try
            Return Nothing
        End Function
      
    End Class

    Public MustInherit Class KeywordItemRowBase
        Private m_DB As Database
        Private m_KeywordId As Int64 = Nothing
        Private m_ItemId As Integer = Nothing
        Private m_TotalPoint As Int64 = Nothing
        Private m_TotalDetail As Int64 = Nothing
        Private m_TotalAddCart As Int64 = Nothing
        Private m_SKU As String = Nothing
        Private m_ItemName As String = Nothing

        Public Property KeywordId() As String
            Get
                Return m_KeywordId
            End Get
            Set(ByVal Value As String)
                m_KeywordId = Value
            End Set
        End Property
        Public Property TotalPoint() As Int64
            Get
                Return m_TotalPoint
            End Get
            Set(ByVal Value As Int64)
                m_TotalPoint = Value
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
        Public Property SKU() As String
            Get
                Return m_SKU
            End Get
            Set(ByVal Value As String)
                m_SKU = Value
            End Set
        End Property
        Public Property ItemName() As String
            Get
                Return m_ItemName
            End Get
            Set(ByVal Value As String)
                m_ItemName = Value
            End Set
        End Property

        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal Id As Integer)
            m_DB = database
            m_KeywordId = Id
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM KeywordItem WHERE KeywordId = " & m_DB.Number(KeywordId)
                r = m_DB.GetReader(SQL)
                If Not r Is Nothing Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Core.LogError("KeywordItem.vb", "Load", ex)
            End Try
        End Sub
        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("KeywordId"))) Then
                        m_KeywordId = Convert.ToInt64(reader("KeywordId"))
                    Else
                        m_KeywordId = ""
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
                    If (Not reader.IsDBNull(reader.GetOrdinal("TotalPoint"))) Then
                        m_TotalPoint = Convert.ToInt64(reader("TotalPoint"))
                    Else
                        m_TotalPoint = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("SKU"))) Then
                        m_SKU = Convert.ToInt64(reader("SKU"))
                    Else
                        m_SKU = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ItemName"))) Then
                        m_ItemName = Convert.ToInt64(reader("ItemName"))
                    Else
                        m_ItemName = 0
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
    End Class

    Public Class KeywordItemCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal data As KeywordItemRowBase)
            Me.List.Add(data)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As KeywordItemRowBase
            Get
                Return CType(Me.List.Item(Index), KeywordItemRowBase)
            End Get

            Set(ByVal Value As KeywordItemRowBase)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace


