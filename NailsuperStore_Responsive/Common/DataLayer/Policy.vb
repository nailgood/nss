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
    Public Class PolicyRow
        Inherits PolicyRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal PolicyId As Integer)
            MyBase.New(PolicyId)
        End Sub 'New

        Public Shared Function GetRow(ByVal PolicyId As Integer) As PolicyRow
            Dim row As New PolicyRow(PolicyId)
            row.Load()
            Return row
        End Function

        Public Shared Function ListByItemId(ByVal ItemId As Integer) As PolicyCollection
            Dim key As String = "Policy_ListByItemId"
            Dim collection As New PolicyCollection
            'collection = CType(CacheUtils.GetCache(key), PolicyCollection)
            'If collection IsNot Nothing Then
            '    Return collection
            'Else
            '    collection = New PolicyCollection()
            'End If

            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_Policy_ListByItemId"
                Dim p As PolicyRow
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "ItemId", DbType.Int32, ItemId)
                dr = db.ExecuteReader(cmd)

                If dr.HasRows Then
                    While dr.Read
                        p = New PolicyRow()
                        p.Load(dr)
                        collection.Add(p)
                    End While
                End If
                
                Core.CloseReader(dr)

                'collection.TotalRecords = collection.Count
                'CacheUtils.SetCache(key, collection, Utility.ConfigData.TimeCacheData)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "Policy.vb > ListByItemId", "Exception: " & ex.ToString())
            End Try

            Return collection
        End Function


        Public Shared Function ListAll() As PolicyCollection
            Dim key As String = "Policy_ListAll"
            Dim collection As New PolicyCollection
            collection = CType(CacheUtils.GetCache(key), PolicyCollection)
            If collection IsNot Nothing Then
                Return collection
            Else
                collection = New PolicyCollection()
            End If

            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_Policy_ListAll"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                dr = db.ExecuteReader(cmd)
                Dim p As PolicyRow
                While dr.Read
                    p = New PolicyRow()
                    p.Load(dr)
                    collection.Add(p)
                End While
                Core.CloseReader(dr)

                collection.TotalRecords = collection.Count
                CacheUtils.SetCache(key, collection, Utility.ConfigData.TimeCacheData)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "Policy.vb > ListAll", "Exception: " & ex.ToString())
            End Try

            Return collection
        End Function

        Public Shared Function Delete(ByVal PolicyId As Integer) As Boolean
            Dim i As Integer = 0
            Try
                CacheUtils.ClearCacheWithPrefix("Policy_")
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_Policy_Delete"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "PolicyId", DbType.Int32, PolicyId)
                i = db.ExecuteNonQuery(cmd)

            Catch ex As Exception
                Components.Email.SendError("ToError500", "Policy.vb > Delete", "PolicyId: " & PolicyId & "<br>Exception: " & ex.ToString())
            End Try

            Return i > 0
        End Function

        Public Shared Function ChangeIsActive(ByVal PolicyId As Integer) As Boolean
            Dim i As Integer = 0

            Try
                CacheUtils.ClearCacheWithPrefix("Policy_")
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_Policy_ChangeIsActive"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "PolicyId", DbType.Int32, PolicyId)
                i = db.ExecuteNonQuery(cmd)

            Catch ex As Exception
                Components.Email.SendError("ToError500", "Policy.vb > ChangeIsActive", "PolicyId: " & PolicyId & "<br>Exception: " & ex.ToString())
            End Try

            Return i > 0
        End Function
    End Class

    Public MustInherit Class PolicyRowBase
        Private m_PolicyId As Integer = Nothing
        Private m_IsActive As Boolean = True
        Private m_Popup As Boolean = True
        Private m_IsPopup As Boolean = True
        Private m_Title As String = Nothing
        Private m_Content As String = Nothing
        Private M_Message As String = Nothing
        Private m_TextLink As String = Nothing

        Public Property PolicyId() As Integer
            Get
                Return m_PolicyId
            End Get
            Set(ByVal Value As Integer)
                m_PolicyId = Value
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

        Public Property TextLink() As String
            Get
                Return m_TextLink
            End Get
            Set(ByVal Value As String)
                m_TextLink = Value
            End Set
        End Property

        Public Property Content() As String
            Get
                Return m_Content
            End Get
            Set(ByVal Value As String)
                m_Content = Value
            End Set
        End Property

        Public Property Message() As String
            Get
                Return M_Message
            End Get
            Set(ByVal Value As String)
                M_Message = Value
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

        Public Property IsPopup() As Boolean
            Get
                Return m_IsPopup
            End Get
            Set(ByVal Value As Boolean)
                m_IsPopup = Value
            End Set
        End Property

        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal PolicyId As Integer)
            m_PolicyId = PolicyId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim dr As SqlDataReader = Nothing
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Try
                Dim SQL As String = "SELECT PolicyId, Title, Message, TextLink, Content, IsActive, IsPopup, CreatedDate, ModifiedDate FROM Policy WHERE PolicyId = " & PolicyId
                dr = db.ExecuteReader(CommandType.Text, SQL)

                If dr.HasRows Then
                    If dr.Read Then
                        Me.Load(dr)
                    End If
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "Policy.vb > Load", ex.ToString())
            End Try
        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("PolicyId"))) Then
                        m_PolicyId = Convert.ToInt32(reader("PolicyId"))
                    Else
                        m_PolicyId = 0
                    End If

                    Try
                        If (Not reader.IsDBNull(reader.GetOrdinal("Title"))) Then
                            m_Title = reader("Title").ToString()
                        Else
                            m_Title = ""
                        End If
                    Catch ex As Exception
                        m_Title = ""
                    End Try
                   

                    Try
                        If (Not reader.IsDBNull(reader.GetOrdinal("TextLink"))) Then
                            m_TextLink = reader("TextLink").ToString()
                        Else
                            m_TextLink = ""
                        End If
                    Catch ex As Exception
                        m_TextLink = ""
                    End Try
                    

                    If (Not reader.IsDBNull(reader.GetOrdinal("Message"))) Then
                        M_Message = reader("Message").ToString()
                    Else
                        M_Message = ""
                    End If

                    Try
                        If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                            m_IsActive = Convert.ToBoolean(reader("IsActive"))
                        Else
                            m_IsActive = True
                        End If
                    Catch ex As Exception
                        m_IsActive = True
                    End Try
                    
                    Try
                        If (Not reader.IsDBNull(reader.GetOrdinal("IsPopup"))) Then
                            m_IsPopup = Convert.ToBoolean(reader("IsPopup"))
                        Else
                            m_IsPopup = True
                        End If
                    Catch ex As Exception
                        m_IsPopup = False
                    End Try
                    
                    Try
                        If (Not reader.IsDBNull(reader.GetOrdinal("Content"))) Then
                            m_Content = reader("Content").ToString()
                        Else
                            m_Content = ""
                        End If
                    Catch ex As Exception
                        m_Content = ""
                    End Try
                    
                End If
            Catch ex As Exception
                Components.Email.SendError("ToError500", "Policy.vb > Load", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub

        Public Overridable Function Insert() As Boolean
            Dim i As Integer = 0
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim dba As New Database()

            Try
                Dim SQL As String = " INSERT INTO Policy (" _
                 & "Title" _
                 & ",CreatedDate" _
                 & ",ModifiedDate" _
                 & ",Message" _
                 & ",TextLink" _
                 & ",Content" _
                 & ",IsActive" _
                 & ",IsPopup" _
                 & ") VALUES (" _
                 & "" & dba.NQuote(Title) _
                 & "," & dba.NullQuote(DateTime.Now) _
                 & "," & dba.NullQuote(DateTime.Now) _
                 & "," & dba.NQuote(Message) _
                 & "," & dba.NQuote(TextLink) _
                 & "," & dba.NQuote(Content) _
                 & "," & CInt(IsActive) _
                 & "," & CInt(IsPopup) _
                 & ")"

                i = db.ExecuteNonQuery(CommandType.Text, SQL)
                CacheUtils.ClearCacheWithPrefix("Policy_")
            Catch ex As Exception
                Components.Email.SendError("ToError500", "Policy.vb > Insert", "Exception: " & ex.ToString())
            End Try

            Return i > 0
        End Function

        Public Overridable Function Update() As Boolean
            Dim i As Integer = 0
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim dba As Database

                Dim SQL As String = " UPDATE Policy SET " _
                 & " Title = " & dba.Quote(Title) _
                 & ",TextLink = " & dba.Quote(TextLink) _
                 & ",Message = " & dba.Quote(Message) _
                 & ",IsActive = " & Convert.ToInt16(IsActive) _
                 & ",IsPopup = " & Convert.ToInt16(IsPopup) _
                 & ",Content = " & dba.Quote(Content) _
                 & ",ModifiedDate = " & dba.Quote(DateTime.Now.ToString()) _
                 & " WHERE PolicyId = " & CInt(PolicyId)

                i = db.ExecuteNonQuery(CommandType.Text, SQL)
                CacheUtils.ClearCacheWithPrefix("Policy_")
            Catch ex As Exception
                Components.Email.SendError("ToError500", "Policy.vb > Update", "Exception: " & ex.ToString())
            End Try

            Return i > 0
        End Function
    End Class

    Public Class PolicyCollection
        Inherits CollectionBase
        Public TotalRecords As Integer = Nothing
        Public Sub New()
        End Sub
        'New
        Public Sub Add(ByVal Policy As PolicyRow)
            Me.List.Add(Policy)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As PolicyRow
            Get
                Return CType(Me.List.Item(Index), PolicyRow)
            End Get

            Set(ByVal Value As PolicyRow)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace