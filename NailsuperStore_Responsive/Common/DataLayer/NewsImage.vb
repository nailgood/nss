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
    Public Class NewsImageRow
        Inherits NewsImageRowBase

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
        Public Shared Function GetRow(ByVal _Database As Database, ByVal id As Integer) As NewsImageRow
            Dim row As NewsImageRow
            Dim key As String = String.Format(cachePrefixKey & "GetRow_{0}", id)
            row = CType(CacheUtils.GetCache(key), NewsImageRow)
            If Not row Is Nothing Then
                Return row
            End If
            row = New NewsImageRow(_Database, id)
            row.Load()
            CacheUtils.SetCache(key, row)
            Return row
        End Function

        Public Shared Function ListAllByNewId(ByVal _Database As Database, ByVal data As NewsImageRow) As NewsImageCollection

            Dim ss As New NewsImageCollection
            Dim keyData As String = String.Format(cachePrefixKey & "ListAllByNewId_{0}_{1}_{2}_{3}_{4}", data.Condition, data.OrderBy, data.OrderDirection, data.PageIndex, data.PageSize)
            Dim keyTotal As String = cachePrefixKey & "ListAllByNewId_Total"
            ss = CType(CacheUtils.GetCache(keyData), NewsImageCollection)
            If Not ss Is Nothing Then
                ''get Total
                data.TotalRow = CType(CacheUtils.GetCache(keyTotal), Integer)
                Return ss
            Else
                ss = New NewsImageCollection
            End If
            Dim sp As String = "sp_Image_ListAllByNewId"
            Dim dr As SqlDataReader = Nothing
            Try
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Condition", SqlDbType.NVarChar, 0, data.Condition))
                cmd.Parameters.Add(_Database.InParam("OrderBy", SqlDbType.NVarChar, 0, data.OrderBy))
                cmd.Parameters.Add(_Database.InParam("OrderDirection", SqlDbType.VarChar, 0, data.OrderDirection))
                cmd.Parameters.Add(_Database.InParam("CurrentPage", SqlDbType.Int, 0, data.PageIndex))
                cmd.Parameters.Add(_Database.InParam("PageSize", SqlDbType.Int, 0, data.PageSize))
                cmd.Parameters.Add(_Database.OutParam("TotalRecords", SqlDbType.Int, 0))
                dr = cmd.ExecuteReader()
                While dr.Read
                    ss.Add(GetDataFromReader(dr))
                End While
                Core.CloseReader(dr)
                data.TotalRow = Convert.ToInt32(cmd.Parameters("TotalRecords").Value)
                CacheUtils.SetCache(keyData, ss)
                CacheUtils.SetCache(keyTotal, data.TotalRow)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try

            Return ss
        End Function
        Public Shared Function GetByNewId(ByVal _Database As Database, ByVal newsId As Integer) As NewsImageCollection
            Dim ss As New NewsImageCollection
            Dim key As String = cachePrefixKey & "GetByNewId_" & newsId
            ss = CType(CacheUtils.GetCache(key), NewsImageCollection)
            If Not ss Is Nothing Then
                Return ss
            Else
                ss = New NewsImageCollection
            End If
            Dim sp As String = "sp_Image_GetByByNewId"
            Dim dr As SqlDataReader = Nothing
            Try
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("NewsId", SqlDbType.Int, 0, newsId))
                dr = cmd.ExecuteReader()
                While dr.Read
                    ss.Add(GetDataFromReader(dr))
                End While
                Core.CloseReader(dr)
                CacheUtils.SetCache(key, ss)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try

            Return ss
        End Function

        Private Shared Function GetDataFromReader(ByVal reader As SqlDataReader) As NewsImageRow
            Dim result As New NewsImageRow
            If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                result.Id = Convert.ToInt32(reader("Id"))
            Else
                result.Id = 0
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("NewsId"))) Then
                result.NewsId = Convert.ToInt32(reader("NewsId"))
            Else
                result.NewsId = 0
            End If

            If (Not reader.IsDBNull(reader.GetOrdinal("ImageId"))) Then
                result.ImageId = Convert.ToInt32(reader("ImageId"))
            Else
                result.ImageId = 0
            End If

            If (Not reader.IsDBNull(reader.GetOrdinal("ImageName"))) Then
                result.ImageName = reader("ImageName").ToString()
            Else
                result.ImageName = ""
            End If

            If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                result.IsActive = Convert.ToBoolean(reader("IsActive"))
            Else
                result.IsActive = True
            End If
            If (Not reader.IsDBNull(reader.GetOrdinal("FileName"))) Then
                result.FileName = reader("FileName").ToString()
            Else
                result.FileName = ""
            End If
            Return result
        End Function

        Public Shared Function Delete(ByVal _Database As Database, ByVal Id As Integer) As Boolean

            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_NewsImage_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Id", SqlDbType.Int, 0, Id))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
                Return True
            End If
            Return False
        End Function

        Public Shared Function Insert(ByVal _Database As Database, ByVal data As NewsImageRow, ByVal clearCacheData As Boolean) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_NewsImage_Insert"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("NewsId", SqlDbType.Int, 0, data.NewsId))
                cmd.Parameters.Add(_Database.InParam("ImageId", SqlDbType.Int, 0, data.ImageId))
                cmd.Parameters.Add(_Database.InParam("IsActive", SqlDbType.Bit, 0, data.IsActive))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
            End Try
            If result = 1 Then
                If (clearCacheData) Then
                    CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
                End If
                Return True
            End If
            Return False
        End Function

        Public Shared Function ChangeIsActive(ByVal _Database As Database, ByVal Id As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_NewsImage_ChangeIsActive"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Id", SqlDbType.Int, 0, Id))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
                Return True
            End If
            Return False
        End Function
        Public Shared Function ChangeArrange(ByVal _Database As Database, ByVal Id As Integer, ByVal IsUp As Boolean) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_NewsImage_ChangeArrange"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Id", SqlDbType.Int, 0, Id))
                cmd.Parameters.Add(_Database.InParam("IsUp", SqlDbType.Bit, 0, IsUp))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey)
                Return True
            End If
            Return False
        End Function
    End Class


    Public MustInherit Class NewsImageRowBase
        Private m_DB As Database
        Private m_Id As Integer = Nothing
        Private m_NewsId As Integer = Nothing
        Private m_ImageId As Integer = Nothing
        Private m_ImageName As String = Nothing
        Private m_IsActive As Boolean = True
        Private m_FileName As String = Nothing
        Private m_TotalRow As Integer = Nothing
        Private m_PageIndex As Integer = Nothing
        Private m_PageSize As Integer = Nothing
        Private m_Condition As String = Nothing
        Private m_OrderBy As String = Nothing
        Private m_OrderDirection As String = Nothing
        Public Shared cachePrefixKey As String = "NewsImage_"
        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal Value As Integer)
                m_Id = Value
            End Set
        End Property
        Public Property NewsId() As Integer
            Get
                Return m_NewsId
            End Get
            Set(ByVal Value As Integer)
                m_NewsId = Value
            End Set
        End Property

        Public Property ImageId() As Integer
            Get
                Return m_ImageId
            End Get
            Set(ByVal Value As Integer)
                m_ImageId = Value
            End Set
        End Property
        Public Property ImageName() As String
            Get
                Return m_ImageName
            End Get
            Set(ByVal Value As String)
                m_ImageName = Value
            End Set
        End Property

        Public Property FileName() As String
            Get
                Return m_FileName
            End Get
            Set(ByVal Value As String)
                m_FileName = Value
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
        Public Property DB() As Database
            Get
                DB = m_DB
            End Get
            Set(ByVal Value As Database)
                m_DB = Value
            End Set
        End Property
        Public Property TotalRow() As Integer
            Get
                Return m_TotalRow
            End Get
            Set(ByVal Value As Integer)
                m_TotalRow = Value
            End Set
        End Property
        Public Property PageIndex() As Integer
            Get
                Return m_PageIndex
            End Get
            Set(ByVal Value As Integer)
                m_PageIndex = Value
            End Set
        End Property
        Public Property PageSize() As Integer
            Get
                Return m_PageSize
            End Get
            Set(ByVal Value As Integer)
                m_PageSize = Value
            End Set
        End Property

        Public Property OrderBy() As String
            Get
                Return m_OrderBy
            End Get
            Set(ByVal Value As String)
                m_OrderBy = Value
            End Set
        End Property
        Public Property OrderDirection() As String
            Get
                Return m_OrderDirection
            End Get
            Set(ByVal Value As String)
                m_OrderDirection = Value
            End Set
        End Property
        Public Property Condition() As String
            Get
                Return m_Condition
            End Get
            Set(ByVal Value As String)
                m_Condition = Value
            End Set
        End Property

        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal Id As Integer)
            m_DB = database
            m_Id = Id
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM NewsImage WHERE Id = " & DB.Number(Id)
                r = m_DB.GetReader(SQL)
                If r.Read Then
                    Me.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)

            If (Not reader Is Nothing And Not reader.IsClosed) Then

                If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                    m_Id = Convert.ToInt32(reader("Id"))
                Else
                    m_Id = 0
                End If


                If (Not reader.IsDBNull(reader.GetOrdinal("ImageId"))) Then
                    m_ImageId = Convert.ToInt32(reader("ImageId"))
                Else
                    m_ImageId = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("ImageName"))) Then
                    m_ImageName = reader("ImageName").ToString()
                Else
                    m_ImageName = ""
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                    m_IsActive = Convert.ToBoolean(reader("IsActive"))
                Else
                    m_IsActive = True
                End If


                If (Not reader.IsDBNull(reader.GetOrdinal("FileName"))) Then
                    m_FileName = reader("FileName").ToString()
                Else
                    m_FileName = ""
                End If

            End If
        End Sub

    End Class

    Public Class NewsImageCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal Image As NewsImageRow)
            Me.List.Add(Image)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As NewsImageRow
            Get
                Return CType(Me.List.Item(Index), NewsImageRow)
            End Get

            Set(ByVal Value As NewsImageRow)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace


