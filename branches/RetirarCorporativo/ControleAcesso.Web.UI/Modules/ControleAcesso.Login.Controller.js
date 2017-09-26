angular.module("ControleAcesso.Login", [])
    .controller("LoginController", function($scope, $http, $uibModal) {
        $scope.getApiInfo = function () {
				$http.get('/api/v2/info').success(function (data) {
					$uibModal.open({
						animation: true,
						templateUrl: 'apiInfo.html',
						controller: 'ModalInstanceCtrl',
						size: 'lg',
						resolve: {
							info : function() {
								return data;
							}
						}
					});
				});
            };
    	$scope.getHelp = function() {
    		$uibModal.open({
						animation: true,
						templateUrl: 'apiHelp.html',
						controller: 'ModalInstanceCtrl',
						size: 'lg',
						resolve: {
							info : undefined
						}
					});
    	};
    })
    .controller('ModalInstanceCtrl', function ($scope, $uibModalInstance, $http, info) {
		$scope.info = info;
	
        $scope.ok = function () {
            $uibModalInstance.dismiss('cancel');
        };
    });