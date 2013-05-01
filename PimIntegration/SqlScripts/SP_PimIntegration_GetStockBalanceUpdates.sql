IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[LuthmanAB].[SP_PimIntegration_GetStockBalanceUpdates]') AND type IN (N'P', N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [LuthmanAB].[SP_PimIntegration_GetStockBalanceUpdates] AS SELECT ''dummy'''
	END
GO

ALTER PROCEDURE [LuthmanAB].[SP_PimIntegration_GetStockBalanceUpdates] (
	@Since DATETIME
)
AS
	SELECT     SSTV.ArticleNo, A.ZUsrPimSku, SUM(SSTV.UnitInStock - SSTV.UnitOnOrder) AS StockBalance
	FROM         _Luthman_.StockSurveyTotalsView AS SSTV INNER JOIN
						  _Luthman_.Article AS A ON SSTV.ArticleNo = A.ArticleNo
	WHERE     (SSTV.WareHouseNo = 1) OR
						  (SSTV.WareHouseNo = 26)
	GROUP BY SSTV.ArticleNo, SSTV.LastUpdate,A.ZUsrPimSku
	HAVING      SSTV.LastUpdate > @Since
GO
