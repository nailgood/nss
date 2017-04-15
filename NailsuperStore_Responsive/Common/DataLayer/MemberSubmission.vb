Option Explicit On

Imports System
Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Components
Imports Utility
Imports System.Text.RegularExpressions
Imports Database
Namespace DataLayer
    Public Class MemberSubmissionRow
        Inherits MemberSubmissionRowBase
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal SubmissionId As Integer)
            MyBase.New(database, SubmissionId)
        End Sub 'New
        
        'end 23/10/2009
        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal SubmissionId As Integer) As MemberSubmissionRow
            Dim row As MemberSubmissionRow

            row = New MemberSubmissionRow(_Database, SubmissionId)
            row.Load()

            Return row
        End Function

        Public Shared Function SetXMLtag(ByVal colName As String, ByVal Value As String, ByVal cData As Boolean)

            Return vbCrLf & "<" & colName & ">" & IIf(cData, CheckCDATA(Value), Value) & "</" & colName & ">"
        End Function

        Private Shared Function CheckCDATA(ByVal strValue As String) As String

            Dim pattern As String = "[^a-zA-Z0-9]"
            If (Regex.IsMatch(strValue, pattern)) Then
                Return "<![CDATA[" & strValue & "]]>"
            End If
            Return strValue
        End Function

    End Class
    Public MustInherit Class MemberSubmissionRowBase
        Private m_DB As Database
        Private m_MemberId As Integer = Nothing
        Private m_SubmissionId As Integer = Nothing
        Private m_Name As String = Nothing
        Private m_Email As String = Nothing
        Private m_Country As String = Nothing
        Private m_ArtName As String = Nothing
        Private m_SalonName As String = Nothing
        Private m_Instruction As String = Nothing
        Private m_SubmittedDate As DateTime = Nothing
        Private m_Status As Boolean = Nothing
        Private m_Type As Integer = Nothing
        Private m_FileName As String = Nothing
        Private m_AdminUploadFile As String = Nothing
        Private Shared cachePrefixKey As String = ""
        Public Shared TotalRecord As Integer
        Public Shared arrFileInsert As String = Nothing
        Private m_FileId As Integer = Nothing
        Public itemIndex As Integer = 0
        Public Property MemberId() As Integer
            Get
                Return m_MemberId
            End Get
            Set(ByVal value As Integer)
                m_MemberId = value
            End Set
        End Property
        Public Property SubmissionId() As Integer
            Get
                Return m_SubmissionId
            End Get
            Set(ByVal value As Integer)
                m_SubmissionId = value
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

        Public Property Email() As String
            Get
                Return m_Email
            End Get
            Set(ByVal Value As String)
                m_Email = Value
            End Set
        End Property

        Public Property Country() As String
            Get
                Return m_Country
            End Get
            Set(ByVal Value As String)
                m_Country = Value
            End Set
        End Property

        Public Property ArtName() As String
            Get
                Return m_ArtName
            End Get
            Set(ByVal Value As String)
                m_ArtName = Value
            End Set
        End Property

        Public Property SalonName() As String
            Get
                Return m_SalonName
            End Get
            Set(ByVal Value As String)
                m_SalonName = Value
            End Set
        End Property

        Public Property Instruction() As String
            Get
                Return m_Instruction
            End Get
            Set(ByVal Value As String)
                m_Instruction = Value
            End Set
        End Property

        Public Property SubmittedDate() As DateTime
            Get
                Return m_SubmittedDate
            End Get
            Set(ByVal Value As DateTime)
                m_SubmittedDate = Value
            End Set
        End Property
        Public Property Status() As Boolean
            Get
                Return m_Status
            End Get
            Set(ByVal Value As Boolean)
                m_Status = Value
            End Set
        End Property

        Public Property Type() As Integer
            Get
                Return m_Type
            End Get
            Set(ByVal Value As Integer)
                m_Type = Value
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
        Public Property AdminUploadFile() As String
            Get
                Return m_AdminUploadFile
            End Get
            Set(ByVal Value As String)
                m_AdminUploadFile = Value
            End Set
        End Property
        Public Property FileId() As Integer
            Get
                Return m_FileId
            End Get
            Set(ByVal Value As Integer)
                m_FileId = Value
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

        Public Sub New(ByVal database As Database, ByVal SubmissionId As Integer)
            m_DB = database
            m_SubmissionId = SubmissionId
        End Sub 'New

        Public Shared Function ChangeIsActive(ByVal _Database As Database, ByVal SubmissionId As Integer) As Boolean
            Dim result As Integer = 0
            Dim sp As String = "sp_MemberSubmission_ChangeIsActive"

            Try
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("SubmissionId", SqlDbType.Int, 0, SubmissionId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
                Utility.CacheUtils.RemoveCache(cachePrefixKey)

            Catch ex As Exception
                Components.Email.SendError("ToError500", "ChangeIsActive", "Exception" & ex.Message & ", Stack trace:" & ex.StackTrace)
            End Try

            Return result = 1
        End Function

        Public Shared Function GetGallery(ByVal PageSize As Integer, ByVal PageIndex As Integer) As MemberSubmissionCollection
            'Get cache
            Dim dr As SqlDataReader = Nothing
            Dim mrc As MemberSubmissionCollection
            cachePrefixKey = String.Format("Gallery_" & "{0}_{1}", PageIndex, PageSize)
            Dim key As String = cachePrefixKey
            mrc = CType(CacheUtils.GetCache(key), MemberSubmissionCollection)
            If Not mrc Is Nothing Then
                Return mrc
            Else
                mrc = New MemberSubmissionCollection
            End If

            'Get db
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_MemberSubmission_GetGallery"
            Try
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, PageIndex)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int32, TotalRecord)
                dr = db.ExecuteReader(cmd)

                While dr.Read
                    Dim mr As MemberSubmissionRow = LoadSubmissionRow(dr)
                    mrc.Add(mr)
                End While
                Core.CloseReader(dr)
                TotalRecord = CInt(cmd.Parameters("@TotalRecords").Value)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "GetGallery", "Exception" & ex.Message & ", Stack trace:" & ex.StackTrace)
            End Try

            CacheUtils.SetCache(key, mrc, Utility.ConfigData.TimeCacheDataItem)
            Return mrc
        End Function
        Public Shared Function ListTop4Gallery() As List(Of MemberSubmissionRow)
            Dim r As SqlDataReader
            Dim result As List(Of MemberSubmissionRow) = New List(Of MemberSubmissionRow)
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_MemberSubmission_Top4Gallery"

            Try
                Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
                r = db.ExecuteReader(cmd)
                If r.HasRows Then
                    result = mapList(Of MemberSubmissionRow)(r)
                End If

                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "ListTop4Gallery", "Exception" & ex.Message & ", Stack trace:" & ex.StackTrace)
            End Try

            Return result
        End Function

        Public Shared Function GetGalleryById(ByVal sId As Integer) As MemberSubmissionCollection
            'Get cache
            Dim mrc As MemberSubmissionCollection
            Dim dr As SqlDataReader = Nothing
            cachePrefixKey = String.Format("Gallery_detail_" & "{0}", sId)
            Dim key As String = cachePrefixKey
            mrc = CType(CacheUtils.GetCache(key), MemberSubmissionCollection)
            If Not mrc Is Nothing Then
                Return mrc
            Else
                mrc = New MemberSubmissionCollection
            End If

            'Get db
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_MemberSubmissionFile_GetGalleryById"
            Try
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "SubmissionId", DbType.Int32, sId)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim mr As MemberSubmissionRow = LoadSubmissionRow(dr)
                    mrc.Add(mr)
                End While

                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "GetGalleryById", "Exception" & ex.Message & ", Stack trace:" & ex.StackTrace)
            End Try

            CacheUtils.SetCache(key, mrc, Utility.ConfigData.TimeCacheDataItem)
            Return mrc
        End Function

        Protected Overridable Sub Load()
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_GETOBJECT As String = "sp_MemberSubmission_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)
                db.AddInParameter(cmd, "SubmissionId", DbType.Int32, SubmissionId)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try
        End Sub
        Private Shared Function LoadSubmissionRow(ByVal r As SqlDataReader) As MemberSubmissionRow
            Dim mr As New MemberSubmissionRow
            Try
                If IsDBNull(r.Item("Row")) Then
                    mr.itemIndex = Nothing
                Else
                    mr.itemIndex = Convert.ToInt32(r.Item("Row"))
                End If
            Catch
                mr.itemIndex = 0
            End Try
            Try
                If IsDBNull(r.Item("FileId")) Then
                    mr.FileId = Nothing
                Else
                    mr.FileId = Convert.ToInt32(r.Item("FileId"))
                End If
            Catch
                mr.FileId = 0
            End Try

            If IsDBNull(r.Item("SubmissionId")) Then
                mr.SubmissionId = Nothing
            Else
                mr.SubmissionId = Convert.ToString(r.Item("SubmissionId"))
            End If
            If IsDBNull(r.Item("AdminUploadFile")) Then
                mr.AdminUploadFile = Nothing
            Else
                mr.AdminUploadFile = Convert.ToString(r.Item("AdminUploadFile"))
            End If
            If IsDBNull(r.Item("Name")) Then
                mr.Name = Nothing
            Else
                mr.Name = Convert.ToString(r.Item("Name"))
            End If
            Try
                If IsDBNull(r.Item("Country")) Then
                    mr.Country = Nothing
                Else
                    mr.Country = Convert.ToString(r.Item("Country"))
                End If
            Catch
                mr.Country = ""
            End Try

            If IsDBNull(r.Item("ArtName")) Then
                mr.ArtName = Nothing
            Else
                mr.ArtName = Convert.ToString(r.Item("ArtName"))
            End If
            If IsDBNull(r.Item("SalonName")) Then
                mr.SalonName = Nothing
            Else
                mr.SalonName = Convert.ToString(r.Item("SalonName"))
            End If
            Try
                If IsDBNull(r.Item("Instruction")) Then
                    mr.Instruction = Nothing
                Else
                    mr.Instruction = Convert.ToString(r.Item("Instruction"))
                End If
            Catch ex As Exception
                mr.Instruction = ""
            End Try

            Return mr
        End Function 'Load
        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            If IsDBNull(r.Item("SubmissionId")) Then
                m_SubmissionId = Nothing
            Else
                m_SubmissionId = Convert.ToInt32(r.Item("SubmissionId"))
            End If
            If IsDBNull(r.Item("MemberId")) Then
                m_MemberId = Nothing
            Else
                m_MemberId = Convert.ToInt32(r.Item("MemberId"))
            End If
            If IsDBNull(r.Item("Name")) Then
                m_Name = Nothing
            Else
                m_Name = Convert.ToString(r.Item("Name"))
            End If
            If IsDBNull(r.Item("Email")) Then
                m_Email = Nothing
            Else
                m_Email = Convert.ToString(r.Item("Email"))
            End If
            If IsDBNull(r.Item("Country")) Then
                m_Country = Nothing
            Else
                m_Country = Convert.ToString(r.Item("Country"))
            End If
            If IsDBNull(r.Item("ArtName")) Then
                m_ArtName = Nothing
            Else
                m_ArtName = Convert.ToString(r.Item("ArtName"))
            End If
            If IsDBNull(r.Item("SalonName")) Then
                m_SalonName = Nothing
            Else
                m_SalonName = Convert.ToString(r.Item("SalonName"))
            End If
            If IsDBNull(r.Item("Instruction")) Then
                m_Instruction = Nothing
            Else
                m_Instruction = Convert.ToString(r.Item("Instruction"))
            End If
            If IsDBNull(r.Item("Status")) Then
                m_Status = Nothing
            Else
                m_Status = Convert.ToInt32(r.Item("Status"))
            End If
            If IsDBNull(r.Item("Type")) Then
                m_Type = Nothing
            Else
                m_Type = Convert.ToInt32(r.Item("Type"))
            End If
            If IsDBNull(r.Item("SubmittedDate")) Then
                m_SubmittedDate = Nothing
            Else
                m_SubmittedDate = Convert.ToDateTime(r.Item("SubmittedDate"))
            End If
            If IsDBNull(r.Item("FileName")) Then
                m_FileName = Nothing
            Else
                m_FileName = Convert.ToString(r.Item("FileName"))
            End If
            If IsDBNull(r.Item("AdminUploadFile")) Then
                m_AdminUploadFile = Nothing
            Else
                m_AdminUploadFile = Convert.ToString(r.Item("AdminUploadFile"))
            End If
        End Sub 'Load
        'Public Overridable Sub Insert()
        '    Dim result As Integer = 0


        '    Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
        '    Dim cmd As DbCommand = db.GetStoredProcCommand("sp_MemberSubmission_Insert")
        '    db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)
        '    db.AddInParameter(cmd, "Name", DbType.String, Name)
        '    db.AddInParameter(cmd, "Email", DbType.String, Email)
        '    db.AddInParameter(cmd, "Country", DbType.String, Country)
        '    db.AddInParameter(cmd, "ArtName", DbType.String, ArtName)
        '    db.AddInParameter(cmd, "SalonName", DbType.String, SalonName)
        '    db.AddInParameter(cmd, "Instruction", DbType.String, Instruction)
        '    db.AddInParameter(cmd, "SubmittedDate", DbType.DateTime, Date.Now)
        '    db.AddInParameter(cmd, "Status", DbType.Int32, Convert.ToUInt32(Status))
        '    db.AddInParameter(cmd, "Type", DbType.Int32, Convert.ToUInt32(Type))
        '    db.AddInParameter(cmd, "FileName", DbType.String, FileName)
        '    db.AddOutParameter(cmd, "SubmissionId", DbType.Int64, 1)
        '    db.ExecuteNonQuery(cmd)
        '    result = Convert.ToInt32(db.GetParameterValue(cmd, "SubmissionId"))

        '    If result > 0 Then
        '        Dim msf As New MemberSubmissionFileRow
        '        Dim arrFileName As String()
        '        If FileName <> Nothing Then
        '            Try
        '                arrFileName = FileName.Split(";")
        '                msf.SubmissionId = result
        '                For i As Integer = 0 To arrFileName.Length - 1
        '                    If arrFileName(i) <> "" Then
        '                        msf.FileName = arrFileName(i)
        '                        msf.Insert()
        '                    End If
        '                Next
        '            Catch ex As Exception
        '                Components.Email.SendError("ToError500", "Insert Submission File", ex.Message & ",Stack trace:" & ex.StackTrace)
        '            End Try
        '        End If
        '    End If
        'End Sub
        Public Overridable Sub Insert(ByVal db As EnterpriseLibrary.Data.Database, ByVal trans As DbTransaction)
            'Dim db As EnterpriseLibrary.Data.Database = dbEnterPrise ' DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = db.GetStoredProcCommand("sp_MemberSubmission_Insert")
            db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)
            db.AddInParameter(cmd, "Name", DbType.String, Name)
            db.AddInParameter(cmd, "Email", DbType.String, Email)
            db.AddInParameter(cmd, "Country", DbType.String, Country)
            db.AddInParameter(cmd, "ArtName", DbType.String, ArtName)
            db.AddInParameter(cmd, "SalonName", DbType.String, SalonName)
            db.AddInParameter(cmd, "Instruction", DbType.String, Instruction)
            db.AddInParameter(cmd, "SubmittedDate", DbType.DateTime, Date.Now)
            db.AddInParameter(cmd, "Status", DbType.Int32, Convert.ToUInt32(Status))
            db.AddInParameter(cmd, "Type", DbType.Int32, Convert.ToUInt32(Type))
            db.AddOutParameter(cmd, "SubmissionId", DbType.Int64, 1)
            db.ExecuteNonQuery(cmd, trans)
            SubmissionId = Convert.ToInt32(db.GetParameterValue(cmd, "SubmissionId"))

            If SubmissionId > 0 Then
                Dim msf As New MemberSubmissionFileRow
                Dim arrFileName As String()
                If FileName <> Nothing Then
                    Try
                        arrFileName = FileName.Split(";")
                        msf.SubmissionId = SubmissionId
                        For i As Integer = 0 To arrFileName.Length - 1
                            If arrFileName(i) <> "" Then
                                msf.FileName = arrFileName(i)
                                msf.Insert(db, trans)
                                arrFileInsert &= msf.NewId & "-" & msf.FileName & ";"

                            End If
                        Next
                        arrFileInsert = arrFileInsert.Replace("-.", ".")
                    Catch ex As Exception
                        Components.Email.SendError("ToError500", "Insert Submission File", ex.Message & ",Stack trace:" & ex.StackTrace)
                    End Try
                End If
            End If
            Utility.CacheUtils.RemoveCache(cachePrefixKey)
        End Sub
        'Public Overridable Sub Insert(ByVal DB1 As Database)
        '    DB = DB1
        '    Dim SQL As String
        '    SQL = " INSERT INTO MemberSubmission (" _
        '     & " MemberId" _
        '     & ",Name" _
        '     & ",Email" _
        '     & ",Country" _
        '     & ",ArtName" _
        '     & ",SalonName" _
        '     & ",Instruction" _
        '     & ",Status" _
        '     & ",Type" _
        '     & ",SubmittedDate" _
        '     & ") VALUES (" _
        '     & m_DB.Number(MemberId) _
        '     & "," & m_DB.Quote(Name) _
        '     & "," & m_DB.Quote(Email) _
        '     & "," & m_DB.Quote(Country) _
        '     & "," & m_DB.Quote(ArtName) _
        '     & "," & m_DB.Quote(SalonName) _
        '     & "," & m_DB.Quote(Instruction) _
        '     & "," & CInt(Status) _
        '     & "," & CInt(Type) _
        '     & "," & m_DB.NullQuote(Now) _
        '     & ")"

        '    SubmissionId = m_DB.InsertSQL(SQL)
        '    If SubmissionId > 0 Then
        '        Dim msf As New MemberSubmissionFileRow
        '        Dim arrFileName As String()
        '        If FileName <> Nothing Then
        '            Try
        '                arrFileName = FileName.Split(";")
        '                msf.SubmissionId = SubmissionId
        '                For i As Integer = 0 To arrFileName.Length - 1
        '                    If arrFileName(i) <> "" Then
        '                        msf.FileName = arrFileName(i)
        '                        msf.Insert()
        '                        arrFileInsert &= msf.NewId & "-" & msf.FileName & ";"
        '                    End If
        '                Next
        '            Catch ex As Exception
        '                Components.Email.SendError("ToError500", "Insert Submission File", ex.Message & ",Stack trace:" & ex.StackTrace)
        '            End Try
        '        End If
        '    End If
        'End Sub
        Public Overloads Sub Update()
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_MemberSubmission_Update"
                Dim cm As DbCommand = db.GetStoredProcCommand(SP)

                'db.AddInParameter(cm, "MemberId", DbType.Int32, MemberId)
                db.AddInParameter(cm, "SubmissionId", DbType.Int32, SubmissionId)
                db.AddInParameter(cm, "Name", DbType.String, Name)
                db.AddInParameter(cm, "Email", DbType.String, Email)
                db.AddInParameter(cm, "Country", DbType.String, Country)
                db.AddInParameter(cm, "ArtName", DbType.String, ArtName)
                db.AddInParameter(cm, "SalonName", DbType.String, SalonName)
                db.AddInParameter(cm, "Instruction", DbType.String, Instruction)
                'db.AddInParameter(cm, "Type", DbType.Int32, Type)
                db.AddInParameter(cm, "Status", DbType.Int32, Status)
                db.AddInParameter(cm, "SubmittedDate", DbType.DateTime, SubmittedDate)
                db.ExecuteNonQuery(cm)
                Utility.CacheUtils.RemoveCache(cachePrefixKey)
            Catch ex As Exception

            End Try
        End Sub
        Public Shared Function Delete(ByVal _Database As Database, ByVal SubmissionId As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_MemberSubmission_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("SubmissionId", SqlDbType.Int, 0, SubmissionId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
                Utility.CacheUtils.RemoveCache(cachePrefixKey)
            Catch ex As Exception
            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function
    End Class
    Public Class MemberSubmissionCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal MemberSubmission As MemberSubmissionRow)
            Me.List.Add(MemberSubmission)
        End Sub

        Public Function Contains(ByVal MemberSubmission As MemberSubmissionRow) As Boolean
            Return Me.List.Contains(MemberSubmission)
        End Function

        Public Function IndexOf(ByVal MemberSubmissionFile As MemberSubmissionFileRow) As Integer
            Return Me.List.IndexOf(MemberSubmissionFile)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal MemberSubmission As MemberSubmissionRow)
            Me.List.Insert(Index, MemberSubmission)
        End Sub
        Default Public Property Item(ByVal Index As Integer) As MemberSubmissionRow
            Get
                Return CType(Me.List.Item(Index), MemberSubmissionRow)
            End Get

            Set(ByVal Value As MemberSubmissionRow)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace
