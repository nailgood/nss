<%@ Control Language="VB" AutoEventWireup="false" CodeFile="search-bar.ascx.vb" Inherits="modules_SearchBar" %>
<%@ Import Namespace="Utility" %>
<!--search bar-->
<script src="/includes/scripts/XmlHttpLookup.js" type="text/javascript" defer="defer"></script>
<script type="text/javascript">
<!-- 
    var lookupField;
    var cache = new Object();
    if (window.addEventListener) {
        window.addEventListener('load', InitializeQuery, false);
    } else if (window.attachEvent) {
        window.attachEvent('onload', InitializeQuery);
    }

    function InitializeQuery() {
        InitQueryCode('LookupField', '<%= IIf(ConfigData.UseSolr, ConfigData.SolrServerURL, "/includes/scripts/ajax.aspx?f=DisplayItemsSearchLucene&q=")%>', 'varHidden' );
	}
	function MyCallback(ItemId) {
	    window.location = <%=up %> + ItemId;
    }
    function MyCallbackKeyword(Keyword) {
        document.getElementById('<%=lnkSearch.ClientID %>').click();
    }
 
    $(window).load(function () {
  
        $('#LookupField').live("keyup", function(e) {
            if (e.keyCode == 13) {
                Search_click();
                return false; // prevent the button click from happening
            }
        });
        $('#LookupField').focusin(function(e) {
            var check = false;
            if ($(window).width() < 768)
                check = true;

            if(check){
                $("#background-search").show();
                $(window).scroll(function(){
                    var height = $(window).scrollTop();
                    var heightSearchResult = $("#varHidden").height();
                    var heightWindow = $(window).height();
                    //console.log(heightSearchResult - heightWindow);
                    if(height >= 30 || height-heightSearchResult > 0){
                        $("#background-search").css('top', 0);
                    }
                    else
                    {
                        $("#background-search").css('top', 57);
                    }          
                });
            }
        });
        $('#LookupField').focusout(function(e){
            $("#background-search").hide();
        });

    });

    //-->
</script>
<div class="input-group group-search" onkeydown = "return (event.keyCode!=13)">
    <input type="text" id="LookupField" name="LookupField" class="form-control" placeholder="Search by keyword or item #" />
    <span class="input-group-addon" onclick="Search_click()"><span class="ic-search"></span><span class="text-search">Search</span>
    </span>
    <input type="hidden" id="LookupHidden" name="LookupHidden" value="" />
    <input type="hidden" id="LookupKeyword" name="LookupKeyword" value="" />
    <asp:LinkButton ID="lnkSearch" runat="server"></asp:LinkButton>
    <div id="varHidden" class="varHidden" ></div>
    <input type="hidden" id="isLogin" value="<%= Utility.Common.GetCurrentMemberId() %>" />
</div>

<script type="text/javascript">
    function Search_click() {

        var keyword = $('#LookupField').val().replace('=', '');
        if (keyword == '') {
            return;
        }

        // __doPostBack(id, '');
        $.ajax({
            url: '/store/search-result.aspx/RedirectSearchPage',
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: "{queryString: '"+keyword.replace("'", '').replace("\\", "") + "'}",
            failure: function (response) {
            },
            success: function (response) {
                if(response.d) {
                    var data = $.parseJSON(response.d);
                    window.location = data.url;
                }
            }
        });
    }
   
    
    
</script>
<!--end search bar-->
