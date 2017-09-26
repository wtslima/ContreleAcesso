'use strict';

angular.module('Corporativo.Utils')
.directive('documentos', function($uibModal) {
    return {
        restrict: 'AE',
        scope: {
            documentos: '=model'
        },
        templateUrl: 'Modules/Utils/views/documentos-list.html',
        link: function (scope, elem, attrs) {
        	scope.NumeroDocumento = function(documento) {
        		if (documento.Tipo.Id == 9) {
        			var str = documento.Numero + '';
			        str = str.replace(/\D/g, '');
			        str = str.replace(/^(\d{2})(\d)/, '$1.$2');
			        str = str.replace(/^(\d{2})\.(\d{3})(\d)/, '$1.$2.$3');
			        str = str.replace(/\.(\d{3})(\d)/, '.$1/$2');
			        str = str.replace(/(\d{4})(\d)/, '$1-$2');
			        return str;
        		}
        		
        		return documento.Numero;
        	}
        	
        	scope.remover = function(documento) {
        		var idx = scope.documentos.indexOf(documento);
        		scope.documentos.splice(idx, 1);
        	}
        	
        	scope.editar = function(documento) {
		        var modalInstance = $uibModal.open({
		            templateUrl: 'editarDocumento.html',
		            controller: 'DocumentosController',
		            size: 'lg',
		            resolve: {
		                documento: function() {
		                	if (documento == undefined) {
		                		documento = { Tipo: 0, Numero: '' }
		                	}
		                
							return {
								Index:		scope.documentos.indexOf(documento),
								Nome:		attrs.nome,
								TipoPessoa:	attrs.tipo,
								Tipo:		documento.Tipo,
								Numero:		documento.Numero,
								Emissao:	documento.Emissao,
								Validade:	documento.Validade,
								Observacao:	documento.Observacao
							};                    
		                }
		            }
		        });
		        
		        modalInstance.result.then(function (novoDocumento) {
		        	documento.Tipo = novoDocumento.Tipo;
			    	documento.Numero = novoDocumento.Numero;
			    	documento.Emissao = novoDocumento.Emissao;
			    	documento.Validade = novoDocumento.Validade;
			    	documento.Observacao = novoDocumento.Observacao;
		        	if (novoDocumento.Index == -1) {
				    	scope.documentos.push(documento);
			    	} else {
			    		scope.documentos[novoDocumento.Index] = documento;
			    	}
			    });
		    };
        }
    }
 })
 .controller('DocumentosController', function($scope, $http, $uibModalInstance, utilsService, documento) {
	$scope.documento = documento;
	$scope.UnidadesFederativas = utilsService.UnidadesFederativas;
	
	$scope.salvar = function() {
		$scope.editDocumento.$setSubmitted();
		$scope.verificaValidade();
		
		if ($scope.editDocumento.$valid) {
			$uibModalInstance.close($scope.documento);
		}
	};
    $scope.cancelar = function () {
        $uibModalInstance.dismiss('cancel');
    };
    
    $scope.verificaValidade = function() {
    	if (!$scope.editDocumento.tipodedocumento.$pristine || $scope.editDocumento.$submitted) {
	    	$scope.editDocumento.$setValidity('tipodedocumento', $scope.editDocumento.tipodedocumento.$viewValue instanceof Object);
	    	if ($scope.editDocumento.tipodedocumento.$viewValue instanceof Object) {
	    		return 'ng-valid';
	    	} else {
	    		return 'ng-invalid';
	    	}
    	}
    };
    
    function loadTiposDocumentos(pagina) {
    	$scope.dataLoading = true;
    	
    	$http.get(utilsService.baseAddress + '/api/v2/tiposdocumentos/?porPagina=30&pagina='+pagina, utilsService.requestConfig)
    	.then(function(response) {
    		if ($scope.TiposDocumentos == undefined) $scope.TiposDocumentos = [];
            for (var index in response.data) {
            	var tipo = response.data[index];
            	if (tipo.TipoPessoa == documento.TipoPessoa)
            		$scope.TiposDocumentos.push(tipo);
            }
    		
    		if (response.headers("X-Pagination-Total-Pages") != response.headers("X-Pagination-Page-Number")) {
				LoadTiposDocumentos($scope, $http, ++pagina);
			}
			else
			{
				$scope.dataLoading = false;
			}
    	});
    }
    
    function selecionarPais() {
    	/*
    		Esta função é necessária pois, devido ao fato da chave do objeto Pais ser uma string,
    		não é possível utilizar a instrução 'track by' diretamente no 'ng-options', uma vez
    		que isso provoca uma série de erros.
    	*/
    	for (var i = 0; i < $scope.Paises.length; i++) {
    		if ($scope.documento.Emissao == undefined || $scope.documento.Emissao.Pais == undefined) break;
    		
    		if ($scope.Paises[i].CodigoISO3 == $scope.documento.Emissao.Pais.CodigoISO3) {
    			$scope.documento.Emissao.Pais = $scope.Paises[i];
    			break;
    		}
    	}
    }
    
    function selecionarUF() {
    	for (var i = 0; i < $scope.UnidadesFederativas.length; i++) {
    		if ($scope.documento.Emissao == undefined || $scope.documento.Emissao.UF == undefined) break;
    		
    		if ($scope.UnidadesFederativas[i].Sigla == $scope.documento.Emissao.UF.Sigla) {
    			$scope.documento.Emissao.UF = $scope.UnidadesFederativas[i];
    			break;
    		}
    	}
    }
    
    function Init() {
		loadTiposDocumentos();
	    utilsService.LoadPaises($scope, $http, 1, selecionarPais);
	    selecionarUF();
		
		if ($scope.documento.Emissao && $scope.documento.Emissao.Data) {
			if ($scope.documento.Emissao.Data instanceof Date) { 
				$scope.documento.Emissao.Data = $scope.documento.Emissao.Data;
			} else {
				$scope.documento.Emissao.Data = new Date($scope.documento.Emissao.Data.substring(0, 10));
			}
		}
		if ($scope.documento.Validade) {
			if ($scope.documento.Validade instanceof Date) {
				$scope.documento.Validade = $scope.documento.Validade;
			} else {
				$scope.documento.Validade = new Date($scope.documento.Validade.toString().substring(0, 10));
			}
		}
    }
    
    Init();
})

