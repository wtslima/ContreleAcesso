using ControleAcesso.Dominio.ObjetosDeValor;

namespace ControleAcesso.Dominio.Infra.Mapeamentos
{
	public class ServidorOrigemMapa : ControleMapa<ServidorOrigem>
	{
		public ServidorOrigemMapa()
		{
            
			Table("TS_PARAMETRO");
			DynamicInsert();
			DynamicUpdate();
			CompositeId()
				.KeyProperty(x => x.CodigoSistema, "IDT_SISTEMA")
                .KeyProperty(x => x.NomeParametro, "NOM_PARAMETRO");
			Map(x => x.Servidor, "CDA_VALOR_PARAMETRO_A").Length(100).Not.Nullable();
			Map(x => x.Token, "CDA_VALOR_PARAMETRO_B").Length(100);
            Where("CDA_VALOR_PARAMETRO_A like 'IpServidorOrigem%'");
		}
	}
}
