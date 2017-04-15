<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ConfirmReview.aspx.vb" Inherits="store_ConfirmReview" %>
<%@ Register TagPrefix="CC" TagName="OrderDetail" Src="~/controls/product/order-review-detail.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Preview Product</title>
    <style>
        body
        {
            color: #454545;
            font: 12px/18px Arial;
        }       
    </style>
</head>
<body style="color: #454545;font: 12px/18px Arial;">
    <form id="form1" runat="server">
    <div>
        <table cellspacing="0" cellpadding="0"  style="width: 850px; height: 100%; border: solid 1px #dadada;color: #454545;font: 12px/18px Open Sans;">
            <tr>
                <td>
                    <table  cellspacing="0" cellpadding="0" border="0" style="background-color: #f5f5f5;height: 93px;padding-bottom: 0;padding-left: 1px;padding-right: 1px;width: 849px;border-top: solid 1px #CCCCCC;border-bottom: solid 1px #CCCCCC;">                    
                        <tr>
                            <td style="float: left;padding-left: 10px;padding-top: 5px;text-align: left;width: 300px;">
                                <a href="<%=Utility.ConfigData.GlobalRefererName() %>"><img alt="Logo" style="border: medium none;" src="<%=Utility.ConfigData.GlobalRefererName() %>/includes/theme/images/logo_send.png"></a>
                            </td>
                            <td style="padding-right: 20px;text-align: right;list-style-type: none;font-family: Open Sans;font-size: 13px;line-height: 1.3;font-weight: 600; color: #59595b;">
                                <div>
                                    3804 Carnation St
                                </div>
                                <div>
                                    Franklin Park, IL 60131, USA
                                </div>
                                <div>
                                    <a style="color:#3b76ba;" href="mailto:customerservice@nss.com">customerservice@nss.com</a>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <CC:OrderDetail ID="od" runat="server"></CC:OrderDetail>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
