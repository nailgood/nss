<%@ Control Language="VB" AutoEventWireup="false" CodeFile="addthis.ascx.vb" Inherits="controls_layout_addthis" %>
<script type="text/javascript" src="//s7.addthis.com/js/300/addthis_widget.js#pubid=ra-520b2d5b4eb4b59a"></script>
<div class="addthis_toolbox addthis_default_style addthis_20x20_style" addthis:url="<%=shareURL %>" addthis:title="<%=shareDescription %>">
    <a class="addthis_button_facebook"></a><a class="addthis_button_twitter"></a><a class="addthis_button_pinterest_share">
    </a><a class="addthis_button_google_plusone_share"></a>
</div>
<script type="text/javascript" language="javascript">
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    function EndRequestHandler(sender, args) {

        $(".addthis_toolbox").each(function () {
            try {
                var shareConfig = toolBoxShareConfigs[this.id];
            }
            catch (err) {
                var shareConfig = {
                    passthrough: {
                        twitter: {
                            via: "<%=shareURL %>"
                        }
                    }
                };
            }
            addthis.toolbox(this, addthis_config, shareConfig);
        });
    }
    var addthis_share = {
        // ... other options
        url_transforms: {
            shorten: {
                twitter: 'bitly'
            }
        },
        shorteners: {
            bitly: {}
        }
    }
</script>