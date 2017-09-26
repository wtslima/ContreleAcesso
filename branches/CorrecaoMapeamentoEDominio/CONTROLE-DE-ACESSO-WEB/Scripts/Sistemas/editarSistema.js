jQuery(function () {
    editar.editarSistema.inicializar();

    jQuery("#tabelaSistemas tbody tr").each(function () {

        var tabela = jQuery(this);
        var id = tabela.find('input').first().val();
        var idId = "#Id_" + id;
        var idCodigoSistema = "#codSistema_" + id;
        var IdDescSistema = "#descSistema_" + id;
        var idBtnEditar = "#btn_editar_" + id;
        var idBtnExcluir = "#btn_excluir_" + id;

        var botaoEditar = tabela.children('td').eq(0).children(idBtnEditar);
        var botaoExcluir = tabela.children('td').eq(0).children(idBtnExcluir);

        //evento editar...
        botaoEditar.click(function () {
            var valId = jQuery(idId).attr('value');
            var valCodSistema = jQuery(idCodigoSistema).attr('value');
            var valDescSistema = jQuery(IdDescSistema).attr('value');

            var parametros = {
                Id: valId,
                CodSistema: valCodSistema,
                DescSistema: valDescSistema
            };
            editar.renderizaModalEditarSistemaP1(parametros);

        });

        //evento excluir...
        botaoExcluir.click(function () {
            alert(jQuery(this).attr('id'));
        });
    });
});

