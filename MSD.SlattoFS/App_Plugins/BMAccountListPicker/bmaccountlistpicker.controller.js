(function () {
    'use strict';

    var injectParams = ["$scope", "$http", "dialogService"];
    var BMAccountPickerController = function ($scope, $http, dialogService) {

        $scope.loading = true;
        $http({ method: 'GET', url: 'backoffice/Accounts/AccountApi/GetAll' })
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

    BMAccountPickerController.$injectParams = injectParams;
    angular.module("umbraco").controller("BMAccountListPickerController", BMAccountPickerController);

})();