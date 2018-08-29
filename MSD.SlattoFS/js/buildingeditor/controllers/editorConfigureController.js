(function () {
    'use strict';
    var injectParams = ['$scope', 'd3Service', 'appConfig', '$http', 'dataModel'];
    var EditorConfigureController = function ($scope, d3Service, appConfig, $http, dataModel) {

        var vm = this;
        vm.Message = "From Configure Controller";
        vm.AssignedApartments = [];
        vm.Apartments = [{
            id: -1,
            name: "Please select an Apartment"
        }];

        vm.BuildingAssets = [];
        vm.SelectedImageUrl = "";
        vm.SelectedImageId = "";
        vm.htmlValue = "";

        vm.IsThumbnailClick = false;

        vm.DataToPass = [];

        vm.ClickedThumbnail = function (asset) {
            vm.SelectedImageUrl = asset.Url;
            vm.SelectedImageId = asset.Id;
            vm.IsThumbnailClick = true;
            GetSvgData();

        };

        vm.SaveSvg = function () {
            SaveUpdateSVG(appConfig.BUILDING_ID, vm.SelectedImageId, angular.element(document.querySelector('svg')).html());
        };

        vm.DefaultHeight = 90;
        vm.DefaultWidth = 150;

        function SaveUpdateSVG(buildingId, assetId, htmlValue) {

            var dataToSend = {
                'buildingId': buildingId,
                'assetId': assetId,
                'svg': htmlValue
            };

            $http({
                url: (appConfig.SAVESVG_URL),
                method: "POST",
                data: JSON.stringify(dataToSend),
                transformRequest: angular.identity,
                headers: { 'Content-Type': 'application/json' }
            }).then(function successCallBack(response) { }, function errorCallBack(response) { });
        }

        function GetSvgData() {

            var dataToSend = {
                'buildingId': appConfig.BUILDING_ID,
                'assetId': vm.SelectedImageId,
                'svg': null
            };

            $http({
                url: (appConfig.GETSVG_URL),
                method: "POST",
                data: JSON.stringify(dataToSend),
                transformRequest: angular.identity,
                headers: { 'Content-Type': 'application/json' }
            }).then(function successCallBack(response) {

                if (response.data.svgData != null) {
                    vm.htmlValue = response.data.svgData.Svg;
                }
                else {
                    vm.htmlValue = "";
                }

                angular.element(document.querySelector('svg')).html(vm.htmlValue);

                d3.LoadEvents();
            }, function errorCallBack(response) { });
        }

        // get building info
        function GetApartments() {

            $http({
                url: appConfig.BUILDING_APARTMENTS_SIMPLE_LIST + appConfig.BUILDING_ID,
                method: "POST",
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            })
            .then(function successCallBack(response) {
                if (typeof response !== 'undefined' && response.data.list !== 'undefined') {
                    angular.forEach(response.data.list, function (value, index) {
                        var apartment = new dataModel.BaseApartment(value);
                        //replace id to apartment id 
                        vm.Apartments.push({ name: apartment["Room"], id: apartment["Id"] });
                    });
                }
            }, function errorCallBack(response) { });
        }

        function GetBuildingAssets() {

            var fd = new FormData();
            fd.append("accountId", appConfig.ACCOUNT_ID);
            fd.append("buildingId", appConfig.BUILDING_ID);
            fd.append("apartmentId", -1);
            $http({
                url: appConfig.MEDIAASSETS_URL,
                method: "POST",
                data: fd,
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            })
            .then(function successCallBack(response) {
                if (typeof response !== 'undefined' && response.data.list !== 'undefined') {
                    angular.forEach(response.data.list, function (value, index) {
                        var buildingAsset = new dataModel.BuildingAsset(value);
                        vm.BuildingAssets.push(buildingAsset);

                        //set default value for the editor
                        if (index == 0) {
                            vm.SelectedImageUrl = buildingAsset["Url"];
                            vm.SelectedImageId = buildingAsset["Id"];
                        }
                        GetSvgData();
                    });

                    //get svg after assigning deafault Value

                }
            }, function errorCallBack(response) { });
        }
        //end get building assets

        //start of load svg
        function LoadSVG(buildingId, assetId) {
            $http({
                url: appConfig.MEDIAASSETS_URL,
                method: "POST",
                data: fd,
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            })
           .then(function successCallBack(response) {
           }, function errorCallBack(response) { });
        }
        //end of load svg

        function Initialize() {

            //this method loads apartment info from the server;
            GetApartments();
            GetBuildingAssets();

            //get the views
            //sample how d3Service is used
            d3Service.d3().then(function (d3) {
                if (d3 !== 'undefined') {

                    //Create a class or id every create of the polygon
                    //get the points value and replace it by its new coordinate
                    var menu = [
                        {
                            title: 'Copy Left',
                            type: 'simple',
                            action: function (elm, d, i) {
                                DrawCopiedRectangle(elm, 0);
                            }
                        },
                        {
                            title: 'Copy Down',
                            type: 'simple',
                            action: function (elm, d, i) {
                                DrawCopiedRectangle(elm, 1);
                            }
                        },
                        {
                            title: 'Copy Right',
                            type: 'simple',
                            action: function (elm, d, i) {
                                DrawCopiedRectangle(elm, 2);
                            }
                        },
                        {
                            title: 'Copy Up',
                            type: 'simple',
                            action: function (elm, d, i) {
                                DrawCopiedRectangle(elm, 3);
                            }
                        },
                        {
                            title: 'Copy',
                            type: 'simple',
                            action: function (elm, d, i) {


                                GetMaxRectangleId();
                                rectangleId++;
                                var newParentId = "rect-" + rectangleId;

                                var newG = d3.select('svg').append('g').attr("id", newParentId)
                                             .attr('class', 'is-main');
                                var parentHtml = d3.select(elm.parentNode).html();
                                var newPoly = newG.html(parentHtml).select("polygon")
                                    .call(dragger)
                                    .on('contextmenu', d3.contextMenu(menu));
                                newG.selectAll("circle").call(dragNode);
                                newPoly.attr("g-id", newParentId);
                                newPoly.attr('class', 'poly');

                            }
                        },
                        {
                            title: 'Delete',
                            type: 'simple',
                            action: function (elm, d, i) {

                                var parentGroup = d3.select(elm.parentNode);
                                var controls = GetControls(parentGroup)

                                UnsetControls(controls);

                                Unassign(parentGroup);
                                parentGroup.remove();
                            }
                        },
                        {
                            title: 'Assigned Apartment',
                            type: 'complex',
                            action: function (elm, d, i) {

                            }
                        },
                        {
                            title: 'Remove Assign',
                            type: 'simple',
                            action: function (elm, d, i) {
                                var parentGroup = d3.select(elm.parentNode);
                                Unassign(parentGroup);
                            }

                        }
                    ];

                    var nodeMenu = [
                            {
                                title: 'delete',
                                action: DeleteNode
                            }
                    ];

                    // global vars
                    var dragging = false, drawing = false, startPoint;
                    var svg = d3.select('.svg-container').append('svg')
                        .style("position", "absolute")
                               .style("z-index", "99")
                               .attr('height', 500)
                               .attr('width', 960)
                               .attr('viewBox','0 0 960 500');

                    //basic config for polygon                    
                    var globalCircleId = 4;

                    //settings
                    var coordinates = [];
                    var createRect = false;
                    var dragging = false;
                    var rectangleId = 0;

                    var newCoordinates = [coordinates.length];

                    d3.LoadEvents = function () {
                        var d3Svg = d3.select("svg");
                        //var lineFirst = parentGroup.select("[point-a='" + draggedCircle.attr("id") + "']");
                        var parentGroup = d3.selectAll("g.is-main");

                        //map polygon to event
                        d3.selectAll("polygon").call(dragger).on('contextmenu', d3.contextMenu(menu));

                        ////map nodes(circles) to event
                        var circles = d3.selectAll("circle").call(dragNode);
                        var circleIds = [];
                        //comment for rebase
                        $.each(circles[0], function (index, value) {
                            var d3Value = d3.select(value);
                            if (value.hasAttribute("is-main") == false) {
                                d3Value.on('contextmenu', d3.contextMenu(nodeMenu));
                            }
                            circleIds.push(parseInt(d3Value.attr("id").split("-")[1]));
                        });

                        SetMaxCircleId(circleIds);
                        HideAllCircles();

                        if (parentGroup[0].length == 0) {
                            rectangleId = 0;
                            RemoveAllAssigned();

                        } else {
                            if (vm.IsThumbnailClick) {
                                rectangleId = 0;
                                UnAssignDefaultApartment(parentGroup[0]);
                            } else {

                                $.each(parentGroup[0], function (index, value) {
                                    var group = d3.select(value);
                                    var groupId = group.attr("id");
                                    var aptId = group.attr("apt-id");

                                    AssignDefaultApartment(aptId);

                                });

                                GetMaxRectangleId();
                            }
                        }

                    };

                    function HideAllCircles() {
                        d3.selectAll("circle").attr("class", "hide-circle");
                    }

                    function SetPolygonAsSelected(parentNode) {
                        HideAllCircles();
                        parentNode.selectAll("circle").attr("class", "show-circle");
                    }

                    function RemoveAllAssigned() {
                        vm.AssignedApartments = $.grep(vm.AssignedApartments, function (e) {

                            if (e.id != -1) {
                                //push the id to unassign
                                vm.Apartments.push({ id: e.id, name: e.name });
                            }

                        });
                    }

                    function AssignDefaultApartment(aptId) {
                        vm.Apartments = $.grep(vm.Apartments, function (e) {
                            if (e.id != aptId) {
                                return e.id != aptId;
                            }
                            else {
                                vm.AssignedApartments.push({ id: e.id, name: e.name });
                            }
                        });
                    }

                    function UnAssignDefaultApartment(parentGroup) {

                        //this unassign apartment
                        $.each(parentGroup, function (index, value) {
                            var group = d3.select(value);
                            var groupId = group.attr("id");
                            var aptId = group.attr("apt-id");

                            vm.AssignedApartments = $.grep(vm.AssignedApartments, function (e) {

                                if (e.id != aptId) {
                                    //push the id to unassign
                                    vm.Apartments.push({ id: e.id, name: e.name });
                                }
                                else {
                                    return e.id;
                                }
                            });
                        });

                        //assign
                        var counts = [];
                        $.each(parentGroup, function (index, value) {
                            var group = d3.select(value);
                            var groupId = group.attr("id");
                            var aptId = group.attr("apt-id");

                            AssignDefaultApartment(aptId);

                        });

                        GetMaxRectangleId();
                    }

                    //TO DO change to Ini
                    d3.CreateRectangle = function () {
                        createRect = true;
                        drawing = false;

                    };

                    svg.on("mouseup", function () {

                        var points = d3.mouse(this);

                        if (createRect) {
                            HideAllCircles();
                            DrawRectangle(points);
                        } else {

                            var target = d3.select(d3.event.target);
                            var parent = d3.select(d3.event.target.parentNode);

                            if (d3.event.target.hasAttribute("class") && (target.attr("class") == "poly" || target.attr("class") == "poly-selected")) {
                                SetPolygonAsSelected(parent);
                            } else if (d3.event.target.hasAttribute("segment")) {
                                CreateChildNode(d3.event.target, points);
                                SetPolygonAsSelected(parent);
                            }
                        }
                    });

                    //behaviors
                    var dragger = d3.behavior.drag()
                        .on('drag', DragPolygon)
                        .on('dragend', function () {

                            dragging = false;
                        });

                    var dragNode = d3.behavior.drag()
                        .on('drag', DragNode)
                        .on('dragend', function () {
                            dragging = false;
                        });

                    function DeleteNode(elm, d, i) {
                        var parentGroup = d3.select(elm.parentNode);
                        var polygon = parentGroup.select("polygon");
                        var segments = parentGroup.selectAll("polyline");
                        var circles = parentGroup.selectAll("circle");

                        var selectedCircle = d3.select(elm);
                        var selectedCircleId = selectedCircle.attr("id");

                        //two segment that are connected by the selectedCircle
                        var affectedSegment = [];
                        var newPoints = [];

                        d3.select(elm).remove();

                        var segmentsToRemove = [];
                        var segmentFirst = null, segmentSecond = null;

                        $.each(segments[0], function (index, value) {
                            var selectedValue = d3.select(value);

                            if (segmentFirst != null && segmentSecond != null) {
                                return false;
                            }

                            if (selectedValue.attr("point-a") == selectedCircleId) {
                                segmentFirst = selectedValue;
                                selectedValue.remove();
                            }
                            if (selectedValue.attr("point-b") == selectedCircleId) {
                                segmentSecond = selectedValue;
                                selectedValue.remove();
                            };
                        });

                        var d3CircleFirst = d3.select(parentGroup.select("[id='" + segmentFirst.attr("point-b") + "']")[0][0]);
                        var d3CircleSecond = d3.select((parentGroup.select("[id='" + segmentSecond.attr("point-a") + "']"))[0][0]);

                        d3CircleFirst.attr("parent-id", segmentSecond.attr("point-a"));
                        //d3CircleSecond.attr("parent-id", segmentSecond.attr("point-b"));

                        CreatePolyLine(parentGroup,
                            segmentSecond.attr("point-a"),
                            segmentFirst.attr("point-b"),
                            d3CircleSecond.attr("cx") + "," + d3CircleSecond.attr("cy"),
                            d3CircleFirst.attr("cx") + "," + d3CircleFirst.attr("cy"));

                        UpdatePolygonByNode(parentGroup);
                    }

                    function DragPolygon() {
                        var dragPoly = d3.select(this);
                        var points = dragPoly.attr("points");
                        var parentGroup = d3.select('#' + dragPoly.attr("g-id"));

                        if (parentGroup.attr("master-group") != null) {

                            var masterGroup = parentGroup.attr('master-group');
                            var mainPoly = d3.select('#' + masterGroup).selectAll('polygon');

                            DragGroup(masterGroup);

                            //check if mainPoly is deleted before dragging
                            if (mainPoly.length > 0) {
                                DragPolygonBody(mainPoly);
                            }


                        } else {
                            DragPolygonBody(dragPoly);
                            DragGroup(parentGroup.attr('id'));
                        }

                    }

                    function DragGroup(masterGroupId) {
                        var children = d3.select('svg').selectAll("g.is-main");

                        $.each(children[0], function (index, value) {
                            var child = d3.select(value);
                            if (value.hasAttribute('master-group') && child.attr('master-group') == masterGroupId) {
                                var poly = child.select('polygon');
                                DragPolygonBody(poly);
                            }
                        });

                    }

                    function DragPolygonBody(dragPoly) {

                        var points = dragPoly.attr("points");
                        var parentGroup = d3.select('#' + dragPoly.attr("g-id"));

                        var arrayCoordinate = points.split(',').map(function (item) {
                            return parseFloat(item);
                        });

                        for (var i = 0; i < arrayCoordinate.length; i++) {
                            if (i % 2 == 0) {

                                arrayCoordinate[i] += d3.event.dx;
                            }
                            else {
                                arrayCoordinate[i] += d3.event.dy;
                            }
                        }

                        dragPoly.attr("points", arrayCoordinate);

                        DragPoyline(dragPoly);
                        DragCircle(dragPoly);



                    }

                    function DragCircle(dragPoly) {
                        var groupId = dragPoly.attr("g-id");
                        var circles = d3.select('#' + groupId).selectAll("circle");

                        var cx, cy;

                        for (var i = 0; i < circles[0].length; i++) {
                            var circle = d3.select(circles[0][i]);

                            //get x and y value and convert to float
                            cx = parseFloat(circle.attr("cx"));
                            cy = parseFloat(circle.attr("cy"));

                            // add every drag value of mouse event
                            cx += d3.event.dx;
                            cy += d3.event.dy;

                            // assign new coordinate 
                            circle.attr("cx", cx).attr("cy", cy);
                        }

                    }

                    function DragPoyline(dragPoly) {
                        var groupId = dragPoly.attr("g-id");
                        var polylines = d3.select('#' + groupId).selectAll("polyline");

                        for (var i = 0; i < polylines[0].length; i++) {
                            var segment = d3.select(polylines[0][i]);
                            var points = segment.attr("points");
                            var arrayCoordinate = points.split(',').map(function (item) {
                                return parseFloat(item);
                            });

                            for (var r = 0; r < arrayCoordinate.length; r++) {
                                if (r % 2 == 0) {

                                    arrayCoordinate[r] += d3.event.dx;
                                }
                                else {
                                    arrayCoordinate[r] += d3.event.dy;
                                }
                            }

                            segment.attr("points", arrayCoordinate);
                        }
                    }

                    function DragNode() {
                        var circle = d3.select(this);
                        var cx, cy;

                        cx = parseFloat(circle.attr("cx"));
                        cy = parseFloat(circle.attr("cy"));

                        cx += d3.event.dx;
                        cy += d3.event.dy;

                        circle.attr("cx", cx);
                        circle.attr("cy", cy);
                        var parentGroup = d3.select(this.parentNode);
                        UpdatePolygonByNode(parentGroup);
                        UpdatePolylineByNode(this);
                    }

                    //Method need to be refactor
                    function DrawCopiedRectangle(rectangle, side) {
                        var endId, currentId;
                        var points_x = [];
                        var points_y = [];

                        currentId = "id-" + side;
                        endId = "id-" + (side == 3 ? 0 : side + 1);

                        GetMaxRectangleId();
                        HideAllCircles();

                        var parentGroup = d3.select(rectangle.parentNode);

                        var endLoop = false;
                        var controlReverse;
                        var down = false;

                        ////when copy down
                        // TO DO: Refactor this
                        if (side == 1 && rectangle.parentNode.hasAttribute("parent-g-id")) {

                            down = side == 1;

                            var parentId = parentGroup.attr("parent-g-id");
                            var fromGroup = d3.select("g#" + parentId);

                            var oppositeGroup;
                            var endLoop = false;

                            if (fromGroup.attr("cd") != "false" && parentGroup.attr("cr") == fromGroup.attr("id")) {

                                oppositeGroup = d3.select("g#" + fromGroup.attr("cd"));

                                side = side - 1;
                                currentId = "id-" + side;
                                endId = "id-" + (side == 3 ? 0 : side + 1);

                                while (!endLoop) {

                                    endLoop = currentId == endId;
                                    var result = GetPointsForCopiedSide(oppositeGroup, currentId);

                                    currentId = result.id;
                                    points_x.push(result.point_x);
                                    points_y.push(result.point_y);
                                };


                                controlReverse = GetControlReverseValue(side, oppositeGroup);
                            } else if (fromGroup.attr("cd") != "false" && parentGroup.attr("cl") == fromGroup.attr("id")) {
                               
                                oppositeGroup = d3.select("g#" + fromGroup.attr("cd"));

                                side = side + 1;
                                currentId = "id-" + side;
                                endId = "id-" + (side == 3 ? 0 : side + 1);

                                while (!endLoop) {

                                    endLoop = currentId == endId;
                                    var result = GetPointsForCopiedSide(oppositeGroup, currentId);

                                    currentId = result.id;
                                    points_x.push(result.point_x);
                                    points_y.push(result.point_y);
                                };


                                controlReverse = GetControlReverseValue(side, oppositeGroup);
                            } else {

                                down = side == 1;

                                var points = GetNormalPointsValue(parentGroup, side);
                                controlReverse = GetControlReverseValue(side, parentGroup);

                                points_x = points.x;
                                points_y = points.y;
                            }
                        } else {

                            down = side == 1;

                            var points = GetNormalPointsValue(parentGroup, side);
                            controlReverse = GetControlReverseValue(side, parentGroup);

                            points_x = points.x;
                            points_y = points.y;

                        }

                        //Drawing
                        rectangleId++;

                        //for grouping
                        var masterGroup = "";

                        if (rectangle.parentNode.hasAttribute('master-group')) {
                            masterGroup = parentGroup.attr('master-group')
                        } else {
                            masterGroup = parentGroup.attr('id');
                        }

                        var crtRect = d3.select('svg').append('g')
                            .attr("class", "is-main")
                            .attr("id", "rect-" + rectangleId)
                            .attr("cl", controlReverse[0])
                            .attr("cd", controlReverse[1])
                            .attr("cr", controlReverse[2])
                            .attr("cu", controlReverse[3])
                            .attr("parent-g-id", down ? null : parentGroup.attr("id"))
                            .attr("master-group", masterGroup);
                       
                        if (down == true && rectangle.parentNode.hasAttribute("parent-g-id") && (side + 1) == 1) {
                       
                            SetControlValue(oppositeGroup, side, crtRect, "copied"); 
                            SetControlValue(parentGroup, side, crtRect, "parent"); 
                            SetControlValue(crtRect, side, parentGroup, "new");               
                          
                        } else if (down == true && rectangle.parentNode.hasAttribute("parent-g-id") && side == 2) {
                     
                            SetControlValue(oppositeGroup, side, crtRect, "copied"); 
                            SetControlValue(parentGroup, side, crtRect, "parent"); 
                          
                            SetControlValue(crtRect, side, parentGroup, "new");
                        } else {

                            SetControlValue(parentGroup, side, crtRect, "copied");

                        }

                        //default height and width is base on the (master) copied polygon
                        var groupBox = rectangle.parentNode.getBBox();
                        vm.DefaultHeight = groupBox.height;
                        vm.DefaultWidth = groupBox.width;

                        var nodes = SetNodesForCopiedRectangle(side, points_x, points_y);

                        CreatePolygon(crtRect, nodes);
                        CreateLineBySegment(crtRect, nodes);
                        CreateCircles(crtRect, nodes);

                        createRect = false;
                        //points = [];
                    }

                    function GetFromReversePoinsValue(parentGroup, side) {
                        var endId, currentId;
                        var points_x = [];
                        var points_y = [];

                        var parentId = parentGroup.attr("parent-g-id");
                        var fromGroup = d3.select("g#" + parentId);

                        var oppositeGroup;


                        side = side - 1;
                        var endLoop = false;

                        if (fromGroup.attr("cd") != "false") {
                            oppositeGroup = d3.select("g#" + fromGroup.attr("cd"));

                            currentId = "id-" + side;
                            endId = "id-" + (side == 3 ? 0 : side + 1);

                        }
                        //check parent if it has child in bottom and set as currentId                                                   
                        //get current id of child set endId                           

                        while (!endLoop) {

                            endLoop = currentId == endId;
                            var result = GetPointsForCopiedSide(oppositeGroup, currentId);

                            currentId = result.id;
                            points_x.push(result.point_x);
                            points_y.push(result.point_y);
                        };

                        var value = {
                            "x": points_x,
                            "y": points_y,
                            "group": oppositeGroup,
                        };

                        return value;
                    }

                    function GetNormalPointsValue(parentGroup, side) {

                        var endId, currentId;
                        var points_x = [];
                        var points_y = [];

                        currentId = "id-" + side;
                        endId = "id-" + (side == 3 ? 0 : side + 1);

                        var endLoop = false;

                        while (!endLoop) {
                            endLoop = currentId == endId;
                            var result = GetPointsForCopiedSide(parentGroup, currentId);

                            currentId = result.id;
                            points_x.push(result.point_x);
                            points_y.push(result.point_y);
                        };

                        var value = {
                            "x": points_x,
                            "y": points_y
                        };

                        return value;
                    }

                    function GetControlReverseValue(side, parentGroup) {

                        var value = [];

                        switch (side) {
                            case 0:
                                value = [false, false, parentGroup.attr("id"), false];
                                break;
                            case 1:
                                value = [false, false, false, parentGroup.attr("id")];
                                break;
                            case 2:
                                value = [parentGroup.attr("id"), false, false, false];
                                break;

                            case 3:
                                value = [false, parentGroup.attr("id"), false, false];
                                break;
                        }

                        return value;
                    }

                    function GetControls(parentGroup) {
                        var value = {
                            "left": parentGroup.attr("cl"),
                            "down": parentGroup.attr("cd"),
                            "right": parentGroup.attr("cr"),
                            "up": parentGroup.attr("cu")
                        };

                        return value;
                    }

                    function UnsetControls(controls)
                    {
                        if (controls.left != "false") {
                            var child = d3.select("g#" + controls.left);
                            child.attr("cr", "false");
                        }

                        if (controls.right != "false") {
                            var child = d3.select("g#" + controls.right);
                            child.attr("cl", "false");
                        }

                        if (controls.up != "false") {
                            var child = d3.select("g#" + controls.up);
                            child.attr("cd", "false");
                        }

                        if (controls.down != "false") {
                            var child = d3.select("g#" + controls.down);
                            child.attr("cu", "false");
                        }
                    }

                    
                    function SetControlValue(parentGroup, side, crtRect, type) {
                  
                        if (side == 0) {
                            if (type == "parent") {
                                side = side + 1;
                            }
                            if (type == "new") {
                                side = side + 3;
                            }
                        }
                        else {
                            if (type == "parent") {
                                side = side - 1 ;
                            }
                            if (type == "new") {
                                side = side + 1;
                            }
                        }

                        switch (side) {
                            case 0:
                                parentGroup.attr("cl", crtRect.attr("id"));
                                break;
                            case 1:
                                parentGroup.attr("cd", crtRect.attr("id"));
                                break;
                            case 2:
                                parentGroup.attr("cr", crtRect.attr("id"));
                                break;
                            case 3:
                                parentGroup.attr("cu", crtRect.attr("id"));
                                break;
                        }

                    }

                    function SetNodesForCopiedRectangle(side, points_x, points_y) {
                        var returnList = [];
                        //InsertChildParam
                        //0:3, 1:3, 2:0, 3:1
                        switch (side) {
                            case 0:
                                //left
                                returnList.push(CreateNode(0, true, (parseInt(points_x[points_x.length - 1]) - vm.DefaultWidth) + ", " + parseInt(points_y[0])));
                                returnList.push(CreateNode(1, true, (parseInt(points_x[points_x.length - 1]) - vm.DefaultWidth) + "," + (parseInt(points_y[0]) + vm.DefaultHeight)));
                                returnList.push(CreateNode(2, true, (points_x[points_x.length - 1] + "," + points_y[points_y.length - 1])));
                                returnList = InsertChildNodes(3, returnList, points_x, points_y);
                                returnList.push(CreateNode(3, true, (points_x[0] + "," + points_y[0])));
                                break;
                            case 1:
                                //down
                                returnList.push(CreateNode(0, true, (points_x[0] + ", " + parseInt(points_y[0]))));
                                returnList.push(CreateNode(1, true, (points_x[0] + "," + (parseInt(points_y[0]) + vm.DefaultHeight))));
                                returnList.push(CreateNode(2, true, (parseInt(points_x[0]) + vm.DefaultWidth) + "," + (parseInt(points_y[0]) + vm.DefaultHeight)));
                                returnList.push(CreateNode(3, true, points_x[points_x.length - 1] + "," + points_y[points_y.length - 1]));
                                returnList = InsertChildNodes(0, returnList, points_x, points_y);
                                break;
                            case 2:
                                //right
                                returnList.push(CreateNode(0, true, points_x[points_x.length - 1] + ", " + parseInt(points_y[points_y.length - 1])));
                                returnList = InsertChildNodes(1, returnList, points_x, points_y);
                                returnList.push(CreateNode(1, true, points_x[0] + "," + points_y[0]));
                                returnList.push(CreateNode(2, true, (parseInt(points_x[points_x.length - 1]) + vm.DefaultWidth) + "," + (parseInt(points_y[points_y.length - 1]) + vm.DefaultHeight)));
                                returnList.push(CreateNode(3, true, (parseInt(points_x[points_x.length - 1]) + vm.DefaultWidth) + "," + points_y[points_y.length - 1]));
                                break;
                            case 3:
                                //up
                                returnList.push(CreateNode(0, true, (parseInt(points_x[0]) - vm.DefaultWidth) + "," + (parseInt(points_y[points_y.length - 1]) - vm.DefaultHeight)));
                                returnList.push(CreateNode(1, true, points_x[points_x.length - 1] + "," + points_y[points_y.length - 1]));
                                returnList = InsertChildNodes(2, returnList, points_x, points_y);
                                returnList.push(CreateNode(2, true, points_x[0] + "," + points_y[0]));
                                returnList.push(CreateNode(3, true, (parseInt(points_x[0])) + ", " + (parseInt(points_y[points_y.length - 1]) - vm.DefaultHeight)));
                                break;
                        }

                        return returnList;
                    }

                    function CreateNode(id, isMain, points) {
                        return {
                            id: "id-" + id,
                            isMain: isMain,
                            points: points
                        };
                    }

                    function InsertChildNodes(indexToInsert, mainList, param_x, param_y) {
                        var returnList = [];

                        for (var j = param_x.length - 2; j >= 1 ; j--) {
                            returnList.push(CreateNode(globalCircleId, false, param_x[j] + "," + param_y[j]));
                            globalCircleId++;
                        }

                        mainList.splice.apply(mainList, [indexToInsert, 0].concat(returnList));
                        return mainList;
                    }

                    function CreateCircles(rectangle, nodes) {
                        for (var i = 0; i < nodes.length; i++) {
                            var point = nodes[i].points.split(',');

                            var parentId = "";
                            switch (i) {
                                case 0:
                                    parentId = nodes[nodes.length - 1].id;
                                    break;
                                case nodes.length:
                                    parentId = nodes[0].id;
                                    break;
                                default:
                                    parentId = nodes[i - 1].id;
                                    break;
                            }

                            rectangle.append("circle")
                                .attr("id", nodes[i].id)
                                .attr("parent-id", parentId)
                                .attr("cx", point[0])
                                .attr("cy", point[1])
                                .attr("r", 5)
                                .attr("is-handle", true)
                                .attr("fill", nodes[i].isMain ? "red" : "white")
                                .attr("is-main", true)
                                .call(dragNode)
                                .on('contextmenu', d3.contextMenu(nodeMenu));
                        }
                    }

                    function GetPointsForCopiedSide(parentGroup, circleId) {
                        var circle = parentGroup.select("[id='" + circleId + "']");
                        var d3Circle = d3.select(circle[0][0]);
                        var nextCircle = parentGroup.select("[parent-id='" + circleId + "']");

                        return {
                            id: d3.select(nextCircle[0][0]).attr("id"),
                            point_x: d3Circle.attr("cx"),
                            point_y: d3Circle.attr("cy")
                        };
                    }

                    function DrawRectangle(points) {
                        rectangleId++;

                        var crtRect = d3.select('svg').append('g')
                            .attr("class", "is-main")
                            .attr("id", "rect-" + rectangleId)
                            .attr("cl", false)
                            .attr("cd", false)
                            .attr("cr", false)
                            .attr("cu", false);

                        var nodes = [];

                        nodes.push(CreateNode(0, true, points[0] + "," + points[1]));
                        nodes.push(CreateNode(1, true, points[0] + "," + (points[1] + vm.DefaultHeight)));
                        nodes.push(CreateNode(2, true, (points[0] + vm.DefaultWidth) + "," + (points[1] + vm.DefaultHeight)));
                        nodes.push(CreateNode(3, true, (points[0] + vm.DefaultWidth) + "," + points[1]));

                        CreatePolygon(crtRect, nodes);
                        CreateLineBySegment(crtRect, nodes);
                        CreateCircles(crtRect, nodes);

                        createRect = false;
                        points = [];

                    }

                    function CreatePolygon(crtRect, points) {
                        var pointsAttr = "";

                        for (var i = 0; i < points.length; i++) {
                            if (pointsAttr.length == 0) {
                                pointsAttr = points[i].points;
                            }
                            else {
                                pointsAttr += "," + points[i].points;
                            }
                        }

                        crtRect.insert("polygon", "text").attr("points", pointsAttr)
                            .attr("is-line", true)
                            .attr("class", "drag")
                            .attr("class", "poly")
                            .attr("g-id", "rect-" + rectangleId)
                            .call(dragger)
                            .on('contextmenu', d3.contextMenu(menu));
                    }

                    function CreateLineBySegment(crtRect, nodes) {

                        var point_a, point_b;

                        for (var i = 0; i < nodes.length; i++) {
                            if (i == nodes.length - 1) {
                                point_a = nodes[nodes.length - 1];
                                point_b = nodes[0];
                            }
                            else {
                                point_a = nodes[i];
                                point_b = nodes[i + 1];
                            }

                            crtRect.append("polyline")
                                .attr("points", point_a.points + "," + point_b.points)
                                .attr("point-a", point_a.id)
                                .attr("point-b", point_b.id)
                                .attr("class", "stroke")
                                .attr("is-line", true)
                                .attr("segment", true)
                                .attr("g-id", "rect-" + rectangleId);
                        }

                    }

                    function CreateChildNode(target, points) {
                        var polyLine = d3.select(d3.event.target);
                        var parentGroup = d3.select(d3.event.target.parentNode);
                        var circleFirst = parentGroup.select("[id='" + polyLine.attr("point-a") + "']");
                        var circleSecond = parentGroup.select("[id='" + polyLine.attr("point-b") + "']");

                        var d3CircleFirst = d3.select(circleFirst[0][0]);
                        var d3CircleSecond = d3.select(circleSecond[0][0]);
                        var pointsFirst = d3CircleFirst.attr("cx") + "," + d3CircleFirst.attr("cy");
                        var pointsSecond = d3CircleSecond.attr("cx") + "," + d3CircleSecond.attr("cy");
                        var mousePoints = points[0] + "," + points[1];

                        var newCircleId = "id-" + globalCircleId;

                        CreatePolyLine(parentGroup, polyLine.attr("point-a"), newCircleId, pointsFirst, mousePoints);
                        CreatePolyLine(parentGroup, newCircleId, polyLine.attr("point-b"), mousePoints, pointsSecond);

                        PlotNewPoint(parentGroup, d3CircleFirst.attr("id"), points);
                        circleSecond.attr("parent-id", newCircleId);

                        polyLine.remove();
                    }

                    function CreatePolyLine(parentGroup, point_a, point_b, pointsFirst, pointsSecond) {
                        parentGroup.insert("polyline", "circle").attr("points", pointsFirst + "," + pointsSecond)
                            .attr("class", "stroke-new")
                            .attr("is-line", true)
                            .attr("segment", true)
                            .attr("point-a", point_a)
                            .attr("point-b", point_b)
                            .attr("g-id", parentGroup.attr("id"));
                    }

                    function PlotNewPoint(parentGroup, parentId, points) {
                        parentGroup.append("circle")
                           .attr("cx", points[0])
                           .attr("cy", points[1])
                           .attr("r", 4)
                           .attr("is-handle", true)
                           .attr("fill", "#FFFFFF")
                           .attr("id", "id-" + globalCircleId)
                           .attr("parent-id", parentId)
                           .call(dragNode)
                           .on('contextmenu', d3.contextMenu(nodeMenu));

                        globalCircleId++;
                    }

                    function UpdatePolygonByNode(parentGroup) {

                        var newPoints = [];
                        var polygon = parentGroup.select("polygon");
                        var mainCircle = parentGroup.select("[id='id-0']");
                        var d3Circle = d3.select(mainCircle[0][0]);
                        var skipFirst = true;

                        while (d3Circle.attr("id") != "id-0" || skipFirst) {
                            skipFirst = false;
                            newPoints.push(d3Circle.attr("cx"));
                            newPoints.push(d3Circle.attr("cy"));

                            mainCircle = parentGroup.select("[id='" + d3Circle.attr("parent-id") + "']");
                            d3Circle = d3.select(mainCircle[0][0]);

                        }

                        polygon.attr("points", newPoints);

                    }

                    function UpdatePolylineByNode(target) {

                        var draggedCircle = d3.select(target);
                        var parentGroup = d3.select(target.parentNode);
                        //get poly-1 point a = target.id
                        //get poly-2 point b = target.id
                        var lineFirst = parentGroup.select("[point-a='" + draggedCircle.attr("id") + "']");
                        var lineSecond = parentGroup.select("[point-b='" + draggedCircle.attr("id") + "']");

                        var d3LineFirst = d3.select(lineFirst[0][0]);
                        var d3LineSecond = d3.select(lineSecond[0][0]);

                        var pointsLineFirst = d3LineFirst.attr("points").split(',');
                        var pointsLineSecond = d3LineSecond.attr("points").split(',');

                        d3LineFirst.attr("points", draggedCircle.attr("cx") + "," + draggedCircle.attr("cy") + "," + pointsLineFirst[2] + "," + pointsLineFirst[3]);
                        d3LineSecond.attr("points", pointsLineSecond[0] + "," + pointsLineSecond[1] + "," + draggedCircle.attr("cx") + "," + draggedCircle.attr("cy"));
                    }

                    function Unassign(parentGroup) {

                        var appartmentId = parseInt(parentGroup.attr("apt-id"));
                        var appartmentName = GetAssignedNameValue(appartmentId);
                        var polygon = parentGroup.select("polygon");

                        if (parentGroup.attr("apt-id") != null) {

                            vm.Apartments.push({ name: appartmentName, id: appartmentId });

                            vm.AssignedApartments = $.grep(vm.AssignedApartments, function (e) {
                                return e.id != appartmentId;
                            });

                            $scope.$apply();

                            parentGroup.attr("apt-id", null);
                            polygon.attr("class", "poly");

                        }

                    }

                    function AssignApartment(polygon, select) {
                        var parentGroup = d3.select(polygon.parentNode);
                        var d3Polygon = d3.select(polygon);
                        var selected = d3.select(select);
                        var selectedIndex = selected.property('selectedIndex');

                        var option = selected.selectAll("option")[0][selectedIndex];
                        var textValue = d3.select(option).html()
                        var value = d3.select(option).attr("value");

                        if (parentGroup.attr("apt-id")) {
                            Unassign(parentGroup);
                        }

                        d3Polygon.attr('class', 'poly-selected');

                        parentGroup.attr("apt-id", value);

                        vm.AssignedApartments.push(vm.Apartments[selectedIndex]);
                        vm.Apartments.splice(selectedIndex, 1);
                        $scope.$apply();

                        d3.select('.d3-context-menu').style('display', 'none');
                    }

                    function GetMaxRectangleId() {
                        var parentGroup = d3.selectAll("g.is-main");
                        var rectIds = [];

                        $.each(parentGroup[0], function (index, value) {
                            var group = d3.select(value);

                            var count = group.attr("id").split('-')[1];

                            rectIds.push(parseInt(count));
                        });

                        // rectIds.sort();
                        //return rectIds[rectIds.length - 1];
                        rectangleId = rectIds[rectIds.length - 1];

                    }

                    function SetMaxCircleId(circleIds) {
                        if (circleIds.length > 0) {
                            circleIds.sort();
                            globalCircleId = (circleIds[circleIds.length - 1]) + 1;
                        }
                    }

                    function GetAssignedNameValue(id) {
                        var name = "";

                        $.each(vm.AssignedApartments, function (index, value) {
                            if (value.id == id) {
                                name = value.name;
                                false;
                            }
                        });

                        return name;
                    }

                    d3.contextMenu = function (menu, openCallback) {

                        // create the div element that will hold the context menu
                        d3.selectAll('.d3-context-menu').data([1])
                            .enter()
                            .append('div')
                            .attr('class', 'd3-context-menu');

                        // close menu
                        d3.select('body').on('click.d3-context-menu', function () {
                            d3.select('.d3-context-menu').style('display', 'none');
                        });

                        // this gets executed when a contextmenu event occurs
                        return function (data, index) {
                            var elm = this;
                            var nodeName = d3.select(elm)[0][0].nodeName;
                            var parentGroup = d3.select(elm.parentNode);

                            d3.selectAll('.d3-context-menu').html('');
                            var list = d3.selectAll('.d3-context-menu').append('ul');
                            var container = d3.selectAll('.d3-context-menu').append('div').style("display", "none").attr("class", "apartments");

                            var label = container.append("label").attr('class', 'apt-label');
                            var apartmentLabel = container.append("labal").attr('class', 'apt-label');
                            var option = container.append("select").attr("value", "");

                            list.selectAll('li').data(menu).enter()
                                .append('li')
                                .html(function (d) {
                                    if (nodeName == "circle")
                                        return d.title;
                                    else {
                                        if (d.type == 'simple') {

                                            if (parentGroup.attr("apt-id") != null) {
                                                return d.title;
                                            } else {
                                                if (d.title != "Remove Assign") {

                                                    var cu = parentGroup.attr("cu");
                                                    var cd = parentGroup.attr("cd");
                                                    var cr = parentGroup.attr("cr");
                                                    var cl = parentGroup.attr("cl");

                                                    if (cu == "false" && d.title == "Copy Up") {
                                                        return d.title;
                                                    } else if (cd == "false" && d.title == "Copy Down") {
                                                        return d.title;
                                                    } else if (cr == "false" && d.title == "Copy Right") {
                                                        return d.title;
                                                    } else if (cl == "false" && d.title == "Copy Left") {
                                                        return d.title;
                                                    } else if (d.title == "Copy" || d.title == "Delete") {
                                                        return d.title
                                                    }
                                                }
                                            }


                                        }
                                        else {
                                            container.style("display", "block");
                                            label.html(d.title);
                                            if (parentGroup.attr("apt-id") != null) {
                                                apartmentLabel.html("Room: " + GetAssignedNameValue(parseInt(parentGroup.attr("apt-id"))));
                                            } else { apartmentLabel.html("No Apartment Assigned"); }
                                            option.selectAll("option").style("display", "block").data(vm.Apartments).enter().append("option")
                                            .attr("value", function (c) { return c.id; }).html(function (c) {
                                                return c.name;
                                            });
                                        }
                                    }
                                })
                                .on('click', function (d, i) {
                                    d.action(elm, data, index);

                                    if (d.title != "Remove Assign") {
                                        d3.select('.d3-context-menu').style('display', 'none');
                                    }

                                    apartmentLabel.html("No Apartment Assigned");
                                    d3.select('.d3-context-menu ul li:last-child').remove();

                                    option.selectAll("option").style("display", "block").data(vm.Apartments).enter().append("option")
                                          .attr("value", function (c) { return c.id; }).html(function (c) {
                                              return c.name;
                                          });

                                });

                            //remove empty li
                            list.selectAll("li").each(function () {

                                var value = $(this).text();

                                if (value.length == 0)
                                    $(this).remove();

                            });

                            option.on("change", function () {
                                //elm is equal to polygon that has on rightclick function
                                //this is equal to select element
                                AssignApartment(elm, this);
                            });

                            // the openCallback allows an action to fire before the menu is displayed
                            // an example usage would be closing a tooltip
                            if (openCallback) openCallback(data, index);

                            // display context menu
                            d3.select('.d3-context-menu')
                                .style('left', (d3.event.pageX - 2) + 'px')
                                .style('top', (d3.event.pageY - 2) + 'px')
                                .style('display', 'block');

                            d3.event.preventDefault();
                        };
                    };
                }
            });
        }

        Initialize();
    };

    EditorConfigureController.$injectParams = injectParams;
    angular.module("buildingEditorApp").controller("EditorConfigureController", EditorConfigureController);
})();
