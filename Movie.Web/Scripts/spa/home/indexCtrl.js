(function (app) {
    'use strict';

    app.controller('indexCtrl', indexCtrl);
    app.filter('utcConvert', function () {
        return function (item) {
            return moment(item).format('YYYY-MM-DD HH:mm');
        };
    });

    indexCtrl.$inject = ['$scope','apiService', 'notificationService','$filter'];

    function indexCtrl($scope, apiService, notificationService) {
        console.log('starting indexCtrl');
        $scope.pageClass = 'page-home';
        $scope.loadingMovies = true;        
        $scope.isReadOnly = true;
        $scope.page = 0;
        $scope.pagesCount = 0;

        $scope.Movies = [];
        $scope.sources = ["Integrated", "YouTube", "TMDB", "Cache"];        
        $scope.dtFrom = new Date();
        $scope.dtTo = new Date();
        $scope.calendarDisabled1 = false;
        $scope.calendarDisabled2 = false;

        $scope.loadData = loadData;
        $scope.search = search;
        $scope.clearSearch = clearSearch;
                
        $scope.facebookShare = facebookShare;
        $scope.twitterShare = twitterShare;
        
        function loadData() {
            apiService.get('/movies/FromRedisCacheAll/', null,
                        moviesLoadCompleted,
                        moviesLoadFailed);         
        }
        

        function moviesLoadCompleted(result) {
            $scope.Movies = result.data.Items;
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingMovies = false;

            if ($scope.filterMovies && $scope.filterMovies.length) {
                notificationService.displayInfo(result.data.Items.length + ' movies paged');
            }
        }     
      
        function facebookShare(title,link,picture,caption,description) {  
            FB.ui({
                method: 'feed',
                name: title,
                link: link,
                picture: picture,
                caption: caption,
                description: description
            });
        }

        function twitterShare(link, text) {
            newwindow = window.open("https://twitter.com/intent/tweet?text=" + text + "&url=" + link, 'name', 'height=550,width=450');
            if (window.focus) { newwindow.focus() }
            return false;
        }

        function moviesLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterMovies = '';
            search();
        }

        function search(page) {
            page = page || 0;

            $scope.loadingMovies = true;

            if ($scope.selectedSource == null) {
                $scope.selectedSource = $scope.sources[0];
            }
            var config = {
                params: {
                    page: page,
                    pageSize: 6,
                    filter: $scope.filterMovies,
                    source: $scope.selectedSource,
                    actor: $scope.filterActor,
                    from: moment($scope.dtFrom).year(),
                    to: moment($scope.dtTo).year()
                }
            };         

            apiService.get('/movies/Pages/', config,
            moviesLoadCompleted,
            moviesLoadFailed);
        }

        $scope.openFrom = function ($event1) {
            $event1.preventDefault();
            $event1.stopPropagation();

            $scope.openedFrom = true;
        };

        $scope.openTo = function ($event2) {
            $event2.preventDefault();
            $event2.stopPropagation();

            $scope.openedTo = true;
        };

        $scope.selectedSourceEvt = function (name) {
            console.log($scope.selectedSource);
            if ($scope.selectedSource === 'YouTube') {
                $scope.calendarDisabled1 = true;
                $scope.calendarDisabled2 = true;
            }
            else {
                $scope.calendarDisabled1 = false;
                $scope.calendarDisabled2 = false;
            }
        };
                    
        $scope.search();        
    }

})(angular.module('TamTamCinema'));
   