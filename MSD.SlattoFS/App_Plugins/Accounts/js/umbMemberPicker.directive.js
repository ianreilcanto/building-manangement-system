angular.module("umbraco.directives")
    .directive('umbMemberPicker', function (dialogService, entityResource) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: '/App_Plugins/Accounts/views/umb-member-picker.html',
            require: "ngModel",
            link: function (scope, element, attr, ctrl) {

                ctrl.$render = function () {
                    var val = parseInt(ctrl.$viewValue);

                    if (!isNaN(val) && angular.isNumber(val) && val > 0) {

                        entityResource.getById(val, "Member").then(function (item) {
                            scope.node = item;
                        });
                    }
                };

                scope.openMemberPicker = function () {
                    //dialogService.mediaPicker({ callback: populatePicture });
                    dialogService.memberPicker({ callback: populateMember });
                }

                scope.removeMember = function () {
                    scope.node = undefined;
                    updateModel(0);
                }

                function populateMember(item) {
                    scope.node = item;
                    updateModel(item.id);
                }

                function updateModel(id) {
                    ctrl.$setViewValue(id);
                }
            }
        };
    });