using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using ControleAcesso.Dominio.Entidades;

namespace GeradorToken
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
			var connString = System.Configuration.ConfigurationManager.ConnectionStrings["INMETRO"].ConnectionString;
			using (var conn = new SqlConnection(connString))
			{
				using (var cmd = new SqlCommand("select * from CONTROLEACESSO.TB_SISTEMA", conn))
				{
					conn.Open();
					var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
					while (reader.Read())
					{
						cmbSistema.Items.Add(reader["CDA_SISTEMA"]);
					}
					reader.Close();
				}
			}
		}
		
		void BtnTokenClick(object sender, EventArgs e)
		{
			if (cmbSistema.Text == null || string.IsNullOrWhiteSpace(cmbSistema.Text))
			{
				MessageBox.Show("Selecione o sistema para o qual será gerado o token.",
				                "Selecione um sistema",
				                MessageBoxButtons.OK,
				                MessageBoxIcon.Error,
				                MessageBoxDefaultButton.Button1);
				
				return;
			} else if (cmbSistema.Text.Length > 20) {
				MessageBox.Show("O código do sistema pode ter, no máximo, vinte caractéres.",
				                "Selecione um sistema",
				                MessageBoxButtons.OK,
				                MessageBoxIcon.Error,
				                MessageBoxDefaultButton.Button1);
				
				return;
			}
			
			if (string.IsNullOrWhiteSpace(txtServidor.Text))
			{
				MessageBox.Show("Informe o nome completo do servidor que disparará requisições ao Controle de Acesso.",
				                "Informe o nome do servidor",
				                MessageBoxButtons.OK,
				                MessageBoxIcon.Error,
				                MessageBoxDefaultButton.Button1);
				
				return;
			}
			
			var sistema = new Sistema{ Codigo = cmbSistema.Text, Excluido = false };
			sistema.AdicionarIpServidorOrigem(txtServidor.Text);
			txtToken.Text = sistema.ServidoresOrigem.FirstOrDefault().Token;
			
			var sql = "declare @i int \r\n" +
				"select @i = coalesce(max(replace(NOM_COMPLEMENTO_PARAMETRO, 'ServidorOrigem', '')), 0) + 1 \r\n" +
				"from CONTROLEACESSO.TS_PARAMETRO where NOM_PARAMETRO = '{0}' \r\n" +
				"insert into CONTROLEACESSO.TS_PARAMETRO \r\n" +
				"(NOM_PARAMETRO, NOM_COMPLEMENTO_PARAMETRO, CDA_VALOR_PARAMETRO_A, CDA_VALOR_PARAMETRO_B, \r\n" +
				"CDA_CONTROLE_ATIVO, CDA_CONTROLE_ORIGEM, CDA_CONTROLE_USO, DAT_CONTROLE_ALTERACAO) values \r\n" +
				"('{0}', 'ServidorOrigem' + cast(@i as varchar), '{1}{2}', '{3}', 'A', 'I', 'I', getdate())";
			txtSql.Text = string.Format(sql, cmbSistema.Text, txtServidor.Text, lblComplementoServidor.Text, txtToken.Text);
		}
	}
}