.directive('contatos', function($uibModal) {
    return {
        restrict: 'AE',
        scope: {
            contatos:	'=model'
        },
        templateUrl: 'Modules/Utils/views/contatos-list.html',
        link: function (scope, elem, attrs) {        
        	scope.ValorContato = function(contato) {
        		if (contato.Tipo.Codigo.substring(0,1) == 'T') {
        			var str = contato.Valor + '';
			        str = str.replace(/\D/g, '');
			        if (str.length === 11) {
			            str = str.replace(/^(\d{2})(\d{5})(\d{4})/, '($1) $2-$3');
			        } else {
			            str = str.replace(/^(\d{2})(\d{4})(\d{4})/, '($1) $2-$3');
			        }
			        return str;
        		}
        		
        		return contato.Valor;
        	}
        	
        	scope.removerContato = function(contato) {
        		var idx = scope.contatos.indexOf(contato);
        		scope.contatos.splice(idx, 1);
        	}
        	
        	scope.editar = function(contato) {
		        var modalInstance = $uibModal.open({
		            templateUrl: 'editarContato.html',
		            controller: 'ContatosController',
		            size: 'lg',
		            resolve: {
		                contato: function() {
		                	if (contato == undefined) {
		                		contato = { Tipo: '', Valor: '' }
		                	}
		                
							return {
								Index:	scope.contatos.indexOf(contato),
								Nome:	attrs.nome,
								Tipo:	contato.Tipo,
								Valor:	contato.Valor
							};                    
		                }
		            }
		        });
		        
		        modalInstance.result.then(function (novoContato) {
		        	contato.Tipo = novoContato.Tipo;
			    	contato.Valor = novoContato.Valor;
		        	if (novoContato.Index == -1) {
				    	scope.contatos.push(contato);
			    	} else {
			    		scope.contatos[novoContato.Index] = contato;
			    	}
			    });
		    };
        }
    }
 })
 .controller('ContatosController', function($scope, $http, $uibModalInstance, utilsService, contato) {
	$scope.contato = contato;
	$scope.salvar = function() {
		$scope.editContato.$setSubmitted();
		if ($scope.editContato.$valid) {
			$uibModalInstance.close($scope.contato);
		}
	};
    $scope.cancelar = function () {
        $uibModalInstance.dismiss('cancel');
    };
    
    $scope.ChangedTipoContato = function(item) {
		if (item && item.Descricao && item.Descricao.substring(0,1) == 'T') {
			//$scope.mask = '(99) ?99999-9999';
			$scope.mask = '';
		} else {
			$scope.mask = '';
		}
    };
    
    function loadTiposContatos(pagina) {
    	$scope.dataLoading = true;
    	
    	$http.get(utilsService.baseAddress + '/api/v2/tiposcontatos/?porPagina=30&pagina='+pagina, utilsService.requestConfig)
    	.then(function(response) {
    		if ($scope.TiposContatos == undefined) $scope.TiposContatos = [];
            for (var index in response.data) {
            	$scope.TiposContatos.push(response.data[index]);
            }
    		
    		if (response.headers("X-Pagination-Total-Pages") != response.headers("X-Pagination-Page-Number")) {
				LoadTiposContatos($scope, $http, ++pagina);
			}
			else
			{
				$scope.ChangedTipoContato($scope.contato.Tipo);
				selecionarTipoContato();
				$scope.dataLoading = false;
			}
    	});
    }
    
    function selecionarTipoContato() {
    	for (var i = 0; i < $scope.TiposContatos.length; i++) {
    		if ($scope.TiposContatos[i].Codigo == $scope.contato.Tipo.Codigo) {
    			$scope.contato.Tipo = $scope.TiposContatos[i];
    			break;
    		}
    	}
    }
    
    loadTiposContatos();
})
 
