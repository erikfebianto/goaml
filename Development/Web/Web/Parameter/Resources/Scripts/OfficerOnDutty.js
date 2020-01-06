var drawingManager;
var selectedShape;
var jsondata = {};
var DataAllArea;
var DataOfficerPos;

function LoadAllData(DataAll) {
    DataAllArea = DataAll;
}

function LoadOfficerOnDuty(DataOffice) {
    DataOfficerPos = DataOffice;
}

function DrawingAllArea(map) {
    if (DataAllArea !== undefined) {
        $.each(DataAllArea, function (idx, obj) {
            var str = obj.Coordinates;
            var temp = str.split("),(");
            var myCoordinates = [];
            for (a in temp) {
                temp[a] = temp[a].replace("(", "");
                temp[a] = temp[a].replace(")", "");
                var temp1 = temp[a].split(",")
                myCoordinates.push(new google.maps.LatLng(temp1[0], temp1[1]));
            }

            //var bgcolor = obj.BgColor
            var bgcolor = '#3DC3DD';


            var PolygonOpt = {
                path: myCoordinates,
                strokeWeight: 3,
                strokeColor: '#3979B6',
                fillOpacity: 0.30,
                fillColor: bgcolor,
                editable: false,
                draggable: false,
                map: map
            };

            var newSign = new google.maps.Polygon(PolygonOpt);

            google.maps.event.addListener(newSign, 'click', function (e) {
                PopupWindow(newSign, map);
            });
        });
    }
}

function DrawingOfficePos(map) {
    if (DataOfficerPos !== undefined) {

        $.each(DataOfficerPos, function (idx, obj) {
            var eLat = obj.Lat;
            var eLng = obj.Lng;
            var myCoordinates = [];

            myCoordinates.push(new google.maps.LatLng(eLat, eLng));

            //var image = 'https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png';
            var image = 'http://localhost/MyJababeka/images/LocationMarker32.png';
            //var image = '../images/LocationMarker.png';
            var marker = new google.maps.Marker({
                position: { lat: eLat, lng: eLng },
                icon: image
            });

            marker.setMap(map);

            google.maps.event.addListener(marker, 'click', function (e) {
                PopupWindowMarker(marker, map);
            });
        });
    }
}

function trafficLayer(map) {
    var trafficLayer = new google.maps.TrafficLayer();
    trafficLayer.setMap(map);
}


function PopupWindow(xSign, map) {
    var xCoordinates;
    var xAreaName;
    var xAreaType;
    var xIDArea;
    var xInfo;

    if (DataAllArea == undefined) {
        return false;
    }

    $.each(DataAllArea, function (idx, obj) {
        if (xSign.getPath().getArray() == obj.Coordinates) {
            xCoordinates = obj.Coordinates;
            xAreaName = obj.AreaName;
            xAreaType = obj.AreaTypeName;
            xIDArea = obj.IDArea;
            xInfo = obj.Info;
        }
    });

    var contentString = '<p><b>' + xAreaType + ' - ' + xAreaName +
        '<br/><b>Information :</b>' + xInfo + '</p>'

    var infowindow = null;

    infowindow = new google.maps.InfoWindow({
        content: contentString
    });

    google.maps.event.addListener(xSign, 'mouseout', function () {
        infowindow.close();
    });

    var Position;
    var myCoordinates = [];
    var bounds = new google.maps.LatLngBounds();

    Position = xSign.getPath().getArray();

    for (i = 0; i < Position.length; i++) {
        bounds.extend(Position[i]);
    }

    infowindow.close();
    infowindow.setPosition(bounds.getCenter());
    infowindow.open(map, xSign);
}

function PopupWindowMarker(xSign, map) {
    var xDateTime;
    var xUserName;
    var xDepartment;

    if (DataOfficerPos == undefined) {
        return false;
    }

    $.each(DataOfficerPos, function (idx, obj) {
        if (xSign.getPosition().lat() == obj.Lat || xSign.getPosition().lng() == obj.Lng) {
            xDateTime = obj.CreatedDate;
            var re = /-?\d+/;
            var m = re.exec(obj.CreatedDate);
            var myDate = new Date(parseInt(m[0]));
            xDateTime = moment(myDate).format('DD-MMM-YYYY hh:mm:ss');
            xUserName = obj.OfficerName;
        }
    });

    var contentString = '<div style="width:200px;height:60px;margin-top: 5px"><div style="background:#3979B6"><p style="font-size:14px;color:White"><b>User :' +
        xUserName.toUpperCase() + '</b></div></p><b>Date Time  :</b>' + xDateTime + '</div>'

    google.maps.event.addListener(xSign, 'mouseout', function () {
        infoBubble.close();
    });

    infoBubble = new InfoBubble({
        content: contentString,
        shadowStyle: 0,
        padding: 0,
        borderRadius: 5,
        borderWidth: 0,
        hideCloseButton: true
    });

    infoBubble.open(map, xSign);
}