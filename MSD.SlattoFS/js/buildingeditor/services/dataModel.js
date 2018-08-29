(function () {
    'use strict';
    var injectParams = [];
    var dataModel = function () {

        var factory = {};
        factory.BaseApartment = function (model) {
            var vm = this;
            vm.Id = model.Id;
            vm.Room = model.Room;
        };

        factory.Apartment = function (model) {

            var vm = this;

            vm.Id = model.Id;
            vm.Room = model.Room;
            vm.BuildingId = model.BuildingId;
            vm.Status = ParseStatus(model.StatusId);
            vm.StatusId = model.StatusId;
            vm.Price = model.Price;
            vm.Size = model.Size;
            vm.FileAssets = [];

            if (typeof model.FileAssets !== 'undefined' && model.FileAssets.length > 0) {
                angular.forEach(model.FileAssets, function (value, key) {
                    var fileAsset = new factory.FileAsset(value);
                    vm.FileAssets.push(fileAsset);
                });
            }
        }

        factory.FileAsset = function (model) {
            this.Url = model.Url;
            this.Type = "";
            this.MediaId = model.MediaId;
        }

        factory.BuildingAsset = function (model) {
            this.Id = model.Id;
            this.Url = model.Url;
        };

        var RoomStatus = {
            1: "Available",
            2: "Rented",
            3: "Reserved",
            4: "Unknown"
        };
        function ParseStatus(statusId) {
            return RoomStatus[statusId] ? RoomStatus[statusId] : "Unknown";
        }
        return factory;
    };

    dataModel.$injectParams = injectParams;
    angular.module("buildingEditorApp").factory("dataModel", dataModel);

})();