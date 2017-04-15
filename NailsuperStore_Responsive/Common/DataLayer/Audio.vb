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
    Public Class AudioRow
        Inherits AudioRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal ID As Integer)
            MyBase.New(database, ID)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal ID As Integer) As AudioRow
            Dim row As AudioRow
            row = New AudioRow(_Database, ID)
            row.Load()
            Return row
        End Function
       

        Public Shared Function Delete(ByVal _Database As Database, ByVal ImageId As Integer) As Boolean

            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Audio_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("AudioId", SqlDbType.Int, 0, ImageId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
                'CacheUtils.RemoveCacheWithPrefix(cachePrefixKey)
                'CacheUtils.RemoveCacheWithPrefix(NewsAudioRow.cachePrefixKey)
                Return True
            End If
            Return False
        End Function

        Public Shared Function Insert(ByVal _Database As Database, ByVal data As AudioRow) As Integer
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Audio_Insert"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Title", SqlDbType.NVarChar, 0, data.Title))
                cmd.Parameters.Add(_Database.InParam("IsActive", SqlDbType.Bit, 0, data.IsActive))
                cmd.Parameters.Add(_Database.InParam("FileUrl", SqlDbType.NVarChar, 0, data.FileUrl))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
            End Try
            Return result
        End Function

    End Class


    Public MustInherit Class AudioRowBase
        Private m_DB As Database
        Private m_AudioId As Integer = Nothing
        Private m_Title As String = Nothing
        Private m_IsActive As Boolean = True
        Private m_FileUrl As String = Nothing
        Public Shared cachePrefixKey As String = "Audio_"

        Public Property AudioId() As Integer
            Get
                Return m_AudioId
            End Get
            Set(ByVal Value As Integer)
                m_AudioId = Value
            End Set
        End Property
        Public Property Title() As String
            Get
                Return m_Title
            End Get
            Set(ByVal Value As String)
                m_Title = Value
            End Set
        End Property

        Public Property FileUrl() As String
            Get
                Return m_FileUrl
            End Get
            Set(ByVal Value As String)
                m_FileUrl = Value
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

        Public Sub New(ByVal database As Database, ByVal Id As Integer)
            m_DB = database
            m_AudioId = Id
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM Audio WHERE AudioId = " & m_DB.Number(AudioId)
                r = m_DB.GetReader(SQL)
                If Not r Is Nothing Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Email.SendError("ToError500", "Audio.vb", "Error: " & ex.Message & "<br><br>Stack trace: " & ex.StackTrace & "<br><br>URL: " & System.Web.HttpContext.Current.Request.Url.ToString())
            End Try

        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("AudioId"))) Then
                        m_AudioId = Convert.ToInt32(reader("AudioId"))
                    Else
                        m_AudioId = 0
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("Title"))) Then
                        m_Title = reader("Title").ToString()
                    Else
                        m_Title = ""
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                        m_IsActive = Convert.ToBoolean(reader("IsActive"))
                    Else
                        m_IsActive = True
                    End If


                    If (Not reader.IsDBNull(reader.GetOrdinal("FileUrl"))) Then
                        m_FileUrl = reader("FileUrl").ToString()
                    Else
                        m_FileUrl = ""
                    End If

                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

    End Class

    Public Class AudioCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal Audio As AudioRow)
            Me.List.Add(Audio)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As AudioRow
            Get
                Return CType(Me.List.Item(Index), AudioRow)
            End Get

            Set(ByVal Value As AudioRow)
                Me.List(Index) = Value
            End Set
        End Property
        Public ReadOnly Property Clone() As AudioCollection
            Get
                If Me.List Is Nothing Then
                    Return Nothing
                End If
                Dim result As New AudioCollection
                For Each obj As AudioRow In Me.List
                    result.Add(CloneObject.Clone(obj))
                Next
                Return result
            End Get
        End Property
    End Class
End Namespace



