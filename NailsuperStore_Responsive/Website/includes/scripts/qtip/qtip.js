$(document).ready(function () {

    // Create the modal backdrop on document load so all modal tooltips can use it
    $('<div id="qtip-blanket">')
      .css({
          position: 'absolute',
          top: $(document).scrollTop(), // Use document scrollTop so it's on-screen even if the window is scrolled
          left: 0,
          height: $(document).height(), // Span the full document height...
          width: '100%', // ...and full width

          opacity: 0.7, // Make it slightly transparent
          backgroundColor: 'black',
          zIndex: 5000  // Make sure the zIndex is below 6000 to keep it below tooltips!
      })
      .appendTo(document.body) // Append to the document body
      .hide(); // Hide it initially
  });
  function showQtipID(classCss, msg, title, ID) {
      if (document.getElementById('qtip-blanket')) {
          $('#qtip-blanket').css({        
          top: 0, // Use document scrollTop so it's on-screen even if the window is scrolled          
          height: $(document).height()
      });
      }
      $(document.body).qtip({
          // your options

          show: '',
          hide: false,
          style: {
              classes: { tooltip: classCss }
          },
          position: {
              target: $(document.body), // Position it via the document body...
              corner: 'center' // ...at the center of the viewport
          },
          api: {
              beforeShow: function () {
                  // Fade in the modal "blanket" using the defined show speed
                  $('#qtip-blanket').fadeIn(this.options.show.effect.length);
                  var html = $('.qtip-wrapper').html();
                  //alert(html);
                  $('.qtip-wrapper').removeAttr('style');
                  if (title == 'Notice' || title == 'Success') {
                      $(".qtip-wrapper").html(msg);
                  }
              },
              beforeHide: function () {
                 
                  if (ID != '') {
                      var param = $('#' + ID + ' .qtip-button').attr('param');
                      if (param) {
                          if (param != '') {
                              var index = param.indexOf("_goSec_");
                              if (index > 1) {
                                  var secGo = param.replace('_goSec_', '');
                                  GoItemError(secGo);
                              }
                          }
                      }
                  }
                  // Fade out the modal "blanket" using the defined hide speed
                  $('#qtip-blanket').fadeOut(this.options.hide.effect.length);
              },
              onRender: function () {
                  this.elements.tooltip.attr('id', ID);
              }
          },
          content: {
              prerender: true, // important
              title: {
                  text: title,
                  button: ''
              },

              text: msg
          }
      }).qtip('show');
  }
  function showQtip(classCss, msg, title) {
      showQtipID(classCss, msg, title, '');
  }

  function showQtipFix(classCss, msg, title) {
        var ID = '';
      if (document.getElementById('qtip-blanket')) {
          $('#qtip-blanket').css({
              top: 0, // Use document scrollTop so it's on-screen even if the window is scrolled          
              height: $(document).height()
          });
      }
      $(document.body).qtip({
          // your options

          show: '',
          hide: false,
          style: {
              classes: { tooltip: classCss }
          },
          position: {
              target: $(document.body), // Position it via the document body...
              corner: 'center' // ...at the center of the viewport
          },
          api: {
              beforeShow: function () {
                  // Fade in the modal "blanket" using the defined show speed
                  $('#qtip-blanket').fadeIn(this.options.show.effect.length);
                  var html = $('.qtip-wrapper').html();
                  //alert(html);
                  $('.qtip-wrapper').removeAttr('style');
                  if (title == 'Notice' || title == 'Success') {
                      $(".qtip-wrapper").html(msg);
                  }
              },
              beforeHide: function () {

                  if (ID != '') {
                      var param = $('#' + ID + ' .qtip-button').attr('param');
                      if (param) {
                          if (param != '') {
                              var index = param.indexOf("_goSec_");
                              if (index > 1) {
                                  var secGo = param.replace('_goSec_', '');
                                  GoItemError(secGo);
                              }
                          }
                      }
                  }
                  // Fade out the modal "blanket" using the defined hide speed
                  $('#qtip-blanket').fadeOut(this.options.hide.effect.length);
              },
              onRender: function () {
                  this.elements.tooltip.attr('id', ID);
              }
          },
          content: {
              prerender: true, // important
              title: {
                  text: title
              },

              text: msg
          }
      }).qtip('show');
  }