$(document).ready(function () {
	function displayJsonResponse(json) {
		$('#response').html(jsontree(json));
	}

	$('#btnGetNewProductsByDateDummy').on('click', function(e) {
		e.preventDefault();
		$.ajax({
			type: 'GET', url: 'products/new',
			data: {},
			success: displayJsonResponse
		});
	});

	$('#btnGetArticlesForPriceUpdate').on('click', function(e) {
		e.preventDefault();
		$.ajax({
			type: 'GET', url: 'products/forpriceupdate',
			data: {},
			success: displayJsonResponse
		});
	});
	
	$('#btnGetArticlesForPriceUpdateAndCalcAgreedPrice').on('click', function (e) {
		e.preventDefault();
		$.ajax({
			type: 'GET', url: 'products/forpriceupdatewithagreedprice',
			data: {},
			success: displayJsonResponse
		});
	});
	
	$('#btnGetStockBalanceQuery').on('click', function (e) {
		e.preventDefault();
		$.ajax({
			type: 'GET', url: 'products/forstockbalanceupdate',
			data: {},
			success: displayJsonResponse
		});
	});
});