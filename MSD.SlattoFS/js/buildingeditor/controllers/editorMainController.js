(function () {

    'use strict';
    var injectParams = ['$scope', 'appConfig'];

    //NOITE: to use the d3Service
    var EditorMainController = function ($scope, appConfig, $http) {

        var vm = this;       

        $scope.InitializeAccount = function (accountId) {
            appConfig.ACCOUNT_ID = accountId;
        };

        $scope.InitializeBuilding = function (buildingId) {
            appConfig.BUILDING_ID = buildingId;
        };

        $scope.InitializeApartmentStatuses = function () {
            $http({
                url: ("/BMBuilding/GetApartmentStatuses"),
                method: "POST",
                transformRequest: angular.identity,
            }).then(function successCallBack(response) {
                appConfig.APARTMENT_STATUSES = response.data.apartmentStatuses;
            }, function errorCallBack(response) { });
        };
    };

    EditorMainController.$injectParams = injectParams;
    angular.module("buildingEditorApp").controller("EditorMainController", EditorMainController);

})();