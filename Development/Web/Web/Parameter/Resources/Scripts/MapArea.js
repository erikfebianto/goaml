var drawingManager;
var selectedShape;
var jsondata = {};
var DataAllMarker;
var DataLastMarker;
var marker;
var markers=[];
function LoadMarker(Data) {
    DataAllMarker = Data;
}

function DrawingMarker(map) {
    if (DataAllMarker !== undefined) {
        var i = 0
        var marker_color = "DC002B";
        var marker_text_color = "FFFFFF";
        markers = [];

        $.each(DataAllMarker, function (idx, obj) {
            var eLat = obj.Lat;
            var eLng = obj.Lng;
            var myCoordinates = [];
            i+=1
            myCoordinates.push(new google.maps.LatLng(eLat, eLng));
            
            //var image = 'http://localhost/nawacoll/images/LocationMarker32.png';
            var image = "http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=" + i + "|" + marker_color + "|" + marker_text_color
            marker = new google.maps.Marker({
                position: { lat: eLat, lng: eLng },
                icon:image
            });

            marker.setMap(map);

            markers.push(marker);

            google.maps.event.addListener(marker, 'click', function (e) {
                PopupWindow(marker, map);
            });
        });
    }
}

function DrawingOfficePos(map) {
    var str = '(-6.227699715564607, 106.65933966636658),(-6.22760372586425, 106.66099190711975),(-6.226707821148154, 106.66133522987366),(-6.225395957913407, 106.66159272193908),(-6.225641265921823, 106.66367411613464),(-6.227774374208297, 106.6631269454956),(-6.2286702771045315, 106.66288018226623),(-6.230430508623315, 106.66237592697143),(-6.230771803418025, 106.6597580909729)';
    var temp = str.split("),(");
    var OfficerAreaCoor = [];
    for (a in temp) {
        temp[a] = temp[a].replace("(", "");
        temp[a] = temp[a].replace(")", "");
        var temp1 = temp[a].split(",")
        OfficerAreaCoor.push(new google.maps.LatLng(temp1[0], temp1[1]));
    }

    if (DataOfficerPos !== undefined) {

        $.each(DataOfficerPos, function (idx, obj) {
            var eLat = obj.Lat;
            var eLng = obj.Lng;
            var myCoordinates = [];
            myCoordinates = new google.maps.LatLng(eLat, eLng);
            var OfficerArea = new google.maps.Polygon({ paths: OfficerAreaCoor });
            var resultColor =
                google.maps.geometry.poly.containsLocation(myCoordinates, OfficerArea) ?
                'red' :
                'green';

            var marker;
            var image;
            if (resultColor !== "green") {
                image = 'http://localhost/nawadev/images/LocationMarker32.png';
                marker = new google.maps.Marker({
                    position: myCoordinates,
                    icon: image
                });
            }
            else {
                image = 'http://localhost/nawadev/images/user_warning.png';
                marker = new google.maps.Marker({
                    position: { lat: eLat, lng: eLng },
                    icon: image
                })
            }
            marker.setMap(map);
            google.maps.event.addListener(marker, 'click', function (e) {
                PopupWindowMarker(marker, map);
            });
        });
    }
}

function DrawingUserPos(map) {
    if (DataAllMarker !== undefined) {
        
        var marker_color = "DC002B";
        var marker_text_color = "FFFFFF";
        markers = [];
        $.each(DataAllMarker, function (idx, obj) {
            var eCurrLat = obj.CurrLat;
            var eCurrLng = obj.CurrLng;
            var eLastLat = obj.LastLat;
            var eLastLng = obj.LastLng;
            var eUserID=obj.UserID;
            var image = 'http://localhost/NawaDev/images/UserRed.png';

            var myCoordinates;
            var as = $(DataLastMarker).filter(function (i, n) { return n.UserID === obj.UserID });
            var i = 0
            var x = 'false';
            var bounds;
            // Set Data Count
            if (as.length >= 3) {
                $.each(as, function (idy, objLast) {
                    myCoordinates = new google.maps.LatLng(objLast.Lat, objLast.Lng);
                    i += 1;
                    if (i == 1) {
                        var circle;
                        if (eLastLat != undefined) {
                            circle = new google.maps.Circle({
                                clickable: false,
                                center: { lat: eLastLat, lng: eLastLng },
                                radius: 1000,// in metters
                                fillOpacity: 0,
                                strokeOpacity: 0,
                                strokeWeight: .8
                            });
                            circle.setMap(map);

                            bounds = circle.getBounds()
                        }
                    }
                    else {
                        if (bounds != undefined) {
                            if (bounds.contains(myCoordinates) == true) {
                                image = 'http://localhost/NawaDev/images/People-Alert-red2.gif';
                            }
                            else {
                                image = 'http://localhost/NawaDev/images/UserRed.png';
                                return false;
                            }
                        }
                    }
                })
            };

            marker = new google.maps.Marker({
                position: { lat: eCurrLat, lng: eCurrLng},
                icon: image,
                optimized: false
            });

            marker.setMap(map);

            markers.push(marker);

            google.maps.event.addListener(marker, 'click', function (e) {
                var contentString = '<div style="width:100px;height:35px;margin-top: 0px"><div style="background:Transparent"><p style="font-size:12px;color:Transparent">' +
                        eUserID.toUpperCase(); + '</p></div></div>'

                var infoBubble = new InfoBubble({
                    content: contentString,
                    position: { lat: eCurrLat, lng: eCurrLng },
                    shadowStyle: 0,
                    padding: 0,
                    borderRadius: 5,
                    borderWidth: 0,
                    hideCloseButton: true
                });
                infoBubble.open(map);
            });

        });
    }
}


function PopupWindow(xSign,map) {
    var xDateTime;
    var xUserName;

    if (DataAllMarker == undefined) {
        return false;
    }

    $.each(DataAllMarker, function (idx, obj) {
        if (xSign.getPosition().lat() == obj.Lat || xSign.getPosition().lng() == obj.Lng) {
            xDateTime = obj.ActivityDatetime;
            var re = /-?\d+/;
            var m = re.exec(obj.ActivityDatetime);
            var myDate = new Date(parseInt(m[0]));
            xDateTime = moment(myDate).format('DD-MMM-YYYY hh:mm:ss');
            xUserName = obj.UserID;
        }
    });

    var contentString = '<div style="width:200px;height:60px;margin-top: 5px"><div style="background:#3979B6"><p style="font-size:14px;color:White"><b>User :' +
        xUserName.toUpperCase() +'</b></div></p><b>Date Time  :</b>' + xDateTime +'</div>'   

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