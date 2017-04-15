Option Explicit On

'Author: Lam Le
'Date: 10/26/2009 2:12:54 PM

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
Imports Utility

Namespace DataLayer

    Public Class MixMatchRow
        Inherits MixMatchRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal Id As Integer)
            MyBase.New(DB, Id)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal MixMatchNo As String)
            MyBase.New(DB, MixMatchNo)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal Id As Integer) As MixMatchRow
            Dim row As MixMatchRow

            row = New MixMatchRow(DB, Id)
            row.Load()

            Return row
        End Function

        Public Shared Function GetRow(ByVal DB As Database, ByVal MixMatchNo As String) As MixMatchRow
            Dim row As MixMatchRow

            row = New MixMatchRow(DB, MixMatchNo)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal Id As Integer)
            Dim row As MixMatchRow

            row = New MixMatchRow(DB, Id)
            row.Remove()
        End Sub

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal MixMatchNo As String)
            Dim row As MixMatchRow

            row = New MixMatchRow(DB, MixMatchNo)
            row.Remove()
        End Sub

        Public Shared Function GetAllMixMatches(ByVal DB1 As Database) As DataTable
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:54 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_MIXMATCH_GETLIST As String = "sp_MixMatch_GetListAll"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MIXMATCH_GETLIST)

            Return db.ExecuteDataSet(cmd).Tables(0)
            '------------------------------------------------------------------------
        End Function
        Public Shared Function CountFreeItem(ByVal DB As Database, ByVal mixmatchId As Integer, ByVal memberId As Integer, ByVal orderId As Integer) As Integer

            Dim countItemFreeAvailable As Integer = 0
            Dim SQL As String = String.Empty
            Try
                SQL = "select [dbo].[fc_MixMatch_CountFreeItemAvailable](" & mixmatchId & "," & memberId & "," & orderId & ")"
                countItemFreeAvailable = DB.ExecuteScalar(SQL)
            Catch ex As Exception
                Components.Core.LogError("MixMatch.vb", SQL & "<br>MixmatchId:" & mixmatchId & "<br>MemberId:" & memberId & "<br>OrderId:" & orderId, ex)
            End Try

            Return countItemFreeAvailable
        End Function
        Public Shared Function CountPurchaseItem(ByVal DB As Database, ByVal mixmatchId As Integer, ByVal memberId As Integer, ByVal orderId As Integer) As Integer
            Dim countItemFreeAvailable As Integer = 0
            Try

                countItemFreeAvailable = DB.ExecuteScalar("select [dbo].[fc_MixMatch_CountPurchaseItemAvailable](" & mixmatchId & "," & memberId & "," & orderId & ")")
            Catch ex As Exception
                Components.Core.LogError("MixMatch.vb", "mixmatchId:" & mixmatchId & ",memberId:" & memberId & ",orderId:" & orderId, ex)
            End Try
            Return countItemFreeAvailable
        End Function
        Public Shared Function GetAllMixMatchesByType(ByVal DB1 As Database, ByVal type As Integer) As DataTable
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 26, 2009 02:12:54 PM
            '------------------------------------------------------------------------
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_MIXMATCH_GETLIST As String = "sp_MixMatch_GetListAllByType"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MIXMATCH_GETLIST)
            db.AddInParameter(cmd, "Type", DbType.Int32, type)
            Return db.ExecuteDataSet(cmd).Tables(0)
            '------------------------------------------------------------------------
        End Function
        Public Shared Function IsDefaultAllFreeItem(ByVal mmId As Integer) As Boolean
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetSqlStringCommand("select [dbo].[fc_MixMatch_IsDefaultAllItem](" & mmId & ")")
                Dim result As Integer = db.ExecuteScalar(cmd)
                If (result = 1) Then
                    Return True
                End If
            Catch ex As Exception
                SendMailLog("IsDefaultAllFreeItem(" & mmId & ")", ex)
            End Try
            Return False
        End Function
        Public Shared Function CountFreeItemInvalidQty(ByVal orderId As Integer, ByVal mmId As Integer) As Integer
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetSqlStringCommand("select [dbo].[fc_StoreCartItem_CountFreeItemInvalidQty](" & orderId & "," & mmId & ")")
                Dim result As Integer = db.ExecuteScalar(cmd)
                Return result
            Catch ex As Exception
                SendMailLog("CountFreeItemInvalidQty(orderId:" & orderId & ",MixMatchId:" & mmId & ")", ex)
            End Try
            Return 0
        End Function


        Public Shared Function GetListFreeItemInvalid(ByVal orderId As Integer, ByVal mmId As Integer, ByVal memberID As Integer, ByVal type As Integer) As StoreItemCollection
            Dim reader As SqlDataReader = Nothing
            Dim result As New StoreItemCollection
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sql As String = "select sc.SKU, si.QtyOnHand from StoreCartItem sc left join StoreItem si on(si.ItemId=sc.ItemId)"
                sql = sql & " where MixMatchId = " & mmId & " And IsFreeItem = 1 And orderId = " & orderId
                If (type = 1) Then ''check qty
                    sql = sql & " and [dbo].[fc_StoreCartItem_GetTotalQtyIncart](OrderId,sc.ItemId)>QtyOnHand and AcceptingOrder<>" & Utility.Common.ItemAcceptingStatus.AcceptingOrder & " and AcceptingOrder<>" & Utility.Common.ItemAcceptingStatus.InStock
                    'ElseIf (type = 2) Then ''check permission buy brand
                    '    sql = sql & "  and [dbo].[fc_CheckPermissionBuyBrand](" & memberID & ",si.BrandId)=0"
                ElseIf (type = 3) Then '' flammable
                    sql = sql & "  and si.IsFlammable=1"
                End If
                Dim cmd As DbCommand = db.GetSqlStringCommand(sql)
                reader = db.ExecuteReader(cmd)
                While (reader.Read())
                    If (Not reader.IsDBNull(reader.GetOrdinal("sku"))) Then
                        Dim objItem As New StoreItemRow
                        objItem.SKU = reader("sku").ToString()
                        objItem.QtyOnHand = CInt(reader("QtyOnHand"))
                        result.Add(objItem)
                    End If
                End While
                Core.CloseReader(reader)
            Catch ex As Exception
                SendMailLog("GetListFreeItemInvalid(orderId:" & orderId & ",mmId:" & mmId & ",memberID:" & memberID & ")", ex)
                Core.CloseReader(reader)
            End Try
            Return result
        End Function

        Public Shared Function IsDiscountPercent(ByVal mmId As Integer) As Integer
            Dim result As Integer = 0
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SQL As String = "select [dbo].[fc_MixMatch_IsDiscountPercent](" & mmId & ")"
                Dim cmd As DbCommand = db.GetSqlStringCommand(SQL)
                result = db.ExecuteScalar(cmd)
            Catch ex As Exception
                SendMailLog("IsDiscountPercent(" & mmId & ")", ex)
            End Try
            Return result
        End Function

        Public Shared Function CheckMixmatchLineType(ByVal mmId As Integer, ByVal ItemId As Integer) As String()
            Dim result As String() = Nothing 'Common.MixmatchLineType = Common.MixmatchLineType.NoMixmatch
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SQL As String = "SELECT CAST([dbo].[fc_MixMatch_CheckMixmatchLineType](" & mmId & "," & ItemId & ") AS varchar(1)) + '|' +  ISNULL((SELECT CAST(DefaultType AS varchar(1)) FROM MixMatch WHERE Id =" & mmId & "), '0')"
                Dim cmd As DbCommand = db.GetSqlStringCommand(SQL)
                result = db.ExecuteScalar(cmd).ToString().Split("|") 'CType(db.ExecuteScalar(cmd), Common.MixmatchLineType)
            Catch ex As Exception
                SendMailLog("CheckMixmatchLineType(" & mmId & "," & ItemId & ")", ex)
            End Try

            Return result
        End Function

        Public Shared Function CountItemBuy(ByVal DB As Database, ByVal mixmatchId As Integer) As Integer
            Dim result As Integer = 0
            Try
                result = DB.ExecuteScalar("SELECT COUNT(*)  FROM MixMatch mm INNER JOIN MixMatchLine mml ON mm.Id = mml.MixMatchID    WHERE Value = 0 and mm.id=" & mixmatchId)
            Catch ex As Exception

            End Try
            Return result
        End Function
        Public Shared Function GetByMixMatchNo(ByVal mixmatchNo As String) As MixMatchRow
            If String.IsNullOrEmpty(mixmatchNo) Then
                Return Nothing
            End If
            Dim reader As SqlDataReader = Nothing
            Dim result As New MixMatchRow
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetSqlStringCommand("Select * from MixMatch where MixMatchNo='" & mixmatchNo & "'")
                reader = db.ExecuteReader(cmd)
                If reader.Read Then

                    If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                        result.Id = Convert.ToInt32(reader("Id"))
                    Else
                        result.Id = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("MixMatchNo"))) Then
                        result.MixMatchNo = reader("MixMatchNo").ToString()
                    Else
                        result.MixMatchNo = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Product"))) Then
                        result.Product = reader("Product").ToString()
                    Else
                        result.Product = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Description"))) Then
                        result.Description = reader("Description").ToString()
                    Else
                        result.Description = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("CustomerPriceGroupId"))) Then
                        result.CustomerPriceGroupId = Convert.ToInt32(reader("CustomerPriceGroupId"))
                    Else
                        result.CustomerPriceGroupId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Type"))) Then
                        result.Type = Convert.ToInt32(reader("Type"))
                    Else
                        result.Type = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("StartingDate"))) Then
                        result.StartingDate = Convert.ToDateTime(reader("StartingDate"))
                    Else
                        result.StartingDate = Nothing
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("EndingDate"))) Then
                        result.EndingDate = Convert.ToDateTime(reader("EndingDate"))
                    Else
                        result.EndingDate = Nothing
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                        result.IsActive = Convert.ToBoolean(reader("IsActive"))
                    Else
                        result.IsActive = False
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("DiscountType"))) Then
                        result.DiscountType = reader("DiscountType").ToString()
                    Else
                        result.DiscountType = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("LinesToTrigger"))) Then
                        result.LinesToTrigger = Convert.ToInt32(reader("LinesToTrigger"))
                    Else
                        result.LinesToTrigger = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("TimesApplicable"))) Then
                        result.TimesApplicable = Convert.ToInt32(reader("TimesApplicable"))
                    Else
                        result.TimesApplicable = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Mandatory"))) Then
                        result.Mandatory = Convert.ToInt32(reader("Mandatory"))
                    Else
                        result.Mandatory = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Optional"))) Then
                        result.Optional = Convert.ToInt32(reader("Optional"))
                    Else
                        result.Optional = 0
                    End If

                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try

            Return result

        End Function
       
        Public Shared Function GetListFreeItem(ByVal mixmatchId As Integer, ByVal memberId As Integer, ByVal isIntCustomer As Boolean) As StoreItemCollection

            Dim reader As SqlDataReader = Nothing
            Dim result As New StoreItemCollection
            Try
                'Dim SQL As String = String.Empty
                'SQL = "select itemname + coalesce((select top 1 case when coalesce(choicename,'') = '' then '' else ' - ' + choicename + ' ' end from storeitemgroupchoicerel r inner join storeitemgroupchoice c on r.choiceid = c.choiceid inner join storeitemgroupoption o on c.optionid = o.optionid where itemid = si.itemid order by o.sortorder),'')   as itemname,coalesce(URLCode,'') as URLCode, si.itemid,coalesce(image,'') as image,QtyOnHand,AcceptingOrder from storeitem si"
                'SQL = SQL & " left join MixMatchLine mml on(mml.ItemId=si.ItemId ) "
                'SQL = SQL & " where  si.IsActive=1 "
                'SQL = SQL & "  and mml.MixMatchId=" & mixmatchId & " and mml.Value>0 and mml.Value<=100"
                'SQL = SQL & "  and (si.QtyOnHand>0 or  (AcceptingOrder=" & Utility.Common.ItemAcceptingStatus.AcceptingOrder & " or AcceptingOrder=" & Utility.Common.ItemAcceptingStatus.InStock & "))"
                ''If (memberId > 0) Then
                ''    SQL = SQL & "  and [dbo].[fc_CheckPermissionBuyBrand](" & memberId & ",si.BrandId)=1"
                ''End If
                ''If (isIntCustomer) Then
                ''    SQL = SQL & "   and (si.IsFlammable is null or si.IsFlammable=0)"
                ''End If
                'SQL = SQL & "  order by si.itemid"
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_Mixmatch_GetListFreeItem")
                db.AddInParameter(cmd, "MixmatchId", DbType.Int32, mixmatchId)
                db.AddInParameter(cmd, "AcceptingOrder", DbType.Int16, Utility.Common.ItemAcceptingStatus.AcceptingOrder)
                db.AddInParameter(cmd, "InStock", DbType.Int16, Utility.Common.ItemAcceptingStatus.InStock)
                reader = db.ExecuteReader(cmd)
                While (reader.Read())
                    Dim objItem As New StoreItemRow
                    If (Not reader.IsDBNull(reader.GetOrdinal("itemid"))) Then
                        objItem.ItemId = Convert.ToInt32(reader("itemid"))
                    Else
                        objItem.ItemId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("image"))) Then
                        objItem.Image = reader("image").ToString()
                    Else
                        objItem.Image = ""
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("ItemName"))) Then
                        objItem.ItemName = reader("ItemName").ToString()
                    Else
                        objItem.ItemName = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("URLCode"))) Then
                        objItem.URLCode = reader("URLCode").ToString()
                    Else
                        objItem.URLCode = ""
                    End If
                    result.Add(objItem)
                End While
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                SendMailLog("GetListFreeItem(mixmatchId:" & mixmatchId & ",memberId:" & memberId & ",isIntCustomer:" & isIntCustomer & ")", ex)
            End Try

            Return result

        End Function
        Private Shared Sub SendMailLog(ByVal func As String, ByVal ex As Exception)
            Core.LogError("MixMatch.vb", func, ex)
        End Sub
    End Class

    Public MustInherit Class MixMatchRowBase
        Private m_DB As Database
        Private m_Id As Integer = Nothing
        Private m_MixMatchNo As String = Nothing
        Private m_Description As String = Nothing
        Private m_IsActive As Boolean = Nothing
        Private m_DiscountType As String = Nothing
        Private m_LinesToTrigger As Integer = Nothing
        Private m_TimesApplicable As Integer = Nothing
        Private m_Mandatory As Integer = Nothing
        Private m_Optional As Integer = Nothing
        Private m_CustomerPriceGroupId As Integer
        Private m_StartingDate As DateTime
        Private m_EndingDate As DateTime
        Private m_Type As Integer
        Private m_Product As String = Nothing
        Public Property IsCollection As Boolean
        Public Property DefaultType As String
        Public Property CustomerPriceGroupId() As Integer
            Get
                Return m_CustomerPriceGroupId
            End Get
            Set(ByVal value As Integer)
                m_CustomerPriceGroupId = value
            End Set
        End Property
        Public Property Type() As Integer
            Get
                Return m_Type
            End Get
            Set(ByVal value As Integer)
                m_Type = value
            End Set
        End Property
        Public Property StartingDate() As DateTime
            Get
                Return m_StartingDate
            End Get
            Set(ByVal value As DateTime)
                m_StartingDate = value
            End Set
        End Property

        Public Property EndingDate() As DateTime
            Get
                Return m_EndingDate
            End Get
            Set(ByVal value As DateTime)
                m_EndingDate = value
            End Set
        End Property

        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal Value As Integer)
                m_Id = Value
            End Set
        End Property

        Public Property MixMatchNo() As String
            Get
                Return m_MixMatchNo
            End Get
            Set(ByVal Value As String)
                m_MixMatchNo = Value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set(ByVal Value As String)
                m_Description = Value
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

        Public Property DiscountType() As String
            Get
                Return m_DiscountType
            End Get
            Set(ByVal Value As String)
                m_DiscountType = Value
            End Set
        End Property

        Public Property LinesToTrigger() As Integer
            Get
                Return m_LinesToTrigger
            End Get
            Set(ByVal Value As Integer)
                m_LinesToTrigger = Value
            End Set
        End Property

        Public Property TimesApplicable() As Integer
            Get
                Return m_TimesApplicable
            End Get
            Set(ByVal Value As Integer)
                m_TimesApplicable = Value
            End Set
        End Property

        Public Property Mandatory() As Integer
            Get
                Return m_Mandatory
            End Get
            Set(ByVal Value As Integer)
                m_Mandatory = Value
            End Set
        End Property

        Public Property [Optional]() As Integer
            Get
                Return m_Optional
            End Get
            Set(ByVal Value As Integer)
                m_Optional = Value
            End Set
        End Property
        Public Property Product() As String
            Get
                Return m_Product
            End Get
            Set(ByVal Value As String)
                m_Product = Value
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

        Public Sub New(ByVal DB As Database, ByVal Id As Integer)
            m_DB = DB
            m_Id = Id
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal MixMatchNo As String)
            m_DB = DB
            m_MixMatchNo = MixMatchNo
        End Sub 'New

        Protected Overridable Sub Load()
            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_MIXMATCH_GETOBJECT As String = "sp_MixMatch_GetObject"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MIXMATCH_GETOBJECT)
                db.AddInParameter(cmd, "Id", DbType.Int32, Id)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If reader.Read() Then
                    Me.Load(reader)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
            End Try

        End Sub


        Protected Overridable Sub Load(ByVal reader As SqlDataReader)

            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                    m_Id = Convert.ToInt32(reader("Id"))
                Else
                    m_Id = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("MixMatchNo"))) Then
                    m_MixMatchNo = reader("MixMatchNo").ToString()
                Else
                    m_MixMatchNo = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Product"))) Then
                    m_Product = reader("Product").ToString()
                Else
                    m_Product = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Description"))) Then
                    m_Description = reader("Description").ToString()
                Else
                    m_Description = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("CustomerPriceGroupId"))) Then
                    m_CustomerPriceGroupId = Convert.ToInt32(reader("CustomerPriceGroupId"))
                Else
                    m_CustomerPriceGroupId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Type"))) Then
                    m_Type = Convert.ToInt32(reader("Type"))
                Else
                    m_Type = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("StartingDate"))) Then
                    m_StartingDate = Convert.ToDateTime(reader("StartingDate"))
                Else
                    m_StartingDate = Nothing
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("EndingDate"))) Then
                    m_EndingDate = Convert.ToDateTime(reader("EndingDate"))
                Else
                    m_EndingDate = Nothing
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                    m_IsActive = Convert.ToBoolean(reader("IsActive"))
                Else
                    m_IsActive = False
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("DiscountType"))) Then
                    m_DiscountType = reader("DiscountType").ToString()
                Else
                    m_DiscountType = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("LinesToTrigger"))) Then
                    m_LinesToTrigger = Convert.ToInt32(reader("LinesToTrigger"))
                Else
                    m_LinesToTrigger = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("TimesApplicable"))) Then
                    m_TimesApplicable = Convert.ToInt32(reader("TimesApplicable"))
                Else
                    m_TimesApplicable = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Mandatory"))) Then
                    m_Mandatory = Convert.ToInt32(reader("Mandatory"))
                Else
                    m_Mandatory = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Optional"))) Then
                    m_Optional = Convert.ToInt32(reader("Optional"))
                Else
                    m_Optional = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("IsCollection"))) Then
                    IsCollection = Convert.ToBoolean(reader("IsCollection"))
                Else
                    IsCollection = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("DefaultType"))) Then
                    DefaultType = Convert.ToString(reader("DefaultType"))
                Else
                    DefaultType = "0"
                End If
            End If
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_MIXMATCH_INSERT As String = "sp_MixMatch_Insert"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MIXMATCH_INSERT)

            db.AddOutParameter(cmd, "Id", DbType.Int32, 21)
            db.AddInParameter(cmd, "MixMatchNo", DbType.String, MixMatchNo)
            db.AddInParameter(cmd, "Description", DbType.String, Description)
            db.AddInParameter(cmd, "CustomerPriceGroupId", DbType.Int32, CustomerPriceGroupId)
            db.AddInParameter(cmd, "Type", DbType.Int32, Type)
            db.AddInParameter(cmd, "StartingDate", DbType.DateTime, Param.ObjectToDB(StartingDate))
            db.AddInParameter(cmd, "EndingDate", DbType.DateTime, Param.ObjectToDB(EndingDate))
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
            db.AddInParameter(cmd, "DiscountType", DbType.String, DiscountType)
            db.AddInParameter(cmd, "LinesToTrigger", DbType.Int32, LinesToTrigger)
            db.AddInParameter(cmd, "TimesApplicable", DbType.Int32, TimesApplicable)
            db.AddInParameter(cmd, "Mandatory", DbType.Int32, Mandatory)
            db.AddInParameter(cmd, "Optional", DbType.Int32, [Optional])
            db.AddInParameter(cmd, "Product", DbType.String, Product)
            db.ExecuteNonQuery(cmd)
            Id = Convert.ToInt32(db.GetParameterValue(cmd, "Id"))
            CacheUtils.ClearCacheWithPrefix(StoreItemRowBase.cachePrefixKey, DepartmentTabItemRowBase.cachePrefixKey, ShopSaveItemRowBase.cachePrefixKey)
            Return Id
        End Function

        Public Overridable Function Update() As String
            Dim result As String = "0"
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_MIXMATCH_UPDATE As String = "sp_MixMatch_Update_V1"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MIXMATCH_UPDATE)
                db.AddInParameter(cmd, "Id", DbType.Int32, Id)
                db.AddInParameter(cmd, "Type", DbType.Int32, Type)
                db.AddInParameter(cmd, "MixMatchNo", DbType.String, MixMatchNo)
                db.AddInParameter(cmd, "Description", DbType.String, Description)
                db.AddInParameter(cmd, "CustomerPriceGroupId", DbType.Int32, CustomerPriceGroupId)
                db.AddInParameter(cmd, "StartingDate", DbType.DateTime, Param.ObjectToDB(StartingDate))
                db.AddInParameter(cmd, "EndingDate", DbType.DateTime, Param.ObjectToDB(EndingDate))
                db.AddInParameter(cmd, "IsActive", DbType.Boolean, IsActive)
                db.AddInParameter(cmd, "DiscountType", DbType.String, DiscountType)
                db.AddInParameter(cmd, "LinesToTrigger", DbType.Int32, LinesToTrigger)
                db.AddInParameter(cmd, "TimesApplicable", DbType.Int32, TimesApplicable)
                db.AddInParameter(cmd, "Mandatory", DbType.Int32, Mandatory)
                db.AddInParameter(cmd, "Optional", DbType.Int32, [Optional])
                db.AddInParameter(cmd, "Product", DbType.String, Product)
                db.AddInParameter(cmd, "IsCollection", DbType.Boolean, IsCollection)
                db.AddInParameter(cmd, "DefaultType", DbType.String, DefaultType)
                db.AddOutParameter(cmd, "result", DbType.Int32, 0)
                db.ExecuteNonQuery(cmd)
                result = db.GetParameterValue(cmd, "result")
                If (result = 1) Then
                    CacheUtils.ClearCacheWithPrefix(StoreItemRowBase.cachePrefixKey, DepartmentTabItemRowBase.cachePrefixKey, ShopSaveItemRowBase.cachePrefixKey)
                End If
                '------------------------------------------------------------------------
            Catch ex As Exception
                Components.Core.LogError("MixMatch.vb", String.Empty, ex)
            End Try
            Return result
        End Function 'Update



        Public Sub Remove()

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim SP_MIXMATCH_DELETE As String = "sp_MixMatch_Delete"
            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_MIXMATCH_DELETE)
            db.AddInParameter(cmd, "Id", DbType.Int32, Id)
            db.ExecuteNonQuery(cmd)
            CacheUtils.ClearCacheWithPrefix(StoreItemRowBase.cachePrefixKey, DepartmentTabItemRowBase.cachePrefixKey, ShopSaveItemRowBase.cachePrefixKey)
        End Sub 'Remove
    End Class

    Public Class MixMatchCollection
        Inherits GenericCollection(Of MixMatchRow)
    End Class

End Namespace


