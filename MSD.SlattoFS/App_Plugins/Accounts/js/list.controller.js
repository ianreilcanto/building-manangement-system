angular.module("umbraco").controller("Accounts.AccountsListController",
	function ($http, $scope, accountResource, notificationsService) {

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
	        accountResource.getPaged($scope.itemsPerPage, $scope.currentPage, $scope.predicate, $scope.reverse ? "desc" : "asc", $scope.searchTerm).then(function (response) {
	            $scope.accounts = response.data.accounts;
	            $scope.totalPages = response.data.totalPages;
	        }, function (response) {
	            notificationsService.error("Error", "Could not load accounts");
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
	        if (confirm("Are you sure you want to delete " + $scope.selectedIds.length + " account(s)?")) {
	            $scope.actionInProgress = true;

	            accountResource.deleteByBatch($scope.selectedIds).
                    then(function (response) {
                        $scope.accounts = _.reject($scope.accounts, function (el) { return $scope.selectedIds.indexOf(el.id) > -1; });
                        $scope.selectedIds = [];

                    }, function (response) {
                        notificationsService.error("Error", "Could not load accounts");
                    });

	            $scope.accounts = _.reject($scope.accounts, function (el) { return $scope.selectedIds.indexOf(el.id) > -1; });
	            $scope.selectedIds = [];
	            $scope.actionInProgress = false;
	        }
	    };

	    fetchData();
	});