.directive('enderecos', function($uibModal) {
	return {
	    restrict: 'AE',
	    scope: {
	        enderecos: '=model'
	    },
	    templateUrl: 'Modules/Utils/views/enderecos-list.html',
	    link: function (scope, elem, attrs) {
	    	scope.CodigoPostal = function(endereco) {
	    		if (endereco.Pais.CodigoISO3 == 'BRA') {
	    			var str = endereco.CEP + '';
			        str = str.replace(/\D/g, '');
			        str = str.replace(/^(\d{2})(\d{3})(\d)/, '$1.$2-$3');
			        return str;
	    		} else {
	    			return endereco.ZipCode;
	    		}
	    	}
	    	
	    	scope.Estado = function(endereco) {
	    		if (endereco.Pais.CodigoISO3 == 'BRA') {
	    			return endereco.Logradouro.UF.Nome;
	    		} else {
	    			return endereco.Estado;
	    		}
	    	}
	    	
	    	scope.Cidade = function(endereco) {
	    		if (endereco.Pais.CodigoISO3 == 'BRA') {
	    			return endereco.Logradouro.Localidade.Nome;
	    		} else {
	    			return endereco.Cidade;
	    		}
	    	}
	    	
	    	scope.Logradouro = function(endereco) {
	    		if (endereco.Pais.CodigoISO3 == 'BRA') {
	    			return endereco.Logradouro.Tipo.Descricao + ' ' +  endereco.Logradouro.Nome + ', ' + endereco.Numero.trim() + ' - ' + endereco.Logradouro.BairroInicial.Nome;
	    		} else {
	    			return endereco.Street;
	    		}
	    	}
	    	
	    	scope.remover = function(endereco) {
        		var idx = scope.enderecos.indexOf(endereco);
        		scope.enderecos.splice(idx, 1);
        	}
        	
        	function openModal($uibModal, endereco, templateUrl, controllerName) {
        		return $uibModal.open({
		            templateUrl: templateUrl,
		            controller: controllerName,
		            size: 'lg',
		            resolve: {
		                endereco: function() {                
							return {
								Index:			scope.enderecos.indexOf(endereco),
								Nome:			attrs.nome,
								Pais:			endereco.Pais,
								CEP:			endereco.CEP,
								Logradouro:		endereco.Logradouro,
								Tipo:			attrs.tipo,
								Numero:			endereco.Numero,
								Complemento:	endereco.Complemento,
								Geolocalizacao:	endereco.Geolocalizacao,
								ZipCode:		endereco.ZipCode,
								Estado:			endereco.Estado,
								Cidade:			endereco.Cidade,
								Street:			endereco.Street
							};                    
		                }
		            }
		        });
        	}
        	
        	scope.editar = function(endereco) {
        		var modalInstance = null;
        		
        		if (!endereco) {
        			endereco = {};
        			modalInstance = openModal($uibModal, endereco, 'novoEndereco.html', 'NovoEnderecoController');
        		} else if (endereco.Pais.CodigoISO3 === 'BRA') {
        			modalInstance = openModal($uibModal, endereco, 'editarEnderecoNacional.html', 'EditarEnderecoNacionalController');
				} else {
					modalInstance = openModal($uibModal, endereco, 'editarEnderecoInternacional.html', 'EditarEnderecoInternacionalController');
				}
		        
		        modalInstance.result.then(function (novoEndereco) {
		        	endereco.Pais			= novoEndereco.Pais;
					endereco.CEP			= novoEndereco.CEP;
					endereco.Logradouro		= novoEndereco.Logradouro;
					endereco.Tipo			= novoEndereco.Tipo;
					endereco.Numero			= novoEndereco.Numero;
					endereco.Complemento	= novoEndereco.Complemento;
					endereco.Geolocalizacao	= novoEndereco.Geolocalizacao;
					endereco.ZipCode		= novoEndereco.ZipCode;
					endereco.Estado			= novoEndereco.Estado;
					endereco.Cidade			= novoEndereco.Cidade;
					endereco.Street			= novoEndereco.Street;
					
		        	if (novoEndereco.Index == -1) {
				    	scope.enderecos.push(endereco);
			    	} else {
			    		scope.enderecos[novoEndereco.Index] = endereco;
			    	}
			    });
		    };
	    }
	}
})
.factory('enderecoService', function() {
	var service = {};
	service.salvar = function($scope, $uibModalInstance) {
		$scope.editEndereco.$setSubmitted();
		if ($scope.editEndereco.$valid) {
			$uibModalInstance.close($scope.endereco);
		}
	};
	service.cancelar = function ($uibModalInstance) {
        $uibModalInstance.dismiss('cancel');
    };
    
    return service;
})
.controller('NovoEnderecoController', function($scope, $http, $uibModalInstance, enderecoService, utilsService, endereco) {
	$scope.endereco = endereco;
	$scope.nacional = true;
	
	$scope.salvar = function () {
		if ($scope.nacional) {
			$scope.editEnderecoNacional.$setSubmitted();
			if ($scope.editEnderecoNacional.$valid) {
				$scope.endereco.Pais = { CodigoISO3: 'BRA' };
				$uibModalInstance.close($scope.endereco);
			}
		} else {
			$scope.editEnderecoInternacional.$setSubmitted();
			if ($scope.editEnderecoInternacional.$valid) {
				$scope.endereco.Logradouro = undefined;
				$uibModalInstance.close($scope.endereco);
			}
		}
	};
    $scope.cancelar = function() { enderecoService.cancelar($uibModalInstance) };
})
.controller('EditarEnderecoNacionalController', function($scope, $http, $uibModalInstance, enderecoService, utilsService, endereco) {
	$scope.endereco = endereco;
	$scope.salvar = function () { enderecoService.salvar($scope, $uibModalInstance); };
    $scope.cancelar = function() { enderecoService.cancelar($uibModalInstance) };
})
.controller('EditarEnderecoInternacionalController', function($scope, $http, $uibModalInstance, enderecoService, utilsService, endereco) {
	$scope.endereco = endereco;
	$scope.salvar = function () { enderecoService.salvar($scope, $uibModalInstance); };
    $scope.cancelar = function() { enderecoService.cancelar($uibModalInstance) };
})

