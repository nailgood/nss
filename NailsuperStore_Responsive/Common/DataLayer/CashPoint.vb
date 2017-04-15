Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Components
Imports Components.Core
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices
Imports Utility
Imports System.Data.SqlClient
Namespace DataLayer
    Public Class CashPointRow
        Inherits CashPointRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New
        Public Sub New(ByVal database As Database, ByVal Type As String, ByVal Id As Integer)
            MyBase.New(database, Type, Id)
        End Sub 'New
        Public Shared Function GetTotalCashPointByMember(ByVal _Database As Database, ByVal memberId As Integer, ByVal OrderId As Integer) As Integer
            Dim i As Integer = 0

            Dim key As String = String.Format(MemberRow.cachePrefixKey & "GetTotalCashPointByMember_{0}_{1}", memberId, OrderId)
            i = CType(CacheUtils.GetCache(key), Integer)

            If i = 0 Then
                i = GetTotalCashPoint(_Database, memberId, OrderId)
                CacheUtils.SetCache(key, i, Utility.ConfigData.TimeCacheMemberData)
            End If
            Return i
        End Function
        Public Shared Function GetTotalCashPointByMember(ByVal _Database As Database, ByVal memberId As Integer) As Integer
            Return GetTotalCashPoint(_Database, memberId, Nothing)
        End Function
        Public Shared Function GetTotalCashPointAndDetailByMember(ByVal _Database As Database, ByVal memberId As Integer, ByVal getTran As Boolean, ByRef cashPoint As CashPointCollection) As DataTable
            Try
                Dim DB As Microsoft.Practices.EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim dt As DataTable = New DataTable()
                Dim storedProcName As String = "sp_CashPoint_GetTotalCashPointAndDetailByMember"
                Using sprocCmd As DbCommand = DB.GetStoredProcCommand(storedProcName)
                    DB.AddInParameter(sprocCmd, "MemberId", DbType.Int32, memberId)
                    If getTran Then
                        DB.AddInParameter(sprocCmd, "GetTranSaction", DbType.Boolean, True)
                    End If

                    Using reader As SqlDataReader = DB.ExecuteReader(sprocCmd)
                        dt.Load(reader)
                        If reader.HasRows() And getTran Then
                            cashPoint = New CashPointCollection()
                            While reader.Read()
                                cashPoint.Add(GetByDataReader(reader))
                            End While
                        End If
                        Return dt
                    End Using
                End Using

            Catch ex As Exception
                Return Nothing
            End Try
        End Function
        Public Shared Sub RemoveByOrderId(ByVal DB As Database, ByVal OrderId As Integer)
            Dim SQL As String = ""

            SQL = "DELETE FROM CashPoint WHERE OrderId = " & DB.Number(OrderId)
            DB.ExecuteSQL(SQL)
        End Sub
        Private Shared Function GetTotalCashPoint(ByVal _Database As Database, ByVal memberId As Integer, ByVal orderId As Integer) As Integer
            Dim dr As SqlDataReader = Nothing
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_CashPoint_GetTotalCashPointByMemberV2"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                If (orderId > 0) Then
                    cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, orderId))
                Else
                    cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, Nothing))
                End If
                cmd.Parameters.Add(_Database.InParam("MemberId", SqlDbType.Int, 0, memberId))
                dr = cmd.ExecuteReader()
                If dr.Read Then
                    result = Convert.ToInt32(dr.GetValue(0).ToString())
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "CashPoint.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return result
        End Function

        Public Shared Function GetPendingCashPoint(ByVal _Database As Database, ByVal memberId As Integer) As Integer
            Dim dr As SqlDataReader = Nothing
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_CashPoint_GetPendingCashPointByMember"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("MemberId", SqlDbType.Int, 0, memberId))
                dr = cmd.ExecuteReader()
                If dr.Read Then
                    result = Convert.ToInt32(dr.GetValue(0).ToString())
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "CashPoint.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return result
        End Function

        Private Shared Function GetByDataReader(ByVal r As SqlDataReader) As CashPointRow
            Try
                Dim result As New CashPointRow
                If IsDBNull(r.Item("OrderId")) Then
                    result.OrderId = 0
                Else
                    result.OrderId = Convert.ToInt32(r.Item("OrderId"))
                End If

                If IsDBNull(r.Item("MemberId")) Then
                    result.MemberId = 0
                Else
                    result.MemberId = Convert.ToInt32(r.Item("MemberId"))
                End If
                If IsDBNull(r.Item("CashPointId")) Then
                    result.CashPointId = 0
                Else
                    result.CashPointId = Convert.ToInt32(r.Item("CashPointId"))
                End If
                If IsDBNull(r.Item("TransactionNo")) Then
                    result.TransactionNo = Nothing
                Else
                    result.TransactionNo = Convert.ToString(r.Item("TransactionNo"))
                End If
                If IsDBNull(r.Item("PointEarned")) Then
                    result.PointEarned = 0
                Else
                    result.PointEarned = Convert.ToInt32(r.Item("PointEarned"))
                End If
                If IsDBNull(r.Item("PointDebit")) Then
                    result.PointDebit = 0
                Else
                    result.PointDebit = Convert.ToInt32(r.Item("PointDebit"))
                End If

                If IsDBNull(r.Item("Notes")) Then
                    result.Notes = Nothing
                Else
                    result.Notes = Convert.ToString(r.Item("Notes"))
                End If
                If IsDBNull(r.Item("Status")) Then
                    result.Status = Nothing
                Else
                    result.Status = Convert.ToInt32(r.Item("Status"))
                End If
                If IsDBNull(r.Item("CreateDate")) Then
                    result.CreateDate = Nothing
                Else
                    result.CreateDate = Convert.ToDateTime(r.Item("CreateDate"))
                End If
                If IsDBNull(r.Item("ModifyDate")) Then
                    result.ModifyDate = Nothing
                Else
                    result.ModifyDate = Convert.ToDateTime(r.Item("ModifyDate"))
                End If
                If IsDBNull(r.Item("ApproveDate")) Then
                    result.ApproveDate = Nothing
                Else
                    result.ApproveDate = Convert.ToDateTime(r.Item("ApproveDate"))
                End If
                If IsDBNull(r.Item("AdminId")) Then
                    result.AdminId = Nothing
                Else
                    result.AdminId = Convert.ToString(r.Item("AdminId"))
                End If
                Try
                    result.Amount = Convert.ToDouble(r.Item("Amount"))
                Catch ex As Exception
                    result.Amount = Nothing
                End Try
                Return result

            Catch ex As Exception
                Throw ex
            End Try
        End Function
        Public Shared Function GetListTransactionByMember(ByVal _Database As Database, ByVal username As String) As CashPointCollection
            If username Is Nothing Or username = "" Then
                Return Nothing
            End If
            Dim dr As SqlDataReader = Nothing
            Dim result As New CashPointCollection
            Try
                Dim sp As String = "sp_CashPoint_GetListTransactionByMember"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Username", SqlDbType.VarChar, 0, username))
                dr = cmd.ExecuteReader()
                While dr.Read()
                    result.Add(GetByDataReader(dr))
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "CashPoint.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return result
        End Function
        Public Shared Function GetListTransactionByMemberAdmin(ByVal _Database As Database, ByVal username As String) As CashPointCollection
            If username Is Nothing Or username = "" Then
                Return Nothing
            End If
            Dim result As New CashPointCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim sp As String = "sp_CashPoint_GetListTransactionByMemberAdmin"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Username", SqlDbType.VarChar, 0, username))
                dr = cmd.ExecuteReader()
                While dr.Read()
                    result.Add(GetByDataReader(dr))
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "CashPoint.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return result
        End Function
        Public Shared Function GetRowByCashPointId(ByVal _Database As Database, ByVal cashPointId As String) As CashPointRow
            Dim reader As SqlDataReader = Nothing
            Dim result As CashPointRow = Nothing
            Try
                reader = _Database.GetReader("Select * from CashPoint where CashPointId=" & cashPointId)
                If Not reader Is Nothing Then
                    If reader.Read() Then
                        result = GetByDataReader(reader)
                    End If
                    Core.CloseReader(reader)
                End If
            Catch ex As Exception
                Core.CloseReader(reader)
                Components.Email.SendError("ToError500", "CashPoint.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return result
        End Function
        Public Shared Function GetRowByTransID(ByVal _Database As Database, ByVal transID As String, ByVal memberId As Integer) As CashPointRow
            Dim reader As SqlDataReader = Nothing
            Dim result As CashPointRow = Nothing
            Try
                reader = _Database.GetReader("Select * from CashPoint where TransactionNo='" & transID & "' and MemberId=" & memberId)
                If Not reader Is Nothing Then
                    If reader.Read() Then
                        result = GetByDataReader(reader)
                    End If
                    Core.CloseReader(reader)
                End If
            Catch ex As Exception
                Core.CloseReader(reader)
                Components.Email.SendError("ToError500", "CashPoint.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return result
        End Function
        Public Shared Function GetRowByTransID(ByVal _Database As Database, ByVal transID As String) As CashPointRow
            Dim reader As SqlDataReader = Nothing
            Dim result As CashPointRow = Nothing
            Try
                reader = _Database.GetReader("Select * from CashPoint where TransactionNo='" & transID & "'")
                If Not reader Is Nothing Then
                    If reader.Read() Then
                        result = GetByDataReader(reader)
                    End If
                    Core.CloseReader(reader)
                End If
            Catch ex As Exception
                Core.CloseReader(reader)
                Components.Email.SendError("ToError500", "CashPoint.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return result
        End Function
        Public Shared Function AddPointProductReview(ByVal _Database As Database, ByVal MemberId As Integer, ByVal itemID As Integer, ByVal addPoint As Integer) As Boolean
            Dim result As Integer
            Try
                Dim sp As String = "sp_CashPoint_AddPointProductReview"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("MemberId", SqlDbType.Int, 0, MemberId))
                cmd.Parameters.Add(_Database.InParam("ItemId", SqlDbType.Int, 0, itemID))
                cmd.Parameters.Add(_Database.InParam("addPoint", SqlDbType.Int, 0, addPoint))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "CashPoint.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function
        Public Shared Function AddPointReview(ByVal _Database As Database, ByVal reviewId As Integer) As Boolean
            Dim result As Integer
            Try
                Dim sp As String = "sp_CashPoint_AddPointReview"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("ReviewId", SqlDbType.Int, 0, reviewId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "CashPoint.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function
        Public Shared Function AddPointSurveyResult(ByVal _Database As Database, ByVal SurveyResultId As Integer) As Boolean
            Dim result As Integer
            Try
                Dim sp As String = "sp_CashPoint_AddPointSurveyResult"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("SurveyResultId", SqlDbType.Int, 0, SurveyResultId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "CashPoint.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function
        Public Shared Function CheckTransactionNoExists(ByVal _Database As Database, ByVal MemberId As Integer, ByVal transNo As String) As Boolean
            Dim result As String = ""
            Dim sql As String = "Select TransactionNo from CashPoint where MemberId=" & MemberId & " and TransactionNo='" & transNo & "'"
            Try
                result = _Database.ExecuteScalar(sql)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "CashPoint.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            If result <> "" Then
                Return True
            End If
            Return False
        End Function
        Public Shared Function GetReportPoint(ByVal DB1 As Database, ByVal Month As Integer, ByVal Year As Integer) As DataTable
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp_CashPoint_Report As String = "sp_CashPoint_Report"
            Dim cmd As DbCommand = db.GetStoredProcCommand(sp_CashPoint_Report)
            db.AddInParameter(cmd, "Month", DbType.Int32, Month)
            db.AddInParameter(cmd, "Year", DbType.Int32, Year)
            Return db.ExecuteDataSet(cmd).Tables(0)
        End Function
        Public Shared Function GetTotalPoint(ByVal Month As Integer, ByVal Year As Integer) As CashPointCollection
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_CashPoint_Total"
            Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
            db.AddInParameter(cmd, "Month", DbType.Int32, Month)
            db.AddInParameter(cmd, "Year", DbType.Int32, Year)
            db.AddOutParameter(cmd, "TTPointAvailable", DbType.Int32, 1)
            db.AddOutParameter(cmd, "TTPointPending", DbType.Int32, 1)
            db.AddOutParameter(cmd, "TTPointDebit", DbType.Int32, 1)
            db.AddOutParameter(cmd, "TTPointAvailableInMonth", DbType.Int32, 1)
            db.AddOutParameter(cmd, "TTPointAvailableInYear", DbType.Int32, 1)
            db.AddOutParameter(cmd, "TTPointDebitInMonth", DbType.Int32, 1)
            db.AddOutParameter(cmd, "TTPointEarnedUptodate", DbType.Int32, 1)
            '' Dim reader As SqlDataReader= db.ExecuteReader(cmd))
            db.ExecuteNonQuery(cmd)
            Dim CaI As New CashPointCollection
            CaI.TTPointAvailable = CInt(cmd.Parameters("@TTPointAvailable").Value)
            CaI.TTPointPending = CInt(cmd.Parameters("@TTPointPending").Value)
            CaI.TTPointDebit = CInt(cmd.Parameters("@TTPointDebit").Value)
            CaI.TTPointAvailableInMonth = CInt(cmd.Parameters("@TTPointAvailableInMonth").Value)
            CaI.TTPointAvailableInYear = CInt(cmd.Parameters("@TTPointAvailableInYear").Value)
            CaI.TTPointDebitInMonth = CInt(cmd.Parameters("@TTPointDebitInMonth").Value)
            CaI.TTPointEarnedUptodate = CInt(cmd.Parameters("@TTPointEarnedUptodate").Value)
            Return CaI
        End Function

        Public Shared Function GetReportPoint1(ByVal Month As Integer, ByVal Year As Integer, ByVal CurrentPage As Integer, ByVal PageSize As Integer, ByVal SortBy As String, ByVal sortExp As String, ByRef total As Integer) As CashPointCollection

            Dim CaI As New CashPointCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_CashPoint_ReportV2"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "Month", DbType.Int32, Month)
                db.AddInParameter(cmd, "Year", DbType.Int32, Year)
                db.AddInParameter(cmd, "OrderBy", DbType.String, SortBy)
                db.AddInParameter(cmd, "OrderDirection", DbType.String, sortExp)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, CurrentPage)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, PageSize)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int32, 1)
                dr = db.ExecuteReader(cmd)
                While dr.Read
                    Dim cp As New CashPointRow()
                    cp.MemberId = CInt(dr("MemberId"))
                    cp.Email = IIf(IsDBNull(dr("Email")) = True, "", dr("Email"))
                    cp.FirstName = IIf(IsDBNull(dr("FirstName")) = True, "", dr("FirstName"))
                    cp.LastName = IIf(IsDBNull(dr("LastName")) = True, "", dr("LastName"))
                    cp.Address1 = IIf(IsDBNull(dr("Address1")) = True, "", dr("Address1"))
                    cp.Address2 = IIf(IsDBNull(dr("Address2")) = True, "", dr("Address2"))
                    cp.City = IIf(IsDBNull(dr("City")) = True, "", dr("City"))
                    cp.State = IIf(IsDBNull(dr("State")) = True, "", dr("State"))
                    cp.Country = IIf(IsDBNull(dr("Country")) = True, "", dr("Country"))
                    cp.Zip = IIf(IsDBNull(dr("Zip")) = True, "", dr("Zip"))
                    If IsDBNull(dr("TotalPointAvailable")) = False Then
                        cp.TotalPointAvailable = CInt(dr("TotalPointAvailable"))
                    Else
                        cp.TotalPointAvailable = 0
                    End If
                    cp.Worth = dr("Worth")
                    If IsDBNull(dr("PointPending")) = False Then
                        cp.PointPending = CInt(dr("PointPending"))
                    Else
                        cp.PointPending = 0
                    End If
                    If IsDBNull(dr("PointDebitinMonth")) = False Then
                        cp.PointDebitinMonth = CInt(dr("PointDebitinMonth"))
                    Else
                        cp.PointDebitinMonth = 0
                    End If
                    If IsDBNull(dr("PointsaccumulatedinMonth")) = False Then
                        cp.PointDebitinMonth = CInt(dr("PointsaccumulatedinMonth"))
                    Else
                        cp.PointDebitinMonth = 0
                    End If
                    If IsDBNull(dr("PointsaccumulatedinYear")) = False Then
                        cp.PointsaccumulatedinYear = CInt(dr("PointsaccumulatedinYear"))
                    Else
                        cp.PointsaccumulatedinYear = 0
                    End If
                    If IsDBNull(dr("Pointsearneduptodate")) = False Then
                        cp.Pointsearneduptodate = CInt(dr("Pointsearneduptodate"))
                    Else
                        cp.Pointsearneduptodate = 0
                    End If
                    If IsDBNull(dr("Pointsdebituptodate")) = False Then
                        cp.Pointsdebituptodate = CInt(dr("Pointsdebituptodate"))
                    Else
                        cp.Pointsdebituptodate = 0
                    End If
                    CaI.Add(cp)
                End While

                Core.CloseReader(dr)
                total = CInt(cmd.Parameters("@TotalRecords").Value)
                Return CaI
            Catch ex As Exception
                Core.CloseReader(dr)
                Components.Email.SendError("ToError500", "CashPoint.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return CaI
        End Function
        Public Shared Function SetValueCashPoint(ByVal cp As CashPointRow, ByVal o As StoreOrderRow, ByVal AdminId As Integer, ByVal cStatus As Integer) As CashPointRow
            cp.MemberId = o.MemberId
            cp.OrderId = o.OrderId
            cp.TransactionNo = "RO" & o.OrderNo
            cp.PointEarned = SysParam.GetValue("OrderReviewPoint")
            cp.Notes = "Order Review: #" & o.OrderNo
            cp.Status = cStatus
            cp.CreateDate = DateTime.Now
            If cp.Status = 1 Then
                cp.ApproveDate = DateTime.Now
            End If
            cp.AdminId = AdminId
            Return cp
        End Function
    End Class
    Public MustInherit Class CashPointRowBase
        Private m_DB As Database
        Private m_CashPointId As Integer = Nothing
        Private m_MemberId As Integer = Nothing
        Private m_OrderId As Integer = Nothing
        Private m_TransactionNo As String = Nothing
        Private m_PointEarned As Integer = Nothing
        Private m_Status As Integer = Nothing
        Private m_PointDebit As Integer = Nothing
        Private m_Notes As String = Nothing
        Private m_CreateDate As DateTime = Nothing
        Private m_ModifyDate As DateTime = Nothing
        Private m_ApproveDate As DateTime = Nothing
        Private m_AdminId As Integer = Nothing
        Private m_Amount As Double = Nothing
        Private m_Email As String = Nothing
        Private m_FirstName As String = Nothing
        Private m_LastName As String = Nothing
        Private m_Address1 As String = Nothing
        Private m_Address2 As String = Nothing
        Private m_City As String = Nothing
        Private m_State As String = Nothing
        Private m_Country As String = Nothing
        Private m_Zip As String = Nothing
        Private m_TotalPointAvailable As Integer = Nothing
        Private m_Worth As String = Nothing
        Private m_PointPending As Integer = Nothing
        Private m_PointDebitinMonth As Integer = Nothing
        Private m_PointsaccumulatedinMonth As Integer = Nothing
        Private m_PointsaccumulatedinYear As Integer = Nothing
        Private m_Pointsearneduptodate As Integer = Nothing
        Private m_Pointsdebituptodate As Integer = Nothing
        Public Shared cachePrefixKey As String = "CashPoint_"
        Public Property CashPointId() As Integer
            Get
                Return m_CashPointId
            End Get
            Set(ByVal value As Integer)
                m_CashPointId = value
            End Set
        End Property
        Public Property MemberId() As Integer
            Get
                Return m_MemberId
            End Get
            Set(ByVal value As Integer)
                m_MemberId = value
            End Set
        End Property
        Public Property OrderId() As Integer
            Get
                Return m_OrderId
            End Get
            Set(ByVal value As Integer)
                m_OrderId = value
            End Set
        End Property
        Public Property TransactionNo() As String
            Get
                Return m_TransactionNo
            End Get
            Set(ByVal value As String)
                m_TransactionNo = value
            End Set
        End Property
        Public Property PointEarned() As Integer
            Get
                Return m_PointEarned
            End Get
            Set(ByVal value As Integer)
                m_PointEarned = value
            End Set
        End Property

        Public Property Status() As Integer
            Get
                Return m_Status
            End Get
            Set(ByVal value As Integer)
                m_Status = value
            End Set
        End Property

        Public Property PointDebit() As Integer
            Get
                Return m_PointDebit
            End Get
            Set(ByVal value As Integer)
                m_PointDebit = value
            End Set
        End Property
        Public Property Notes() As String
            Get
                Return m_Notes
            End Get
            Set(ByVal Value As String)
                m_Notes = Value
            End Set
        End Property

        Public Property CreateDate() As DateTime
            Get
                Return m_CreateDate
            End Get
            Set(ByVal value As DateTime)
                m_CreateDate = value
            End Set
        End Property

        Public Property ModifyDate() As DateTime
            Get
                Return m_ModifyDate
            End Get
            Set(ByVal value As DateTime)
                m_ModifyDate = value
            End Set
        End Property
        Public Property ApproveDate() As DateTime
            Get
                Return m_ApproveDate
            End Get
            Set(ByVal value As DateTime)
                m_ApproveDate = value
            End Set
        End Property
        Public Property AdminId() As Integer
            Get
                Return m_AdminId
            End Get
            Set(ByVal value As Integer)
                m_AdminId = value
            End Set
        End Property

        Public Property Email() As String
            Get
                Return m_Email
            End Get
            Set(ByVal value As String)
                m_Email = value
            End Set
        End Property

        Public Property FirstName() As String
            Get
                Return m_FirstName
            End Get
            Set(ByVal value As String)
                m_FirstName = value
            End Set
        End Property

        Public Property LastName() As String
            Get
                Return m_LastName
            End Get
            Set(ByVal value As String)
                m_LastName = value
            End Set
        End Property

        Public Property Address1() As String
            Get
                Return m_Address1
            End Get
            Set(ByVal value As String)
                m_Address1 = value
            End Set
        End Property

        Public Property Address2() As String
            Get
                Return m_Address2
            End Get
            Set(ByVal value As String)
                m_Address2 = value
            End Set
        End Property

        Public Property City() As String
            Get
                Return m_City
            End Get
            Set(ByVal value As String)
                m_City = value
            End Set
        End Property

        Public Property State() As String
            Get
                Return m_State
            End Get
            Set(ByVal value As String)
                m_State = value
            End Set
        End Property

        Public Property Country() As String
            Get
                Return m_Country
            End Get
            Set(ByVal value As String)
                m_Country = value
            End Set
        End Property

        Public Property Zip() As String
            Get
                Return m_Zip
            End Get
            Set(ByVal value As String)
                m_Zip = value
            End Set
        End Property
        Public Property TotalPointAvailable() As Integer
            Get
                Return m_TotalPointAvailable
            End Get
            Set(ByVal value As Integer)
                m_TotalPointAvailable = value
            End Set
        End Property
        Public Property Worth() As String
            Get
                Return m_Worth
            End Get
            Set(ByVal value As String)
                m_Worth = value
            End Set
        End Property
        Public Property PointPending() As Integer
            Get
                Return m_PointPending
            End Get
            Set(ByVal value As Integer)
                m_PointPending = value
            End Set
        End Property
        Public Property PointDebitinMonth() As Integer
            Get
                Return m_PointDebitinMonth
            End Get
            Set(ByVal value As Integer)
                m_PointDebitinMonth = value
            End Set
        End Property
        Public Property PointsaccumulatedinMonth() As Integer
            Get
                Return m_PointsaccumulatedinMonth
            End Get
            Set(ByVal value As Integer)
                m_PointsaccumulatedinMonth = value
            End Set
        End Property
        Public Property PointsaccumulatedinYear() As Integer
            Get
                Return m_PointsaccumulatedinYear
            End Get
            Set(ByVal value As Integer)
                m_PointsaccumulatedinYear = value
            End Set
        End Property
        Public Property Pointsearneduptodate() As Integer
            Get
                Return m_Pointsearneduptodate
            End Get
            Set(ByVal value As Integer)
                m_Pointsearneduptodate = value
            End Set
        End Property
        Public Property Pointsdebituptodate() As Integer
            Get
                Return m_Pointsdebituptodate
            End Get
            Set(ByVal value As Integer)
                m_Pointsdebituptodate = value
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
        Public Property Amount() As Double
            Get
                Return m_Amount
            End Get
            Set(ByVal value As Double)
                m_Amount = value
            End Set
        End Property
        Public Sub New()
        End Sub 'New
        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New
        Public Sub New(ByVal database As Database, ByVal Type As String, ByVal Id As Integer)
            m_DB = database
            Type = ""
            Id = 0
        End Sub 'New
        Public Shared Function GetRow(ByVal _Database As Database, ByVal Type As String, ByVal Id As Integer) As CashPointRow
            Dim row As CashPointRow

            row = New CashPointRow(_Database, Type, Id)
            row.LoadByType(Type, Id)

            Return row
        End Function
        Protected Overridable Sub LoadByType(ByVal Type As String, ByVal Id As Integer)
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String = ""
                If Type = "M" Then
                    SQL = "SELECT * FROM CashPoint WHERE MemberId = " & Id
                ElseIf Type = "O" Then
                    SQL = "SELECT * FROM CashPoint WHERE OrderId = " & Id
                ElseIf Type = "C" Then
                    SQL = "SELECT * FROM CashPoint WHERE CashPointId = " & CashPointId
                End If
                r = m_DB.GetReader(SQL)
                If Not r Is Nothing Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "CashPoint.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub
        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                If IsDBNull(r.Item("OrderId")) Then
                    m_OrderId = 0
                Else
                    m_OrderId = Convert.ToInt32(r.Item("OrderId"))
                End If

                If IsDBNull(r.Item("MemberId")) Then
                    m_MemberId = 0
                Else
                    m_MemberId = Convert.ToInt32(r.Item("MemberId"))
                End If
                If IsDBNull(r.Item("CashPointId")) Then
                    m_CashPointId = 0
                Else
                    m_CashPointId = Convert.ToInt32(r.Item("CashPointId"))
                End If
                If IsDBNull(r.Item("TransactionNo")) Then
                    m_TransactionNo = Nothing
                Else
                    m_TransactionNo = Convert.ToString(r.Item("TransactionNo"))
                End If
                If IsDBNull(r.Item("PointEarned")) Then
                    m_PointEarned = 0
                Else
                    m_PointEarned = Convert.ToInt32(r.Item("PointEarned"))
                End If
                If IsDBNull(r.Item("PointDebit")) Then
                    m_PointDebit = 0
                Else
                    m_PointDebit = Convert.ToInt32(r.Item("PointDebit"))
                End If

                If IsDBNull(r.Item("Notes")) Then
                    m_Notes = Nothing
                Else
                    m_Notes = Convert.ToString(r.Item("Notes"))
                End If
                If IsDBNull(r.Item("Status")) Then
                    m_Status = Nothing
                Else
                    m_Status = Convert.ToInt32(r.Item("Status"))
                End If
                If IsDBNull(r.Item("CreateDate")) Then
                    m_CreateDate = Nothing
                Else
                    m_CreateDate = Convert.ToDateTime(r.Item("CreateDate"))
                End If
                If IsDBNull(r.Item("ModifyDate")) Then
                    m_ModifyDate = Nothing
                Else
                    m_ModifyDate = Convert.ToDateTime(r.Item("ModifyDate"))
                End If
                If IsDBNull(r.Item("ApproveDate")) Then
                    m_ApproveDate = Nothing
                Else
                    m_ApproveDate = Convert.ToDateTime(r.Item("ApproveDate"))
                End If
                If IsDBNull(r.Item("AdminId")) Then
                    m_AdminId = Nothing
                Else
                    m_AdminId = Convert.ToString(r.Item("AdminId"))
                End If
                'If IsDBNull(r.Item("Amount")) Then
                '    m_Amount = Nothing
                'Else
                '    m_Amount = Convert.ToDouble(r.Item("Amount"))
                'End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
        Public Overridable Sub Insert()
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_CashPoint_Insert"
                Dim cmd As SqlCommand = DB.CreateCommand(sp)
                cmd.Parameters.Add(DB.InParam("MemberId", SqlDbType.Int, 0, MemberId))
                If (OrderId > 0) Then
                    cmd.Parameters.Add(DB.InParam("OrderId", SqlDbType.Int, 0, OrderId))
                End If
                cmd.Parameters.Add(DB.InParam("TransactionNo", SqlDbType.VarChar, 0, TransactionNo))
                If (PointEarned > 0) Then
                    cmd.Parameters.Add(DB.InParam("PointEarned", SqlDbType.Int, 0, PointEarned))
                End If
                If (PointDebit > 0) Then
                    cmd.Parameters.Add(DB.InParam("PointDebit", SqlDbType.Int, 0, PointDebit))
                End If
                cmd.Parameters.Add(DB.InParam("Status", SqlDbType.Int, 0, Status))
                cmd.Parameters.Add(DB.InParam("Notes", SqlDbType.NVarChar, 0, Notes))
                cmd.Parameters.Add(DB.InParam("CreateDate", SqlDbType.DateTime, 0, Param.ObjectToDB(CreateDate)))
                If Status = 1 Then
                    cmd.Parameters.Add(DB.InParam("ApproveDate", SqlDbType.DateTime, 0, Param.ObjectToDB(ApproveDate)))
                End If
                cmd.Parameters.Add(DB.InParam("AdminId", SqlDbType.Int, 0, AdminId))
                cmd.Parameters.Add(DB.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
                If (result = 2) Then
                    CacheUtils.ClearCacheWithPrefix(StoreItemRow.cachePrefixKey)
                End If
                CacheUtils.ClearCacheWithPrefix(MemberRow.cachePrefixKey)
            Catch ex As Exception
            End Try

        End Sub

        Public Overridable Sub InsertIPN()
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_CashPoint_InsertIPN"
                Dim cmd As SqlCommand = DB.CreateCommand(sp)
                cmd.Parameters.Add(DB.InParam("MemberId", SqlDbType.Int, 0, MemberId))
                If (OrderId > 0) Then
                    cmd.Parameters.Add(DB.InParam("OrderId", SqlDbType.Int, 0, OrderId))
                End If
                cmd.Parameters.Add(DB.InParam("TransactionNo", SqlDbType.VarChar, 0, TransactionNo))
                If (PointEarned > 0) Then
                    cmd.Parameters.Add(DB.InParam("PointEarned", SqlDbType.Int, 0, PointEarned))
                End If
                If (PointDebit > 0) Then
                    cmd.Parameters.Add(DB.InParam("PointDebit", SqlDbType.Int, 0, PointDebit))
                End If
                cmd.Parameters.Add(DB.InParam("Status", SqlDbType.Int, 0, Status))
                cmd.Parameters.Add(DB.InParam("Notes", SqlDbType.NVarChar, 0, Notes))
                cmd.Parameters.Add(DB.InParam("CreateDate", SqlDbType.DateTime, 0, Param.ObjectToDB(CreateDate)))
                If Status = 1 Then
                    cmd.Parameters.Add(DB.InParam("ApproveDate", SqlDbType.DateTime, 0, Param.ObjectToDB(ApproveDate)))
                End If
                cmd.Parameters.Add(DB.InParam("AdminId", SqlDbType.Int, 0, AdminId))
                cmd.Parameters.Add(DB.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
                CacheUtils.ClearCacheWithPrefix(MemberRow.cachePrefixKey)
            Catch ex As Exception
                Dim str As String = ex.ToString()
            End Try

        End Sub

        Public Overloads Sub Update()
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_CashPoint_Update"
                Dim cm As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cm, "CashPointId", DbType.Int32, CashPointId)
                db.AddInParameter(cm, "MemberId", DbType.Int32, MemberId)
                If OrderId > 0 Then
                    db.AddInParameter(cm, "OrderId", DbType.Int32, OrderId)
                End If
                db.AddInParameter(cm, "TransactionNo", DbType.String, TransactionNo)
                If PointEarned > 0 Then
                    db.AddInParameter(cm, "PointEarned", DbType.Int32, PointEarned)
                End If
                db.AddInParameter(cm, "Status", DbType.Int32, Status)
                If PointDebit > 0 Then
                    db.AddInParameter(cm, "PointDebit", DbType.Int32, PointDebit)
                End If
                db.AddInParameter(cm, "Notes", DbType.String, Notes)
                db.AddInParameter(cm, "ModifyDate", DbType.DateTime, ModifyDate)
                If Status = 1 Then
                    db.AddInParameter(cm, "ApproveDate", DbType.DateTime, ApproveDate)
                Else
                    db.AddInParameter(cm, "ApproveDate", DbType.DateTime, Nothing)
                End If
                db.AddInParameter(cm, "AdminId", DbType.String, AdminId)
                db.AddParameter(cm, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cm)
                Dim result As Integer = Convert.ToInt32(db.GetParameterValue(cm, "return_value"))
                If (result = 2) Then
                    CacheUtils.ClearCacheWithPrefix(StoreItemRow.cachePrefixKey)
                End If
                CacheUtils.ClearCacheWithPrefix(MemberRow.cachePrefixKey)
            Catch ex As Exception

            End Try

        End Sub

    End Class
    Public Class CashPointCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal data As CashPointRow)
            Me.List.Add(data)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As CashPointRow
            Get
                Return CType(Me.List.Item(Index), CashPointRow)
            End Get

            Set(ByVal Value As CashPointRow)
                Me.List(Index) = Value
            End Set
        End Property
        Private m_TTPointAvailable As Integer = Nothing
        Private m_TTPointPending As Integer = Nothing
        Private m_TTPointDebit As Integer = Nothing
        Private m_TTPointAvailableInMonth As Integer = Nothing
        Private m_TTPointAvailableInYear As Integer = Nothing
        Private m_TTPointDebitInMonth As Integer = Nothing
        Private m_TTPointEarnedUptodate As Integer = Nothing
        Private m_TotalRecords As Integer = Nothing
        Public Property TTPointAvailable() As Integer
            Get
                Return m_TTPointAvailable
            End Get
            Set(ByVal value As Integer)
                m_TTPointAvailable = value
            End Set
        End Property

        Public Property TTPointPending() As Integer
            Get
                Return m_TTPointPending
            End Get
            Set(ByVal value As Integer)
                m_TTPointPending = value
            End Set
        End Property

        Public Property TTPointDebit() As Integer
            Get
                Return m_TTPointDebit
            End Get
            Set(ByVal value As Integer)
                m_TTPointDebit = value
            End Set
        End Property

        Public Property TTPointAvailableInMonth() As Integer
            Get
                Return m_TTPointAvailableInMonth
            End Get
            Set(ByVal value As Integer)
                m_TTPointAvailableInMonth = value
            End Set
        End Property

        Public Property TTPointAvailableInYear() As Integer
            Get
                Return m_TTPointAvailableInYear
            End Get
            Set(ByVal value As Integer)
                m_TTPointAvailableInYear = value
            End Set
        End Property

        Public Property TTPointDebitInMonth() As Integer
            Get
                Return m_TTPointDebitInMonth
            End Get
            Set(ByVal value As Integer)
                m_TTPointDebitInMonth = value
            End Set
        End Property

        Public Property TTPointEarnedUptodate() As Integer
            Get
                Return m_TTPointEarnedUptodate
            End Get
            Set(ByVal value As Integer)
                m_TTPointEarnedUptodate = value
            End Set
        End Property

        Public Property TotalRecords() As Integer
            Get
                Return m_TotalRecords
            End Get
            Set(ByVal value As Integer)
                m_TotalRecords = value
            End Set
        End Property
    End Class
End Namespace

