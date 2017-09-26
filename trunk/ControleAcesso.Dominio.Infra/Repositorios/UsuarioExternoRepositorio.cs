using System;
using ControleAcesso.Dominio.Entidades;
using NHibernate;
using NHibernate.Event;


namespace ControleAcesso.Dominio.Infra.Repositorios
{
    public class UsuarioExternoRepositorio : Repositorio<UsuarioExterno>
    {


        public override void Salvar(UsuarioExterno objeto)
        {
            //var session = Conexao.ObterSessao();
            //session.SaveOrUpdate(objeto);
            //session.SaveOrUpdateCopy(objeto);
            //session.Flush();

            var session = Conexao.ObterSessao(true);


            try
            {
                session.Close();
                SalvarComTransacao(objeto);
                session.SaveOrUpdate(objeto);
                session.Flush();
            }
            catch (Exception ex)
            {
                try
                {

                    session.SaveOrUpdateCopy(objeto);
                    session.Flush();

                }
                catch (Exception e)
                {
                    try
                    {
                        session.Merge(objeto);
                        session.Flush();
                    }
                    catch
                    {
                        session.Evict(objeto);

                    }
                    throw e;
                }

                throw ex;
            }


        }

    }
}