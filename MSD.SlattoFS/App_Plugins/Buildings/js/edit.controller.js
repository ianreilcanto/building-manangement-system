angular.module("umbraco").controller("BuildingsTree.EditController",
	function ($scope, $routeParams, buildingResource, addressResource, notificationsService, navigationService, $location) {

	    $scope.loaded = true;

	    if ($routeParams.id == -1) {
	        $scope.buildingModel = {};
	        $scope.loaded = true;
	    }
	    else {
	        buildingResource.getById($routeParams.id).then(function (response) {
	            $scope.buildingModel = response.data;
	            $scope.loaded = true;
	        });
	    }

	    $scope.save = function (buildingModel) {
	        if ($scope.buildingModel.addressList != null) {
	            buildingResource.save(buildingModel).then(function (response) {
	                var bldgId = $scope.buildingModel.building.id;
	                $scope.buildingModel = response.data;
	                $scope.buildingForm.$dirty = false;
	                navigationService.syncTree({ tree: 'BuildingsTree', path: [-1, -1], forceReload: true });
	                if (bldgId == 0) {
	                    $location.path('/buildings');
	                }
	                notificationsService.success("Success", $scope.buildingModel.building.name + " has been saved");
	            });
	        }
	        else {
	            notificationsService.warning("Address Required", "You have to add atleast one address to create a building.");
	        }

	    };

	    $scope.addAddress = function (buildingModel) {
	        addressResource.addBlankAddress(buildingModel).then(function (response) {
	            $scope.buildingModel = response.data;
	            navigationService.syncTree({ tree: 'BuildingsTree', path: [-1, -1], forceReload: true });
	        });
	    }

	    $scope.deleteAddress = function (id,addressesArray,index) {
	        if (id > 0) {
	            if (confirm("Are you sure you want to delete this address?")) {
	                addressResource.deleteById(id).then(function (response) {
	                    if (response.data != null && response.data > 0) {
	                        notificationsService.warning("Address Deleted", "You have successfully removed the building's address.");
	                        addressesArray.splice(index, 1);
	                    }
	                    else {
	                        notificationsService.error("Cannot Delete Address", "A building should have at least one address.");
	                    }
	                });
	            }
	        }
	        else {
	            addressesArray.splice(index, 1);
	        }
	    };
	});