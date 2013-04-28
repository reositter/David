IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[LuthmanAB].[sp_GetStockBalanceForPimIntegration]') AND type IN (N'P', N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [LuthmanAB].[sp_GetStockBalanceForPimIntegration] AS SELECT ''dummy'''
	END
GO

ALTER PROCEDURE [LuthmanAB].[sp_GetStockBalanceForPimIntegration] (
	@TimeOfLastQuery DATETIME
)
AS
	-- TODO: Write a query that actually works.
	SELECT
		ART.ZUsrPimSku AS PimSku,
		SUM(SST.UnitInStock-SST.UnitOnOrder) AS StockBalance
	FROM
		LuthmanAB.StockSurveyTotalsView SST INNER JOIN LuthmanAB.Article ART ON ART.ArticleNo = SST.ArticleNo
	WHERE
			SST.WareHouseNo = 1 OR SST.WareHouseNo = 26
		AND
			SST.LastUpdate > @TimeOfLastQuery
		AND
			ART.ZUsrPimSku IS NOT NULL
	GROUP BY 
		SST.ArticleNo, 
		ART.ZUsrPimSku;
GO