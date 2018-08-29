(function () {
    'use strict';
    var injectParams = ['appConfig'];

    var templateProvider = function (appConfig) {

        // The provider must include a $get() method This $get() method  
        // will be invoked using $injector.invoke() and can therefore use  
        // dependency-injection.
        this.$get = function () {
            return {};
        };

        this.GetTemplate = function (templateName) {
            return appConfig.BASEAPP_VIEWSURL + "/" + templateName + ".html"
        };

    };

    templateProvider.$injectParams = injectParams;
    angular.module("buildingEditorApp").provider("template", templateProvider);

})();