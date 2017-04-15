<%@ Page Title="" Language="VB" MasterPageFile="~/includes/masterpage/interior.master"
    AutoEventWireup="false" CodeFile="about-directions-to-our-warehouse.aspx.vb"
    Inherits="services_about_directions_to_our_warehouse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <script type="text/javascript" src="https://maps.google.com/maps/api/js?sensor=false"></script>
    <script type="text/javascript">
        function initialize() {
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
                    var pointsArray = [];
                    pointsArray = response.routes[0].overview_path;
                    var point1 = new google.maps.Marker({
                        position: pointsArray[0],
                        draggable: true,
                        map: map,
                        flat: true
                    });
                    var point2 = new google.maps.Marker({
                        position: pointsArray[1],
                        draggable: true,
                        map: map,
                        flat: true
                    });
                    var point3 = new google.maps.Marker({
                        position: pointsArray[2],
                        draggable: true,
                        map: map,
                        flat: true
                    });
                    var point4 = new google.maps.Marker({
                        position: pointsArray[3],
                        draggable: true,
                        map: map,
                        flat: true
                    });
                    // Display the distance:
                    document.getElementById('distance').innerHTML = 'Distance :' +
            response.routes[0].legs[0].distance.value + " meters";

                    //document.getElementById('distance').style.display = '';
                    directionsDisplay.setDirections(response);
                }
            });
        }
        google.maps.event.addDomListener(window, 'load', initialize);
    </script>
    <div id="contact_form" class="form-horizontal" role="form" runat="server">
        <div class="panel-content">
            <h1 class="formtitle">
                Directions to Our Warehouse</h1>
            <div class="content">
                <div class="form-group">
                    <div class="col-sm-3 hidden-xs text-right">
                        From Address</div>
                    <div class="col-sm-5  txt-required">
                        <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control" placeholder="From Address"></asp:TextBox>
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
                        <asp:TextBox ID="txtTo" runat="server" Enabled="false" CssClass="form-control" placeholder="3804 Carnation St, Franklin Park, IL, USA" ></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-3 text-right">
                    </div>
                    <div class="col-sm-8 content">
                        <input type="button" onclick="SetMap()" class="btn btn-submit" id="btnSubmit"  value="Get Directions" name="btnSubmit" />
                    </div>
                </div>
                <div id="directionsPanel"></div>
                <div id="map"></div>
                <div id="distance"> </div>
            </div>
        </div>
    </div>
</asp:Content>
