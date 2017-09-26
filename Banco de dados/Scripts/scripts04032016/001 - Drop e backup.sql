use INMETRO
go

-- #### Remover objetos n�o utilizados ####

drop procedure [CONSUMO].[-- apagar 2016.01.21 PU_CADASTRO_AVALIADOR]
go

-- #### Backup das linhas das tabelas a serem alteradas ####

Select * into CONTROLEACESSO.BKP_TB_LOGIN from CONTROLEACESSO.TB_LOGIN
go
Select * into CONTROLEACESSO.BKP_TB_LOGIN_SISTEMA_PERFIL from CONTROLEACESSO.TB_LOGIN_SISTEMA_PERFIL
go

Select * into CONTROLEACESSO.BKP_TB_LOGIN_EXTERNO from CONTROLEACESSO.TB_LOGIN_EXTERNO
go
Select * into CONTROLEACESSO.BKP_TB_LOGIN_EXTERNO_SENHA from CONTROLEACESSO.TB_LOGIN_EXTERNO_SENHA
go
Select * into CONTROLEACESSO.BKP_TB_LOGIN_EXTERNO_SISTEMA_PERFIL from CONTROLEACESSO.TB_LOGIN_EXTERNO_SISTEMA_PERFIL
go

-- #### Drop das tabelas a serem alteradas ####

Drop table CONTROLEACESSO.TB_LOGIN_SISTEMA_PERFIL
go
Drop table CONTROLEACESSO.TB_LOGIN
go

Drop table CONTROLEACESSO.TB_LOGIN_EXTERNO_SENHA
go
Drop table CONTROLEACESSO.TB_LOGIN_EXTERNO_SISTEMA_PERFIL
go
Drop table CONTROLEACESSO.TB_LOGIN_EXTERNO
go