var editar = function () {

    /*************************************************************************
    * Renderizar modal de sistema parte 3
    *************************************************************************/
    var renderizaModalEditarSistemaP1 = function (parametros) {

        jQuery('#modal-editar-sistema-p1').modal('show');
        jQuery("#modal-editar-sistema-p1 .modal-title").html("Editar Sistemas");

        jQuery("#modal-editar-sistema-p1 #Id").val(parametros.Id);
        jQuery("#modal-editar-sistema-p1 #ModalEdtCodSistema").val(parametros.CodSistema);
        jQuery("#modal-editar-sistema-p1 #ModalEdtDescSistema").val(parametros.DescSistema);
        jQuery("#modal-editar-sistema-p1 #display-load-editar").hide();
        jQuery("#modal-editar-sistema-p1 #display-error-editar").hide();
        jQuery("#modal-editar-sistema-p1 #display-sucesso-editar").hide();
        jQuery("#modal-editar-sistema-p1 #formulario").show();
        jQuery("#modal-editar-sistema-p1 #btnEdtModalDesistir").show();
        jQuery("#modal-editar-sistema-p1 #btnModalEditar").show();
        jQuery('#modal-editar-sistema-p1 #btnEdtModalDesistir').removeAttr('disabled');
        jQuery('#modal-editar-sistema-p1 #btnModalEditar').removeAttr('disabled');
    };

    /*************************************************************************
    * Editar sistema
    *************************************************************************/
    var editarSistema = function () {

        var inicializar = function () {
            jQuery("#btnModalEditar").click(function () {
                executar();
            });
        };

        var mensagens = { naoprocessado: "Não foi possível editar o sistema.", falha: "Não foi possível processar a requisição." };

        var executar = function () {

            var parametros = {
                Id: jQuery("#modal-editar-sistema-p1 #Id").val(),
                CodSistema: jQuery("#modal-editar-sistema-p1 #ModalEdtCodSistema").val(),
                DescSistema: jQuery("#modal-editar-sistema-p1 #ModalEdtDescSistema").val()
            };

            var functionBeforeSend = function (mensagemErro) {
                console.log("processando...");
                jQuery("#modal-editar-sistema-p1 #display-load-editar").show();
                jQuery("#modal-editar-sistema-p1 #display-error-editar").hide();
                jQuery("#modal-editar-sistema-p1 #display-sucesso-editar").hide();
                jQuery("#modal-editar-sistema-p1 #formulario").hide();
                jQuery("#modal-editar-sistema-p1 #btnEdtModalDesistir").show();
                jQuery("#modal-editar-sistema-p1 #btnModalEditar").show();
                jQuery('#modal-editar-sistema-p1 #btnEdtModalDesistir').attr('disabled', 'disabled');
                jQuery('#modal-editar-sistema-p1 #btnModalEditar').attr('disabled', 'disabled');

            };
            var functionError = function (XMLHttpRequest, textStatus, errorThrown, mensagemErro) {
                console.log(mensagemErro);
                //implementacao error
                jQuery("#modal-editar-sistema-p1 #display-load-editar").hide();
                jQuery("#modal-editar-sistema-p1 #display-error-editar").show();
                jQuery("#modal-editar-sistema-p1 #display-sucesso-editar").hide();
                jQuery("#modal-editar-sistema-p1 #formulario").show();
                jQuery("#modal-editar-sistema-p1 #btnEdtModalDesistir").show();
                jQuery("#modal-editar-sistema-p1 #btnModalEditar").show();
                jQuery('#modal-editar-sistema-p1 #btnEdtModalDesistir').removeAttr('disabled');
                jQuery('#modal-editar-sistema-p1 #btnModalEditar').removeAttr('disabled');

                jQuery("#modal-editar-sistema-p1 #display-error-editar  > div > div > p").html(mensagemErro);
            };
            var functionSuccessJson = function (data) {
                console.log(data);
                if (typeof data.Mensage === "undefined") {

                    //implementacao sucesso
                    jQuery('#modal-editar-sistema-p1').modal('hide');
                    jQuery("#display-load-editar").hide();
                    jQuery("#display-error-editar").hide();

                    jQuery('#modal-editar-sistema-p2').modal('show');
                    ////jQuery("#modal-cadastro-sistema-p3 #Id").val(data.sistema.Id);
                    //jQuery("#modal-cadastro-sistema-p3 #ModalCodSistema").val(data.sistema.ModalCodSistema);
                    ////jQuery("#modal-cadastro-sistema-p3 #ModalDescSistema").val(data.sistema.ModalDescSistema);
                    //var mensagemHtml = " Sistema cadastrado: <br /> <strong>" + data.sistema.ModalCodSistema + "</strong>";
                    //jQuery("#modal-cadastro-sistema-p3 #display-sucesso > div > div.texto").show().html(mensagemHtml);



                } else {
                    //implementacao exception
                    jQuery("#modal-editar-sistema-p1 #display-load-editar").hide();
                    jQuery("#modal-editar-sistema-p1 #display-error-editar").show();
                    jQuery("#modal-editar-sistema-p1 #display-sucesso-editar").hide();
                    jQuery("#modal-editar-sistema-p1 #formulario").show();
                    jQuery("#modal-editar-sistema-p1 #btnEdtModalDesistir").show();
                    jQuery("#modal-editar-sistema-p1 #btnModalEditar").show();
                    jQuery('#modal-editar-sistema-p1 #btnEdtModalDesistir').removeAttr('disabled');
                    jQuery('#modal-editar-sistema-p1 #btnModalEditar').removeAttr('disabled');

                    //jQuery("#display-sistema").hide();
                    //console.log('Else');

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

                    jQuery("#modal-editar-sistema-p1 #display-error-editar  > div > div > p").html(msgerros);
                }
            };

            //url, tipo, parametros, functionBeforeSend, functionError, functionSuccessJson, mensagemErro
            if (parametros.value != "undefined") {
                system.ajax.callJson("/Sistemas/EditarSistemas/", "POST", parametros, functionBeforeSend, functionError, functionSuccessJson, mensagens.falha);

                //var codSistema = "sis"//jQuery("#modal-editar-sistema-p1 #pesqInicial").attr('value');
                //console.log(codSistema);
                //alert(codSistema);
                //var param = {
                //    Id: 0,
                //    CodSistema: codSistema,
                //    DescSistema: ''
                //};

                //system.ajax.callJsonList("/Sistemas/ListarSistemasPorCodigo/", "GET", param, "#grdSistemas", "#ajax-load", "#display-error");
                //alert("Valor da pesquisa =" + param.CodSistema);
                //console.log(param.CodSistema);
            }
        };
        return{
            inicializar: inicializar
        }
    }();
    return{
        renderizaModalEditarSistemaP1: renderizaModalEditarSistemaP1,
        editarSistema: editarSistema
    }
}();