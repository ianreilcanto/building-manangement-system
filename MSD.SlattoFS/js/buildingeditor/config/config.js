(function () {
    'use strict';
    var app = angular.module("buildingEditorApp");
    app
        .constant("appConfig", {
            BASEAPP_URL: "/js/buildingeditor",
            BASEAPP_VIEWSURL: "/js/buildingeditor/views",
            MEDIAUPLOAD_URL: "/bmbuilding/bmbuildinguploadassets",
            MEDIAASSETS_URL: "/bmbuilding/bmbuildingassets",
            MEDIAREMOVE_URL: "/bmbuilding/bmbuildingremoveasset",
            MEDIASORT_URL: "/bmbuilding/bmbuildingsortassets", 
            INFOUPLOAD_URL: "/bmbuilding/bmbuildinguploadinfo",
            APARTMENT_INFOUPLOAD_URL: "/bmbuilding/bmbuildingapartmentuploadinfo",
            APARTMENT_ASSIGNMENTDETAILS_URL: "/bmbuilding/bmbuildingapartmentassignmentdetails",
            BUILDING_INFO_URL: "/bmbuilding/bmbuildinginfo",
            BUILDING_APARTMENTS_SIMPLE_LIST: "/bmbuilding/getsimpleapartmentslist/",
            SAVESVG_URL: "/BMBuilding/SaveSvgData",
            GETSVG_URL: "/BMBuilding/GetSvgData",
            CURRENT_LOCATION: window.location,
            ACCOUNT_ID: -1,
            BUILDING_ID: -1,
            APARTMENT_STATUSES: []
        })
        .config(["templateProvider", "$routeProvider", function (templateProvider, $routeProvider) {

            $routeProvider
                .when("/info", {
                    controller: "EditorInfoController",
                    templateUrl: templateProvider.GetTemplate("editor.information"),
                    controllerAs: "vm"
                })
                .when("/media", {
                    controller: "EditorMediaController",
                    templateUrl: templateProvider.GetTemplate("editor.media"),
                    controllerAs: "vm"
                })
                .when("/configure", {
                    controller: "EditorConfigureController",
                    templateUrl: templateProvider.GetTemplate("editor.configuration"),
                    controllerAs: "vm"
                })
                .otherwise({
                    redirectTo: "/info"
                });
        }]);

})();