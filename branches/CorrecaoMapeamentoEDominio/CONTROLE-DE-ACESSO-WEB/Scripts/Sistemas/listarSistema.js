jQuery(function() {
    sistemasListarView.inicializar();
});

var sistemasListarView = function () {
    /*************************************************************************
    * Botão para listar contratos da tela de busca por contratos.
    *************************************************************************/

    var inicializar = function () {
        jQuery("#btn_filtrar").click(function () {

            var count = jQuery("#CodSistema").val().length;

            if (count < 3) {
                var mensagens = { caracteres: "O campo deve conter mais de três caracteres.", falha: "Não foi possível processar a requisição." };
               
                jQuery("#mensagem-erro").show().html(mensagens.caracteres);
                jQuery("#grdSistemas").hide();
            }
            else {
                var parametros = {
                    Id: jQuery("#Id").val(),
                    CodSistema: jQuery("#CodSistema").val(),
                    DescSistema: jQuery("#DescSistema").val()
                };

                //var parametros = { CodSistema: jQuery("#CodSistema").val()};
                jQuery("#mensagem-erro").hide();
                system.ajax.callJsonList("/Sistemas/ListarSistemasPorCodigo/", "GET", parametros, "#grdSistemas", "#ajax-load", "#display-error");
            }
        });
    };
    return { inicializar: inicializar };
}();