'use strict';

angular.module('Corporativo.Utils', ['ui.bootstrap'])
.factory('utilsService', function () {
    var service = {};
    service.baseAddress = 'http://localhost:1482';
    service.requestConfig = {
            headers: {
                'Authorization': 'Basic d2VudHdvcnRobWFuOkNoYW5nZV9tZQ==',
                'Accept': 'application/json;odata=verbose',
                'X-Projection-FotoBase64': 'true'
            },
            withCredentials: true
        };

    service.UnidadesFederativas = [{ "Sigla": "AC", "Nome": "Acre" }, { "Sigla": "AL", "Nome": "Alagoas" }, { "Sigla": "AM", "Nome": "Amazonas" }, { "Sigla": "AP", "Nome": "Amapá" },
        { "Sigla": "BA", "Nome": "Bahia" }, { "Sigla": "CE", "Nome": "Ceará" }, { "Sigla": "DF", "Nome": "Distrito Federal" }, { "Sigla": "ES", "Nome": "Espírito Santo" },
        { "Sigla": "GO", "Nome": "Goiás" }, { "Sigla": "MA", "Nome": "Maranhão" }, { "Sigla": "MG", "Nome": "Minas Gerais" }, { "Sigla": "MS", "Nome": "Mato Grosso do Sul" },
        { "Sigla": "MT", "Nome": "Mato Grosso" }, { "Sigla": "PA", "Nome": "Pará" }, { "Sigla": "PB", "Nome": "Paraíba" }, { "Sigla": "PE", "Nome": "Pernambuco" },
        { "Sigla": "PI", "Nome": "Piauí" }, { "Sigla": "PR", "Nome": "Paraná" }, { "Sigla": "RJ", "Nome": "Rio de Janeiro" }, { "Sigla": "RN", "Nome": "Rio Grande do Norte" },
        { "Sigla": "RO", "Nome": "Rondônia" }, { "Sigla": "RR", "Nome": "Roraima" }, { "Sigla": "RS", "Nome": "Rio Grande do Sul" }, { "Sigla": "SC", "Nome": "Santa Catarina" },
        { "Sigla": "SE", "Nome": "Sergipe" }, { "Sigla": "SP", "Nome": "São Paulo" }, { "Sigla": "TO", "Nome": "Tocantins" }];

    service.GetItems = function ($scope, $http, recurso, filtro, id, orderBy) {
        $scope.dataLoading = true;
        
        filtro = filtro != undefined && filtro !== '' ? filtro : '';
        $scope.filtrado = (filtro != undefined && filtro !== '') || (id !== undefined && id !== '');
        var url = this.baseAddress + '/api/v2/' + recurso;
        if (id === undefined || id === '') {
            url += '/?porPagina=' + $scope.porPagina + '&pagina=' + $scope.currentPage + filtro;
            if (orderBy !== undefined && orderBy !== '') {
                url += "&ordem=" + orderBy;
            }
        } else {
            url += '/' + id;
        }

        $http.get(url, this.requestConfig)
            .success(function (data, status, headers, config) {
                if (id === undefined || id === '') {
                    $scope.data = data;
                } else {
                    $scope.data = [data];
                }

                $scope.totalItens = id !== undefined ? 1 : headers("X-Pagination-Total-Count");
                $scope.totalPages = headers("X-Pagination-Total-Pages");
                $scope.currentPage = headers("X-Pagination-Page-Number");
                $scope.dataLoading = false;
            })
            .error(function (err) {
                $scope.dataLoading = false;
            });
    }

	service.LoadPaises = function($scope, $http, pagina, callback) {
		$scope.dataLoading = true;
		if (pagina == undefined) pagina = 1;
	
		$http.get(this.baseAddress + '/api/v2/paises/?porPagina=100&pagina=' + pagina, this.requestConfig)
            .then(function (response) {
            	if ($scope.Paises == undefined) $scope.Paises = [];
                for (var index in response.data) {
                	$scope.Paises.push(response.data[index]);
                }
				if (response.headers("X-Pagination-Total-Pages") != response.headers("X-Pagination-Page-Number")) {
					service.LoadPaises($scope, $http, ++pagina, callback);
				}
				else
				{
					if (callback != undefined) callback();
					$scope.dataLoading = false;
				}
            },
            function() {
            	$scope.dataLoading = false;
            });
	}

    service.FormatEndereco = function(endereco) {
        if (endereco == undefined) return '';
        var complemento = endereco.Complemento != undefined ? ' - ' + endereco.Complemento : '';

        if (endereco.Pais.CodigoISO3 == "BRA") {
            return endereco.Logradouro.Tipo.Codigo + '. ' + endereco.Logradouro.Nome + ', ' + endereco.Numero + complemento +
                ', ' + endereco.Logradouro.BairroInicial.Nome + ', ' + endereco.Logradouro.Localidade.Nome + ' - ' + endereco.Logradouro.UF.Sigla;
        } else {
            return endereco.Street + complemento + ', ' + endereco.Cidade + ' - ' + endereco.Estado + '/' + endereco.Pais.CodigoISO3;
        }
    }

    service.detalhesItem = function($uibModal, item) {
        $uibModal.open({
            animation: true,
            templateUrl: 'detalhes.html',
            controller: 'ModalInstanceCtrl',
            size: 'lg',
            resolve: {
                item: function () {
                    return item;
                }
            }
        });
    }

    service.sort = function ($scope, $http, prop) {
        var sortBy = $scope.sortBy.substring(0, $scope.sortBy.length - 1);
        var sortDir = $scope.sortBy.substring($scope.sortBy.length - 1, $scope.sortBy.length);
        if (prop !== sortBy) {
            $scope.sortBy = prop + "+";
        } else {
            $scope.sortBy = sortBy + (sortDir === "+" ? "-" : "+");
        }

        $scope.data = [];
        this.GetItems($scope, $http, $scope.recurso, $scope.filtro.compilar(), $scope.filtro.Id, $scope.sortBy);
    };
    service.sortClass = function ($scope, prop) {
        var classe = "";
        var sortBy = $scope.sortBy.substring(0, $scope.sortBy.length - 1);
        if (prop !== sortBy) return "";

        var sortDir = $scope.sortBy.substring($scope.sortBy.length - 1, $scope.sortBy.length);
        if (sortDir === "+") {
            classe = "glyphicon glyphicon-triangle-top";
        } else {
            classe = "glyphicon glyphicon-triangle-bottom";
        }

        return classe;
    }

    return service;
})
.controller('ModalInstanceCtrl', function ($scope, $uibModalInstance, utilsService, item) {
    $scope.item = item;

    $scope.Sexo = function (sexo) { return sexo === 1 ? 'Feminino' : 'Masculino' };
    $scope.Endereco = utilsService.FormatEndereco;
    $scope.Foto = function (foto) {
        if (foto != undefined && foto !== '') {
            return 'data:image/jpeg;base64,' + foto;
        } else {
            return '/Content/no_photo.png';
        }
    };

    $scope.ok = function () {
        $uibModalInstance.dismiss('cancel');
    };
})
.directive('autoComplete', ['$http', '$timeout',function($http, $timeout) {
    return {
        restrict: 'AE',
        scope: {
            selectedItem: '=model'
        },
        templateUrl: '/Modules/Utils/views/autocomplete-template.html',
        link: function (scope, elem, attrs) {
            scope.selectedItem = undefined;
            scope.suggestions = [];
            scope.selectedIndex = -1;
            scope.label = attrs.label;

            scope.recursiveSearch = function (pagina) {
                if (pagina == undefined) pagina = 1;
                $http.get(attrs.searchurl + scope.searchText + "&porPagina=30&pagina=" + pagina)
                    .success(function (data, status, headers) {
                        if (data != undefined && data.length === 0) return;

                        if (data.indexOf(scope.searchText) === -1) {
                            data.unshift(scope.searchText);
                        }

                        scope.suggestions.push.apply(scope.suggestions, data);
                        scope.selectedIndex = -1;

                        //scope.recursiveSearch(parseInt(headers("X-Pagination-Page-Number")) + 1);
                    });
            }

            scope.search = function () {
                scope.suggestions = [];
                if (scope.searchText != undefined && scope.searchText.length >= 3) {
                    scope.recursiveSearch(1);
                }
            }

            scope.suggestionsStyle = function() {
                return { 'margin-left' : (scope.label.length / 2 + 1) + '%' };
            }

            scope.setSelectedItem = function(index) {
                scope.selectedItem = scope.suggestions[index];
                scope.searchText = scope.selectedItem.Nome;
                scope.suggestions = [];
            }

            scope.clearSuggestions = function() {
                scope.suggestions = [];
            }

            scope.checkKeyDown = function (event) {
                if (event.keyCode === 40) {
                    event.preventDefault();
                    if (scope.selectedIndex + 1 !== scope.suggestions.length) {
                        scope.selectedIndex++;
                    }
                }
                else if (event.keyCode === 38) {
                    event.preventDefault();
                    if (scope.selectedIndex - 1 !== -1) {
                        scope.selectedIndex--;
                    }
                }
                else if (event.keyCode === 13) {
                    setSelectedItem(scope.selectedIndex);
                }
            }

            scope.$watch('selectedIndex', function (val) {
                if (val !== -1) {
                    scope.searchText = scope.suggestions[scope.selectedIndex].Nome;
                }
            });
            scope.$watch('selectedItem', function(val) {
                if (val == undefined) {
                    scope.searchText = '';
                }
            });
        }
    }
}]);