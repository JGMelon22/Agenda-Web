CREATE TABLE [dbo].[EVENTO] (
    [ID]            UNIQUEIDENTIFIER NOT NULL,
    [NOME]          NVARCHAR (150)   NOT NULL,
    [DATA]          DATE             NOT NULL,
    [HORA]          TIME (7)         NOT NULL,
    [DESCRICAO]     NVARCHAR (500)   NOT NULL,
    [PRIORIDADE]    INT              NOT NULL,
    [DATAINCLUSAO]  DATETIME         NOT NULL,
    [DATAALTERACAO] DATETIME         NOT NULL,
    [ATIVO]         INT              DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

