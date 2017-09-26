USE [INMETRO]
GO

-- Remove PK

ALTER TABLE [CONTROLEACESSO].[TS_PARAMETRO] DROP CONSTRAINT [PK_TS_PARAMETRO]
GO


-- Atualiza c�digos antigos para os novos idts

update SP set
SP.NOM_PARAMETRO = S.IDT_SISTEMA
from CONTROLEACESSO.TS_PARAMETRO SP join
     CONTROLEACESSO.TB_SISTEMA S on 
                               S.CDA_SISTEMA = SP.NOM_PARAMETRO

-- altera o nome das colunas

EXEC sp_rename 'CONTROLEACESSO.TS_PARAMETRO.NOM_PARAMETRO', 'IDT_SISTEMA'
GO
EXEC sp_rename 'CONTROLEACESSO.TS_PARAMETRO.NOM_COMPLEMENTO_PARAMETRO', 'NOM_PARAMETRO'
GO

-- altera o tipo das colunas

alter table CONTROLEACESSO.TS_PARAMETRO alter column IDT_SISTEMA int not null 
GO

-- Recria PK

ALTER TABLE [CONTROLEACESSO].[TS_PARAMETRO] ADD  CONSTRAINT [PK_TS_PARAMETRO] PRIMARY KEY CLUSTERED 
(
      [IDT_SISTEMA] ASC,
      [NOM_PARAMETRO] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [CONTROLEACESSO]
GO