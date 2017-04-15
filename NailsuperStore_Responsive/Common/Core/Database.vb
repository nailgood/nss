Imports System
Imports System.ComponentModel
Imports System.Collections
Imports System.Diagnostics
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Components
Imports System.Reflection

Public Class Database
    Private con As SqlConnection = Nothing
    Private tran As SqlTransaction = Nothing
    Private RefCount As Integer = 0

    Public Sub New()
    End Sub

    Public Sub New(ByVal connection As SqlConnection, ByVal transaction As SqlTransaction)
        con = connection
        tran = transaction
    End Sub

    Public Property Connection() As SqlConnection
        Get
            Return con
        End Get
        Set(ByVal value As SqlConnection)
            con = value
        End Set
    End Property

    Public Property Transaction() As SqlTransaction
        Get
            Return tran
        End Get
        Set(ByVal value As SqlTransaction)
            tran = value
        End Set
    End Property

    Public Sub Open(ByVal connectionstring As String)
        RefCount = RefCount + 1

        If con Is Nothing Then
            con = New SqlConnection(connectionstring)
        End If
        If Not IsOpen() Then
            con.Open()
        End If
    End Sub

    Public Sub Close()
        If Not con Is Nothing Then
            RefCount = RefCount - 1
            If RefCount = 0 Then
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
                con = Nothing
            End If
        End If
    End Sub

    Public Sub Dispose()
        RefCount = 0
        If Not con Is Nothing Then
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
            con = Nothing
        End If
    End Sub

    Public Function IsOpen() As Boolean
        If Not con Is Nothing Then
            If con.State = ConnectionState.Open Then
                Return True
            End If
        End If
        Return False
    End Function
    Public Function PreventSQLInjection(ByVal sql As String) As String
        Try
            If (String.IsNullOrEmpty(sql)) Then
                Return String.Empty
            End If
            sql = sql.Replace("execute", "")
            sql = sql.Replace("exec", "")
            sql = sql.Replace("drop", "")
            sql = sql.Replace("delete", "")
            sql = sql.Replace("select", "")
            sql = sql.Replace("insert", "")
            Return sql
        Catch ex As Exception

        End Try
        Return String.Empty
    End Function
    Public Shared Function Quote(ByVal input As String) As String
        If IsEmpty(input) Then
            Return "NULL"
        Else
            Return "'" + input.Trim().Replace("'", "''") + "'"
        End If
    End Function

    Public Function NQuote(ByVal input As String) As String
        If IsEmpty(input) Then
            Return "NULL"
        Else
            Return "N'" + input.Trim().Replace("'", "''") + "'"
        End If
    End Function

    Public Shared Function Number(ByVal input As String) As String
        If IsEmpty(input) Then
            Return "NULL"
        Else
            Try
                Return CDbl(input)
            Catch ex As Exception
                Return "NULL"
            End Try
        End If
    End Function

    Public Shared Function NullNumber(ByVal input As Integer) As String
        If IsEmpty(input) Then
            Return "NULL"
        Else
            If input = 0 Then
                Return "NULL"
            Else
                Return input
            End If
        End If
    End Function

    Public Function NullDouble(ByVal input As Double) As String
        If IsEmpty(input) Then
            Return "NULL"
        Else
            If input = 0 Then
                Return "NULL"
            Else
                Return input
            End If
        End If
    End Function

    Public Function NullQuote(ByVal input As Date) As String
        If input = Nothing Then
            Return "NULL"
        Else
            Return Quote(input.ToString())
        End If
    End Function

    Public Function NullQuote(ByVal input As Integer) As String
        If input = 0 Then
            Return "NULL"
        Else
            Return Quote(input.ToString())
        End If
    End Function

    Public Function Quote(ByVal input As DateTime) As String
        If input = DateTime.MinValue Then
            Return "NULL"
        Else
            Return Quote(input.ToString())
        End If
    End Function

    Public Function NullDate(ByVal input As DateTime) As Date
        Try
            Return DateTime.Parse(input)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function QuoteMultiple(ByVal input As String) As String
        If IsEmpty(input) Then
            Return "(NULL)"
        Else
            Dim aMultiple() As String = input.Split(","c)
            Dim i As Integer
            For i = 0 To aMultiple.Length - 1 Step i + 1
                aMultiple(i) = Quote(aMultiple(i))
            Next
            Return "(" + String.Join(",", aMultiple) + ")"
        End If
    End Function

    Public Function NQuoteMultiple(ByVal input As String) As String
        If IsEmpty(input) Then
            Return "(NULL)"
        Else
            Dim aMultiple() As String = input.Split(","c)
            Dim i As Integer
            For i = 0 To aMultiple.Length - 1 Step i + 1
                aMultiple(i) = NQuote(aMultiple(i))
            Next
            Return "(" + String.Join(",", aMultiple) + ")"
        End If
    End Function

    Public Function NumberMultiple(ByVal input As String) As String
        Return QuoteMultiple(input)
    End Function

    Public Function FilterQuote(ByVal input As String) As String
        If IsEmpty(input) Then
            Return "NULL"
        Else
            Return "'%" + input.Trim().Replace("'", "''") + "%'"
        End If
    End Function

    Public Shared Function IsEmpty(ByVal input As String) As Boolean
        If input = String.Empty Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub BeginTransaction()
        If tran Is Nothing Then
            tran = con.BeginTransaction()
        End If
    End Sub

    Public Sub CommitTransaction()
        If Not tran Is Nothing Then
            tran.Commit()
            tran = Nothing
        End If
    End Sub

    Public Sub RollbackTransaction()
        If Not tran Is Nothing Then
            tran.Rollback()
            tran = Nothing
        End If
    End Sub

    Public Function RunProc(ByVal procName As String) As Integer
        Dim cmd As SqlCommand = CreateCommand(procName, Nothing)
        cmd.ExecuteNonQuery()
        Return CType(cmd.Parameters("ReturnValue").Value, Integer)
    End Function

    Public Function RunProc(ByVal procName As String, ByVal prams() As SqlParameter) As Integer
        Dim cmd As SqlCommand = CreateCommand(procName, prams)
        cmd.ExecuteNonQuery()
        Return CType(cmd.Parameters("ReturnValue").Value, Integer)
    End Function

    Public Sub RunProc(ByVal procName As String, ByRef dataReader As SqlDataReader)
        Dim cmd As SqlCommand = CreateCommand(procName, Nothing)
        dataReader = cmd.ExecuteReader()
    End Sub

    Public Sub RunProc(ByVal procName As String, ByVal prams() As SqlParameter, ByRef dataReader As SqlDataReader)
        Dim cmd As SqlCommand = CreateCommand(procName, prams)
        dataReader = cmd.ExecuteReader()
    End Sub

    Public Sub RunProc(ByVal procName As String, ByRef dsDataSet As DataSet)
        Dim cmd As SqlCommand = CreateCommand(procName, Nothing)
        Dim daDataAdapter As SqlDataAdapter = New SqlDataAdapter(cmd)
        dsDataSet = New DataSet()
        daDataAdapter.Fill(dsDataSet)
    End Sub

    Public Sub RunProc(ByVal procName As String, ByVal prams() As SqlParameter, ByRef dsDataSet As DataSet)
        Dim cmd As SqlCommand = CreateCommand(procName, prams)
        Dim daDataAdapter As SqlDataAdapter = New SqlDataAdapter(cmd)
        dsDataSet = New DataSet()
        daDataAdapter.Fill(dsDataSet)
    End Sub

    Public Function GetReader(ByVal SQL As String) As SqlDataReader
        If System.Web.HttpContext.Current.Session("SQL") IsNot Nothing Then
            System.Web.HttpContext.Current.Session("SQLOld") = System.Web.HttpContext.Current.Session("SQL")
        End If

        SetSQLToSession(SQL)

        If Not IsOpen() Then
            con.Open()
        End If

        Dim cmd As SqlCommand
        Dim myReader As SqlDataReader = Nothing
        If Not tran Is Nothing Then
            cmd = New SqlCommand(SQL, con, tran)
        Else
            cmd = New SqlCommand(SQL, con)
        End If

        Try
            myReader = cmd.ExecuteReader()
        Catch ex As Exception
            Dim url As String = System.Web.HttpContext.Current.Request.Url.ToString()
            Email.SendError("ToError500", "Database.vb", "Url: " & url & "<br><br>SQL: " & SQL & "<br><br>Function: GetReader(ByVal SQL As String) As SqlDataReader" & "<br><br>Exception: " & ex.ToString())
        End Try

        Return myReader
    End Function

    Public Shared Function mapList(Of T As New)(ByVal dr As IDataReader) As List(Of T)
        Dim businessEntityType As Type = GetType(T)
        Dim entitys As New List(Of T)()
        Dim hashtable As New Hashtable()
        Dim properties As PropertyInfo() = businessEntityType.GetProperties()
        For Each info As PropertyInfo In properties
            hashtable(info.Name.ToUpper()) = info
        Next
        While dr.Read()
            Dim newObject As New T()
            For index As Integer = 0 To dr.FieldCount - 1
                Dim info As PropertyInfo = DirectCast(hashtable(dr.GetName(index).ToUpper()), PropertyInfo)
                If (info IsNot Nothing) AndAlso info.CanWrite Then
                    Try
                        If info.PropertyType.Name.Contains("Boolean") Then
                            If dr.GetValue(index) Is DBNull.Value Then
                                info.SetValue(newObject, False, Nothing)
                            Else
                                info.SetValue(newObject, CBool(dr.GetValue(index)), Nothing)
                            End If
                        ElseIf dr.GetValue(index) Is DBNull.Value Then
                            info.SetValue(newObject, Nothing, Nothing)
                        Else
                            info.SetValue(newObject, dr.GetValue(index), Nothing)
                        End If

                    Catch ex As Exception

                    End Try

                End If
            Next
            entitys.Add(newObject)
        End While
        dr.Close()
        Return entitys
    End Function

    Public Function ExecuteScalar(ByVal SQL As String) As Object
        Try
            If System.Web.HttpContext.Current.Session("SQL") IsNot Nothing Then
                System.Web.HttpContext.Current.Session("SQLOld") = System.Web.HttpContext.Current.Session("SQL")
            End If

            SetSQLToSession(SQL)

            If Not IsOpen() Then
                If con Is Nothing Then
                    con = New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
                End If

                con.Open()

            End If

            Dim cmd As SqlCommand
            If Not tran Is Nothing Then
                cmd = New SqlCommand(SQL, con, tran)
            Else
                cmd = New SqlCommand(SQL, con)
            End If
            Return cmd.ExecuteScalar()
        Catch ex As Exception
            Dim rawURL As String = String.Empty
            If Not System.Web.HttpContext.Current Is Nothing Then
                If Not System.Web.HttpContext.Current.Request Is Nothing Then
                    rawURL = System.Web.HttpContext.Current.Request.RawUrl
                End If
            End If
            Dim memberId As Integer = Utility.Common.GetCurrentMemberId()
            Dim orderID As Integer = Utility.Common.GetCurrentOrderId

            Components.Email.SendError("ToError500", "Database.vb-ExecuteScalar", "page:" & rawURL & "<br/>MemberId:" & memberId & "<br/>OrderId:=" & orderID & "<br/>SQL:=" & SQL & "<br>Exception: " & ex.ToString() + "<br>RefCount:" + RefCount.ToString() + "<br>SessionList: " & BaseShoppingCart.GetSessionList())
        End Try
        Return Nothing
    End Function
    Public Function GetDataSet2(ByVal SQL As String) As DataSet
        Dim ds As New DataSet
        ds.Tables.Add(GetDataTable(SQL))
        Return ds
    End Function
    Private Sub SetSQLToSession(ByVal sql As String)
        If sql Is Nothing Then
            Exit Sub
        End If
        Try
            System.Web.HttpContext.Current.Session("SQL") = sql
        Catch ex As Exception

        End Try
    End Sub
    Public Function GetDataSet(ByVal SQL As String) As DataSet
        SetSQLToSession(SQL)
        Dim ds As DataSet = New DataSet()
        Dim da As New SqlDataAdapter()
        Dim cmd As SqlCommand
        If Not tran Is Nothing Then
            cmd = New SqlCommand(SQL, con, tran)
        Else
            cmd = New SqlCommand(SQL, con)
        End If
        cmd.CommandType = CommandType.Text

        Try
            da.SelectCommand = cmd
            da.Fill(ds)
        Catch ex As Exception
            Dim url As String = System.Web.HttpContext.Current.Request.Url.ToString()
            Email.SendError("ToError500", "Database.vb", "SQL: " & SQL & "<br><br>Function: GetDataSet(ByVal SQL As String) As DataSet<br><br>Url: " & url & "<br><br>Exception: " & ex.ToString())
        Finally
            'If cmd.Connection.State = ConnectionState.Open Then
            '    cmd.Connection.Close()
            'End If

            cmd.Dispose()
            da.Dispose()
        End Try

        Return ds
    End Function

    Public Function GetNewDataTable(ByVal cmd As SqlCommand, ByVal SQL As String)
        SetSQLToSession(SQL)
        If (cmd.Connection.State = ConnectionState.Open) Then
            cmd.Connection.Close()
        End If
        Try
            cmd.Connection.Open()
        Catch ex As Exception

        End Try

        Dim dt As DataTable = New DataTable()
        Dim da As New SqlDataAdapter()

        If con Is Nothing Then
            Open(ConfigurationManager.AppSettings("ConnectionString"))
        End If
        If Not tran Is Nothing Then
            cmd = New SqlCommand(SQL, con, tran)
        Else
            cmd = New SqlCommand(SQL, con)
        End If
        cmd.CommandType = CommandType.Text

        Try
            da.SelectCommand = cmd
            da.Fill(dt)
        Catch ex As Exception
            Email.SendError("ToError500", "Database.vb", "SQL: " & SQL & "<br><br>Function: GetNewDataTable(ByVal SQL As String) As DataTable" & "<br><br>Exception: " & ex.ToString())
        Finally
            cmd.Dispose()
            da.Dispose()
        End Try

        Return dt
    End Function

    Public Function GetDataTable(ByVal SQL As String) As DataTable
        If System.Web.HttpContext.Current.Session("SQL") IsNot Nothing Then
            System.Web.HttpContext.Current.Session("SQLOld") = System.Web.HttpContext.Current.Session("SQL")
        End If

        SetSQLToSession(SQL)

        If Not IsOpen() Then
            con.Open()
        End If

        Dim dt As New DataTable()
        Dim da As New SqlDataAdapter()
        Dim cmd As SqlCommand
        If Not tran Is Nothing Then
            cmd = New SqlCommand(SQL, con, tran)
        Else
            cmd = New SqlCommand(SQL, con)
        End If

        cmd.CommandType = CommandType.Text

        Try
            da.SelectCommand = cmd
            da.Fill(dt)
        Catch ex As Exception
            If (ex.Message.Contains("already an open DataReader associated") And tran Is Nothing) Then
                Return GetNewDataTable(cmd, SQL)
            Else
                Dim sTran As String
                If tran Is Nothing Then
                    sTran = "nothing"
                Else
                    sTran = tran.ToString()
                End If
                Email.SendError("ToError500", "Database.vb", "SQL: " & IIf(String.IsNullOrEmpty(SQL), "nothing", SQL) & "<br><br>Function: GetDataTable(ByVal SQL As String) As DataTable, transactionId: " & sTran & "<br><br>Exception: " & ex.ToString())
            End If
        Finally
            cmd.Dispose()
            da.Dispose()
        End Try

        Return dt
    End Function

    Public Function GetDataTable3(ByVal SQL As String) As DataTable
        SetSQLToSession(SQL)
        Trace.Write(SQL)

        Dim dt As New DataTable()
        Dim cmd As SqlCommand
        Dim dr As SqlDataReader = Nothing

        If Not tran Is Nothing Then
            cmd = New SqlCommand(SQL, con, tran)
        Else
            cmd = New SqlCommand(SQL, con)
        End If

        Try
            dr = cmd.ExecuteReader()
            dt.Load(dr)
            Core.CloseReader(dr)
        Catch ex As Exception
            Core.CloseReader(dr)
            Email.SendError("ToError500", "Database.vb", "SQL: " & SQL & "<br><br>Function: GetDataTable2(ByVal SQL As String) As DataTable" & "<br><br>Exception: " & ex.ToString())
        End Try

        Return dt
    End Function

    Public Function GetDataView(ByVal SQL As String) As DataView
        Try
            Return GetDataTable3(SQL).DefaultView
        Catch ex As Exception
            Email.SendError("ToError500", "Database.vb", "SQL: " & SQL & "<br>Url: " & Web.HttpContext.Current.Request.RawUrl)
            Return Nothing
        End Try
    End Function

    Public Function InsertSQL(ByVal SQL As String) As Integer
        Dim myReader As SqlDataReader = Nothing
        Try
            SQL = "SET NOCOUNT ON; " + SQL + "; SELECT @@IDENTITY AS NewId; SET NOCOUNT OFF;"
            Trace.Write(SQL)
            Dim cmd As SqlCommand
            If Not tran Is Nothing Then
                cmd = New SqlCommand(SQL, con, tran)
            Else
                cmd = New SqlCommand(SQL, con)
            End If
            Dim result As Integer
            myReader = cmd.ExecuteReader()
            If myReader.Read() Then
                result = Convert.ToInt32(myReader("NewId"))
            Else
                result = 0
            End If
            Core.CloseReader(myReader)
            Return result
        Catch ex As Exception
            Core.CloseReader(myReader)
            Dim rawURL As String = String.Empty
            If Not System.Web.HttpContext.Current Is Nothing Then
                If Not System.Web.HttpContext.Current.Request Is Nothing Then
                    rawURL = System.Web.HttpContext.Current.Request.RawUrl
                End If
            End If
            Dim memberId As Integer = Utility.Common.GetCurrentMemberId()
            Dim orderID As Integer = Utility.Common.GetCurrentOrderId
            Components.Email.SendError("ToError500", "Database.vb-InsertSQL", "page:" & rawURL & "<br/>MemberId:" & memberId & "<br/>OrderId:=" & orderID & "<br/>SQL:=" & SQL & "<br>Exception: " & ex.ToString() + "")

        End Try

    End Function

    Public Function ExecuteSQL(ByVal SQL As String) As Integer
        If String.IsNullOrEmpty(SQL) Then
            Return 0
        End If

        Try
            SetSQLToSession(SQL)
            Trace.Write(SQL)

            Dim cmd As SqlCommand
            If Not tran Is Nothing Then
                cmd = New SqlCommand(SQL, con, tran)
            Else
                cmd = New SqlCommand(SQL, con)
            End If
            Return cmd.ExecuteNonQuery()
        Catch ex As Exception
            '' Components.Email.SendError("ToError500", "Database.vb-ExecuteSQL", "SQL:=" & SQL & "<br>Exception: " & ex.ToString() + "")
            Dim rawURL As String = String.Empty
            If Not System.Web.HttpContext.Current Is Nothing Then
                If Not System.Web.HttpContext.Current.Request Is Nothing Then
                    rawURL = System.Web.HttpContext.Current.Request.RawUrl
                End If
            End If
            Dim memberId As Integer = Utility.Common.GetCurrentMemberId()
            Dim orderID As Integer = Utility.Common.GetCurrentOrderId
            Components.Email.SendError("ToError500", "Database.vb-ExecuteSQL(ByVal SQL As String)", "page: " & rawURL & "<br/>MemberId: " & memberId & "<br/>OrderId: " & orderID & "<br/>SQL: " & SQL & "<br>Exception: " & ex.ToString() + "" & BaseShoppingCart.GetSessionList())

        End Try

    End Function

    Public Function ExecuteSQL(ByVal SQL As String, ByVal Timeout As Integer) As Integer
        Try
            SetSQLToSession(SQL)
            Trace.Write(SQL)
            Dim cmd As SqlCommand
            If Not tran Is Nothing Then
                cmd = New SqlCommand(SQL, con, tran)
            Else
                cmd = New SqlCommand(SQL, con)
            End If
            cmd.CommandTimeout = Timeout
            Return cmd.ExecuteNonQuery()
        Catch ex As Exception
            ''Components.Email.SendError("ToError500", "Database.vb-ExecuteSQL", "SQL:=" & SQL & ",Timeout:" & Timeout & "<br>Exception: " & ex.ToString() + "")
            Dim rawURL As String = String.Empty
            If Not System.Web.HttpContext.Current Is Nothing Then
                If Not System.Web.HttpContext.Current.Request Is Nothing Then
                    rawURL = System.Web.HttpContext.Current.Request.RawUrl
                End If
            End If
            Dim memberId As Integer = Utility.Common.GetCurrentMemberId()
            Dim orderID As Integer = Utility.Common.GetCurrentOrderId
            Components.Email.SendError("ToError500", "Database.vb-ExecuteSQL(ByVal SQL As String, ByVal Timeout As Integer)", "page: " & rawURL & "<br/>MemberId: " & memberId & "<br/>OrderId: " & orderID & "<br/>SQL: " & SQL & "<br/>,Timeout:" & Timeout.ToString() & "<br>Exception: " & ex.ToString() + "" & BaseShoppingCart.GetSessionList())
        End Try
       
    End Function

    Private Function CreateCommand(ByVal procName As String, ByVal prams() As SqlParameter) As SqlCommand
        Dim cmd As SqlCommand
        If Not tran Is Nothing Then
            cmd = New SqlCommand(procName, con, tran)
        Else
            cmd = New SqlCommand(procName, con)
        End If
        cmd.CommandType = CommandType.StoredProcedure

        ' add proc parameters
        If Not prams Is Nothing Then
            Dim parameter As SqlParameter
            For Each parameter In prams
                cmd.Parameters.Add(parameter)
            Next
        End If

        cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, False, 0, 0, String.Empty, DataRowVersion.Default, Nothing))
        Return cmd
    End Function

    Public Function CreateCommand(ByVal procName As String) As SqlCommand
        Dim cmd As SqlCommand
        If Not tran Is Nothing Then
            cmd = New SqlCommand(procName, con, tran)
        Else
            cmd = New SqlCommand(procName, con)
        End If
        cmd.CommandType = CommandType.StoredProcedure
        Return cmd
    End Function

    Public Function InParam(ByVal ParamName As String, ByVal DbType As SqlDbType, ByVal Size As Integer, ByVal Value As Object) As SqlParameter
        Return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value)
    End Function
    Public Function OutParam(ByVal ParamName As String, ByVal DbType As SqlDbType, ByVal Size As Integer) As SqlParameter
        Return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, Nothing)
    End Function
    Public Function ReturnParam(ByVal ParamName As String, ByVal DbType As SqlDbType) As SqlParameter
        Return MakeParam(ParamName, DbType, ParameterDirection.ReturnValue, Nothing)
    End Function

    Public Function MakeParam(ByVal ParamName As String, ByVal DbType As SqlDbType, ByVal Size As Int32, ByVal Direction As ParameterDirection, ByVal Value As Object) As SqlParameter
        Dim param As SqlParameter

        If Size > 0 Then
            param = New SqlParameter(ParamName, DbType, Size)
        Else
            param = New SqlParameter(ParamName, DbType)
        End If

        param.Direction = Direction
        If Not (Direction = ParameterDirection.Output And Value Is Nothing) Then
            param.Value = Value
        End If
        Return param
    End Function
    Public Function MakeParam(ByVal ParamName As String, ByVal DbType As SqlDbType, ByVal Direction As ParameterDirection, ByVal Value As Object) As SqlParameter
        Dim param As New SqlParameter(ParamName, DbType)
        param.Direction = Direction
        If Not (Direction = ParameterDirection.Output And Value Is Nothing) Then
            param.Value = Value
        End If
        Return param
    End Function
End Class
