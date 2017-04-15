
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
    Public Class ImageRow
        Inherits ImageRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal ImageId As Integer)
            MyBase.New(database, ImageId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal ImageId As Integer) As ImageRow
            Dim row As ImageRow
            row = New ImageRow(_Database, ImageId)
            row.Load()
            Return row
        End Function

        Public Shared Function ListAll(ByVal _Database As Database) As ImageCollection
            Dim ss As New ImageCollection
            Dim keyData As String = cachePrefixKey & "ListAll"
            ss = CType(CacheUtils.GetCache(keyData), ImageCollection)
            If Not ss Is Nothing Then
                Return ss
            Else
                ss = New ImageCollection
            End If
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_Image_ListAll"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    ss.Add(GetDataFromReader(dr))
                End While
                Core.CloseReader(dr)
                CacheUtils.SetCache(keyData, ss)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
            Return ss
        End Function
        Private Shared Function GetDataFromReader(ByVal reader As SqlDataReader) As ImageRow
            Dim result As New ImageRow
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

        Public Shared Function Delete(ByVal _Database As Database, ByVal ImageId As Integer) As Boolean

            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Image_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("ImageId", SqlDbType.Int, 0, ImageId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey, NewsImageRow.cachePrefixKey)
                Return True
            End If
            Return False
        End Function

        Public Shared Function Insert(ByVal _Database As Database, ByVal data As ImageRow) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Image_Insert"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("ImageName", SqlDbType.NVarChar, 0, data.ImageName))
                cmd.Parameters.Add(_Database.InParam("IsActive", SqlDbType.Bit, 0, data.IsActive))
                cmd.Parameters.Add(_Database.InParam("FileName", SqlDbType.NVarChar, 0, data.FileName))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
            End Try
            If result = 1 Then
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey, NewsImageRow.cachePrefixKey)
                Return True
            End If
            Return False
        End Function
        Public Shared Function Update(ByVal _Database As Database, ByVal data As ImageRow) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Image_Update"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("ImageId", SqlDbType.NVarChar, 0, data.ImageId))
                cmd.Parameters.Add(_Database.InParam("ImageName", SqlDbType.NVarChar, 0, data.ImageName))
                cmd.Parameters.Add(_Database.InParam("IsActive", SqlDbType.Bit, 0, data.IsActive))
                cmd.Parameters.Add(_Database.InParam("FileName", SqlDbType.NVarChar, 0, data.FileName))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
               CacheUtils.ClearCacheWithPrefix(cachePrefixKey, NewsImageRow.cachePrefixKey)
                Return True
            End If
            Return False
        End Function
        Public Shared Function ChangeIsActive(ByVal _Database As Database, ByVal ImageId As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Image_ChangeIsActive"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("ImageId", SqlDbType.Int, 0, ImageId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
                 CacheUtils.ClearCacheWithPrefix(cachePrefixKey, NewsImageRow.cachePrefixKey)
                Return True
            End If
            Return False
        End Function
    End Class


    Public MustInherit Class ImageRowBase
        Private m_DB As Database
        Private m_ImageId As Integer = Nothing
        Private m_ImageName As String = Nothing
        Private m_IsActive As Boolean = True
        Private m_FileName As String = Nothing
        Public Shared cachePrefixKey As String = "Image_"

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

        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal ImageId As Integer)
            m_DB = database
            m_ImageId = ImageId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String

                SQL = "SELECT * FROM Image WHERE ImageId = " & DB.Number(ImageId)
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

    Public Class ImageCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal Image As ImageRow)
            Me.List.Add(Image)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As ImageRow
            Get
                Return CType(Me.List.Item(Index), ImageRow)
            End Get

            Set(ByVal Value As ImageRow)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace


