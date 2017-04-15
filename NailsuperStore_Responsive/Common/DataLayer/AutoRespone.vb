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
    Public Class AutoRespondRow
        Inherits AutoRespondRowBase
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal DayId As Integer)
            MyBase.New(database, DayId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal DayId As Integer) As AutoRespondRow
            Dim row As AutoRespondRow
            row = New AutoRespondRow(_Database, DayId)
            row.Load()
            Return row
        End Function
    End Class
    Public MustInherit Class AutoRespondRowBase
        Private m_DB As Database
        Private m_DayId As Integer = Nothing
        Private m_DayName As String = Nothing
        Private m_StartingDate As DateTime
        Private m_EndingDate As DateTime
        Private m_CreateDate As DateTime
        Private m_ModifyDate As DateTime
        Private m_AdminId As Integer = Nothing
        Public Property DayId() As Integer
            Get
                Return m_DayId
            End Get
            Set(ByVal Value As Integer)
                m_DayId = Value
            End Set
        End Property
        Public Property DayName() As String
            Get
                Return m_DayName
            End Get
            Set(ByVal Value As String)
                m_DayName = Value
            End Set
        End Property
        Public Property StartingDate() As DateTime
            Get
                Return m_StartingDate
            End Get
            Set(ByVal Value As DateTime)
                m_StartingDate = Value
            End Set
        End Property
        Public Property EndingDate() As DateTime
            Get
                Return m_EndingDate
            End Get
            Set(ByVal Value As DateTime)
                m_EndingDate = Value
            End Set
        End Property
        Public Property CreateDate() As DateTime
            Get
                Return m_CreateDate
            End Get
            Set(ByVal Value As DateTime)
                m_CreateDate = Value
            End Set
        End Property
        Public Property ModifyDate() As DateTime
            Get
                Return m_ModifyDate
            End Get
            Set(ByVal Value As DateTime)
                m_ModifyDate = Value
            End Set
        End Property
        
        Public Property AdminId() As Integer
            Get
                Return m_AdminId
            End Get
            Set(ByVal Value As Integer)
                m_AdminId = Value
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

        Public Sub New(ByVal DB As Database)
            m_DB = DB
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal DayId As Integer)
            m_DB = DB
            m_DayId = DayId
        End Sub 'New
        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM AutoRespond WHERE DayId = " & DB.Number(DayId)
                r = m_DB.GetReader(SQL)
                If Not r Is Nothing Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Email.SendError("ToError500", "AutoResponse.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
        End Sub

        Public Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                If (Not r Is Nothing And Not r.IsClosed) Then
                    If (Not r.IsDBNull(r.GetOrdinal("DayId"))) Then
                        m_DayId = Convert.ToInt32(r("DayId"))
                    Else
                        m_DayId = 0
                    End If
                    If (Not r.IsDBNull(r.GetOrdinal("DayName"))) Then
                        m_DayName = r("DayName").ToString()
                    Else
                        m_DayName = ""
                    End If
                    If IsDBNull(r.GetOrdinal("StartingDate")) Then
                        m_StartingDate = Nothing
                    Else
                        m_StartingDate = Convert.ToDateTime(r.Item("StartingDate"))
                    End If
                    If IsDBNull(r.GetOrdinal("EndingDate")) Then
                        m_EndingDate = Nothing
                    Else
                        m_EndingDate = Convert.ToDateTime(r.Item("EndingDate"))
                    End If

                    If IsDBNull(r.Item("CreateDate")) Then
                        m_CreateDate = Nothing
                    Else
                        m_CreateDate = DateTime.Now
                    End If
                    If IsDBNull(r.Item("ModifyDate")) Then
                        m_ModifyDate = Nothing
                    Else
                        m_ModifyDate = Convert.ToDateTime(r.Item("ModifyDate"))
                    End If
                    If (Not r.IsDBNull(r.GetOrdinal("AdminId"))) Then
                        m_AdminId = Convert.ToInt32(r.Item("AdminId"))
                    Else
                        m_AdminId = 0
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try
            
        End Sub
        Public Shared Function Delete(ByVal _Database As Database, ByVal DayId As Integer) As Boolean

            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_AutoRespond_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("DayId", SqlDbType.Int, 0, DayId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception

            End Try
            If result = 1 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function CheckDate(ByVal database As Database, ByVal sDate As DateTime) As Boolean
            Dim StartDate, strStartTime, EndDate, strEndTime As String
            Dim iStartDay, iEndDay As Integer

            Try
                StartDate = SysParam.GetValue("AutoRespondStartDate")
                iStartDay = CInt([Enum].Parse(GetType(Common.Days), StartDate.Split(" ")(0)))
                strStartTime = StartDate.Split(" ")(1)
            Catch ex As Exception
                iStartDay = 6 'SAT
                strStartTime = "17:30"
            End Try

            Try
                EndDate = SysParam.GetValue("AutoRespondEndDate")

                iEndDay = CInt([Enum].Parse(GetType(Common.Days), EndDate.Split(" ")(0)))
                strEndTime = EndDate.Split(" ")(1)
            Catch ex As Exception
                iEndDay = 0 'SUN
                strEndTime = "23:59"
            End Try

            Try
                Dim inputDay As Integer = CInt([Enum].Parse(GetType(Common.Days), UCase(Left(sDate.DayOfWeek.ToString(), 3))))
                Dim iNowTime As Integer = CInt(sDate.Hour.ToString("00") & sDate.Minute.ToString("00"))
                Dim iStartTime As Integer = CInt(strStartTime.Replace(":", ""))
                Dim iEndTime As Integer = CInt(strEndTime.Replace(":", ""))

                If iStartDay < iEndDay And inputDay > iStartDay And inputDay < iEndDay Then
                    DayName = "holiday"
                    Return True
                ElseIf iStartDay > iEndDay And (inputDay > iStartDay Or inputDay < iEndDay) Then
                    DayName = "holiday"
                    Return True
                ElseIf inputDay = iStartDay And iNowTime >= iStartTime Then
                    DayName = "holiday"
                    Return True
                ElseIf inputDay = iEndDay And iNowTime < iEndTime Then
                    DayName = "holiday"
                    Return True
                Else
                    Dim DayId As Integer = database.ExecuteScalar("Select isnull(DayId,0) from AutoRespond where StartingDate <= '" & sDate & "' and EndingDate >= '" & sDate & "'")
                    If DayId > 0 Then
                        DayName = database.ExecuteScalar("Select isnull(DayName,'holiday') from AutoRespond where DayId = " & DayId)
                        Return True
                    Else
                        Return False
                    End If
                End If
            Catch ex As Exception
                Return False
            End Try

        End Function
        Public Function SendAuto(ByVal ToEmail As String, ByVal ToName As String) As Boolean

            Dim strEx As String = String.Empty

            Try
                Dim et As EmailTempletRow = EmailTempletRow.GetRow(DB, 19)
                Dim Subject As String = et.Subject
                Dim sMsg As String = et.Contents
                sMsg = sMsg.Replace("#USERNAME#", ToName)
                sMsg = sMsg.Replace("#DAYNAME#", DayName)
                Dim bcc As String = SysParam.GetValue("AutoResponseBCC")
                Return Email.SendHTMLMail(FromEmailType.NoReply, ToEmail, ToName, "Thank you for your email", sMsg)

            Catch ex As Exception
                Email.SendError("ToError500", "SendAuto", ex.ToString())
                Return False
            End Try

        End Function
        Public Overridable Sub Insert()
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_AutoRespond_Insert"
            Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
            db.AddOutParameter(cmd, "DayId", DbType.Int32, 32)
            db.AddInParameter(cmd, "DayName", DbType.String, DayName)
            db.AddInParameter(cmd, "StartingDate", DbType.DateTime, Param.ObjectToDB(StartingDate))
            db.AddInParameter(cmd, "EndingDate", DbType.DateTime, Param.ObjectToDB(EndingDate))
            db.AddInParameter(cmd, "CreateDate", DbType.DateTime, Param.ObjectToDB(DateTime.Now))
            db.AddInParameter(cmd, "AdminId", DbType.Int32, AdminId)

            db.ExecuteNonQuery(cmd)
            DayId = Convert.ToInt32(db.GetParameterValue(cmd, "DayId"))
        End Sub
        Public Overridable Sub Update()
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_AutoRespond_Update"
            Dim cmd As SqlCommand = db.GetStoredProcCommand(sp)
            db.AddInParameter(cmd, "DayId", DbType.Int32, DayId)
            db.AddInParameter(cmd, "DayName", DbType.String, DayName)
            db.AddInParameter(cmd, "StartingDate", DbType.DateTime, Param.ObjectToDB(StartingDate))
            db.AddInParameter(cmd, "EndingDate", DbType.DateTime, Param.ObjectToDB(EndingDate))
            db.AddInParameter(cmd, "ModifyDate", DbType.DateTime, Param.ObjectToDB(DateTime.Now))
            db.AddInParameter(cmd, "AdminId", DbType.Int32, AdminId)
            db.ExecuteNonQuery(cmd)
        End Sub

    End Class
    Public Class AutoRespondCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal AutoRespond As AutoRespondRow)
            Me.List.Add(AutoRespond)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As AutoRespondRow
            Get
                Return CType(Me.List.Item(Index), AutoRespondRow)
            End Get

            Set(ByVal Value As AutoRespondRow)
                Me.List(Index) = Value
            End Set
        End Property
        Public ReadOnly Property Clone() As AutoRespondCollection
            Get
                If Me.List Is Nothing Then
                    Return Nothing
                End If
                Dim result As New AutoRespondCollection
                For Each obj As AutoRespondRow In Me.List
                    result.Add(CloneObject.Clone(obj))
                Next
                Return result
            End Get
        End Property
    End Class
End Namespace

