angular.module("umbraco").controller("AccountsTree.EditController",
 function ($scope, $routeParams, accountResource, accountUserResource, notificationsService, navigationService, dialogService, entityResource, mediaResource, $location) {

     $scope.loaded = false;
     $scope.isChangingPassword = false;

     if ($routeParams.id == -1) {
         $scope.isNew = true;
         $scope.email = {};
         $scope.user = {};
         $scope.account = {};
         $scope.loaded = true;
     }
     else {
         $scope.isNew = false;

         accountResource.getById($routeParams.id).then(function (response) {
             $scope.account = response.data;
             var mediaId = parseInt($scope.account.imageId)

             if ($scope.account.imageId > 0) {
                 entityResource.getById(mediaId, "Media").then(function (item) {
                     $scope.node = item;
                     $scope.node.thumbnail = item.metaData.umbracoFile.Value.src;
                 });
             }
             $scope.loaded = true;
         });
     }

     $scope.save = function (account) {
         accountResource.save(account).then(function (response) {
             var accId = $scope.account.id;
            $scope.account = response.data;
            $scope.accountForm.$dirty = false;
            navigationService.syncTree({ tree: 'accountsTree', path: [-1, -1], forceReload: true });
            if (accId == null) {
                $location.path('/accounts');
            }
            notificationsService.success("Success! Account saved!");
            //organizeFiles();
        });
     };

     $scope.openMediaPicker = function () {
         dialogService.mediaPicker({
             onlyImages: false,
             callback: function (item) {
                 $scope.node = item;
                 $scope.account.ImageId = item.id;
             }
         });
     }

     $scope.removePicture = function () {
         $scope.node = undefined;
         $scope.account.ImageId = 0;
     }

     $scope.changePassword = function () {
         $scope.isChangingPassword = true;
         $scope.user.Password = "";
         $scope.user.Password2 = "";
     }
 });