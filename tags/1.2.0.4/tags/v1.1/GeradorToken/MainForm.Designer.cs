/*
 * Created by SharpDevelop.
 * User: dcbelmont
 * Date: 17/06/2014
 * Time: 10:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace GeradorToken
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.label1 = new System.Windows.Forms.Label();
			this.cmbSistema = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtServidor = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.btnToken = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.txtSql = new System.Windows.Forms.TextBox();
			this.txtToken = new System.Windows.Forms.TextBox();
			this.lblComplementoServidor = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(132, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Código do sistema";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cmbSistema
			// 
			this.cmbSistema.FormattingEnabled = true;
			this.cmbSistema.Location = new System.Drawing.Point(150, 8);
			this.cmbSistema.MaxLength = 20;
			this.cmbSistema.Name = "cmbSistema";
			this.cmbSistema.Size = new System.Drawing.Size(113, 21);
			this.cmbSistema.Sorted = true;
			this.cmbSistema.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(12, 36);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(132, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "Nome do servidor";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtServidor
			// 
			this.txtServidor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtServidor.Location = new System.Drawing.Point(150, 38);
			this.txtServidor.Name = "txtServidor";
			this.txtServidor.Size = new System.Drawing.Size(246, 20);
			this.txtServidor.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(12, 107);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(132, 21);
			this.label3.TabIndex = 4;
			this.label3.Text = "Token:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// btnToken
			// 
			this.btnToken.Location = new System.Drawing.Point(150, 65);
			this.btnToken.Name = "btnToken";
			this.btnToken.Size = new System.Drawing.Size(91, 33);
			this.btnToken.TabIndex = 5;
			this.btnToken.Text = "Gerar Token";
			this.btnToken.UseVisualStyleBackColor = true;
			this.btnToken.Click += new System.EventHandler(this.BtnTokenClick);
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(12, 128);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(132, 23);
			this.label5.TabIndex = 7;
			this.label5.Text = "SQL insert";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// txtSql
			// 
			this.txtSql.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSql.ForeColor = System.Drawing.Color.RoyalBlue;
			this.txtSql.Location = new System.Drawing.Point(150, 134);
			this.txtSql.Multiline = true;
			this.txtSql.Name = "txtSql";
			this.txtSql.ReadOnly = true;
			this.txtSql.Size = new System.Drawing.Size(745, 131);
			this.txtSql.TabIndex = 8;
			// 
			// txtToken
			// 
			this.txtToken.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtToken.Location = new System.Drawing.Point(150, 108);
			this.txtToken.Name = "txtToken";
			this.txtToken.ReadOnly = true;
			this.txtToken.Size = new System.Drawing.Size(745, 20);
			this.txtToken.TabIndex = 9;
			// 
			// lblComplementoServidor
			// 
			this.lblComplementoServidor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblComplementoServidor.Location = new System.Drawing.Point(402, 36);
			this.lblComplementoServidor.Name = "lblComplementoServidor";
			this.lblComplementoServidor.Size = new System.Drawing.Size(100, 23);
			this.lblComplementoServidor.TabIndex = 10;
			this.lblComplementoServidor.Text = ".inmetro.local";
			this.lblComplementoServidor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(907, 277);
			this.Controls.Add(this.lblComplementoServidor);
			this.Controls.Add(this.txtToken);
			this.Controls.Add(this.txtSql);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.btnToken);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtServidor);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cmbSistema);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Gerador Token - Controle de Acesso";
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Label lblComplementoServidor;
		private System.Windows.Forms.TextBox txtToken;
		private System.Windows.Forms.TextBox txtSql;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btnToken;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtServidor;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cmbSistema;
		private System.Windows.Forms.Label label1;
	}
}
