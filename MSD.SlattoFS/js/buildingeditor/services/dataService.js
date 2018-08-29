(function () {
    'use strict';
    var injectParams = ['appSettings', '$http'];
    var dataService = function (appSettings, $http) {
        var factory = {};

        return factory;
    };

    dataService.$injectParams = injectParams;
    angular.module("buildingEditorApp").factory("dataService", dataService);

})();