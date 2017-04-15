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
    Public Class ContentToolPageRegionControlRow
        Inherits ContentToolPageRegionControlRowBase
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(ByVal DB As Database, ByVal PageRegionControlId As Integer)
            MyBase.New(DB, PageRegionControlId)
        End Sub

        Public Shared Function GetRow(ByVal DB As Database, ByVal PageRegionControlId As Integer) As ContentToolPageRegionControlRow
            Dim row As New ContentToolPageRegionControlRow
            row.Load()
            Return row
        End Function

        Public Shared Function LoadListByPageId(ByVal pageId As String, ByVal region As String) As ContentToolPageRegionControlCollection
            Dim ss As New ContentToolPageRegionControlCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_ContentToolPageRegionControl_ListControlByPageId"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "PageId", DbType.String, pageId)
                db.AddInParameter(cmd, "Region", DbType.String, region)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    ss.Add(GetDataFromReader(dr))
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "ContentToolControl.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return ss
        End Function

        Private Shared Function GetDataFromReader(ByVal reader As SqlDataReader) As ContentToolPageRegionControlRow
            Dim result As New ContentToolPageRegionControlRow
            Try
                If (Not reader.IsDBNull(reader.GetOrdinal("PageRegionControlId"))) Then
                    result.PageRegionControlId = Convert.ToInt32(reader("PageRegionControlId"))
                Else
                    result.PageRegionControlId = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("Name"))) Then
                    result.Name = reader("Name").ToString()
                Else
                    result.Name = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Param"))) Then
                    result.Param = reader("Param").ToString()
                Else
                    result.Param = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("URL"))) Then
                    result.URL = reader("URL").ToString()
                Else
                    result.URL = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                    result.IsActive = reader("IsActive")
                Else
                    result.IsActive = False
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("SortOrder"))) Then
                    result.SortOrder = CInt(reader("SortOrder"))
                Else
                    result.SortOrder = ""
                End If
                Return result
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Shared Function ChangeActive(ByVal PageRegionControlId As Integer, ByVal pageUrl As String, ByVal region As String) As Boolean
            Dim result As Integer = 0
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_ContentToolPageRegionControl_ChangeActive"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "PageRegionControlId", DbType.Int32, PageRegionControlId)
                result = CInt(db.ExecuteNonQuery(cmd))
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey & "LoadListByRegionPage_" & pageUrl & "_" & region)
            Catch ex As Exception

            End Try
            If result > 0 Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function ChangeIsActive(ByVal PageRegionControlId As Integer, ByVal IsActive As Boolean, ByVal pageUrl As String, ByVal region As String) As Boolean
            Dim result As Integer = 0
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_ContentToolPageRegionControl_ChangeIsActive"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "PageRegionControlId", DbType.Int32, PageRegionControlId)
                db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
                result = CInt(db.ExecuteNonQuery(cmd))
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey & "LoadListByRegionPage_" & pageUrl & "_" & region)
            Catch ex As Exception

            End Try
            If result > 0 Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function Delete(ByVal PageRegionControlId As Integer, ByVal pageUrl As String, ByVal region As String) As Boolean
            Dim result As Integer = 0
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_ContentToolPageRegionControl_Delete"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "PageRegionControlId", DbType.Int32, PageRegionControlId)
                result = CInt(db.ExecuteNonQuery(cmd))
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey & "LoadListByRegionPage_" & pageUrl & "_" & region)
            Catch ex As Exception

            End Try
            If result > 0 Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function ChangeSortOrder(ByVal PageRegionControlId As Integer, ByVal IsUp As Boolean, ByVal pageUrl As String, ByVal region As String) As Boolean
            Dim result As Integer = 0
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_ContentToolPageRegionControl_ChangeSortOrder"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "PageRegionControlId", DbType.Int32, PageRegionControlId)
                db.AddInParameter(cmd, "IsUp", DbType.Boolean, IsUp)
                result = CInt(db.ExecuteNonQuery(cmd))
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey & "LoadListByRegionPage_" & pageUrl & "_" & region)
            Catch ex As Exception

            End Try
            If result > 0 Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function Insert(ByVal item As ContentToolPageRegionControlRow, ByVal pageUrl As String, ByVal region As String) As Boolean
            Dim result As Integer = 0
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_ContentToolPageRegionControl_Insert"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "ControlId", DbType.Int32, item.ControlId)
                db.AddInParameter(cmd, "PageId", DbType.Int32, item.PageId)
                db.AddInParameter(cmd, "Region", DbType.String, item.Region)
                db.AddInParameter(cmd, "Param", DbType.String, item.Param)
                db.AddInParameter(cmd, "IsActive", DbType.Boolean, item.IsActive)
                result = CInt(db.ExecuteNonQuery(cmd))
                CacheUtils.ClearCacheWithPrefix(cachePrefixKey & "LoadListByRegionPage_" & pageUrl & "_" & region)
            Catch ex As Exception

            End Try
            If result > 0 Then
                Return True
            End If
            Return False
        End Function

    End Class

    Public Class ContentToolPageRegionControlRowBase
        Private m_DB As Database
        Private m_PageRegionControlId As Integer = Nothing
        Private m_PageId As Integer = Nothing
        Private m_Region As String = Nothing
        Private m_ControlId As Integer = Nothing
        Private m_SortOrder As Integer = Nothing
        Private m_Param As String = Nothing
        Private m_IsActive As Boolean = False
        Private m_Name As String = Nothing
        Private m_URL As String = Nothing
        Public Shared cachePrefixKey As String = "ContentToolControl_"

        Public Property DB() As Database
            Get
                Return m_DB
            End Get
            Set(ByVal Value As Database)
                m_DB = Value
            End Set
        End Property
        Public Property PageRegionControlId() As Integer
            Get
                Return m_PageRegionControlId
            End Get
            Set(ByVal value As Integer)
                m_PageRegionControlId = value
            End Set
        End Property
        Public Property PageId() As Integer
            Get
                Return m_PageId
            End Get
            Set(ByVal value As Integer)
                m_PageId = value
            End Set
        End Property
        Public Property ControlId() As Integer
            Get
                Return m_ControlId
            End Get
            Set(ByVal value As Integer)
                m_ControlId = value
            End Set
        End Property
        Public Property SortOrder() As Integer
            Get
                Return m_SortOrder
            End Get
            Set(ByVal value As Integer)
                m_SortOrder = value
            End Set
        End Property
        Public Property Region() As String
            Get
                Return m_Region
            End Get
            Set(ByVal value As String)
                m_Region = value
            End Set
        End Property
        Public Property Param() As String
            Get
                Return m_Param
            End Get
            Set(ByVal value As String)
                m_Param = value
            End Set
        End Property
        Public Property IsActive() As Boolean
            Get
                Return m_IsActive
            End Get
            Set(ByVal value As Boolean)
                m_IsActive = value
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
        Public Property URL() As String
            Get
                Return m_URL
            End Get
            Set(ByVal Value As String)
                m_URL = Value
            End Set
        End Property

        Public Sub New()
        End Sub
        Public Sub New(ByVal DB As Database, ByVal PageRegionControlId As Integer)
            m_DB = m_DB
            m_PageRegionControlId = PageRegionControlId
        End Sub
        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM ContentToolPageRegionControl WHERE PageRegionControlId = " & DB.Number(PageRegionControlId)
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
            Try
                If (Not reader Is Nothing AndAlso Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("PageRegionControlId"))) Then
                        m_PageRegionControlId = Convert.ToInt32(reader("PageRegionControlId"))
                    Else
                        m_PageRegionControlId = 0
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("PageId"))) Then
                        m_PageId = reader("PageId").ToString()
                    Else
                        m_PageId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Region"))) Then
                        m_Region = reader("Region").ToString()
                    Else
                        m_Region = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ControlId"))) Then
                        m_ControlId = reader("ControlId").ToString()
                    Else
                        m_ControlId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("SortOrder"))) Then
                        m_SortOrder = reader("SortOrder").ToString()
                    Else
                        m_SortOrder = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Param"))) Then
                        m_Param = reader("Param").ToString()
                    Else
                        m_Param = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                        m_IsActive = reader("IsActive").ToString()
                    Else
                        m_IsActive = 0
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
    End Class

    Public Class ContentToolPageRegionControlCollection
        Inherits CollectionBase
        Public Sub New()
        End Sub
        Public Sub Add(ByVal Category As ContentToolPageRegionControlRowBase)
            Me.List.Add(Category)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As ContentToolPageRegionControlRow
            Get
                Return CType(Me.List.Item(Index), ContentToolPageRegionControlRow)
            End Get

            Set(ByVal Value As ContentToolPageRegionControlRow)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace


