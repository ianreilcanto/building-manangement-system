angular.module("umbraco.resources")
	.factory("buildingResource", function ($http) {
	    return {
	        getById: function (id) {
	            return $http.get("backoffice/Buildings/BuildingApi/GetById?id=" + id);
	        },
	        save: function (building) {
	            return $http.post("backoffice/Buildings/BuildingApi/PostSave", angular.toJson(building));
	        },
	        deleteById: function (id) {
	            return $http.delete("backoffice/Buildings/BuildingApi/DeleteById?id=" + id);
	        },
	        getPaged: function (itemsPerPage, pageNumber, sortColumn, sortOrder, searchTerm) {
	            if (sortColumn == undefined)
	                sortColumn = "";
	            if (sortOrder == undefined)
	                sortOrder = "";
	            return $http.get("backoffice/Buildings/BuildingApi/GetPaged?itemsPerPage=" + itemsPerPage + "&pageNumber=" + pageNumber + "&sortColumn=" + sortColumn + "&sortOrder=" + sortOrder + "&searchTerm=" + searchTerm);
	        },
	        deleteByBatch: function (ids) {
	            return $http.post("backoffice/Buildings/BuildingApi/PostDeleteByBatch", ids);
	        }
	    };
	});

angular.module("umbraco.resources")
	.factory("addressResource", function ($http) {
	    return {
	        getById: function (id) {
	            return $http.get("backoffice/Buildings/AddressApi/GetById?id=" + id);
	        },
	        save: function (address) {
	            return $http.post("backoffice/Buildings/AddressApi/PostSave", angular.toJson(address));
	        },
	        deleteById: function (id) {
	            return $http.delete("backoffice/Buildings/AddressApi/DeleteById?id=" + id);
	        },
	        addBlankAddress: function (building) {
	            return $http.post("backoffice/Buildings/AddressApi/AddBlankAddress", angular.toJson(building));
	        }	        
	    };
	});
