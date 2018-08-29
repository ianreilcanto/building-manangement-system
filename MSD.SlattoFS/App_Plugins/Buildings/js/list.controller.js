angular.module("umbraco").controller("Buildings.BuildingsListController",
	function ($http, $scope, buildingResource, notificationsService) {

	    $scope.selectedIds = [];
	    $scope.currentPage = 1;
	    $scope.itemsPerPage = 10;
	    $scope.totalPages = 1;

	    $scope.reverse = false;

	    $scope.searchTerm = "";
	    $scope.predicate = 'id';

	    //fetch the data from the API by using the resource  created previously by calling the 
	    //getPaged-method with some $scope variables which are holding information about the search term, 
        //itemsPerPage, current page , etc.
	    function fetchData() {
	        buildingResource.getPaged($scope.itemsPerPage, $scope.currentPage, $scope.predicate, $scope.reverse ? "desc" : "asc", $scope.searchTerm).then(function (response) {
	            $scope.buildings = response.data.buildings;
	            $scope.totalPages = response.data.totalPages;
	        }, function (response) {
	            notificationsService.error("Error", "Could not load buildings");
	        });
	    };

	    $scope.order = function (predicate) {
	        $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
	        $scope.predicate = predicate;
	        $scope.currentPage = 1;
	        fetchData();
	    };

	    //used to store the ids of the items we checked in the listview. This is needed for bulk deleting, updating or whatever else.
	    $scope.toggleSelection = function (val) {
	        var idx = $scope.selectedIds.indexOf(val);
	        if (idx > -1) {
	            $scope.selectedIds.splice(idx, 1);
	        } else {
	            $scope.selectedIds.push(val);
	        }
	    };

	    $scope.isRowSelected = function (id) {
	        return $scope.selectedIds.indexOf(id) > -1;
	    };

	    $scope.isAnythingSelected = function () {
	        return $scope.selectedIds.length > 0;
	    };

	    $scope.prevPage = function () {
	        if ($scope.currentPage > 1) {
	            $scope.currentPage--;
	            fetchData();
	        }
	    };

	    $scope.nextPage = function () {
	        if ($scope.currentPage < $scope.totalPages) {
	            $scope.currentPage++;
	            fetchData();
	        }
	    };

	    $scope.setPage = function (pageNumber) {
	        $scope.currentPage = pageNumber;
	        fetchData();
	    };

	    $scope.search = function (searchFilter) {
	        $scope.searchTerm = searchFilter;
	        $scope.currentPage = 1;
	        fetchData();
	    };

	    $scope.delete = function () {
	        if (confirm("Are you sure you want to delete " + $scope.selectedIds.length + " building(s)?")) {
	            $scope.actionInProgress = true;

	            //TODO: do the real deleting here
	            //This should be done by calling the api controller with the buildingResource using $scope.selectedIds
	            buildingResource.deleteByBatch($scope.selectedIds).
                    then(function (response) {
                        $scope.buildings = _.reject($scope.buildings, function (el) { return $scope.selectedIds.indexOf(el.id) > -1; });
                        $scope.selectedIds = [];

                    }, function (response) {
                        notificationsService.error("Error", "Could not load buildings");
                    });

	            $scope.buildings = _.reject($scope.buildings, function (el) { return $scope.selectedIds.indexOf(el.id) > -1; });
	            $scope.selectedIds = [];
	            $scope.actionInProgress = false;
	        }
	    };

	    fetchData();
	});