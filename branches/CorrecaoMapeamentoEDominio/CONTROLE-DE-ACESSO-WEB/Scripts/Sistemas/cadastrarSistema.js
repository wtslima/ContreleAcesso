jQuery(function () {
    cadastrar.cadastroDesistema.inicializar();
    
    jQuery("#btn_novo").click(function () {
        cadastrar.renderizaModalCadastroSistemaP1("");
        jQuery("#alert").hide();

    });

});

var cadastrar = function () {

    /*************************************************************************
    * Renderizar modal de sistema parte 1
    *************************************************************************/
    var renderizaModalCadastroSistemaP1 = function () {


        jQuery('#modal-cadastro-sistema-p1').modal('show');
        jQuery("#modal-cadastro-sistema-p1 .modal-title").html("Cadastro de Sistemas");

        jQuery("#modal-cadastro-sistema-p1 #Id").val("");
        jQuery("#modal-cadastro-sistema-p1 #ModalCodSistema").val("");
        jQuery("#modal-cadastro-sistema-p1 #ModalDescSistema").val("");
        jQuery("#modal-cadastro-sistema-p1 #display-load-sistema").hide();
        jQuery("#modal-cadastro-sistema-p1 #display-error-sistema").hide();
        jQuery("#modal-cadastro-sistema-p1 #display-sucesso").hide();
        jQuery("#modal-cadastro-sistema-p1 #formulario").show();
        jQuery("#modal-cadastro-sistema-p1 #btnModalDesistir").show();
        jQuery("#modal-cadastro-sistema-p1 #btnModalCadastrar").show();
        jQuery('#modal-cadastro-sistema-p1 #btnModalDesistir').removeAttr('disabled');
        jQuery('#modal-cadastro-sistema-p1 #btnModalCadastrar').removeAttr('disabled');
    };

    /*************************************************************************
    * Renderizar modal de sistema parte 2
    *************************************************************************/
    var renderizaModalCadastroSistemaP2 = function () {
        jQuery('#modal-cadastro-sistema-p2').modal('show');
        jQuery("#modal-cadastro-sistema-p2 .modal-title").html("Confirmação de Cadastro de Sistemas");

        jQuery("#modal-cadastro-sistema-p2 #display-sucesso").show();
        jQuery("#modal-cadastro-sistema-p2 #btnModalCancelar").show();
        jQuery("#modal-cadastro-sistema-p2 #btnConfirmarCadastroSistema").show();
        jQuery('#modal-cadastro-sistema-p2 #btnModalCancelar').removeAttr('disabled');
        jQuery('#modal-cadastro-sistema-p2 #btnConfirmarCadastroSistema').removeAttr('disabled');
    };

    /*************************************************************************
    * Cadastrar sistema
    *************************************************************************/
    var cadastroDesistema = function () {

        var inicializar = function () {
            jQuery("#modal-cadastro-sistema-p1 #btnModalCadastrar").click(function () {
                executar();
            });
        };

        var mensagens = { naoprocessado: "Não foi possível cadastrar a sistema.", falha: "Não foi possível processar a requisição." };

        var executar = function () {

            var parametros = {
                Id: 0,
                CodSistema: jQuery("#modal-cadastro-sistema-p1 #ModalCodSistema").val(),
                DescSistema: jQuery("#modal-cadastro-sistema-p1 #ModalDescSistema").val()
            };

            var functionBeforeSend = function (mensagemErro) {
                console.log("processando...");
                jQuery("#modal-cadastro-sistema-p1 #display-load").show();
                jQuery("#modal-cadastro-sistema-p1 #display-error").hide();
                jQuery("#modal-cadastro-sistema-p1 #display-sucesso").hide();
                jQuery("#modal-cadastro-sistema-p1 #formulario").hide();
                jQuery("#modal-cadastro-sistema-p1 #btnModalDesistir").show();
                jQuery("#modal-cadastro-sistema-p1 #btnModalCadastrar").show();
                jQuery('#modal-cadastro-sistema-p1 #btnModalDesistir').attr('disabled', 'disabled');
                jQuery('#modal-cadastro-sistema-p1 #btnModalCadastrar').attr('disabled', 'disabled');
            };
            var functionError = function (XMLHttpRequest, textStatus, errorThrown, mensagemErro) {
                console.log(mensagemErro);
                //implementacao error
                jQuery("#modal-cadastro-sistema-p1 #display-load").hide();
                jQuery("#modal-cadastro-sistema-p1 #display-error").show();
                jQuery("#modal-cadastro-sistema-p1 #display-sucesso").hide();
                jQuery("#modal-cadastro-sistema-p1 #formulario").show();
                jQuery("#modal-cadastro-sistema-p1 #btnModalDesistir").show();
                jQuery("#modal-cadastro-sistema-p1 #btnModalCadastrar").show();
                jQuery('#modal-cadastro-sistema-p1 #btnModalDesistir').removeAttr('disabled');
                jQuery('#modal-cadastro-sistema-p1 #btnModalCadastrar').removeAttr('disabled');
            };
            var functionSuccessJson = function (data) {
                console.log(data);
                if (typeof data.Mensage === "undefined") {

                    //implementacao sucesso
                    jQuery('#modal-cadastro-sistema-p1').modal('hide');
                    jQuery("#display-load-sistema").hide();
                    jQuery("#display-error-sistema").hide();

                    jQuery('#modal-cadastro-sistema-p3').modal('show');
                    ////jQuery("#modal-cadastro-sistema-p3 #Id").val(data.sistema.Id);
                    //jQuery("#modal-cadastro-sistema-p3 #ModalCodSistema").val(data.sistema.ModalCodSistema);
                    ////jQuery("#modal-cadastro-sistema-p3 #ModalDescSistema").val(data.sistema.ModalDescSistema);
                    //var mensagemHtml = " Sistema cadastrado: <br /> <strong>" + data.sistema.ModalCodSistema + "</strong>";
                    //jQuery("#modal-cadastro-sistema-p3 #display-sucesso > div > div.texto").show().html(mensagemHtml);
                } else {
                    //implementacao exception
                    jQuery("#modal-cadastro-sistema-p3 #display-load").hide();
                    jQuery("#modal-cadastro-sistema-p3 #display-error").show();
                    jQuery("#modal-cadastro-sistema-p3 #display-sucesso").hide();
                    jQuery("#modal-cadastro-sistema-p3 #formulario").show();
                    jQuery("#modal-cadastro-sistema-p3 #btnModalDesistir").show();
                    jQuery("#modal-cadastro-sistema-p3 #btnModalCadastrar").show();
                    jQuery('#modal-cadastro-sistema-p3 #btnModalDesistir').removeAttr('disabled');
                    jQuery('#modal-cadastro-sistema-p3 #btnModalCadastrar').removeAttr('disabled');

                    jQuery("#display-sistema").hide();
                    //jQuery("#ContratosistemaId").val("");
                    //system.mascaras.cnpjComValor("#CnpjsistemaContratada", "");
                    var mensagemHtml = "";
                    jQuery("#display-sistema > div > div > p").html(mensagemHtml);

                    var msgerros = "";
                    if (data.Itens.length > 0) {
                        msgerros = "<ul>";
                        for (var i = 0; i < data.Itens.length; i++) {
                            msgerros = msgerros + "<li>" + data.Itens[i] + "</li>";
                        }
                        msgerros = msgerros + "</ul>";
                    } else {
                        msgerros = data.Mensage;
                    }

                    jQuery("#modal-cadastro-sistema-p3 #display-error  > div > div > p").html(msgerros);
                }
            };

            //url, tipo, parametros, functionBeforeSend, functionError, functionSuccessJson, mensagemErro
            if (parametros.value != "undefined") {
                system.ajax.callJson("/Sistemas/CadastrarSistemas/", "POST", parametros, functionBeforeSend, functionError, functionSuccessJson, mensagens.falha);
            }
        };
        return {
            inicializar: inicializar
        }
    }();
    return {
        renderizaModalCadastroSistemaP1: renderizaModalCadastroSistemaP1,
        renderizaModalCadastroSistemaP2: renderizaModalCadastroSistemaP2,
        cadastroDesistema: cadastroDesistema
    }
}();