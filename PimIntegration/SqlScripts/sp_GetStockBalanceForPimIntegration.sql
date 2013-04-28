IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[LuthmanAB].[sp_GetStockBalanceForPimIntegration]') AND type IN (N'P', N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [LuthmanAB].[sp_GetStockBalanceForPimIntegration] AS SELECT ''dummy'''
	END
GO

ALTER PROCEDURE [LuthmanAB].[sp_GetStockBalanceForPimIntegration] (
	@TimeOfLastQuery DATETIME
)
AS
	SELECT
		ArticleNo,
		SUM(UnitInStock-UnitOnOrder) AS StockBalance
	FROM
		LuthmanAB.StockSurveyTotalsView
	WHERE
			WareHouseNo = 1 OR WareHouseNo = 26
		AND
			LastUpdate > @TimeOfLastQuery
	GROUP BY 
		ArticleNo;
GO