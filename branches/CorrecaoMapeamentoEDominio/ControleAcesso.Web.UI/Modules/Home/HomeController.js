'use strict';

angular.module('ControleAcesso.Home', ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        .when('/', {
            templateUrl: 'Modules/Home/Views/Index.html',
            controller: 'HomeController'
        });
}])
.controller('HomeController', ['$scope', function($scope) {
  
}]);