.directive('enderecoNacional', function($http, utilsService) {
	return {
	    restrict: 'AE',
	    scope: {
	        endereco: '=model'
	    },
	    templateUrl: 'Modules/Utils/views/enderecoNacional-form.html',
	    link: function (scope, elem, attrs) {
	    	function loadTiposLogradouros(pagina) {
	    		pagina = pagina ? pagina : 1;
		    	scope.dataLoading = true;
		    	
		    	$http.get(utilsService.baseAddress + '/api/v2/tiposlogradouros/?porPagina=100&pagina=' + pagina, utilsService.requestConfig)
		    	.then(function(response) {
		    		if (scope.TiposLogradouros == undefined) scope.TiposLogradouros = [];
		            for (var index in response.data) {
		            	scope.TiposLogradouros.push(response.data[index]);
		            }
		    		
		    		if (response.headers("X-Pagination-Total-Pages") != response.headers("X-Pagination-Page-Number")) {
						loadTiposLogradouros(++pagina);
					}
					else
					{
						selecionarTipoLogradouro();
						scope.dataLoading = false;
					}
		    	});
		    }
		    
		    function selecionarTipoLogradouro() {
		    	if (!scope.endereco || !scope.endereco.Logradouro || !scope.endereco.Logradouro.Tipo) return;
		    
		    	for (var i = 0; i < scope.TiposLogradouros.length; i++) {
		    		if (scope.TiposLogradouros[i].Codigo == scope.endereco.Logradouro.Tipo.Codigo) {
		    			scope.endereco.Logradouro.Tipo = scope.TiposLogradouros[i];
		    			break;
		    		}
		    	}
		    }
		    
		    function selecionarUF() {
		    	if (scope.endereco.Logradouro == undefined || scope.endereco.Logradouro.UF == undefined) return;
		    
		    	for (var i = 0; i < scope.UnidadesFederativas.length; i++) {
		    		if (scope.UnidadesFederativas[i].Sigla == scope.endereco.Logradouro.UF.Sigla) {
		    			scope.endereco.Logradouro.UF = scope.UnidadesFederativas[i];
		    			break;
		    		}
		    	}
		    }
		    
		    function Init() {
		    	scope.UnidadesFederativas = utilsService.UnidadesFederativas;
			    loadTiposLogradouros();
			    selecionarUF();
			    
			    scope.disableLogradouro = true;
			    scope.disableBairro = true;
		    }
		    
		    scope.buscarCEP = function() {
		    	if (!scope.endereco.CEP || scope.endereco.CEP.length == 0) {
		    		scope.endereco.Logradouro = undefined;
		    		scope.disableBairro = true;
		    		scope.disableLogradouro = true;
		    		return;
		    	}
		    
		    	scope.dataLoading = true;
		    	$http.get(utilsService.baseAddress + '/api/v2/logradouros/?cep=' + scope.endereco.CEP, utilsService.requestConfig)
		    	.then(function(response) {
		    		var logradouro = response.data[0];
		    		scope.endereco.Logradouro = logradouro;
		    		selecionarTipoLogradouro();
		    		scope.disableBairro = (logradouro.BairroInicial != undefined);
		    		scope.disableLogradouro = (logradouro.Tipo != undefined);
		    		scope.dataLoading = false;
		    	});
		    }
		    
		    Init();
	    }
    }
})
.directive('enderecoInternacional', function($http, utilsService) {
	return {
	    restrict: 'AE',
	    scope: {
	        endereco: '=model'
	    },
	    templateUrl: 'Modules/Utils/views/enderecoInternacional-form.html',
	    link: function (scope, elem, attrs) {
	    	scope.dataLoading = false;
	    	
	    	function selecionarPais() {
				for (var i = 0; i < scope.Paises.length; i++) {
					if (scope.Paises[i].CodigoISO3 === 'BRA') {
						scope.Paises.splice(i, 1);
						continue;
					}
				
					if (scope.endereco && scope.endereco.Pais && scope.endereco.Pais.CodigoISO3 &&
						scope.Paises[i].CodigoISO3 == scope.endereco.Pais.CodigoISO3) {
							scope.endereco.Pais = scope.Paises[i];
					}
				}
			}
	    	
	    	utilsService.LoadPaises(scope, $http, 1, selecionarPais);
	    }
    }
});