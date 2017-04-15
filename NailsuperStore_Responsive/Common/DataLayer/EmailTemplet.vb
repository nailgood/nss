Option Explicit On

'Author: Lam Le
'Date: 9/28/2009 9:48:35 AM

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Components
Imports DataLayer

Namespace DataLayer
    Public Class EmailTempletRow
        Inherits EmailTempletRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal EmailID As Integer)
            MyBase.New(DB, EmailID)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal EmailID As Integer) As EmailTempletRow
            Dim row As EmailTempletRow

            row = New EmailTempletRow(DB, EmailID)
            row.Load()

            Return row
        End Function

        'end 23/10/2009
        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal EmailID As Integer)
            Dim row As EmailTempletRow

            row = New EmailTempletRow(DB, EmailID)
            row.Remove()
        End Sub

        'end 23/10/2009
        'Custom Methods
        Public Shared Function GetAllEmailTemplets(ByVal DB1 As Database) As DataTable
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 28, 2009 09:48:35 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_EMAILTEMPLET_GETLIST As String = "sp_EmailTemplet_GetListAll"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_EMAILTEMPLET_GETLIST)
            Return db.ExecuteDataSet(cmd).Tables(0)
            '------------------------------------------------------------------------
        End Function

        Public Shared Function GetSubjectEmailTemplets(ByVal DB1 As Database, ByVal SubjectID As Integer) As DataTable
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 28, 2009 09:48:35 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_EMAILTEMPLET_GETLIST As String = "sp_EmailTemplet_GetSubjectEmailTemplets"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_EMAILTEMPLET_GETLIST)
            db.AddInParameter(cmd, "SubjectID", DbType.Int32, SubjectID)
            Return db.ExecuteDataSet(cmd).Tables(0)
            '------------------------------------------------------------------------
        End Function

        Public Shared Function GetOutboundEmailTemplets(ByVal DB As Database, ByVal SubjectID As Integer) As DataTable
            Dim dt As DataTable = DB.GetDataTable("select * from EmailTemplet where isactive=1 and getdate() between coalesce(startdate,getdate()) and coalesce(enddate+1,getdate()+1) and subjectid=" & DB.Number(SubjectID))
            Return dt
        End Function

        Public Shared Sub FormatContentsEmailTemplet(ByVal StrFirstname As String, ByVal StrLastname As String, ByVal StrUsername As String, ByVal StrPassword As String, ByVal StrFirstnameContact As String, ByVal StrLastnameContact As String, ByVal StrEmailContact As String, ByVal StrPhoneContact As String, ByVal StrCommentsContact As String, ByVal StrCountryContact As String, ByVal StrSalonContact As String, ByVal StrShippingContact As String, ByVal StrCityContact As String, ByVal StrStateContact As String, ByVal StrZipcodeContact As String, ByVal StrInvoiceContact As String, ByVal StrItemNotReceived As String, ByVal StrDamaged As String, ByVal StrDamagedItem As String, ByVal StrPackage As String, ByVal StrCarton As String, ByVal StrReship As String, ByVal StrItem As String, ByVal StrData As String, ByVal StrPhoneCompany As String, ByVal StrWebsite As String, ByRef Contents As String)
            Contents = Replace(Contents, "#FIRSTNAME#", StrFirstname)
            Contents = Replace(Contents, "#LASTNAME#", StrLastname)
            Contents = Replace(Contents, "#USERNAME#", StrUsername)
            Contents = Replace(Contents, "#PASSWORD#", StrPassword)

            Contents = Replace(Contents, "#FIRSTNAMECONTACT#", StrFirstnameContact)
            Contents = Replace(Contents, "#LASTNAMECONTACT#", StrLastnameContact)
            Contents = Replace(Contents, "#EMAILCONTACT#", StrEmailContact)
            Contents = Replace(Contents, "#PHONECONTACT#", StrPhoneContact)
            Contents = Replace(Contents, "#COMMENTSCONTACT#", StrCommentsContact)
            Contents = Replace(Contents, "#COUNTRYCONTACT#", StrCountryContact)
            Contents = Replace(Contents, "#SALONCONTACT#", StrSalonContact)
            Contents = Replace(Contents, "#SHIPPINGCONTACT#", StrShippingContact)
            Contents = Replace(Contents, "#CITYCONTACT#", StrCityContact)
            If StrCountryContact = "United States" Then
                Contents = Replace(Contents, "#ZIPCODECONTACT#", StrZipcodeContact)
                Contents = Replace(Contents, "#STATECONTACT#", StrStateContact)
            Else
                Contents = Replace(Contents, "<p>ZipCode: #ZIPCODECONTACT#</p>", "")
                Contents = Replace(Contents, "#STATECONTACT#", StrStateContact)
                Contents = Replace(Contents, "State:", "Province/Region:")
            End If

            Contents = Replace(Contents, "#INVOICECONTACT#", StrInvoiceContact)

            Contents = Replace(Contents, "#ITEMNOTRECEIVED#", StrItemNotReceived)
            Contents = Replace(Contents, "#DAMAGED#", StrDamaged)
            Contents = Replace(Contents, "#DAMAGEDITEM#", StrDamagedItem)
            Contents = Replace(Contents, "#PACKAGE#", StrPackage)
            Contents = Replace(Contents, "#CARTON#", StrCarton)
            Contents = Replace(Contents, "#RESHIP#", StrReship)
            Contents = Replace(Contents, "#ITEM#", StrItem)
            Contents = Replace(Contents, "#DATA#", StrData)
            Contents = Replace(Contents, "#PHONECOMPANY#", StrPhoneCompany)
            Contents = Replace(Contents, "#WEBSITE#", StrWebsite)
        End Sub
        Public Shared Sub FormatContentsEmailTempletForItemRelate(ByVal StrItemid As String, ByVal StrItemName As String, ByVal StrSku As String, ByVal StrWeight As String, ByVal StrPriceDesc As String, ByVal StrImage As String, ByVal StrShortDesc As String, ByVal StrLongDesc As String, ByVal StrShortViet As String, ByVal StrLongViet As String, ByRef Contents As String)
            Contents = Replace(Contents, "#ITEMID#", StrItemid)
            Contents = Replace(Contents, "#ITEMNAME#", StrItemName)
            Contents = Replace(Contents, "#SKU#", StrSku)
            Contents = Replace(Contents, "#WEIGHT#", StrWeight)

            Contents = Replace(Contents, "#PRICEDESC#", StrPriceDesc)
            Contents = Replace(Contents, "#IMAGE#", StrImage)
            Contents = Replace(Contents, "#SHORTDESC#", StrShortDesc)
            Contents = Replace(Contents, "#LONGDESC#", StrLongDesc)
            Contents = Replace(Contents, "#SHORTVIET#", StrShortViet)
            Contents = Replace(Contents, "#LONGVIET#", StrLongViet)

        End Sub

        Public Shared Sub FormatOutboundEmailTemplet(ByVal StrFirstname As String, ByVal StrLastname As String, ByVal StrUsername As String, ByVal StrPassword As String, ByVal StrEmail As String, ByVal StrBilFirstname As String, ByVal StrBilLastname As String, ByVal StrBilAddress As String, ByVal StrBilAddress1 As String, ByVal StrBilPhone As String, ByVal StrBilCountry As String, ByVal StrBilSalon As String, ByVal StrBilCity As String, ByVal StrBilState As String, ByVal StrBilProvince As String, ByVal StrBilZipcode As String, ByVal StrBilFax As String, ByVal StrShipFirstname As String, ByVal StrShipLastname As String, ByVal StrShipAddress As String, ByVal StrShipAddress1 As String, ByVal StrShipPhone As String, ByVal StrShipCountry As String, ByVal StrShipSalon As String, ByVal StrShipCity As String, ByVal StrShipState As String, ByVal StrShipProvince As String, ByVal StrShipZipcode As String, ByVal StrCoupon As String, ByVal StrActiveCode As String, ByRef Contents As String, ByRef AwardedPoint As String, ByVal ReviewPoint As String)
            Contents = Replace(Contents, "#FIRSTNAME#", StrFirstname)
            Contents = Replace(Contents, "#LASTNAME#", StrLastname)
            Contents = Replace(Contents, "#USERNAME#", StrUsername)
            Contents = Replace(Contents, "#PASSWORD#", StrPassword)
            Contents = Replace(Contents, "#EMAIL#", StrEmail)

            Contents = Replace(Contents, "#BILFIRSTNAME#", StrBilFirstname)
            Contents = Replace(Contents, "#BILLASTNAME#", StrBilLastname)
            Contents = Replace(Contents, "#BILPHONE#", StrBilPhone)
            Contents = Replace(Contents, "#BILSALON#", StrBilSalon)
            Contents = Replace(Contents, "#BILADDRESS#", StrBilAddress)
            Contents = Replace(Contents, "#BILADDRESS1#", StrBilAddress1)
            Contents = Replace(Contents, "#BILCOUNTRY#", StrBilCountry)
            Contents = Replace(Contents, "#BILCITY#", StrBilCity)
            Contents = Replace(Contents, "#BILZIPCODE#", StrBilZipcode)
            Contents = Replace(Contents, "#BILSTATE#", StrBilState)
            Contents = Replace(Contents, "#BILPROVINCE#", StrBilProvince)
            Contents = Replace(Contents, "#BILFAX#", StrBilFax)

            Contents = Replace(Contents, "#SHIPFIRSTNAME#", StrShipFirstname)
            Contents = Replace(Contents, "#SHIPLASTNAME#", StrShipLastname)
            Contents = Replace(Contents, "#SHIPPHONE#", StrShipPhone)
            Contents = Replace(Contents, "#SHIPSALON#", StrShipSalon)
            Contents = Replace(Contents, "#SHIPADDRESS#", StrShipAddress)
            Contents = Replace(Contents, "#SHIPADDRESS1#", StrShipAddress1)
            Contents = Replace(Contents, "#SHIPCOUNTRY#", StrShipCountry)
            Contents = Replace(Contents, "#SHIPCITY#", StrShipCity)
            Contents = Replace(Contents, "#SHIPZIPCODE#", StrShipZipcode)
            Contents = Replace(Contents, "#SHIPSTATE#", StrShipState)
            Contents = Replace(Contents, "#SHIPPROVINCE#", StrShipProvince)
            Contents = Replace(Contents, "#COUPON#", StrCoupon)
            Contents = Replace(Contents, "#ACTIVECODE#", StrActiveCode)
            Contents = Replace(Contents, "#AWARDEDPOINT#", AwardedPoint)
            Contents = Replace(Contents, "#REVIEWPOINT#", ReviewPoint)
        End Sub

