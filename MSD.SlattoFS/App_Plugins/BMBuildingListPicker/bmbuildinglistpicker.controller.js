(function () {
    'use strict';

    var injectParams = ["$scope", "$http", "dialogService"];
    var BMBuildingPickerController = function ($scope, $http, dialogService) {

        $scope.loading = true;
        $http({ method: 'GET', url: 'backoffice/Buildings/BuildingApi/GetAll' })
          .success(function (data) {
              $scope.lists = data;
              $scope.loading = false;
          })
          .error(function () {
              $scope.error = "An Error has occured while loading!";
              $scope.loading = false;
          });

        // $scope.model.value = -1;

    };

    BMBuildingPickerController.$injectParams = injectParams;
    angular.module("umbraco").controller("BMBuildingListPickerController", BMBuildingPickerController);

})();