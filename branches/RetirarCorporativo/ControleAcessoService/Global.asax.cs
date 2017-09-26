using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using Castle.Facilities.WcfIntegration;
using Castle.Windsor;
using ControleAcesso.Infra.IoC;
using log4net.Config;

namespace ControleAcessoService
{
	public class WcfApplication : HttpApplication
	{
	    private WindsorContainer _container;
		private void CriarArquivoLog()
		{
            String basePath = Server.MapPath("~/App_Data") + "\\";
            if (!File.Exists(basePath + "Log.db") && File.Exists(basePath + "Log.sql"))
            {
                //Cria o arquivo de log com base no arquivo SQL
                using (SQLiteConnection conn = new SQLiteConnection())
                {
                    try
                    {
                        SQLiteConnection.CreateFile(basePath + "Log.db");
                        conn.ConnectionString = "Data Source=" + basePath + "Log.db; Version=3; New=True;";

                        String sql = File.ReadAllText(basePath + "Log.sql");
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                    catch
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                            if (File.Exists(basePath + "Log.db"))
                                File.Delete(basePath + "Log.db");
                        }
                    }
                }
            }
                
			DOMConfigurator.Configure(); 
		}
		
		protected void Application_Start(object sender, EventArgs e)
		{
            _container = new WindsorContainer();
		    _container.AddFacility<WcfFacility>();

            new ConfigurarDependencias().Install(_container);

			RouteTable.Routes.Add(new ServiceRoute("Autenticacao", new WebServiceHostFactory(), typeof(AutenticacaoServico)));
		}

        protected void Application_End(object sender, EventArgs e)
	    {
	        if(_container != null)
                _container.Dispose();
	    }
		
		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			CriarArquivoLog();
			
			//var origens = string.Join(",", SistemaServico.Instancia.Buscar().Select(s => s.IpServidorAmbiente));
			/*HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
			if (HttpContext.Current.Request.HttpMethod == "OPTIONS" )
			{
				HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods" , "GET, POST" );
				HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers" , "Content-Type, Accept" );
				HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
				HttpContext.Current.Response.End();
			}*/
		}

	   
	}
}