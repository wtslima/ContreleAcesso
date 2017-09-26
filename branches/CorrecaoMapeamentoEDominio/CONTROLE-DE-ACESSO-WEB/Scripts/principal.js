//----------------------------------------------------------------------
// Arquivo de scripts javascript usados pela aplicação - v 1.0
//
//
//
//----------------------------------------------------------------------
jQuery(document).ajaxError(function (xhr, props) {
    if (props.status === 401) {
        location.reload();
    }
});

var system = function () {

    /**
    * Funcao para renderizar console.log() para debug
    * ==========================================
    * Nao utilize console.log
    */
    var debug = function (dado) {
        if (true)
            console.log(dado);
    };

    /**
    * Funcao para chamadas assincronas utilizando ajax
    * ==========================================
    * implemente os metodos de render e passe como argumentos das funcoes call
    * 
    * chamadas:
    * - call: chamada principal para retornar qualquer tipo de informacao, html, json, xml, text/plain
    * - callJson: retorna chamada passando um objeto json
    * - callJsonStringfy: retorna chamada passando um objeto que foi rebaixo a string como json
    */
    var ajax = function () {

        /*
        Função chamada pelo ajax ao obter sucesso na requisição
        */
        var renderResposta = function (data, componente, ajaxLoad, mensagemErro) {
            jQuery(componente).fadeIn().html(data);
            jQuery(ajaxLoad).hide();
            jQuery(mensagemErro).hide();
        }
        /*
        Função chamada pelo ajax antes de enviar a requisição
        */
        var renderCarregando = function (componente, ajaxLoad, mensagemErro) {
            jQuery(componente).hide();
            jQuery(mensagemErro).hide();
            jQuery(ajaxLoad).show();
        }
        /*
        Função chamada pelo ajax ao cmopletar a requisição
        */
        var renderComplete = function (componente, ajaxLoad) {
            jQuery(componente).show();
            jQuery(ajaxLoad).hide();
        }
        /*
        Função chamada pelo ajax quando ocorrer um erro na requisição
        */
        var renderErro = function (xmlHttpRequest, textStatus, errorThrown, componente, ajaxLoad, mensagemErro) {
            jQuery(ajaxLoad).hide();
            jQuery(componente).hide();
            jQuery(mensagemErro).show().html(textStatus + " / " + errorThrown + " : " + xmlHttpRequest);
        }

        /**
        =========================================================================
            Descrição          : Função chamada pelos scripts de views que fazem requisições ajax.
            Param URL          : Rota da view.
            Param tipo         : Tipo do post.
            Param parametros   : Parametros de filtro enviados para a view.
            Param componente   : Componente a ser atualizados.
            Param ajaxLoad     : Componente que será exibido enquanto a requisição ajax é executada.
            Param mensagemErro : Componente tela que exibirá mensagem de erro, caso ocorra algum problema na requisição ajax.
            Retorno            : void execucao da chamada assincrona inserindo o objeto no HTML
        =========================================================================
        **/
        var call = function (url, tipo, parametros, componente, ajaxLoad, mensagemErro) {
            jQuery.ajax({
                url: url,
                type: tipo,
                data: parametros,
                success: function (data) {
                    renderResposta(data, componente, ajaxLoad, mensagemErro);
                },
                beforeSend: function () {
                    renderCarregando(componente, ajaxLoad, mensagemErro);
                },
                complete: function () {
                    renderComplete(componente, ajaxLoad);
                },
                error: function (xmlHttpRequest, textStatus, errorThrown) {
                    renderErro(xmlHttpRequest, textStatus, errorThrown, componente, ajaxLoad, mensagemErro);
                }
            });
        };

        var callJson = function (url, tipo, parametros, functionBeforeSend, functionError, functionSuccessJson, mensagemErro) {
            jQuery.ajax({
                url: url,
                type: tipo,
                data: parametros,
                success: function (data) {
                    functionSuccessJson(data);
                },
                beforeSend: function () {
                    functionBeforeSend(mensagemErro);
                },
                error: function (xmlHttpRequest, textStatus, errorThrown) {
                    functionError(xmlHttpRequest, textStatus, errorThrown, mensagemErro);
                }
            });
        };

        var callJsonList = function (url, tipo, parametros, componente, ajaxLoad, mensagemErro) {
           
            jQuery.ajax({
                url: url,
                type: tipo,
                data: parametros,
                dataType: "html",
                traditional: true,
                success: function (data) {
                    renderResposta(data, componente, ajaxLoad, mensagemErro);
                },
                beforeSend: function () {
                    renderCarregando(componente, ajaxLoad, mensagemErro);
                },
                complete: function () {
                    renderComplete(componente, ajaxLoad);
                },
                error: function (xmlHttpRequest, textStatus, errorThrown) {
                    renderErro(xmlHttpRequest, textStatus, errorThrown, componente, ajaxLoad, mensagemErro);
                }
            });
        };

        var callJsonListNew = function (url, tipo, parametros, functionBeforeSend, functionError, functionSuccessJson, mensagemErro) {
            jQuery.ajax({
                url: url,
                type: tipo,
                data: parametros,
                dataType: "html",
                traditional: true,
                success: function (data) {
                    functionSuccessJson(data);
                },
                beforeSend: function () {
                    functionBeforeSend(mensagemErro);
                },
                error: function (xmlHttpRequest, textStatus, errorThrown) {
                    functionError(xmlHttpRequest, textStatus, errorThrown, mensagemErro);
                }
            });
        };

        var callJsonStringfy = function (url, tipo, parametros, functionBeforeSend, functionError, functionSuccessJson, mensagemErro) {
            jQuery.ajax({
                url: url,
                type: tipo,
                data: parametros,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    functionSuccessJson(data);
                },
                beforeSend: function () {
                    functionBeforeSend(mensagemErro);
                },
                error: function (xmlHttpRequest, textStatus, errorThrown) {
                    functionError(xmlHttpRequest, textStatus, errorThrown, mensagemErro);
                }
            });
        };

        var callFileUpload = function (url, tipo, parametros, functionBeforeSend, functionError, functionSuccessJson, mensagemErro) {
            jQuery.ajax({
                type: tipo,
                url: url,
                data: parametros,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (data) {
                    functionSuccessJson(data);
                },
                beforeSend: function () {
                    functionBeforeSend(mensagemErro);
                },
                error: function (xmlHttpRequest, textStatus, errorThrown) {
                    functionError(xmlHttpRequest, textStatus, errorThrown, mensagemErro);
                }
            });
        };


        return {
            call: call,
            callJson: callJson,
            callJsonStringfy: callJsonStringfy,
            callFileUpload: callFileUpload,
            callJsonList: callJsonList,
            callJsonListNew: callJsonListNew
        };
    }();


    /**
    * Funcao de adicionar componentes HTML nas views
    * ==========================================
    * Crie componentes dinamicos, combos, inputs, listas, tabelas etc.
    */
    var componente = function () {

        var combobox = function (name, lista) {
            var html = "<select id='" + name + "' name='" + name + "' class='form-control'>";
            html = html + "<option value = '0' >---</option>";
            for (var i = 0; i < lista.length; i++) {
                html = html + "<option value = '" + lista[i].Value + "' >" + lista[i].Name + "</option>";
            }
            html = html + "</select>";
            return html;
        };

        var comboboxErro = function (msg) {
            if (msg == null || msg == '')
                return '<span class="label label-danger">Não foi possível carregar as informações.</span>';
            else
                return '<span class="label label-danger">' + msg + '</span>';
        };

        var comboboxProcessando = function () {
            return '<span class="label label-warning"><span class="glyphicon glyphicon-refresh" aria-hidden="true"></span> procesando...</span>';
        };

        var multiSelect = function (item) {
            return jQuery(item).multiselect({
                buttonText: function (options, select) {
                    if (options.length === 0) {
                        return 'Selecione...';
                    }
                    else if (options.length > 3) {
                        return options.length + ' opções selecionadas!';
                    }
                    else {
                        var labels = [];
                        options.each(function () {
                            if (jQuery(this).attr('label') !== undefined) {
                                labels.push(jQuery(this).attr('label'));
                            }
                            else {
                                labels.push(jQuery(this).html());
                            }
                        });
                        return labels.join(', ') + '';
                    }
                }
            });
        };

        var multiSelectTransformar = function (item, functionOnChange) {
            return jQuery(item).multiselect({
                buttonText: function (options, select) {
                    if (options.length === 0) {
                        return 'Selecione...';
                    }
                    else if (options.length > 3) {
                        return options.length + ' opções selecionadas!';
                    }
                    else {
                        var labels = [];
                        options.each(function () {
                            if (jQuery(this).attr('label') !== undefined) {
                                labels.push(jQuery(this).attr('label'));
                            }
                            else {
                                labels.push(jQuery(this).html());
                            }
                        });
                        return labels.join(', ') + '';
                    }
                },
                onChange: function () {
                    functionOnChange();
                },
            });
        };

        var multiSelectCarregar = function (name, lista) {
            var html = "<select id='" + name + "' name='" + name + "' class='multiselect' multiple='multiple'>";
            for (var i = 0; i < lista.length; i++) {
                html = html + "<option value = '" + lista[i].Value + "' >" + lista[i].Name + "</option>";
            }
            html = html + "</select>";
            return html;
        };

        return {
            combobox: combobox,
            comboboxErro: comboboxErro,
            comboboxProcessando: comboboxProcessando,
            multiSelect: multiSelect,
            multiSelectCarregar: multiSelectCarregar,
            multiSelectTransformar: multiSelectTransformar
        }
    }();


    /**
     * Funcao de adicionar mascaras aos campos de formulario
     * ==========================================
     * Precisa utilizar o maskinput ao projeto.
     * 
     * Biblioteca: jquery.maskinput.js (nuget)
     * 
     * referencia: http://igorescobar.github.io/jQuery-Mask-Plugin/
     */
    var mascaras = function () {

        /**
         * Executa mascara padrao
         * 
         * @param {id do campo} id 
         * @param {padrao a ser aplicado na mascara} padrao 
         * @returns {}
         * 
         */
        var aplicar = function (id, padrao) {
            return jQuery(id).mask(padrao);
        }

        var aplicarComValor = function (id, padrao, valor) {
            return jQuery(id).val(valor).mask(padrao);
        }

        var aplicarLabelComValor = function (id, padrao, valor) {
            return jQuery(id).text(valor).mask(padrao);
        }



        var limparCnpj = function (valor) {
            var resultado = valor.replace(".", "");
            resultado = resultado.replace(".", "");
            resultado = resultado.replace("/", "");
            resultado = resultado.replace("-", "");
            return resultado;
        }

        var limparCpf = function (valor) {
            var resultado = valor.replace(".", "");
            resultado = resultado.replace(".", "");
            resultado = resultado.replace("-", "");
            return resultado;
        }

        var limpar = function (valor) {
            var resultado = valor.replace(/[\-\_\.\/\s-]+/g, '');
            return resultado;
        }

        var cpf = function (id) {
            return aplicar(id, '?999.999.999-99');
        }

        var cnpj = function (id) {
            return aplicar(id, '?99.999.999/9999-999');
        }
        var cnpjComValor = function (id, valor) {
            return aplicarComValor(id, '?99.999.999/9999-999', valor);
        }
        var cpfComValor = function (id, valor) {
            return aplicarComValor(id, '?999.999.999-99', valor);
        }
        var data = function (id) {
            return aplicar(id, '?99/99/9999');
        }

        var meseano = function (id) {
            return aplicar(id, '?99/9999');
        }

        var numeroContrato = function (id) {
            return aplicar(id, '?999/9999');
        }
        var numeroProcessoInmetro = function (id) {
            return aplicar(id, '?99999.999999/99-99');
        }

        var TelefoneLabelComValor = function (id, valor) {
            return aplicarLabelComValor(id, '?(99)9999-9999', valor);
        }



        return {
            limparCnpj: limparCnpj,
            limparCpf: limparCpf,
            cpf: cpf,
            cnpj: cnpj,
            cnpjComValor: cnpjComValor,
            data: data,
            meseano: meseano,
            limpar: limpar,
            numeroContrato: numeroContrato,
            numeroProcessoInmetro: numeroProcessoInmetro,
            cpfComValor: cpfComValor

        }
    }();


    /**
    * Funcao de adicionar calendario aos campos de data
    * ==========================================
    * Precisa utilizar o jquery UI com a biblioteca DatePicker
    * 
    * Biblioteca: jquery-ui-1.11.4.js (nuget), complemento com CSS + DatePicker
    * 
    */
    var calendario = function () {

        var padrao = function (id, meses, anos, rangeAnos) {
            jQuery(id).datepicker({
                dateFormat: 'dd/mm/yy',
                dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
                dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
                dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
                monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
                monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
                nextText: 'Próximo',
                prevText: 'Anterior',
                changeMonth: meses,
                changeYear: anos,
                yearRange: rangeAnos,
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 99999999999999);
                    }, 0);
                }
            });

        }

        var aniversario = function (id) {
            padrao(id, true, true, '1910:2080');
        }

        return {
            aniversario: aniversario
        }
    }();

    /**
   * Funcao de adicionar calendario com escholha de Mes\ANO
   * ==========================================
   * Precisa utilizar o jquery UI com a biblioteca DatePicker
   * 
   * Biblioteca: jquery-ui-1.11.4.js (nuget), complemento com CSS + DatePicker
   * 
   */
    var mesano = function () {

        var padraomesano = function (id, meses, anos, mesinicio, anoinicio, mesfim, anofim) {
            jQuery(id).datepicker({
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                buttonText: "OK",
                dateFormat: 'mm/yy',
                dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
                dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
                dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
                monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
                monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
                minDate: new Date(anoinicio, mesinicio),
                maxDate: new Date(anofim, mesfim),
                changeMonth: meses,
                changeYear: anos,
                nextText: 'Próximo',
                prevText: 'Anterior',
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 99999999999999);
                    }, 0);
                },
                onClose: function (dateText, inst) {
                    var month = jQuery("#ui-datepicker-div .ui-datepicker-month :selected").val();
                    var year = jQuery("#ui-datepicker-div .ui-datepicker-year :selected").val();
                    jQuery(this).datepicker('setDate', new Date(year, month, 1));
                },
            });

        }

        var aniversariomesano = function (id, mesinicio, anoinicio, mesfim, anofim) {
            padraomesano(id, true, true, mesinicio, anoinicio, mesfim, anofim);
        }

        return {
            aniversariomesano: aniversariomesano
        }
    }();

    /**
     * Padrao de mensagens
     * ==========================================
     * 
     */
    var mensagens = function () {

        var displayErros = function (idElemento, dataItens, dataMensage) {
            var msgerros = "";
            var itens = [];
            var itens = dataItens;
            if (itens.length > 0) {
                msgerros = "<ul>";
                for (var i = 0; i < itens.length; i++) {
                    msgerros = msgerros + "<li>" + itens[i] + "</li>";
                }
                msgerros = msgerros + "</ul>";
            } else {
                msgerros = dataMensage;
            }
            jQuery(idElemento).html(msgerros);
        };

        return {
            displayErros: displayErros
        }
    }();


    /**
     * Funcoes helper para manipular os elementos html de formulario
     * ==========================================
     * 
     */
    var html = function () {

        var preencherInputText = function (id, texto) {
            if (texto != "")
                jQuery(id).val(texto);
        };

        var preencherRadioButton = function (radios, texto) {
            if (texto != "") {
                jQuery(radios).each(function () {
                    if (texto == jQuery(this).attr('value'))
                        jQuery(this).attr('checked', true);
                });
            }
        };

        var selecionarComboBox = function (options, texto) {
            if (texto != "") {
                jQuery(options).each(function () {
                    if (texto == jQuery(this).attr('value'))
                        jQuery(this).attr('selected', true);
                });
            }
        };

        var ativarBotao = function (id) {
            jQuery(id).removeAttr('disabled');
        };

        var inativarBotao = function (id) {
            jQuery(id).attr('disabled', 'disabled');
        };

        return {
            preencherInputText: preencherInputText,
            preencherRadioButton: preencherRadioButton,
            selecionarComboBox: selecionarComboBox,
            ativarBotao: ativarBotao,
            inativarBotao: inativarBotao
        }

    }();


    /**
     * Animacao de scroll
     * ==========================================
     * 
     */
    var scroll = function () {

        var topo = function () {
            jQuery("html, body").animate({ scrollTop: jQuery(document).height() - jQuery(window).height() }, "fast");
        };

        var componente = function (idComponete) {
            //alert(jQuery(idComponete).position().top);
            var posVertical = jQuery(idComponete).position().top;
            jQuery("html, body").animate({ scrollTop: posVertical }, "fast");
        };

        return {
            topo: topo,
            componente: componente
        }

    }();

    return {
        debug: debug,
        ajax: ajax,
        componente: componente,
        mascaras: mascaras,
        calendario: calendario,
        mesano: mesano,
        mensagens: mensagens,
        html: html,
        scroll: scroll
    };


}();