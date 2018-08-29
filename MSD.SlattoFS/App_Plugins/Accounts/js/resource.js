angular.module("umbraco.resources").factory("accountResource", function ($http) {
    return {
        getById: function (id) {
            return $http.get("backoffice/Accounts/AccountApi/GetById?id=" + id);
        },
        getAccountsFolderId: function () {
            return $http.get("backoffice/Accounts/AccountApi/GetAccountsFolderId");
        },
        save: function (account) {
            return $http.post("backoffice/Accounts/AccountApi/PostSave", angular.toJson(account));
        },
        deleteById: function (id) {
            return $http.delete("backoffice/Accounts/AccountApi/DeleteById/" + id);
        },
        getPaged: function (itemsPerPage, pageNumber, sortColumn, sortOrder, searchTerm) {
            if (sortColumn == undefined)
                sortColumn = "";
            if (sortOrder == undefined)
                sortOrder = "";
            return $http.get("backoffice/Accounts/AccountApi/GetPaged?itemsPerPage=" + itemsPerPage + "&pageNumber=" + pageNumber + "&sortColumn=" + sortColumn + "&sortOrder=" + sortOrder + "&searchTerm=" + searchTerm);
        },
        deleteByBatch: function (ids) {
            return $http.post("backoffice/Accounts/AccountApi/PostDeleteByBatch", ids);
        }
    };
});

angular.module("umbraco.resources").factory("accountUserResource", function ($http) {
    return {
        getById: function (id) {
            return $http.get("backoffice/Accounts/AccountUserApi/GetById?id=" + id);
        },
        save: function (user) {
            return $http.post("backoffice/Accounts/AccountUserApi/PostSave", angular.toJson(user));
        },
        deleteById: function (id) {
            return $http.delete("backoffice/Accounts/AccountUserApi/DeleteById?id=" + id);
        }
    };
});
