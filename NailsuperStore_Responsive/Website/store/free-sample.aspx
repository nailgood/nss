<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/main.master" CodeFile="free-sample.aspx.vb" Inherits="Store_FreeSample" %>
<%@ Register Src="~/controls/product/free-item-page.ascx" TagName="free" TagPrefix="uc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" runat="Server">
    <link rel="canonical" href="<%=Utility.ConfigData.GlobalRefererName  %>/free-samples" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <h1 id="hTitle" runat="server">Free samples with every order</h1>
    <div class="msg" id="divMsg" visible="false" runat="server" clientidmode="Static"> </div>
    <uc1:free ID="ucListItem" runat="server" />
    <script type="text/javascript">
        $(window).load(function () {
            fnGetTopRowTitle($(".dept-desc"), 6, 20, 65, 60, 0, false); //1:container, 2:line show,  3:line height, 4: min space of word last line, 5: min left postion read more, 6: word add,7: end call function
            ResetHeightList('.list .sample-item', 'free-sample');
        });

        $(window).resize(function () {
            ResetHeightList('.list .sample-item', 'free-sample');
        });

        function CheckItem(id) {
            if (document.getElementById('chkSelect_' + id)) {
                if (document.getElementById('chkSelect_' + id).checked == true)
                    document.getElementById('chkSelect_' + id).checked = false;
                else
                    document.getElementById('chkSelect_' + id).checked = true;
            }
        }
        function AddToCart() {
            var lstId = '';
            var arrSelectItem = [];
            if (document.getElementById('hidListId')) {
                lstId = $('#hidListId').val();
                if (lstId != '') {
                    var arr = new Array();
                    arr = lstId.split(',');
                    if (arr.length > 0) {
                        var itemId = '';
                        for (var i = 0; i < arr.length; i++) {
                            itemId = arr[i].toString();
                            if (itemId != '') {
                                if (document.getElementById('chkSelect_' + itemId)) {
                                    var check = $('#chkSelect_' + itemId).is(':checked');
                                    if (check) {
                                        arrSelectItem.push(itemId);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            // alert(arrSelectItem.length);
            if (isCheckOut() == false && arrSelectItem.length < 1) {
                ShowError('Please select at least one item to add to your shopping cart.');
                return false;
            }
            mainScreen.ExecuteCommand('AddCartFreeSample', 'methodHandlers.AddCartFreeSampleCallBack', [arrSelectItem]);
            return false;
        }
        methodHandlers.AddCartFreeSampleCallBack = function (htmlReturn, linkredirect) {

            var htmlError = '';
            if (htmlReturn != null) {
                if (htmlReturn.length > 0) {
                    htmlError = htmlReturn[0];
                }
            }
            if (htmlError != '') {
                ShowError(htmlError);
                return;
            }
            if (linkredirect == '') {
                if (document.getElementById('hidContinueLink')) {
                    linkredirect = $('#hidContinueLink').val();
                }
            }
            if (linkredirect == '') {
                linkredirect = '/store/payment.aspx'
            }
            window.location.href = linkredirect; // '/store/free-sample.aspx';
            return false;
        }

        function isCheckOut() {
            var linkredirect = '';
            if (document.getElementById('hidContinueLink')) {
                linkredirect = $('#hidContinueLink').val();
            }
            if (linkredirect == '/store/cart.aspx') {

                return false;
            }
            return true;
        }
  
    </script>
</asp:Content>
