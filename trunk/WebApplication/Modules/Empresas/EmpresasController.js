'use strict';

angular.module('Corporativo.Empresas', ['ngRoute', 'ui.mask', 'Corporativo.Utils'])
.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
    	.when('/empresas', {
    	    templateUrl: 'Modules/Empresas/Views/Index.html',
    	    controller: 'EmpresasController'
		})
		.when('/empresas/:empresaId', {
    	    templateUrl: 'Modules/Empresas/Views/Edit.html',
    	    controller: 'EmpresasEditController'
		})
		.when('/empresas/nova', {
    	    templateUrl: 'Modules/Empresas/Views/Edit.html',
    	    controller: 'EmpresasEditController'
		})
}])
.controller('EmpresasController', function ($scope, $http, $uibModal, utilsService) {
    $scope.porPagina = 10;
    $scope.currentPage = 1;
    $scope.totalItens = 0;
    $scope.irParaPagina = '';
    $scope.sortBy = 'Id+';
    $scope.recurso = "empresas";
    utilsService.GetItems($scope, $http, $scope.recurso);

    $scope.filtro = {
        Id: undefined,
        CNPJ: undefined,
        Nome: undefined,
        NomeExato: undefined,
        SiglaUF: undefined,
        Pais: undefined,
        compilar: function (c) {
            var params = new Array();
            if (this.Nome != undefined && this.Nome !== '') params.push('Nome=' + this.Nome);
            if (this.NomeExato != undefined && this.NomeExato !== '') params.push('NomeExato=' + this.NomeExato);
            if (this.SiglaUF != undefined && this.SiglaUF !== '') params.push('SiglaUf=' + this.SiglaUF);
            if (this.Pais != undefined && this.Pais !== '') params.push('CodigoPais=' + this.Pais);

            var param = '';
            for (var i = 0; i < params.length; i++) {
                param += '&' + params[i];
            }

            return param;
        },
        limpar: function() {
            this.Id = undefined;
            this.Nome = undefined;
            this.CNPJ = undefined;
            this.NomeExato = undefined;
            this.SiglaUF = undefined;
            this.Pais = undefined;
        },
        identificador: function (i) {
            if (this.Id != undefined && this.Id !== '') return this.Id;
            if (this.CNPJ != undefined && this.CNPJ !== '') return 'PorCNPJ/?cnpj=' + this.CNPJ;
        }
    };

    $scope.UFs = utilsService.UnidadesFederativas;
    $scope.LoadPaises = utilsService.LoadPaises($scope, $http);
    
    $scope.Endereco = utilsService.FormatEndereco;
    $scope.pageChanged = function () {
        $scope.data = [];
        utilsService.GetItems($scope, $http, $scope.recurso, $scope.filtro.compilar(), $scope.filtro.identificador(), $scope.sortBy);
    };
    $scope.detalhes = function (item) {
    	$uibModal.open({
            animation: true,
            templateUrl: 'detalhes.html',
            controller: 'ModalEmpresaCtrl',
            size: 'lg',
            resolve: {
                item: function () {
                    return item;
                }
            }
        });
    };
    $scope.paraPagina = function () {
        $scope.data = [];
        $scope.currentPage = $scope.irParaPagina;
        utilsService.GetItems($scope, $http, $scope.recurso, $scope.filtro.compilar(), $scope.filtro.identificador(), $scope.sortBy);
    };
    $scope.filtrar = function () {
        $scope.data = [];
        $scope.currentPage = 1;
        $scope.totalItens = 0;
        $scope.showFilter = false;
        utilsService.GetItems($scope, $http, $scope.recurso, $scope.filtro.compilar(), $scope.filtro.identificador(), $scope.sortBy);
    };
    $scope.limparFiltro = function () {
        $scope.data = [];
        $scope.filtro.limpar();
        $scope.currentPage = 1;
        utilsService.GetItems($scope, $http, $scope.recurso, $scope.filtro.compilar(), undefined, $scope.sortBy);
    };
    $scope.sort = function (prop) {
        utilsService.sort($scope, $http, prop);
    };
    $scope.sortClass = function (prop) {
        return utilsService.sortClass($scope, prop);
    }

    $scope.editar = function(empresa) {
        $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'edit.html',
            controller: 'EditarEmpresaController',
            size: 'lg',
            resolve: {
                item: function() {
                    return empresa;
                }
            }
        });
    };
    
    $scope.excluir = function(empresa) {
        var modal = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'excluir.html',
            controller: 'ExclusaoEmpresaDialogController',
            size: 'lg',
            resolve: {
                item: function() {
                    return empresa;
                }
            }
        });
        
        modal.result.then(function () {
        	$scope.limparFiltro();
        });
    };

    $scope.openDateTimePicker = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.opened = true;
    };

    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1
    };
})
.controller('ModalEmpresaCtrl', function ($scope, $uibModalInstance, utilsService, item) {
    $scope.item = item;
    $scope.Endereco = utilsService.FormatEndereco;
    $scope.ok = function () {
        $uibModalInstance.dismiss('cancel');
    };
})
.controller('ExclusaoEmpresaDialogController', function($scope, $http, $uibModalInstance, utilsService, item) {
	$scope.item = item;
	
	$scope.verificaValidade = function() {
    	if (!$scope.deleteEmpresa.nomeempresa.$pristine || $scope.deleteEmpresa.$submitted) {
	    	var test = $scope.item.Nome.toLowerCase().trim() == $scope.deleteEmpresa.nomeempresa.$modelValue.toLowerCase();
	    	$scope.deleteEmpresa.$setValidity('nomeempresa', test);
	    	
	    	if (test) {
	    		return 'ng-valid';
	    	} else {
	    		return 'ng-invalid';
	    	}
    	}
    };
	
	$scope.exclusaoChange = function() {
		if (!$scope.deleteEmpresa) return;
		
	};
	
	$scope.excluir = function() {
		$scope.deleteEmpresa.$setSubmitted();
		if ($scope.deleteEmpresa.$valid) {
			$http.delete(utilsService.baseAddress + '/api/v2/empresas/' + $scope.item.Id, utilsService.requestConfig)
				.then(function () {
					alert('Empresa excluída.');
				}, function() {
					alert('Ocorreu um erro ao excluir a empresa.\rPor favor, tente novamente.');
				});
		
			$uibModalInstance.close(item);
		} else {
			alert('Verifique o nome informado.');
		}
	};
	
	$scope.cancelar = function () {
        $uibModalInstance.dismiss('cancel');
    };
})
.controller('EmpresasEditController', function($scope, $routeParams, $location, $http, $q, utilsService) {
	$scope.Id = $routeParams.empresaId;
	$scope.dataLoading = true;
   $scope.situacoesReceitaFederal = [
      {id: 'A', name: 'Ativa'},
      {id: 'S', name: 'Suspensa'},
      {id: 'I', name: 'Inativa'}
   ];

	$scope.LoadNaturezasJuridicas = function($scope, $http, pagina) {
    	$scope.dataLoading = true;
		if (pagina == undefined) pagina = 1;
	
		$http.get(utilsService.baseAddress + '/api/v2/naturezasJuridicas/?ordem=Descricao&porPagina=100&pagina=' + pagina, utilsService.requestConfig)
            .success(function (data, status, headers, config) {
            	if ($scope.NaturezasJuridicas == undefined) $scope.NaturezasJuridicas = [];
                for (var index in data) {
                	$scope.NaturezasJuridicas.push(data[index]);
                }
                
				if (headers("X-Pagination-Total-Pages") != headers("X-Pagination-Page-Number")) {
					$scope.LoadNaturezasJuridicas($scope, $http, ++pagina);
				}
				else
				{
					$scope.dataLoading = false;
				}
            })
            .error(function (err) {
                $scope.dataLoading = false;
            });
    };
	
    $scope.salvar = function() {
    	$scope.editEmpresa.$setSubmitted();
		
		if ($scope.editEmpresa.$valid) {
			$scope.dataLoading = true;
			$scope.data.CNPJ = undefined;
			var method = 'PUT';
			var url = utilsService.baseAddress + '/api/v2/empresas/' + $scope.data.Id;
			
			if ($scope.Id === 'nova') {
				method = 'POST';
				url = utilsService.baseAddress + '/api/v2/empresas';
			}
			
			var config = {
				method: method,
				url: url,
				data: $scope.data,
				headers: utilsService.requestConfig.headers,
				withCredentials: true
			};
			
			$http(config)
				.then(function (data, status, headers, config){
					$location.path('empresas');
				},function (err){
					alert('Ocorreu um erro ao enviar as informações.');
					$scope.dataLoading = false;
				});
		} else {
			alert('Verfique os campos do formulário.');
		}
    };
    
    function selecionarPais() {
    	/*
    		Esta função é necessária pois, devido ao fato da chave do objeto Pais ser uma string,
    		não é possível utilizar a instrução 'track by' diretamente no 'ng-options', uma vez
    		que isso provoca uma série de erros.
    	*/
    	if (!$scope.data || !$scope.data.PaisOrigem) return;
    	
    	for (var i = 0; i < $scope.Paises.length; i++) {
    		if ($scope.Paises[i].CodigoISO3 == $scope.data.PaisOrigem.CodigoISO3) {
    			$scope.data.PaisOrigem = $scope.Paises[i];
    			break;
    		}
    	}
    }
    
    function Init() {
    	if ($scope.Id === 'nova') {
    		utilsService.LoadPaises($scope, $http, 1, selecionarPais);
			$scope.LoadNaturezasJuridicas($scope, $http);
			$scope.dataLoading = false;
			$scope.data = { Documentos: [], Enderecos: [], Contatos: [] };
			return;
    	}
    
		$http.get(utilsService.baseAddress + '/api/v2/empresas/'+ $scope.Id, utilsService.requestConfig)
			.then(function (response) {
				$scope.data = response.data;
				if ($scope.data.RegistroJuridico && $scope.data.RegistroJuridico.Abertura) $scope.data.RegistroJuridico.Abertura = new Date($scope.data.RegistroJuridico.Abertura.substring(0, 10));
				utilsService.LoadPaises($scope, $http, 1, selecionarPais);
				$scope.LoadNaturezasJuridicas($scope, $http);
				$scope.dataLoading = false;
			});
	}
	
	Init();
});