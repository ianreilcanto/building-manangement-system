angular.module("umbraco").controller("AccountsTree.DeleteController",
function ($scope, accountResource, accountUserResource, treeService, navigationService, notificationsService, entityResource, mediaResource, contentResource, contentEditingHelper, $route) {
    $scope.delete = function (id) {
        accountResource.deleteById(id).then(function () {
            treeService.removeNode($scope.currentNode);
            navigationService.syncTree({ tree: 'AccountsTree', path: [-1, -1], forceReload: true, activate: true });
            navigationService.hideNavigation();
            $route.reload();
            notificationsService.success("Success! Account has been deleted.");
        });
    };
    $scope.cancelDelete = function () {
        navigationService.hideNavigation();
    };
});