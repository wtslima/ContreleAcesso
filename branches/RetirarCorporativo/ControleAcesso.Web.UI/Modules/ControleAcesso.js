'use strict';

angular.module('ControleAcesso', [
	'ngRoute',
	'Corporativo.Utils',
	'Corporativo.Home',
	'Corporativo.Empresas',
	'ControleAcesso.Login'
])
.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.otherwise({ redirectTo: '/' });
}]);