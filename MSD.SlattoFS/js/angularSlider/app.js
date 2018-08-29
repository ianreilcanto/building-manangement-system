angular.module('website', [])
    .controller('MainCtrl', function ($scope) {

     
        $scope.slides = [];

        var images = $(".image-values");


        angular.forEach(images, function (value, index) {
            var url = $(value).val();
            var id = $(value).attr("id");

            var dataToPass = { image: url, imageId: id };

            $scope.slides.push(dataToPass);
        });

     

        $scope.direction = 'left';
        $scope.currentIndex = 0;

        $scope.setCurrentSlideIndex = function (index) {
            $scope.direction = (index > $scope.currentIndex) ? 'left' : 'right';
            $scope.currentIndex = index;

            $('.svg').find('svg').remove();
            $('.tooltip').remove();
        };

        $scope.isCurrentSlideIndex = function (index) {
            return $scope.currentIndex === index;
        };

        $scope.prevSlide = function () {
            $scope.direction = 'left';
            $scope.currentIndex = ($scope.currentIndex < $scope.slides.length - 1) ? ++$scope.currentIndex : 0;
                    $('.svg').find('svg').remove();
                    $('.tooltip').remove();
        };

        $scope.nextSlide = function () {
            $scope.direction = 'right';
            $scope.currentIndex = ($scope.currentIndex > 0) ? --$scope.currentIndex : $scope.slides.length - 1;
                    $('.svg').find('svg').remove();
                    $('.tooltip').remove();
        };
    });