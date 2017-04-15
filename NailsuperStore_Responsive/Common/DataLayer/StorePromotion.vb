Option Explicit On

Imports System
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Database
Imports Components
Imports Components.Core

Namespace DataLayer

    Public Class StorePromotionRow
        Inherits StorePromotionRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            MyBase.New(DB)
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal PromotionId As Integer)
            MyBase.New(DB, PromotionId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal DB As Database, ByVal PromotionId As Integer) As StorePromotionRow
            Dim row As StorePromotionRow

            row = New StorePromotionRow(DB, PromotionId)
            row.Load()

            Return row
        End Function

        Public Shared Function GetRow(ByVal DB As Database, ByVal PromotionCode As String) As StorePromotionRow
            Return StorePromotionRow.LoadByPromotionCode(DB, PromotionCode)
        End Function

        Public Shared Function CheckCoupon(ByVal PromotionCode As String, ByVal OrderId As Integer) As StorePromotionRow
            Dim row As New StorePromotionRow()
            Dim r As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_StorePromotion_CheckCoupon")
                db.AddInParameter(cmd, "PromotionCode", DbType.String, PromotionCode)
                db.AddInParameter(cmd, "OrderId", DbType.Int32, OrderId)
                r = db.ExecuteReader(cmd)
                If (r.Read()) Then
                    row.PromotionId = Convert.ToInt32(r.Item("PromotionId"))
                    If IsDBNull(r.Item("PromotionName")) Then
                        row.PromotionName = Nothing
                    Else
                        row.PromotionName = Convert.ToString(r.Item("PromotionName"))
                    End If
                    If IsDBNull(r.Item("PromotionCode")) Then
                        row.PromotionCode = Nothing
                    Else
                        row.PromotionCode = Convert.ToString(r.Item("PromotionCode"))
                    End If
                    If IsDBNull(r.Item("PromotionType")) Then
                        row.PromotionType = Nothing
                    Else
                        row.PromotionType = Convert.ToString(r.Item("PromotionType"))
                    End If
                    If IsDBNull(r.Item("Message")) Then
                        row.Message = Nothing
                    Else
                        row.Message = Convert.ToString(r.Item("Message"))
                    End If
                    If IsDBNull(r.Item("Discount")) Then
                        row.Discount = Nothing
                    Else
                        row.Discount = Convert.ToDouble(r.Item("Discount"))
                    End If
                    If IsDBNull(r.Item("IsFreeShipping")) Then
                        row.IsFreeShipping = False
                    Else
                        row.IsFreeShipping = Convert.ToDouble(r.Item("IsFreeShipping"))
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "StorePromotion.vb > CheckCoupon(PromotionCode:" & PromotionCode & ", OrderId:" & OrderId & ")", ex.ToString())
            End Try

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal DB As Database, ByVal PromotionId As Integer)
            Dim row As StorePromotionRow

            row = New StorePromotionRow(DB, PromotionId)
            row.Remove()
        End Sub

        Private Shared Sub SendMailLog(ByVal ex As Exception)
            Components.Email.SendError("ToError500", "StorePromotion.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
        End Sub
        Public Shared Function GetListStorePromotionCollection(ByVal Condition As String) As StorePromotionCollection
            Dim r As SqlDataReader = Nothing
            Dim c As New StorePromotionCollection
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_GETLIST As String = "sp_Admin_GetListStorePromotion"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST)
                db.AddInParameter(cmd, "Condition", DbType.String, Condition)
                r = db.ExecuteReader(cmd)
                While r.Read
                    Dim i As New StorePromotionRow()
                    i.PromotionId = r("PromotionId")
                    i.PromotionCode = r("PromotionCode")
                    i.PromotionName = r("PromotionName")
                    i.Image = IIf(IsDBNull(r("image")), Nothing, r("Image"))
                    i.ImageMap = IIf(IsDBNull(r("ImageMap")), Nothing, r("ImageMap"))
                    c.Add(i)
                End While
                Components.Core.CloseReader(r)
            Catch ex As Exception
                Components.Core.CloseReader(r)
                SendMailLog(ex)
            End Try
            Return c
        End Function
        Public Shared Function GetPromotionCodeById(ByVal DB As Database, ByVal id As Integer) As String
            If id < 0 Then
                Return String.Empty
            End If
            Dim result As String = Nothing
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "Select PromotionCode from StorePromotion where PromotionId=" & id
                r = DB.GetReader(SQL)
                If Not r Is Nothing Then
                    If r.Read Then
                        If IsDBNull(r.Item("PromotionCode")) Then
                            result = Nothing
                        Else
                            result = Convert.ToString(r.Item("PromotionCode"))
                        End If
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                SendMailLog(ex)
            End Try

            Return result
        End Function
        'Custom Methods
        'Public Shared Function GetStorePromotions(ByVal DB As Database, ByVal IsProductCoupon As Boolean) As DataTable
        '    Dim dt As DataTable = DB.GetDataTable("select * from StorePromotion WHERE IsActive='1' and getdate() between coalesce(startdate,getdate()) and coalesce(enddate + 1,getdate() + 1) and IsProductCoupon = " & DB.Quote(IsProductCoupon))
        '    Return dt
        'End Function

        Public Shared Function GetStorePromotions(ByVal DB As Database, ByVal IsProductCoupon As Boolean, ByVal Promotionid As String) As DataTable
            Dim dt As DataTable = DB.GetDataTable("select * from StorePromotion WHERE promotionid not in (select promotionid from storeitem where promotionid is not null and promotionid<>'" & Promotionid & "') and IsActive='1' and getdate() between coalesce(startdate,getdate()) and coalesce(enddate + 1,getdate() + 1) and IsProductCoupon = " & DB.Quote(IsProductCoupon))
            Return dt
        End Function

        Public Shared Function GetStorePromotionRow(ByVal DB As Database, ByVal promotionID As Integer) As DataTable
            Dim dt As DataTable = DB.GetDataTable("select * from StorePromotion WHERE IsActive='1' and getdate() between coalesce(startdate,getdate()) and coalesce(enddate + 1,getdate() + 1) and promotionid = " & DB.Number(promotionID))
            Return dt
        End Function
        Public Shared Function GetCouponColection(ByVal DB1 As Database, ByVal filter As DepartmentFilterFields, ByRef TotalRecords As Integer, ByVal isProduct As Integer) As StorePromotionCollection
            'Dim SQL As String = "select distinct TOP " & TopRecords & " min(sortorder) as sortorder, si.itemname , isnull(si.itemnamenew,'') as itemnamenew, si.shortdesc, si.shortviet, si.shortfrench, si.shortspanish, si.itemid, si.image, si.brandid, sb.brandname from relateditem ri inner join storeitem si on ri.itemid = si.itemid left outer join storebrand sb on si.brandid = sb.brandid  where si.isactive=1 " & IIf(ItemGroupId <> Nothing, " and ri.parentid in (select itemid from storeitem where ItemGroupId = " & ItemGroupId & ")", " and ri.parentid = " & ItemId) & " group by si.itemname, itemnamenew, si.shortdesc, si.shortviet, si.shortfrench, si.shortspanish, si.itemid, si.image, si.brandid, sb.brandid, sb.brandname order by sortorder"
            'Return DB.GetDataSet(SQL)
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 28, 2009 02:13:03 PM
            '------------------------------------------------------------------------
            Dim c As New StorePromotionCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_STORECOUPON_GETLIST As String = "sp_StoreCoupon_List"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STORECOUPON_GETLIST)
                db.AddInParameter(cmd, "OrderBy", DbType.String, "PromotionID")
                db.AddInParameter(cmd, "OrderDirection", DbType.String, "DESC")
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, filter.pg)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, filter.MaxPerPage)
                db.AddInParameter(cmd, "IsProduct", DbType.Int32, isProduct)
                'Return db.ExecuteDataSet(cmd)
                dr = db.ExecuteReader(cmd)
                Dim Count As Integer = 0
                While dr.Read
                    Count = Count + 1
                    Dim sp As New StorePromotionRow(DB1)
                    sp.PromotionId = dr("PromotionId")
                    sp.PromotionName = dr("PromotionName")
                    sp.PromotionCode = dr("PromotionCode")
                    sp.Message = dr("Message")
                    sp.StartDate = dr("StartDate")
                    sp.EndDate = dr("EndDate")
                    sp.IsProductCoupon = dr("IsProductCoupon")
                    sp.Image = IIf(IsDBNull(dr("image")), Nothing, dr("image"))
                    TotalRecords = Convert.ToInt32(dr("TotalRecords"))
                    c.Add(sp)
                End While
                If Count Mod 2 <> 0 Then
                    Dim sp As New StorePromotionRow(DB1)
                    sp.PromotionName = ""
                    sp.PromotionCode = ""
                    sp.Message = ""
                    sp.StartDate = Date.Now
                    sp.EndDate = Date.Now
                    sp.Image = "nobg.gif"
                    c.Add(sp)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog(ex)
            End Try

            Return c
            '------------------------------------------------------------------------
        End Function
        Public Shared Function GetCouponColectionOnMobile(ByVal DB1 As Database, ByVal filter As DepartmentFilterFields, ByRef TotalRecords As Integer, ByVal isProduct As Integer) As StorePromotionCollection
            '------------------------------------------------------------------------
            'Author: Lam Le
            'Date: October 28, 2009 02:13:03 PM
            '------------------------------------------------------------------------
            Dim c As New StorePromotionCollection
            Dim dr As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim SP_STORECOUPON_GETLIST As String = "sp_StoreCoupon_List"
                Dim cmd As DbCommand = db.GetStoredProcCommand(SP_STORECOUPON_GETLIST)
                db.AddInParameter(cmd, "OrderBy", DbType.String, "PromotionID")
                db.AddInParameter(cmd, "OrderDirection", DbType.String, "DESC")
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, filter.pg)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, filter.MaxPerPage)
                db.AddInParameter(cmd, "IsProduct", DbType.Int32, isProduct)
                'Return db.ExecuteDataSet(cmd)
                dr = db.ExecuteReader(cmd)
                Dim Count As Integer = 0
                While dr.Read
                    Count = Count + 1
                    Dim sp As New StorePromotionRow(DB1)
                    sp.PromotionId = dr("PromotionId")
                    sp.PromotionName = dr("PromotionName")
                    sp.PromotionCode = dr("PromotionCode")
                    sp.Message = dr("Message")
                    sp.StartDate = dr("StartDate")
                    sp.EndDate = dr("EndDate")
                    sp.IsProductCoupon = dr("IsProductCoupon")
                    sp.Image = IIf(IsDBNull(dr("image")), Nothing, dr("image"))
                    TotalRecords = Convert.ToInt32(dr("TotalRecords"))
                    c.Add(sp)
                End While
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                SendMailLog(ex)
            End Try
            Return c
            '------------------------------------------------------------------------
        End Function
        '''''''''''''''
        'Custom Methods
        Public Sub InsertRelatedItem(ByVal Record As String)
            Dim aTmp() As String = Split(Record, "-")
            If aTmp(1) = "Item" Then
                DB.ExecuteSQL("INSERT INTO StorePromotionItem (PromotionId, ItemId) VALUES (" & PromotionId & "," & DB.Quote(aTmp(0)) & ")")
            ElseIf aTmp(1) = "Department" Then
                DB.ExecuteSQL("INSERT INTO StorePromotionItem (PromotionId, DepartmentId) VALUES (" & PromotionId & "," & DB.Quote(aTmp(0)) & ")")
            End If
        End Sub
        Public Shared Sub GetMessageByCode(ByVal DB As Database, ByRef promotionCode As String, ByRef message As String, ByRef IsProductCoupon As Boolean)
            If promotionCode = String.Empty Or promotionCode Is Nothing Then
                promotionCode = Nothing
                message = String.Empty
                Exit Sub
            End If
            Dim temp As String = promotionCode
            promotionCode = Nothing
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT isProductCoupon, Message FROM StorePromotion WHERE PromotionCode = '" & temp & "'"
                r = DB.GetReader(SQL)
                If Not r Is Nothing Then
                    If r.Read Then
                        promotionCode = temp
                        If IsDBNull(r.Item("Message")) Then
                            message = Nothing
                        Else
                            message = Convert.ToString(r.Item("Message"))
                        End If
                        If IsDBNull(r.Item("IsProductCoupon")) Then
                            IsProductCoupon = Nothing
                        Else
                            IsProductCoupon = Convert.ToBoolean(r.Item("IsProductCoupon"))
                        End If
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                SendMailLog(ex)
            End Try

        End Sub
        Public Shared Function LoadByPromotionCode(ByVal DB As Database, ByVal promotionCode As String) As StorePromotionRow
            Dim result As New StorePromotionRow
            If promotionCode = String.Empty Then
                Return result
            End If
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM StorePromotion WHERE PromotionCode = '" & promotionCode & "'"
                r = DB.GetReader(SQL)
                If Not r Is Nothing Then
                    If r.Read Then
                        result.PromotionId = Convert.ToInt32(r.Item("PromotionId"))
                        If IsDBNull(r.Item("PromotionName")) Then
                            result.PromotionName = Nothing
                        Else
                            result.PromotionName = Convert.ToString(r.Item("PromotionName"))
                        End If
                        If IsDBNull(r.Item("PromotionCode")) Then
                            result.PromotionCode = Nothing
                        Else
                            result.PromotionCode = Convert.ToString(r.Item("PromotionCode"))
                        End If
                        If IsDBNull(r.Item("PromotionType")) Then
                            result.PromotionType = Nothing
                        Else
                            result.PromotionType = Convert.ToString(r.Item("PromotionType"))
                        End If
                        If IsDBNull(r.Item("Message")) Then
                            result.Message = Nothing
                        Else
                            result.Message = Convert.ToString(r.Item("Message"))
                        End If
                        If IsDBNull(r.Item("PricingType")) Then
                            result.PricingType = Nothing
                        Else
                            result.PricingType = Convert.ToString(r.Item("PricingType"))
                        End If
                        If IsDBNull(r.Item("Discount")) Then
                            result.Discount = Nothing
                        Else
                            result.Discount = Convert.ToDouble(r.Item("Discount"))
                        End If
                        result.StartDate = Convert.ToDateTime(r.Item("StartDate"))
                        result.EndDate = Convert.ToDateTime(r.Item("EndDate"))
                        If IsDBNull(r.Item("MinimumPurchase")) Then
                            result.MinimumPurchase = Nothing
                        Else
                            result.MinimumPurchase = Convert.ToDouble(r.Item("MinimumPurchase"))
                        End If
                        If IsDBNull(r.Item("MinimumQuantityPurchase")) Then
                            result.MinimumQuantityPurchase = Nothing
                        Else
                            result.MinimumQuantityPurchase = Convert.ToDouble(r.Item("MinimumQuantityPurchase"))
                        End If
                        If IsDBNull(r.Item("MaximumPurchase")) Then
                            result.MaximumPurchase = Nothing
                        Else
                            result.MaximumPurchase = Convert.ToDouble(r.Item("MaximumPurchase"))
                        End If
                        result.IsItemSpecific = Convert.ToBoolean(r.Item("IsItemSpecific"))
                        result.IsFreeShipping = Convert.ToBoolean(r.Item("IsFreeShipping"))
                        result.IsActive = Convert.ToBoolean(r.Item("IsActive"))
                        result.IsOneUse = Convert.ToBoolean(r.Item("IsOneUse"))
                        result.IsUsed = Convert.ToBoolean(r.Item("IsUsed"))
                        result.IsRegisterSend = Convert.ToBoolean(r.Item("IsRegisterSend"))
                        result.IsProductCoupon = Convert.ToBoolean(r.Item("IsProductCoupon"))

                        If IsDBNull(r.Item("Image")) Then
                            result.Image = Nothing
                        Else
                            result.Image = Convert.ToString(r.Item("Image"))
                        End If
                        If IsDBNull(r.Item("ImageMap")) Then
                            result.ImageMap = Nothing
                        Else
                            result.ImageMap = Convert.ToString(r.Item("ImageMap"))
                        End If
                        If IsDBNull(r.Item("IsActiveImage")) Then
                            result.IsActiveImage = False
                        Else
                            result.IsActiveImage = Convert.ToBoolean(r.Item("IsActiveImage"))
                        End If
                        If IsDBNull(r.Item("IsTotalProduct")) Then
                            result.IsTotalProduct = False
                        Else
                            result.IsTotalProduct = Convert.ToBoolean(r.Item("IsTotalProduct"))
                        End If
                        If IsDBNull(r.Item("MixmatchId")) Then
                            result.MixmatchId = Nothing
                        Else
                            result.MixmatchId = Convert.ToInt32(r.Item("MixmatchId"))
                        End If
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                SendMailLog(ex)
            End Try

            Return result
        End Function
        Public Sub RemoveRelatedItem(ByVal Id As Integer)
            DB.ExecuteSQL("DELETE FROM StorePromotionItem WHERE Id=" & DB.Number(Id))
        End Sub

        Public Shared Function ValidatePromotionAdmin(ByVal DB As Database, ByVal Customer As CustomerRow, ByVal PromotionCode As String, ByRef sMsg As String, ByVal Subtotal As Double) As Boolean
            ValidatePromotionAdmin = False
            If Not String.IsNullOrEmpty(PromotionCode) Then
                Dim PromotionId As Integer = DB.ExecuteScalar("SELECT PromotionId FROM StorePromotion WHERE PromotionCode=" & DB.Quote(PromotionCode))
                If PromotionId = 0 Then
                    sMsg = "The promotion code you entered does not exist"
                    Exit Function
                End If

                Dim dbPromotion As StorePromotionRow = StorePromotionRow.GetRow(DB, PromotionId)

                If Not dbPromotion.MinimumPurchase = Nothing Then
                    If dbPromotion.MinimumPurchase > Subtotal Then
                        sMsg = "This promotion has a minimum purchase amount of " & FormatCurrency(dbPromotion.MinimumPurchase)
                        Exit Function
                    End If
                End If

                If Not dbPromotion.MaximumPurchase = Nothing Then
                    If dbPromotion.MaximumPurchase < Subtotal Then
                        sMsg = "This promotion has a maximum purchase amount of " & FormatCurrency(dbPromotion.MaximumPurchase)
                        Exit Function
                    End If
                End If
                sMsg = "The promotion code you entered is valid."
                ValidatePromotionAdmin = True
            End If
        End Function

        Public Shared Function ValidatePromotion(ByVal DB As Database, ByVal Customer As CustomerRow, ByVal PromotionCode As String, ByRef sMsg As String, ByVal Subtotal As Double) As Boolean
            ValidatePromotion = False
            If Not String.IsNullOrEmpty(PromotionCode) Then
                Dim PromotionId As Integer = DB.ExecuteScalar("SELECT PromotionId FROM StorePromotion WHERE PromotionCode=" & DB.Quote(PromotionCode))
                If PromotionId = 0 Then
                    sMsg = "The promotion code you entered does not exist"
                    Exit Function
                End If

                Dim dbPromotion As StorePromotionRow = StorePromotionRow.GetRow(DB, PromotionId)

                Dim CustomerPriceGroupId As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
                If (dbPromotion.CustomerPriceGroupId > 0) Then
                    If dbPromotion.CustomerPriceGroupId <> CustomerPriceGroupId Then
                        sMsg = "The promotion code you entered does not exist"
                        Exit Function
                    End If
                End If

                If DateDiff(DateInterval.Day, Now, dbPromotion.StartDate) > 0 Or DateDiff(DateInterval.Day, Now, dbPromotion.EndDate) < 0 Then
                    sMsg = "The promotion code you entered is only valid from " & FormatDateTime(dbPromotion.StartDate, DateFormat.ShortDate) & " and " & FormatDateTime(dbPromotion.EndDate, DateFormat.ShortDate)
                    Exit Function
                End If

                If dbPromotion.IsActive = False Then
                    sMsg = "The promotion you entered is no longer active on the website"
                    Exit Function
                End If

                If Not dbPromotion.MinimumPurchase = Nothing Then
                    If dbPromotion.MinimumPurchase > Subtotal Then
                        sMsg = "This promotion has a minimum purchase amount of " & FormatCurrency(dbPromotion.MinimumPurchase)
                        Exit Function
                    End If
                End If

                If Not dbPromotion.MaximumPurchase = Nothing Then
                    If dbPromotion.MaximumPurchase < Subtotal Then
                        sMsg = "This promotion has a maximum purchase amount of " & FormatCurrency(dbPromotion.MaximumPurchase)
                        Exit Function
                    End If
                End If

                If Utility.Common.IsPromotionFreeItem(dbPromotion.PromotionType) Then
                    Dim countFreeItem As Integer = DB.ExecuteScalar("Select count(pri.ItemId) from StorePromotionItem pri left join StoreItem si on(si.ItemId=pri.ItemId) where si.IsActive=1 and pri.PromotionId=" & dbPromotion.PromotionId)
                    If (countFreeItem < 1) Then
                        sMsg = "The promotion code you entered does not valid"
                        Exit Function
                    End If
                End If
                sMsg = dbPromotion.Message
                ValidatePromotion = True
            End If
        End Function
        ''''''''vuphuong add : 11/07/2009
        Public Shared Function ValidateProductPromotion(ByVal DB As Database, ByVal PromotionCode As String, ByVal memberId As Integer, ByVal cartItemId As Integer) As Integer
            Dim validatePromotion As Integer = DB.ExecuteScalar("Select [dbo].[fc_StorePromotion_ValidateProductPromotion]('" & PromotionCode & "'," & memberId & "," & cartItemId & ")")
            Return validatePromotion
        End Function
        Public Shared Function ValidateProductCoupon(ByVal DB As Database, ByVal dbPromotion As StorePromotionRow, ByVal PromotionCode As String, ByRef sMsg As String, ByVal memberId As Integer, ByVal cartItemId As Integer) As Boolean

            ValidateProductCoupon = False
            Dim resultCheck As Integer = ValidateProductPromotion(DB, PromotionCode, memberId, cartItemId)
            If resultCheck = 2 Then
                sMsg = "The promotion code you entered does not exist"
                Exit Function
            End If

            If resultCheck = 4 Then
                sMsg = "The promotion code you entered does not exist"
                Exit Function
            End If
            If resultCheck = 3 Then
                sMsg = "The promotion you entered is no longer active on the website"
                Exit Function
            End If
            If dbPromotion Is Nothing Then
                dbPromotion = StorePromotionRow.GetRow(DB, PromotionCode)
            End If

            If resultCheck = 5 Then
                sMsg = "The promotion code you entered is only valid from " & FormatDateTime(dbPromotion.StartDate, DateFormat.ShortDate) & " and " & FormatDateTime(dbPromotion.EndDate, DateFormat.ShortDate)
                Exit Function

            End If

            If resultCheck = 6 Then
                sMsg = "This promotion has a minimum purchase amount of " & FormatCurrency(dbPromotion.MinimumPurchase)
                Exit Function

            End If
            If resultCheck = 7 Then
                sMsg = "This promotion has a maximum purchase amount of " & FormatCurrency(dbPromotion.MaximumPurchase)
                Exit Function

            End If
            If resultCheck = 8 Then
                sMsg = "This promotion has a minimum purchase Quantity of " & Format(dbPromotion.MinimumQuantityPurchase)
                Exit Function

            End If
            If resultCheck = 9 Then
                sMsg = "Although you've entered a valid promo code " & PromotionCode & ", your order does not currently meet the code's usage criteria"
                Exit Function

            End If
            sMsg = dbPromotion.Message
            ValidateProductCoupon = True
        End Function

        '''''''''''''''''''''''''''''''''''
    End Class

    Public MustInherit Class StorePromotionRowBase
        Private m_DB As Database
        Private m_PromotionId As Integer = Nothing
        Private m_PromotionName As String = Nothing
        Private m_PromotionCode As String = Nothing
        Private m_PromotionType As String = Nothing
        Private m_Message As String = Nothing
        Private m_Discount As Double = Nothing
        Private m_StartDate As DateTime = Nothing
        Private m_EndDate As DateTime = Nothing
        Private m_MinimumPurchase As Double = Nothing
        Private m_MaximumPurchase As Double = Nothing
        Private m_MinimumQuantityPurchase As Double = Nothing
        Private m_IsItemSpecific As Boolean = Nothing
        Private m_IsFreeShipping As Boolean = Nothing
        Private m_IsActive As Boolean = Nothing
        Private m_IsOneUse As Boolean = Nothing
        Private m_IsUsed As Boolean = Nothing
        Private m_IsRegisterSend As Boolean = Nothing
        Private m_IsProductCoupon As Boolean = Nothing
        Private m_PricingType As String = Nothing
        Private m_Image As String = Nothing
        Private m_ImageMap As String = Nothing
        Private m_IsActiveImage As Boolean = Nothing
        Private m_IsTotalProduct As Boolean = Nothing
        Private m_MixmatchId As Integer = Nothing
        Private m_CustomerPriceGroupId As Integer = Nothing

        Public Property PromotionId() As Integer
            Get
                Return m_PromotionId
            End Get
            Set(ByVal Value As Integer)
                m_PromotionId = Value
            End Set
        End Property
        Public Property CustomerPriceGroupId() As Integer
            Get
                Return m_CustomerPriceGroupId
            End Get
            Set(ByVal Value As Integer)
                m_CustomerPriceGroupId = Value
            End Set
        End Property
        Public Property MixmatchId() As Integer
            Get
                Return m_MixmatchId
            End Get
            Set(ByVal Value As Integer)
                m_MixmatchId = Value
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

        Public Property PromotionName() As String
            Get
                Return m_PromotionName
            End Get
            Set(ByVal Value As String)
                m_PromotionName = Value
            End Set
        End Property

        Public Property PromotionCode() As String
            Get
                Return m_PromotionCode
            End Get
            Set(ByVal Value As String)
                m_PromotionCode = Value
            End Set
        End Property

        Public Property PromotionType() As String
            Get
                Return m_PromotionType
            End Get
            Set(ByVal Value As String)
                m_PromotionType = Value
            End Set
        End Property

        Public Property Message() As String
            Get
                Return m_Message
            End Get
            Set(ByVal Value As String)
                m_Message = Value
            End Set
        End Property

        Public Property Discount() As Double
            Get
                Return m_Discount
            End Get
            Set(ByVal Value As Double)
                m_Discount = Value
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

        Public Property MinimumPurchase() As Double
            Get
                Return m_MinimumPurchase
            End Get
            Set(ByVal Value As Double)
                m_MinimumPurchase = Value
            End Set
        End Property


        Public Property MaximumPurchase() As Double
            Get
                Return m_MaximumPurchase
            End Get
            Set(ByVal Value As Double)
                m_MaximumPurchase = Value
            End Set
        End Property

        Public Property MinimumQuantityPurchase() As Double
            Get
                Return m_MinimumQuantityPurchase
            End Get
            Set(ByVal Value As Double)
                m_MinimumQuantityPurchase = Value
            End Set
        End Property

        Public Property IsItemSpecific() As Boolean
            Get
                Return m_IsItemSpecific
            End Get
            Set(ByVal Value As Boolean)
                m_IsItemSpecific = Value
            End Set
        End Property

        Public Property IsFreeShipping() As Boolean
            Get
                Return m_IsFreeShipping
            End Get
            Set(ByVal Value As Boolean)
                m_IsFreeShipping = Value
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

        Public Property IsOneUse() As Boolean
            Get
                Return m_IsOneUse
            End Get
            Set(ByVal Value As Boolean)
                m_IsOneUse = Value
            End Set
        End Property

        Public Property IsUsed() As Boolean
            Get
                Return m_IsUsed
            End Get
            Set(ByVal Value As Boolean)
                m_IsUsed = Value
            End Set
        End Property

        Public Property IsRegisterSend() As Boolean
            Get
                Return m_IsRegisterSend
            End Get
            Set(ByVal Value As Boolean)
                m_IsRegisterSend = Value
            End Set
        End Property

        Public Property IsProductCoupon() As Boolean
            Get
                Return m_IsProductCoupon
            End Get
            Set(ByVal Value As Boolean)
                m_IsProductCoupon = Value
            End Set
        End Property

        Public Property Image() As String
            Get
                Return m_Image
            End Get
            Set(ByVal Value As String)
                m_Image = Value
            End Set
        End Property

        Public Property ImageMap() As String
            Get
                Return m_ImageMap
            End Get
            Set(ByVal Value As String)
                m_ImageMap = Value
            End Set
        End Property

        Public Property IsActiveImage() As Boolean
            Get
                Return m_IsActiveImage
            End Get
            Set(ByVal Value As Boolean)
                m_IsActiveImage = Value
            End Set
        End Property
        Public Property IsTotalProduct() As Boolean
            Get
                Return m_IsTotalProduct
            End Get
            Set(ByVal Value As Boolean)
                m_IsTotalProduct = Value
            End Set
        End Property
        'Public Property CouponProduct() As String
        '    Get
        '        Return m_CouponProduct
        '    End Get
        '    Set(ByVal Value As String)
        '        m_CouponProduct = Value
        '    End Set
        'End Property

        'Public Property CouponOrder() As String
        '    Get
        '        Return m_CouponOrder
        '    End Get
        '    Set(ByVal Value As String)
        '        m_CouponOrder = Value
        '    End Set
        'End Property

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

        Public Sub New(ByVal DB As Database, ByVal PromotionId As Integer)
            m_DB = DB
            m_PromotionId = PromotionId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM StorePromotion WHERE PromotionId = " & DB.Number(PromotionId)
                r = m_DB.GetReader(SQL)
                If Not r Is Nothing Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "Error 500", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try

        End Sub

        Public Shared Sub GetAllCoupon(ByVal OrderId As Integer, ByRef CouponProduct As String, ByRef CouponOrder As String)
            Dim result As Integer = 0
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp_Coupon_LoadAll As String = "sp_Coupon_LoadAll"
            Dim cmd As DbCommand = db.GetStoredProcCommand(sp_Coupon_LoadAll)
            db.AddInParameter(cmd, "OrderId", DbType.Int32, OrderId)
            db.AddOutParameter(cmd, "CouponProduct", DbType.String, 200)
            db.AddOutParameter(cmd, "CouponOrder", DbType.String, 200)
            db.ExecuteNonQuery(cmd)
            CouponProduct = IIf(IsDBNull(cmd.Parameters("@CouponProduct").Value), "", cmd.Parameters("@CouponProduct").Value)
            CouponOrder = IIf(IsDBNull(cmd.Parameters("@CouponOrder").Value), "", cmd.Parameters("@CouponOrder").Value)
        End Sub

        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                m_PromotionId = Convert.ToInt32(r.Item("PromotionId"))
                If IsDBNull(r.Item("PromotionName")) Then
                    m_PromotionName = Nothing
                Else
                    m_PromotionName = Convert.ToString(r.Item("PromotionName"))
                End If
                If IsDBNull(r.Item("MixmatchId")) Then
                    m_MixmatchId = Nothing
                Else
                    m_MixmatchId = Convert.ToInt32(r.Item("MixmatchId"))
                End If
                If IsDBNull(r.Item("CustomerPriceGroupId")) Then
                    m_CustomerPriceGroupId = Nothing
                Else
                    m_CustomerPriceGroupId = Convert.ToInt32(r.Item("CustomerPriceGroupId"))
                End If

                If IsDBNull(r.Item("PromotionCode")) Then
                    m_PromotionCode = Nothing
                Else
                    m_PromotionCode = Convert.ToString(r.Item("PromotionCode"))
                End If
                If IsDBNull(r.Item("PromotionType")) Then
                    m_PromotionType = Nothing
                Else
                    m_PromotionType = Convert.ToString(r.Item("PromotionType"))
                End If
                If IsDBNull(r.Item("Message")) Then
                    m_Message = Nothing
                Else
                    m_Message = Convert.ToString(r.Item("Message"))
                End If
                If IsDBNull(r.Item("PricingType")) Then
                    m_PricingType = Nothing
                Else
                    m_PricingType = Convert.ToString(r.Item("PricingType"))
                End If
                If IsDBNull(r.Item("Discount")) Then
                    m_Discount = Nothing
                Else
                    m_Discount = Convert.ToDouble(r.Item("Discount"))
                End If
                m_StartDate = Convert.ToDateTime(r.Item("StartDate"))
                m_EndDate = Convert.ToDateTime(r.Item("EndDate"))
                If IsDBNull(r.Item("MinimumPurchase")) Then
                    m_MinimumPurchase = Nothing
                Else
                    m_MinimumPurchase = Convert.ToDouble(r.Item("MinimumPurchase"))
                End If
                If IsDBNull(r.Item("MinimumQuantityPurchase")) Then
                    m_MinimumQuantityPurchase = Nothing
                Else
                    m_MinimumQuantityPurchase = Convert.ToDouble(r.Item("MinimumQuantityPurchase"))
                End If
                If IsDBNull(r.Item("MaximumPurchase")) Then
                    m_MaximumPurchase = Nothing
                Else
                    m_MaximumPurchase = Convert.ToDouble(r.Item("MaximumPurchase"))
                End If
                m_IsItemSpecific = Convert.ToBoolean(r.Item("IsItemSpecific"))
                m_IsFreeShipping = Convert.ToBoolean(r.Item("IsFreeShipping"))
                m_IsActive = Convert.ToBoolean(r.Item("IsActive"))
                m_IsOneUse = Convert.ToBoolean(r.Item("IsOneUse"))
                m_IsUsed = Convert.ToBoolean(r.Item("IsUsed"))
                m_IsRegisterSend = Convert.ToBoolean(r.Item("IsRegisterSend"))
                m_IsProductCoupon = Convert.ToBoolean(r.Item("IsProductCoupon"))

                If IsDBNull(r.Item("Image")) Then
                    m_Image = Nothing
                Else
                    m_Image = Convert.ToString(r.Item("Image"))
                End If
                If IsDBNull(r.Item("ImageMap")) Then
                    m_ImageMap = Nothing
                Else
                    m_ImageMap = Convert.ToString(r.Item("ImageMap"))
                End If
                If IsDBNull(r.Item("IsActiveImage")) Then
                    m_IsActiveImage = False
                Else
                    m_IsActiveImage = Convert.ToBoolean(r.Item("IsActiveImage"))
                End If
                If IsDBNull(r.Item("IsTotalProduct")) Then
                    m_IsTotalProduct = False
                Else
                    m_IsTotalProduct = Convert.ToBoolean(r.Item("IsTotalProduct"))
                End If
            Catch ex As Exception
                Throw ex
            End Try

        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String

            SQL = " INSERT INTO StorePromotion (" _
             & " PromotionName" _
             & ",PromotionCode" _
             & ",PromotionType" _
             & ",Message" _
             & ",Discount" _
             & ",MixmatchId" _
              & ",CustomerPriceGroupId" _
             & ",StartDate" _
             & ",EndDate" _
             & ",MinimumPurchase" _
             & ",MaximumPurchase" _
             & ",MinimumQuantityPurchase" _
             & ",IsItemSpecific" _
             & ",IsFreeShipping" _
             & ",IsActive" _
             & ",IsOneUse" _
             & ",IsUsed" _
             & ",IsRegisterSend" _
             & ",IsProductCoupon" _
             & ",PricingType" _
             & ",Image" _
             & ",IsTotalProduct" _
             & ") VALUES (" _
             & m_DB.Quote(PromotionName) _
             & "," & m_DB.Quote(PromotionCode) _
             & "," & m_DB.Quote(PromotionType) _
             & "," & m_DB.Quote(Message) _
             & "," & m_DB.Number(Discount) _
              & "," & m_DB.Number(MixmatchId) _
               & "," & m_DB.Number(CustomerPriceGroupId) _
             & "," & m_DB.NullQuote(StartDate) _
             & "," & m_DB.NullQuote(EndDate) _
             & "," & m_DB.NullNumber(MinimumPurchase) _
             & "," & m_DB.NullNumber(MaximumPurchase) _
             & "," & m_DB.NullNumber(MinimumQuantityPurchase) _
             & "," & CInt(IsItemSpecific) _
             & "," & CInt(IsFreeShipping) _
             & "," & CInt(IsActive) _
             & "," & CInt(IsOneUse) _
             & "," & CInt(IsUsed) _
             & "," & CInt(IsRegisterSend) _
             & "," & CInt(IsProductCoupon) _
             & "," & m_DB.Quote(PricingType) _
             & "," & m_DB.Quote(Image) _
             & "," & CInt(IsTotalProduct) _
             & ")"

            PromotionId = m_DB.InsertSQL(SQL)

            Return PromotionId
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE StorePromotion SET " _
             & " PromotionName = " & m_DB.Quote(PromotionName) _
             & ",PromotionCode = " & m_DB.Quote(PromotionCode) _
             & ",PromotionType = " & m_DB.Quote(PromotionType) _
             & ",Message = " & m_DB.Quote(Message) _
             & ",Discount = " & m_DB.Number(Discount) _
              & ",MixmatchId = " & m_DB.Number(MixmatchId) _
               & ",CustomerPriceGroupId = " & m_DB.Number(CustomerPriceGroupId) _
             & ",StartDate = " & m_DB.NullQuote(StartDate) _
             & ",EndDate = " & m_DB.NullQuote(EndDate) _
             & ",MinimumPurchase = " & m_DB.NullNumber(MinimumPurchase) _
             & ",MaximumPurchase = " & m_DB.NullNumber(MaximumPurchase) _
             & ",MinimumQuantityPurchase = " & m_DB.NullNumber(MinimumQuantityPurchase) _
             & ",IsItemSpecific = " & CInt(IsItemSpecific) _
             & ",IsFreeShipping = " & CInt(IsFreeShipping) _
             & ",IsActive = " & CInt(IsActive) _
             & ",IsOneUse = " & CInt(IsOneUse) _
             & ",IsUsed = " & CInt(IsUsed) _
             & ",IsRegisterSend = " & CInt(IsRegisterSend) _
             & ",IsProductCoupon = " & CInt(IsProductCoupon) _
             & ",PricingType = " & m_DB.Quote(PricingType) _
             & ",Image = " & m_DB.Quote(Image) _
              & ",IsTotalProduct = " & CInt(IsTotalProduct) _
             & " WHERE PromotionId = " & m_DB.Quote(PromotionId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Overridable Sub UpdateImage()
            Dim SQL As String

            SQL = " UPDATE StorePromotion SET " _
             & "Image = " & m_DB.Quote(Image) _
             & ",ImageMap = " & m_DB.Quote(ImageMap) _
             & ",IsActiveImage = " & CInt(IsActiveImage) _
             & " WHERE PromotionId = " & m_DB.Quote(PromotionId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'UpdateImage

        Public Sub Remove()
            Dim SQL As String

            'SQL = "DELETE FROM StorePromotionItem WHERE PromotionId = " & m_DB.Quote(PromotionId)
            'm_DB.ExecuteSQL(SQL)

            SQL = "DELETE FROM StorePromotion WHERE PromotionId = " & m_DB.Quote(PromotionId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    'Public Class StorePromotionCollection
    '    Inherits GenericCollection(Of StorePromotionRow)
    'End Class

    Public Class StorePromotionCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal StorePromotion As StorePromotionRow)
            Me.List.Add(StorePromotion)
        End Sub

        Public Function Contains(ByVal StorePromotion As StorePromotionRow) As Boolean
            Return Me.List.Contains(StorePromotion)
        End Function

        Public Function IndexOf(ByVal StorePromotion As StorePromotionRow) As Integer
            Return Me.List.IndexOf(StorePromotion)

        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal StorePromotion As StorePromotionRow)
            Me.List.Insert(Index, StorePromotion)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As StorePromotionRow
            Get
                Return CType(Me.List.Item(Index), StorePromotionRow)
            End Get

            Set(ByVal Value As StorePromotionRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal StorePromotion As StorePromotionRow)
            Me.List.Remove(StorePromotion)
        End Sub
    End Class
End Namespace

