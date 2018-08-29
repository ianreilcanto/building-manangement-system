(function () {

    'use strict';
    var injectParams = ['appConfig', 'dataModel', '$scope', '$http', '$location', '$route'];

    var EditorInfoController = function (appConfig, dataModel, $scope, $http, $location, $route) {

        var vm = this;
        vm.Message = "Loaded Information Controller";
        vm.Files = [];
        vm.Apartments = [];
        
        vm.ApartmentStatusList = appConfig.APARTMENT_STATUSES;

        function GetApartmentStatusList()
        {
            $http({
                url: ("/BMBuilding/GetApartmentStatuses"),
                method: "POST",
                transformRequest: angular.identity,
            }).then(function successCallBack(response) {
                // appConfig.APARTMENT_STATUSES = response.data.apartmentStatuses;
                vm.ApartmentStatusList = response.data.apartmentStatuses;
            }, function errorCallBack(response) { });

        }


  


        $scope.GetFileDetails = function (e) {

            if (typeof e === 'undefined') return;
            for (var i = 0; i < e.files.length; i++) {
                vm.Files.push(e.files[i]);
            }
        };
        
        vm.UploadApartmentFiles = function (apartment) {

            var fd = new FormData();
            fd.append("accountId", appConfig.ACCOUNT_ID);
            fd.append("buildingId", appConfig.BUILDING_ID);
            fd.append("apartmentId", apartment.Id);

            for (var i = 0; i < vm.Files.length; i++) {
                fd.append('file' + i, vm.Files[i]);
            }

            $http({
                url: appConfig.APARTMENT_INFOUPLOAD_URL,
                method: "POST",
                data: fd,
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            })
            .then(function successCallBack(response) {
                if (typeof response !== 'undefined' && response.data.list !== 'undefined') {
                    angular.forEach(vm.Apartments, function (value, index) {
                        $route.reload();
                        if (value.Id == apartment.Id) {

                            var apartment = vm.Apartments[index];
                            apartment.FileAssets = [];

                            //update the file assets of apartment
                            //TODO: check why this part is not showing/rendering on the view
                            value.FileAssets = [];
                            angular.forEach(response.data.list, function (value, key) {
                                apartment.FileAssets.push(new dataModel.FileAsset(value));
                            });
                        }
                    });
                }

            }, function errorCallBack(response) {

            });

        };

        vm.UploadFiles = function () {

            var fd = new FormData();
            fd.append("accountId", appConfig.ACCOUNT_ID);
            fd.append("buildingId", appConfig.BUILDING_ID);
            //fd.append("type", 'building'); TODO: may not be needed now, still to be implemented

            for (var i = 0; i < vm.Files.length; i++) {
                fd.append('file' + i, vm.Files[i]);
            }

            $http({
                url: appConfig.INFOUPLOAD_URL,
                method: "POST",
                data: fd,
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            })
            .then(function successCallBack(response) {

                if (typeof response !== 'undefined' && response.data.list !== 'undefined') {
                    angular.forEach(response.data.list, function (value, index) {
                        var apartment = new dataModel.Apartment(value);
                        vm.Apartments.push(apartment);
                        $route.reload();
                    });
                }

            }, function errorCallBack(response) {

            });
        };

        function GetBuildingInfo() {

            var fd = new FormData();
            fd.append("accountId", appConfig.ACCOUNT_ID);
            fd.append("buildingId", appConfig.BUILDING_ID);

            $http({
                url: appConfig.BUILDING_INFO_URL,
                method: "POST",
                data: fd,
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            })
            .then(function successCallBack(response) {
                if (typeof response !== 'undefined' && response.data.list !== 'undefined') {
                    $scope.apartmentsWithFiles = [];
                    angular.forEach(response.data.list, function (value, index) {
                        var apartment = new dataModel.Apartment(value);
                        vm.Apartments.push(apartment);

                        if (apartment.FileAssets.length > 0) {
                            $scope.apartmentsWithFiles.push(apartment);
                        }
                    });
                }
            }, function errorCallBack(response) {

            });
        }

        function GetApartmentAssignmentDetails() {
            $http({
                url: (appConfig.APARTMENT_ASSIGNMENTDETAILS_URL),
                method: "POST",
                data: JSON.stringify({'buildingId': appConfig.BUILDING_ID}),
                transformRequest: angular.identity,
                headers: { 'Content-Type': 'application/json' }
            }).then(function successCallBack(response) {
                    $scope.apartmentAssignment = response.data.apartmentAssignmentDetails;
            }, function errorCallBack(response) { });
        }

        function Initialize() {
            GetBuildingInfo();
            GetApartmentAssignmentDetails();
            GetApartmentStatusList();
        }

        Initialize();
    };

    EditorInfoController.$injectParams = injectParams;
    angular.module("buildingEditorApp").controller("EditorInfoController", EditorInfoController);

})();