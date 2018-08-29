(function () {
    'use strict';
    var injectParams = ['appConfig','$scope', '$http', '$route'];
    var EditorMediaController = function (appConfig,$scope, $http, $route) {

        var vm = this;
                
        $scope.GetFileDetails = function (e) {

            if (typeof e === 'undefined') return;
            for (var i = 0; i < e.files.length; i++) {
                vm.Files.push(e.files[i]);
            }
        };

        vm.RemoveMedia = function (media) {
            RemoveMedia(media);
        };

        vm.SaveSort = function () {

            var fd = new FormData();

            for (var i = 0; i < vm.MediaFiles.length ; i++) {
                var media = vm.MediaFiles[i];
                if (media.Name.length == 0 || media.Name == "") {
                    media.Name = media.Id; //default
                }
                var mediaString = media.Id + "|" + media.SortOrder + "|" + media.Name;
                fd.append("mediaUpdate", mediaString);
            }

            fd.append("accountId", appConfig.ACCOUNT_ID);
            fd.append("buildingId", appConfig.BUILDING_ID);

            $http({

                url: appConfig.MEDIASORT_URL,
                method: "POST",
                data: fd,
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            })
            .then(function successCallBack(response) {

                //if (typeof response.data !== 'undefined' && response.data.hasData) {
                //    vm.MediaFiles = response.list;
                //    SortMediaFiles();
                //}

            }, function errorCallBack(response) {

            });
        };

        vm.SortUp = function (media) {

            var currentOrder = media.SortOrder;
            var newOrder = media.SortOrder - 1;

            //if new order is 0 or less then don't change
            if (newOrder <= 0) return;
            
            
            angular.forEach(vm.MediaFiles, function (value, key) {
                if (value.SortOrder === newOrder) {
                    var prev = value;
                    prev.SortOrder = newOrder + 1; //switch
                    media.SortOrder = newOrder;
                }
            });
           
        };

        vm.SortDown = function (media) {

            var currentOrder = media.SortOrder;
            var newOrder = media.SortOrder + 1;

            //if new order is more than total length of media files
            if (newOrder > vm.MediaFiles.length) return;


            angular.forEach(vm.MediaFiles, function (value, key) {
                if (value.SortOrder === newOrder) {
                    var next = value;
                    next.SortOrder = newOrder - 1; //switch
                    media.SortOrder = newOrder;
                }
            });

        };

        vm.MediaFiles = [];
        vm.UploadFiles = function () {

            var fd = new FormData();
            fd.append("accountId", appConfig.ACCOUNT_ID);
            fd.append("buildingId", appConfig.BUILDING_ID);

            for (var i = 0; i < vm.Files.length; i++) {
                fd.append('file' + i, vm.Files[i]);
            }

            $http({

                url: appConfig.MEDIAUPLOAD_URL,
                method: "POST",
                data: fd,
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            })
            .then(function successCallBack(response) {
                
                if (typeof response.data !== 'undefined' && response.data.hasData) {
                    vm.MediaFiles = response.list;
                    SortMediaFiles();
                    $route.reload();
                }
                
            },function errorCallBack(response) {

            });

        };

        function RemoveMedia(media) {
            var mediaFound = GetMedia(media.Id);
            if (mediaFound) {

                $http({
                    url: appConfig.MEDIAREMOVE_URL,
                    method: "POST",
                    data: { id: mediaFound.Id },
                    headers: { 'Content-Type' : "application/json"}
                }).then(function success(response) {

                    for (var i = 0; i < vm.MediaFiles.length; i++) {
                        var media = vm.MediaFiles[i];
                        if (media.Id === mediaFound.Id) {
                            vm.MediaFiles.splice(i, 1);
                            SortMediaFiles();
                            break;
                        }
                    }

                }, function error(response) { })

            }
        }

        function GetMedia(mediaId) {
            for (var i = 0; i < vm.MediaFiles.length; i++) {
                var media = vm.MediaFiles[i];
                if (media.Id === mediaId) {
                    return media;
                }
            }
        }

        function PopulateMediaList() {

            var fd = new FormData();

            fd.append("accountId", appConfig.ACCOUNT_ID);
            fd.append("buildingId", appConfig.BUILDING_ID);

            $http({
                url: appConfig.MEDIAASSETS_URL,
                method: "POST",
                data: fd,
                dataType: "json",
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            }).then(function successCallBack(response) {
                if (typeof response.data !== 'undefined') {
                    vm.MediaFiles = response.data.list;
                    SortMediaFiles();
                }

            }, function errorCallBack(response) {

            });
        }

        function SortMediaFiles() {
            var startIndex = 1;
            angular.forEach(vm.MediaFiles, function (value, key) {
                value.SortOrder = startIndex;
                startIndex++;
            });
            console.log(vm.MediaFiles);
        }

        function Initialize() {
            vm.Files = [];
            vm.MediaFiles = [];
            vm.Message = "From Media Controller";

            PopulateMediaList();
        }

        Initialize();

    };

    EditorMediaController.$injectParams = injectParams;
    angular.module("buildingEditorApp").controller("EditorMediaController", EditorMediaController);

})();