#Region "Thank you customer email"
        Public Shared Sub SendEmailList(ByVal DB As Database, ByVal mailSubjectID As Integer, ByVal customerFirstName As String, ByVal customerLastName As String, ByVal customerEmail As String, ByVal sSubject As String, ByVal Msg As String)
            Dim mailTo As String = String.Empty
            Dim mailtoName As String = String.Empty
            Dim mailBCC As String = String.Empty
            Dim res As DataTable = DB.GetDataTable("select * from Vie_ContactUsSubjectEmail where subjectid=" & DB.Quote(mailSubjectID))

            If res.Rows.Count > 0 Then
                Dim i As Integer
                For i = 0 To res.Rows.Count - 1
                    If String.IsNullOrEmpty(mailTo) Then
                        mailTo = res.Rows(i)("Email")
                        mailtoName = res.Rows(i)("Name")
                    Else
                        mailBCC = res.Rows(i)("Email") & "," & mailBCC
                    End If
                Next

                If String.IsNullOrEmpty(mailBCC) Then
                    Components.Email.SendHTMLMail(customerEmail, customerFirstName & " " & customerLastName, mailTo, mailtoName, sSubject, Msg, "")
                Else
                    If mailBCC.LastIndexOf(",") = mailBCC.Length - 1 Then
                        mailBCC = mailBCC.Substring(0, mailBCC.Length - 1)
                    End If
                    Components.Email.SendHTMLMail(customerEmail, customerFirstName & " " & customerLastName, mailTo, mailtoName, sSubject, Msg, mailBCC)
                End If

                Components.Email.SendHTMLMail(SysParam.GetValue("FromContactUs"), SysParam.GetValue("FromContactUsName"), customerEmail, customerFirstName & " " & customerLastName, sSubject, Msg, "")
            End If
        End Sub
        Public Shared Sub SendThankyouMail(ByVal DB As Database, ByVal firstName As String, ByVal lastName As String, ByVal email As String)
            Dim subject As Integer = 14
            Dim msg As String = ""
            Dim mailSubject As String = ""
            GetThankyouMailContent(DB, subject, firstName, lastName, mailSubject, msg)
            Components.Email.SendHTMLMail(FromEmailType.NoReply, email, firstName & " " & lastName, mailSubject, msg)
        End Sub
        Public Shared Sub GetThankyouMailContent(ByVal DB As Database, ByVal subjectID As String, ByVal firstName As String, ByVal lastName As String, ByRef mailSubjectMsg As String, ByRef mailContent As String)
            Dim dbEmailTemplet As DataTable = GetSubjectEmailTemplets(DB, subjectID)
            If dbEmailTemplet.Rows.Count > 0 Then
                mailSubjectMsg = dbEmailTemplet.Rows(0)("Subject")
                mailContent = dbEmailTemplet.Rows(0)("Contents")
                EmailTempletRow.FormatContentsEmailTemplet("", "", "", "", firstName, lastName, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", mailContent)
            End If

        End Sub

#End Region
    End Class

    Public MustInherit Class EmailTempletRowBase
        Private m_DB As Database
        Private m_EmailID As Integer = Nothing
        Private m_SubjectID As Integer = Nothing
        Private m_Subject As String = Nothing
        Private m_Name As String = Nothing
        Private m_Contents As String = Nothing
        Private m_StartDate As DateTime = Nothing
        Private m_EndDate As DateTime = Nothing
        Private m_IsActive As Boolean = Nothing
        Public Property EmailID() As Integer
            Get
                Return m_EmailID
            End Get
            Set(ByVal Value As Integer)
                m_EmailID = Value
            End Set
        End Property

        Public Property SubjectID() As Integer
            Get
                Return m_SubjectID
            End Get
            Set(ByVal Value As Integer)
                m_SubjectID = Value
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

        Public Property Subject() As String
            Get
                Return m_Subject
            End Get
            Set(ByVal Value As String)
                m_Subject = Value
            End Set
        End Property

        Public Property Contents() As String
            Get
                Return m_Contents
            End Get
            Set(ByVal Value As String)
                m_Contents = Value
            End Set
        End Property

        Public Property StartDate() As DateTime
            Get
                Return m_StartDate
            End Get
            Set(ByVal Value As DateTime)
                m_StartDate = Value
            End Set
        End Property

        Public Property EndDate() As DateTime
            Get
                Return m_EndDate
            End Get
            Set(ByVal Value As DateTime)
                m_EndDate = Value
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

        Public Sub New(ByVal DB As Database)
            m_DB = DB
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal EmailID As Integer)
            m_DB = DB
            m_EmailID = EmailID
        End Sub 'New

        Protected Overridable Sub Load()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 28, 2009 09:48:35 AM
            '------------------------------------------------------------------------
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_EMAILTEMPLET_GETOBJECT As String = "sp_EmailTemplet_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_EMAILTEMPLET_GETOBJECT)
                db.AddInParameter(cmd, "EmailID", DbType.Int32, EmailID)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try

            '------------------------------------------------------------------------
        End Sub

        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 28, 2009 09:48:35 AM
            '------------------------------------------------------------------------
            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("EmailID"))) Then
                    m_EmailID = Convert.ToInt32(reader("EmailID"))
                Else
                    m_EmailID = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("SubjectID"))) Then
                    m_SubjectID = Convert.ToInt32(reader("SubjectID"))
                Else
                    m_SubjectID = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Subject"))) Then
                    m_Subject = reader("Subject").ToString()
                Else
                    m_Subject = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Name"))) Then
                    m_Name = reader("Name").ToString()
                Else
                    m_Name = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Contents"))) Then
                    m_Contents = reader("Contents").ToString()
                Else
                    m_Contents = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("StartDate"))) Then
                    m_StartDate = Convert.ToDateTime(reader("StartDate"))
                Else
                    m_StartDate = DateTime.Now
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("EndDate"))) Then
                    m_EndDate = Convert.ToDateTime(reader("EndDate"))
                Else
                    m_EndDate = DateTime.Now
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                    m_IsActive = Convert.ToBoolean(reader("IsActive"))
                Else
                    m_IsActive = False
                End If
            End If
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 28, 2009 09:48:35 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_EMAILTEMPLET_INSERT As String = "sp_EmailTemplet_Insert"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_EMAILTEMPLET_INSERT)
            db.AddOutParameter(cmd, "EmailID", DbType.Int32, 32)
            db.AddInParameter(cmd, "SubjectID", DbType.Int32, SubjectID)
            db.AddInParameter(cmd, "Subject", DbType.String, Subject)
            db.AddInParameter(cmd, "Name", DbType.String, Name)
            db.AddInParameter(cmd, "Contents", DbType.String, Contents)
            db.AddInParameter(cmd, "StartDate", DbType.DateTime, Date.Now)
            db.AddInParameter(cmd, "EndDate", DbType.DateTime, Date.Now)
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
            db.ExecuteNonQuery(cmd)
            EmailID = Convert.ToInt32(db.GetParameterValue(cmd, "EmailID"))
            '------------------------------------------------------------------------
            Return EmailID
        End Function

        Public Overridable Sub Update()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 28, 2009 09:48:35 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_EMAILTEMPLET_UPDATE As String = "sp_EmailTemplet_Update"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_EMAILTEMPLET_UPDATE)

            db.AddInParameter(cmd, "EmailID", DbType.Int32, EmailID)
            db.AddInParameter(cmd, "SubjectID", DbType.Int32, SubjectID)
            db.AddInParameter(cmd, "Subject", DbType.String, Subject)
            db.AddInParameter(cmd, "Name", DbType.String, Name)
            db.AddInParameter(cmd, "Contents", DbType.String, Contents)
            db.AddInParameter(cmd, "StartDate", DbType.DateTime, StartDate)
            db.AddInParameter(cmd, "EndDate", DbType.DateTime, EndDate)
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)

            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Update      

        Public Sub Remove()
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: September 28, 2009 09:48:35 AM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_EMAILTEMPLET_DELETE As String = "sp_EmailTemplet_Delete"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_EMAILTEMPLET_DELETE)
            db.AddInParameter(cmd, "EmailID", DbType.Int32, EmailID)
            db.ExecuteNonQuery(cmd)
            '------------------------------------------------------------------------
        End Sub 'Remove
    End Class

    Public Class EmailTempletCollection
        Inherits GenericCollection(Of EmailTempletRow)
    End Class
End Namespace

