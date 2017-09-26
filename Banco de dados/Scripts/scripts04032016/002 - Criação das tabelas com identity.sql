USE INMETRO
GO




SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE CONTROLEACESSO.TB_LOGIN_EXTERNO(
    IDT_LOGIN_EXTERNO int IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	CDA_LOGIN_EXTERNO varchar(100) NOT NULL,
	IDT_PESSOA_FISICA int NOT NULL,
	DES_EMAIL varchar(100) NOT NULL,
	BLN_CONTROLE_EXCLUIDO bit NOT NULL,
	CDA_CONTROLE_ORIGEM char(2) NOT NULL,
	DAT_CONTROLE_ALTERACAO datetime NOT NULL,
 CONSTRAINT PK_TB_LOGIN_EXTERNO PRIMARY KEY CLUSTERED 
(
	IDT_LOGIN_EXTERNO ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON CONTROLEACESSO
)
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificador único do login do usuário' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_EXTERNO', @level2type=N'COLUMN',@level2name=N'IDT_LOGIN_EXTERNO'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Login do usuário' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_EXTERNO', @level2type=N'COLUMN',@level2name=N'CDA_LOGIN_EXTERNO'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome do usuário. Será utilizada para o caso do usuário esquecer a senha e ser necessário resetar.' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_EXTERNO', @level2type=N'COLUMN',@level2name=N'IDT_PESSOA_FISICA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'E-mail da pessoa' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_EXTERNO', @level2type=N'COLUMN',@level2name=N'DES_EMAIL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica se a linha está ativa "A", desativada "D" ... (Deleção lógica)' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_EXTERNO', @level2type=N'COLUMN',@level2name=N'BLN_CONTROLE_EXCLUIDO'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica a origem do dado - Inmetro "I", SIAPE "S" ... ' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_EXTERNO', @level2type=N'COLUMN',@level2name=N'CDA_CONTROLE_ORIGEM'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data e hora da última alteração na linha, em qualquer uma das colunas' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_EXTERNO', @level2type=N'COLUMN',@level2name=N'DAT_CONTROLE_ALTERACAO'
GO




SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE CONTROLEACESSO.TB_LOGIN(
	IDT_LOGIN int IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	CDA_LOGIN char(20) NOT NULL,
	NOM_PESSOA_FISICA varchar(100) NOT NULL,
	DES_EMAIL varchar(100) NULL,
	IDT_PESSOA_FISICA int NOT NULL,
	BLN_CONTROLE_EXCLUIDO bit NOT NULL,
	CDA_CONTROLE_ORIGEM char(2) NOT NULL,
	DAT_CONTROLE_ALTERACAO datetime NOT NULL,
 CONSTRAINT PK_TB_LOGIN PRIMARY KEY CLUSTERED 
(
	IDT_LOGIN ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON CONTROLEACESSO
) ON CONTROLEACESSO
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificador único do login do usuário' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN', @level2type=N'COLUMN',@level2name=N'IDT_LOGIN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Login do usuário' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN', @level2type=N'COLUMN',@level2name=N'CDA_LOGIN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome do usuário. Vem da pessoa física.' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN', @level2type=N'COLUMN',@level2name=N'NOM_PESSOA_FISICA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'E-mail da pessoa' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN', @level2type=N'COLUMN',@level2name=N'DES_EMAIL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CPF da pessoa. Vem da TB_PESSOA_FISICA.' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN', @level2type=N'COLUMN',@level2name=N'IDT_PESSOA_FISICA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica se a linha está ativa "A", desativada "D" ... (Deleção lógica)' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN', @level2type=N'COLUMN',@level2name=N'BLN_CONTROLE_EXCLUIDO'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica a origem do dado - Inmetro "I", SIAPE "S" ... ' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN', @level2type=N'COLUMN',@level2name=N'CDA_CONTROLE_ORIGEM'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data e hora da última alteração na linha, em qualquer uma das colunas' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN', @level2type=N'COLUMN',@level2name=N'DAT_CONTROLE_ALTERACAO'
GO




SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE CONTROLEACESSO.TB_LOGIN_SISTEMA_PERFIL(
	IDT_LOGIN int NOT NULL,
	IDT_SISTEMA int NOT NULL,
	IDT_PERFIL int NOT NULL,
	BLN_CONTROLE_EXCLUIDO bit NOT NULL,
	CDA_CONTROLE_ORIGEM char(2) NOT NULL,
	DAT_CONTROLE_ALTERACAO datetime NOT NULL,
 CONSTRAINT PK_TB_LOGIN_SISTEMA_PERFIL PRIMARY KEY CLUSTERED 
(
	IDT_LOGIN ASC,
	IDT_SISTEMA ASC,
	IDT_PERFIL ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON CONTROLEACESSO
) ON CONTROLEACESSO
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificador único do login do usuário' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_SISTEMA_PERFIL', @level2type=N'COLUMN',@level2name=N'IDT_LOGIN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sistema acessado' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_SISTEMA_PERFIL', @level2type=N'COLUMN',@level2name=N'IDT_SISTEMA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Perfil de acesso' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_SISTEMA_PERFIL', @level2type=N'COLUMN',@level2name=N'IDT_PERFIL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica se a linha está ativa "A", desativada "D" ... (Deleção lógica)' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_SISTEMA_PERFIL', @level2type=N'COLUMN',@level2name=N'BLN_CONTROLE_EXCLUIDO'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica a origem do dado - Inmetro "I", SIAPE "S" ... ' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_SISTEMA_PERFIL', @level2type=N'COLUMN',@level2name=N'CDA_CONTROLE_ORIGEM'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data e hora da última alteração na linha, em qualquer uma das colunas' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_SISTEMA_PERFIL', @level2type=N'COLUMN',@level2name=N'DAT_CONTROLE_ALTERACAO'
GO




SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE CONTROLEACESSO.TB_LOGIN_EXTERNO_SISTEMA_PERFIL(
	IDT_LOGIN_EXTERNO int NOT NULL,
	IDT_SISTEMA int NOT NULL,
	IDT_PERFIL int NOT NULL,
	BLN_CONTROLE_EXCLUIDO bit NOT NULL,
	CDA_CONTROLE_ORIGEM char(2) NOT NULL,
	DAT_CONTROLE_ALTERACAO datetime NOT NULL,
 CONSTRAINT PK_TB_LOGIN_EXTERNO_SISTEMA_PERFIL PRIMARY KEY CLUSTERED 
(
	IDT_LOGIN_EXTERNO ASC,
	IDT_SISTEMA ASC,
	IDT_PERFIL ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON CONTROLEACESSO
) ON CONTROLEACESSO
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificador único do login do usuário' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_EXTERNO_SISTEMA_PERFIL', @level2type=N'COLUMN',@level2name=N'IDT_LOGIN_EXTERNO'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sistema acessado' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_EXTERNO_SISTEMA_PERFIL', @level2type=N'COLUMN',@level2name=N'IDT_SISTEMA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Perfil de acesso' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_EXTERNO_SISTEMA_PERFIL', @level2type=N'COLUMN',@level2name=N'IDT_PERFIL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica se a linha está ativa "A", desativada "D" ... (Deleção lógica)' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_EXTERNO_SISTEMA_PERFIL', @level2type=N'COLUMN',@level2name=N'BLN_CONTROLE_EXCLUIDO'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica a origem do dado - Inmetro "I", SIAPE "S" ... ' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_EXTERNO_SISTEMA_PERFIL', @level2type=N'COLUMN',@level2name=N'CDA_CONTROLE_ORIGEM'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data e hora da última alteração na linha, em qualquer uma das colunas' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_EXTERNO_SISTEMA_PERFIL', @level2type=N'COLUMN',@level2name=N'DAT_CONTROLE_ALTERACAO'
GO




SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE CONTROLEACESSO.TB_LOGIN_EXTERNO_SENHA(
	IDT_LOGIN_EXTERNO int NOT NULL,
	DES_SENHA varchar(100) NOT NULL,
	DAT_EXPIRACAO_SENHA datetime NOT NULL,
	CDA_TIPO_SENHA char(1) NOT NULL,
	BLN_CONTROLE_EXCLUIDO bit NOT NULL,
	CDA_CONTROLE_ORIGEM char(2) NOT NULL,
	DAT_CONTROLE_ALTERACAO datetime NOT NULL,
 CONSTRAINT PK_TB_LOGIN_EXTERNO_SENHA PRIMARY KEY CLUSTERED 
(
	IDT_LOGIN_EXTERNO ASC,
	CDA_TIPO_SENHA ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON CONTROLEACESSO
) ON CONTROLEACESSO
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificador único do login do usuário' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_EXTERNO_SENHA', @level2type=N'COLUMN',@level2name=N'IDT_LOGIN_EXTERNO'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Senha criptografada (Hash)' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_EXTERNO_SENHA', @level2type=N'COLUMN',@level2name=N'DES_SENHA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data de expiração da senha. Colocar hoje mais 100 anos para senhas permanentes.' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_EXTERNO_SENHA', @level2type=N'COLUMN',@level2name=N'DAT_EXPIRACAO_SENHA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipo da senha: Temporária "T" ou Permanente "P"' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_EXTERNO_SENHA', @level2type=N'COLUMN',@level2name=N'CDA_TIPO_SENHA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica se a linha está ativa "A", desativada "D" ... (Deleção lógica)' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_EXTERNO_SENHA', @level2type=N'COLUMN',@level2name=N'BLN_CONTROLE_EXCLUIDO'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica a origem do dado - Inmetro "I", SIAPE "S" ... ' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_EXTERNO_SENHA', @level2type=N'COLUMN',@level2name=N'CDA_CONTROLE_ORIGEM'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data e hora da última alteração na linha, em qualquer uma das colunas' , @level0type=N'SCHEMA',@level0name=N'CONTROLEACESSO', @level1type=N'TABLE',@level1name=N'TB_LOGIN_EXTERNO_SENHA', @level2type=N'COLUMN',@level2name=N'DAT_CONTROLE_ALTERACAO'
GO




/****** Object:  Default DF__TB_LOGIN__CDA_CO__7266E4EE    Script Date: 02/22/2016 11:55:01 ******/
ALTER TABLE CONTROLEACESSO.TB_LOGIN ADD  DEFAULT ('I') FOR CDA_CONTROLE_ORIGEM
GO
/****** Object:  Default DF__TB_LOGIN__DAT_CO__744F2D60    Script Date: 02/22/2016 11:55:01 ******/
ALTER TABLE CONTROLEACESSO.TB_LOGIN ADD  DEFAULT (getdate()) FOR DAT_CONTROLE_ALTERACAO
GO
/****** Object:  Default DF__TB_LOGIN___CDA_C__359DCDD0    Script Date: 02/22/2016 11:55:01 ******/
ALTER TABLE CONTROLEACESSO.TB_LOGIN_EXTERNO ADD  DEFAULT ('I') FOR CDA_CONTROLE_ORIGEM
GO
/****** Object:  Default DF__TB_LOGIN___DAT_C__37861642    Script Date: 02/22/2016 11:55:01 ******/
ALTER TABLE CONTROLEACESSO.TB_LOGIN_EXTERNO ADD  DEFAULT (getdate()) FOR DAT_CONTROLE_ALTERACAO
GO
/****** Object:  Default DF__TB_LOGIN___CDA_C__396E5EB4    Script Date: 02/22/2016 11:55:01 ******/
ALTER TABLE CONTROLEACESSO.TB_LOGIN_EXTERNO_SISTEMA_PERFIL ADD  DEFAULT ('I') FOR CDA_CONTROLE_ORIGEM
GO
/****** Object:  Default DF__TB_LOGIN___DAT_C__3B56A726    Script Date: 02/22/2016 11:55:01 ******/
ALTER TABLE CONTROLEACESSO.TB_LOGIN_EXTERNO_SISTEMA_PERFIL ADD  DEFAULT (getdate()) FOR DAT_CONTROLE_ALTERACAO
GO
/****** Object:  Default DF__TB_LOGIN___CDA_C__763775D2    Script Date: 02/22/2016 11:55:01 ******/
ALTER TABLE CONTROLEACESSO.TB_LOGIN_SISTEMA_PERFIL ADD  DEFAULT ('I') FOR CDA_CONTROLE_ORIGEM
GO
/****** Object:  Default DF__TB_LOGIN___DAT_C__781FBE44    Script Date: 02/22/2016 11:55:01 ******/
ALTER TABLE CONTROLEACESSO.TB_LOGIN_SISTEMA_PERFIL ADD  DEFAULT (getdate()) FOR DAT_CONTROLE_ALTERACAO
GO




/****** Object:  Check CK_TB_LOGIN_EXTERNO_SENHA_CDA_TIPO_SENHA    Script Date: 02/22/2016 11:55:01 ******/
ALTER TABLE CONTROLEACESSO.TB_LOGIN_EXTERNO_SENHA  WITH CHECK ADD  CONSTRAINT CK_TB_LOGIN_EXTERNO_SENHA_CDA_TIPO_SENHA CHECK  ((CDA_TIPO_SENHA='T' OR CDA_TIPO_SENHA='P'))
GO
ALTER TABLE CONTROLEACESSO.TB_LOGIN_EXTERNO_SENHA CHECK CONSTRAINT CK_TB_LOGIN_EXTERNO_SENHA_CDA_TIPO_SENHA
GO



/****** Object:  ForeignKey FK_TB_LOGIN_TB_CONTROLE_ORIGEM    Script Date: 02/22/2016 11:55:01 ******/
ALTER TABLE CONTROLEACESSO.TB_LOGIN  WITH CHECK ADD  CONSTRAINT FK_TB_LOGIN_TB_CONTROLE_ORIGEM FOREIGN KEY(CDA_CONTROLE_ORIGEM)
REFERENCES CONTROLEACESSO.TB_CONTROLE_ORIGEM (CDA_CONTROLE_ORIGEM)
GO
ALTER TABLE CONTROLEACESSO.TB_LOGIN CHECK CONSTRAINT FK_TB_LOGIN_TB_CONTROLE_ORIGEM
GO
/****** Object:  ForeignKey FK_TB_LOGIN_TB_PESSOA_FISICA    Script Date: 02/22/2016 11:55:01 ******/
ALTER TABLE CONTROLEACESSO.TB_LOGIN  WITH CHECK ADD  CONSTRAINT FK_TB_LOGIN_TB_PESSOA_FISICA FOREIGN KEY(IDT_PESSOA_FISICA)
REFERENCES CORPORATIVO.TB_PESSOA_FISICA (IDT_PESSOA_FISICA)
GO
ALTER TABLE CONTROLEACESSO.TB_LOGIN CHECK CONSTRAINT FK_TB_LOGIN_TB_PESSOA_FISICA
GO
/****** Object:  ForeignKey FK_TB_LOGIN_EXTERNO_TB_CONTROLE_ORIGEM    Script Date: 02/22/2016 11:55:01 ******/
ALTER TABLE CONTROLEACESSO.TB_LOGIN_EXTERNO  WITH CHECK ADD  CONSTRAINT FK_TB_LOGIN_EXTERNO_TB_CONTROLE_ORIGEM FOREIGN KEY(CDA_CONTROLE_ORIGEM)
REFERENCES CONTROLEACESSO.TB_CONTROLE_ORIGEM (CDA_CONTROLE_ORIGEM)
GO
ALTER TABLE CONTROLEACESSO.TB_LOGIN_EXTERNO CHECK CONSTRAINT FK_TB_LOGIN_EXTERNO_TB_CONTROLE_ORIGEM
GO
/****** Object:  ForeignKey FK_TB_LOGIN_EXTERNO_TB_PESSOA_FISICA    Script Date: 02/22/2016 11:55:01 ******/
ALTER TABLE CONTROLEACESSO.TB_LOGIN_EXTERNO  WITH CHECK ADD  CONSTRAINT FK_TB_LOGIN_EXTERNO_TB_PESSOA_FISICA FOREIGN KEY(IDT_PESSOA_FISICA)
REFERENCES CORPORATIVO.TB_PESSOA_FISICA (IDT_PESSOA_FISICA)
GO
ALTER TABLE CONTROLEACESSO.TB_LOGIN_EXTERNO CHECK CONSTRAINT FK_TB_LOGIN_EXTERNO_TB_PESSOA_FISICA
GO
/****** Object:  ForeignKey FK_TB_LOGIN_EXTERNO_SENHA_TB_CONTROLE_ORIGEM    Script Date: 02/22/2016 11:55:01 ******/
ALTER TABLE CONTROLEACESSO.TB_LOGIN_EXTERNO_SENHA  WITH CHECK ADD  CONSTRAINT FK_TB_LOGIN_EXTERNO_SENHA_TB_CONTROLE_ORIGEM FOREIGN KEY(CDA_CONTROLE_ORIGEM)
REFERENCES CONTROLEACESSO.TB_CONTROLE_ORIGEM (CDA_CONTROLE_ORIGEM)
GO
ALTER TABLE CONTROLEACESSO.TB_LOGIN_EXTERNO_SENHA CHECK CONSTRAINT FK_TB_LOGIN_EXTERNO_SENHA_TB_CONTROLE_ORIGEM
GO
/****** Object:  ForeignKey FK_TB_LOGIN_EXTERNO_SENHA_TB_LOGIN_EXTERNO    Script Date: 02/22/2016 11:55:01 ******/
ALTER TABLE CONTROLEACESSO.TB_LOGIN_EXTERNO_SENHA  WITH CHECK ADD  CONSTRAINT FK_TB_LOGIN_EXTERNO_SENHA_TB_LOGIN_EXTERNO FOREIGN KEY(IDT_LOGIN_EXTERNO)
REFERENCES CONTROLEACESSO.TB_LOGIN_EXTERNO (IDT_LOGIN_EXTERNO)
GO
ALTER TABLE CONTROLEACESSO.TB_LOGIN_EXTERNO_SENHA CHECK CONSTRAINT FK_TB_LOGIN_EXTERNO_SENHA_TB_LOGIN_EXTERNO
GO
/****** Object:  ForeignKey FK_TB_LOGIN_EXTERNO_SISTEMA_PERFIL_TB_CONTROLE_ORIGEM    Script Date: 02/22/2016 11:55:01 ******/
ALTER TABLE CONTROLEACESSO.TB_LOGIN_EXTERNO_SISTEMA_PERFIL  WITH CHECK ADD  CONSTRAINT FK_TB_LOGIN_EXTERNO_SISTEMA_PERFIL_TB_CONTROLE_ORIGEM FOREIGN KEY(CDA_CONTROLE_ORIGEM)
REFERENCES CONTROLEACESSO.TB_CONTROLE_ORIGEM (CDA_CONTROLE_ORIGEM)
GO
ALTER TABLE CONTROLEACESSO.TB_LOGIN_EXTERNO_SISTEMA_PERFIL CHECK CONSTRAINT FK_TB_LOGIN_EXTERNO_SISTEMA_PERFIL_TB_CONTROLE_ORIGEM
GO
/****** Object:  ForeignKey FK_TB_LOGIN_EXTERNO_SISTEMA_PERFIL_TB_LOGIN_EXTERNO    Script Date: 02/22/2016 11:55:01 ******/
ALTER TABLE CONTROLEACESSO.TB_LOGIN_EXTERNO_SISTEMA_PERFIL  WITH CHECK ADD  CONSTRAINT FK_TB_LOGIN_EXTERNO_SISTEMA_PERFIL_TB_LOGIN_EXTERNO FOREIGN KEY(IDT_LOGIN_EXTERNO)
REFERENCES CONTROLEACESSO.TB_LOGIN_EXTERNO (IDT_LOGIN_EXTERNO)
GO
ALTER TABLE CONTROLEACESSO.TB_LOGIN_EXTERNO_SISTEMA_PERFIL CHECK CONSTRAINT FK_TB_LOGIN_EXTERNO_SISTEMA_PERFIL_TB_LOGIN_EXTERNO
GO
/****** Object:  ForeignKey FK_TB_LOGIN_EXTERNO_SISTEMA_PERFIL_TB_SISTEMA_PERFIL    Script Date: 02/22/2016 11:55:01 ******/
ALTER TABLE CONTROLEACESSO.TB_LOGIN_EXTERNO_SISTEMA_PERFIL  WITH CHECK ADD  CONSTRAINT FK_TB_LOGIN_EXTERNO_SISTEMA_PERFIL_TB_SISTEMA_PERFIL FOREIGN KEY(IDT_SISTEMA, IDT_PERFIL)
REFERENCES CONTROLEACESSO.TB_SISTEMA_PERFIL (IDT_SISTEMA, IDT_PERFIL)
GO
ALTER TABLE CONTROLEACESSO.TB_LOGIN_EXTERNO_SISTEMA_PERFIL CHECK CONSTRAINT FK_TB_LOGIN_EXTERNO_SISTEMA_PERFIL_TB_SISTEMA_PERFIL
GO
/****** Object:  ForeignKey FK_TB_LOGIN_SISTEMA_PERFIL_TB_CONTROLE_ORIGEM    Script Date: 02/22/2016 11:55:01 ******/
ALTER TABLE CONTROLEACESSO.TB_LOGIN_SISTEMA_PERFIL  WITH CHECK ADD  CONSTRAINT FK_TB_LOGIN_SISTEMA_PERFIL_TB_CONTROLE_ORIGEM FOREIGN KEY(CDA_CONTROLE_ORIGEM)
REFERENCES CONTROLEACESSO.TB_CONTROLE_ORIGEM (CDA_CONTROLE_ORIGEM)
GO
ALTER TABLE CONTROLEACESSO.TB_LOGIN_SISTEMA_PERFIL CHECK CONSTRAINT FK_TB_LOGIN_SISTEMA_PERFIL_TB_CONTROLE_ORIGEM
GO
/****** Object:  ForeignKey FK_TB_LOGIN_SISTEMA_PERFIL_TB_LOGIN    Script Date: 02/22/2016 11:55:01 ******/
ALTER TABLE CONTROLEACESSO.TB_LOGIN_SISTEMA_PERFIL  WITH CHECK ADD  CONSTRAINT FK_TB_LOGIN_SISTEMA_PERFIL_TB_LOGIN FOREIGN KEY(IDT_LOGIN)
REFERENCES CONTROLEACESSO.TB_LOGIN (IDT_LOGIN)
GO
ALTER TABLE CONTROLEACESSO.TB_LOGIN_SISTEMA_PERFIL CHECK CONSTRAINT FK_TB_LOGIN_SISTEMA_PERFIL_TB_LOGIN
GO
/****** Object:  ForeignKey FK_TB_LOGIN_SISTEMA_PERFIL_TB_SISTEMA_PERFIL    Script Date: 02/22/2016 11:55:01 ******/
ALTER TABLE CONTROLEACESSO.TB_LOGIN_SISTEMA_PERFIL  WITH CHECK ADD  CONSTRAINT FK_TB_LOGIN_SISTEMA_PERFIL_TB_SISTEMA_PERFIL FOREIGN KEY(IDT_SISTEMA, IDT_PERFIL)
REFERENCES CONTROLEACESSO.TB_SISTEMA_PERFIL (IDT_SISTEMA, IDT_PERFIL)
GO
ALTER TABLE CONTROLEACESSO.TB_LOGIN_SISTEMA_PERFIL CHECK CONSTRAINT FK_TB_LOGIN_SISTEMA_PERFIL_TB_SISTEMA_PERFIL
GO





ALTER TABLE CONTROLEACESSO.TB_LOGIN ADD CONSTRAINT UK_TB_LOGIN_CDA_LOGIN UNIQUE (CDA_LOGIN) 	
GO

ALTER TABLE CONTROLEACESSO.TB_LOGIN_EXTERNO ADD CONSTRAINT UK_TB_LOGIN_EXTERNO_CDA_LOGIN_EXTERNO UNIQUE (CDA_LOGIN_EXTERNO) 	
GO




ALTER TABLE CONTROLEACESSO.TB_PERFIL DROP CONSTRAINT UK_TB_PERFIL_CDA_PERFIL
GO
alter table CONTROLEACESSO.TB_PERFIL alter column CDA_PERFIL varchar(20) not null
go
ALTER TABLE CONTROLEACESSO.TB_PERFIL ADD  CONSTRAINT UK_TB_PERFIL_CDA_PERFIL UNIQUE NONCLUSTERED 
(
	CDA_PERFIL ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON CONTROLEACESSO
GO









