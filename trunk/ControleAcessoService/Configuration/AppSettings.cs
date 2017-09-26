using System.Configuration;
using System.Diagnostics.Contracts;

namespace ControleAcessoService.Configuration {

    public static class AppSettings {

        public static string CredencialAcessoAnonimo {
            get {
                return GetAppSettingsValue("CredencialAcessoAnonimo");
            }
        }

        public static string EmailAdministrador {
            get {
                return GetAppSettingsValue("EmailAdministrador");
            }
        }

        public static string log4net_Internal_Debug {
            get {
                return GetAppSettingsValue("log4net.Internal.Debug");
            }
        }

        public static int PrazoExpiracaoSenhaTemporaria {
            get {

                int prazoExpiracaoSenhaTemporaria;
                if(!int.TryParse(GetAppSettingsValue("PrazoExpiracaoSenhaTemporaria"), out prazoExpiracaoSenhaTemporaria)){
                    return 7;
                }

                return prazoExpiracaoSenhaTemporaria;
            }
        }

        private static string GetAppSettingsValue(string key) {
            Contract.Ensures(Contract.Result<string>() != null);

            var value = ConfigurationManager.AppSettings[key];
            if (value == null)
                throw new ConfigurationErrorsException(string.Format("A chave ({0}) não está presente na seção AppSettings do arquivo de configuração.", key));
            return value.ToString();
        }

    }
}
