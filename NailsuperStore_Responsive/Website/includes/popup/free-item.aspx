<%@ Page Language="VB" AutoEventWireup="false" CodeFile="free-item.aspx.vb" Inherits="Components.Popup_FreeItem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
     <meta name="viewport" content="width=device-width, initial-scale=1">
     <link href='//fonts.googleapis.com/css?family=Open+Sans:400,600' rel='stylesheet' />
    <asp:Literal ID="litCSS" runat="server"></asp:Literal>
    <asp:Literal ID="litScript" runat="server"></asp:Literal>
    <meta name="robots" content="noindex, nofollow" />
</head>
<body>
    <form id="form1" runat="server">
     <asp:ScriptManager runat="server" EnablePageMethods="true" ID="MainSM" runat="server"
        ScriptMode="Release" LoadScriptsBeforeUI="true">
        <Scripts>
            <asp:ScriptReference Path="~/includes/scripts/command.js" />
        </Scripts>
    </asp:ScriptManager>
     <center>
       <div id="loading" class="bg-loading" style="display: none;">Please wait...<br /><img src="/includes/theme/images/loader.gif" alt="" /></div>                      
      </center>
      <div id="promotion-main">
        <div class="header">
            <div class="pro-header-wrapper">
                <div class="panel">
                    <div class="msg"></div>
                    <div class="free-add-btn">
                       <asp:Literal ID="ltrButton" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="error" id="divError" style="display:none" ></div>
            </div>
        </div>
        <div id="main" class="cart list" runat="server">
            <asp:Literal ID="ltrItemList" runat="server">
            </asp:Literal>
            <input type="hidden" id="hidListId" runat="server"  value="" />
         </div>
    </div>
    <input type="hidden" id="hidQtyAllow" value="<%=TotalFreeAllowed %>"" />
    <input type="hidden" id="hidQtyChoose" value="<%=TotalFreeCanChoose %>"" />
    </form>    
  <script type="text/javascript">
      $(document).ready(function () {

          $('.addedItem').each(function (i, item) {
              var id = $(item).find('input.txt-qty.added').attr('name');
              $('input[id=' + id + ']').last().parent().parent().parent().parent().hide();
          });

          if ($('.item:not(.addedItem):visible').length == 0)
              $('#endAddedItem').hide();
          else
              $('#endAddedItem').show();

          $('.txt-qty').on('input', function (e) {
              var _this = this;
              var value = $(_this).val();
              var id = $(_this).attr('id');
              if (id.length > 0 && $('[id="' + id + '"]').length > 1)
                  $('[id="' + id + '"]').each(function (index, item) {
                      $(item).val(value);
                      fnGetvalQty(0);
                  });
          });

          $('.min').each(function (item) {
              $(this).click(function () {
                  var input = $(this).next().find('input').attr('id');
                  var first = ($('#' + input));
                  setTimeout(function () {
                      $(first).trigger('input');
                  }, 50);
              });
          });
          $('.plus').each(function (item) {
              $(this).click(function () {
                  var input = $(this).next().next().find('input').attr('id');
                  var first = ($('#' + input));
                  setTimeout(function () {
                      $(first).trigger('input');
                  }, 50);
              });
          });

      });
     
      CloseFreeItem = function () {
          if (window.parent.document.getElementById('closeBut')) {
              window.parent.document.getElementById('closeBut').click();
          }
      }

      methodHandlers = {};
      methodHandlers.InsertCartFreeItemCallBack = function (html, cartItemCount, linkredirect) {
          if (linkredirect != '') {
              parent.document.location.href = linkredirect; // '/store/free-sample.aspx';   
          }

          var htmlSumaryBox = '';
          var htmlCart = '';
          var htmlError = '';
          if (html) {
              htmlCart = html[0];
              htmlSumaryBox = html[1];
              htmlError = html[2];
          }

          if (htmlError != '') {
              ShowErrorPopupReviseFreeItem(htmlError);
              return;
          }
          if (window.parent) {
              if (window.parent.document.getElementById('divListCart')) {
                  window.parent.document.getElementById('divListCart').innerHTML = htmlCart;
              }
              window.parent.UpdateCartSummaryBox(htmlSumaryBox)

              if (window.parent.document.getElementById('closeBut')) {
                  window.parent.document.getElementById('closeBut').click();
              }

          }

      }
      InputCartQty = function ($id, txt, event) {
          if (event.keyCode == 13) {
              event.preventDefault();
          }
          if (numbersonly(txt, event)) {
                DetectQty(txt);
              //$('#pnUpdate_' + $id).show();
          }
      }

      CancelUpdate = function (id, txt) {
          $qty = $('#hid' + id).val();

          var updateElements = $('.update-ctr:visible');
          var totalSelect = 0;
          var totalqty = $('#hidQtyChoose').val();
          updateElements.each(function (index, element) {
              if(element == undefined) return;
              var idElement = $(element).attr('id').split('_')[1];
              var count = $('#txtQty_' + idElement).val();
              if (count > 0)
                  totalSelect += count;
          });
          if (updateElements.length == 1 || (totalqty >= totalSelect)) {
              $('#divError').html('');
              $('#divError').hide();
              $('#spanError').remove();
          }
          //$('#' + txt).val($qty);
          $('[id=' + txt + ']').each(function (index, item) {
              $(item).val($qty);
          });

          $('[id=pnUpdate_' + id + '').hide();
          $('#pnUpdate_' + id + '').hide();
          if ($qty > 0) {
              $('[id=lblInCart' + txt + ']').show();
          }
          fnGetvalQty();
      }

      DetectQty = function (txt, isPlus, _this) {
          
          $id = txt.replace('txtQty_', '');
          $totalqty = $('#hidQtyChoose').val();
          $qty = $('#' + txt).val();
          if (isPlus) {

              if ($totalqty > 0 && $qty >= 0) {
                  $('[id=pnUpdate_' + $id + ']').show();
                  $('[id=lblInCart' + txt + ']').hide();
              }
          }
          else {
              if ($qty >= 0) {
                  $('[id=pnUpdate_' + $id + ']').show();
                  $('[id=lblInCart' + txt + ']').hide();
              }
          }

          $val = $('#hid' + $id).val();
          if ($totalqty <= ($qty - $val)) {
              if (isPlus)
              {
                  $('#' + txt).val(parseInt($('#' + txt).val()) - 1);
              }
          }
          $('[id=pnUpdate_' + $id + ']').each(function (idx, item) {

              var text = $(item).next().text();
              if (!text.length) {
                  if (text.length == 0)
                      $(item).children().first().text("Add");
              }
              else {
                  $(item).children().first().text("Update");
              }
          });
          fnGetvalQty();
      }

      DeleteCartItemMixMatch = function (ItemId) {
          $('#lblInCarttxtQty_' + ItemId).html('');
          $('[id=pnUpdate_' + ItemId + ']').hide();
          mainScreen.ExecuteCommand('DeleteCartItemMixMatch', 'methodHandlers.DeleteCartItemMixMatchCallBack', [ItemId]);
          return false;
      }

      OnChangeQty = function (mmId, TotalFreeAllowed, FreeItemIds, Id, txt) {
          var qty = parseInt($('#' + txt).val());
          if (qty > 0) {
              mainScreen.ExecuteCommand('InsertCartFreeDiscountItem', 'methodHandlers.InsertCartFreeDiscountItemCallBack', [mmId, TotalFreeAllowed, FreeItemIds, Id, qty]);
          }
          else {
              $qtyhid = $('#hid' + Id).val();
              if ($qtyhid > 0)
              {
                  $('#txtQty_' + Id).val($qtyhid);
                  mainScreen.ExecuteCommand('DeleteCartItemMixMatch', 'methodHandlers.DeleteCartItemMixMatchCallBack', [Id]);
              }
              $('[id=pnUpdate_' + Id+ ']').hide();
          }
          return false;
      }

      methodHandlers.DeleteCartItemMixMatchCallBack = function (html, ItemId) {
          var htmlError = '';
          if (html) {
              htmlError = html[0];
          }
          if (htmlError != '') {
              ShowErrorPopupReviseFreeItem(htmlError);
          }

          $('#divError').html('');
          $('#divError').hide();
          $('#spanError').remove();
          $totalqty = $('#hidQtyChoose').val();

          $totalremove = $('#txtQty_' + ItemId).val();

          $totalqty = parseInt($totalqty) + parseInt($totalremove);
          $('#txtQty_' + ItemId).val('0');

          
          $('#hidQtyChoose').val($totalqty);
          

          $qtyallow = $('#hidQtyAllow').val();
          ShowMessage($qtyallow, $totalqty)

          //remove first id
          if ($('input[id=txtQty_' + ItemId + ']').length > 1) {
              var itemRemove = $('input#txtQty_' + ItemId).parent().parent().parent().parent();
              if (itemRemove.length == 2) {
                  itemRemove[0].remove();
                  if ($('.addedItem').length == 0)
                      $('#endAddedItem').remove();

              }
              $('#txtQty_' + ItemId).val('0');
              $('[id="lblInCarttxtQty_' + ItemId + '"]').html('');

              $('input#txtQty_' + ItemId).parent().parent().parent().parent().show();
          }

          $('#hid' + ItemId).val('0');

          if ($('.item:not(.addedItem):visible').length == 0)
              $('#endAddedItem').hide();
          else
              $('#endAddedItem').show();
          setTimeout(function () {
              fnGetvalQty();
          }, 100)
      }


      methodHandlers.InsertCartFreeDiscountItemCallBack = function (html, ItemId, CountFreeItem, Qty) {
          var htmlError = '';
          if (html) {
              htmlError = html[0];
          }

          if (htmlError != '') {
              //try {
              //    showQtip('qtip-error', htmlError, 'Ooops');

              //} catch (e) {
              //    console.log(e);
              //}
              if (htmlError == 'error') {
                  var number = $('.alladded').text();
                  var s = '';
                  try {
                      var texts = parseInt(number.split(')')[0]);
                      if (texts != 1)
                          s = 's have';
                      else
                          s = ' has';

                  } catch (e) {

                  }
                  if ($('div.msg > #spanError').length == 0) {
                      $('div.msg').append('<span id="spanError" class="error" style="float:none;"> Please revise and try again</span>');
                      $('#divError').hide();
                  }

                  //htmlError = 'All ' + $('.alladded').text() + ' free item' + s + ' been added to your cart. Please revise your free item(s) quantity and try it again';
              }
              //ShowErrorPopupReviseFreeItem('');
              return; 
          }
          else {
              $('#divError').hide();
              $('#divError').html();
          }
          $totalqty = CountFreeItem;
          $totaladd = $('#txtQty_' + ItemId).val();
          //$totalqty = parseInt($totalqty) - parseInt($totaladd);
          $('#hidQtyChoose').val($totalqty);
          $('#hid' + ItemId).val(Qty);
          $('[id=pnUpdate_' + ItemId + ']').hide();
          if (Qty > 0) {
              $('[id^="lblInCarttxtQty_' + ItemId + '"]').show();
              $('[id^="txtQty_' + ItemId + '"]').val(Qty);
              //$('#lblInCarttxtQty_' + ItemId).show();
              //$('#txtQty_' + ItemId).val(Qty);
          }

          $('[id^="lblInCarttxtQty_' + ItemId + '"]').html('Added | <a onclick="DeleteCartItemMixMatch(\'' + ItemId + '\');" class="remove">Remove</a>');

          //added item
          if ($('input[id=txtQty_' + ItemId + ']').length == 1)
          {
              var lastAddedItem = $('.addedItem').last();
              if (lastAddedItem.length > 0) {
                  var addedItem = $('#txtQty_' + ItemId).parent().parent().parent().parent().clone();

                  $('#txtQty_' + ItemId).parent().parent().parent().parent().hide();

                  $(addedItem).find('#txtQty_' + ItemId).addClass('added');
                  addedItem.addClass('addedItem');
                  lastAddedItem.after(addedItem);
              }
              else {
                  if ($('endAddedItem').length == 0)
                      $('#main').prepend("<div id='endAddedItem'></div>");

                  var addedItem = $('#txtQty_' + ItemId).parent().parent().parent().parent().clone();
                  $('#txtQty_' + ItemId).parent().parent().parent().parent().hide();

                  $(addedItem).find('#txtQty_' + ItemId).addClass('added');
                  addedItem.addClass('addedItem');
                  $('#main').prepend(addedItem);

              }
             
              $('.txt-qty').on('input', function (e) {
                  var _this = this;
                  var value = $(_this).val();
                  var id = $(_this).attr('id');
                  if (id.length > 0 && $('[id="' + id + '"]').length > 1)
                      $('[id="' + id + '"]').each(function (index, item) {
                          $(item).val(value);
                          fnGetvalQty(0);
                      });
              });

              $('.min').each(function (item) {
                  $(this).unbind('click').click(function (e) {
                      var input = $(this).next().find('input').attr('id');
                      var first = ($('#' + input));
                      setTimeout(function () {
                          $(first).trigger('input');
                      }, 50);
                  });
              });
              $('.plus').each(function (item) {
                  $(this).unbind('click').click(function (e) {
                      var input = $(this).next().next().find('input').attr('id');
                      var first = ($('#' + input));
                      setTimeout(function () {
                          $(first).trigger('input');
                      }, 50);
                  });
              });
             
          }
          
          $qtyallow = $('#hidQtyAllow').val();
          ShowMessage($qtyallow, $totalqty);

          if ($('.item:not(.addedItem):visible').length == 0)
              $('#endAddedItem').hide();
          else
              $('#endAddedItem').show();

          setTimeout(function () {
              fnGetvalQty();
          }, 100)
      }

      jQuery('.txt-qty').on('input propertychange paste', function () {
            fnGetvalQty(0);//command.js
        });

      $(window).load(function () {
          if($('#main').attr('data-fix'))
              $('.item').height('300px');
          else
            ResetHeightList('.list .item', 'free-item');
          fnGetvalQty(1);

          $qtychoose = $('#hidQtyChoose').val();
          $qtyallow = $('#hidQtyAllow').val();
          ShowMessage($qtyallow,$qtychoose)
      });

      ShowMessage = function(qtyallow, qtychoose)
      {
          $s = '';
          if (qtychoose == 0) {
              if (qtyallow > 1) $s = 's';
              $('div.msg').html('All <span class="alladded">' + qtyallow + ' (' + toWords(qtyallow) + ')</span> free item' + $s + ' added to cart.');
          }
          else {
              $qtyadded = qtyallow - qtychoose;
              if (qtyallow == qtychoose) {
                  if (qtychoose > 1) $s = 's';
                  $('div.msg').html('Choose <span class="noadd">' + qtychoose + ' (' + toWords(qtychoose) + ')</span> more free item' + $s + '.');
              }
              else {
                  if ($qtyadded > 1) $s = 's';
                  $('div.msg').html('<span class="added">' + $qtyadded + ' (' + toWords($qtyadded) + ')</span> free item' + $s + ' added to your cart. Choose <span class="noadd">' + qtychoose + ' (' + toWords(qtychoose) + ')</span> more.');
              }
          }
      }

      $(window).scroll(function () {
          FixPagePopupReviseFreeItem();
      });
      $(window).resize(function () {
          if ($('#main').attr('data-fix'))
              $('.item').height('300px');
          else
              ResetHeightList('.list .item', 'free-item');
          ClearFixHeaderPopupReviseFreeItem();
          FixPagePopupReviseFreeItem();
      });
      

  </script>
</body>
</html>
