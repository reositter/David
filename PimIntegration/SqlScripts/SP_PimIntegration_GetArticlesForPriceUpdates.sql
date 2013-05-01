IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[LuthmanAB].[SP_PimIntegration_GetArticlesForPriceUpdate]') AND type IN (N'P', N'PC'))
	BEGIN
		EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [LuthmanAB].[SP_PimIntegration_GetArticlesForPriceUpdate] AS SELECT ''dummy'''
	END
GO

ALTER PROCEDURE [LuthmanAB].[SP_PimIntegration_GetArticlesForPriceUpdate] (
	@Since DATETIME
)
AS
	-- Letar efter förändringar på artikelkortet --

	SELECT CL.PrimaryKeys AS ArticleNo, A.ZUsrPimSku, '1' AS Market, MAX(CL.Created) AS LastUpdate
	FROM _Luthman_.ChangedLog AS CL INNER JOIN
		  _Luthman_.Article AS A ON CL.PrimaryKeys = A.ArticleNo
	WHERE (CL.LogID = 107) AND (CL.ObjectID = 2011)
	GROUP BY CL.PrimaryKeys, A.Name, A.ZUsrPimSku
	HAVING MAX(CL.Created) > @Since

	UNION

	-- Letar efter förändringar i bruttoprislistorna --

	SELECT A.ArticleNo, A.ZUsrPimSku, CASE WHEN DAC.CurrencyNo = 208 THEN 'DK' WHEN DAC.CurrencyNo = 578 THEN 'NO' END AS Market, MAX(DAC.LastUpdate) AS LastUpdate
	FROM _Luthman_.DiscountAgreementCustomer AS DAC INNER JOIN
		  _Luthman_.Article AS A ON DAC.ArticleNo = A.ArticleNo
	GROUP BY DAC.CurrencyNo, A.ArticleNo, DAC.DiscountType, A.ZUsrPimSku
	HAVING (MAX(DAC.LastUpdate) > @Since) AND (DAC.DiscountType = 10) AND (DAC.CurrencyNo = 208 OR DAC.CurrencyNo = 578)

	UNION

	-- Letar efter förändringar i rabattyp 3, Kundrabattgrupp / Artikelnummer --

	SELECT DAC.ArticleNo, A.ZUsrPimSku, CASE WHEN DAC.CurrencyNo = 0 THEN 'SE' WHEN DAC.CurrencyNo = 208 THEN 'DK' WHEN DAC.CurrencyNo = 578 THEN 'NO' END AS Market, DAC.LastUpdate
	FROM _Luthman_.DiscountAgreementCustomer AS DAC INNER JOIN
		  _Luthman_.Article AS A ON DAC.ArticleNo = A.ArticleNo
	WHERE (DAC.LastUpdate > @Since) AND (DAC.DiscountGrpCustNo IN (11, 32, 42)) AND 
		  (DAC.DiscountType = 3) AND (DAC.CurrencyNo = 208 OR DAC.CurrencyNo = 578 OR DAC.CurrencyNo = 0)

	UNION

	-- Letar efter förändringar i rabattyp 4, KundRabattGrupp / ArtikelRabattGrupp --

	SELECT A.ArticleNo, A.ZUsrPimSku, CASE WHEN DAC.DiscountGrpCustNo = 11 THEN 'SE' WHEN DAC.DiscountGrpCustNo = 32 THEN 'DK' WHEN DAC.DiscountGrpCustNo = 42 THEN 'NO' END AS Market, MAX(DAC.LastUpdate) AS LastUpdate
	FROM _Luthman_.DiscountAgreementCustomer AS DAC INNER JOIN
		  _Luthman_.Article AS A ON 
		  DAC.DiscountGrpArtNo = A.DiscountGrpArtNo
	WHERE (DAC.DiscountGrpCustNo IN (11, 32, 42)) AND (DAC.DiscountType = 4)
	GROUP BY DAC.DiscountGrpArtNo, A.ArticleNo, A.ZUsrPimSku, DAC.DiscountGrpCustNo
	HAVING (MAX(DAC.LastUpdate) > @Since)
GO