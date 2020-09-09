USE Teste
GO
CREATE PROCEDURE P_BUSCA_IMPOSTOS 
@pNotaFiscalId int 
AS
SELECT Cfop, SUM(BaseIcms), SUM(ValorIcms), SUM(BaseIPI), SUM(ValorIPI) FROM NotaFiscalItem WHERE IdNotaFiscal = @pNotaFiscalId GROUP BY CFOP
