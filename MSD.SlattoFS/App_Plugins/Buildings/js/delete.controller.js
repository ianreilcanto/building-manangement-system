angular.module("umbraco").controller("BuildingsTree.DeleteController",
	function ($scope, buildingResource, navigationService, treeService, $location) {
	    $scope.delete = function (id) {
	        buildingResource.deleteById(id).then(function () {
	            navigationService.hideNavigation();
	            $location.path('/buildings');
	        });
	    };
	    $scope.cancelDelete = function () {
	        navigationService.hideNavigation();
	    };
	});