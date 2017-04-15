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
Imports System.Web

Namespace DataLayer

    Public Class MemberRow
        Inherits MemberRowBase

        Public Shared ReadOnly Property MemberNavigationString() As String
            Get
                Dim GlobalSecureName As String = System.Configuration.ConfigurationManager.AppSettings("GLobalSecureName")
                Return "<a href=""" & GlobalSecureName & "/members/"" class='noul'>My Account</a> | <a href=""" & GlobalSecureName & "/members/orderhistory/"" class='noul'>Order History</a> | <a href=""" & GlobalSecureName & "/members/creditmemo/"" class='noul'>Credit History</a> | <a href=""" & GlobalSecureName & "/members/wishlist/"" class='noul'>Wishlist</a> | <a href=""" & GlobalSecureName & "/members/addressbook/"" class='noul'>Address Book</a> | <a href=""" & GlobalSecureName & "/members/reminders/"" class='noul'>Reminders</a> | <a href=""" & GlobalSecureName & "/logout.aspx"" class='noul'>Logout</a>"
            End Get
        End Property

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal MemberId As Integer)
            MyBase.New(database, MemberId)
        End Sub 'New
        Public Sub New(ByVal MemberId As Integer)
            MyBase.New(MemberId)
        End Sub 'New
        'end 23/10/2009

        Public Shared Function MemberInGroupWHS(ByVal memberId As Integer) As Boolean
            Dim s As String = String.Empty
            Dim key As String = String.Format(MemberRow.cachePrefixKey & "MemberInGroupWHS_{0}", memberId)
            s = CType(CacheUtils.GetCache(key), String)

            If Not String.IsNullOrEmpty(s) Then
                Return CBool(s)
            Else
                Try
                    Dim sql As String = "SELECT COUNT(m.CustomerId) FROM Member m LEFT JOIN Customer c ON(c.CustomerId=m.CustomerId) where m.MemberId=" & memberId & " and c.CustomerPostingGroup='WHS'"
                    Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                    Dim cmd As DbCommand = db.GetSqlStringCommand(sql)
                    cmd.CommandType = CommandType.Text
                    s = db.ExecuteScalar(cmd).ToString()

                    CacheUtils.SetCache(key, s, Utility.ConfigData.TimeCacheMemberData)
                    Return CBool(s)
                Catch ex As Exception
                    Components.Email.SendError("ToError500", "MemberInGroupWHS", "MemberId=" & memberId & "<br>Exception: " & ex.ToString())
                    Return False
                End Try
            End If

        End Function

        Public Shared Function CheckMemberIsInternational(ByVal MemberId As Integer, ByVal orderid As Integer) As Boolean

            Dim result As Boolean = False
            If MemberId > 0 Then
                Try
                    If HttpContext.Current.Session("CheckMemberIsInternational") IsNot Nothing Then
                        result = CBool(HttpContext.Current.Session("CheckMemberIsInternational"))
                        Return result
                    End If

                    Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                    Dim sql As String = String.Empty
                    If orderid > 0 Then
                        sql = "Select [dbo].[fc_IsMemberInternational](" & orderid & "," & MemberId & ")"
                    Else
                        sql = "Select [dbo].[fc_IsMemberInternational](null," & MemberId & ")"
                    End If

                    Dim cmd As DbCommand = db.GetSqlStringCommand(sql)
                    cmd.CommandType = CommandType.Text
                    result = CInt(db.ExecuteScalar(cmd)) = 1

                Catch ex As Exception
                    Components.Email.SendError("ToError500", "IsInternational", "MemberId=" & MemberId & ",orderid=" & orderid & "<br>Exception: " & ex.ToString() + "")
                End Try

                HttpContext.Current.Session("CheckMemberIsInternational") = result
            End If

            Return result
        End Function
        Public Shared Function GetLastOrderId(ByVal MemberId As Integer) As Integer
            Dim result As Integer = Nothing
            If MemberId > 0 Then
                Try
                    Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                    Dim sql As String = "SELECT LastOrderId FROM Member WHERE MemberId=" & MemberId
                    Dim cmd As DbCommand = db.GetSqlStringCommand(sql)
                    cmd.CommandType = CommandType.Text
                    result = CInt(db.ExecuteScalar(cmd))
                Catch ex As Exception
                    Components.Email.SendError("ToError500", "GetLastOrderId", HttpContext.Current.Request.Url.ToString() & "<br>MemberId=" & MemberId & "<br>Exception: " & ex.ToString() + "")
                End Try
            End If

            Return result
        End Function

        Public Shared Function GetRow(ByVal MemberId As Integer) As MemberRow
            Dim row As MemberRow
            Dim key As String = String.Format(MemberRow.cachePrefixKey & "GetRow_{0}", MemberId)

            row = CType(CacheUtils.GetCache(key), MemberRow)
            If row Is Nothing Then
                row = New MemberRow
            Else
                row.IsInternational = CheckMemberIsInternational(row.MemberId, 0)
                row.LastOrderId = GetLastOrderId(row.MemberId)
                Return row
            End If

            row = New MemberRow(MemberId)
            row.Load()
            CacheUtils.SetCache(key, row, Utility.ConfigData.TimeCacheMemberData)
            Return row
        End Function

        Public Shared Function GetRowLogin(ByVal MemberId As Integer) As MemberRow
            Dim row As New MemberRow(MemberId)
            row.Load()

            Return row
        End Function

        Public Shared Function GetListPendingPoint(ByVal DB1 As Database, ByVal pIndex As Integer, ByVal pSize As Integer, ByVal condition As String, ByVal sortBy As String, ByVal sortExp As String, ByRef total As Integer) As DataTable

            Dim c As New MemberCollection
            Dim dsResult As New DataTable
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_Member_GetListPendingPoint"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "Condition", DbType.String, condition)
                db.AddInParameter(cmd, "OrderBy", DbType.String, sortBy)
                db.AddInParameter(cmd, "OrderDirection", DbType.String, sortExp)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, pIndex)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, pSize)
                Dim ds As DataSet = db.ExecuteDataSet(cmd)
                ''get total
                Try
                    total = ds.Tables(1).Rows(0)(0).ToString()
                Catch ex As Exception

                End Try
                ''get data source
                dsResult = ds.Tables(0)
            Catch ex As Exception
                total = 0
                dsResult = Nothing
            End Try
            Return dsResult
        End Function
        Public Shared Function GetListAdmin(ByVal pIndex As Integer, ByVal pSize As Integer, ByVal condition As String, ByVal sortBy As String, ByVal sortExp As String) As MemberCollection

            Dim c As New MemberCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP As String = "sp_Member_GetListAdmin"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP)
                db.AddInParameter(cmd, "Condition", DbType.String, condition)
                db.AddInParameter(cmd, "OrderBy", DbType.String, sortBy)
                db.AddInParameter(cmd, "OrderDirection", DbType.String, sortExp)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, pIndex)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, pSize)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int32, 1)
                'Dim ds As DataSet = db.ExecuteDataSet(cmd)
                dr = db.ExecuteReader(cmd)
                ''get total
                While dr.Read
                    Dim m As New MemberRow()
                    m.MemberId = CInt(dr("MemberId"))
                    m.ProfessionalStatus = IIf(IsDBNull(dr("MemberType")), Nothing, dr("MemberType"))
                    m.Username = dr("Username")
                    m.Country = dr("Country")
                    m.CreateDate = IIf(IsDBNull(dr("CreateDate")), Nothing, dr("CreateDate"))
                    m.LastLoginDate = IIf(IsDBNull(dr("LastLoginDate")), Nothing, dr("LastLoginDate"))
                    m.IsActive = dr("IsActive")
                    m.DeActive = IIf(IsDBNull(dr("DeActive")), Nothing, dr("DeActive"))
                    m.TotalPoint = IIf(IsDBNull(dr("TotalPoint")), 0, dr("TotalPoint"))
                    m.CustomerPostingGroup = dr("CustomerPostingGroup")
                    c.Add(m)
                End While
                Core.CloseReader(dr)
                c.TotalRecords = CInt(cmd.Parameters("@TotalRecords").Value)
                Return c

            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
            Return c
        End Function
        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal MemberId As Integer)
            Dim row As MemberRow

            row = New MemberRow(_Database, MemberId)
            row.Remove()
        End Sub
        Public Shared Function IsEbayCustomer(ByVal _Database As Database, ByVal memberId As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Member_IsEbayCustomer"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("MemberId", SqlDbType.Int, 0, memberId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
                If result = 1 Then
                    Return True
                End If
            Catch ex As Exception

            End Try
            Return False
        End Function

        Public Shared Function SetLastLoginDate(ByVal _Database As Database, ByVal memberId As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Member_UpdateLastLoginDate"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("MemberId", SqlDbType.Int, 0, memberId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)

                If result = 1 Then
                    Return True
                End If
            Catch ex As Exception

            End Try
            Return False

        End Function
        'Custom Methods
        Public Shared Function ValidateMemberCredentials(ByVal DB1 As Database, ByVal Username As String, ByVal Password As String) As Integer
            Dim Decrypted As String = String.Empty
            Dim MemberId As Integer = 0
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

                Dim SP_GETOBJECT As String = "sp_Member_ValidateMemberCredentials"

                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)

                db.AddInParameter(cmd, "Username", DbType.String, Username)

                reader = CType(db.ExecuteReader(cmd), SqlDataReader)

                If reader.Read() Then
                    MemberId = Convert.ToInt32(reader("MemberId"))
                    Decrypted = CryptData.Crypt.DecryptTripleDes(reader("Password"))
                End If
                Core.CloseReader(reader)
                If Decrypted = Password Then
                    Return MemberId
                ElseIf Decrypted = String.Empty Then
                    Return -1
                Else
                    Return 0
                End If
            Catch ex As Exception

            End Try
            Return 0
        End Function

        Public Shared Function GetRowByUsername(ByVal DB1 As Database, ByVal Username As String) As MemberRow
            Dim row As MemberRow = New MemberRow(DB1)
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_GETOBJECT As String = "sp_Member_GetObjectByUsername"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)
                db.AddInParameter(cmd, "Username", DbType.String, Username)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    row.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try
            Return row
        End Function
        Public Shared Function CheckIdExists(ByVal memberId As Integer) As Boolean
            Dim result As Boolean = False
            If memberId > 0 Then
                Try
                    Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

                    Dim count As Integer = db.ExecuteScalar(CommandType.Text, "SELECT MemberId FROM Member WHERE MemberId=" & memberId)
                    If count > 0 Then
                        result = True
                    End If
                Catch ex As Exception
                End Try
            End If

            Return result
        End Function

        Public Shared Function UpdateAfterSendOrderConfirmation(ByVal memberId As Integer) As Boolean
            Dim result As Boolean = False
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_Member_UpdateAfterSendOrderConfirmation"
            Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
            db.AddInParameter(cmd, "MemberId", DbType.Int32, memberId)
            result = CBool(db.ExecuteScalar(cmd))
            CacheUtils.RemoveCacheItemWithPrefix(cachePrefixKey)

            Return result 'True=Guest Checkout
        End Function

        'end 23/10/2009
        Public Shared Function GetRowByEmail(ByVal DB As Database, ByVal Email As String) As MemberRow
            Dim SQL As String = "SELECT * FROM Member WHERE CustomerId in (select customerid from customer where email = " & DB.Quote(Email) & ")"
            ''Dim Ds As DataSet = DB.GetDataSet(SQL)
            Dim r As SqlDataReader = Nothing
            Dim row As MemberRow = New MemberRow(DB)
            Try

                r = DB.GetReader(SQL)
                If r.Read Then
                    row.Load(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
            End Try
            Return row
        End Function
        Public Shared Function GetNameForSendMail(ByVal _Database As Database, ByVal MemberId As Integer) As String
            Dim result As String = String.Empty
            Dim dr As SqlDataReader = Nothing
            Try
                Dim sp As String = "sp_Member_GetNameForEmail"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("MemberId", SqlDbType.Int, 0, MemberId))
                dr = cmd.ExecuteReader()
                If dr.Read Then
                    result = dr.GetValue(0).ToString()
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
            End Try
            Return result

        End Function
        Public Shared Function GetMemberPurchasedProduct(ByVal pgSize As Integer, ByVal pgIndex As Integer, ByVal MemberId As Integer, ByRef TotalRecords As Integer) As StoreOrderCollection
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETLIST As String = "sp_Member_MemberProductPurchased"
            Dim reader As SqlDataReader = Nothing
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST)
            db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)
            db.AddInParameter(cmd, "CurrentPage", DbType.Int32, pgIndex)
            db.AddInParameter(cmd, "PageSize", DbType.Int32, pgSize)
            db.AddOutParameter(cmd, "TotalRecords", DbType.Int32, 1)
            reader = db.ExecuteReader(cmd)
            Dim result As New StoreOrderCollection
            Try
                While (reader.Read())
                    Dim o As New StoreOrderRow
                    o.OrderId = Convert.ToInt32(reader.Item("OrderId"))
                    o.itemindex = Convert.ToInt32(reader.Item("RowNum"))
                    result.Add(o)
                End While
                Core.CloseReader(reader)
                TotalRecords = CInt(cmd.Parameters("@TotalRecords").Value)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try

            Return result
        End Function
        Public Function GetMemberOrderHistory() As DataSet
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETLIST As String = "sp_Member_MemberOrderHistory"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST)

            db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)

            Return db.ExecuteDataSet(cmd)
        End Function

        Public Function GetCreditMemoHistory() As DataTable
            'Return DB.GetDataTable("select selltocustomername, selltocustomername2, memoid, no, posting, (select sum(lineamount) from salescreditmemoline l where l.memoid = h.memoid) as total from salescreditmemoheader h where memberid = " & MemberId)
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETLIST As String = "sp_Member_GetCreditMemoHistory"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST)

            db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)

            Return db.ExecuteDataSet(cmd).Tables(0)
        End Function

        Public Function GetAddressBook() As DataSet
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETLIST As String = "sp_Member_GetAddressBook"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST)

            db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)

            Return db.ExecuteDataSet(cmd)
        End Function

        Public Function GetFullAddressBook() As DataSet
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETLIST As String = "sp_Member_GetFullAddressBook"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST)

            db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)

            Return db.ExecuteDataSet(cmd)
        End Function

        Public Function GetReminders() As DataSet
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETLIST As String = "sp_Member_GetReminders"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST)

            db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)

            Return db.ExecuteDataSet(cmd)
        End Function

        Public Function GetWishlistItems() As DataSet
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETLIST As String = "sp_Member_GetWishListItems"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST)

            db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)

            Return db.ExecuteDataSet(cmd)
        End Function

        Public Shared Function GetAllMembers(ByVal DB1 As Database) As DataTable
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETLIST_ALL As String = "sp_Member_GetListAll"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST_ALL)

            Return db.ExecuteDataSet(cmd).Tables(0)
        End Function

        Public Shared Function GetListSumaryRewardPointExportToExcel(ByVal Month As Integer, ByVal Year As Integer) As DataTable
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETLIST_SUMARYPOINT As String = "sp_Member_GetListSumaryRewardPointExportToExcel"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST_SUMARYPOINT)

            db.AddInParameter(cmd, "Month", DbType.Int32, Month)
            db.AddInParameter(cmd, "Year", DbType.Int32, Year)

            Return db.ExecuteDataSet(cmd).Tables(0)
        End Function
        Public Shared Function GetCustomerPriceGroupIdByMember(ByVal MemberId As Integer) As String


            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                'Return db.ExecuteScalar(CommandType.Text, "select top 1 coalesce(CustomerPriceGroupId,0) from customer where customerid = (select top 1 CustomerId from member where memberid = " & MemberId & ")")
                Return db.ExecuteScalar(CommandType.Text, "select coalesce(CustomerPriceGroupId,0) from customer ct inner join Member m on ct.CustomerId = m.CustomerId where MemberId = " & MemberId & ")")

            Catch ex As Exception

            End Try
            Return 0

        End Function
    End Class

    Public MustInherit Class MemberRowBase
        Private m_DB As Database
        Private m_MemberId As Integer = Nothing
        Private m_MemberTypeId As Integer = Nothing
        Private m_Username As String = Nothing
        Private m_Password As String = Nothing
        Private m_PricingType As String = Nothing
        Private m_DiscountType As String = Nothing
        Private m_PercentageDiscount As Double = Nothing
        Private m_CustomerId As Integer = Nothing
        Private m_CreateDate As DateTime = Nothing
        Private m_ModifyDate As DateTime = Nothing
        Private m_IsActive As Boolean = Nothing
        Private m_IsSameDefaultAddress As Boolean = Nothing
        Private m_Customer As CustomerRow = Nothing
        Private m_CustomerContact As CustomerContactRow = Nothing

        Private m_LicenseNumber As String = Nothing
        Private m_LicenseState As String = Nothing
        Private m_LicenseExpirationDate As DateTime = Nothing
        Private m_StudentNumber As String = Nothing
        Private m_SchoolName As String = Nothing
        Private m_ContactName As String = Nothing
        Private m_ContactPhone As String = Nothing
        Private m_ExpectedGraduationDate As DateTime = Nothing
        Private m_SalesTaxExemptionNumber As String = Nothing
        Private m_DeptOfRevenueRegistered As Boolean = Nothing
        Private m_ResaleAcceptance As Boolean = Nothing
        Private m_InformationAccuracyAgreement As Boolean = Nothing
        Private m_AuthorizedSignatureName As String = Nothing
        Private m_AuthorizedSignatureTitle As String = Nothing
        Private m_AuthorizedSignatureDate As DateTime = Nothing

        Private m_CanPostJob As Boolean = Nothing
        Private m_CanPostClassified As Boolean = Nothing
        Private m_LastOrderId As Integer
        Private m_IsInternational As Boolean = Nothing
        Private m_GuestStatus As Integer = Nothing

        Private m_ProfessionalStatus As String = Nothing
        Private m_DeActive As Boolean = Nothing
        Private m_ReferCode As String = Nothing

        '' Use in Admin
        Private m_CustomerPostingGroup As String = Nothing
        Private m_Country As String = Nothing
        Private m_LastLoginDate As DateTime = Nothing
        Private m_TotalPoint As Integer = Nothing
        '' End Use in Admin
        Public Shared cachePrefixKey As String = "Member_"

        Public Property LastOrderId() As Integer
            Get
                Return m_LastOrderId
            End Get
            Set(ByVal value As Integer)
                m_LastOrderId = value
            End Set
        End Property

        Public Property ProfessionalStatus() As String
            Get
                Return m_ProfessionalStatus
            End Get
            Set(ByVal value As String)
                m_ProfessionalStatus = value
            End Set
        End Property

        Public Property CanPostJob() As Boolean
            Get
                Return m_CanPostJob
            End Get
            Set(ByVal Value As Boolean)
                m_CanPostJob = Value
            End Set
        End Property

        Public Property CanPostClassified() As Boolean
            Get
                Return m_CanPostClassified
            End Get
            Set(ByVal Value As Boolean)
                m_CanPostClassified = Value
            End Set
        End Property

        Public Property LicenseNumber() As String
            Get
                Return m_LicenseNumber
            End Get
            Set(ByVal Value As String)
                m_LicenseNumber = Value
            End Set
        End Property

        Public Property LicenseState() As String
            Get
                Return m_LicenseState
            End Get
            Set(ByVal Value As String)
                m_LicenseState = Value
            End Set
        End Property

        Public Property LicenseExpirationDate() As DateTime
            Get
                Return m_LicenseExpirationDate
            End Get
            Set(ByVal Value As DateTime)
                m_LicenseExpirationDate = Value
            End Set
        End Property

        Public Property StudentNumber() As String
            Get
                Return m_StudentNumber
            End Get
            Set(ByVal Value As String)
                m_StudentNumber = Value
            End Set
        End Property

        Public Property SchoolName() As String
            Get
                Return m_SchoolName
            End Get
            Set(ByVal Value As String)
                m_SchoolName = Value
            End Set
        End Property
        Public Property ReferCode() As String
            Get
                Return m_ReferCode
            End Get
            Set(ByVal Value As String)
                m_ReferCode = Value
            End Set
        End Property

        Public Property ContactName() As String
            Get
                Return m_ContactName
            End Get
            Set(ByVal Value As String)
                m_ContactName = Value
            End Set
        End Property

        Public Property ContactPhone() As String
            Get
                Return m_ContactPhone
            End Get
            Set(ByVal Value As String)
                m_ContactPhone = Value
            End Set
        End Property

        Public Property ExpectedGraduationDate() As DateTime
            Get
                Return m_ExpectedGraduationDate
            End Get
            Set(ByVal Value As DateTime)
                m_ExpectedGraduationDate = Value
            End Set
        End Property

        Public Property SalesTaxExemptionNumber() As String
            Get
                Return m_SalesTaxExemptionNumber
            End Get
            Set(ByVal Value As String)
                m_SalesTaxExemptionNumber = Value
            End Set
        End Property

        Public Property DeptOfRevenueRegistered() As Boolean
            Get
                Return m_DeptOfRevenueRegistered
            End Get
            Set(ByVal Value As Boolean)
                m_DeptOfRevenueRegistered = Value
            End Set
        End Property

        Public Property ResaleAcceptance() As Boolean
            Get
                Return m_ResaleAcceptance
            End Get
            Set(ByVal Value As Boolean)
                m_ResaleAcceptance = Value
            End Set
        End Property

        Public Property InformationAccuracyAgreement() As Boolean
            Get
                Return m_InformationAccuracyAgreement
            End Get
            Set(ByVal Value As Boolean)
                m_InformationAccuracyAgreement = Value
            End Set
        End Property

        Public Property AuthorizedSignatureName() As String
            Get
                Return m_AuthorizedSignatureName
            End Get
            Set(ByVal Value As String)
                m_AuthorizedSignatureName = Value
            End Set
        End Property

        Public Property AuthorizedSignatureTitle() As String
            Get
                Return m_AuthorizedSignatureTitle
            End Get
            Set(ByVal Value As String)
                m_AuthorizedSignatureTitle = Value
            End Set
        End Property

        Public Property AuthorizedSignatureDate() As DateTime
            Get
                Return m_AuthorizedSignatureDate
            End Get
            Set(ByVal Value As DateTime)
                m_AuthorizedSignatureDate = Value
            End Set
        End Property

        Public Property Customer() As CustomerRow
            Get
                If m_Customer Is Nothing Then m_Customer = CustomerRow.GetRow(m_DB, m_CustomerId)
                If m_Customer Is Nothing Then
                    m_Customer = CustomerRow.GetRow(m_DB, m_CustomerId)
                Else
                    m_Customer.DB = DB
                End If
                Return m_Customer
            End Get
            Set(ByVal Value As CustomerRow)
                m_Customer = Value
            End Set
        End Property

        Public Property CustomerContact() As CustomerContactRow
            Get
                If m_CustomerContact Is Nothing Then m_CustomerContact = CustomerContactRow.GetRow(DB, Customer.ContactId)
                Return m_CustomerContact
            End Get
            Set(ByVal Value As CustomerContactRow)
                m_CustomerContact = Value
            End Set
        End Property

        Public Property MemberId() As Integer
            Get
                Return m_MemberId
            End Get
            Set(ByVal Value As Integer)
                m_MemberId = Value
            End Set
        End Property

        Public Property MemberTypeId() As Integer
            Get
                Return m_MemberTypeId
            End Get
            Set(ByVal Value As Integer)
                m_MemberTypeId = Value
            End Set
        End Property

        Public Property PricingType() As String
            Get
                Return m_PricingType
            End Get
            Set(ByVal value As String)
                m_PricingType = value
            End Set
        End Property

        Public Property DiscountType() As String
            Get
                Return m_DiscountType
            End Get
            Set(ByVal value As String)
                m_DiscountType = value
            End Set
        End Property

        Public Property PercentageDiscount() As Double
            Get
                Return m_PercentageDiscount
            End Get
            Set(ByVal value As Double)
                m_PercentageDiscount = value
            End Set
        End Property

        Public Property Username() As String
            Get
                Return m_Username
            End Get
            Set(ByVal Value As String)
                m_Username = Value
            End Set
        End Property

        Public Property Password() As String
            Get
                Return m_Password
            End Get
            Set(ByVal Value As String)
                m_Password = Value
            End Set
        End Property

        Public Property CustomerId() As Integer
            Get
                Return m_CustomerId
            End Get
            Set(ByVal value As Integer)
                m_CustomerId = value
            End Set
        End Property

        Public ReadOnly Property EncryptedPassword() As String
            Get
                If m_Password = String.Empty Then
                    Return String.Empty
                End If
                Return CryptData.Crypt.EncryptTripleDes(Password)
            End Get
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

        Public Property IsActive() As Boolean
            Get
                Return m_IsActive
            End Get
            Set(ByVal Value As Boolean)
                m_IsActive = Value
            End Set
        End Property

        Public Property GuestStatus() As Integer
            Get
                Return m_GuestStatus
            End Get
            Set(ByVal Value As Integer)
                m_GuestStatus = Value
            End Set
        End Property

        Public Property IsSameDefaultAddress() As Boolean
            Get
                Return m_IsSameDefaultAddress
            End Get
            Set(ByVal Value As Boolean)
                m_IsSameDefaultAddress = Value
            End Set
        End Property
        Public Property IsInternational() As Boolean
            Get
                Return m_IsInternational
            End Get
            Set(ByVal Value As Boolean)
                m_IsInternational = Value
            End Set
        End Property
        Public Property DeActive() As Boolean
            Get
                Return m_DeActive
            End Get
            Set(ByVal Value As Boolean)
                m_DeActive = Value
            End Set
        End Property

        Public Property CustomerPostingGroup() As String
            Get
                Return m_CustomerPostingGroup
            End Get
            Set(ByVal Value As String)
                m_CustomerPostingGroup = Value
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

        Public Property LastLoginDate() As DateTime
            Get
                Return m_LastLoginDate
            End Get
            Set(ByVal Value As DateTime)
                m_LastLoginDate = Value
            End Set
        End Property

        Public Property TotalPoint() As Integer
            Get
                Return m_TotalPoint
            End Get
            Set(ByVal Value As Integer)
                m_TotalPoint = Value
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

        Public Sub New(ByVal database As Database, ByVal MemberId As Integer)
            m_DB = database
            m_MemberId = MemberId
        End Sub 'New
        Public Sub New(ByVal MemberId As Integer)
            m_MemberId = MemberId
        End Sub 'New
        Protected Overridable Sub Load()
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_GETOBJECT As String = "sp_Member_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETOBJECT)
                db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try


        End Sub

        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            m_MemberId = Convert.ToInt32(r.Item("MemberId"))
            If IsDBNull(r.Item("MemberTypeId")) Then
                m_MemberTypeId = Nothing
            Else
                m_MemberTypeId = Convert.ToInt32(r.Item("MemberTypeId"))
            End If

            m_Username = Convert.ToString(r.Item("Username"))
            m_Password = CryptData.Crypt.DecryptTripleDes(r.Item("Password"))
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
            m_IsActive = Convert.ToBoolean(r.Item("IsActive"))
            m_IsSameDefaultAddress = Convert.ToBoolean(r.Item("IsSameDefaultAddress"))
            If IsDBNull(r.Item("CustomerId")) Then
                m_CustomerId = Nothing
            Else
                m_CustomerId = Convert.ToInt32(r.Item("CustomerId"))
            End If

            If IsDBNull(r.Item("LicenseNumber")) Then
                m_LicenseNumber = Nothing
            Else
                m_LicenseNumber = Convert.ToString(r.Item("LicenseNumber"))
            End If
            If IsDBNull(r.Item("LicenseState")) Then
                m_LicenseState = Nothing
            Else
                m_LicenseState = Convert.ToString(r.Item("LicenseState"))
            End If
            If IsDBNull(r.Item("LicenseExpirationDate")) Then
                m_LicenseExpirationDate = Nothing
            Else
                m_LicenseExpirationDate = Convert.ToDateTime(r.Item("LicenseExpirationDate"))
            End If
            If IsDBNull(r.Item("StudentNumber")) Then
                m_StudentNumber = Nothing
            Else
                m_StudentNumber = Convert.ToString(r.Item("StudentNumber"))
            End If
            If IsDBNull(r.Item("SchoolName")) Then
                m_SchoolName = Nothing
            Else
                m_SchoolName = Convert.ToString(r.Item("SchoolName"))
            End If
            If IsDBNull(r.Item("ContactName")) Then
                m_ContactName = Nothing
            Else
                m_ContactName = Convert.ToString(r.Item("ContactName"))
            End If
            If IsDBNull(r.Item("ContactPhone")) Then
                m_ContactPhone = Nothing
            Else
                m_ContactPhone = Convert.ToString(r.Item("ContactPhone"))
            End If
            If IsDBNull(r.Item("ExpectedGraduationDate")) Then
                m_ExpectedGraduationDate = Nothing
            Else
                m_ExpectedGraduationDate = Convert.ToDateTime(r.Item("ExpectedGraduationDate"))
            End If
            If IsDBNull(r.Item("SalesTaxExemptionNumber")) Then
                m_SalesTaxExemptionNumber = Nothing
            Else
                m_SalesTaxExemptionNumber = Convert.ToString(r.Item("SalesTaxExemptionNumber"))
            End If
            m_DeptOfRevenueRegistered = Convert.ToBoolean(r.Item("DeptOfRevenueRegistered"))
            m_ResaleAcceptance = Convert.ToBoolean(r.Item("ResaleAcceptance"))
            m_CanPostJob = Convert.ToBoolean(r.Item("CanPostJob"))
            If Not IsDBNull(r.Item("LastOrderId")) Then
                m_LastOrderId = Convert.ToInt32(r.Item("LastOrderId"))
            Else
                m_LastOrderId = Nothing
            End If
            m_CanPostClassified = Convert.ToBoolean(r.Item("CanPostClassified"))
            m_InformationAccuracyAgreement = Convert.ToBoolean(r.Item("InformationAccuracyAgreement"))
            If IsDBNull(r.Item("AuthorizedSignatureName")) Then
                m_AuthorizedSignatureName = Nothing
            Else
                m_AuthorizedSignatureName = Convert.ToString(r.Item("AuthorizedSignatureName"))
            End If
            If IsDBNull(r.Item("AuthorizedSignatureTitle")) Then
                m_AuthorizedSignatureTitle = Nothing
            Else
                m_AuthorizedSignatureTitle = Convert.ToString(r.Item("AuthorizedSignatureTitle"))
            End If
            If IsDBNull(r.Item("AuthorizedSignatureDate")) Then
                m_AuthorizedSignatureDate = Nothing
            Else
                m_AuthorizedSignatureDate = Convert.ToDateTime(r.Item("AuthorizedSignatureDate"))
            End If
            Try
                If r.Item("IsInternational") Is Convert.DBNull Then
                    m_IsInternational = False
                Else
                    m_IsInternational = CBool(r.Item("IsInternational"))
                End If
            Catch ex As Exception
                m_IsInternational = False
            End Try
            Try
                If r.Item("GuestStatus") Is Convert.DBNull Then
                    m_GuestStatus = 0
                Else
                    m_GuestStatus = CInt(r.Item("GuestStatus"))
                End If
            Catch ex As Exception
                m_GuestStatus = 0
            End Try
            Try
                If r.Item("ProfessionalStatus") Is Convert.DBNull Then
                    m_ProfessionalStatus = Nothing
                Else
                    m_ProfessionalStatus = CInt(r.Item("ProfessionalStatus"))
                End If
            Catch ex As Exception
                m_ProfessionalStatus = Nothing
            End Try
            Try
                If r.Item("DeActive") Is Convert.DBNull Then
                    m_DeActive = False
                Else
                    m_DeActive = CBool(r.Item("DeActive"))
                End If
            Catch ex As Exception
                m_IsInternational = False
            End Try
            If IsDBNull(r.Item("ReferCode")) Then
                m_ReferCode = Nothing
            Else
                m_ReferCode = Convert.ToString(r.Item("ReferCode"))
            End If

        End Sub 'Load


        Public Overridable Function Insert(ByVal bGuest As Boolean) As Integer
            Dim SQL As String

            If Not bGuest Then
                CustomerContact = New CustomerContactRow(DB)
                Customer = New CustomerRow(DB)
            End If

            CustomerContact.DoExport = Not bGuest
            CustomerContact.Insert()

            Customer.ContactId = CustomerContact.ContactId
            Customer.ContactNo = CustomerContact.ContactNo
            Customer.CustomerPostingGroup = "NVN"
            Customer.Insert()

            SQL = " INSERT INTO Member (" _
             & " Username" _
             & ",MemberTypeId" _
             & ",Password" _
             & ",IsSameDefaultAddress" _
             & ",CreateDate" _
             & ",ModifyDate" _
             & ",IsActive" _
             & ",CustomerId" _
             & ",LicenseNumber" _
             & ",LicenseState" _
             & ",LicenseExpirationDate" _
             & ",StudentNumber" _
             & ",SchoolName" _
             & ",ContactName" _
             & ",ContactPhone" _
             & ",ExpectedGraduationDate" _
             & ",SalesTaxExemptionNumber" _
             & ",DeptOfRevenueRegistered" _
             & ",ResaleAcceptance" _
             & ",CanPostJob" _
             & ",CanPostClassified" _
             & ",InformationAccuracyAgreement" _
             & ",AuthorizedSignatureName" _
             & ",AuthorizedSignatureTitle" _
             & ",AuthorizedSignatureDate" _
             & ",LastOrderId" _
             & ",ProfessionalStatus" _
             & ",DeActive" _
             & ",ReferCode" _
             & ",GuestStatus" _
             & ") VALUES (" _
             & m_DB.Quote(Username) _
             & "," & IIf(MemberTypeId > 0, m_DB.Number(MemberTypeId), "NULL") _
             & "," & m_DB.Quote(EncryptedPassword) _
             & "," & CInt(IsSameDefaultAddress) _
             & "," & m_DB.Quote(CreateDate) _
             & "," & m_DB.Quote(ModifyDate) _
             & "," & CInt(IsActive) _
             & "," & m_DB.Quote(Customer.CustomerId) _
             & "," & m_DB.Quote(LicenseNumber) _
             & "," & m_DB.Quote(LicenseState) _
             & "," & m_DB.NullQuote(LicenseExpirationDate) _
             & "," & m_DB.Quote(StudentNumber) _
             & "," & m_DB.Quote(SchoolName) _
             & "," & m_DB.Quote(ContactName) _
             & "," & m_DB.Quote(ContactPhone) _
             & "," & m_DB.NullQuote(ExpectedGraduationDate) _
             & "," & m_DB.Quote(SalesTaxExemptionNumber) _
             & "," & CInt(DeptOfRevenueRegistered) _
             & "," & CInt(ResaleAcceptance) _
             & ",1" _
             & ",1" _
             & "," & CInt(InformationAccuracyAgreement) _
             & "," & m_DB.Quote(AuthorizedSignatureName) _
             & "," & m_DB.Quote(AuthorizedSignatureTitle) _
             & "," & m_DB.NullQuote(AuthorizedSignatureDate) _
             & "," & DB.Number(LastOrderId) _
             & "," & DB.NullNumber(ProfessionalStatus) _
             & "," & CInt(DeActive) _
             & "," & m_DB.Quote(Customer.CustomerNo) _
             & "," & IIf(bGuest, "1", "0") _
             & ")"

            MemberId = m_DB.InsertSQL(SQL)
            CacheUtils.RemoveCacheItemWithPrefix(CustomerRow.cachePrefixKey)
            Return MemberId
        End Function

        Public Overridable Sub Update(ByVal DB As Database)
            If Not DB Is Nothing Then
                m_DB = DB
            End If
            Dim SQL As String

            SQL = " UPDATE Member SET " _
             & " Username = " & m_DB.Quote(Username) _
             & ",MemberTypeId = " & IIf(MemberTypeId > 0, m_DB.Quote(MemberTypeId), "NULL") _
             & ",Password = " & m_DB.Quote(EncryptedPassword) _
             & ",ModifyDate = " & m_DB.Quote(ModifyDate) _
             & ",IsActive = " & CInt(IsActive) _
             & ",IsSameDefaultAddress = " & CInt(IsSameDefaultAddress) _
             & ",CustomerId = " & m_DB.Quote(CustomerId) _
             & ",LicenseNumber = " & m_DB.Quote(LicenseNumber) _
             & ",LicenseState = " & m_DB.Quote(LicenseState) _
             & ",LicenseExpirationDate = " & m_DB.NullQuote(LicenseExpirationDate) _
             & ",StudentNumber = " & m_DB.Quote(StudentNumber) _
             & ",SchoolName = " & m_DB.Quote(SchoolName) _
             & ",ContactName = " & m_DB.Quote(ContactName) _
             & ",ContactPhone = " & m_DB.Quote(ContactPhone) _
             & ",ExpectedGraduationDate = " & m_DB.NullQuote(ExpectedGraduationDate) _
             & ",SalesTaxExemptionNumber = " & m_DB.Quote(SalesTaxExemptionNumber) _
             & ",DeptOfRevenueRegistered = " & CInt(DeptOfRevenueRegistered) _
             & ",ResaleAcceptance = " & CInt(ResaleAcceptance) _
             & ",CanPostJob = " & CInt(CanPostJob) _
             & ",CanPostClassified = " & CInt(CanPostClassified) _
             & ",InformationAccuracyAgreement = " & CInt(InformationAccuracyAgreement) _
             & ",AuthorizedSignatureName = " & m_DB.Quote(AuthorizedSignatureName) _
             & ",AuthorizedSignatureTitle = " & m_DB.Quote(AuthorizedSignatureTitle) _
             & ",AuthorizedSignatureDate = " & m_DB.NullQuote(AuthorizedSignatureDate) _
             & ",LastOrderId = " & DB.Number(LastOrderId) _
             & ",ProfessionalStatus = " & DB.NullNumber(ProfessionalStatus) _
             & ",DeActive = " & CInt(DeActive) _
             & " WHERE MemberId = " & m_DB.Quote(MemberId)

            m_DB.ExecuteSQL(SQL)
            CacheUtils.ClearCacheWithPrefix(CustomerRow.cachePrefixKey, MemberRow.cachePrefixKey, StoreItemRow.cachePrefixKey)
            'Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            'Dim SP_UPDATE As String = "sp_Member_Update"

            'Dim cmd As DbCommand = db.GetStoredProcCommand(SP_UPDATE)

            'db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)
            'db.AddInParameter(cmd, "MemberTypeId", DbType.Int32, MemberTypeId)
            'db.AddInParameter(cmd, "Username", DbType.String, Username)
            'db.AddInParameter(cmd, "Password", DbType.String, EncryptedPassword)
            'db.AddInParameter(cmd, "IsSameDefaultAddress", DbType.Boolean, IsSameDefaultAddress)
            'db.AddInParameter(cmd, "Modifydate", DbType.DateTime, m_DB.NullDate(ModifyDate))
            'db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
            'db.AddInParameter(cmd, "CustomerId", DbType.Int32, CustomerId)
            'db.AddInParameter(cmd, "LicenseNumber", DbType.String, LicenseNumber)
            'db.AddInParameter(cmd, "LicenseState", DbType.String, LicenseState)
            'db.AddInParameter(cmd, "LicenseExpirationDate", DbType.DateTime, m_DB.NullDate(LicenseExpirationDate))
            'db.AddInParameter(cmd, "StudentNumber", DbType.String, StudentNumber)
            'db.AddInParameter(cmd, "SchoolName", DbType.String, SchoolName)
            'db.AddInParameter(cmd, "ContactName", DbType.String, ContactName)
            'db.AddInParameter(cmd, "ContactPhone", DbType.String, ContactPhone)
            'db.AddInParameter(cmd, "ExpectedGraduationDate", DbType.DateTime, m_DB.NullDate(ExpectedGraduationDate))
            'db.AddInParameter(cmd, "SalesTaxExemptionNumber", DbType.String, SalesTaxExemptionNumber)
            'db.AddInParameter(cmd, "DeptOfRevenueRegistered", DbType.Boolean, DeptOfRevenueRegistered)
            'db.AddInParameter(cmd, "ResaleAcceptance", DbType.Boolean, ResaleAcceptance)
            'db.AddInParameter(cmd, "InformationAccuracyAgreement", DbType.Boolean, InformationAccuracyAgreement)
            'db.AddInParameter(cmd, "AuthorizedSignatureName", DbType.String, AuthorizedSignatureName)
            'db.AddInParameter(cmd, "AuthorizedSignatureTitle", DbType.String, AuthorizedSignatureTitle)
            'db.AddInParameter(cmd, "AuthorizedSignatureDate", DbType.DateTime, AuthorizedSignatureDate)
            'db.AddInParameter(cmd, "CanPostJob", DbType.Boolean, CanPostJob)
            'db.AddInParameter(cmd, "CanPostClassified", DbType.Boolean, CanPostClassified)
            'db.AddInParameter(cmd, "LastOrderId", DbType.Int32, LastOrderId)

            'db.ExecuteNonQuery(cmd)

        End Sub 'Update

        Public Sub Remove()
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_DELETE As String = "sp_Member_Delete"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_DELETE)

            db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)

            db.ExecuteNonQuery(cmd)
            CacheUtils.ClearCacheWithPrefix(CustomerRow.cachePrefixKey, MemberRow.cachePrefixKey)
        End Sub 'Remove
    End Class

    Public Class MemberCollection
        Inherits GenericCollection(Of MemberRow)
        Private m_TotalRecords As Integer
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


