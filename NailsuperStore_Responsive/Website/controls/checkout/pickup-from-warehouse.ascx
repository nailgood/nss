<%@ Control Language="VB" AutoEventWireup="false" CodeFile="pickup-from-warehouse.ascx.vb" Inherits="controls_pickupfromwarehouse" %>

<div id="contact_form" class="form-horizontal">
<script type="text/javascript" src="https://maps.google.com/maps/api/js?sensor=false"></script>
<script type="text/javascript">
    function fillAddress() {
        try {

            if ($('#rdoSameAddress').is(":checked"))
                if (rdoBillingAddressChecked != '') {
                    var address = $('#' + rdoBillingAddressChecked + ' .address .value')[0].innerHTML.replace('<br>', ', ');
                    $('#txtFrom').val(address);
                    rdoBillingAddressChecked = '';
                }
                else if ($('#divBillingAddress').css('display') != 'none') {
                    var address = $('#divBillingAddress .address')[0].innerHTML.replace('<br>', ', ');
                    $('#txtFrom').val(address);
                }
                else {
                    var parent = $('input[name="rdoBillingAddress"]:checked').parent().parent().children()[2];
                    address = parent.childNodes[1].childNodes[1].innerHTML.replace('<br>', ', ');
                    $('#txtFrom').val(address);
                }
            else {
                var parent = $('input[name="rdoShippingAddress"]:checked').parent().parent().children()[2];
                address = parent.childNodes[1].childNodes[1].innerHTML.replace('<br>', ', ');
                $('#txtFrom').val(address);
            }
        } catch (e) {

        }

    }
    $(document).ready(function () {
        fillAddress();
    });
    function initialize() {
        document.getElementById("map").style.height = "480px";

        var myLatlng = new google.maps.LatLng(41.947171, -87.888565);
        var mapOptions = {
            zoom: 15,
            center: myLatlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        }
        var map = new google.maps.Map(document.getElementById("map"), mapOptions);

        var marker = new google.maps.Marker({
            position: myLatlng,
            map: map,
            title: "The Nail Superstore"
        });
    }
    function FromEnter(e) {

        if (e.keyCode == 13) {
            SetMap();
            return false;
        }
        return true;
    }
    function SetMap() {
        initialize();
        var from = '';
        if (document.getElementById('txtFrom'))
            from = document.getElementById('txtFrom').value;
        if (from == '') {
            if (document.getElementById('regFrom')) {
                document.getElementById('regFrom').style.display = 'inline';
            }
            return;
        }
        if (document.getElementById('regFrom')) {
            document.getElementById('regFrom').style.display = 'none';
        }

        var directionsService = new google.maps.DirectionsService();
        var directionsDisplay = new google.maps.DirectionsRenderer();

        var myOptions = {
            zoom: 7,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        }
        var map = new google.maps.Map(document.getElementById("map"), myOptions);
        directionsDisplay.setMap(map);
        //display route
        document.getElementById("directionsPanel").innerHTML = '';
        directionsDisplay.setPanel(document.getElementById("directionsPanel"));
        document.getElementById('directionsPanel').style.display = 'block';

        var request = {
            origin: from,
            destination: '3804 Carnation St, Franklin Park, IL, USA',
            travelMode: google.maps.DirectionsTravelMode.DRIVING
        };

        directionsService.route(request, function (response, status) {
            if (status == google.maps.DirectionsStatus.OK) {
                // Display the distance:
                document.getElementById('distance').innerHTML = 'Distance :' +
        response.routes[0].legs[0].distance.value + " meters";

                //document.getElementById('distance').style.display = '';
                directionsDisplay.setDirections(response);
            }
        });
    }

</script>
    <div class="panel-content">
        <%--<h1 class="formtitle">
            Directions to Our Warehouse</h1>--%>
        <div class="content">
            <div class="form-group">
                <div 
                    class="col-sm-3 hidden-xs text-right">
                    From Address</div>
                <div class="col-sm-5  txt-required">
                    <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control" placeholder="From Address" ClientIDMode="Static"></asp:TextBox>
                </div>
                <div class="div-error">
                    <asp:RequiredFieldValidator runat="server" ID="regFrom" ControlToValidate="txtFrom"
                        Display="Dynamic" ValidationGroup="valContact" ErrorMessage="From Address is required"
                        CssClass="text-danger"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-3 hidden-xs text-right">
                    To Address</div>
                <div class="col-sm-5">
                    <asp:TextBox ID="txtTo" runat="server" MaxLength="30" Enabled="false" CssClass="form-control" placeholder="3804 Carnation St, Franklin Park, IL, USA" ></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-3 text-right">
                </div>
                <div class="col-sm-5">
                    <input type="button" onclick="SetMap()" class="btn btn-submit" id="btnSubmit"  value="Get Directions" name="btnSubmit" />
                </div>
            </div>
            <div id="directionsPanel"></div>
            <div id="map"></div>
            <div id="distance"> </div>
        </div>
    </div>
</div>
